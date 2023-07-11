using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

public class StartButtonInitializer : MonoBehaviour
{
    [SerializeField] private CharacterSelector _characterSelectorPrefab;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener(() => SampleScene.Load(_characterSelectorPrefab.SelectedCharacterIndex));
    }
}
