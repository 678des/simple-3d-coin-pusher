// Responsibility: Manages the physics-based launching of coin objects upon collision.
// Attachment: Attach this script to a GameObject representing a flipper mechanism.
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinFlipper : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Coin")) {
            LaunchCoin(collision.rigidbody);
            SoundManager.Instance.PlayFlipperSound();
        }
    }
    public void LaunchCoin(Rigidbody coinRb) { coinRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse); }
}