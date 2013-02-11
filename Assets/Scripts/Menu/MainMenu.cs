using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public SplashScreen lastSplash;
    public GameObject logo;

    public float startButtonPosition;

    private bool isActive = false;
    private bool isLoading = false;

	private void Start()
    {
	
	}
	
	private void Update()
    {
        if (!isActive)
        {
            if (lastSplash.splashStage == SplashScreen.SplashStage.HasEnded)
            {
                isActive = true;

                logo.active = true;
            }
        }
	}

    private void OnGUI()
    {
        if (isActive)
        {
            if (!isLoading)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, startButtonPosition * (float)Screen.height, 200, 30), "Start game"))
                {
                    isLoading = true;

                    Application.LoadLevelAsync("Ambience");
                }
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, startButtonPosition * (float)Screen.height, 200, 30), "Loading...");
            }
        }
    }
}
