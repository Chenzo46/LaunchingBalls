using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource ad;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void playSoundNormal(AudioClip clip)
    {
        GameObject g = new GameObject();
        g.AddComponent<AudioSource>();
        g.name = "Audio m1";
        AudioSource AdSrc = g.GetComponent<AudioSource>();


        Destroy(g, 1f);

        AdSrc.volume = 1;
        AdSrc.pitch = 1;

        AdSrc.PlayOneShot(clip);
    }

    public void playSoundRand(AudioClip clip, float vol_magnitude, bool randomPitch)
    {
        GameObject g = new GameObject();
        g.AddComponent<AudioSource>();
        AudioSource AdSrc = g.GetComponent<AudioSource>();
        g.name = "Audio m2";
        Destroy(g, 1f);

        AdSrc.volume = 1;
        AdSrc.pitch = 1;

        AdSrc.volume *= vol_magnitude;

        if (randomPitch)
        {
            AdSrc.pitch = Random.Range(0.5f,1.5f);
        }

        AdSrc.PlayOneShot(clip);
                

    }

    public void playSoundDesc(AudioClip clip, float vol_magnitude, float pitch)
    {
        GameObject g = new GameObject();
        g.AddComponent<AudioSource>();
        AudioSource AdSrc = g.GetComponent<AudioSource>();
        g.name = "Audio m3";
        Destroy(g, 1f);

        AdSrc.volume = 1;

        AdSrc.volume *= vol_magnitude;

        AdSrc.pitch = pitch;

        Debug.Log("Pitch: " + AdSrc.pitch.ToString());

        AdSrc.PlayOneShot(clip);

    }



}
