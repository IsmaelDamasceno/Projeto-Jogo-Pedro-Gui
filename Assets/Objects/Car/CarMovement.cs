using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float carInitialSpeed;
    [SerializeField] private float carHeight;
    [SerializeField] private float leftCornerDistance;
    [SerializeField] private float rightCornerDistance;
    [SerializeField] private LineRenderer trackLine;
	[SerializeField] private float gravScale;
    [SerializeField] private float cameraHeight;

	private Vector2 carPosition;
    private Vector2 leftCorner;
    private Vector2 rightCorner;

    private Vector2 currentDirection;

    private bool grounded = true;
    private Rigidbody2D rb;

    private float yForce = 0f;

    void Start()
    {
		carPosition = transform.position;
        leftCorner = carPosition - leftCornerDistance * Vector2.right;
        rightCorner = carPosition + rightCornerDistance * Vector2.right;

        currentDirection = Vector2.right;

    }

    private void OnEnable()
    {
		CameraMovement.SetCameraHeight(cameraHeight, .5f);
		CameraMovement.SetTarget(transform);
	}

    void Update()
    {
        float groundHeight = Utils.LineGetYBasedOnX(trackLine, carPosition.x);

		leftCorner = carPosition - leftCornerDistance * currentDirection;
		rightCorner = carPosition + rightCornerDistance * currentDirection;

		float leftHeight = Utils.LineGetYBasedOnX(trackLine, leftCorner.x);
		float rightHeight = Utils.LineGetYBasedOnX(trackLine, rightCorner.x);

		leftCorner.y = leftHeight;
		rightCorner.y = rightHeight;

		currentDirection = new Vector2(
			rightCorner.x - leftCorner.x, rightHeight - leftHeight
		).normalized;

		if (grounded)
        {
			float height = groundHeight + carHeight;
			carPosition.y = height;

			float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);

			carPosition += carInitialSpeed * Time.deltaTime * currentDirection;
		}
        else {
			float gravity = -9.81f * gravScale * Time.deltaTime;

            yForce += gravity;

			carPosition += new Vector2(carInitialSpeed, yForce) * Time.deltaTime;
			if (groundHeight > carPosition.y - carHeight - yForce * Time.deltaTime && yForce < 0f)
            {
                Debug.Log("Grounded");
                carPosition.y = groundHeight + carHeight;
                yForce = 0f;
                grounded = true;
			}
        }
		transform.position = carPosition;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = false;
        yForce = collision.GetComponent<TrackJump>().jumpForce;
	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(leftCorner, .1f);
		Gizmos.DrawSphere(carPosition, .1f);
		Gizmos.DrawSphere(rightCorner, .1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(carPosition - currentDirection, carPosition + currentDirection);
	}
#endif
}
