using System;
using UnityEngine;
using TMPro;
namespace Bruno.Scripts
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private FakeInventory inventory;
        private bool _mInteract;
        
        void Start()
        {
            text.text = "Press [Space] to pick ";
            text.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_mInteract) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inventory.GetItem();
                _mInteract = false;
                text.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            text.gameObject.SetActive(true);
            _mInteract = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            text.gameObject.SetActive(true);
            _mInteract = true;
        }

        private void OnTriggerExit(Collider other)
        {
            text.gameObject.SetActive(false);
            _mInteract = false;
        }
    }
}
