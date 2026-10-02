using UnityEngine;

/// <summary>
/// プッシャー上の特定判定ポケットに入ったコインを検知し、
/// スロットの回転トリガーを安全に実行するための判定スクリプト。
/// </summary>
public class SlotTriggerZone : MonoBehaviour
{
    private float cooldownTimer = 0f;
    private const float CooldownDuration = 1.2f;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cooldownTimer > 0f) return;

        // 接触したオブジェクトがコインかどうか判定
        if (other.TryGetComponent<Coin>(out var coin))
        {
            cooldownTimer = CooldownDuration;

            // スロットマシンマネージャーに回転要求を発行
            if (SlotMachineController.Instance != null)
            {
                SlotMachineController.Instance.TriggerSpin();
            }

            // フィーバー演出のように、獲得時のビジュアルフィードバックを追加
            if (EffectsManager.Instance != null)
            {
                // ポケット位置に吸い込まれた際の美しい軌跡エフェクト
                EffectsManager.Instance.PlayScoreEffect(transform.position, CoinType.Bonus);
            }
        }
    }
}