using System;
using UnityEngine;
using MBT;
using UnityEngine.AI;

namespace Bruno.Scripts.AI
{
    public class NativeMob : MonoBehaviour
    {
        private Blackboard m_Blackboard;
        private MonoBehaviourTree m_Tree;
        private NavMeshAgent m_Agent;
        private Animator m_Animator;
        [Header("Locomotion")]
        [SerializeField] private float radius = 10.5f;
        [SerializeField] private float spreadRadius = 2.0f;
        [SerializeField] private float attackAreaThreshold = 0.8f;
        [Header("Engage")]
        [SerializeField] private GameObject arrowSpawner;
        [SerializeField] private GameObject arrowPrefab;
        
        public float speed { get; set; } = 1.0f;
        public GameObject player { get; private set; }
        public NavMeshAgent agent => m_Agent;
        public bool gotHit { get; set; }
        
        private void Start()
        {
            m_Blackboard = GetComponent<Blackboard>();
            m_Tree = GetComponent<MonoBehaviourTree>();
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
            Debug.Log($"agent {m_Agent}");
            
            PickRandomDestination();
        }
    
        private void Update()
        {
            if (!m_Tree) return;
            m_Tree.Tick();
        }

        public bool PlayerDetected()
        {
            var detectionRadius = (m_Agent.height * 0.5f) * radius;
            var hitColliders = Physics.OverlapSphere(m_Agent.transform.position, detectionRadius);

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

        /// <summary>
        /// Shot arrow - Couldn't adapt the throw function due to camera forward calculation within player.Throw()
        /// </summary>
        public void Pop()
        {
            if (!arrowSpawner || !arrowPrefab)
                return;
            Instantiate(arrowPrefab, arrowSpawner.transform.position, arrowSpawner.transform.rotation);
        }

        
        private void PickRandomDestination()
        {
            var randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * radius * spreadRadius;
            randomPoint.y = transform.position.y; 

            if (NavMesh.SamplePosition(randomPoint, out var hit, radius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        public void SetIdleAnimation()
        {
            if (!m_Animator) return;
            m_Animator.SetFloat("velocity", 0.0f);
        }

        public void SetWalkAnimation()
        {
            if(!m_Animator) return;
            m_Animator.SetFloat("velocity", 1.0f);
        }

        public void SetAttackAnimation()
        {
            if(!m_Animator) return;
            m_Animator.SetBool("attack", true);
        }

        public void DisableAttackAnimation()
        {
            if(!m_Animator) return;
            m_Animator.SetBool("attack", false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("stun")) return;
            gotHit = true;
            Debug.Log("casualties");
        }
    }
}
