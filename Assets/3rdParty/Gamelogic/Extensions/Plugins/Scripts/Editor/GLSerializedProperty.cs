using System;
using Gamelogic.Extensions.Internal;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// Wraps a SerializedProperty, and provides additional functions, such as
	/// tooltips and a more powerful Find method.
	/// </summary>
	[Version(1, 2)]
	public class GLSerializedProperty
	{
		public SerializedProperty SerializedProperty { get; set; }

		public string CustomTooltip { get; set; }

		[Obsolete("Use PropertyType instead")]
		public SerializedPropertyType propertyType
		{
			get { return PropertyType; }
		}

		public SerializedPropertyType PropertyType
		{
			get { return SerializedProperty.propertyType; }
		}

		[Obsolete("Use ObjectReferenceValue instead")]
		public Object objectReferenceValue
		{
			get { return ObjectReferenceValue; }
			set { ObjectReferenceValue = value; }
		}

		public Object ObjectReferenceValue
		{
			get { return SerializedProperty.objectReferenceValue; }
			set { SerializedProperty.objectReferenceValue = value; }
		}

		[Obsolete("Use EnumValueIndex instead")]
		public int enumValueIndex
		{
			get { return EnumValueIndex; }
			set { EnumValueIndex = value; }
		}

		public int EnumValueIndex
		{
			get { return SerializedProperty.enumValueIndex; }
			set { SerializedProperty.enumValueIndex = value; }
		}

		public string[] EnumNames
		{
			get { return SerializedProperty.enumNames; }
		}

		[Obsolete("Use BoolValue instead")]
		public bool boolValue
		{
			get { return BoolValue; }
			set { BoolValue = value; }
		}

		public bool BoolValue
		{
			get { return SerializedProperty.boolValue; }
			set { SerializedProperty.boolValue = value; }
		}

		[Obsolete("Use IntValue instead")]
		public int intValue
		{
			get { return IntValue; }
			set { IntValue = value; }
		}

		public int IntValue
		{
			get { return SerializedProperty.intValue; }
			set { SerializedProperty.intValue = value; }
		}

		public float FloatValue
		{
			get { return SerializedProperty.floatValue; }
			set { SerializedProperty.floatValue = value; }
		}

		[Obsolete("Use StringValue instead")]
		public string stringValue
		{
			get { return StringValue; }
			set { StringValue = value; }
		}

		public string StringValue
		{
			get { return SerializedProperty.stringValue; }
			set { SerializedProperty.stringValue = value; }
		}

		public GLSerializedProperty FindPropertyRelative(string name)
		{
			SerializedProperty property = SerializedProperty.FindPropertyRelative(name);

			return new GLSerializedProperty
			{
				SerializedProperty = property
			};
		}
	}
}
