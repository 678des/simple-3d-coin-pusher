using UnityEngine;

public class FeverModeManager : MonoBehaviour
{
    public static FeverModeManager Instance { get; private set; }
    private int comboCount = 0;
    private void Awake() { Instance = this; }
    public void AddScore(int amount) { comboCount++; if (comboCount >= 10) TriggerFever(); }
    public void ResetCombo() { comboCount = 0; }
    public void TriggerFever() { GameManager.Instance.UpdateFeverState(true); comboCount = 0; }
}