using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class InfiniteGenNet : NetworkBehaviour
{
    [SerializeField] private GameObject[] rooms;

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private int gameTime = 5; //Time in minutes

    [SerializeField] private Animator transAnim;

    private float gameSeconds = 60;
    
    private List<GameObject> loadedRooms = new List<GameObject>();

    private int roomIndex = 0;

    private int roomsCompleted = 0;

    private bool end = false;


    public static InfiniteGenNet instance;

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
        end = true;

        transAnim.SetTrigger("out");

        yield return new WaitForSeconds(1.3f);
        Debug.Log("Game ended");
        //SceneManager.LoadScene(0);
    }
    public void loadNextLevel(){
        roomIndex += 29;

        if(loadedRooms.Count > rooms.Length-1){
            GameObject s = loadedRooms[0];
            GameObject instanceOf = GameObject.Find(s.name + "(Clone)");
            instanceOf.GetComponent<NetworkObject>().Despawn();
            loadedRooms.Remove(s);
            Debug.Log("Room Unloaded: " + s.name);
        }

        List<GameObject> unloadedRooms = getUnloadedRooms();

        int r = Random.Range(0,unloadedRooms.Count);

        GameObject newR = Instantiate(unloadedRooms[r], Vector2.right*roomIndex, Quaternion.identity);
        newR.GetComponent<NetworkObject>().Spawn();
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

    private void clearAllRooms(){
        //Gets all the loaded rooms, clears the loaded room list, and destroys all rooms in the scene.
    }
}
