
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
    AudioSource source;
    public const int FFTSIZE = 1024;
    public static float[] samples = new float[FFTSIZE];
    float[] prevSamples = new float[FFTSIZE];
    bool prevOnset = false;
    public static bool onsetDetected;
    public static float spectralFlux;

    // piano frequency bins: 200Hz-2000Hz at 44100Hz/1024 = ~43Hz per bin
    const int binMin = 5;
    const int binMax = 47;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        source.GetSpectrumData(samples, 0, FFTWindow.Hanning);
        spectralFlux = 0f;

        for (int i = binMin; i <= binMax; i++)
        {
            float diff = samples[i] - prevSamples[i];
            if (diff > 0)
                spectralFlux += diff;
            prevSamples[i] = samples[i];
        }

        spectralFlux *= 100f;
        float threshold = 1.5f; // tune this in Inspector if needed
        bool currentOnset = spectralFlux > threshold;
        onsetDetected = currentOnset && !prevOnset;
        prevOnset = currentOnset;
    }
}