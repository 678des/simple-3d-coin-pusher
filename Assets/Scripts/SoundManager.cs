using UnityEngine;

/// <summary>
/// 音響アセットを一切必要とせず、ピュアC#で波形合成を行うプロシージャルレトロ音源マネージャー。
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;

    private AudioClip spawnStandard;
    private AudioClip spawnGold;
    private AudioClip spawnGiant;
    private AudioClip scoreStandard;
    private AudioClip scoreGold;
    private AudioClip scoreGiant;
    private AudioClip feverStart;
    private AudioClip lossTone;
    private AudioClip wallRaise;
    private AudioClip wallLower;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        SynthesizeRetroArcadeClips();
    }

    private void SynthesizeRetroArcadeClips()
    {
        spawnStandard = GenerateTone(523.25f, 0.05f, 0.15f); // High C
        spawnGold = GenerateTone(659.25f, 0.05f, 0.18f); // High E
        spawnGiant = GenerateTone(130.81f, 0.15f, 0.4f); // Low C Bass

        scoreStandard = GenerateArpeggio(new float[] { 523.25f, 659.25f, 783.99f }, 0.06f, 0.2f);
        scoreGold = GenerateArpeggio(new float[] { 587.33f, 739.99f, 880.00f, 1174.66f }, 0.05f, 0.25f);
        scoreGiant = GenerateArpeggio(new float[] { 261.63f, 329.63f, 392.00f, 523.25f }, 0.1f, 0.35f);

        feverStart = GenerateArpeggio(new float[] { 523f, 659f, 783f, 1046f, 1318f, 1568f }, 0.04f, 0.3f);
        lossTone = GenerateSlideTone(380f, 120f, 0.3f, 0.25f);

        wallRaise = GenerateTone(880.00f, 0.1f, 0.2f);
        wallLower = GenerateTone(440.00f, 0.1f, 0.2f);
    }

    private AudioClip GenerateTone(float frequency, float duration, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * volume * (1.0f - (t / duration));
        }

        AudioClip clip = AudioClip.Create("ProceduralTone_" + frequency, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private AudioClip GenerateSlideTone(float startFreq, float endFreq, float duration, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float ratio = t / duration;
            float currentFreq = Mathf.Lerp(startFreq, endFreq, ratio);
            samples[i] = Mathf.Sin(2 * Mathf.PI * currentFreq * t) * volume * (1.0f - ratio);
        }

        AudioClip clip = AudioClip.Create("ProceduralSlide", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private AudioClip GenerateArpeggio(float[] frequencies, float noteDuration, float volume)
    {
        int sampleRate = 44100;
        int totalSamples = (int)(sampleRate * noteDuration * frequencies.Length);
        float[] samples = new float[totalSamples];

        for (int step = 0; step < frequencies.Length; step++)
        {
            float freq = frequencies[step];
            int startOffset = (int)(sampleRate * noteDuration * step);
            int length = (int)(sampleRate * noteDuration);

            for (int i = 0; i < length; i++)
            {
                float t = (float)i / sampleRate;
                samples[startOffset + i] = Mathf.Sin(2 * Mathf.PI * freq * t) * volume * (1.0f - (t / noteDuration));
            }
        }

        AudioClip clip = AudioClip.Create("ProceduralArp", totalSamples, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public void PlaySpawnSound(CoinType type)
    {
        switch (type)
        {
            case CoinType.Standard: audioSource.PlayOneShot(spawnStandard); break;
            case CoinType.Gold: audioSource.PlayOneShot(spawnGold); break;
            case CoinType.Giant: audioSource.PlayOneShot(spawnGiant); break;
            case CoinType.Bonus: audioSource.PlayOneShot(spawnGold); break;
        }
    }

    public void PlayScoreSound(CoinType type)
    {
        switch (type)
        {
            case CoinType.Standard: audioSource.PlayOneShot(scoreStandard); break;
            case CoinType.Gold: audioSource.PlayOneShot(scoreGold); break;
            case CoinType.Giant: audioSource.PlayOneShot(scoreGiant); break;
            case CoinType.Bonus: audioSource.PlayOneShot(scoreGold); break;
        }
    }

    public void PlayLossSound()
    {
        audioSource.PlayOneShot(lossTone);
    }

    public void PlayFeverStartSound()
    {
        audioSource.PlayOneShot(feverStart);
    }

    public void PlayWallToggleSound(bool raised)
    {
        audioSource.PlayOneShot(raised ? wallRaise : wallLower);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("SoundManager_Procedural");
            go.AddComponent<SoundManager>();
        }
    }
}