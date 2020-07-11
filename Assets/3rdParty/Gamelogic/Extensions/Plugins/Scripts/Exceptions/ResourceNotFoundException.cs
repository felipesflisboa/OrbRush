// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// Thrown when trying to load a resource (using <see cref="UnityEngine.Resources.Load(string)"/> and variants) but the resource is not found.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class ResourceNotFoundException : Exception
	{
		public string resourceName;
		public string resourcePath;

		public ResourceNotFoundException() : base("Resource not found")
		{

		} 

		public ResourceNotFoundException(string resourceName) : base(string.Format("Resource '{0}' not found", resourceName))
		{
			this.resourceName = resourceName;
		}

		public ResourceNotFoundException(string resourceName, string resourcePath) : base(string.Format("Resource '{0}' not found at '{1}'", resourceName, resourcePath))
		{
			this.resourceName = resourceName;
			this.resourcePath = resourcePath;
		}

		public ResourceNotFoundException(string resourceName, string resourcePath, string message) : base(message)
		{
			this.resourceName = resourceName;
			this.resourcePath = resourcePath;
		}
	}
}
