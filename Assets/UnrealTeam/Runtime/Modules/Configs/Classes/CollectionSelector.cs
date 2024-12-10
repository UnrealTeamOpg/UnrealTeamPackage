using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealTeam.Runtime.Modules.Configs.Classes
{
    [Serializable]
    public class CollectionSelector<T>
    {
        [HideInInspector] public string Id;
        [HideInInspector] public string Name;

        [Tooltip("Collection to select from")]
        public List<T> Collection;

        public T SelectedItem
        {
            get
            {
                var index = int.TryParse(Id, out var parsedId) ? parsedId : -1;
                return index >= 0 && index < Collection.Count ? Collection[index] : default;
            }
        }
    }
}