using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int initialCoins = 50;
    public bool IsGameOver { get; private set; }
    
    private void Awake() { Instance = this; }
    
    public bool TryUseCoin() { return !IsGameOver; }
    public void ProcessCoinScore(CoinType type, Vector3 worldPosition) { }
    public void UpdateFeverState(bool isActive) { }
    public void RegisterCoin() { }
    public void UnregisterCoin() { }
}