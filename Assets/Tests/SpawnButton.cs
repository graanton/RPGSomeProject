using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Tests
{
    public class SpawnButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Material _placeMaterial;
        [SerializeField] private float _range = 3;

        public SpawnEvent spawnEvent = new();

        private Random _random = new Random();

        public void OnPointerDown(PointerEventData eventData)
        {
            var spawned = Instantiate(_prefab, _spawnPoint.position + 
                new Vector3((float)_random.NextDouble() * _range, 0, (float)_random.NextDouble() * _range) - new Vector3(_range / 2, 0 , _range / 2), _spawnPoint.rotation);
            spawned.GetComponent<MeshRenderer>().material = _placeMaterial;
            spawnEvent.Invoke(spawned);
        }
    }

    [Serializable]
    public class SpawnEvent: UnityEvent<GameObject> { }
}
