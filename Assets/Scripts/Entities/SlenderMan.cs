using UnityEngine;
using System.Collections;

/// <summary>
/// Should do spawn system, but i'm too lazy to do that now
/// </summary>
public class SlenderMan : MonoBehaviour
{
    public bool doTransport;
    public Player player;
    public GameObject slendy;

    public bool isRendererEnabled { get; set; }

    public bool isColliding { get; set; }

    public Transform slenderTransform { get; set; }

    public Vector3[] collisionOffsets;
    public float minCollisionDistance;
    public float maxCollisionDistance;
    public float collisionHeight = 1f;

    public void SetRendererEnabled(bool enabled)
    {
        slendy.SetActiveRecursively(enabled);
        isRendererEnabled = enabled;
    }

    public bool IsColliding()
    {
        //Debug.Log("IsColliding " + Time.time);
        Vector3 heightOffset = Vector3.up * collisionHeight;
        Vector3 position = slenderTransform.position + heightOffset;
        Quaternion rotation = slenderTransform.rotation;
        Vector3 from;
        Vector3 to;
        RaycastHit hit;
        //Color color;
        //bool isColliding = false;
        for (int i = 0; i < collisionOffsets.Length; i++)
        {
            from = (rotation * collisionOffsets[i]) * minCollisionDistance + heightOffset;
            to = (rotation * collisionOffsets[i]) + heightOffset;
            //color = Color.green;
            if (Physics.Raycast(position + from, to, maxCollisionDistance))
            {
                //color = Color.red;
                return true;
                //isColliding = true;
            }

            //Debug.DrawLine(position + from, position + to * maxCollisionDistance, color);
        }

        return false;
        //return isColliding;
    }

    private void Start()
    {
        slenderTransform = transform;
    }

    private void Update()
    {
        if (GameController.instance.debug)
        {
            for (float j = 1; j < 4; j++)
            {
                for (float i = 0; i < Mathf.PI * 2; i += 0.02f)
                {
                    Vector3 from = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)) * (0.5f + j);
                    Vector3 to = new Vector3(Mathf.Sin(i + 0.05f), 0, Mathf.Cos(i + 0.05f)) * (0.5f + j);

                    Debug.DrawLine(transform.position + from, transform.position + to, Color.white);
                }
            }

            for (float i = 0; i < Mathf.PI * 2; i += 0.02f)
            {
                Vector3 from = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)) * (0.5f + 4);
                Vector3 to = new Vector3(Mathf.Sin(i + 0.05f), 0, Mathf.Cos(i + 0.05f)) * (0.5f + 4);

                Debug.DrawLine(transform.position + from, transform.position + to, (Player.instance.isRendererEnabled) ? Color.red : Color.green);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        isColliding = false;
    }
}