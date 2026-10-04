using UnityEngine;

public class CoinPhysics : MonoBehaviour
{
    private Rigidbody rb;

    private void Start() { rb = GetComponent<Rigidbody>(); }

    public void ApplyModifier(float forceMultiplier) { rb.velocity *= forceMultiplier; }

    public void OnPhysicsEventStarted(string type)
    {
        if (type == "CoinRain") rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }
}