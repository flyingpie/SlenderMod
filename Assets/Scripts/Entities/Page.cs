using UnityEngine;
using System.Collections;

/// <summary>
/// Removes a note when the player clicks on it within a certain distance and notifies the game controller
/// </summary>
public class Page : MonoBehaviour
{
    public GameObject page;

    private bool isFound;
    private Camera playerCamera;

	private void Start()
    {
        playerCamera = GameController.instance.player.GetComponentInChildren<Camera>();
	}
	
	private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector3.Distance(transform.position, playerCamera.transform.position) < GameController.instance.minDistanceToPage)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
                {
                    if (hit.transform.GetComponent<Page>() == this)
                    {
                        gameObject.active = false;

                        GameController.instance.IncreasePagesFound();
                    }
                }
            }
        }
	}
}
