using UnityEngine.InputSystem;

namespace UnrealTeam.Common.Modules.InputControl
{
    public class InputKeyModel : IKeyInputModel
    {
        private readonly InputAction _inputAction;
        
        public InputKeyModel(InputAction inputAction)
        {
            _inputAction = inputAction;
        }
        public bool IsPressed() => _inputAction.WasPressedThisFrame();
        public bool IsReleased() => _inputAction.WasReleasedThisFrame();
        public bool IsHold() => _inputAction.IsPressed();
        public void Enable() => _inputAction.Enable();
        public void Disable() => _inputAction.Disable();
    }
}