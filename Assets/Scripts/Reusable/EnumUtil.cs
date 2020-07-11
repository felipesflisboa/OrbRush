using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnumUtil {
    /// <summary>
    /// Get the random value from an enum
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="min">Min range value</param>
    /// <param name="maxOffset">Max range value offset. Put -2 to exclude the last 2 values.</param>
    public static T GetRandomValueFromEnum<T>(int min = 0, int maxOffset = 0) {
        T[] enumArray = System.Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        return enumArray[Random.Range(min, enumArray.Length + maxOffset)];
    }
}