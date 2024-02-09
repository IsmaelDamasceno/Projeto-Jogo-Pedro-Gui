using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class GroundDetection : MonoBehaviour
	{

		public static int touchCount = 0;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			touchCount++;
			PlayerCore.grounded = true;
			Debug.Log($"Player Collision deteced with {collision.gameObject.name}, player is now in the ground");
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			touchCount--;
			if (touchCount == 0)
			{
				Debug.Log("player is not on the ground anymore");
				PlayerCore.grounded = false;
			}
		}
	}
}
