using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{

	public static GameObject SearchObjectIntransform(Transform transformSearch, string objectName)
	{
		foreach (Transform trs in transformSearch)
		{
			if (trs.gameObject.name == objectName)
			{
				return trs.gameObject;
			}
		}
		return default(GameObject);
	}

	/// <summary>
	/// Pesquisa por um objeto dentro de um transform, e retorna o componente de tipo T dentro desse objeto
	/// </summary>
	/// <typeparam name="T">Tipo de componente a ser retornado dentro do objeto</typeparam>
	/// <param name="transformSearch">Transform para ser usado na pesquisa</param>
	/// <param name="objectName">Nome do objeto para ser encontrado</param>
	/// <returns></returns>
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

	/// <summary>
	/// Pesquisa por um objeto dentro de um transform, e retorna o componente de tipo T dentro de cada child desse objeto objeto
	/// </summary>
	/// <typeparam name="T">Tipo de componente a ser retornado dentro do objeto</typeparam>
	/// <param name="transformSearch">Transform para ser usado na pesquisa</param>
	/// <param name="objectName">Nome do objeto para ser encontrado</param>
	/// <returns></returns>
	public static T[] SearchObjectsWithComponent<T>(Transform transformSearch, string objectName)
	{
		T[] returnArray;
		foreach (Transform trs in transformSearch)
		{
			if (trs.gameObject.name == objectName)
			{
				returnArray = new T[trs.childCount];
				for(int i = 0; i < returnArray.Length; i++)
				{
					returnArray[i] = trs.GetChild(i).GetComponent<T>();
				}
				return returnArray;
			}
		}
		return new T[0];
	}

	public static Vector2 Vector2Abs(Vector2 vector)
	{
		return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	public static Vector3 Vector3Abs(Vector3 vector)
	{
		return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}
}
