using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	private Spawner spawnerRef;

	public void Initialize( Spawner spawner )
	{
		spawnerRef = spawner;
	}

	private void OnDisable()
	{
		spawnerRef.RemoveItem( transform );
	}
}