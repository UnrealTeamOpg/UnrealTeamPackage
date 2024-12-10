using UnityEngine;

namespace UnrealTeam.Common.Modules.InputControl
{
    public interface IValue2DInputModel : IPressed, IReleased, IHold, IName, IEnable, IDisable
    {
        public Vector2 GetValue2D();
    }
}