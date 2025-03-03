using System.Collections.Generic;
using Bruno.Scripts.AI;
using UnityEngine;
using TMPro;

namespace Bruno.Scripts
{
    public class EndGameWave : MonoBehaviour
    {
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private List<GuardianMob> guardians;
        
        private float _mTimeLeft = 120.0f;
        private bool _mStop;
        private bool _mGuardianSpawned;
        void Start()
        {
            waveText.gameObject.SetActive(false);
            waveText.enableAutoSizing = false;
        }
        

        public void StartGame()
        {
            if (_mStop  || !FakeInventory.winConditionReached) return;
            waveText.gameObject.SetActive(true);
            _mTimeLeft -= Time.deltaTime * 1.5f;
            waveText.text = "Rescues will arrive in " + _mTimeLeft + " seconds.";
            
            if (!_mGuardianSpawned)
            {
                SpawnGuardian();
            }

            if (_mTimeLeft <= 0.0f)
            {
                _mTimeLeft = 0.0f; 
                
                waveText.text = "Rescues will arrive in " + _mTimeLeft + " seconds."; 
                DespawnGuardian();
                _mStop = true;
            }
        }

        private void SpawnGuardian()
        {
            foreach (var g in guardians)
            {
                g.gameObject.SetActive(true);
            }

            _mGuardianSpawned = true;
        }

        private void DespawnGuardian()
        {
            foreach (var g in guardians)
            {
                g.gameObject.SetActive(false);
            }
        }
    }
}
