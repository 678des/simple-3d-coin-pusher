using UnityEngine;

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