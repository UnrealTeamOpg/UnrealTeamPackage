using System;

namespace UnrealTeam.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class TriArrayElementLabelAttribute : Attribute
    {
        public string LabelGetterMethod { get; private set; }

        public TriArrayElementLabelAttribute(string labelGetterMethod)
        {
            LabelGetterMethod = labelGetterMethod;
        }
    }
}