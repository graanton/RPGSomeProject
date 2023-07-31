using IJunior.TypedScenes;
using System;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller, ISceneLoadHandler<int>
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _defaultCharacterIndex;
    [SerializeField] private CharacterDatabase _characterDatabase;

    private bool _isSceneLoaded;
    private int _characterIndex;

    public void OnSceneLoaded(int characterIndex)
    {
        _characterIndex = characterIndex;
        _isSceneLoaded = true;
    }

    public override void InstallBindings()
    {
        if (!_isSceneLoaded)
        {
            _characterIndex = _defaultCharacterIndex;
        }
        var player = SpawnPlayer(_characterIndex);

        Container
            .Bind<PlayerHealth>()
            .FromInstance(player)
            .AsSingle();
    }

    private PlayerHealth SpawnPlayer(int characterIndex)
    {
        PlayerHealth spawnedPlayer = Container.InstantiatePrefabForComponent<PlayerHealth>(
            _characterDatabase.Characters[characterIndex].CharacterPrefab,
            _spawnPoint.position, Quaternion.identity, null);

        return spawnedPlayer;
    }
}