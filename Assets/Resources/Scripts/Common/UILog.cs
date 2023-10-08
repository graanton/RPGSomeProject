using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _log;

    private List<string> _messages = new();
    
    private void Awake()
    {
        Application.logMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(string condition, string stacktrace, LogType type)
    {
        if (_messages.Contains(stacktrace) == false)
        {
            _log.text += stacktrace + "\n";
            _messages.Add(stacktrace);
        }
    }
}
