using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private void Awake() { Instance = this; audioSource = gameObject.AddComponent<AudioSource>(); }
    public void PlayFlipperSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("FlipperImpact")); }
    public void PlayFeverLoop() { audioSource.PlayOneShot(Resources.Load<AudioClip>("FeverMusic")); }
    public void PlayScoreSound(CoinType type) {}
}