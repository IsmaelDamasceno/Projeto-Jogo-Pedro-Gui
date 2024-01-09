using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimation : MonoBehaviour
{
	[SerializeField] private float upTime;
	[SerializeField] private float cooldownTime;

	private Animator animator;
	IEnumerator CooldwonCoroutine()
	{
		yield return new WaitForSeconds(cooldownTime);
		animator.SetBool("Active", true);
		StartCoroutine(UpCoroutine());
	}
	IEnumerator UpCoroutine()
	{
		yield return new WaitForSeconds(upTime);
		animator.SetBool("Active", false);
		StartCoroutine(CooldwonCoroutine());
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		StartCoroutine(CooldwonCoroutine());
	}
}
