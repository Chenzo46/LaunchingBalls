using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CreateSinCurve : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    [SerializeField] private int pointsInCurve;

    [SerializeField] private float Amplitude = 1;
    [SerializeField] private float Frequency = 1;

    [SerializeField] private float Distance = 1;

    [SerializeField] private EdgeCollider2D col;

    private SpriteShapeGenerator gen;

    [SerializeField] private SpriteShapeController genCon;

    private List<Vector2> linePoints = new List<Vector2>();



    private void Awake(){
    }

    void Update(){
        createSinCurve();
    }

    /*
    if(Input.GetKeyDown(KeyCode.Space)){
            createSinCurve(Amplitude, Frequency, Accuracy);
        }
    */




    private void createSinCurve(){
        linePoints.Clear();
        col.points = new Vector2[pointsInCurve];

        lr.positionCount = pointsInCurve;

        float x,y;

        float xStart = 0;

        float xFinish = 2 * Mathf.PI * Distance;

        for(int i = 0; i < pointsInCurve; i++){

            float progress = (float)i/(pointsInCurve-1);
            x = Mathf.Lerp(xStart, xFinish,progress);
            y = Amplitude*Mathf.Cos(x)*Mathf.Sin(Frequency*Time.time*x*Mathf.Cos(x));  //Amplitude*Mathf.Cos(Time.time)*Mathf.Sin(x*Frequency*Mathf.Cos(Time.time)+Time.time);
            lr.SetPosition(i,new Vector2(x,y));
            linePoints.Add(new Vector2(x,y));

        }

        col.SetPoints(linePoints);


    }


}
