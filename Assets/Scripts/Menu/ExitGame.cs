using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.Escape;

	private void Update()
    {
        if (Input.GetKeyDown(exitKey))
        {
            Application.Quit();
        }
	}
}
