using Godot;
using System;
using System.Collections.Generic;

public class soundSettings : Resource
{
    [Export]
    public float MusicVolumeDb { get; set; }

    [Export]
    public double MusicSliderValue { get; set; }

    [Export]
    public float WhiteNoiseVolumeDb { get; set; }

    [Export]
    public double WhiteNoiseSliderValue { get; set; }

    [Export]
    public float SoundEffectsVolumeDb { get; set; }

    [Export]
    public double SoundEffectsSliderValue { get; set; }

    public soundSettings()
    {
        MusicVolumeDb = 0.5f;
        MusicSliderValue = 0.5;
        WhiteNoiseVolumeDb = 0.5f;
        WhiteNoiseSliderValue = 0.5;
        SoundEffectsVolumeDb = 0.5f;
        SoundEffectsSliderValue = 0.5;
    }
    public soundSettings(float musicVolumeDb, double musicSliderValue, float whiteNoiseVolumeDb, double whiteNoiseSliderdouble ,float soundEffectsVolumeDb, double soundEffectsSliderValue)
    {
        MusicVolumeDb = musicVolumeDb;
        MusicSliderValue = musicSliderValue;
        WhiteNoiseVolumeDb = whiteNoiseVolumeDb;
        WhiteNoiseSliderValue = whiteNoiseSliderdouble;
        SoundEffectsVolumeDb = soundEffectsVolumeDb;
        SoundEffectsSliderValue = soundEffectsSliderValue;
    }
}
