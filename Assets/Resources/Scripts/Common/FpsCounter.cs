using UnityEngine;
using TMPro;
using System.Linq;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private float _hudRefreshTime = 1f;
    [SerializeField] private int _smoothFix = 20;

    private string[] _possibleValues;
    private float[] _fpsValues;
    private int _currentFrame;
    private float _timer;

    private const int MaxFpsValue = 60;

    private void OnValidate()
    {
        _fpsValues = new float[_smoothFix];
    }

    private void Start()
    {
        var possibleValues = Enumerable.Range(0, MaxFpsValue + 1);
        _possibleValues = possibleValues.Select(x => x.ToString()).ToArray();
    }

    private void Update()
    {
        if (_timer > _hudRefreshTime)
        {
            UpdateFpsCounter();
            _timer = _timer - _hudRefreshTime + Time.unscaledDeltaTime;
        }
        else
        {
            _timer += Time.unscaledDeltaTime;
        }

        if (_currentFrame >= _fpsValues.Length)
        {
            _currentFrame = 0;
        }
        _fpsValues[_currentFrame] = 1f / Time.unscaledDeltaTime;
        _currentFrame++;
    }

    private void UpdateFpsCounter()
    {
        float uncupedAverageFps = 0;
        foreach(float currentFps in _fpsValues)
        {
            uncupedAverageFps += currentFps;
        }
        uncupedAverageFps /= _fpsValues.Length;
        int averageFps = Mathf.Clamp(
            Mathf.RoundToInt(uncupedAverageFps),
            0, MaxFpsValue);

        _fpsText.SetText(_possibleValues[averageFps]);
    }
}
