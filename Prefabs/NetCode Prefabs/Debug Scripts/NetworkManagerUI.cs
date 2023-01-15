using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.Collections;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private RealyManager rm;

    [SerializeField] private TMP_Text joinCodeText;

    [SerializeField] private Animator multGUI;

    [SerializeField] private Button startGame;

    [Header("Objects to enable/disable")]
    [SerializeField] private GameObject p2GUI;

    public static NetworkManagerUI instance;


    void Awake(){
        instance = this;
    }

    private void OnEnable() {
        NetworkManager.Singleton.OnClientDisconnectCallback += p2Left;
    }

    private void p2Left(ulong clientId){
         p2GUI.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerJoinedUIServerRpc(){
        p2GUI.SetActive(true);
    }

    public void localUpdatePlayerUI(){
        if(!IsHost){
            p2GUI.SetActive(true);
            startGame.gameObject.SetActive(false);
            joinCodeText.text = "Waiting for host to start game...";
        }

    }

    public void start_Host(){
        StartCoroutine(rm.startRelayHost());
    }


    public void start_Client(){
        try{
            StartCoroutine(rm.JoinRelay(joinInput.text));
        }
        catch (Exception e){
            Debug.Log($"Function at {e.StackTrace} failed with the following Exception: {e.Message}");
        }
    }

    public void toHostScreen(){
        try{
            start_Host();
        }
        catch (Exception e){
            Debug.Log($"Function at {e.StackTrace} failed with the following Exception: {e.Message}");
        }
    }

    public void toMultOptions(){
        multGUI.SetTrigger("toMain");
    }

    public void startGameFromMenu(){
        multGUI.SetTrigger("toGame");
    }

    public void connectionFailed(){
        multGUI.SetTrigger("connectFailed");
    }

    public void loading(){
        multGUI.SetTrigger("toJoin");
    }

    public void toRoom(){
        multGUI.SetTrigger("connected");
    }


}
