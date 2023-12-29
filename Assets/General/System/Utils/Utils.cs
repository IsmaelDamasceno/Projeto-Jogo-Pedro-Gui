using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{

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
	public static T[] SearchObjectWithComponents<T>(Transform transformSearch, string objectName)
	{
		T[] returnArray;
		foreach (Transform trs in transformSearch)
		{
			if (trs.gameObject.name == objectName)
			{
				returnArray = new T[trs.childCount];
				for(int i = 0; i < returnArray.Length; i++)
				{
					returnArray[i] = trs.GetComponent<T>();
				}
				return returnArray;
			}
		}
		return new T[0];
	}
}
