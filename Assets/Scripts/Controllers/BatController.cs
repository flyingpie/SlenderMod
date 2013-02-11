using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour
{
    public static BatController instance { get; set; }

    public ParticleSystem particles;
    public Vector3 particleOffset;

    public AudioClip sound;

    public int initialTimespan = 1; //15;
    public int timespan = 60;
    public float probabilityOfTrigger = 1f;

    private bool isInitial = true;
    private float lastTime = 0;

    public bool CanTriggerEffect
    {
        get
        {
            bool canTrigger = (isInitial) ? lastTime + initialTimespan < Time.time : lastTime + timespan < Time.time;

            if (isInitial && canTrigger) isInitial = false;
            
            return canTrigger;
        }
    }

    public bool ShouldDoEffect
    {
        get
        {
            if (CanTriggerEffect)
            {
                return Random.Range(0f, 1f) <= probabilityOfTrigger;
            }

            return false;
        }
    }

    public void PositionParticles(Vector3 position)
    {
        particles.transform.position = position + particleOffset;
    }

    public void PlaySound()
    {
        audio.PlayOneShot(sound);
    }

    public void UpdateLastTime()
    {
        lastTime = Time.time;
    }

    public void DoBatTrigger(Vector3 position)
    {
        if (ShouldDoEffect)
        {
            PositionParticles(position);
            particles.Play();
            PlaySound();
            UpdateLastTime();
        }
    }

    private void Awake()
    {
        instance = this;
    }

	private void Start()
    {
        UpdateLastTime();
	}
}
