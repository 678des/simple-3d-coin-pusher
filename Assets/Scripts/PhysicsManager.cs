using UnityEngine;
using System.Collections.Generic;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance { get; private set; }
    public float gravityMultiplier = 1.0f;
    private Dictionary<string, float> activeEvents = new Dictionary<string, float>();

    private void Awake() { Instance = this; }

    public void SetGravityMultiplier(float factor) { gravityMultiplier = factor; Physics.gravity = Physics.gravity * factor; }

    public void TriggerPhysicsEvent(string eventType, float duration)
    {
        activeEvents[eventType] = Time.time + duration;
        Debug.Log($"Physics Event Triggered: {eventType} for {duration}s");
    }

    private void Update()
    {
        foreach (var key in new List<string>(activeEvents.Keys))
        {
            if (Time.time > activeEvents[key]) activeEvents.Remove(key);
        }
    }
}