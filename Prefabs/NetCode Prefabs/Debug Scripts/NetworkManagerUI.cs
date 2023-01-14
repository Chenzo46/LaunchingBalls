using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;
using UnityEngine.UI;
public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private RealyManager rm;

    [SerializeField] private Animator multGUI;

    [SerializeField] private Button startGame;

    [SerializeField] private TMP_InputField nameInput;

    private void OnEnable() {
        NetworkManager.Singleton.OnClientDisconnectCallback += connectionFailed;
    }

    public void start_Host(){
        StartCoroutine(rm.startRelayHost());
    }
    public void start_Client(){
        if(nameInput.text.Length != 0){
            multGUI.SetTrigger("toJoin");
            try{
                StartCoroutine(rm.JoinRelay(joinInput.text));
            }
            catch (Exception e){
                Debug.Log($"Function at {e.StackTrace} failed with the following Exception: {e.Message}");
            }
            
        }
        else{
            //Warn player that they must type a name
            Debug.Log("You must type a name!");
        }
        
    }

    public void toHostScreen(){
        if(nameInput.text.Length != 0){
            multGUI.SetTrigger("toRoom");
            try{
                start_Host();
            }
            catch (Exception e){
                Debug.Log($"Function at {e.StackTrace} failed with the following Exception: {e.Message}");
            }
            
        }
        else{
            //Warn player that they must type a name
            Debug.Log("You must type a name!");
        }
    }

    public void toMultOptions(){
        multGUI.SetTrigger("toMain");
    }

    public void startGameFromMenu(){
        multGUI.SetTrigger("toGame");
    }

    private void connectionFailed(ulong clientId){
        Debug.Log($"Connection failed from client id of {clientId}");
        multGUI.SetTrigger("connectFailed");
    }
}
