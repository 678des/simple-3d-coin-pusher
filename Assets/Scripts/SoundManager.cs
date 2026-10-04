using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private void Awake() { Instance = this; audioSource = gameObject.AddComponent<AudioSource>(); }
    public void PlaySpawnSound(CoinType type) {}
    public void PlayScoreSound(CoinType type) {}
    public void PlayLossSound() {}
    public void PlayFeverStartSound() {}
    public void PlayWallToggleSound(bool raised) {}
    public void PlaySlotTickSound() {}
    public void PlaySlotWinSound() {}
    public void PlaySlotJackpotSound() {}
    public void PlayBumperHitSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("BumperImpact")); }
}