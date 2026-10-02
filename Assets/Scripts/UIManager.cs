using UnityEngine;
using TMPro;

/// <summary>
/// 3DコインプッシャーゲームのUI表示を管理するクラス。
/// スコアや残りコイン数のテキスト表示を更新します。
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] 
    private TextMeshProUGUI scoreText;

    [SerializeField] 
    private TextMeshProUGUI coinText;

    [SerializeField]
    private GameObject gameOverUI;

    // 文字列結合によるGC Allocを避けるためのフォーマット定義
    private const string ScoreFormat = "SCORE: {0}";
    private const string CoinFormat = "COINS: {0}";

    private void Awake()
    {
        // 初期状態ではゲームオーバー表示を非アクティブにする
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // 参照チェック
        if (scoreText == null)
        {
            Debug.LogError("[UIManager] Score Text がアサインされていません。");
        }
        if (coinText == null)
        {
            Debug.LogError("[UIManager] Coin Text がアサインされていません。");
        }
    }

    /// <summary>
    /// スコア表示を更新します（GC Allocを発生させないTMPのSetTextを使用）。
    /// </summary>
    /// <param name="score">現在のスコア</param>
    public void UpdateUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.SetText(ScoreFormat, score);
        }
    }

    /// <summary>
    /// 残りコイン数の表示を更新します（GC Allocを発生させないTMPのSetTextを使用）。
    /// </summary>
    /// <param name="coins">現在の所持コイン数</param>
    public void UpdateCoins(int coins)
    {
        if (coinText != null)
        {
            coinText.SetText(CoinFormat, coins);
        }
    }

    /// <summary>
    /// ゲームオーバー画面の表示状態を切り替えます。
    /// </summary>
    /// <param name="isGameOver">ゲームオーバー状態かどうか</param>
    public void ShowGameOver(bool isGameOver)
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(isGameOver);
        }
    }
}
