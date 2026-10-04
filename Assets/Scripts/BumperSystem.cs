using UnityEngine;

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