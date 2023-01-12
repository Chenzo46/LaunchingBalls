using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class levelLoader : MonoBehaviour
{
    [SerializeField] private bool inc = true;

    private void OnTriggerEnter2D(Collider2D other) {

        //GetComponent<NetworkObject>().NetworkManager

        if(other.tag.Equals("Player")){
            if(InfiniteGen.instance != null){
                InfiniteGen.instance.loadNextLevel();
                if(inc){
                    InfiniteGen.instance.incRoomsCompleted();
                }
                
                Destroy(gameObject);
            }
            else{
                //other.gameObject.GetComponent<NetworkPlayerController>().spawnNextRoomServerRpc(inc);
                Destroy(gameObject);
            }
            
        }
    }

}
