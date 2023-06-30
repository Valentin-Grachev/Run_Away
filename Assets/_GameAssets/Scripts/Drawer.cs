using System;
using UnityEngine;


namespace DrawRoad
{
    public class Drawer : MonoBehaviour
    {
        public static event Action onBeginDrawing;
        public static event Action onEndDrawing;

        private bool _enableDrawing = true;

        [SerializeField] private Material _material;
        [SerializeField] private float _width;
        [SerializeField] private Vector2 _drawArea;

        private LineRenderer _line;

        private Runner[] _movers;
        private Goal[] _goals;

        private bool _drawing = false;
        private Runner _handlableMover;

        private void Awake()
        {
            Level.onLevelStarted += OnLevelStarted;
            Runner.onRunnerCollided += OnMoverCollided;
        }

        private void OnMoverCollided(Vector2 collidePosition, Runner.CollisionType collideType)
            => _enableDrawing = false;

        private void OnDestroy()
        {
            Level.onLevelStarted -= OnLevelStarted;
            Runner.onRunnerCollided -= OnMoverCollided;
        }

        private void OnLevelStarted()
        {
            _movers = FindObjectsOfType<Runner>();
            _goals = FindObjectsOfType<Goal>();
            _enableDrawing = true;
        }


        void Update()
        {
            if (!_enableDrawing) return;
            if (Input.GetMouseButtonDown(0)) BeginDrawing();
            if (Input.GetMouseButton(0) && _drawing) Drawing();
            if (Input.GetMouseButtonUp(0) && _drawing) EndDrawing();
        }

        private void BeginDrawing()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (IsNearMover(touchPosition, out _handlableMover))
            {
                _line = new GameObject().AddComponent<LineRenderer>();
                _line.positionCount = 0;
                _line.widthCurve = AnimationCurve.Constant(0f, 1f, _width);
                _line.startColor = _handlableMover.roadColor;
                _line.endColor = _handlableMover.roadColor;
                _line.material = _material;
                _line.sortingLayerName = "Middle";
                _line.sortingOrder = 0;
                _drawing = true;
                _handlableMover.ClearPath();
                onBeginDrawing?.Invoke();
            }
        }

        private void Drawing()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!PositionIsInsideArea(touchPosition, _drawArea))
            {
                _drawing = false;
                Destroy(_line.gameObject);
                onEndDrawing?.Invoke();
                return;
            }

            int newPositionIndex = _line.positionCount;
            _line.positionCount = _line.positionCount + 1;
            _line.SetPosition(newPositionIndex, touchPosition);

            if (IsNearGoal(touchPosition, out Goal goal)) EndDrawing();
        }


        private void EndDrawing()
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _drawing = false;

            if (IsNearGoal(touchPosition, out Goal goal))
            {
                if (goal.acceptedMover != null && goal.acceptedMover != _handlableMover)
                    goal.acceptedMover.ClearPath();

                goal.acceptedMover = _handlableMover;
                _handlableMover.targetGoal = goal;
                _handlableMover.SetPath(_line);
            }
            
            else Destroy(_line.gameObject);

            onEndDrawing?.Invoke();
        }

        private bool IsNearMover(Vector2 position, out Runner nearMover)
        {         
            foreach (var mover in _movers)
                if (mover != null && Vector2.Distance(position, mover.transform.position) < Runner.touchRadius)
                {
                    nearMover = mover;
                    return true;
                }

            nearMover = null;
            return false;
        }

        private bool IsNearGoal(Vector2 position, out Goal nearGoal)
        {
            foreach (var goal in _goals)
                if (goal != null && Vector2.Distance(position, goal.transform.position) < Goal.touchRadius)
                    foreach (var acceptableId in goal.acceptableIds)
                        if (acceptableId == _handlableMover.id)
                        {
                            nearGoal = goal;
                            return true;
                        }

            nearGoal = null;
            return false;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector2.zero, _drawArea);
        }


        private bool PositionIsInsideArea(Vector2 position, Vector2 area)
            => Mathf.Abs(position.x) < area.x / 2 && Mathf.Abs(position.y) < area.y / 2;



    }
}



