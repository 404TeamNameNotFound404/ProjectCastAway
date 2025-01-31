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
        [SerializeField] private float radius = 10.5f;
        [SerializeField] private float spreadRadius = 2.0f;
        [SerializeField] private float attackAreaThreshold = 0.8f;

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

        public bool IsCloseToAttack(GameObject target)
        {
            if (!PlayerDetected()) return false;

            var distance = Vector3.Distance(target.transform.position, agent.transform.position);
            
            if (distance <= attackAreaThreshold)
            {
                return true;
            }

            return false;
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
