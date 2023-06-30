using System;
using System.Collections.Generic;
using UnityEngine;



namespace DrawRoad
{
    public abstract class Movable : MonoBehaviour
    {
        public enum Side { Left, Right }


        public bool Move { get; protected set; }
        public Side moveSide { get; protected set; }
    }


    public class Runner : Movable
    {
        public enum State { NotReady, Ready, GoalReached }
        public enum CollisionType { Hunter, Obstacle }
        

        public static Dictionary<Runner, State> moverStates = new Dictionary<Runner, State>();
        public static event Action onAllRunnersReady;
        public static event Action<Vector2, CollisionType> onRunnerCollided;

        private static bool alreadyCollided = false;
        [SerializeField] private int _id; public int id { get => _id; }
        [SerializeField] private float _speed;
        [SerializeField] private Color _roadColor; public Color roadColor { get => _roadColor; }


        public const float touchRadius = 0.7f;
        public Vector2 moveDirection { get; private set; }

        private CircleCollider2D _circleCollider;
        


        private const float translationTime = 1.5f;


        private LineRenderer _line;
        private Vector3[] _movePath;
        private int _currentPathIndex;

        private const float eps = 0.2f;

        public Goal targetGoal;

        private bool IsNear(Vector2 position) 
            => Vector2.Distance(transform.position, position) < eps;


        private void Awake()
        {
            _circleCollider = GetComponent<CircleCollider2D>();

            onAllRunnersReady += OnAllMoversReady;
            Level.onLevelStarted += OnLevelStarted;
            Level.onFinished += OnLevelFinished;
        }


        private void OnLevelFinished(Level.FinishType finishType)
        {
            onAllRunnersReady -= OnAllMoversReady;
            Level.onLevelStarted -= OnLevelStarted;
            Level.onFinished -= OnLevelFinished;

            ClearPath();
            moverStates.Clear();
            Move = false;
        }

        private void OnLevelStarted()
        {
            alreadyCollided = false;
            moverStates.Add(this, State.NotReady);
            Move = false;

        }

        private void OnAllMoversReady()
        {
            Move = true;
            Sounds.Play(Sounds.Key.run);
        }


        private void Update()
        {
            if (Move) Moving();
        }


        public void SetPath(LineRenderer line)
        {
            Vector3[] path = new Vector3[line.positionCount];
            line.GetPositions(path);

            _line = line;
            _movePath = SimplePath(path);
            _currentPathIndex = 0;

            moverStates[this] = State.Ready;

            bool allMoversReady = true;
            foreach (var state in moverStates.Values)
                if (state != State.Ready) allMoversReady = false;

            if (allMoversReady)
                onAllRunnersReady?.Invoke();

        }

        public void ClearPath()
        {
            moverStates[this] = State.NotReady;
            if (_line != null) Destroy(_line.gameObject);
        }

        private void Moving()
        {
            for (int i = _currentPathIndex; i < _movePath.Length; i++)
            {
                if (IsNear(_movePath[i])) _currentPathIndex++;
                else break;
            }

            if (_currentPathIndex == _movePath.Length)
                GoalReached();

            else
            {
                moveDirection = (_movePath[_currentPathIndex] - transform.position).normalized;
                moveSide = moveDirection.x >= 0 ? Side.Right : Side.Left;
                transform.Translate(moveDirection * _speed * Time.deltaTime);
            }
        }

        private void GoalReached()
        {
            Move = false;
            _circleCollider.enabled = false;
            transform.position = targetGoal.transform.position;
            moverStates[this] = State.GoalReached;

            bool allGoalsReached = true;
            foreach (var state in moverStates.Values)
                if (state != State.GoalReached) allGoalsReached = false;

            if (allGoalsReached)
            {
                Level.Finish(Level.FinishType.Win);
                Sounds.Stop(Sounds.Key.run);
            }
        }

        private Vector3[] SimplePath(Vector3[] movePath)
        {
            int saveEveryVertex;
            if (movePath.Length < 50) return movePath;
            else if (movePath.Length < 100) saveEveryVertex = 2;
            else if (movePath.Length < 200) saveEveryVertex = 4;
            else if (movePath.Length < 400) saveEveryVertex = 8;
            else if (movePath.Length < 800) saveEveryVertex = 16;
            else if (movePath.Length < 1600) saveEveryVertex = 32;
            else saveEveryVertex = 48;

            List<Vector3> path = new List<Vector3>(capacity: movePath.Length);
            for (int i = 0; i < movePath.Length; i++)
                if (i % saveEveryVertex == 0) path.Add(movePath[i]);

            path.Add(targetGoal.transform.position);
            return path.ToArray();
        }




        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, touchRadius);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!alreadyCollided && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Hunter"))
            {
                var collisionType = CollisionType.Obstacle;
                if (collision.gameObject.layer == LayerMask.NameToLayer("Hunter")) 
                    collisionType = CollisionType.Hunter;

                alreadyCollided = true;
                onRunnerCollided?.Invoke(collision.GetContact(0).point, collisionType);

                foreach (var mover in moverStates.Keys)
                    mover.Move = false;

                Sounds.Stop(Sounds.Key.run);
                Sounds.Play(Sounds.Key.dead);
            }
        }


    }
}


