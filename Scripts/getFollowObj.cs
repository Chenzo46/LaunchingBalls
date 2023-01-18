using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Data;
using Unity.VisualScripting;

public class getFollowObj : MonoBehaviour
{
    private CinemachineVirtualCamera cmCam;

    public static getFollowObj instance;


    [SerializeField] private float mainOrthoSize = 6.875f;
    [SerializeField] private float anchoredCamSize = 10f;

    private float plrOrthoSize;

    private bool anchored = false;

    private GameObject anchorPoint;
    private GameObject Player;

    private void Awake()
    {
        instance= this;
         
        cmCam = gameObject.GetComponent<CinemachineVirtualCamera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        setFollowObj(Player);
        anchorPoint = GameObject.FindGameObjectWithTag("camAnch");

        plrOrthoSize = mainOrthoSize;
    }

    public void setFollowObj(GameObject obj)
    {
        //transform.position = obj.transform.position;
        cmCam.Follow = obj.transform;
    }

    private void Update()
    {
        //cmCam.m_Lens.OrthographicSize = Mathf.Lerp(cmCam.m_Lens.OrthographicSize, mainOrthoSize, followTransitionTime);
        if(anchorPoint){

            if (Input.GetKeyDown(KeyCode.Space) )
            {
                toggleAnchor();
            }
        }
        
        
    }

    public void toggleAnchor()
    {
        anchored = !anchored;

        if (anchored) 
        {
            setFollowObj(anchorPoint);
        }
        else
        {
            setFollowObj(Player);
        }
    }

}
