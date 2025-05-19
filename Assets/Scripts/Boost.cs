using System;
using DG.Tweening;
using UnityEngine;

public class Boost : MonoBehaviour
{
	[ SerializeField ] private float boostAmount = 50f;

	[ SerializeField ] private float scaleAnimationDuration = 0.5f;
	[ SerializeField ] private float scaleAmount = 1.5f;

	private GameManager gameManagerRef;
	private Spawner spawnerRef;

	public float GetBoostAmount()
	{
		return boostAmount;
	}

	public void Initialize( Spawner spawner )
	{
		spawnerRef = spawner;
		gameManagerRef = GameManager.Instance;
		transform.DOScale( new Vector3( scaleAmount, scaleAmount, scaleAmount ), scaleAnimationDuration ).SetEase( Ease.InSine ).SetLoops( -1, LoopType.Yoyo );
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