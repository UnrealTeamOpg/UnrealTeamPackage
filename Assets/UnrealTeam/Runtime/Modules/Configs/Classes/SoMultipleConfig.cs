using UnityEngine;

namespace UnrealTeam.Common.Modules.Configs
{
    public abstract class SoMultipleConfig : ScriptableObject, IMultipleConfig
    {
        [SerializeField] private UniqueId _uniqueId = new();
        
        public string Id => _uniqueId.Value;
    }
}