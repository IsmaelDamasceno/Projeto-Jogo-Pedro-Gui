using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class GroundDetection : MonoBehaviour
	{

		private int touchCount = 0;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			touchCount++;
			PlayerCore.grounded = true;
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			touchCount--;
			if (touchCount == 0)
			{
				PlayerCore.grounded = false;
			}
		}
	}
}
