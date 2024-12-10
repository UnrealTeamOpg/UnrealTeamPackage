namespace UnrealTeam.Common.Modules.InputControl
{
    public interface IValueInputModel : IPressed, IReleased, IHold, IName, IEnable, IDisable
    {
        float GetValue();
    }
}