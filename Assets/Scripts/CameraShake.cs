using UnityEngine;

/// <summary>
/// 巨大コインなどの重量衝突イベントに合わせて臨場感あふれるカメラ揺れを提供するクラス。
/// シーン開始時に自動でメインカメラを補強します。
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPos;
    private float shakeDuration = 0f;
    private float shakeAmount = 0.2f;
    private float decreaseFactor = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0f)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

    /// <summary>
    /// 指定された時間と強度で画面を振動させます。
    /// </summary>
    public void Shake(float duration, float amount)
    {
        shakeDuration = duration;
        shakeAmount = amount;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoAttachToCamera()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            if (mainCam.GetComponent<CameraShake>() == null)
            {
                mainCam.gameObject.AddComponent<CameraShake>();
            }
        }
    }
}