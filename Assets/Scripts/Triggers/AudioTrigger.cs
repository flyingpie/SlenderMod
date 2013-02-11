using UnityEngine;
using System.Collections;

/// <summary>
/// Plays a clip exactly ones when a trigger is entered
/// </summary>
public class AudioTrigger : MonoBehaviour
{
    public AudioClip clip;

    private bool hasPlayed;

    private void OnTriggerEnter()
    {
        if (!hasPlayed)
        {
            audio.PlayOneShot(clip);

            hasPlayed = true;
        }
    }
}