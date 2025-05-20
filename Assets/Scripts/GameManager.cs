using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[ SerializeField ] private float delayAfterDeath = 1f;
	public Vector2 screenBoundsXAxis = Vector2.zero;
	private int coinsOwned = 0;
	[ field: SerializeField ] public float YBuffer { get; private set; }

	private float maxHeightReached;
	private GameUI gameUIRef;

	public int MaxHeightReached
	{
		get { return ( int )maxHeightReached; }
		set
		{
			maxHeightReached = value;
			UpdateScoreUI();
		}
	}

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
		gameUIRef = GameUI.Instance;
		Time.timeScale = 1;

		if ( PlayerPrefs.HasKey( "CoinsOwned" ) )
		{
			coinsOwned = PlayerPrefs.GetInt( "CoinsOwned" );
		}
		else
		{
			PlayerPrefs.SetInt( "CoinsOwned", 0 );
		}

		Debug.Log( "Coins Owned" + coinsOwned );
		gameUIRef.UpdateCoins( coinsOwned );
		gameUIRef.UpdateScore( MaxHeightReached );

		CalculateScreenBounds();

		Application.targetFrameRate = 90;
	}

	public void AddCoins( int value )
	{
		coinsOwned += value;
		gameUIRef.UpdateCoins( coinsOwned );
	}

	public void PlayerDied()
	{
		PlayerPrefs.SetInt( "CoinsOwned", coinsOwned );
		Invoke( nameof(ShowRestartScreen), delayAfterDeath );
	}

	private void ShowRestartScreen()
	{
		gameUIRef.PlayerDied( MaxHeightReached );
	}

	private void CalculateScreenBounds()
	{
		screenBoundsXAxis.x = Camera.main.ViewportToWorldPoint( new Vector3( 0.0f, 0f, Camera.main.nearClipPlane ) ).x;
		screenBoundsXAxis.y = Camera.main.ViewportToWorldPoint( new Vector3( 1.0f, 1.0f, Camera.main.nearClipPlane ) ).x;
	}

	private void UpdateScoreUI()
	{
		gameUIRef.UpdateScore( maxHeightReached );
	}

	public void RestartGame()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
	}

	public void ExitToMenu()
	{
		SceneManager.LoadScene( "MainMenu" );
	}
}