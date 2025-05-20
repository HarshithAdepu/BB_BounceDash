using System;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	[ SerializeField ] private float moveSpeed;
	private Vector2 targetPosition;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		if ( Input.touchCount > 0 )
		{
			Touch touch = Input.GetTouch( 0 );
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint( touch.position );

			// Only move along X, keep Y and Z the same
			targetPosition = new Vector2( touchPosition.x, rb.position.y );

			// Smooth movement with Rigidbody2D
			Vector2 newPosition = Vector2.MoveTowards( rb.position, targetPosition, moveSpeed * Time.deltaTime );
			rb.MovePosition( newPosition );
		}
	}
}