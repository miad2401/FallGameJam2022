using Godot;
using System;

public class StartingMenu : Control
{
	private void OnStartButtonPressed()
	{
		//GetTree().ChangeScene("res://Scenes/");
	}

	private void OnSettingsButtonPressed()
	{
		//Open Settings
	}
	private void OnCreditsButtonPressed()
	{
		//Open Credits
	}

	private void OnExitButtonPressed()
	{
		// quit game
		GetTree().Quit();
	}
}
