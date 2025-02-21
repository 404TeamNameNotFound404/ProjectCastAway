using MBT;
using UnityEngine;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Stun")]
    public class NativeStunBT : Leaf
    {
        public Blackboard blackboard;
        private GameObjectVariable _mSelf;
        private GameObjectVariable _mTarget;
        private IntVariable _mId;
        private NativeMob _mMob;

        private float _mTimer = 6.0f;

        public override void OnEnter()
        {
            _mSelf = blackboard.GetVariable<GameObjectVariable>("Self");
            _mTarget = blackboard.GetVariable<GameObjectVariable>("Target");
            _mId = blackboard.GetVariable<IntVariable>("id");
            _mMob = _mSelf.Value.GetComponent<NativeMob>();
            Debug.Log("stunned");
        }

        public override NodeResult Execute()
        {
            _mMob.agent.ResetPath();

            _mId.Value = 4;
            _mMob.SetStunAnimation();

            _mTimer -= Time.deltaTime * 2.0f;

            if (_mTimer <= 0.0f)
            {
                Debug.Log("timer ended ");
               _mMob.DisableStunAnimation();

                if (_mMob.PlayerDetected())
                {
                    _mId.Value = 1;
                    _mMob.SetWalkAnimation();
                    _mMob.gotHit = false;
                    return NodeResult.success;
                }

                _mId.Value = 0;
                _mMob.SetIdleAnimation();
                _mMob.gotHit = false;
                return NodeResult.success;

            }

            return NodeResult.running;
        }

    }
}

