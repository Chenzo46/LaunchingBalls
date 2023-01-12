using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private GameObject KeyIcon;

    private void Awake()
    {
        KeyIcon = GameObject.FindGameObjectWithTag("KeyIcon");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            //Key Collection
            AudioManager.instance.playSoundNormal(collectSound);
            throwScript.instance.incKeys();
            throwScript.instance.lerpPlayerToPos(transform.position);
            throwScript.instance.stopPlayer();
            throwScript.instance.incThrows();
            KeyIcon.GetComponent<Image>().enabled = true;
            Destroy(gameObject);
        }
    }
}
