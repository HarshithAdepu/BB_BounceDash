using System;
using DG.Tweening;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
	[ SerializeField ] private float rotateDuration;

	private Vector3 fullRotation = new Vector3( 0, 0, 360 );
	private GameManager gameManagerRef;
	private Spawner spawnerRef;

	public void Initialize( Spawner spawner )
	{
		spawnerRef = spawner;
		transform.DORotate( fullRotation, rotateDuration, RotateMode.FastBeyond360 ).SetLoops( -1 ).SetEase( Ease.Linear );
		gameManagerRef = GameManager.Instance;
	}

	private void FixedUpdate()
	{
		if ( transform.position.y < gameManagerRef.MaxHeightReached - gameManagerRef.YBuffer )
		{
			Destroy( gameObject );
		}
	}

	private void OnDisable()
	{
		spawnerRef.RemoveItem( transform );
		transform.DOKill();
	}
}