using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の進行管理、スコア、所持コイン数の管理、およびゲームオーバー判定を行うマネージャークラス。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// GameManagerのシングルトンインスタンス。
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("コイン設定")]
    [Tooltip("ゲーム開始時の所持コイン数")]
    [SerializeField] private int initialCoins = 50;

    private UIManager uiManager;
    private int currentScore;
    private int remainingCoins;
    private int activeCoinsOnBoard;
    private bool isGameOver;

    /// <summary>
    /// 現在のスコアを取得します。
    /// </summary>
    public int CurrentScore => currentScore;

    /// <summary>
    /// 残りコイン数を取得します。
    /// </summary>
    public int RemainingCoins => remainingCoins;

    /// <summary>
    /// ゲームオーバー状態かどうかを取得します。
    /// </summary>
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        // シングルトンの確立
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 同一オブジェクトにアタッチされているUIManagerを取得
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        // ゲームオーバー時に画面クリックまたはRキー入力でリスタート
        if (isGameOver && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R)))
        {
            RestartGame();
        }
    }

    /// <summary>
    /// ゲーム状態を初期化します。
    /// </summary>
    public void ResetGame()
    {
        currentScore = 0;
        remainingCoins = initialCoins;
        activeCoinsOnBoard = 0;
        isGameOver = false;

        if (uiManager != null)
        {
            uiManager.UpdateUI(currentScore, remainingCoins);
            uiManager.ShowGameOver(false);
        }
    }

    /// <summary>
    /// コインを消費して投入を試みます。
    /// </summary>
    /// <returns>投入に成功した場合はtrue、コイン不足またはゲームオーバー時はfalse</returns>
    public bool TryUseCoin()
    {
        if (isGameOver || remainingCoins <= 0)
        {
            return false;
        }

        remainingCoins--;
        UpdateUI();
        return true;
    }

    /// <summary>
    /// スコアを加算し、手前に落ちた報酬として持ちコインを増やします。
    /// </summary>
    /// <param name="points">加算するスコア値</param>
    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        remainingCoins++; // 手前に落ちたらコインが1枚戻る
        UpdateUI();
    }

    /// <summary>
    /// 盤面にコインが生成された際に呼び出され、アクティブなコイン数をカウントします。
    /// </summary>
    public void RegisterCoin()
    {
        activeCoinsOnBoard++;
    }

    /// <summary>
    /// コインが破棄された（回収または場外落下）際に呼び出され、ゲームオーバー判定を行います。
    /// </summary>
    public void UnregisterCoin()
    {
        activeCoinsOnBoard--;
        
        // 持ちコインがなく、かつ盤面上に動かすコインが1つもなくなった場合にゲームオーバー
        if (remainingCoins <= 0 && activeCoinsOnBoard <= 0)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// UIの表示を最新の状態に更新します。
    /// </summary>
    private void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateUI(currentScore, remainingCoins);
        }
    }

    /// <summary>
    /// ゲームオーバー状態に移行します。
    /// </summary>
    private void TriggerGameOver()
    {
        isGameOver = true;
        if (uiManager != null)
        {
            uiManager.ShowGameOver(true);
        }
    }

    /// <summary>
    /// シーンを再読み込みしてゲームをリスタートします。
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
