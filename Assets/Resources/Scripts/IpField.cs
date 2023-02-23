using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class IpField : MonoBehaviour
{
    [SerializeField] private UnityTransport _transport;
    [SerializeField] private TMP_InputField _ipText;

    private void Start()
    {
        _ipText.text = _transport.ConnectionData.Address;
    }

    public void SetIp(string ip)
    {
        _transport.ConnectionData.Address = ip;
    }
}
