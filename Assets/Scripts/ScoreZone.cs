using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手前に落ちてきたコインの接触を検知し、GameManagerへスコア加算を通知するスクリプト。
/// </summary>
[RequireComponent(typeof(Collider))]
public class ScoreZone : MonoBehaviour
{
    [SerializeField]
    [Tooltip("コイン1枚あたりに獲得できるスコア")]
    private int _scoreValue = 10;

    private GameManager _gameManager;
    
    // 重複カウントを確実に防止するためのキャッシュ（インスタンスIDで管理）
    private readonly HashSet<int> _processedCoinIds = new HashSet<int>();

    private void Start()
    {
        // Unity 6推奨の FindAnyObjectByType を使用してGameManagerを取得・キャッシュ
        _gameManager = Object.FindAnyObjectByType<GameManager>();
        
        if (_gameManager == null)
        {
            Debug.LogWarning("[ScoreZone] GameManagerが見つかりません。スコアが加算されない可能性があります。");
        }

        // コライダーがトリガーとして設定されていることを保証
        if (TryGetComponent<Collider>(out var col))
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトがCoinコンポーネントを持っているか判定（TryGetComponentでアロケーションを回避）
        if (other.TryGetComponent<Coin>(out var coin))
        {
            int coinId = coin.gameObject.GetInstanceID();

            // すでに処理済みのコインである場合は処理をスキップ
            if (_processedCoinIds.Contains(coinId))
            {
                return;
            }

            // 処理済みリストに登録
            _processedCoinIds.Add(coinId);

            // GameManagerにスコア加算を通知
            if (_gameManager != null)
            {
                _gameManager.AddScore(_scoreValue);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ライフサイクルや物理挙動の例外に備え、エリアから離脱したコインのIDをクリーンアップ
        if (other.TryGetComponent<Coin>(out var coin))
        {
            _processedCoinIds.Remove(coin.gameObject.GetInstanceID());
        }
    }
}
