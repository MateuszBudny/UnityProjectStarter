//using LowkeyFramework.AttributeSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 Rotate(this Vector2 vector, float eulerAngle)
    {
        float radians = eulerAngle * Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians),
            vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians)
        );
    }

    public static Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        return (point - pivot).Rotate(angle) + pivot;
    }

    public static Vector3 Rotate(this Vector3 vector, Vector3 angles)
    {
        return Quaternion.Euler(angles) * vector;
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        int chosenElementIndex = UnityEngine.Random.Range(0, list.Count);
        return list[chosenElementIndex];
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> listCopy = new List<T>(list);
        List<T> finalShuffledList = new List<T>();
        while(listCopy.Count > 0)
        {
            T elementDrawn = listCopy.GetRandomElement();
            finalShuffledList.Add(elementDrawn);
            listCopy.Remove(elementDrawn);
        }

        return finalShuffledList;
    }

    public static void ForEach<T>(this List<T> list, Action<T, int> forEachDo)
    {
        for(int i = 0; i < list.Count; i++)
        {
            forEachDo(list[i], i);
        }
    }

    public static bool RandomChance(this float chance)
    {
        return UnityEngine.Random.Range(0f, 1f) <= chance;
    }

    public static float RandomRangeFloat(this Vector2 minMax)
    {
        return UnityEngine.Random.Range(minMax.x, minMax.y);
    }

    public static Vector3 RandomRangeVector(this Vector3 minInclusive, Vector3 maxInclusive)
    {
        return new Vector3(UnityEngine.Random.Range(minInclusive.x, maxInclusive.x), UnityEngine.Random.Range(minInclusive.y, maxInclusive.y), UnityEngine.Random.Range(minInclusive.z, maxInclusive.z));
    }

    // MinBy and MaxBy borrowed from: https://github.com/morelinq/MoreLINQ
    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MinBy(selector, null);
    }

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if(source == null)
            throw new ArgumentNullException(nameof(source));
        if(selector == null)
            throw new ArgumentNullException(nameof(selector));
        comparer = comparer ?? Comparer<TKey>.Default;

        using(var sourceIterator = source.GetEnumerator())
        {
            if(!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            var min = sourceIterator.Current;
            var minKey = selector(min);
            while(sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if(comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }
            return min;
        }
    }

    /// <summary>
    /// Returns the maximal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MaxBy(selector, null);
    }

    /// <summary>
    /// Returns the maximal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values. 
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if(source == null)
            throw new ArgumentNullException(nameof(source));
        if(selector == null)
            throw new ArgumentNullException(nameof(selector));
        comparer = comparer ?? Comparer<TKey>.Default;

        using(var sourceIterator = source.GetEnumerator())
        {
            if(!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            var max = sourceIterator.Current;
            var maxKey = selector(max);
            while(sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if(comparer.Compare(candidateProjected, maxKey) > 0)
                {
                    max = candidate;
                    maxKey = candidateProjected;
                }
            }
            return max;
        }
    }

    private const string MATERIAL_EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";

    public static void ToggleEmissionKeyword(this Material material, bool toggle)
    {
        if(toggle)
            material.EnableKeyword("_EMISSION");
        else
            material.DisableKeyword("_EMISSION");
    }

    /// <summary>
    /// Sets the "real" emissive color. This color already has intensity factored in (i.e. HDR). So, the components of that color might be greater than 1 (brightness/intensity). The formula is combinedColor = baseColor * (2^intensity).
    /// </summary>
    /// <param name="material"></param>
    /// <param name="combinedEmissiveHDRColor"></param>
    public static void SetEmissiveCombinedColor(this Material material, Color combinedEmissiveHDRColor)
    {
        material.SetColor(MATERIAL_EMISSION_COLOR_PROPERTY_NAME, combinedEmissiveHDRColor);
    }

    public static void SetEmissiveBaseColorAndIntensity(this Material material, Color color, float intensity)
    {
        material.SetColor(MATERIAL_EMISSION_COLOR_PROPERTY_NAME, color * Mathf.Pow(2, intensity));
    }

    public static void SetEmissiveBaseColor(this Material material, Color color)
    {
        material.SetEmissiveBaseColorAndIntensity(color, material.GetEmissiveIntensity());
    }

    public static void SetEmissiveIntensity(this Material material, float intensity)
    {
        material.SetEmissiveBaseColorAndIntensity(material.GetEmissiveBaseColor(), intensity);
    }

    public static Color GetEmissiveBaseColor(this Material material)
    {
        if(material == null || !material.HasProperty(MATERIAL_EMISSION_COLOR_PROPERTY_NAME))
            return Color.black;

        Color emissiveColor = material.GetColor(MATERIAL_EMISSION_COLOR_PROPERTY_NAME);
        float max = Mathf.Max(emissiveColor.r, emissiveColor.g, emissiveColor.b);

        if(Mathf.Approximately(max, 0f)) // black
            return Color.black;

        // stops (Unity HDR "intensity" value) = log2(max) -> scale (real color multiplier) = 2^stops == max
        float scale = max;
        Color baseColor = emissiveColor / scale;

        // Clamp to [0,1] and ensure alpha = 1 (inspector-like)
        baseColor.r = Mathf.Clamp01(baseColor.r);
        baseColor.g = Mathf.Clamp01(baseColor.g);
        baseColor.b = Mathf.Clamp01(baseColor.b);
        baseColor.a = 1f;

        return baseColor;
    }

    public static float GetEmissiveIntensity(this Material material)
    {
        Color emissiveColorAndIntensity = material.GetColor(MATERIAL_EMISSION_COLOR_PROPERTY_NAME);
        return Mathf.Log(emissiveColorAndIntensity.maxColorComponent, 2);
    }

    //public static void SafeDestroy(this GameObject gameObject)
    //{
    //    gameObject.transform.parent = null;
    //    gameObject.name = "$disposed";
    //    SaveableBehaviour saveable = gameObject.GetComponent<SaveableBehaviour>();
    //    if(saveable)
    //    {
    //        saveable.TurnOffSavingAndLoadingForThisBehaviour = true;
    //    }
    //    UnityEngine.Object.Destroy(gameObject);
    //    gameObject.SetActive(false);
    //}
}
