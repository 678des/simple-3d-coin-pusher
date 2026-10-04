using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int initialCoins = 50;
    private UIManager uiManager;
    private int currentScore;
    private int remainingCoins;
    private int activeCoinsOnBoard;
    private bool isGameOver;
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

    private void Awake() { Instance = this; uiManager = GetComponent<UIManager>(); }
    private void Start() { ResetGame(); }
    private void Update() {
        if (isFeverMode) { feverTimer -= Time.deltaTime; if (feverTimer <= 0f) EndFeverMode(); }
        if (areSideWallsActive) { sideWallsTimer -= Time.deltaTime; if (sideWallsTimer <= 0f) DeactivateSideWalls(); }
        if (uiManager != null) { uiManager.UpdateFeverUI(feverMeter, isFeverMode, feverTimer); uiManager.UpdateWallUI(areSideWallsActive, sideWallsTimer); }
    }
    public void ResetGame() {
        currentScore = 0; remainingCoins = initialCoins; activeCoinsOnBoard = 0; isGameOver = false;
        feverMeter = 0f; isFeverMode = false; feverTimer = 0f; sideWallsTimer = 0f; areSideWallsActive = false;
        GlobalPhysicsManager.Instance?.ResetPhysics();
    }
    public bool TryUseCoin() { if (isGameOver) return false; if (isFeverMode) return true; if (remainingCoins <= 0) return false; remainingCoins--; return true; }
    public void ProcessCoinScore(CoinType type, Vector3 worldPosition) {
        currentScore += 10; remainingCoins += 1;
        if (SoundManager.Instance != null) SoundManager.Instance.PlayScoreSound(type);
    }
    public void RegisterCoin() => activeCoinsOnBoard++;
    public void UnregisterCoin() { activeCoinsOnBoard--; if (remainingCoins <= 0 && activeCoinsOnBoard <= 0) TriggerGameOver(); }
    public void TriggerPhysicsModifier(string type) {
        if (type == "Chaos") GlobalPhysicsManager.Instance?.SetGravityMultiplier(0.5f);
        else if (type == "Slow") GlobalPhysicsManager.Instance?.SetTimeScale(0.5f, 2.0f);
    }
    private void TriggerGameOver() { isGameOver = true; uiManager?.ShowGameOver(true); }
    private void StartFeverMode() { isFeverMode = true; feverTimer = FeverDuration; }
    private void EndFeverMode() { isFeverMode = false; GlobalPhysicsManager.Instance?.ResetPhysics(); }
    private void DeactivateSideWalls() { areSideWallsActive = false; }
}