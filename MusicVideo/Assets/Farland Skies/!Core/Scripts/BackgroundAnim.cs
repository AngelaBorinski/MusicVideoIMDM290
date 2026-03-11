using UnityEngine;
using System;

public class BackgroundAnim : MonoBehaviour
{
    private float rotationSpeed = 10f;
    private float currentRotation = 0f;
    private float time = 0f;

    // Colors converted from 0-255 to 0-1 range
    // Purple/pink
    static Color purpleTop = new Color(0f, 140 / 255f, 255 / 255f);
    static Color purpleMid = new Color(228 / 255f, 140 / 255f, 232 / 255f);
    static Color purpleBot = new Color(255 / 255f, 0f, 0f);

    // Orange-pink
    static Color orangeTop = new Color(255 / 255f, 196 / 255f, 171 / 255f);
    static Color orangeMid = new Color(255 / 255f, 133 / 255f, 38 / 255f);
    static Color orangeBot = new Color(255 / 255f, 0f, 227 / 255f);

    // Pink-red
    static Color pinkredTop = new Color(255 / 255f, 74 / 255f, 28 / 255f);
    static Color pinkredMid = new Color(255 / 255f, 136 / 255f, 172 / 255f);
    static Color pinkredBot = new Color(255 / 255f, 43 / 255f, 0f);

    // Fiery pink/red
    static Color fieryTop = new Color(253 / 255f, 85 / 255f, 162 / 255f);
    static Color fieryMid = new Color(255 / 255f, 0f, 29 / 255f);
    static Color fieryBot = new Color(230 / 255f, 209 / 255f, 0f);

    // Each entry: (transitionStartTime, fullColorStartTime, top, middle, bottom)
    // Transition starts 4 seconds before the section's full color kicks in
    struct ColorSection
    {
        public float transitionStart; // when to begin fading IN to this color
        public float fullStart;       // when this color is fully shown
        public float fullEnd;         // when this color starts fading OUT
        public Color top, mid, bot;

        public ColorSection(float tStart, float fStart, float fEnd, Color t, Color m, Color b)
        {
            transitionStart = tStart;
            fullStart = fStart;
            fullEnd = fEnd;
            top = t; mid = m; bot = b;
        }
    }

    // Timestamps: 1:05=65s, 1:13=73s, 1:17=77s, 1:37=97s
    ColorSection[] sections;

    void Start()
    {
        sections = new ColorSection[]
        {
            //                  fadeIn   fullStart  fullEnd    top         mid         bot
            new ColorSection(   0f,      0f,        11f,       purpleTop,  purpleMid,  purpleBot),  // 0-15s purple
            new ColorSection(   11f,     15f,       26f,       orangeTop,  orangeMid,  orangeBot),  // 15-30s orange
            new ColorSection(   26f,     30f,       48f,       pinkredTop, pinkredMid, pinkredBot), // 30-52s pink-red
            new ColorSection(   48f,     52f,       61f,       purpleTop,  purpleMid,  purpleBot),  // 52-1:05 purple
            new ColorSection(   61f,     65f,       73f,       orangeTop,  orangeMid,  orangeBot),  // 1:05-1:17 orange
            new ColorSection(   73f,     77f,       93f,       fieryTop,   fieryMid,   fieryBot),   // 1:17-1:37 fiery
            new ColorSection(   93f,     97f,       101.8f,    purpleTop,  purpleMid,  purpleBot),  // 1:37-end purple
        };
    }

    void Update()
    {
        // Clouds rotation
        currentRotation = (currentRotation + rotationSpeed * Time.deltaTime) % 360f;
        RenderSettings.skybox.SetFloat("_CloudsRotation", currentRotation);

        // Moon halo pulse
        time += Time.deltaTime;
        RenderSettings.skybox.SetFloat("_MoonHalo", (float)Math.Sin(time));

        float songTime = time;

        // Find which transition we're in by scanning all sections
        Color top, mid, bot;

        // Default to first section
        top = sections[0].top;
        mid = sections[0].mid;
        bot = sections[0].bot;

        for (int i = 0; i < sections.Length - 1; i++)
        {
            float transStart = sections[i + 1].transitionStart; // 4s before next section
            float transEnd = sections[i + 1].fullStart;       // when next section is fully shown

            if (songTime >= transStart && songTime <= transEnd)
            {
                // Actively transitioning from section i to section i+1
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((songTime - transStart) / (transEnd - transStart)));
                top = Color.Lerp(sections[i].top, sections[i + 1].top, t);
                mid = Color.Lerp(sections[i].mid, sections[i + 1].mid, t);
                bot = Color.Lerp(sections[i].bot, sections[i + 1].bot, t);
                break;
            }
            else if (songTime >= transEnd && songTime < sections[i + 1].fullEnd)
            {
                // Fully in section i+1, no transition
                top = sections[i + 1].top;
                mid = sections[i + 1].mid;
                bot = sections[i + 1].bot;
                break;
            }
        }

        RenderSettings.skybox.SetColor("_TopColor", top);
        RenderSettings.skybox.SetColor("_MiddleColor", mid);
        RenderSettings.skybox.SetColor("_BottomColor", bot);
    }
}