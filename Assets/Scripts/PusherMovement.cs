using UnityEngine;

/// <summary>
/// プッシャーオブジェクトをサイン波を用いて一定範囲で前後往復移動させるスクリプト。
/// コインを物理的に正しく押し出すため、RigidbodyのKinematic移動を使用します。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PusherMovement : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("往復移動の振幅（初期位置からの移動距離）")]
    [SerializeField] private float _amplitude = 1.2f;

    [Tooltip("移動の速度")]
    [SerializeField] private float _speed = 1.5f;

    [Tooltip("移動する方向ベクトル")]
    [SerializeField] private Vector3 _moveDirection = Vector3.forward;

    private Rigidbody _rigidbody;
    private Vector3 _startPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // 物理演算でコインを押し出すため、Kinematicを有効にし、補間を設定して挙動を滑らかにする
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        // 初期位置を基準点として保持
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // 時間経過に基づき、サイン波（-1.0 ～ 1.0）を利用して移動オフセットを計算
        float wave = Mathf.Sin(Time.time * _speed);
        Vector3 offset = _moveDirection.normalized * (wave * _amplitude);

        // 物理挙動を維持しながら安全にオブジェクトを移動
        _rigidbody.MovePosition(_startPosition + offset);
    }
}
