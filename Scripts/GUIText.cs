using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text level_text;
    [SerializeField] private TMP_Text throw_text;
    [SerializeField] private TMP_Text Bounce_text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        level_text.text = "Level: " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        throw_text.text = "Throws: " + throwScript.instance.getThrows().ToString();
        Bounce_text.text = "Bounces: " + throwScript.instance.getBounces().ToString();
    }
}
