using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GameUI : MonoBehaviour
{
	[ SerializeField ] private RectTransform hud;
	[ SerializeField ] private RectTransform pauseMenu;
	[ SerializeField ] private RectTransform restartMenu;

	[ SerializeField ] private Button hudPauseButton;
	[ SerializeField ] private TMP_Text hudScoreText;
	[ SerializeField ] private TMP_Text hudCoinText;

	[ SerializeField ] private Button pauseMenuResumeButton;
	[ SerializeField ] private Button pauseMenuRestartButton;
	[ SerializeField ] private Button pauseMenuExitButton;

	[ SerializeField ] private TMP_Text restartMenuScoreText;
	[ SerializeField ] private Button restartMenuRestartButton;
	[ SerializeField ] private Button restartMenuExitButton;

	public static GameUI Instance;

	private bool isScoreAnimating = false;

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

	private void OnEnable()
	{
		hudPauseButton.onClick.AddListener( hudPauseButton_OnClick );
		pauseMenuResumeButton.onClick.AddListener( pauseMenuResumeButton_OnClick );
		pauseMenuRestartButton.onClick.AddListener( pauseMenuRestartButton_OnClick );
		pauseMenuExitButton.onClick.AddListener( pauseMenuExitButton_OnClick );
		restartMenuRestartButton.onClick.AddListener( restartMenuRestartButton_OnClick );
		restartMenuExitButton.onClick.AddListener( restartMenuExitButton_OnClick );
	}

	private void OnDisable()
	{
		hudPauseButton.onClick.RemoveListener( hudPauseButton_OnClick );
		pauseMenuResumeButton.onClick.RemoveListener( pauseMenuResumeButton_OnClick );
		pauseMenuRestartButton.onClick.RemoveListener( pauseMenuRestartButton_OnClick );
		pauseMenuExitButton.onClick.RemoveListener( pauseMenuExitButton_OnClick );
		restartMenuRestartButton.onClick.RemoveListener( restartMenuRestartButton_OnClick );
		restartMenuExitButton.onClick.RemoveListener( restartMenuExitButton_OnClick );
	}

	private void AnimateText( TMP_Text textComponent )
	{
		if ( isScoreAnimating )
		{
			return;
		}

		isScoreAnimating = true;

		Transform textTransform = textComponent.transform;

		Sequence seq = DOTween.Sequence();
		seq.Append( textTransform.DOScale( 1.4f, 0.1f ).SetEase( Ease.InBack, 1f ) )
		   .Append( textTransform.DOScale( 1f, 0.2f ).SetEase( Ease.OutBack, 3f ) )
		   .OnComplete( () => isScoreAnimating = false );
	}

	public void UpdateScore( float score )
	{
		hudScoreText.text = score.ToString();
		AnimateText( hudScoreText );
	}

	public void UpdateCoins( float coins )
	{
		hudCoinText.text = coins.ToString();
		AnimateText( hudCoinText );
	}

	private void hudPauseButton_OnClick()
	{
		GameManager.Instance.PauseGame();

		pauseMenu.gameObject.SetActive( true );
	}

	private void pauseMenuResumeButton_OnClick()
	{
		GameManager.Instance.ResumeGame();

		pauseMenu.gameObject.SetActive( false );
	}

	private void pauseMenuRestartButton_OnClick()
	{
		GameManager.Instance.RestartGame();
	}

	private void pauseMenuExitButton_OnClick()
	{
		GameManager.Instance.ExitToMenu();
	}

	private void restartMenuRestartButton_OnClick()
	{
		GameManager.Instance.RestartGame();
	}

	private void restartMenuExitButton_OnClick()
	{
		GameManager.Instance.ExitToMenu();
	}
}