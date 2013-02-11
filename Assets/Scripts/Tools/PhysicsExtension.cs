using UnityEngine;
using System.Collections;

public class Physics : UnityEngine.Physics
{
    /// <summary>
    /// Raycasts objects on a certain layer
    /// </summary>
    /// <param name="origin">Where to send the ray from</param>
    /// <param name="direction">Where to send the ray to</param>
    /// <param name="hit">Hit info output</param>
    /// <param name="layer">Which layer to raycast on</param>
    /// <returns>Whether an object was hit</returns>
    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, int layer)
    {
        RaycastHit[] hits = Physics.RaycastAll(origin, direction);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == layer)
            {
                hit = hits[i];
                return true;
            }
        }

        hit = new RaycastHit();
        return false;
    }
}