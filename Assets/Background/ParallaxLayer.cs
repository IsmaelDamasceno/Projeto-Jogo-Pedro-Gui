using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Código de parallax do fundo, esse componente é adicionado a cada camada que possui parallax
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    /// <summary>
    /// O nível de paralalx da camada
    /// </summary>
    [SerializeField] private float parallaxMultiplier;

    /// <summary>
    /// Variáveis para trackear o movimento da câmera
    /// </summary>
    private Vector2 currentPos, lastPos, posDifference;

    /// <summary>
    /// Transform da câmera
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

        // Move a layer usando a fórmula (m * (1f - p))
        // m = vector representando movimento da câmera no frame específico
        // p = nível de parallax da layer
        transform.Translate(posDifference * (1f - parallaxMultiplier), Space.Self);
	}
}
