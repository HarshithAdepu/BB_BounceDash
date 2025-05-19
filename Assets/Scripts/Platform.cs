using System;
using DG.Tweening;
using UnityEngine;

public class Platform : MonoBehaviour
{
	private float moveSpeed;
	private float moveDistance;
	private EMoveDirection moveDirection;

	private Vector3 startPosition;
	private Vector3 endPosition;

	private Spawner spawnerRef;
	private GameManager gameManagerRef;

	private void Start()
	{
		gameManagerRef = GameManager.Instance;
	}

	public Vector3 Initialize( PlatformSpawnData spawnData, Spawner spawner )
	{
		transform.position = spawnData.spawnPosition;
		transform.localScale = new Vector3( spawnData.xScale, transform.localScale.y, transform.localScale.z );

		moveSpeed = spawnData.moveSpeed;
		moveDistance = spawnData.moveDistance;
		moveDirection = spawnData.moveDirection;

		spawnerRef = spawner;

		startPosition = transform.position;

		switch ( moveDirection )
		{
			case EMoveDirection.None:
				endPosition = startPosition;
				break;
			case EMoveDirection.Horizontal:
				endPosition = startPosition + new Vector3( Mathf.Sign( startPosition.x ) * -moveDistance, 0f, 0f );
				break;
			case EMoveDirection.Vertical:
				endPosition = startPosition + new Vector3( 0f, moveDistance, 0f );
				break;
			case EMoveDirection.Diagonal:
				endPosition = startPosition + new Vector3( Mathf.Sign( startPosition.x ) * -moveDistance, moveDistance, 0f );
				break;
		}

		transform.DOMove( endPosition, moveSpeed ).SetEase( Ease.Linear ).SetSpeedBased( true ).SetLoops( -1, LoopType.Yoyo );

		return endPosition;
	}

	private void FixedUpdate()
	{
		if ( spawnerRef == null || gameManagerRef == null )
		{
			return;
		}

		if ( transform.position.y < gameManagerRef.MaxHeightReached - gameManagerRef.YBuffer )
		{
			Destroy( gameObject );
		}
	}

	private void OnDisable()
	{
		transform.DOKill();
		spawnerRef.RemoveItem( transform );
	}
}