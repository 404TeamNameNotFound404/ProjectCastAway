using UnityEngine;
using MBT;

namespace Bruno.Scripts.AI.CustomNodes
{
    [AddComponentMenu("")]
    [MBTNode("NativeMob/Chase")]
    public class NativeChase : Leaf
    {
        public override NodeResult Execute()
        {
            
            return NodeResult.running;
        }
    }
}
