using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InfiniteGen : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private int gameTime = 5; //Time in minutes

    [SerializeField] private Animator transAnim;

    [SerializeField] private GameObject lvlLoader;

    private float gameSeconds = 60;
    
    private List<GameObject> loadedRooms = new List<GameObject>();

    private int roomIndex = 0;

    private int roomsCompleted = 0;

    private bool end = false;


    public static InfiniteGen instance;

    private void Awake() {
        instance = this;
    }

    void Update()
    {
        if(gameTime >= 0){
            gameSeconds -= Time.deltaTime;
            if(gameSeconds <= 0){
                gameTime-=1;
                gameSeconds = 60;
            }
            if(gameSeconds > 10){
                timeText.text = gameTime.ToString() + ":" + ((int)gameSeconds).ToString() + "s";
            }
            else{
                timeText.text = gameTime.ToString() + ":0" + ((int)gameSeconds).ToString() + "s";
            }
            
        }
        else if (!end){
            timeText.text = "0:00s";
            StartCoroutine(endMode());
        }

        
    }

    private IEnumerator endMode(){
        throwScript.instance.toggleInput();
        end = true;

        if(PlayerPrefs.GetInt("TA_SCORE") < roomsCompleted){
            PlayerPrefs.SetInt("TA_SCORE",  roomsCompleted);
        }
        transAnim.SetTrigger("out");

        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(0);
    }
    public void loadNextLevel(){
        roomIndex += 29;

        Instantiate(lvlLoader, new Vector2(roomIndex+6, -0.5f), Quaternion.identity);

        if(loadedRooms.Count > rooms.Length-(int)(rooms.Length/2)){
            GameObject s = loadedRooms[0];
            GameObject instanceOf = GameObject.Find(s.name + "(Clone)");
            Destroy(instanceOf);
            loadedRooms.Remove(s);
            Debug.Log("Room Unloaded: " + s.name);
        }

        List<GameObject> unloadedRooms = getUnloadedRooms();

        int r = Random.Range(0,unloadedRooms.Count);

        Instantiate(unloadedRooms[r], Vector2.right*roomIndex, Quaternion.identity);
        loadedRooms.Add(unloadedRooms[r]);
    }

    private List<GameObject> getUnloadedRooms(){
        List<GameObject> ulRooms = new List<GameObject>();

        foreach(GameObject g in rooms){
            if(!loadedRooms.Contains(g)){
                ulRooms.Add(g);
            }
        }

        return ulRooms;
    }

    public void incRoomsCompleted(){
        roomsCompleted++;
        scoreText.text = "Rooms Cleared: " + roomsCompleted.ToString();
    }
}
