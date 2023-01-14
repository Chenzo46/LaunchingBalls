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

    private SpriteShapeGenerator gen;

    [SerializeField] private SpriteShapeController genCon;

    private List<Vector3> linePoints = new List<Vector3>();

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

        lr.positionCount = pointsInCurve;

        float x,y;

        float xStart = 0;

        float xFinish = 2 * Mathf.PI * Distance;

        for(int i = 0; i < pointsInCurve; i++){

            float progress = (float)i/(pointsInCurve-1);
            x = Mathf.Lerp(xStart, xFinish,progress);
            y = Amplitude*Mathf.Sin(x*Frequency);
            lr.SetPosition(i,new Vector2(x,y));
            linePoints.Add(new Vector3(x,y));

        }


    }

    [System.Obsolete]
    private void generateAreUnderCurve(){
        NativeArray<Vector3> nm = new NativeArray<Vector3>();
        NativeSlice<Vector3> nm1 = nm;
        nm.CopyFrom(linePoints.ToArray());

        gen.m_PosArray = nm1;

        
        SpriteShapeParameters spriteParams = new SpriteShapeParameters();

        Sprite[] s = new Sprite[0];

        AngleRangeInfo[] ang = new AngleRangeInfo[0];

        gen.Prepare(genCon, spriteParams, pointsInCurve*2, new NativeArray<ShapeControlPoint>(),new NativeArray<SpriteShapeMetaData>(),
        ang, s,  s);

        gen.Execute();


    }


}
