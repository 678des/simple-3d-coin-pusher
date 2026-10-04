using UnityEngine;

public class ChaosModeManager : MonoBehaviour
{
    public static ChaosModeManager Instance { get; private set; }
    private bool isChaosMode = false;
    private float chaosTimer = 0f;

    private void Awake() { Instance = this; }

    public void ToggleChaosMode()
    {
        isChaosMode = !isChaosMode;
        if (isChaosMode)
        {
            GlobalPhysicsManager.Instance?.SetGravityMultiplier(0.5f);
            chaosTimer = 5.0f;
        }
        else
        {
            GlobalPhysicsManager.Instance?.ResetPhysics();
        }
    }

    private void Update()
    {
        if (isChaosMode)
        {
            ApplyRandomForce();
            chaosTimer -= Time.deltaTime;
            if (chaosTimer <= 0) ToggleChaosMode();
        }
    }

    public void ApplyRandomForce()
    {
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var c in coins)
        {
            if (c.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(new Vector3(Random.Range(-5, 5), Random.Range(2, 8), Random.Range(-5, 5)), ForceMode.Impulse);
            }
        }
    }
}