using UnityEngine;
using System.Collections;

/// <summary>
/// Plays the piano slamming sound when the player turns to slendy from a certain distance
/// </summary>
public class PianoSoundTrigger : MonoBehaviour
{
    public AudioClip clip;
    public Player player;

    public float minDistanceToSlendy = 10f;
    public float minBurstInterval = 2f;

    private bool lastCanSeeSlendy;
    private float lastBurstTime = 0;

	private void Update()
    {
        if (player.canSeeSlendy && !lastCanSeeSlendy)
        {
            lastCanSeeSlendy = true;

            if (player.distanceToSlendy <= minDistanceToSlendy && lastBurstTime + minBurstInterval < Time.time)
            {
                audio.PlayOneShot(clip);
            }
        }

        if (lastCanSeeSlendy)
        {
            lastBurstTime = Time.time;
        }

        if (!player.canSeeSlendy)
        {
            lastCanSeeSlendy = false;
        }
	}
}