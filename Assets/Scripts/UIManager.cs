using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject feverOverlay;
    [SerializeField] private TextMeshProUGUI comboText;

    public void ShowFeverOverlay(bool active) { if(feverOverlay) feverOverlay.SetActive(active); }
    public void UpdateComboText(int count) { if(comboText) comboText.SetText("COMBO: " + count); }
    public void UpdateFeverUI(float v, bool b, float t) {}
    public void UpdateWallUI(bool b, float t) {}
    public void ShowGameOver(bool b) {}
}