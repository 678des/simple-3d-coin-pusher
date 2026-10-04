using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private void Awake() { Instance = this; audioSource = gameObject.AddComponent<AudioSource>(); }
    public void PlayFlipperSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("FlipperImpact")); }
    public void PlayFeverLoop() { audioSource.PlayOneShot(Resources.Load<AudioClip>("FeverMusic")); }
    public void PlayScoreSound(CoinType type) {}
    public void PlayBumperHitSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("BumperHit")); }
    public void PlayBumperMissSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("BumperMiss")); }
    public void PlayWallToggleSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("WallToggle")); }
    public void PlaySlotTickSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("SlotTick")); }
    public void PlaySlotWinSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("SlotWin")); }
    public void PlaySlotJackpotSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("SlotJackpot")); }
    public void PlayLossSound() { audioSource.PlayOneShot(Resources.Load<AudioClip>("Miss")); }
}