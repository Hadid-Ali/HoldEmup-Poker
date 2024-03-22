using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericExtensions
{
    public static T GetRandomObject<T>(this List<T> list) => list[Random.Range(0, list.Count)];
}
