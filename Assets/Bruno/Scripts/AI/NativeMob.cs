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

        public float Speed { get; set; } = 1.0f;
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
    }
}
