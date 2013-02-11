using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public enum SplashStage
    {
        Idle,
        IsWaiting,
        IsStarting,
        IsShowing,
        IsEnding,
        HasEnded
    }

    public Material material;

    public float delay = 1f;
    public float fadeIn = 1f;
    public float fadeOut = 1f;
    public float duration = 4f;

    public bool startSplash;

    public SplashStage splashStage;

    public SplashScreen nextScreen;

    private Color color;
    private float time = 0f;

    public void StartSplash()
    {
        splashStage = SplashStage.IsWaiting;
    }

	private void Start()
    {
        color = material.color;
        color.a = 0;
        material.color = color;
	}
	
	private void Update()
    {
        if (startSplash)
        {
            startSplash = false;

            StartSplash();
        }

        switch (splashStage)
        {
            case SplashStage.Idle:

                break;
            case SplashStage.IsWaiting:
                time += Time.deltaTime;

                if (time >= delay)
                {
                    splashStage = SplashStage.IsStarting;
                    time = 0;
                }
                break;
            case SplashStage.IsStarting:
                color.a += fadeIn * Time.deltaTime;
            
                if (color.a >= 1f)
                {
                    color.a = 1f;

                    splashStage = SplashStage.IsShowing;
                }

                material.color = color;
                break;
            case SplashStage.IsShowing:
                time += Time.deltaTime;

                if (time >= duration)
                {
                    splashStage = SplashStage.IsEnding;
                }
                break;
            case SplashStage.IsEnding:
                color.a -= fadeOut * Time.deltaTime;

                if (color.a <= 0)
                {
                    color.a = 0f;

                    splashStage = SplashStage.HasEnded;
                }

                material.color = color;
                break;
            case SplashStage.HasEnded:
                if (nextScreen != null)
                {
                    nextScreen.StartSplash();
                }

                splashStage = SplashStage.Idle;
                break;
        }
	}
}
