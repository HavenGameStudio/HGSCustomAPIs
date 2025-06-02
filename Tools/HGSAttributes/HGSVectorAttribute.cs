using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HGS.Tools
{
	public class HGSVectorAttribute : PropertyAttribute
	{
		public readonly string[] Labels;

		public HGSVectorAttribute(params string[] labels)
		{
			Labels = labels;
		}
	}
}