using UnityEngine;
using MBT;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Chase")]
    public class NativeChase : Leaf
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
            if (!m_Mob.PlayerDetected())
            {
                m_Id.Value = 0;
                m_Mob.agent.ResetPath();
                Debug.Log("not detected anymore");
                return NodeResult.success;
            }

            if (!DayNightCycle.isDayTime)
            {
                m_Id.Value = 2;
                m_Mob.gotHit = false;
                return NodeResult.success;
            }

            if (m_Mob.gotHit)
            {
                m_Id.Value = 3;
                m_Mob.gotHit = false;
                return NodeResult.success;
            }

            m_Target.Value = m_Mob.player;
            m_Id.Value = 1;
            m_Mob.agent.SetDestination(m_Target.Value.transform.position);
            return NodeResult.running;
        }
    }
}
