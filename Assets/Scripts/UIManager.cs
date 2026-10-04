using UnityEngine;
using TMPro;

/// <summary>
/// 3DコインプッシャーゲームのUI表示を管理するクラス。
/// フィーバー表示、特殊タイマー表示、次弾スロット表示などを手続き型UIでフォールバック支援します。
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject gameOverUI;

    [Header("Enriched Option UI Elements")]
    [SerializeField] private TextMeshProUGUI feverText;
    [SerializeField] private UnityEngine.UI.Slider feverSlider;
    [SerializeField] private TextMeshProUGUI wallTimerText;
    [SerializeField] private TextMeshProUGUI nextCoinText;

    private const string ScoreFormat = "SCORE: {0}";
    private const string CoinFormat = "COINS: {0}";
    private const string FeverFormat = "FEVER: {0:F0}%";
    private const string FeverActiveFormat = "★FEVER TIME: {0:F1}s★";
    private const string WallFormat = "WALLS: {0:F1}s";
    private const string NextCoinFormat = "NEXT COIN: {0}";

    private void Awake()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // インスペクターで設定されていない場合、自動的にUI要素を生成または参照取得してバグを防ぎます
        EnsureDynamicUIFallbacks();
    }

    public void UpdateUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.SetText(ScoreFormat, score);
        }
    }

    public void UpdateCoins(int coins)
    {
        if (coinText != null)
        {
            coinText.SetText(CoinFormat, coins);
        }
    }

    public void UpdateFeverUI(float feverValue, bool isActive, float timeRemaining)
    {
        if (feverSlider != null)
        {
            feverSlider.value = feverValue / 100f;
        }

        if (feverText != null)
        {
            if (isActive)
            {
                feverText.color = new Color(1f, 0.3f, 0.3f);
                feverText.SetText(FeverActiveFormat, timeRemaining);
            }
            else
            {
                feverText.color = Color.white;
                feverText.SetText(FeverFormat, feverValue);
            }
        }
    }

    public void UpdateWallUI(bool isActive, float timeRemaining)
    {
        if (wallTimerText != null)
        {
            wallTimerText.gameObject.SetActive(isActive);
            if (isActive)
            {
                wallTimerText.SetText(WallFormat, timeRemaining);
            }
        }
    }

    public void UpdateNextCoinUI(CoinType nextType)
    {
        if (nextCoinText != null)
        {
            //nextCoinText.SetText(NextCoinFormat, nextType.ToString());
            
            switch (nextType)
            {
                case CoinType.Standard: nextCoinText.color = new Color(1f, 0.9f, 0.3f); break;
                case CoinType.Gold: nextCoinText.color = new Color(1f, 0.5f, 0f); break;
                case CoinType.Giant: nextCoinText.color = new Color(0.9f, 0.3f, 0.3f); break;
                case CoinType.Bonus: nextCoinText.color = new Color(0.1f, 0.8f, 1f); break;
            }
        }
    }

    public void ShowGameOver(bool isGameOver)
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(isGameOver);
        }
    }

    private void EnsureDynamicUIFallbacks()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        if (feverText == null || nextCoinText == null || wallTimerText == null)
        {
            GameObject panel = new GameObject("RichUI_Overlay");
            panel.transform.SetParent(this.transform, false);
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 1f);
            panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.anchoredPosition = new Vector2(0f, -80f);
            panelRect.sizeDelta = new Vector2(500f, 120f);

            if (feverText == null)
            {
                GameObject go = new GameObject("FeverText_Dynamic");
                go.transform.SetParent(panel.transform, false);
                feverText = go.AddComponent<TextMeshProUGUI>();
                feverText.fontSize = 22;
                feverText.alignment = TextAlignmentOptions.Center;
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0f, 30f);
                rt.sizeDelta = new Vector2(400f, 30f);
            }

            if (nextCoinText == null)
            {
                GameObject go = new GameObject("NextCoinText_Dynamic");
                go.transform.SetParent(panel.transform, false);
                nextCoinText = go.AddComponent<TextMeshProUGUI>();
                nextCoinText.fontSize = 18;
                nextCoinText.alignment = TextAlignmentOptions.Center;
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0f, 0f);
                rt.sizeDelta = new Vector2(400f, 30f);
            }

            if (wallTimerText == null)
            {
                GameObject go = new GameObject("WallTimerText_Dynamic");
                go.transform.SetParent(panel.transform, false);
                wallTimerText = go.AddComponent<TextMeshProUGUI>();
                wallTimerText.fontSize = 18;
                wallTimerText.alignment = TextAlignmentOptions.Center;
                wallTimerText.color = new Color(0.3f, 0.8f, 1f);
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0f, -30f);
                rt.sizeDelta = new Vector2(400f, 30f);
                wallTimerText.gameObject.SetActive(false);
            }
        }
    }
}