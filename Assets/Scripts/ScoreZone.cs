using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手前に落ちてきたコインの接触を検知し、コインの種類別スコア増減判定をGameManagerに委ねるスクリプト。
/// </summary>
[RequireComponent(typeof(Collider))]
public class ScoreZone : MonoBehaviour
{
    private GameManager _gameManager;
    
    // 重複カウントを完全に防止するためのキャッシュ
    private readonly HashSet<int> _processedCoinIds = new HashSet<int>();

    private void Start()
    {
        _gameManager = Object.FindAnyObjectByType<GameManager>();
        
        if (TryGetComponent<Collider>(out var col))
        {
            col.isTrigger = true;
        }
    }

    public void OnCoinEntered(Coin coin)
    {
        int coinId = coin.gameObject.GetInstanceID();

        if (_processedCoinIds.Contains(coinId))
        {
            return;
        }

        _processedCoinIds.Add(coinId);

        if (_gameManager != null)
        {
            _gameManager.ProcessCoinScore(coin.Type, coin.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Coin>(out var coin))
        {
            _processedCoinIds.Remove(coin.gameObject.GetInstanceID());
        }
    }
}