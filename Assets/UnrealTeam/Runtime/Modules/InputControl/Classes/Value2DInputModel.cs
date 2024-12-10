using UnityEngine;
using UnityEngine.InputSystem;

namespace UnrealTeam.Common.Modules.InputControl
{
    public class Value2DInputModel : IValue2DInputModel
    {
        private readonly InputAction _inputAction;
        
        public Value2DInputModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
        public void Enable() => _inputAction.Enable();
        public void Disable() => _inputAction.Disable();
        public Vector2 GetValue2D() => _inputAction.ReadValue<Vector2>();
        public string GetNameKey =>_inputAction.GetBindingDisplayString();
    }
}