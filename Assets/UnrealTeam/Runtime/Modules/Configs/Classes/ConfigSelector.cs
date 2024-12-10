using System;
using UnityEngine;
using UnrealTeam.Common.Modules.Configs;

namespace UnrealTeam.Common.Configs
{
    [Serializable]
    // ReSharper disable once UnusedTypeParameter
    public class ConfigSelector<TConfig>
        where TConfig : IMultipleConfig
    {
        [field: SerializeField, HideInInspector] 
        public string Id { get; private set; }

        public static implicit operator string(ConfigSelector<TConfig> configSelector)
            => configSelector.Id;
        
#if UNITY_EDITOR
        [field: SerializeField, HideInInspector]
        public string Name { get; private set; }
#endif
    }
}