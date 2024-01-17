using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// C�digo de parallax do fundo, esse componente � adicionado a cada camada que possui parallax
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    /// <summary>
    /// O n�vel de paralalx da camada
    /// </summary>
    [SerializeField] private float parallaxMultiplier;

    /// <summary>
    /// Vari�veis para trackear o movimento da c�mera
    /// </summary>
    private Vector2 currentPos, lastPos, posDifference;

    /// <summary>
    /// Transform da c�mera
    /// </summary>
    private new Transform camera;

    void Start()
    {
        camera = Camera.main.transform;
        currentPos = lastPos = camera.position;
        posDifference = Vector2.zero;
    }

    void LateUpdate()
    {
		currentPos = camera.position;
		posDifference = currentPos - lastPos;
        lastPos = currentPos;

        // Move a layer usando a f�rmula (m * (1f - p))
        // m = vector representando movimento da c�mera no frame espec�fico
        // p = n�vel de parallax da layer
        transform.Translate(posDifference * (1f - parallaxMultiplier), Space.Self);
	}
}
