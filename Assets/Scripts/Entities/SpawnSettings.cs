using System;
using UnityEngine;

/// <summary>
/// Represents a set of settings to use when slendy is spawning
/// </summary>
[Serializable]
public class SpawnSettings
{
    public float minDistanceToFov = 0;
    public float maxDistanceToFov = 1;

    public float minDistanceToOrigin = 2;
    public float maxDistanceToOrigin = 10;

    public float updateInterval = 2f;
    public float teleportProbability = 0.3f;

    public float minStopChaseDistance = 10f;
    public float distanceToOriginShrink = 1f;

    public void Apply(Player player)
    {
        player.minDistanceToFov = minDistanceToFov;
        player.maxDistanceToFov = maxDistanceToFov;

        player.minDistanceToOrigin = minDistanceToOrigin;
        player.maxDistanceToOrigin = maxDistanceToOrigin;

        player.updateInterval = updateInterval;
        player.teleportProbability = teleportProbability;

        player.minStopChaseDistance = minStopChaseDistance;
        player.distanceToOriginShrink = distanceToOriginShrink;

        player.isChasing = false;

        //Debug.Log("Applied new settings");
    }
}