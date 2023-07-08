using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;

public class ServerData
{
    public int Level { get; set; }
    public int PlayerCount { get; set; }
    public bool IsInLobby { get; set; }
}

namespace ChatGPTCommon
{
    public class NetworkDiscoveryManager : MonoBehaviour
    {
        
    }
}