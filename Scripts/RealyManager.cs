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

    public async Task<RelayHostData> SetupRelay(){
        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if(!AuthenticationService.Instance.IsSignedIn){
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);

        RelayHostData relayHostData = new RelayHostData
        {
            key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData
            
        };

        relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

        Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes, relayHostData.key, relayHostData.ConnectionData);

        Debug.Log($"Join code created: {relayHostData.JoinCode}");
        joinText.text = relayHostData.JoinCode;

        return relayHostData;
    }

    public async Task<RelayJoinData> JoinRelay(string joinCode){
        InitializationOptions options = new InitializationOptions()
        .SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if(!AuthenticationService.Instance.IsSignedIn){
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }


        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

        RelayJoinData relayJoinData = new RelayJoinData
        {
            key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            HostConnectionData = allocation.HostConnectionData,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4,
            JoinCode = joinCode
            
        };

        Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes, relayJoinData.key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

        Debug.Log("Client Joined game");

        return relayJoinData;
    }
}
