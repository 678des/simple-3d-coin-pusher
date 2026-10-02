using UnityEngine;

/// <summary>
/// 左右の隙間に落下したコインを回収し、報酬なし（損失）として処理するスクリプト。
/// </summary>
[RequireComponent(typeof(Collider))]
public class SideLossZone : MonoBehaviour
{
    private void Start()
    {
        if (TryGetComponent<Collider>(out var col))
        {
            col.isTrigger = true;
        }
    }

    public void OnCoinEntered(Coin coin)
    {
        // 落下効果音のトリガー
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLossSound();
        }

        // 塵埃パーティクル発生
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.PlayLossEffect(coin.transform.position);
        }
    }
}