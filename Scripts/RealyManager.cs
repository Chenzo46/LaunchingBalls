using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using TMPro;
using System;
using System.Linq;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

public class RealyManager : MonoBehaviour
{
    [SerializeField] private string environment = "production";

    [SerializeField] private TMP_Text joinText;

    [SerializeField] private int maxConnections = 2;

    public static RealyManager instance;

    public bool isRelayEnabled => Transport != null && 
        Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    private void Awake() {
        instance = this;
    }
    
    public async Task<RelayServerData> SetupRelay(){
        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerData rData = new RelayServerData(allocation, "dtls");

        Transport.SetRelayServerData(rData);

        Debug.Log($"Join code created: {joinCode}");
        joinText.text = "Join Code: " + joinCode;
        
        return rData;
    }

    public async Task<RelayServerData> JoinRelay(string joinCode){
        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();


        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayServerData rData = new RelayServerData(allocation, "dtls");

        Transport.SetRelayServerData(rData);

        Debug.Log("Client Joined game");

        return rData;
    }
}
