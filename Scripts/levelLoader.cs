using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class levelLoader : NetworkBehaviour
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
                if(NetworkManager.Singleton.IsHost){
                    InfiniteGenNet.instance.loadNextLevel();
                    GetComponent<NetworkObject>().Despawn(true);
                    Destroy(gameObject);
                }
                else{
                    InfiniteGenNet.instance.loadNextLevelServerRpc();
                    destroyLoaderServerRpc();
                }


                
            }
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void destroyLoaderServerRpc(){
        GetComponent<NetworkObject>().Despawn(true);
        Destroy(gameObject);
    }

}
