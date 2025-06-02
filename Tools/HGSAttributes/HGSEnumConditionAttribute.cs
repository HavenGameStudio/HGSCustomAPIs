using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HGS.Tools
{
	/// <summary>
	/// An attribute to conditionnally hide fields based on the current selection in an enum.
	/// Usage :  [MMEnumCondition("rotationMode", (int)RotationMode.LookAtTarget, (int)RotationMode.RotateToAngles)]
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class HGSEnumConditionAttribute : PropertyAttribute
	{
		public string EnumFieldName { get; }
		public object EnumValue { get; }

		public HGSEnumConditionAttribute(string enumFieldName, object enumValue)
		{
			EnumFieldName = enumFieldName;
			EnumValue = enumValue;
		}
	}
}