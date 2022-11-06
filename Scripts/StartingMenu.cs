using Godot;
using System;
using System.Collections.Generic;

public class StartingMenu : Control
{
	Panel SettingsMenu;

	Slider MusicSlider;
	Slider WhiteNoiseSlider;
	Slider SoundEffectsSlider;

	List<Slider> sliderList = new List<Slider>();

	float MusicVolumeDb;
	float WhiteNoiseVolumeDb;
	float SoundEffectsVolumeDb;

	AudioStreamPlayer MusicPlayer;
	AudioStreamPlayer SoundEffectsPlayer;
	AudioStreamPlayer WhiteNoisePlayer;
	public override void _Ready()
    {
		SettingsMenu = GetNode<Panel>("Settings");
		SettingsMenu.Visible = false;

		MusicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
		SoundEffectsPlayer = GetNode<AudioStreamPlayer>("SoundEffectsPlayer");
		WhiteNoisePlayer = GetNode<AudioStreamPlayer>("WhiteNoisePlayer");

		sliderList.Add(MusicSlider = SettingsMenu.GetNode<Slider>("SliderHolder/MusicSlider"));
		sliderList.Add(WhiteNoiseSlider = SettingsMenu.GetNode<Slider>("SliderHolder/WhiteNoiseSlider"));
		sliderList.Add(SoundEffectsSlider = SettingsMenu.GetNode<Slider>("SliderHolder/SoundEffectsSlider"));

		foreach	(Slider slider in sliderList)
        {
			slider.MinValue = 0.0001;
			slider.MaxValue = 1.5;
			slider.Step = 0.0001;
			slider.Value = 0.5;
        }

		loadSoundSettings("res://Sounds/soundSettings.tres");
	}
	private void OnStartButtonPressed()
	{
		var soundSettings = GD.Load<CSharpScript>("res://Scripts/soundSettings.cs").New(MusicVolumeDb, MusicSlider.Value, WhiteNoiseVolumeDb, WhiteNoiseSlider.Value, SoundEffectsVolumeDb, SoundEffectsSlider.Value);
		GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

    private void OnSettingsButtonPressed()
    {
		SettingsMenu.Visible = true;
		MusicPlayer.Stop();
    }
    private void OnCreditsButtonPressed()
	{
        GetTree().ChangeScene("res://Scenes/Credits.tscn");
        //Open Credits
    }

	private void OnExitButtonPressed()
	{
		// quit game
		GetTree().Quit();
	}

	private void OnExitSettingsButtonPressed()
    {
		var soundSettings = GD.Load<CSharpScript>("res://Scripts/soundSettings.cs").New(MusicVolumeDb, MusicSlider.Value, WhiteNoiseVolumeDb, WhiteNoiseSlider.Value, SoundEffectsVolumeDb, SoundEffectsSlider.Value);
		ResourceSaver.Save("res://Sounds/soundSettings.tres", soundSettings as soundSettings);
		SettingsMenu.Visible=false;
		MusicPlayer.Play();
    }

	private void OnSoundSliderValueChanged(float linearValue, String name)
    {
		switch (name)
        {
			case "Music":
				MusicVolumeDb = GD.Linear2Db(linearValue);
				MusicPlayer.VolumeDb = MusicVolumeDb;
				break;
			case "WhiteNoise":
				WhiteNoiseVolumeDb = GD.Linear2Db(linearValue);
				WhiteNoisePlayer.VolumeDb = WhiteNoiseVolumeDb;
				break;
			case "SoundEffects":
				SoundEffectsVolumeDb = GD.Linear2Db(linearValue);
				SoundEffectsPlayer.VolumeDb = SoundEffectsVolumeDb;
				break;
		}
    }

	private void OnSliderDragStarted(String name)
    {
		switch (name)
		{
			case "Music":
				MusicPlayer.Play();
				break;
			case "WhiteNoise":
				WhiteNoisePlayer.Play();
				break;
			case "SoundEffects":
				SoundEffectsPlayer.Play();
				break;
		}
	}

	private void OnSliderDragEnded(bool changed, String name)
	{
		switch (name)
		{
			case "Music":
				MusicPlayer.Stop();
				break;
			case "WhiteNoise":
				WhiteNoisePlayer.Stop();
				break;
			case "SoundEffects":
				SoundEffectsPlayer.Stop();
				break;
		}
	}

	public void loadSoundSettings(String filePath)
    {
		soundSettings soundSettings = ResourceLoader.Load<soundSettings>(filePath, null, true);
		MusicVolumeDb = soundSettings.MusicVolumeDb;
		MusicPlayer.VolumeDb = MusicVolumeDb;
		MusicSlider.Value = soundSettings.MusicSliderValue;
		WhiteNoiseVolumeDb = soundSettings.WhiteNoiseVolumeDb;
		WhiteNoisePlayer.VolumeDb = WhiteNoiseVolumeDb;
		WhiteNoiseSlider.Value = soundSettings.WhiteNoiseSliderValue;
		SoundEffectsVolumeDb = soundSettings.SoundEffectsVolumeDb;
		SoundEffectsPlayer.VolumeDb = SoundEffectsVolumeDb;
		SoundEffectsSlider.Value = soundSettings.SoundEffectsSliderValue;
		MusicPlayer.Play();
	}
}
