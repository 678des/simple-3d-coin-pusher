// Responsibility: Handles bumper collision logic and triggers physics-based scoring events.
// Attachment: Attach this script to a GameObject representing a bumper in the scene.
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BumperSystem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) ActivateBumperBonus();
    }

    public void ActivateBumperBonus()
    {
        PhysicsManager.Instance.TriggerPhysicsEvent("BumperMultiplier", 5.0f);
    }
}