using System;
using UnityEngine;

namespace Bruno.Scripts
{
    public class LightHouse : MonoBehaviour
    {
        private EndGameWave _mWave;
        [SerializeField] private RescueShip ship;
        
        public bool beginWave { get; private set; }

        private void Start()
        {
            _mWave = GetComponent<EndGameWave>();
            Debug.Log("ship? " + ship.name);
        }

        void Update()
        {
            if (!FakeInventory.winConditionReached) return;

            if (beginWave)
            {
                ship.gameObject.SetActive(true);
                StartWave();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !FakeInventory.winConditionReached) return;
            beginWave = true;
        }


        public void StartWave()
        {
            _mWave.StartGame();
        }
    }
}
