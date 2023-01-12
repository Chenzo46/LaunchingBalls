using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_Text hsText;
    private Animator gui_anim;
    private Animator main_Anim;

    private void Awake()
    {
        try
        {
            gui_anim = GameObject.FindGameObjectWithTag("transition").GetComponent<Animator>();
            main_Anim = GetComponent<Animator>();
            //plr_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }
        catch
        {
            Debug.Log("Transition Object not present in scene");
        }

        hsText.text = "High Score: " + PlayerPrefs.GetInt("TA_SCORE").ToString();
    }



    public void PlayGame()
    {
        StartCoroutine(startGame());
    }

    public void PlayMultiplayer(){
        StartCoroutine(startMult());
    }

    public void loadTimeAttack(){
        StartCoroutine(startTA());
    }

    public void goToModes(){
        main_Anim.SetTrigger("toModes");
    }

    public void goToMain(){
        main_Anim.SetTrigger("toMain");
    }

    public void goToMult(){
        main_Anim.SetTrigger("toMult");
    }

    private IEnumerator startGame()
    {
        gui_anim.SetTrigger("out");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
        private IEnumerator startTA()
    {
        gui_anim.SetTrigger("out");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(18);
    }

    private IEnumerator startMult(){
        gui_anim.SetTrigger("out");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(19);
    }

}
