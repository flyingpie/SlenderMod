using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
    public Credits lastCredits;

    public float restartButtonPosition = 0.5f;
    public float exitButtonOffset = 40f;

    public float delay;

    private float time;

    private bool isActive;
    private bool isLoading;

    public void ShowMenu()
    {
        isActive = true;
    }

	private void Start()
    {
	
	}
	
	private void Update()
    {
        if (!isActive && lastCredits.stage == Credits.CreditsStage.HasEnded)
        {
            isActive = true;
        }
	}

    private void OnGUI()
    {
        if (isActive)
        {
            if (time >= delay)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, restartButtonPosition * (float)Screen.height, 200, 30), "Restart game"))
                {
                    isLoading = true;

                    Application.LoadLevelAsync("Ambience");
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 100, restartButtonPosition * (float)Screen.height + exitButtonOffset, 200, 30), "Exit game"))
                {
                    Application.Quit();
                }
            }
            else
            {
                time += Time.deltaTime;
            }
        }

        if (isLoading)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, restartButtonPosition * (float)Screen.height - 40, 200, 30), "Loading...");
        }
    }
}
