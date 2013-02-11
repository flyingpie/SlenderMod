using UnityEngine;
using System.Collections;

/// <summary>
/// Adds noise to the view based on the visibility of- and the distance to slendy
/// </summary>
public class EffectNoise : MonoBehaviour
{
    public float minNoise = 0.2f;
    public float maxNoise = 10f;

    public float closeDistance = 4f;
    public float farDistance = 60f;
    public float noiseUpMultiplier = 0.4f;
    public float noiseDownMultiplier = 2f;

    public float soundLowerLimit = 0.4f;

    public Player player;
    public SlenderMan slenderMan;
    public NoiseEffect noiseEffect;
    public AudioSource noiseSound;

    private float noiseValue = 0;

	private void Start()
    {
        noiseValue = minNoise;
	}
	
	private void Update()
    {
        if (player.canSeeSlendy)
        {
            float distance = Vector3.Distance(transform.position, slenderMan.transform.position);
            float distanceIndex = 1f - Mathf.Clamp((distance - closeDistance) / (farDistance - closeDistance), 0f, 1f);

            noiseValue += Time.deltaTime * distanceIndex * distanceIndex * noiseUpMultiplier;

            if (noiseValue > maxNoise)
            {
                noiseValue = maxNoise;
            }
        }
        else
        {
            noiseValue -= Time.deltaTime * noiseDownMultiplier;

            if (noiseValue < minNoise)
            {
                noiseValue = minNoise;
            }
        }

        noiseEffect.grainIntensityMin = noiseEffect.grainIntensityMax = noiseValue;
        noiseSound.volume = (noiseValue - soundLowerLimit) * (noiseValue / maxNoise);
	}
}
