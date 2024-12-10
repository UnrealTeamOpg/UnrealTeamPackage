using System;
using TriInspector;
using UnityEngine;

namespace UnrealTeam.Common.Additional
{
    [Serializable, InlineProperty, DeclareHorizontalGroup("group")]
    public abstract class NumberRange<T>
    {
        [field: SerializeField, Group("group"), LabelWidth(30)] 
        public T Min { get; private set; }
        
        [field: SerializeField, Group("group"), LabelWidth(30)] 
        public T Max { get; private set; }
    }
}