using UnityEngine;
using System.Collections;

public class BreathingSound : MonoBehaviour
{
    public AudioSource breathingSound;

    public CharacterMotor characterMotor;

    public float startBreathingAt = 12f;
    public float maxVolume = 1f;
    public float volumeDelta = 1f;
    public float volume = 0f;

	private void Start()
    {
	    
	}
	
	private void Update()
    {
        if (characterMotor.sprintTime <= startBreathingAt && volume < maxVolume)
        {
            volume += Time.deltaTime * volumeDelta;

            if (volume > maxVolume)
            {
                volume = maxVolume;
            }
        }
        else if (volume > 0)
        {
            volume -= Time.deltaTime * volumeDelta;
        }

        breathingSound.volume = volume;
	}
}
