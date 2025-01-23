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
        [SerializeField] private float radius = 10.5f;

        public float speed { get; set; } = 1.0f;
        public GameObject player { get; private set; }
        public NavMeshAgent agent => m_Agent;
        
        private void Start()
        {
            m_Blackboard = GetComponent<Blackboard>();
            m_Tree = GetComponent<MonoBehaviourTree>();
            m_Agent = GetComponent<NavMeshAgent>();
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
        
    }
}
