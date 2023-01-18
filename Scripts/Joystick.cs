using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private float clickRange = 1f;
    [SerializeField] private float throwMult;

    [SerializeField] private float dragLimit = 5f;

    public delegate void JoystickChanged(Vector2 throwDirection, float throwMultiplier);

    public static event JoystickChanged OnJoystickLetGo;
    public static event JoystickChanged OnJoystickPressed;

    private Vector2 mainPos;
    private Vector2 dir = new Vector2();

    [HideInInspector]
    public bool holdingStick = false;

    public static Joystick instance;


    void Awake(){
        instance = this;
        mainPos = transform.position;
    }

    private void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButton(0) && Vector2.Distance(transform.position, mousePos) <= clickRange && !holdingStick && throwScript.instance.getThrows() > 0){

            if(OnJoystickPressed != null){
                OnJoystickPressed.Invoke(Vector2.zero, throwMult);
            }
            
            holdingStick = true;
        
        }

        if(holdingStick){

            if(Vector2.Distance((Vector2)gameObject.transform.parent.position, (Vector2)mousePos) <= dragLimit){
                dir = -((Vector2)transform.position - (Vector2)gameObject.transform.parent.position);
            }
            transform.position = new Vector3(mousePos.x, mousePos.y,0);
            
        }

        if(Input.GetMouseButtonUp(0) && holdingStick){
            if(OnJoystickLetGo != null){
                OnJoystickLetGo.Invoke(dir, throwMult);
            }
            holdingStick = false;
            transform.localPosition = Vector2.zero;
        }
    }

    public Vector2 getStickPositon(){
        return transform.localPosition;
    }
}
