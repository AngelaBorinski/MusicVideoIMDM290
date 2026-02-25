using System;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class celestial : MonoBehaviour
{
    public AudioSource source;
    public float updateStep = 0.1f;
    public static int FFTSIZE = 1024;
    public static float[] samples = new float[FFTSIZE];
    public float audioAmp;
    public float songLength = 101;

    static int flyingObjNum = 10;
    public static GameObject[] flyingObjects = new GameObject[flyingObjNum];
    public Vector3[] startPos, endPos;
    //bool[] moved = new bool[flyingObjNum];


    public float xPos = 1000;
    public float yPos = 2000;
    public float zPos = -1000;

    float lerpFraction;


    float time = 0f;

    [SerializeField] GameObject shootingStarPrefab;
    [SerializeField] GameObject cometPrefab;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        // Assign proper types and sizes to the variables.
        //flyingObjects = new GameObject[flyingObjNum];
        startPos = new Vector3[flyingObjNum];
        endPos = new Vector3[flyingObjNum];

        // Define target positions
        for (int i = 0; i < flyingObjNum; i++)
        {
            startPos[i] = new Vector3(xPos, yPos, zPos);
            float xEndPos = -xPos;
            float yEndPos = -600;
            float zEndPos = -800;
            endPos[i] = new Vector3(xEndPos, yEndPos, zEndPos);
            xPos -= 200;

            flyingObjects[i] = Instantiate((i % 2 == 0) ? shootingStarPrefab : cometPrefab, startPos[i], Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        //int objNum = UnityEngine.Random.Range(0, flyingObjNum);
        float lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

        for (int i = 0; i < flyingObjNum; i++)
        {
            flyingObjects[i].transform.position = Vector3.Lerp(startPos[i], endPos[i], lerpFraction);
        }

        // Lerp logic. Update position
        //flyingObjects[objNum].transform.position = Vector3.Lerp(startPos[objNum], endPos[objNum], lerpFraction);

        if (time % songLength >= 0 && time % songLength < 15)
        {

        }
        else if (time % songLength >= 15 && time % songLength < 75)
        {

        }
        else if (time % songLength >= 75)
        {

        }

    }
}
