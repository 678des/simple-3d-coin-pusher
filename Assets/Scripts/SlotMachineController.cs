using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// 3Dプッシャーゲームにアーケード特有の興奮をもたらす「スロットマシン」システム。
/// プッシャー上に動的に判定ポケットを配置し、投入されたコインのタイミングゲームによる報酬駆動を実現します。
/// </summary>
public class SlotMachineController : MonoBehaviour
{
    public static SlotMachineController Instance { get; private set; }

    [Header("Slot Settings")]
    [Tooltip("スロットリールの各シンボルリスト")]
    [SerializeField] private string[] symbols = { "COIN", "WALL", "GIANT", "FEVER", "777" };

    [Tooltip("リール回転の演出時間（秒）")]
    [SerializeField] private float spinDuration = 1.8f;

    [Tooltip("リール変更ごとのビープ感覚（秒）")]
    [SerializeField] private float tickInterval = 0.1f;

    private bool isSpinning = false;
    private string reel1 = "7";
    private string reel2 = "7";
    private string reel3 = "7";

    private TextMeshPro 3dBillboardText;
    private GameObject billboardContainer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ゲームが起動した後に自動で看板UIとプッシャーに判定ポケットを構築
        CreateDynamic3DBillboard();
        InjectPocketsToPusher();
    }

    /// <summary>
    /// スロットの回転をリクエストします。すでに回転中の場合は無視されます。
    /// </summary>
    public void TriggerSpin()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float elapsed = 0f;
        float nextTick = 0f;

        while (elapsed < spinDuration)
        {
            if (elapsed >= nextTick)
            {
                // 疑似的にリールシンボルを高速更新
                reel1 = symbols[Random.Range(0, symbols.Length)];
                reel2 = symbols[Random.Range(0, symbols.Length)];
                reel3 = symbols[Random.Range(0, symbols.Length)];
                
                UpdateBillboardText(true);
                
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySlotTickSound();
                }

                nextTick = elapsed + tickInterval;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 最終確率計算による結果決定 (均等ではなくアーケード向けに調整)
        DetermineResult();
        isSpinning = false;
    }

    private void DetermineResult()
    {
        float rand = Random.value;

        // 確率コントロール (30%で何らかのペア、10%でジャックポットトリプル、60%ではズレ)
        if (rand < 0.08f)
        {
            // 超大当たり: 777 Triple!
            reel1 = reel2 = reel3 = "777";
            ApplyReward("777");
        }
        else if (rand < 0.18f)
        {
            // フィーバー大盛り
            reel1 = reel2 = reel3 = "FEVER";
            ApplyReward("FEVER");
        }
        else if (rand < 0.32f)
        {
            // コインボーナス
            reel1 = reel2 = reel3 = "COIN";
            ApplyReward("COIN");
        }
        else if (rand < 0.42f)
        {
            // 巨大コイン降臨
            reel1 = reel2 = reel3 = "GIANT";
            ApplyReward("GIANT");
        }
        else if (rand < 0.52f)
        {
            // 壁復活
            reel1 = reel2 = reel3 = "WALL";
            ApplyReward("WALL");
        }
        else
        {
            // ハズレ（または2つの部分一致によるコンソレーション）
            string[] choices = { "COIN", "WALL", "GIANT", "FEVER" };
            reel1 = choices[Random.Range(0, choices.Length)];
            reel2 = choices[Random.Range(0, choices.Length)];
            reel3 = choices[Random.Range(0, choices.Length)];

            // 偶発的に揃ってしまった場合の整合性チェック
            if (reel1 == reel2 && reel2 == reel3)
            {
                ApplyReward(reel1);
            }
            else
            {
                // 完全ハズレ：ちょっとしたスコアおまけ
                if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
                {
                    GameManager.Instance.ProcessCoinScore(CoinType.Standard, transform.position);
                }
                UpdateBillboardText(false, "TRY AGAIN");
            }
        }
    }

    private void ApplyReward(string symbol)
    {
        UpdateBillboardText(false, "WIN: " + symbol + "!");

        if (SoundManager.Instance != null)
        {
            if (symbol == "777")
            {
                SoundManager.Instance.PlaySlotJackpotSound();
            }
            else
            {
                SoundManager.Instance.PlaySlotWinSound();
            }
        }

        switch (symbol)
        {
            case "777":
                // 777大当たり: 大量コインをシャワー召喚し、Feverを即時MAXに
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessCoinScore(CoinType.Giant, new Vector3(0, 2f, -1f));
                    StartCoroutine(InstantCoinShower(8));
                }
                break;

            case "FEVER":
                // フィーバーゲージを一気に補給
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessCoinScore(CoinType.Bonus, new Vector3(0, 1.5f, 0f));
                }
                break;

            case "COIN":
                // 直接物理ステージにコインを5枚連射で降らせる
                StartCoroutine(InstantCoinShower(5));
                break;

            case "GIANT":
                // 巨大コインを空中から投下して大揺れ誘発
                SpawnCoinAtCenter(CoinType.Giant);
                break;

            case "WALL":
                // 壁パワーアップをダイレクトに駆動
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessCoinScore(CoinType.Bonus, new Vector3(0, 1f, -1f));
                }
                break;
        }
    }

    private IEnumerator InstantCoinShower(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (CoinSpawner.Instance != null)
            {
                CoinSpawner.Instance.SpawnRandomCoinBonus();
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void SpawnCoinAtCenter(CoinType type)
    {
        GameObject coinObj = GameObject.FindWithTag("Coin");
        if (coinObj != null && CoinSpawner.Instance != null)
        {
            CoinSpawner.Instance.SpawnRandomCoinBonus();
        }
    }

    private void UpdateBillboardText(bool active, string msg = "")
    {
        if (3dBillboardText == null) return;

        if (active)
        {
            3dBillboardText.text = $"<color=#FFCC00>[ {reel1} ]</color>  <color=#FF5555>[ {reel2} ]</color>  <color=#33CCFF>[ {reel3} ]</color>";
        }
        else
        {
            3dBillboardText.text = $"<color=#FF4500>★{msg}★</color>\n<size=18>[ {reel1} ] [ {reel2} ] [ {reel3} ]</size>";
        }
    }

    /// <summary>
    /// アセットの参照なしでシーン上に動的な3D電光掲示板を配置します。
    /// </summary>
    private void CreateDynamic3DBillboard()
    {
        billboardContainer = new GameObject("Slot_3DBillboard");
        // プッシャーステージ（原点[0,0,0]、奥行き6）の奥側の背景壁あたりに配置
        billboardContainer.transform.position = new Vector3(0f, 3.8f, 2.5f);

        // 黒いバックボード作成
        GameObject board = GameObject.CreatePrimitive(PrimitiveType.Cube);
        board.name = "Board";
        board.transform.SetParent(billboardContainer.transform);
        board.transform.localPosition = Vector3.zero;
        board.transform.localScale = new Vector3(4.5f, 1.2f, 0.2f);
        Destroy(board.GetComponent<Rigidbody>());
        if (board.TryGetComponent<Renderer>(out var ren))
        {
            ren.material.color = new Color(0.05f, 0.05f, 0.1f);
        }

        // ネオンの外枠を追加して美しく見せる
        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);
        border.name = "NeonBorder";
        border.transform.SetParent(billboardContainer.transform);
        border.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        border.transform.localScale = new Vector3(4.6f, 0.08f, 0.22f);
        Destroy(border.GetComponent<Rigidbody>());
        if (border.TryGetComponent<Renderer>(out var bRen))
        {
            bRen.material.color = Color.magenta;
        }

        // TMProを使用したテキスト表示
        GameObject textObj = new GameObject("BillboardText");
        textObj.transform.SetParent(billboardContainer.transform);
        textObj.transform.localPosition = new Vector3(0f, 0f, -0.12f);
        
        3dBillboardText = textObj.AddComponent<TextMeshPro>();
        3dBillboardText.fontSize = 6;
        3dBillboardText.alignment = TextAlignmentOptions.Center;
        3dBillboardText.text = "<color=#FFCC00>[ BAR ]</color>  <color=#FF5555>[ BAR ]</color>  <color=#33CCFF>[ BAR ]</color>";
    }

    /// <summary>
    /// 動いているプッシャー上に自動検知式のトリガーポケットを左右中に合計3つインジェクションします。
    /// 動くプッシャーの穴に入れる「タイミング要素」を自動生成します。
    /// </summary>
    private void InjectPocketsToPusher()
    {
        // シーン内のPusherMovementを自動的に走査
        PusherMovement pusher = Object.FindFirstObjectByType<PusherMovement>();
        if (pusher == null) return;

        // 3つのポケット（左・中・右）を動的作成し、プッシャーの子としてバインド
        CreatePocket("MiddlePocket", pusher.transform, new Vector3(0f, 0.45f, -0.8f), Color.green);
        CreatePocket("LeftPocket", pusher.transform, new Vector3(-1.1f, 0.45f, -0.8f), Color.cyan);
        CreatePocket("RightPocket", pusher.transform, new Vector3(1.1f, 0.45f, -0.8f), Color.cyan);
    }

    private void CreatePocket(string name, Transform parent, Vector3 localPos, Color neonColor)
    {
        GameObject pocket = new GameObject(name);
        pocket.transform.SetParent(parent, false);
        pocket.transform.localPosition = localPos;

        // トリガーコライダーの設定
        BoxCollider col = pocket.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(0.6f, 0.6f, 0.6f);

        // トリガースクリプトのバインド
        pocket.AddComponent<SlotTriggerZone>();

        // プレイヤーに見えるように視覚的なネオン枠を生成
        GameObject visualRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        visualRing.name = "VisualRing";
        visualRing.transform.SetParent(pocket.transform, false);
        visualRing.transform.localPosition = Vector3.zero;
        visualRing.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);
        Destroy(visualRing.GetComponent<Rigidbody>());
        Destroy(visualRing.GetComponent<Collider>());
        
        if (visualRing.TryGetComponent<Renderer>(out var ren))
        {
            ren.material.color = neonColor;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoInitialize()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("SlotMachineController_Procedural");
            go.AddComponent<SlotMachineController>();
        }
    }
}