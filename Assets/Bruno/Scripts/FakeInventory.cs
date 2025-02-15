using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Bruno.Scripts
{
    public class FakeInventory : MonoBehaviour
    {
        [SerializeField] private RawImage ui;
        [SerializeField] private TMP_Text textCounter;
        private int _mBulbCounter = 0;

        public static bool itemPicked { get; set; } = false;

        private void Start()
        {
            textCounter.text = _mBulbCounter.ToString();
        }
        
        // itemPicked will set to true via player OnTriggerEnter function with the relative tag 
        public void GetItem()
        {
            _mBulbCounter++;
            textCounter.text = _mBulbCounter.ToString();
            itemPicked = false;
        }

        public void CheckWinCondition()
        {
            if (_mBulbCounter >= 2)
            {
                //TODO lighthouse will call the boat
            }
        }
    
    }
}
