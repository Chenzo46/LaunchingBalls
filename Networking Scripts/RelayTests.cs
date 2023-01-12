using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;

public class RelayTests : MonoBehaviour
{
    [SerializeField] private TMP_Text joinCode;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private GameObject n_Buttons;

    private UnityTransport _transport;
    private const int MaxPlayers = 2;
    

    void Awake() {
        _transport = FindObjectOfType<UnityTransport>();
    }
}
