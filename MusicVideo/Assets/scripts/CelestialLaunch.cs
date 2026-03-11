// CelestialLaunch.cs
using System.Collections;
using UnityEngine;

public class CelestialLaunch : MonoBehaviour
{
    const int totalObjects = 10;
    Celestial[] flyingObjects = new Celestial[totalObjects];
    int[] objNums = { 2, 7, 3, 9, 1, 5, 4, 8, 0, 6 };
    int objNumIdx = 0;

    // 125 BPM, launch once per 4-note cluster = every 1.92s
    const float bpm = 125f;
    const float clusterInterval = (60f / bpm) * 4f;
    public float startDelay = 0f;
    float nextLaunchTime = 0f;
    float launchDuration = 1.8f;

    // minimum time between onset-triggered launches to avoid double firing
    float lastLaunchTime = -999f;
    float launchCooldown = 1.5f; // slightly under clusterInterval

    [SerializeField] GameObject shootingStarPrefab;
    [SerializeField] GameObject cometPrefab;

    void Start()
    {
        GetComponent<AudioSource>().Play();
        nextLaunchTime = startDelay;

        Vector3[] startPositions = new Vector3[totalObjects];
        Vector3[] endPositions = new Vector3[totalObjects];

        for (int i = 0; i < totalObjects; i++)
        {
            float startX = 900f - (i * 200f);
            float endX = (i < 5) ? (-100f - (i * 100f)) : (100f + ((i - 5) * 100f));
            startPositions[i] = new Vector3(startX, 350f, -1000f);
            endPositions[i] = new Vector3(endX, -700f, -1000f);

            flyingObjects[i] = new Celestial();
            flyingObjects[i].objNum = i;
            flyingObjects[i].startPos = startPositions[i];
            flyingObjects[i].endPos = endPositions[i];
            flyingObjects[i].controlPoint = new Vector3(
                (startPositions[i].x + endPositions[i].x) / 2f,
                0f,
                -1000f
            );
            flyingObjects[i].obj =
                Instantiate((i % 2 == 0) ? shootingStarPrefab : cometPrefab,
                flyingObjects[i].startPos,
                Quaternion.identity);
        }
    }

    void Update()
    {
        // BPM timer drives the launch so it stays locked to the beat
        if (Time.time >= nextLaunchTime)
        {
            nextLaunchTime += clusterInterval;

            // only fire if onset is also detected nearby, keeping sync tight
            if (AudioSpectrum.onsetDetected || Time.time - lastLaunchTime > launchCooldown)
            {
                LaunchNextObject();
                lastLaunchTime = Time.time;
            }
        }
    }

    void LaunchNextObject()
    {
        int i = objNums[objNumIdx % objNums.Length];
        if (flyingObjects[i].isLaunching) return;

        StartCoroutine(LaunchRoutine(flyingObjects[i]));
        objNumIdx++;
    }

    IEnumerator LaunchRoutine(Celestial c)
    {
        c.isLaunching = true;
        float elapsed = 0f;

        while (elapsed < launchDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / launchDuration);
            c.obj.transform.position =
                c.QuadraticBezier(c.startPos, c.controlPoint, c.endPos, t);
            yield return null;
        }

        c.obj.transform.position = c.startPos;
        c.isLaunching = false;
    }
}