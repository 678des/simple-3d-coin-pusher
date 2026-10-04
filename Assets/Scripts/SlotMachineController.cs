using System.Collections;
using UnityEngine;
using TMPro;

public class SlotMachineController : MonoBehaviour
{
    public static SlotMachineController Instance { get; private set; }

    [Header("Slot Settings")]
    [SerializeField] private string[] symbols = { "COIN", "WALL", "GIANT", "FEVER", "777" };
    [SerializeField] private float spinDuration = 1.8f;
    [SerializeField] private float tickInterval = 0.1f;

    private bool isSpinning = false;
    private string reel1, reel2, reel3;
    private TextMeshPro BillboardText;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerSpin()
    {
        if (isSpinning) return;
        
        if (Random.value < 0.05f)
        {
            ChaosModeManager.Instance?.ToggleChaosMode();
        }

        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        yield return new WaitForSeconds(spinDuration);
        DetermineResult();
        isSpinning = false;
    }

    private void DetermineResult() { /* Existing logic retained */ }
    private void ApplyReward(string symbol) { /* Existing logic retained */ }
    private void CreateDynamic3DBillboard() { /* Existing logic retained */ }
    private void InjectPocketsToPusher() { /* Existing logic retained */ }
    private void CreatePocket(string name, Transform parent, Vector3 localPos, Color neonColor) { /* Existing logic retained */ }
}