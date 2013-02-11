using UnityEngine;
using System.Collections;

public class WalkSound : MonoBehaviour
{
    public AudioSource walkSound;
    public AudioSource runSound;

    public float minWalkVelocity = 0.2f;
    public float maxWalkVelocity = 4f;

    public float walkRunTreshold = 0.4f;
    public float walkMultiplier = 0.6f;
    public float runMultiplier = 0.6f;

    private CharacterController characterController;

	private void Start()
    {
        characterController = GetComponent<CharacterController>();
	}
	
	private void Update()
    {
        float speedIndex = Mathf.Clamp((characterController.velocity.magnitude - minWalkVelocity) / (maxWalkVelocity - minWalkVelocity), 0f, 100f);

        if (speedIndex <= walkRunTreshold)
        {
            float walk = speedIndex * (1f / walkRunTreshold);

            walkSound.volume = walk * walkMultiplier;
            runSound.volume = 0f;
        }
        else
        {
            walkSound.volume = 0f;
            runSound.volume = speedIndex * runMultiplier;
        }
	}
}
