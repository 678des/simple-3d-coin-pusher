using UnityEngine;

public class GlobalPhysicsManager : MonoBehaviour
{
    public static GlobalPhysicsManager Instance { get; private set; }
    private float originalGravity;

    private void Awake() { Instance = this; originalGravity = Physics.gravity.y; }

    public void SetTimeScale(float scale) { Time.timeScale = scale; }

    public void SetGravityMultiplier(float multiplier) {
        Physics.gravity = new Vector3(0, originalGravity * multiplier, 0);
    }

    public void ResetPhysics() {
        Time.timeScale = 1.0f;
        Physics.gravity = new Vector3(0, originalGravity, 0);
    }
}