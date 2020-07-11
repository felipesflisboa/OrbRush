namespace Gamelogic.Extensions.Internal.HashFunctions
{
	/// <summary>
	/// Base class for representations of hash functions.
	/// </summary>
	public abstract class HashFunction
	{
		/// <summary>
		/// Main hash function for any number of parameters.
		/// </summary>
		/// <param name="data">The data to hash.</param>
		public abstract uint GetHash(params int[] data);

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public virtual uint GetHash(int data)
		{
			return GetHash(new int[] {data});
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public virtual uint GetHash(int x, int y)
		{
			return GetHash(new int[] {x, y});
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public virtual uint GetHash(int x, int y, int z)
		{
			return GetHash(new int[] {x, y, z});
		}

		/// <summary>
		/// Gets a floating point value representation (between 0 and 1) of the hash for the given data.
		/// </summary>
		public float Value(params int[] data)
		{
			return GetHash(data)/(float) uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Value(int data)
		{
			return GetHash(data)/(float) uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Value(int x, int y)
		{
			return GetHash(x, y)/(float) uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Value(int x, int y, int z)
		{
			return GetHash(x, y, z)/(float) uint.MaxValue;
		}

		/// <summary>
		/// Gets a hash value in the given range (including the minimum value but exlcuding the maximum).
		/// </summary>
		/// <param name="min">The minimum value (inclusive) that this method will return.</param>
		/// <param name="max">The maximum value (exclusive) that this method will return.</param>
		/// <param name="data">The data to hash.</param>
		public int Range(int min, int max, params int[] data)
		{
			return min + (int) (GetHash(data)%(max - min));
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public int Range(int min, int max, int data)
		{
			return min + (int) (GetHash(data)%(max - min));
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public int Range(int min, int max, int x, int y)
		{
			return min + (int) (GetHash(x, y)%(max - min));
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public int Range(int min, int max, int x, int y, int z)
		{
			return min + (int) (GetHash(x, y, z)%(max - min));
		}

		/// <summary>
		/// Gets a float presentation of the hash value in the given range.
		/// </summary>
		/// <param name="min">The minimum value (inclusive) that this method will return.</param>
		/// <param name="max">The maximum value (inclusive) that this method will return.</param>
		/// <param name="data">The data to hash.</param>
		public float Range(float min, float max, params int[] data)
		{
			return min + (GetHash(data)*(float) (max - min))/uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Range(float min, float max, int data)
		{
			return min + (GetHash(data)*(float) (max - min))/uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Range(float min, float max, int x, int y)
		{
			return min + (GetHash(x, y)*(float) (max - min))/uint.MaxValue;
		}

		/// <summary>
		/// Optional method that can be implemented to optimize this special case.
		/// </summary>
		public float Range(float min, float max, int x, int y, int z)
		{
			return min + (GetHash(x, y, z)*(float) (max - min))/uint.MaxValue;
		}
	}
}
