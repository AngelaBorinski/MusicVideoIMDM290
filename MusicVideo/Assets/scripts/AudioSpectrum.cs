// Unity Audio Spectrum data analysis
// IMDM Course Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioSpectrum : MonoBehaviour
{
    AudioSource source;
    public static int FFTSIZE = 1024; // https://en.wikipedia.org/wiki/Fast_Fourier_transform
    public static float[] samples = new float[FFTSIZE];
    public static float audioAmp = 0f;
    public static float piano, synth, chime, transitionSound;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        // The source (time domain) transforms into samples in frequency domain 
        GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.Hanning);
        // Empty first, and pull down the value.
        audioAmp = 0f;
        for (int i = 0; i < FFTSIZE; i++)
        {
            audioAmp += samples[i];
        }
        piano = 0f;
        synth = 0f;
        chime = 0f;
        transitionSound = 0f;

        for (int i = 0; i < 400; i++)
        {
            synth += samples[i];
        }
        synth = synth / 400f;



        for(int i = 400; i < 800; i++)
        {
            piano += samples[i];
        }
        piano = piano/ 400f;

        for(int i = 800; i < FFTSIZE; i++)
        {
            chime += samples[i];
        }
        chime = chime / 224f;
    }
}
