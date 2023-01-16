using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private string direction;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float startDir = 1;

    [SerializeField] private float startDelay = 0f;

    private float timePassed = 0f;
    private void Update(){

        if(startDelay > 0){
            startDelay -= Time.deltaTime;
            timePassed = Time.time;
        }
        else{
            if(direction.Equals("h")){
                transform.localPosition = new Vector2(startDir*Mathf.Sin((Time.time - timePassed) * speed)*distance, 0);
            }
            else if(direction.Equals("v")){
                transform.localPosition = new Vector2(0, startDir*Mathf.Sin((Time.time - timePassed) *speed)*distance);
            }
        }
        
        
        
    }
}
