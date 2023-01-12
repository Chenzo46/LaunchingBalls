using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    [SerializeField] private AudioClip connectionSound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            AudioManager.instance.playSoundNormal(connectionSound);
            if(throwScript.instance != null){
                throwScript.instance.stopPlayer();
                throwScript.instance.lerpPlayerToPos(transform.position);
                throwScript.instance.incThrows();
                throwScript.instance.updateLastCheckPointTouched(transform.position);
                throwScript.instance.toggleResapwn();
            }
            else{
                NetworkPlayerController netPlayer = collision.gameObject.GetComponent<NetworkPlayerController>();

                netPlayer.stopPlayer();
                netPlayer.lerpPlayerToPos(transform.position);
                netPlayer.incThrows();
                netPlayer.updateLastCheckPointTouched(transform.position);
                netPlayer.toggleResapwn();
            }

        }
    }
}
