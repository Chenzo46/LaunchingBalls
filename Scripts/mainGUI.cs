using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainGUI : MonoBehaviour
{
    [SerializeField] private Image toggledVisionImg;
    [SerializeField] private Sprite plrAnch;
    [SerializeField] private Sprite lvlAnch;

    private bool tog = false;

    public void ResetLevel()
    {
        StartCoroutine(throwScript.instance.killPlayer());

        
        if(Camera.main != null){
            Canvas ca = GetComponent<Canvas>();
            ca.renderMode = RenderMode.ScreenSpaceCamera;
            ca.worldCamera = Camera.main;
        }
        
    }

    public void toggleAnch()
    {

        getFollowObj.instance.toggleAnchor();
        tog = !tog;

        if (tog)
        {
            toggledVisionImg.sprite = lvlAnch;
        }
        else
        {
            toggledVisionImg.sprite = plrAnch;
        }
    }
}
