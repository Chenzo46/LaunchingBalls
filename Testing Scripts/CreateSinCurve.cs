using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSinCurve : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    [SerializeField] private int pointsInCurve;

    [SerializeField] private float Amplitude = 1;
    [SerializeField] private float Frequency = 1;

    [SerializeField] private float Accuracy = 1;

    private void Awake(){
        createSinCurve(Amplitude, Frequency, Accuracy);
    }

    void Update(){
        createSinCurve(Amplitude, Frequency, Accuracy);
    }

    /*
    if(Input.GetKeyDown(KeyCode.Space)){
            createSinCurve(Amplitude, Frequency, Accuracy);
        }
    */




    private void createSinCurve(float amplitude, float frequency, float accuracy){
        lr.positionCount = pointsInCurve;
        float x = 0;
        for(int i = 0; i < pointsInCurve; i++){
            x += Mathf.PI/pointsInCurve + accuracy;
            float y = Amplitude * Mathf.Sin(x * frequency);

            lr.SetPosition(i, new Vector2(x,y));
        }
    }
}
