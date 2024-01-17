using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Fun��es �teis
/// </summary>
public static class Utils
{
	/// <summary>
	/// Procura por um objeto filho de um transform usando seu nome
	/// </summary>
	/// <param name="transformSearch">Todos os filhos desse transform ser�o analisados, procurando o GameObject espec�fico</param>
	/// <param name="objectName">Nome do objeto que sera procurado nos filhos do transform</param>
	/// <returns></returns>
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

	/// <summary>
	/// Retorna o valor absoluto de um Vector2
	/// </summary>
	/// <param name="vector">Vector para executar o abs</param>
	/// <returns></returns>
	public static Vector2 Vector2Abs(Vector2 vector)
	{
		return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
	}

	/// <summary>
	/// Retorna o valor absoluto de um Vector3
	/// </summary>
	/// <param name="vector">Vector para executar o abs</param>
	/// <returns></returns>
	public static Vector3 Vector3Abs(Vector3 vector)
	{
		return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
	}
}
