using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class flagPole : MonoBehaviour
{

    [SerializeField] private AudioClip finishLevel;
    [SerializeField] private AudioClip expunge;
    [SerializeField] private bool needsKey;
    private Animator gui_anim;
    private Animator plr_anim;

    private void Awake()
    {
        try
        {
            gui_anim = GameObject.FindGameObjectWithTag("transition").GetComponent<Animator>();
            plr_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }
        catch
        {
            Debug.Log("Transition Object not present in scene");
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !needsKey)
        {
            StartCoroutine(moveToNextLevel());
        }
        else if(collision.tag.Equals("Player") && throwScript.instance.getKeys() > 0)
        {
            StartCoroutine(moveToNextLevel());
        }
    }

    private IEnumerator moveToNextLevel()
    {
        //Put other animations here
        AudioManager.instance.playSoundNormal(finishLevel);
        throwScript.instance.lerpPlayerToPos(transform.position);
        throwScript.instance.toggleInput();
        throwScript.instance.stopPlayer();
        plr_anim.SetTrigger("levelEnd");
        yield return new WaitForSeconds(0.5f);
        gui_anim.SetTrigger("out");
        AudioManager.instance.playSoundNormal(expunge);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
