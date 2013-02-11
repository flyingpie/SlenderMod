using UnityEngine;
using System.Collections;

/// <summary>
/// Handles game changes while more notes are found
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Player player;

    public AudioSource[] sounds;
    public SpawnSettings[] spawnSettings;

    public int pagesFound = 0;

    public float minDistanceToPage = 2;
    public AudioSource notePickupSound;

    public float textDisplayTime = 2f;

    public bool debug = false;

    private float textTime;
    private GUIStyle style;

    public void IncreasePagesFound()
    {
        pagesFound++;

        // Start an additional soundtrack if any
        if (sounds[pagesFound - 1] != null)
        {
            sounds[pagesFound - 1].Play();
        }

        // Update slendy's spawn settings if any
        if (spawnSettings[pagesFound - 1] != null)
        {
            spawnSettings[pagesFound - 1].Apply(player);
        }

        notePickupSound.Play();

        textTime = textDisplayTime;
    }

    private void Awake()
    {
        instance = this;
    }

	private void Start()
    {
        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 24;
	}
	
	private void Update()
    {
        
	}

    private void OnGUI()
    {
        if (debug)
        {
            GUI.Label(new Rect(Screen.width - 220, 20, 200, 20), "Notes: " + pagesFound);
            if (GUI.Button(new Rect(Screen.width - 220, 60, 200, 20), "Next note"))
            {
                IncreasePagesFound();
            }
        }

        if (textTime > 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), string.Format("Page {0}/8", pagesFound), style);

            textTime -= Time.deltaTime;
        }
    }
}