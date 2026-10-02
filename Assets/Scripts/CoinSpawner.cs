using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 画面のタップ位置を取得し、対応するX軸位置からコインを生成・落とすスクリプト。
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    [Header("References")]
    [Tooltip("生成するコインのプレハブ")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("ゲームマネージャーへの参照（未設定時は自動取得）")]
    [SerializeField] private GameManager gameManager;

    [Header("Spawn Settings")]
    [Tooltip("コインを生成できる左右の最大幅 (X軸の±範囲)")]
    [SerializeField] private float spawnWidth = 2.0f;

    [Tooltip("連続生成を防ぐクールダウン時間（秒）")]
    [SerializeField] private float spawnCooldown = 0.25f;

    private Camera mainCamera;
    private float cooldownTimer;

    private void Awake()
    {
        // メインカメラのキャッシュ
        mainCamera = Camera.main;

        // シーン内からGameManagerを自動取得（未設定時のみ）
        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }
    }

    private void Update()
    {
        // クールダウンタイマーの更新
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 入力検知（マウスクリックまたは画面タップ）
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // UI要素（ボタン等）をタップしている場合はコイン生成をスルー
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // クールダウン中でなく、かつGameManager側でコイン消費に成功した場合のみ生成
            if (cooldownTimer <= 0f)
            {
                if (gameManager != null && gameManager.TryUseCoin())
                {
                    SpawnCoinAtMousePosition();
                    cooldownTimer = spawnCooldown;
                }
            }
        }
    }

    /// <summary>
    /// 入力されたスクリーン座標のX位置をワールド座標にマッピングしてコインを生成します。
    /// </summary>
    private void SpawnCoinAtMousePosition()
    {
        if (coinPrefab == null) return;

        // 入力座標の取得（マルチタッチ時は最初のタッチ位置、それ以外はマウス位置）
        Vector3 inputPosition = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            inputPosition = Input.GetTouch(0).position;
        }

        // 画面の横幅に対する割合 (0.0 ～ 1.0) を算出
        float normalizedX = Mathf.Clamp01(inputPosition.x / Screen.width);

        // 割合をスポナーの左右幅 [-spawnWidth, spawnWidth] にマッピング
        float spawnX = Mathf.Lerp(-spawnWidth, spawnWidth, normalizedX);

        // 生成位置の決定 (Y軸とZ軸は本オブジェクトの位置を基準にする)
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);

        // コインを生成 (物理挙動が自然になるよう、Y軸にランダムな初期回転を与える)
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(coinPrefab, spawnPosition, randomRotation);
    }

    /// <summary>
    /// ギズモを描画して、エディタ上でコインの生成可能範囲を視覚化します。
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 leftPoint = new Vector3(-spawnWidth, transform.position.y, transform.position.z);
        Vector3 rightPoint = new Vector3(spawnWidth, transform.position.y, transform.position.z);
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.1f);
        Gizmos.DrawWireSphere(rightPoint, 0.1f);
    }
}
