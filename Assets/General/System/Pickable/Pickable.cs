using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estado base de um objeto dropado que pode ser pego
/// </summary>
public abstract class Pickable: BaseState
{
	// Está dentro do raio aceito para ser pego
	public bool hover;
	[SerializeField] protected LineRenderer lineRenderer;

	/// <summary>
	/// Função que executa ao pegar o item
	/// </summary>
	public abstract void PickUp();

	public override void Enter()
	{
		lineRenderer = GetComponent<LineRenderer>();
		LineHelper.DrawCircle(lineRenderer, 16, Pickup.PickupRadius);
	}
	public override void Step()
	{
		// Ativa e desativa o circulo em volta do objeto de acordo com o hover
		lineRenderer.enabled = hover;
	}
	public override void FixedStep()
	{

	}
	public override void Exit()
	{

	}
}
