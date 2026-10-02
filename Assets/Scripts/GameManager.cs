using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の進行管理、スコア、所持コイン数の管理、およびゲームオーバー判定を行うマネージャークラス。
/// フィーバー、サイドガードパワーアップ、UI更新不具合修正等の要素を含みます。
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("コイン設定")]
    [Tooltip("ゲーム開始時の所持コイン数")]
    [SerializeField] private int initialCoins = 50;

    private UIManager uiManager;
    private int currentScore;
    private int remainingCoins;
    private int activeCoinsOnBoard;
    private bool isGameOver;

    // 特殊ゲームシステム変数
    private float feverMeter = 0f;
    private bool isFeverMode = false;
    private float feverTimer = 0f;
    private const float FeverDuration = 10.0f;

    private float sideWallsTimer = 0f;
    private bool areSideWallsActive = false;

    public int CurrentScore => currentScore;
    public int RemainingCoins => remainingCoins;
    public bool IsGameOver => isGameOver;
    public bool IsFeverMode => isFeverMode;
    public float FeverMeter => feverMeter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            uiManager = Object.FindAnyObjectByType<UIManager>();
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (isGameOver && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R)))
        {
            RestartGame();
            return;
        }

        // フィーバータイマー管理
        if (isFeverMode)
        {
            feverTimer -= Time.deltaTime;
            if (feverTimer <= 0f)
            {
                EndFeverMode();
            }
        }

        // 壁パワーアップタイマー管理
        if (areSideWallsActive)
        {
            sideWallsTimer -= Time.deltaTime;
            if (sideWallsTimer <= 0f)
            {
                DeactivateSideWalls();
            }
        }

        // UIのリアルタイムステータス更新
        if (uiManager != null)
        {
            uiManager.UpdateFeverUI(feverMeter, isFeverMode, feverTimer);
            uiManager.UpdateWallUI(areSideWallsActive, sideWallsTimer);
        }
    }

    public void ResetGame()
    {
        currentScore = 0;
        remainingCoins = initialCoins;
        activeCoinsOnBoard = 0;
        isGameOver = false;
        feverMeter = 0f;
        isFeverMode = false;
        feverTimer = 0f;
        sideWallsTimer = 0f;
        areSideWallsActive = false;

        if (SideWallsController.Instance != null)
        {
            SideWallsController.Instance.LowerWalls();
        }

        UpdateUI();

        if (uiManager != null)
        {
            uiManager.ShowGameOver(false);
        }
    }

    /// <summary>
    /// コインを消費して投入を試みます。
    /// </summary>
    public bool TryUseCoin()
    {
        if (isGameOver) return false;

        // フィーバー中は完全にコイン消費なしで投入可能！
        if (isFeverMode)
        {
            return true;
        }

        if (remainingCoins <= 0)
        {
            return false;
        }

        remainingCoins--;
        UpdateUI();
        return true;
    }

    /// <summary>
    /// スコアゾーンにコインが入ったときのポイント加算と各種特殊処理を一括管理します。
    /// </summary>
    public void ProcessCoinScore(CoinType type, Vector3 worldPosition)
    {
        if (isGameOver) return;

        int basePoints = 10;
        int coinsRewarded = 1;
        float feverGain = 6.0f;

        switch (type)
        {
            case CoinType.Standard:
                basePoints = 10;
                coinsRewarded = 1;
                feverGain = 5.0f;
                break;
            case CoinType.Gold:
                basePoints = 30;
                coinsRewarded = 3;
                feverGain = 15.0f;
                break;
            case CoinType.Giant:
                basePoints = 100;
                coinsRewarded = 5;
                feverGain = 25.0f;
                if (CameraShake.Instance != null)
                {
                    CameraShake.Instance.Shake(0.4f, 0.35f);
                }
                break;
            case CoinType.Bonus:
                basePoints = 20;
                coinsRewarded = 2;
                feverGain = 20.0f;
                ActivateSideWalls(8.0f);
                break;
        }

        // フィーバー中はスコアが2倍
        if (isFeverMode)
        {
            basePoints *= 2;
        }

        currentScore += basePoints;
        remainingCoins += coinsRewarded;

        if (!isFeverMode)
        {
            feverMeter = Mathf.Clamp(feverMeter + feverGain, 0f, 100f);
            if (feverMeter >= 100f)
            {
                StartFeverMode();
            }
        }

        // 効果音とパーティクル演出
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayScoreSound(type);
        }
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.PlayScoreEffect(worldPosition, type);
        }

        UpdateUI();
    }

    private void StartFeverMode()
    {
        isFeverMode = true;
        feverTimer = FeverDuration;
        feverMeter = 100f;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayFeverStartSound();
        }

        StartCoroutine(FeverCoinRainRoutine());
    }

    private void EndFeverMode()
    {
        isFeverMode = false;
        feverMeter = 0f;
        feverTimer = 0f;
    }

    private IEnumerator FeverCoinRainRoutine()
    {
        float duration = 4.0f;
        float interval = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration && !isGameOver)
        {
            if (CoinSpawner.Instance != null)
            {
                CoinSpawner.Instance.SpawnRandomCoinBonus();
            }
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }

    private void ActivateSideWalls(float duration)
    {
        areSideWallsActive = true;
        sideWallsTimer = duration;
        if (SideWallsController.Instance != null)
        {
            SideWallsController.Instance.RaiseWalls();
        }
    }

    private void DeactivateSideWalls()
    {
        areSideWallsActive = false;
        sideWallsTimer = 0f;
        if (SideWallsController.Instance != null)
        {
            SideWallsController.Instance.LowerWalls();
        }
    }

    public void RegisterCoin()
    {
        activeCoinsOnBoard++;
    }

    public void UnregisterCoin()
    {
        activeCoinsOnBoard--;
        
        if (remainingCoins <= 0 && activeCoinsOnBoard <= 0)
        {
            TriggerGameOver();
        }
    }

    private void UpdateUI()
    {
        if (uiManager != null)
        {
            // UIManagerのバグ(スコアテキストに残りコイン数を送っていた)を正しく修正
            uiManager.UpdateUI(currentScore);
            uiManager.UpdateCoins(remainingCoins);

            if (CoinSpawner.Instance != null)
            {
                uiManager.UpdateNextCoinUI(CoinSpawner.Instance.NextCoinType);
            }
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        if (uiManager != null)
        {
            uiManager.ShowGameOver(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}