using UnityEngine;

/// <summary>
/// 外部プレハブを使用せずに手続き型コードでパーティクルシステムを即時構成・再生するエフェクト管理クラス。
/// </summary>
public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySpawnEffect(Vector3 position)
    {
        GenerateParticleBurst(position, new Color(1f, 0.9f, 0.5f, 0.6f), 12, 0.25f);
    }

    public void PlayScoreEffect(Vector3 position, CoinType type)
    {
        Color effectColor = Color.yellow;
        int count = 15;
        float size = 0.2f;

        switch (type)
        {
            case CoinType.Standard:
                effectColor = new Color(0.95f, 0.8f, 0.1f);
                count = 15;
                break;
            case CoinType.Gold:
                effectColor = new Color(1.0f, 0.5f, 0.0f);
                count = 30;
                size = 0.25f;
                break;
            case CoinType.Giant:
                effectColor = new Color(0.9f, 0.2f, 0.2f);
                count = 60;
                size = 0.45f;
                break;
            case CoinType.Bonus:
                effectColor = new Color(0.1f, 0.8f, 1.0f);
                count = 30;
                size = 0.3f;
                break;
        }

        GenerateParticleBurst(position, effectColor, count, size);
    }

    public void PlayLossEffect(Vector3 position)
    {
        GenerateParticleBurst(position, new Color(0.4f, 0.4f, 0.4f, 0.8f), 8, 0.15f);
    }

    private void GenerateParticleBurst(Vector3 position, Color color, int count, float size)
    {
        GameObject go = new GameObject("ProceduralBurstParticle");
        go.transform.position = position;

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startColor = color;
        main.startSize = new ParticleSystem.MinMaxCurve(size * 0.5f, size * 1.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 4.0f);
        main.startLifetime = 0.6f;
        main.gravityModifier = 0.8f;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = ps.emission;
        emission.rateOverTime = 0;

        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0f, (short)count);
        emission.SetBursts(new ParticleSystem.Burst[] { burst });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.15f;

        ps.Play();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("EffectsManager_Procedural");
            go.AddComponent<EffectsManager>();
        }
    }
}