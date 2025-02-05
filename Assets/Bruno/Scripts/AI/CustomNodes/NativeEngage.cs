using UnityEngine;
using MBT;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Engage")]
    public class NativeEngage : Leaf
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
        }

        public override NodeResult Execute()
        {
            if (m_Mob.PlayerDetected())
            {
                m_Id.Value = 1;
                m_Mob.DisableAttackAnimation();
                m_Mob.SetWalkAnimation();
                return NodeResult.success;
            }

            if (!m_Mob.PlayerDetected())
            {
                m_Id.Value = 0;
                m_Mob.DisableAttackAnimation();
                m_Mob.SetWalkAnimation();
                return NodeResult.success;
            }

            if (!DayNightCycle.isDayTime)
            {
                m_Id.Value = 2;
                m_Mob.DisableAttackAnimation();
                m_Mob.SetWalkAnimation();
                return NodeResult.success;
            }

            if (m_Mob.PlayerDetected())
            {
                m_Id.Value = 4;
                return NodeResult.success;
            }
            
            m_Id.Value = 4;
           
            m_Mob.agent.ResetPath();
            m_Mob.SetAttackAnimation();
            
            return NodeResult.running;
        }
    }
}
