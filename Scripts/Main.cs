using Godot;
using System;
using System.Collections.Generic;

public class Main : Control
{
    [Export] private readonly int numHomeless;
    private List<homeless> homelessList;
    public override void _Ready()
    {
        homelessList = GenerateHomeless(numHomeless);
    }

    private List<homeless> GenerateHomeless(int numHomeless)
    {
        List<homeless> homelessListBuilder = new List<homeless>();
        for (int i = 0; i < numHomeless; i++)
        {
            homelessListBuilder.Add(new homeless());
        }
        return homelessListBuilder;
    }
}
