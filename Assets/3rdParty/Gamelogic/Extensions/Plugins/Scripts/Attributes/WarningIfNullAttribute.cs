using System;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Mark fields in a MonoBehaviour with this attribute to give a specific warning 
	/// when the field is not set.
	/// </summary>
	/// <example>
	/// <code>
	/// public class MyMonoBehaviour : MonoBehaviour
	/// {
	///		[WarningIfNull("Assign the prefab")]
	///		public GameObject playerPrefab;
	///		
	///		//...
	///	}
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Field)]
	public class WarningIfNullAttribute : PropertyAttribute
	{
		/// <summary>
		/// Gets the warning message.
		/// </summary>
		/// <value>The warning message.</value>
		public string WarningMessage
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WarningIfNullAttribute"/> class.
		/// </summary>
		/// <param name="warningMessage">The warning message to display when the marked field is null.</param>
		public WarningIfNullAttribute(string warningMessage)
		{
			WarningMessage = warningMessage;
		}
	}
}
