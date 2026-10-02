using UnityEngine;

public enum CoinType
{
    Standard,
    Gold,
    Giant,
    Bonus
}

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

    [Header("コインのカスタム属性")]
    [SerializeField] private CoinType _coinType = CoinType.Standard;

    public CoinType Type => _coinType;

    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private bool _isDestroyed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        // ゲームマネージャーへのコイン登録
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterCoin();
        }

        // タイプに応じた色や物理特性の設定を適用
        ApplyCoinTypeProperties();

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
    /// 特殊コインの種類を設定し、即時描画・物理状態に反映させます。
    /// </summary>
    public void Initialize(CoinType type)
    {
        _coinType = type;
        ApplyCoinTypeProperties();
    }

    private void ApplyCoinTypeProperties()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();

        Color coinColor = Color.white;
        Vector3 baseScale = new Vector3(1.0f, 0.2f, 1.0f); // 既存のCylinderのスケール

        switch (_coinType)
        {
            case CoinType.Standard:
                coinColor = new Color(0.9f, 0.75f, 0.2f); // ゴールデンイエロー
                _rigidbody.mass = 1.0f;
                break;
            case CoinType.Gold:
                coinColor = new Color(1.0f, 0.5f, 0.0f); // オレンジゴールド
                _rigidbody.mass = 1.2f;
                break;
            case CoinType.Giant:
                coinColor = new Color(0.8f, 0.3f, 0.3f); // 赤銅メタリック
                baseScale = new Vector3(2.0f, 0.4f, 2.0f); // 巨大化
                _rigidbody.mass = 5.0f; // 重厚な物理移動力
                break;
            case CoinType.Bonus:
                coinColor = new Color(0.0f, 0.8f, 1.0f); // サイアンネオン
                _rigidbody.mass = 1.0f;
                break;
        }

        transform.localScale = baseScale;

        if (_renderer != null)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", coinColor);
            _renderer.SetPropertyBlock(_propBlock);
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

    private void OnDestroy()
    {
        // ゲームマネージャーにコイン破棄を通知してゲームオーバー判定を促す
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterCoin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ScoreZone、SideLossZoneなどの回収エリアに接触した場合は自身を安全に破棄する
        if (other.TryGetComponent<ScoreZone>(out var scoreZone))
        {
            scoreZone.OnCoinEntered(this);
            DestroyCoin();
        }
        else if (other.TryGetComponent<SideLossZone>(out var lossZone))
        {
            lossZone.OnCoinEntered(this);
            DestroyCoin();
        }
        else if (other.CompareTag("ScoreZone") || other.CompareTag("KillZone"))
        {
            DestroyCoin();
        }
    }
}