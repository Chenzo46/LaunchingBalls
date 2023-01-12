using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForMusic : MonoBehaviour
{
    [SerializeField] private GameObject mainMusic;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("mainLoop") == null)
        {
            GameObject g = Instantiate(mainMusic);
            DontDestroyOnLoad(g);
        }
    }
}
