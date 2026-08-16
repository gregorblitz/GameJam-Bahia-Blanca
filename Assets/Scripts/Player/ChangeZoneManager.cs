using UnityEngine;
using System.Collections.Generic;

public class ChangeZoneManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    public static ChangeZoneManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    [SerializeField] private List<Transform> zones = new();
    public int CurrentZoneIndex { get; private set; } = 0;
    public void Start()
    {
        if (zones.Count == 0)
        {
            Debug.LogWarning("No zones assigned to ChangeZoneManager.");
            return;
        }
        else
            transform.position = zones[CurrentZoneIndex].position;
    }
    public void ChangeZone(int zoneIndex)
    {
        zoneIndex = zoneIndex < 0 ? zones.Count - 1 : zoneIndex >= zones.Count ? 0 : zoneIndex;
        Debug.Log($"Adjusted zone index: {zoneIndex}");

        Transform targetZone = zones[zoneIndex];
        if (targetZone == null)
        {
            Debug.LogWarning($"Target zone at index {zoneIndex} is null.");
            return;
        }

        Debug.Log($"Changing zone to index: {zoneIndex}");
        // Move the player to the target zone's position
        if(player == null)
        {
            Debug.LogWarning("Player reference is not assigned in ChangeZoneManager.");
            return;
        }
        player.position = targetZone.position;
        CurrentZoneIndex = zoneIndex;
    }

}