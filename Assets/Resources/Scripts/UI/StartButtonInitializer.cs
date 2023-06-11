using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

public class StartButtonInitializer : MonoBehaviour
{
    [SerializeField] private CharacterSelector _characterSelector;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener( delegate () { SampleScene.Load(_characterSelector.SelectedCharacterIndex); });
    }
}
