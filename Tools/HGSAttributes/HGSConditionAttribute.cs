using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HGS.Tools
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class HGSConditionAttribute : PropertyAttribute
	{
		public string ConditionFieldName = "";
		public bool Hidden = false;
		public bool Negative = false;

		public HGSConditionAttribute(string conditionBoolean)
		{
			this.ConditionFieldName = conditionBoolean;
			this.Hidden = false;
		}

		public HGSConditionAttribute(string conditionBoolean, bool hideInInspector)
		{
			this.ConditionFieldName = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = false;
		}

		public HGSConditionAttribute(string conditionBoolean, bool hideInInspector, bool negative)
		{
			this.ConditionFieldName = conditionBoolean;
			this.Hidden = hideInInspector;
			this.Negative = negative;
		}

	}
}
