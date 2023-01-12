using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinecamFollowScript : MonoBehaviour
{
   private CinemachineVirtualCamera m_Cam;
   public static CinecamFollowScript instance;

   private void Awake(){
        m_Cam = GetComponent<CinemachineVirtualCamera>();
        instance = this;
   }



   public void setFollowTarget(Transform target){
        m_Cam.Follow = target;
   }
}
