using UnityEngine;
using UnityEngine.AI;

namespace DrawRoad
{
    public class Hunter : Movable
    {
        [SerializeField] private float _speed;

        private NavMeshAgent _navAgent;


        private const float updateBehaviourTime = 0f;
        private const float prediction = 0.2f;
        

        private float _timeToUpdateBehaviour;

        //private Vector3 _startPosition;

        private void Awake()
        {
            //_startPosition = transform.position;
            _navAgent = GetComponent<NavMeshAgent>();
            _navAgent.updateUpAxis = false;
            _navAgent.updateRotation = false;
            _navAgent.speed = _speed;
            Runner.onAllRunnersReady += OnAllMoversReady;
            Level.onLevelStarted += OnLevelStarted;
            Runner.onRunnerCollided += OnMoverCollided;
            Level.onFinished += OnLevelFinished;
        }

        private void OnLevelFinished(Level.FinishType finishType)
        {
            Runner.onAllRunnersReady -= OnAllMoversReady;
            Level.onLevelStarted -= OnLevelStarted;
            Runner.onRunnerCollided -= OnMoverCollided;
            Level.onFinished -= OnLevelFinished;
            Move = false;
            _navAgent.isStopped = true;
            _navAgent.velocity = Vector3.zero;
        }



        private void OnLevelStarted()
        {
            Move = false;
            //transform.position = _startPosition;
        }


        private void OnAllMoversReady()
        {
            _timeToUpdateBehaviour = 0f;
            Move = true;
        }

        private void OnMoverCollided(Vector2 collidePosition, Runner.CollisionType collideType)
        {
            Move = false;
            _navAgent.isStopped = true;
            _navAgent.velocity = Vector3.zero;
        }


        private void Update()
        {
            if (!Move) return;


            _navAgent.isStopped = false;
            if (_navAgent.velocity.x >= 0f) moveSide = Side.Right;
            else moveSide = Side.Left;

            _timeToUpdateBehaviour -= Time.deltaTime;
            if (_timeToUpdateBehaviour < 0f)
            {
                _timeToUpdateBehaviour = updateBehaviourTime;
                UpdateBehaviour();
            }


        }


        private void UpdateBehaviour()
        {
            var nearestMover = FindNearestMover();
            _navAgent.SetDestination((Vector2)nearestMover.transform.position + nearestMover.moveDirection * prediction);
        }

        private Runner FindNearestMover()
        {
            float minDistance = 10000f;
            Runner nearestMover = null;

            foreach (var mover in Runner.moverStates.Keys)
            {
                if (mover.Move == false) continue;

                float distanceToMover = Vector2.Distance(mover.transform.position, transform.position);
                if (distanceToMover < minDistance )
                {
                    minDistance = distanceToMover;
                    nearestMover = mover;
                }
            }
                
            return nearestMover;
        }


    }

}




