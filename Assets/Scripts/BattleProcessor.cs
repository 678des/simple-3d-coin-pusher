// Responsibility: Handles the core logic and flow of the battle system.
// Attachment Note: Attach to a dedicated BattleManager GameObject.
using UnityEngine;

public class BattleProcessor : BaseController
{
    [SerializeField] private string battleName = "Default Battle";

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("BattleProcessor initialized: " + battleName);
    }

    protected override void Start()
    {
        base.Start();
    }

    public void ProcessTurn()
    {
        Debug.Log("Processing turn for " + battleName);
    }
}