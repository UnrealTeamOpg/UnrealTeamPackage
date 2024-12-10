using System;
using UnityEngine;

namespace UnrealTeam.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CollectionElementLabel : PropertyAttribute
    {
        public string LabelGetterMethod { get; private set; }

        public CollectionElementLabel(string labelGetterMethod)
        {
            LabelGetterMethod = labelGetterMethod;
        }
    }
}