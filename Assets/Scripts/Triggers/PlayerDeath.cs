using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public float deathMarker = 10f;

    public float closeDistance = 4f;
    public float farDistance = 60f;
    public float upMultiplier = 0.4f;
    public float downMultiplier = 2f;
    public float distancePow = 2f;

    public Player player;
    public SlenderMan slenderMan;

    private float deathValue = 0;
    private bool hasDied;

	private void Start()
    {
        player = GetComponent<Player>();
	}
	
	private void Update()
    {
        if (player.canSeeSlendy)
        {
            float distance = Vector3.Distance(transform.position, slenderMan.slenderTransform.position);
            float distanceIndex = 1f - Mathf.Clamp((distance - closeDistance) / (farDistance - closeDistance), 0f, 1f);

            deathValue += Time.deltaTime * Mathf.Pow(distanceIndex, distancePow) * upMultiplier;

            if (deathValue > deathMarker)
            {
                deathValue = deathMarker;
            }
        }
        else
        {
            deathValue -= Time.deltaTime * downMultiplier;

            if (deathValue < 0)
            {
                deathValue = 0;
            }
        }

        if (deathValue >= deathMarker)
        {
            hasDied = true;

            DeathController.showCredits = GameController.instance.pagesFound == 8;

            Application.LoadLevelAsync("Death");
        }
	}

    private void OnGUI()
    {
        //GUI.Label(new Rect(20, 100, 200, 20), string.Format("Death factor: {0}", deathValue));

        if (hasDied)
        {
            //GUI.Label(new Rect(20, 140, 200, 20), "You died!!");
        }
    }
}
