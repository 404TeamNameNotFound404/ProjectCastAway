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
        private static int mBulbCounter = 0;
        public static bool WinConditionReached => WinCondition();

        public static bool itemPicked { get; set; } = false;

        private void Start()
        {
            textCounter.text = mBulbCounter.ToString();
        }
        
        // itemPicked will set to true via player OnTriggerEnter function with the relative tag 
        public void GetItem()
        {
            mBulbCounter++;
            textCounter.text = mBulbCounter.ToString();
            itemPicked = false;
        }

        private static bool WinCondition()
        {
            return mBulbCounter >= 2;
        }
    
    }
}
