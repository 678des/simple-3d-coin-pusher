// Responsibility: Provides a base class for all game controllers to ensure consistent initialization and lifecycle management.
// Attachment Note: Attach to any GameObject that acts as a controller in the game.
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }
}