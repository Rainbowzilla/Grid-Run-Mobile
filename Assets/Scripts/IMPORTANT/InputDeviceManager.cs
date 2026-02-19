using UnityEngine;
using UnityEngine.InputSystem;

public static class InputDeviceManager
{
    public static bool UsingController { get; private set; }

    static InputDeviceManager()
    {
        UpdateControllerState();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            UpdateControllerState();
        }
    }

    private static void UpdateControllerState()
    {
        UsingController = Gamepad.current != null;
    }
}
