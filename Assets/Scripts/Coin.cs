using UnityEngine;

/// <summary>
/// コイン自体の物理挙動および落下のトリガー接触時の破棄処理を行うスクリプト。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    [Header("物理挙動設定")]
    [Tooltip("生成時にランダムに加える回転力の強さ。コインが不自然に重なるのを防ぎます。")]
    [SerializeField] private float _initialRandomTorque = 2.0f;

    [Header("落下・破棄設定")]
    [Tooltip("このY座標を下回った場合、画面外に落下したとみなして自動的に破棄します。")]
    [SerializeField] private float _outOfBoundsY = -5.0f;

    private Rigidbody _rigidbody;
    private bool _isDestroyed;

    private void Awake()
    {
        // 物理コンポーネントのキャッシュ
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // コイン同士が完全に重なって不自然な挙動になるのを防ぐため、微小なランダムトルクを加える
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * _initialRandomTorque;

        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
    }

    private void Update()
    {
        // 盤面外（左右や奥）に落下してトリガーをすり抜けた場合の安全対策
        if (transform.position.y < _outOfBoundsY)
        {
            DestroyCoin();
        }
    }

    /// <summary>
    /// コインを安全に破棄します。二重破棄や不要な物理衝突を防ぎます。
    /// </summary>
    public void DestroyCoin()
    {
        if (_isDestroyed) return;
        _isDestroyed = true;

        // 破棄直前の不要な物理挙動を防止
        _rigidbody.detectCollisions = false;
        _rigidbody.isKinematic = true;

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ScoreZoneなどの回収・消滅エリアに接触した場合は自身を破棄する
        // ※スコア加算などのゲームロジックはScoreZone側で検知して処理します
        if (other.CompareTag("ScoreZone") || other.CompareTag("KillZone"))
        {
            DestroyCoin();
        }
    }
}
