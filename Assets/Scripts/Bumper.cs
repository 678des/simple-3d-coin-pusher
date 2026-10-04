using UnityEngine;

public class Bumper : MonoBehaviour
{
    private float bounceStrength = 10f;

    public void Initialize(float strength) {
        bounceStrength = strength;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Coin")) {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * bounceStrength;
                if (SoundManager.Instance != null) SoundManager.Instance.PlayBumperHitSound();
            }
        }
    }
}