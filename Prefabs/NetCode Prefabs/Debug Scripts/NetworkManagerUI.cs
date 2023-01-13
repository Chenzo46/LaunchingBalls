using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private RealyManager rm;

    private void OnEnable() {
        //NetworkManager.Singleton.OnClientConnectedCallback += toHostScreen;
    }

    public void start_Server(){
        NetworkManager.Singleton.StartServer();
    }
    public async void start_Host(){
        await rm.SetupRelay();
        NetworkManager.Singleton.StartHost();
        
        //hostStarted = true;
    }
    public async void start_Client(){
        await rm.JoinRelay(joinInput.text);
        NetworkManager.Singleton.StartClient();
    }

    public void toHostScreen(){

    }
}
