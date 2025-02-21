using System;
using UnityEngine;

namespace Bruno.Scripts
{
    public class LightHouse : MonoBehaviour
    {
        private bool _mEnd = false;
        private bool _mSendSignal = false;
        void Update()
        {
            if (!FakeInventory.WinConditionReached) return;
            _mEnd = true;
            Debug.Log("check condition met");

            if (_mSendSignal)
            {
                //TODO ADD RESCUE SHIP
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !_mEnd) return;
            _mSendSignal = true;
        }
    }
}
