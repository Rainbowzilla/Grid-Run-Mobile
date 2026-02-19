using UnityEngine.InputSystem;
using UnityEngine;

public class FrontEndInputManager : MonoBehaviour
{
    private FrontEndInput input;
    private CameraLook cam;

    private void Awake()
    {
        input = new FrontEndInput();
        input.Enable();
        cam = GetComponent<CameraLook>();
    }


    private void Update()
    {
        if (Gamepad.current != null)
        {
            Vector2 stickDelta = input.FrontEnd.RotateController.ReadValue<Vector2>();
            cam.Look(stickDelta);
        }
        else
            if (input.FrontEnd.Rotate.IsPressed())
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                cam.Look(mouseDelta);
            }
    }
    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();
}
