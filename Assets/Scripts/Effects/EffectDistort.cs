using UnityEngine;
using System.Collections;

/// <summary>
/// Distorts the view using a vortex effect based on the visibility of- and the distance to slendy
/// </summary>
public class EffectDistort : MonoBehaviour
{
    public float maxDistort = 10f;

    public float closeDistance = 4f;
    public float farDistance = 60f;

    public float distortUpMultiplier = 0.4f;
    public float distortDownMultiplier = 2f;

    public Player player;
    public SlenderMan slenderMan;
    public VortexEffect vortexEffect;

    private float seeingSlandy = 0;

    private void Update()
    {
        if (player.canSeeSlendy)
        {
            float distance = Vector3.Distance(transform.position, slenderMan.transform.position);
            float distanceIndex = 1f - Mathf.Clamp((distance - closeDistance) / (farDistance - closeDistance), 0f, 1f);

            seeingSlandy += Time.deltaTime * distanceIndex * distanceIndex * distortUpMultiplier;

            if (seeingSlandy > maxDistort)
            {
                seeingSlandy = maxDistort;
            }
        }
        else
        {
            seeingSlandy -= Time.deltaTime * distortDownMultiplier;

            if (seeingSlandy < 0)
            {
                seeingSlandy = 0;
            }
        }

        vortexEffect.angle = seeingSlandy;
    }
}