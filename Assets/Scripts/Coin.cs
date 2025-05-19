using System;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public int coinValue = 1;
	[ SerializeField ] private float shrinkAnimationDuration = 0.2f;

	private GameManager gameManagerRef;
	private Spawner spawnerRef;

	public void Initialize( Spawner spawner )
	{
		spawnerRef = spawner;
		gameManagerRef = GameManager.Instance;
		transform.DOScaleX( 0f, shrinkAnimationDuration ).SetLoops( -1, LoopType.Yoyo ).SetEase( Ease.InOutCubic );
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

	public int GetValue()
	{
		return coinValue;
	}
}