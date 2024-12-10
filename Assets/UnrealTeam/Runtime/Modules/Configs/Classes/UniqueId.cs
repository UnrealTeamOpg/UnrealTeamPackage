using System;
using UnityEngine;

namespace UnrealTeam.Common.Modules.Configs
{
    [Serializable]
    public class UniqueId
    {
        [field: SerializeField] public string Value { get; set; }
    }
}