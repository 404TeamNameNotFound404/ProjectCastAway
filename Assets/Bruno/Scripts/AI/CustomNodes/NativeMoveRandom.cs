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
        public float range = 10.0f;
        public MobSpawner m_Spawner;
        private GameObjectVariable m_Self;
        private GameObjectVariable m_Target;
        private BoolVariable m_PlayerSeen;
        private IntVariable m_Id;
        private NativeMob m_Mob;
        
        public override void OnEnter()
        {
            m_Self = blackboard.GetVariable<GameObjectVariable>("Self");
            m_Target = blackboard.GetVariable<GameObjectVariable>("Target");
            m_PlayerSeen = blackboard.GetVariable<BoolVariable>("PlayerSeen");
            m_Id = blackboard.GetVariable<IntVariable>("id");
            m_Mob = m_Self.Value.GetComponent<NativeMob>();
            Debug.Log("move random started");
        }

        public override NodeResult Execute()
        {
            if (!DayNightCycle.isDayTime)
            {
                m_Id.Value = 2;
                m_Mob.agent.ResetPath();
                return NodeResult.success;
            }

            if (m_Mob.PlayerDetected())
            {
                m_Id.Value = 1;
                m_Mob.agent.ResetPath();
                return NodeResult.success;
            }
            
            if (m_Mob.gotHit)
            {
                m_Id.Value = 3;
                m_Mob.gotHit = false;
                return NodeResult.success;
            }

            m_Target.Value = null;
            m_Id.Value = 0;
            m_Mob.agent.speed = Random.Range(0.4f, m_Mob.speed);
            m_Mob.agent.stoppingDistance = 0.2f;
            m_Mob.SetWalkAnimation();

            var success = GetRandomPosition(out var direction);

            if (m_Mob.agent.remainingDistance <= m_Mob.agent.stoppingDistance)
            {
                if (success)
                {
                    Debug.Log($"move to destination {direction}");
                    m_Mob.agent.SetDestination(direction);
                }
            }
          
            return NodeResult.running;
        }


        private bool GetRandomPosition(out Vector3 direction)
        {
            var randomDirection = m_Mob.transform.position + Random.insideUnitSphere * range; //Random.insideUnitSphere * range;
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
