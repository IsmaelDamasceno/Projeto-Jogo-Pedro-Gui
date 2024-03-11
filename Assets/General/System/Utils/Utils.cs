using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Funções úteis
/// </summary>
public static class Utils
{
	/// <summary>
	/// Procura por um objeto filho de um transform usando seu nome
	/// </summary>
	/// <param name="transformSearch">Todos os filhos desse transform serão analisados, procurando o GameObject específico</param>
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
					if (trs.GetChild(i).TryGetComponent(out T component)) { 
						returnArray[i] = component;
					}
					else
					{
						returnArray[i] = default;
					}
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

	public static float LineGetYBasedOnX(LineRenderer line, float xPosition)
	{
		for(int i = 0; i < line.positionCount; i++)
		{
			Vector3 point = line.GetPosition(i);
			Vector3 nextPoint = point;
			if (i < line.positionCount - 1)
			{
				nextPoint = line.GetPosition(i + 1);
			}

			if (point.x <= xPosition && nextPoint.x >= xPosition)
			{
				if (point.y == nextPoint.y)
				{
					return point.y;
				}
				else
				{
					float totalDistance = nextPoint.x - point.x;
					float distance = xPosition - point.x;
					float percent = Mathf.Clamp(distance / totalDistance, 0f, 1f);
					float y = Mathf.Lerp(point.y, nextPoint.y, percent);

					return y;
				}
			}
		}

		return -100f;
	}
}
