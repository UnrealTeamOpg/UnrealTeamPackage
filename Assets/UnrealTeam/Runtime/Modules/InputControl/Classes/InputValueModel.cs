using UnityEngine.InputSystem;

namespace UnrealTeam.Common.Modules.InputControl
{
    public class InputValueModel : IValueInputModel
    {
        private readonly InputAction _inputAction;

        public InputValueModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
        public void Enable() => _inputAction.Enable();
        public void Disable() => _inputAction.Disable();
        public float GetValue() => _inputAction.ReadValue<float>();
        public string GetNameKey => _inputAction.GetBindingDisplayString();
    }
}