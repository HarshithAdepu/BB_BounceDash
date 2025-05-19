using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	[ SerializeField ] private RectTransform mainMenuPanel;

	[ SerializeField ] private Button playButton;
	[ SerializeField ] private Button storeButton;
	[ SerializeField ] private Button exitButton;

	private void OnEnable()
	{
		playButton.onClick.AddListener( playButton_OnClick );
		storeButton.onClick.AddListener( storeButton_OnClick );
		exitButton.onClick.AddListener( exitButton_OnClick );
	}

	private void OnDisable()
	{
		playButton.onClick.RemoveListener( playButton_OnClick );
		storeButton.onClick.RemoveListener( storeButton_OnClick );
		exitButton.onClick.RemoveListener( exitButton_OnClick );
	}

	private void playButton_OnClick()
	{
		SceneManager.LoadScene( "GameScene" );
	}

	private void storeButton_OnClick() { }

	private void exitButton_OnClick()
	{
		Application.Quit();
	}
}