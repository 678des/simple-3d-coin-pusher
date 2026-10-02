using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 画面のタップ位置を取得し、対応するX軸位置からコインを生成・落とすスクリプト。
/// フィーバーモードの連射、特殊コインの次弾予告表示機能付き。
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance { get; private set; }

    [Header("References")]
    [Tooltip("生成するコインのプレハブ")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("ゲームマネージャーへの参照（未設定時は自動取得）")]
    [SerializeField] private GameManager gameManager;

    [Header("Spawn Settings")]
    [Tooltip("コインを生成できる左右の最大幅 (X軸の±範囲)")]
    [SerializeField] private float spawnWidth = 2.0f;

    [Tooltip("連続生成を防ぐクールダウン時間（秒）")]
    [SerializeField] private float spawnCooldown = 0.25f;

    [Header("Special Coin Spawn Weights")]
    [Range(0f, 1f)] [SerializeField] private float goldChance = 0.15f;
    [Range(0f, 1f)] [SerializeField] private float giantChance = 0.05f;
    [Range(0f, 1f)] [SerializeField] private float bonusChance = 0.08f;

    private Camera mainCamera;
    private float cooldownTimer;
    private CoinType nextCoinType = CoinType.Standard;

    public CoinType NextCoinType => nextCoinType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        mainCamera = Camera.main;

        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }
    }

    private void Start()
    {
        RollNextCoinType();
    }

    private void Update()
    {
        // フィーバーモード時はクールダウンを大幅短縮して爽快な連打を可能にする
        float activeCooldown = spawnCooldown;
        if (gameManager != null && gameManager.IsFeverMode)
        {
            activeCooldown = spawnCooldown * 0.4f; // 60%短縮
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 長押し(ドラッグ)での連続投入も可能にしてプレイ感を向上
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (cooldownTimer <= 0f)
            {
                if (gameManager != null && gameManager.TryUseCoin())
                {
                    SpawnCoinAtMousePosition();
                    cooldownTimer = activeCooldown;
                }
            }
        }
    }

    private void RollNextCoinType()
    {
        float rand = Random.value;
        if (rand < giantChance)
        {
            nextCoinType = CoinType.Giant;
        }
        else if (rand < giantChance + bonusChance)
        {
            nextCoinType = CoinType.Bonus;
        }
        else if (rand < giantChance + bonusChance + goldChance)
        {
            nextCoinType = CoinType.Gold;
        }
        else
        {
            nextCoinType = CoinType.Standard;
        }
    }

    private void SpawnCoinAtMousePosition()
    {
        if (coinPrefab == null) return;

        Vector3 inputPosition = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            inputPosition = Input.GetTouch(0).position;
        }

        float normalizedX = Mathf.Clamp01(inputPosition.x / Screen.width);
        float spawnX = Mathf.Lerp(-spawnWidth, spawnWidth, normalizedX);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);

        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject spawnedObj = Instantiate(coinPrefab, spawnPosition, randomRotation);
        
        if (spawnedObj.TryGetComponent<Coin>(out var coin))
        {
            coin.Initialize(nextCoinType);

            // エフェクト & 音声演出トリガー
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySpawnSound(nextCoinType);
            }
            if (EffectsManager.Instance != null)
            {
                EffectsManager.Instance.PlaySpawnEffect(spawnPosition);
            }
        }

        RollNextCoinType();
    }

    /// <summary>
    /// フィーバー時のコインのシャワーをランダム座標から発生させる
    /// </summary>
    public void SpawnRandomCoinBonus()
    {
        if (coinPrefab == null) return;

        float spawnX = Random.Range(-spawnWidth, spawnWidth);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject spawnedObj = Instantiate(coinPrefab, spawnPosition, randomRotation);
        if (spawnedObj.TryGetComponent<Coin>(out var coin))
        {
            // フィーバー中は通常コインかゴールドを高確率で散りばめる
            CoinType bonusType = Random.value < 0.25f ? CoinType.Gold : CoinType.Standard;
            coin.Initialize(bonusType);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 leftPoint = new Vector3(-spawnWidth, transform.position.y, transform.position.z);
        Vector3 rightPoint = new Vector3(spawnWidth, transform.position.y, transform.position.z);
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.1f);
        Gizmos.DrawWireSphere(rightPoint, 0.1f);
    }
}