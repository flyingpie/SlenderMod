using UnityEngine;
using System.Collections;

/// <summary>
/// Handles slendy's spawn system
/// </summary>
public class Player : MonoBehaviour
{
    public static Player instance { get; set; }

    public float fieldOfView = Mathf.PI / 2;

    public float minDistanceToFov = 0;
    public float maxDistanceToFov = 1;

    public float minDistanceToOrigin = 2;
    public float maxDistanceToOrigin = 10;

    public float updateInterval = 2f;
    public float teleportProbability = 0.3f;

    public float minStopChaseDistance = 10f;
    public float distanceToOriginShrink = 1f;

    public float minDistanceToOriginCap = 2f;
    public float maxDistanceToOriginCap = 3f;

    public float minTriggerEnabledDistance = 6f;

    public Vector3 raycastOffset;
    public Vector3[] rayCastOffsets;
    public Vector3[] rayCastOriginOffsets;

    public bool isChasing = false;

    public bool updateSpawnPoint;
    public bool drawSpawnArea;

    public SlenderMan slenderMan;
    public VortexEffect vortexEffect;

    private float target;
    private Vector3 transportTarget;
    private float time = 0;
    private int shrinkStepCount = 0;

    public bool canSeeSlendy;
    public float distanceToSlendy;

    public Transform playerTransform { get; private set; }
    public Camera playerCamera;

    private void Awake()
    {
        instance = this;

        playerTransform = transform;
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        isRendererEnabled = false;
    }

    private void Update()
    {
        if (GameController.instance.debug && drawSpawnArea)
        {
            DrawSpawnArea();
        }

        UpdateCanSeeSlendy();
        
        // Spawn point
        if (updateSpawnPoint && !canSeeSlendy)
        {
            time += Time.deltaTime;

            if (time >= updateInterval)
            {
                if (Random.value >= 1f - teleportProbability)
                {
                    if (isChasing)
                    {
                        DoChase();
                    }

                    isRendererEnabled = true;

                    //transportTarget = SearchTransportTarget(fieldOfView, minDistanceToFov, maxDistanceToFov, minDistanceToOrigin, maxDistanceToOrigin);

                    time = 0;

                    //slenderMan.slenderTransform.position = transportTarget;

                    int tries = 0;
                    do
                    {
                        transportTarget = SearchTransportTarget(fieldOfView, minDistanceToFov, maxDistanceToFov, minDistanceToOrigin, maxDistanceToOrigin);

                        slenderMan.slenderTransform.position = transportTarget;

                        tries++;
                    }
                    while(slenderMan.IsColliding() && tries < 20);

                    //Debug.Log(string.Format("Spawned in {0} tries", tries));

                    PooferTrigger.triggersEnabled = Vector3.Distance(slenderMan.slenderTransform.position, playerTransform.position) >= minTriggerEnabledDistance;
                }
                else
                {
                    isRendererEnabled = false;
                }

                time = 0;
            }

            // Look at player
            Quaternion delta = Quaternion.LookRotation(slenderMan.slenderTransform.position - playerTransform.position);
            delta.x = 0f;
            delta.z = 0f;

            slenderMan.slenderTransform.rotation = delta;
        }

        if (GameController.instance.debug)
        {
            DrawTransportTarget();

            DrawChaseArea();
        }

        slenderMan.IsColliding();
    }

    public enum Handedness
    {
        Left,
        Right
    }

    /// <summary>
    /// Searches for a target, taking walkable terrain into account
    /// </summary>
    /// <param name="fieldOfView"></param>
    /// <param name="minDistanceToFov"></param>
    /// <param name="maxDistanceToFov"></param>
    /// <param name="minDistanceToOrigin"></param>
    /// <param name="maxDistanceToOrigin"></param>
    /// <returns></returns>
    private Vector3 SearchTransportTarget(float fieldOfView, float minDistanceToFov, float maxDistanceToFov, float minDistanceToOrigin, float maxDistanceToOrigin)
    {
        Vector3 target = GetTransportTarget(fieldOfView, minDistanceToFov, maxDistanceToFov, minDistanceToOrigin, maxDistanceToOrigin);
        target = transform.rotation * target;

        Vector3 newPosition = slenderMan.slenderTransform.position;
        target = transform.position + target;
        newPosition.x = target.x;
        newPosition.z = target.z;
        
        RaycastHit hit;
        if (Physics.Raycast(target + Vector3.up * 100, Vector3.down, out hit, 8))
        {
            target.y = hit.point.y;
        }
        
        return target;
    }

    private Vector3 GetTransportTarget(float fieldOfView, float minDistanceToFov, float maxDistanceToFov, float minDistanceToOrigin, float maxDistanceToOrigin)
    {
        return GetTransportTarget(fieldOfView, Random.Range(minDistanceToFov, maxDistanceToFov), Random.Range(minDistanceToOrigin, maxDistanceToOrigin), (Handedness)Random.Range(0, 2));
    }

    /// <summary>
    /// Returns a point within a radius around point 0.0
    /// </summary>
    /// <param name="fieldOfView">The field of view of the player, points will not be placed inside this area.</param>
    /// <param name="distanceToFov">The distance between the resulting point and the field of view, the closer, the easier to spot.</param>
    /// <param name="distanceToOrigin">The distance between the resulting point and the center, the closer, the easier to die.</param>
    /// <param name="handedness">Which side of the player to place the point at.</param>
    /// <returns>A vector3 which will satisfy the specified parameters.</returns>
    private Vector3 GetTransportTarget(float fieldOfView, float distanceToFov, float distanceToOrigin, Handedness handedness)
    {
        // We need to divide the fov in 2, because the calculation is one-side based
        float halfFieldOfView = fieldOfView / 2;

        // Get the angle between the players' z-axis and the target point
        float target = halfFieldOfView + distanceToFov * (Mathf.PI - halfFieldOfView);

        // Flip the target to the other side if left-handed
        if (handedness == Handedness.Left) target *= -1;

        // Get the vector3 and multiply with the desired distance to the origin
        return new Vector3(Mathf.Sin(target), 0, Mathf.Cos(target)) * distanceToOrigin;
    }

    private void DrawSpawnArea()
    {
        // Cap settings
        if (minDistanceToFov < 0) minDistanceToFov = 0;
        if (minDistanceToFov > maxDistanceToFov) minDistanceToFov = maxDistanceToFov;

        if (maxDistanceToFov < minDistanceToFov) maxDistanceToFov = minDistanceToFov;
        if (maxDistanceToFov > 1) maxDistanceToFov = 1;

        if (minDistanceToOrigin < 0) minDistanceToOrigin = 0;
        if (minDistanceToOrigin > maxDistanceToOrigin) minDistanceToOrigin = maxDistanceToOrigin;

        if (maxDistanceToOrigin < minDistanceToOrigin) maxDistanceToOrigin = minDistanceToOrigin;

        // Field of view
        float halfFieldOfView = fieldOfView / 2;

        Vector3 left = new Vector3(Mathf.Sin(-halfFieldOfView), 0, Mathf.Cos(-halfFieldOfView));
        Vector3 right = new Vector3(Mathf.Sin(halfFieldOfView), 0, Mathf.Cos(halfFieldOfView));

        left = transform.rotation * left;
        right = transform.rotation * right;

        Debug.DrawLine(transform.position + left * minDistanceToOrigin, transform.position + left * maxDistanceToOrigin);
        Debug.DrawLine(transform.position + right * minDistanceToOrigin, transform.position + right * maxDistanceToOrigin);

        // Minimum distance to field of view
        float minTarget = halfFieldOfView + (minDistanceToFov * (Mathf.PI - halfFieldOfView));
        Vector3 minFovLeft = transform.rotation * new Vector3(Mathf.Sin(-minTarget), 0, Mathf.Cos(-minTarget));
        Vector3 minFovRight = transform.rotation * new Vector3(Mathf.Sin(minTarget), 0, Mathf.Cos(minTarget));

        float minColorValue = (minTarget - halfFieldOfView) / (Mathf.PI - halfFieldOfView);
        Color minColor = new Color(1 - minColorValue, minColorValue, 0);

        Debug.DrawLine(transform.position + minFovLeft * minDistanceToOrigin, transform.position + minFovLeft * maxDistanceToOrigin, minColor);
        Debug.DrawLine(transform.position + minFovRight * minDistanceToOrigin, transform.position + minFovRight * maxDistanceToOrigin, minColor);

        // Maximum distance to fov
        float maxTarget = halfFieldOfView + ((maxDistanceToFov) * (Mathf.PI - halfFieldOfView));
        Vector3 maxFovLeft = transform.rotation * new Vector3(Mathf.Sin(-maxTarget), 0, Mathf.Cos(-maxTarget));
        Vector3 maxFovRight = transform.rotation * new Vector3(Mathf.Sin(maxTarget), 0, Mathf.Cos(maxTarget));

        float maxColorValue = (maxTarget - halfFieldOfView) / (Mathf.PI - halfFieldOfView);
        Color maxColor = new Color(1 - maxColorValue, maxColorValue, 0);

        Debug.DrawLine(transform.position + maxFovLeft * minDistanceToOrigin, transform.position + maxFovLeft * maxDistanceToOrigin, maxColor);
        Debug.DrawLine(transform.position + maxFovRight * minDistanceToOrigin, transform.position + maxFovRight * maxDistanceToOrigin, maxColor);

        // Area
        for (float i = halfFieldOfView; i < Mathf.PI * 2 - halfFieldOfView; i += 0.1f)
        {
            Vector3 from = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i));
            Vector3 to = new Vector3(Mathf.Sin(i + 0.05f), 0, Mathf.Cos(i + 0.05f));

            from = transform.rotation * from;
            to = transform.rotation * to;

            Debug.DrawLine(transform.position + from * maxDistanceToOrigin, transform.position + to * maxDistanceToOrigin);
            Debug.DrawLine(transform.position + from * minDistanceToOrigin, transform.position + to * minDistanceToOrigin);
        }
    }

    private void DrawTransportTarget()
    {
        for (float i = 0; i < Mathf.PI * 2; i += 0.02f)
        {
            Vector3 from = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)) * 0.5f;
            Vector3 to = new Vector3(Mathf.Sin(i + 0.05f), 0, Mathf.Cos(i + 0.05f)) * 0.5f;

            Debug.DrawLine(transform.position + transportTarget + from, transform.position + transportTarget + to);
        }
    }

    private void DrawChaseArea()
    {
        Color chaseAreaColor = isChasing ? Color.red : Color.green;
        for (float i = 0; i < Mathf.PI * 2; i += 0.02f)
        {
            Vector3 from = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)) * minStopChaseDistance;
            Vector3 to = new Vector3(Mathf.Sin(i + 0.05f), 0, Mathf.Cos(i + 0.05f)) * minStopChaseDistance;

            Debug.DrawLine(transform.position + from, transform.position + to, chaseAreaColor);
        }
    }

    public bool isRendererEnabled
    {
        get
        {
            return slenderMan.isRendererEnabled;
        }
        set
        {
            slenderMan.SetRendererEnabled(value);
        }
    }

    private void UpdateCanSeeSlendy()
    {
        if (!isRendererEnabled)
        {
            canSeeSlendy = false;
            return;
        }

        Vector3 playerForward = playerTransform.forward;
        Vector3 playerToSlendy = slenderMan.slenderTransform.position - playerTransform.position;
        float angle = Vector3.Angle(playerForward, playerToSlendy) / (360 / Mathf.PI);
        
        // Spawn point
        canSeeSlendy = angle < fieldOfView / 2;

        if (canSeeSlendy) canSeeSlendy = IsInLineOfSight();
        
        distanceToSlendy = Vector3.Distance(playerTransform.position, slenderMan.slenderTransform.position);

        if (canSeeSlendy && !isChasing)
        {
            isChasing = true;
        }
    }

    public bool IsInLineOfSight()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 to = slenderMan.slenderTransform.position;
        Quaternion playerRotation = playerTransform.rotation;
        Quaternion slendyRotation = slenderMan.slenderTransform.rotation;

        Vector3 originOffset;
        Vector3 castOffset;
        RaycastHit hit;
        for (int j = 0; j < rayCastOriginOffsets.Length; j++)
        {
            originOffset = playerRotation * rayCastOriginOffsets[j];
            for (int i = 0; i < rayCastOffsets.Length; i++)
            {
                castOffset = slendyRotation * rayCastOffsets[i];
                if (Physics.Raycast(cameraPosition + originOffset, ((to + castOffset + raycastOffset) - cameraPosition).normalized, out hit))
                {

                    if (hit.transform.GetComponent<SlenderMan>())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void DoChase()
    {
        if (Vector3.Distance(slenderMan.slenderTransform.position, playerTransform.position) > minStopChaseDistance)
        {
            minDistanceToOrigin += distanceToOriginShrink * shrinkStepCount;
            maxDistanceToOrigin += distanceToOriginShrink * shrinkStepCount;
            shrinkStepCount = 0;
            isChasing = false;
            return;
        }

        if (minDistanceToOrigin - distanceToOriginShrink >= minDistanceToOriginCap
            && maxDistanceToOrigin - distanceToOriginShrink >= maxDistanceToOriginCap)
        {
            minDistanceToOrigin -= distanceToOriginShrink;
            maxDistanceToOrigin -= distanceToOriginShrink;

            shrinkStepCount++;
        }
    }
}