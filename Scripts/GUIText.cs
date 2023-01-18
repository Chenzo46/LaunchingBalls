using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text level_text;
    [SerializeField] private TMP_Text throw_text;
    private void Awake()
    {
        level_text.text = "Level: " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    void Update()
    {
        throw_text.text =  throwScript.instance.getThrows().ToString();
    }
}
