using UnityEngine;
using System.Collections;

public class RandomNoise : MonoBehaviour
{
    public NoiseEffect noise;
    public AudioSource noiseSound;

    public float updateInterval = 5f;
    public float noiseProbability = 1f;

    public float minNoiseDuration = 0.1f;
    public float maxNoiseDuration = 0.2f;

    public float minNoiseIntensity = 2f;
    public float maxNoiseIntensity = 4f;

    private float time = 0f;

    private float noiseTime;
    private bool isNoiseActive;

    private float defaultMinNoiseIntensity;
    private float defaultMaxNoiseIntensity;

	private void Start()
    {
        defaultMinNoiseIntensity = noise.grainIntensityMin;
        defaultMaxNoiseIntensity = noise.grainIntensityMax;
	}
	
	private void Update()
    {
        time += Time.deltaTime;

        if (time >= updateInterval)
        {
            if (Random.Range(0f, 1f) <= noiseProbability)
            {
                noiseTime = 0f;
                SetNoiseActive(true);
            }
            time = 0f;
        }

        if (isNoiseActive)
        {
            noiseTime += Time.deltaTime;

            if (noiseTime >= minNoiseDuration)
            {
                if (Random.Range(0f, 1f) <= 0.5f)
                {
                    SetNoiseActive(false);
                }
            }

            if (noiseTime >= maxNoiseDuration)
            {
                SetNoiseActive(false);
            }
        }
	}

    private void SetNoiseActive(bool active)
    {
        isNoiseActive = active;

        if (active)
        {
            noise.grainIntensityMin = minNoiseIntensity;
            noise.grainIntensityMax = maxNoiseIntensity;

            noiseSound.mute = false;
        }
        else
        {
            noise.grainIntensityMin = defaultMinNoiseIntensity;
            noise.grainIntensityMax = defaultMaxNoiseIntensity;

            noiseSound.mute = true;
        }
    }
}
