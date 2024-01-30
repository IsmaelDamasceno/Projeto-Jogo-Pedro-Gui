using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class GroundDetection : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			PlayerCore.grounded = true;
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			PlayerCore.grounded = false;
		}
	}
}
