using Godot;
using System;
using System.Collections.Generic;

public class Credits : Node2D
{
    void _on_MainButton_pressed(){
        GetTree().ChangeScene("res://Scenes/StartingMenu.tscn");
    }

    List<Slider> sliderList = new List<Slider>();

    float MusicVolumeDb;

    AudioStreamPlayer MusicPlayer;
    public override void _Ready()
    {
        MusicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");

        loadSoundSettings("res://Sounds/soundSettings.tres");
    }
    public void loadSoundSettings(String filePath)
    {
        soundSettings soundSettings = ResourceLoader.Load<soundSettings>(filePath, null, true);
        MusicVolumeDb = soundSettings.MusicVolumeDb;
        MusicPlayer.VolumeDb = MusicVolumeDb;
        MusicPlayer.Play();
    }
}
