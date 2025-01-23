using UnityEngine;
using MBT;
using Unity.AI.Navigation;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Despawn")]
    public class NativeDespawn : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable m_Self;
        private GameObjectVariable m_Target;
        private IntVariable m_Id;
        private NativeMob m_Mob;
        
        public override void OnEnter()
        {
            m_Self = blackboard.GetVariable<GameObjectVariable>("Self");
            m_Target = blackboard.GetVariable<GameObjectVariable>("Target");
            m_Id = blackboard.GetVariable<IntVariable>("id");
            m_Mob = m_Self.Value.GetComponent<NativeMob>();

            m_Target.Value = GameObject.Find("RetreatPoint");
        }

        public override NodeResult Execute()
        {
            if (DayNightCycle.isDayTime)
            {
                m_Id.Value = 0;
                m_Mob.agent.ResetPath();
                return NodeResult.success;
            }
            
            m_Id.Value = 2;
            m_Mob.agent.SetDestination(m_Target.Value.transform.position);
            
            
            return NodeResult.running;
        }
    }
}
