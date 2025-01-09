using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public Vector2 movement { get; private set; }
    public void Movement(InputAction.CallbackContext callbackContext)
    {
        movement = callbackContext.ReadValue<Vector2>();
    }
}
