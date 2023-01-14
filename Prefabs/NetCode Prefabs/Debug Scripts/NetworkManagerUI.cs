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

    [SerializeField] private Animator multGUI;

    public void start_Server(){
        NetworkManager.Singleton.StartServer();
    }
    public void start_Host(){
        StartCoroutine(rm.startRelayHost());

    }
    public void start_Client(){
        StartCoroutine(rm.JoinRelay(joinInput.text));
    }

    public void toHostScreen(){
        start_Host();
        multGUI.SetTrigger("toRoom");
    }

    public void toMultOptions(){
        multGUI.SetTrigger("toMain");
    }

    public void startGameFromMenu(){
        multGUI.SetTrigger("toGame");
    }
}
