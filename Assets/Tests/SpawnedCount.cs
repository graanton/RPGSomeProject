using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tests
{
    public class SpawnedCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countLabel;

        private int _count;

        public void OnSpawn(GameObject spawned)
        {
            _count++;
            _countLabel.text = _count.ToString();
        }
    }
}

