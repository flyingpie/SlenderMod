using UnityEngine;
using System.Collections;

public class NoiseLine : MonoBehaviour
{
    public NoiseEffect noise;
    public AudioSource[] noiseSources;

    public float grainIntensityMin = 4f;
    public float grainIntensityMax = 5f;

    public float[] line;

    private int segment;
    private float time;

    private float minGrainIntensity;
    private float maxGrainIntensity;

    private float[] originalVolume;

    private bool hasNoise;

	private void Start()
    {
        segment = 0;

        minGrainIntensity = noise.grainIntensityMin;
        maxGrainIntensity = noise.grainIntensityMax;

        originalVolume = new float[noiseSources.Length];
        for (int i = 0; i < noiseSources.Length; i++)
        {
            originalVolume[i] = noiseSources[i].volume;
            noiseSources[i].volume = 0;
        }
	}
	
	private void Update()
    {
        if (segment < line.Length)
        {
            time += Time.deltaTime;

            if (time >= line[segment])
            {
                time = 0;
                segment++;
            }

            hasNoise = segment % 2 == 0;

            noise.grainIntensityMin = (hasNoise) ? grainIntensityMin : minGrainIntensity;
            noise.grainIntensityMax = (hasNoise) ? grainIntensityMax : maxGrainIntensity;

            for (int i = 0; i < noiseSources.Length; i++)
            {
                noiseSources[i].volume = (hasNoise) ? originalVolume[i] : 0f;
            }
        }
	}
}
