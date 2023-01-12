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
