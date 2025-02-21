using System;
using MBT;
using UnityEngine;
using UnityEngine.AI;

namespace Bruno.Scripts.AI
{
    public class Mob : MonoBehaviour
    {
        private static readonly int Stun1 = Animator.StringToHash("isStunned?");
        protected Blackboard MBlackboard;
        protected MonoBehaviourTree MTree;
        protected NavMeshAgent MAgent;
        protected Animator MAnimator;
        protected float MStunTimer;
        
        [Header("Locomotion")]
        [SerializeField] [Range(0.1f, 10.8f)] private float radius = 10.5f;
        [SerializeField] [Range(0.1f, 5.0f)] private float spreadRadius = 2.0f;
        [SerializeField] [Range(0.05f, 0.9f)] private float attackAreaThreshold = 0.8f;
        
        public float speed { get; set; } = 1.0f;
        public GameObject player { get; private set; }
        public NavMeshAgent agent => MAgent;
        public bool gotHit { get; set; }
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            MBlackboard = GetComponent<Blackboard>();
            MTree = GetComponent<MonoBehaviourTree>();
            MAgent = GetComponent<NavMeshAgent>();
            MAnimator = GetComponent<Animator>();
            Debug.Log($"agent {MAgent}");
            
            PickRandomDestination();
        }

        // Update is called once per frame
        protected void Update()
        {
            // if (gotHit)
            // {
            //     MStunTimer -= Time.deltaTime;
            //     
            //     if (MStunTimer <= 0)
            //     {
            //         gotHit = false;
            //     }
            // }
            
        }

        protected void FixedUpdate()
        {
            if (!MTree) return;
            MTree.Tick();
        }

        public bool PlayerDetected()
        {
            var detectionRadius = (MAgent.height * 0.5f) * radius;
            var hitColliders = Physics.OverlapSphere(MAgent.transform.position, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    player = hitCollider.gameObject;
                    return true;
                }
            }

            player = null;
            return false;
        }
        
        /// <summary>
        /// Check whether the AI is looking at the direction of the player and if so, perform attack
        /// </summary>
        /// <param name="target"> The player </param>
        /// <returns></returns>
        public bool IsCloseToAttack(GameObject target)
        {
            if (!PlayerDetected()) return false;

            var directionToTarget = (target.transform.position - agent.transform.position).normalized;
            var dotProduct = Vector3.Dot(agent.transform.forward, directionToTarget);

            if (dotProduct <= attackAreaThreshold)
            {
                agent.transform.forward = Vector3.Lerp(agent.transform.forward, directionToTarget, Time.deltaTime * 6.0f); 
                return true;
            }

            return false;
        }
        
        
        protected void PickRandomDestination()
        {
            var randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * radius * spreadRadius;
            randomPoint.y = transform.position.y; 

            if (NavMesh.SamplePosition(randomPoint, out var hit, radius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        
        protected void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("stun")) return;
            gotHit = true;
            Debug.Log("casualties");
        }


        public void SetStunAnimation()
        {
            MAnimator.SetBool(Stun1, true);
        }

        public void DisableStunAnimation()
        {
            MAnimator.SetBool(Stun1, false);
        }

    }
}
