// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;
using Gamelogic.Extensions.Internal;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Used to mark a field to add a comment above the field in the inspector.
	/// </summary>
	/// <seealso cref="UnityEngine.PropertyAttribute" />
	[AttributeUsage(AttributeTargets.Field)]
	[Version(2, 3)]
	public class CommentAttribute : PropertyAttribute
	{
		public readonly GUIContent content;

		public CommentAttribute(string comment, string tooltip = "")
		{
			content = string.IsNullOrEmpty(tooltip) ? new GUIContent(comment) : new GUIContent(comment + " [?]", tooltip);
		}
	}
}
