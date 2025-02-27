using System;
using UnityEngine;

namespace Bruno.Scripts
{
    public class LightHouse : MonoBehaviour
    {
        private EndGameWave _mWave;
        
        public bool beginWave { get; private set; }

        private void Start()
        {
            _mWave = GetComponent<EndGameWave>();
        }

        void Update()
        {
            if (!FakeInventory.WinConditionReached) return;

            if (beginWave)
            {
                StartWave();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !FakeInventory.WinConditionReached) return;
            beginWave = true;
        }


        public void StartWave()
        {
            _mWave.StartGame();
        }
    }
}
