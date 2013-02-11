using UnityEngine;
using System.Collections;

public class FadeToCredits : MonoBehaviour
{
    public Material blackPlane;
    public NoiseEffect noise;
    public AudioSource[] audioSources;
    public Credits firstCredits;
    public DeathMenu deathMenu;

    public float delay = 4f;
    public float speed = 1f;
    public float speedNoise = 1f;
    public float speedSound = 1f;
    public float delayCreditsTime = 2f;

    private float time = 0f;
    private Color color;
    private float timeAfterDelay = 0f;

    private bool isActive = true;

    private void Start()
    {
        color = blackPlane.color;
        color.a = 0f;

        blackPlane.color = color;
    }

	private void Update()
    {
        if (isActive)
        {
            if (time >= delay)
            {
                if (color.a < 1f)
                {
                    color.a += speed * Time.deltaTime;
                    blackPlane.color = color;
                }

                if (noise.grainIntensityMin > 0f)
                {
                    noise.grainIntensityMin -= speedNoise * Time.deltaTime;
                }

                if (noise.grainIntensityMax > 0f)
                {
                    noise.grainIntensityMax -= speedNoise * Time.deltaTime;
                }

                for (int i = 0; i < audioSources.Length; i++)
                {
                    if (audioSources[i].volume > 0f)
                    {
                        audioSources[i].volume -= speedSound * Time.deltaTime;
                    }
                }

                timeAfterDelay += Time.deltaTime;

                if (timeAfterDelay >= delayCreditsTime)
                {
                    if (DeathController.showCredits)
                    {
                        firstCredits.StartCredits();
                    }
                    else
                    {
                        deathMenu.ShowMenu();
                    }

                    isActive = false;
                }
            }
            else
            {
                time += Time.deltaTime;
            }
        }
	}
}
