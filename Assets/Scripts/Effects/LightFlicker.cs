using UnityEngine;
using System.Collections;

/// <summary>
/// Makes a light flicker randomly
/// </summary>
public class LightFlicker : MonoBehaviour
{
    public float flickerTime = 0.07f;

    private float deltaTime = 0;

	private void Update()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime >= flickerTime)
        {
            light.enabled = Random.Range(0, 2) == 0;

            deltaTime = 0;
        }
	}
}