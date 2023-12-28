using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T SearchObjectWithComponent<T>(Transform transformSearch, string objectName)
    {
        foreach(Transform trs in transformSearch) {
            if (trs.gameObject.name == objectName)
            {
                return trs.GetComponent<T>();
            }
        }
        return default(T);
    }
}
