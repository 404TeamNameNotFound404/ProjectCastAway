using MBT;
using UnityEngine;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Retreat")]
    public class NativeRetreat : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable m_Self;
        private GameObjectVariable m_Target;
        private IntVariable m_Id;
        private NativeMob m_Mob;
        private BoolVariable m_Retreat;
        private float m_Timer = 0.0f;
        private const float TimeToRetreat = 8.0f;

        public override void OnEnter()
        {
            m_Self = blackboard.GetVariable<GameObjectVariable>("Self");
            m_Target = blackboard.GetVariable<GameObjectVariable>("Target");
            m_Id = blackboard.GetVariable<IntVariable>("id");
            m_Mob = m_Self.Value.GetComponent<NativeMob>();
            m_Retreat = blackboard.GetVariable<BoolVariable>("Retreat");
            m_Target.Value = GameObject.Find("RetreatPoint");
        }

        public override NodeResult Execute()
        {
            m_Id.Value = 3;

            m_Mob.agent.SetDestination(m_Target.Value.transform.position);
            
            m_Timer += Time.deltaTime * 2.0f;
            
            if (m_Timer >= TimeToRetreat)
            {
                m_Mob.agent.ResetPath();

                if (m_Mob.PlayerDetected())
                {
                    m_Id.Value = 1;
                    m_Timer = 0;
                    m_Target.Value = null;
                    return NodeResult.success;
                }

                m_Id.Value = 0;
                m_Target.Value = null;
                m_Timer = 0;
                return NodeResult.success;
            }
            
            return NodeResult.running;
        }
    }
}
