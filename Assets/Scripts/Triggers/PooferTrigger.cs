using UnityEngine;
using System.Collections;

public class PooferTrigger : MonoBehaviour
{
    public static bool triggersEnabled = true;

    private static bool poofHasOccured;

    public AudioClip sound;
    public GameObject slendy;

    public Material[] materials;
    public float alphaDecrease = 0.2f;
    public float poofDelay = 0.3f;
    public float probabilityOfPoofing = 1f;

    public ParticleSystem particles;

    private bool isPooferEnabled;
    private bool hasPoofed;
    private float alpha = 1f;

    private void Start()
    {
        slendy.SetActiveRecursively(false);
    }

    private void OnTriggerEnter()
    {
        // If no poofer has triggered, try to trigger this one
        if (triggersEnabled && !poofHasOccured && !hasPoofed)
        {
            // If the player is not looking towards the poofer, enable him based on the probability
            float angle = GetAngleToPoofer();
            
            if (angle > Player.instance.fieldOfView / 2)
            {
                if (Random.Range(0f, 1f) <= probabilityOfPoofing)
                {
                    slendy.SetActiveRecursively(true);
                    alpha = 1f;
                    SetAlpha(1f);
                    isPooferEnabled = true;
                }
            }
        }
    }

    private void OnTriggerExit()
    {
        if (isPooferEnabled)
        {
            isPooferEnabled = false;
            slendy.SetActiveRecursively(false);
        }
    }

    private void Update()
    {
        if (isPooferEnabled && !triggersEnabled)
        {
            isPooferEnabled = false;
            slendy.SetActiveRecursively(false);
        }

        if (isPooferEnabled)
        {
            Player player = Player.instance;

            float angle = GetAngleToPoofer();

            bool canSeeSlendy = angle < player.fieldOfView / 2;

            if (canSeeSlendy)
            {                
                audio.PlayOneShot(sound);

                particles.Play();

                isPooferEnabled = false;
                hasPoofed = true;

                alpha = 1f;
                SetAlpha(1f);

                poofHasOccured = true;
            }
        }

        if (hasPoofed && alpha > 0)
        {
            if (poofDelay > 0)
            {
                poofDelay -= Time.deltaTime;
                if (poofDelay < 0) poofDelay = 0;
                else return;
            }

            alpha -= alphaDecrease * Time.deltaTime;

            SetAlpha(alpha);

            if (alpha < 0) alpha = 0;
        }
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_Cutoff", 1 - alpha);
        }
    }

    private float GetAngleToPoofer()
    {
        Vector3 playerForward = Player.instance.playerTransform.forward;
        Vector3 playerToSlendy = slendy.transform.position - Player.instance.playerTransform.position;
        float angle = Vector3.Angle(playerForward, playerToSlendy) / (360 / Mathf.PI);

        return angle;
    }
}
