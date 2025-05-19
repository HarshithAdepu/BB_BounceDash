using System;
using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
	public float yOffset = 2f;

	[ Range( 0.1f, 1f ) ] [ SerializeField ]
	private float smoothTime = 0.3f;

	private Camera mainCam;
	private Transform killTrigger;
	private Vector3 screenEdgePosition;

	private Vector3 velocity;
	private Vector3 targetPosition = Vector3.zero;

	private GameManager gameManagerRef;

	private void Start()
	{
		mainCam = Camera.main;
		gameManagerRef = GameManager.Instance;
		killTrigger = transform.GetChild( 0 );
	}

	private void LateUpdate()
	{
		if ( gameManagerRef == null )
		{
			Debug.LogWarning( "GameManager not found!" );
			return;
		}

		targetPosition = new Vector3( transform.position.x, gameManagerRef.MaxHeightReached + yOffset, transform.position.z );
		transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref velocity, smoothTime );

		screenEdgePosition = mainCam.ViewportToWorldPoint( new Vector3( 0.5f, 0f, mainCam.nearClipPlane ) );
		killTrigger.position = screenEdgePosition;
	}
}