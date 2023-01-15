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

    [SerializeField] private TMP_Text joinText;

    [SerializeField] private int maxConnections = 2;

    public static RealyManager instance;

    private void Awake() {
        instance = this;
    }
    
    public async Task<RelayServerData> SetupRelay(){

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerData rData = new RelayServerData(allocation, "dtls");
        
        //Transport.SetRelayServerData(rData);

        Debug.Log($"Join code created: {joinCode}");
        joinText.text = "Join Code: " + joinCode;
        
        return rData;
    }

    public IEnumerator startRelayHost(){
        var serverAlloTask = SetupRelay();

        NetworkManagerUI.instance.loading();

        while(!serverAlloTask.IsCompleted){
            yield return null;
        }
        if(serverAlloTask.IsFaulted){
            Debug.Log("Relay Server not started because the task failing");
            NetworkManagerUI.instance.connectionFailed();
            yield break;
        }

        var serverAlloData = serverAlloTask.Result;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverAlloData);

        NetworkManager.Singleton.StartHost();

        NetworkManagerUI.instance.toRoom();

        yield return null;

    }

    public async Task<RelayServerData> setupJoinRelay(string joinCode){

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();


        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayServerData rData = new RelayServerData(allocation, "dtls");

        return rData;
    }

    public IEnumerator JoinRelay(string joinCode){

        var clientAlloTask = setupJoinRelay(joinCode);

        NetworkManagerUI.instance.loading();

        while (!clientAlloTask.IsCompleted)
        {
            yield return null;
        }

        if (clientAlloTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientAlloTask.Exception.Message + " Stack Trace: " + clientAlloTask.Exception.StackTrace);
            yield break;
        }

        var relayServerData = clientAlloTask.Result;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();
        
        NetworkManagerUI.instance.toRoom();

        yield return null;
    }
}
