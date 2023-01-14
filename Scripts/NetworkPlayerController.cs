using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections.Generic;
using Cinemachine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float radiusOffset;
    [SerializeField] private float throwMultiplier;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform throwInd;
    [SerializeField] private float indicatorDrawback = 1f;
    [SerializeField] private float dragLimit = 30f;
    [SerializeField] private GameObject wallSplat;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private GameObject plr_Death;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private Color p2Color;

    [SerializeField] private Color otherAlphaColor;

    private Camera cineCamParent;

    NetworkVariable<Color> p2ColorNet = new NetworkVariable<Color>(Color.red, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    private NetworkClient networkClient; 

    private int keysCollected = 0;

    private int bounces = 0;

    private Animator gui_anim;

    private bool stopPlr = false;

    private bool hasLerped = true;

    private Vector2 lerpPos = new Vector2();

    [SerializeField] private int maxThrows = 1;

    private bool holding = false;
    private float radius = 0f;

    float mouseDist = 0f;

    private Vector2 lastCheckPointTouched = new Vector2();

    private Vector2 mousePos = new Vector2();

    private Vector2 throwDir = new Vector2();

    private bool respawning = false;

    // Start is called before the first frame update

    void Start()
    {
        radius = transform.localScale.x;

    }

    public void updateAlphaValues(){

        if(IsOwner){
            GameObject[] plrs = GameObject.FindGameObjectsWithTag("Player");
            List<GameObject> pl_ls = new List<GameObject>(plrs);

            pl_ls.Remove(gameObject);

            foreach(GameObject g in pl_ls){
                Destroy(g.GetComponent<LineRenderer>());
                Destroy(g.GetComponentInChildren<LineRenderer>());
                Color orgColor = g.GetComponent<SpriteRenderer>().color;  
                g.GetComponent<SpriteRenderer>().color = new Color(p2Color.r, p2Color.g,p2Color.b, orgColor.a/2);
            }

        }
    }

    public override void OnNetworkSpawn()
    {
        GameObject[] plrs = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject g in plrs){
            g.GetComponent<NetworkPlayerController>().updateAlphaValues();
        }


        if(IsOwner){
            Vector2 spawnPos = GameObject.FindGameObjectWithTag("spawnPos1").transform.position;
            transform.position = spawnPos;
            lastCheckPointTouched = spawnPos;
        }

        if(IsClient && IsOwner){
            cineCamParent = CinecamFollowScript.instance.gameObject.GetComponentInParent<Camera>();

            CinecamFollowScript.instance.setFollowTarget(transform);
        }
        
    }
    /*
    [ServerRpc]
    public void spawnNextRoomServerRpc(bool incRm){
        Debug.Log("Spawning next room through serverrpc");
        InfiniteGenNet.instance.loadNextLevel();
        if(incRm){
            InfiniteGenNet.instance.incRoomsCompleted();
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
        {
            return;
        } 
        if (!stopPlr)
        {
            mousePos = cineCamParent.ScreenToWorldPoint(Input.mousePosition);

            if(mouseDist <= dragLimit)
            {
                throwDir = (Vector2)transform.position - mousePos;
            }

            throwInd.position = mousePos;
            mouseDist = Vector2.Distance(mousePos, transform.position);

            if (Input.GetMouseButtonDown(0) && mouseDist <= radius + radiusOffset && maxThrows > 0)
            {
                holding = true;
            }
            else if (Input.GetMouseButtonUp(0) && holding)
            {

                AudioManager.instance.playSoundNormal(throwSound);

                rb2D.velocity = Vector2.zero;

                hasLerped = true;

                
                rb2D.AddForce(throwMultiplier * throwDir, ForceMode2D.Impulse);
                holding = false;

                maxThrows--;
            }

            //Debug.Log(rb2D.velocity.magnitude);

            if(rb2D.velocity.magnitude <= 0.1f && maxThrows < 1 && !stopPlr && !respawning)
            {
                StartCoroutine(killPlayer());
            }


            if (holding)
            {
                lr.SetPosition(1, -throwInd.localPosition / indicatorDrawback);
            }
            else
            {
                lr.SetPosition(1, Vector2.zero);
                lr.startColor = new Color(255, 255, 255);
            }
        }
        

        if (!hasLerped)
        {
            if (Vector2.Distance(transform.position, lerpPos) > 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, lerpPos, 0.1f);
            }
            else if (Vector2.Distance(transform.position, lerpPos) <= 0.1f)
            {
                hasLerped = true;
            }
        }
        

    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "walls")
        {

            //CamerShake.instance.StartShake(0.1f, 0.07f * rb2D.velocity.magnitude/75);
            AudioManager.instance.playSoundRand(bounceSound, rb2D.velocity.magnitude / 75, true);

            ContactPoint2D ct = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, ct.normal);
            Vector2 pos = ct.point;
            GameObject g = Instantiate(wallSplat, pos, rot);
            Destroy(g,1.5f);
            g.GetComponent<ParticleSystem>().Stop();
            var gp = g.GetComponent<ParticleSystem>().main;
            gp.duration *= rb2D.velocity.magnitude/100;
            gp.startSizeMultiplier = rb2D.velocity.magnitude/100;
            gp.emitterVelocity *= rb2D.velocity.magnitude/200;
            g.GetComponent<ParticleSystem>().Play();

            bounces++;
        }
        else if (collision.gameObject.tag == "badwalls")
        {
            //CamerShake.instance.StartShake(0.1f, 0.07f * rb2D.velocity.magnitude / 75);
            AudioManager.instance.playSoundRand(bounceSound, rb2D.velocity.magnitude / 75, true);

            StartCoroutine(killPlayer());
            
        }
        else if (collision.gameObject.tag == "UIWalls" || collision.gameObject.tag == "Player")
        {
            //CamerShake.instance.StartShake(0.1f, 0.07f * 1.5f * rb2D.velocity.magnitude / 75);
            AudioManager.instance.playSoundRand(bounceSound, rb2D.velocity.magnitude / 75, true);
        }
    }


    public void incThrows()
    {
        maxThrows = 1;
    }

    public void stopPlayer()
    {
        rb2D.velocity = Vector2.zero;
    }

    public void lerpPlayerToPos(Vector2 pos)
    {
        hasLerped = false;
        lerpPos = pos;

    }

    public void toggleInput()
    {
        stopPlr = !stopPlr;
    }

    public void incKeys()
    {
        keysCollected++;
    }

    public int getKeys()
    {
        return keysCollected;
    }

    public IEnumerator killPlayer()
    {
        stopPlayer();
        toggleInput();
        Instantiate(plr_Death, transform.position, plr_Death.transform.rotation);

        //for when we don't want to reload the scene
        Sprite orgSpr = gameObject.GetComponent<SpriteRenderer>().sprite;
        float orgWidth = gameObject.GetComponent<LineRenderer>().widthMultiplier;

        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<LineRenderer>().widthMultiplier = 0;
        AudioManager.instance.playSoundNormal(deathSound);
        
        respawning = true;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = orgSpr;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<LineRenderer>().widthMultiplier = orgWidth;

        incThrows();

        respawning = false;

        transform.position = lastCheckPointTouched;

        toggleInput();
    }

    public void updateLastCheckPointTouched(Vector2 point){
        lastCheckPointTouched = point;
    }

    public void toggleResapwn(){
        respawning = false;
    }
}
