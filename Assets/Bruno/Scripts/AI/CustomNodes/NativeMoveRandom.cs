using MBT;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/MoveRandom")]
    public class NativeMoveRandom : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable m_Self;
        private GameObjectVariable m_Target;
        private BoolVariable m_PlayerSeen;
        private BoolVariable m_IsDayTime;
        private IntVariable m_Id;
        private NativeMob m_Mob;
        
        public override void OnEnter()
        {
            m_Self = blackboard.GetVariable<GameObjectVariable>("Self");
            m_Target = blackboard.GetVariable<GameObjectVariable>("Target");
            m_PlayerSeen = blackboard.GetVariable<BoolVariable>("PlayerSeen");
            m_IsDayTime = blackboard.GetVariable<BoolVariable>("IsDayTime");
            m_Id = blackboard.GetVariable<IntVariable>("id");
            m_Mob = m_Self.Value.GetComponent<NativeMob>();
        }

        public override NodeResult Execute()
        {
            if (!DayNightCycle.isDayTime)
            {
                m_Id.Value = 2;
                return NodeResult.success;
            }

            m_Target.Value = null;
            m_Id.Value = 0;
            m_Mob.agent.speed = Random.Range(0.4f, m_Mob.Speed);

            if (m_Mob.agent.remainingDistance <= m_Mob.agent.stoppingDistance)
            {
                Vector3 point;
                if (GetRandomPosition(m_Mob.transform.position, 10.0f, out point))
                {
                    m_Mob.agent.SetDestination(point);
                }
            }
          
            return NodeResult.running;
        }


        private bool GetRandomPosition(Vector3 center ,float range, out Vector3 direction)
        {
            var randomDirection = UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 1.0f, NavMesh.AllAreas))
            {
                direction = hit.position;
                return true;
            }
            
            direction = Vector3.zero;
            return false;
        }
    }
}
