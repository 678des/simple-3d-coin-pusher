using UnityEngine;

/// <summary>
/// アクティブ時に物理的なサイド壁を動的に上昇させ、コインの横漏れを防ぐバリア制御システム。
/// 事前配置なしでも安全動作するためのプロシージャル自動生成。
/// </summary>
public class SideWallsController : MonoBehaviour
{
    public static SideWallsController Instance { get; private set; }

    [Header("Wall GameObjects")]
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    [Header("Animation Config")]
    [SerializeField] private float loweredY = -1.2f;
    [SerializeField] private float raisedY = 0.5f;
    [SerializeField] private float transitionSpeed = 5.0f;

    private bool wallsRaised = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (leftWall == null || rightWall == null)
        {
            GenerateProceduralWalls();
        }

        SetWallsY(loweredY);
    }

    private void Update()
    {
        float targetY = wallsRaised ? raisedY : loweredY;
        
        if (leftWall != null)
        {
            Vector3 pos = leftWall.transform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetY, transitionSpeed * Time.deltaTime);
            leftWall.transform.position = pos;
        }

        if (rightWall != null)
        {
            Vector3 pos = rightWall.transform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetY, transitionSpeed * Time.deltaTime);
            rightWall.transform.position = pos;
        }
    }

    public void RaiseWalls()
    {
        wallsRaised = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWallToggleSound(true);
        }
    }

    public void LowerWalls()
    {
        wallsRaised = false;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWallToggleSound(false);
        }
    }

    private void SetWallsY(float y)
    {
        if (leftWall != null)
        {
            Vector3 pos = leftWall.transform.position;
            pos.y = y;
            leftWall.transform.position = pos;
        }
        if (rightWall != null)
        {
            Vector3 pos = rightWall.transform.position;
            pos.y = y;
            rightWall.transform.position = pos;
        }
    }

    private void GenerateProceduralWalls()
    {
        // 左壁生成 (ステージ幅4の中央から左寄り)
        if (leftWall == null)
        {
            leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftWall.name = "DynamicLeftWall";
            leftWall.transform.position = new Vector3(-1.95f, loweredY, 0f);
            leftWall.transform.localScale = new Vector3(0.15f, 1.8f, 5.5f);
            Destroy(leftWall.GetComponent<Rigidbody>()); // 安定用のキネマティック壁
            if (leftWall.TryGetComponent<Renderer>(out var ren))
            {
                ren.material.color = new Color(0.1f, 0.7f, 1.0f, 0.8f);
            }
        }

        // 右壁生成 (ステージ幅4の中央から右寄り)
        if (rightWall == null)
        {
            rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightWall.name = "DynamicRightWall";
            rightWall.transform.position = new Vector3(1.95f, loweredY, 0f);
            rightWall.transform.localScale = new Vector3(0.15f, 1.8f, 5.5f);
            Destroy(rightWall.GetComponent<Rigidbody>());
            if (rightWall.TryGetComponent<Renderer>(out var ren))
            {
                ren.material.color = new Color(0.1f, 0.7f, 1.0f, 0.8f);
            }
        }
    }
}