using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Random = UnityEngine.Random;

public enum EMoveDirection
{
	None,
	Horizontal,
	Vertical,
	Diagonal
}

public struct PlatformSpawnData
{
	public Vector3 spawnPosition;
	public float xScale;
	public float moveSpeed;
	public float moveDistance;
	public EMoveDirection moveDirection;
}

public class Spawner : MonoBehaviour
{
	public static Spawner Instance;

	[ SerializeField ] private Platform platformPrefab;
	[ SerializeField ] private float spawnDistance;
	[ SerializeField ] private float yVariance;
	[ SerializeField ] private Vector2 xOffsetRange;
	[ SerializeField ] private Vector2 scaleRange;

	[ Header( "Extras" ) ] [ Range( 0f, 1f ) ] [ SerializeField ]
	private float extrasSpawnProbability;

	[ SerializeField ] private float extrasSpawnHeight = 50f;

	[ SerializeField ] private RotatingBlade rotatingBladePrefab;
	[ SerializeField ] private Spikes spikesPrefab;
	[ SerializeField ] private Coin coinPrefab;
	[ SerializeField ] private Boost boostPrefab;

	private Vector3 lastPlatformLocation;
	private GameManager gameManagerRef;
	private PlatformSpawnData platformSpawnData;

	private List<Transform> spawnedItems = new List<Transform>();

	private void Awake()
	{
		if ( Instance != null )
		{
			Destroy( this );
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		gameManagerRef = GameManager.Instance;
		lastPlatformLocation = Vector3.zero;

		platformSpawnData = new PlatformSpawnData
							{
								spawnPosition = Vector3.zero,
								xScale = 1f,
								moveDirection = EMoveDirection.None,
								moveDistance = 2f,
								moveSpeed = 1.5f
							};
	}

	public void ClearLevel()
	{
		for ( int i = 0; i < spawnedItems.Count; i++ )
		{
			Destroy( spawnedItems[ i ].gameObject );
		}
	}

	private void FixedUpdate()
	{
		CheckSpawnConditions();
	}

	public void CheckSpawnConditions()
	{
		if ( lastPlatformLocation.y < gameManagerRef.MaxHeightReached + gameManagerRef.YBuffer )
		{
			SpawnPlatform();
		}
	}

	public void SpawnPlatform()
	{
		float xPos = Random.Range( xOffsetRange.x, xOffsetRange.y );
		float yPos = lastPlatformLocation.y + spawnDistance + Random.Range( -yVariance, yVariance );
		Vector3 spawnPosition = new Vector3( xPos, yPos, lastPlatformLocation.z );
		Platform spawnedPlatform = Instantiate( platformPrefab );
		spawnedItems.Add( spawnedPlatform.transform );

		platformSpawnData.spawnPosition = spawnPosition;
		platformSpawnData.moveDirection = RandomPlatformDirection();
		lastPlatformLocation = spawnedPlatform.Initialize( platformSpawnData, this );

		bool spawnExtra = Random.Range( 0.0f, 1.0f ) <= extrasSpawnProbability;
		if ( spawnExtra )
		{
			SpawnExtra();
		}
	}

	private EMoveDirection RandomPlatformDirection()
	{
		EMoveDirection direction = EMoveDirection.None;

		int choice = Random.Range( 0, 100 );

		if ( choice >= 0 && choice < 70 )
		{
			direction = EMoveDirection.None;
		}
		else if ( choice < 85 )
		{
			direction = EMoveDirection.Horizontal;
		}
		else if ( choice < 95 )
		{
			direction = EMoveDirection.Vertical;
		}
		else
		{
			direction = EMoveDirection.Diagonal;
		}

		return direction;
	}

	public void RemoveItem( Transform item )
	{
		spawnedItems.Remove( item );
	}

	private void SpawnExtra()
	{
		if ( gameManagerRef.MaxHeightReached < extrasSpawnHeight )
		{
			return;
		}

		int extraType = Random.Range( 0, 4 );
		switch ( extraType )
		{
			case 0:
				SpawnRotatingBlade();
				break;
			case 1:
				SpawnSpikes();
				break;
			case 2:
				SpawnCoin();
				break;
			case 3:
				SpawnBoost();
				break;
		}
	}

	private void SpawnRotatingBlade()
	{
		float xPos = Random.Range( xOffsetRange.x, xOffsetRange.y );
		float yPos = lastPlatformLocation.y + Random.Range( -yVariance, yVariance );
		Vector3 spawnPosition = new Vector3( xPos, yPos, lastPlatformLocation.z );

		RotatingBlade blade = Instantiate( rotatingBladePrefab, spawnPosition, Quaternion.identity );
		blade.Initialize( this );
		spawnedItems.Add( blade.transform );
	}

	private void SpawnCoin()
	{
		float xPos = Random.Range( xOffsetRange.x, xOffsetRange.y );
		float yPos = lastPlatformLocation.y + Random.Range( -yVariance, yVariance );
		Vector3 spawnPosition = new Vector3( xPos, yPos, lastPlatformLocation.z );

		Coin coin = Instantiate( coinPrefab, spawnPosition, Quaternion.identity );
		coin.Initialize( this );
		spawnedItems.Add( coin.transform );
	}

	private void SpawnBoost()
	{
		float xPos = Random.Range( xOffsetRange.x, xOffsetRange.y );
		float yPos = lastPlatformLocation.y + Random.Range( -yVariance, yVariance );
		Vector3 spawnPosition = new Vector3( xPos, yPos, lastPlatformLocation.z );

		Boost boost = Instantiate( boostPrefab, spawnPosition, Quaternion.identity );
		boost.Initialize( this );
		spawnedItems.Add( boost.transform );
	}

	private void SpawnSpikes()
	{
		Transform lastSpawnedObject = spawnedItems.Last();
		if ( !lastSpawnedObject.CompareTag( "Ground" ) )
		{
			Debug.LogWarning( "Last spawned object not platform!" );
			return;
		}

		float xPos = Random.Range( -0.5f, 0.5f );
		float yPos = 0.1f;
		Vector3 spawnPosition = new Vector3( xPos, yPos, lastPlatformLocation.z );

		Spikes spikes = Instantiate( spikesPrefab, lastSpawnedObject );
		spikes.transform.localPosition = spawnPosition;

		spikes.Initialize( this );
		spawnedItems.Add( spikes.transform );
	}
}