using Godot;
using System;
using System.Collections.Generic;

public class JobListingBase : GridContainer
{
    [Signal] public delegate void SelectedAnswersChanged(bool increased);

    private Texture Icon;
    private String Description;
    private homeless correctApplicant;
    private bool selectedCorrect = false;
    private bool selectedAnAnswer = false;
    private String selectedDecriptionFormat;

    private OptionButton optionButton;
    public override void _Ready()
    {
        optionButton = GetNode<OptionButton>("OptionButton");
        Connect(nameof(SelectedAnswersChanged), GetNode("/root/Main/HBoxContainer/Computer"), "selectedAnswersChanged");
        
        String[] JobDescriptions = System.IO.File.ReadAllLines("Resources/jobDescriptions.tres");
        List<String> usedJobDescriptions = new List<string>();
        Random rand = new Random();
        selectedDecriptionFormat =JobDescriptions[rand.Next(JobDescriptions.Length)];

        String[] jobImageLocations = System.IO.File.ReadAllLines("Resources/jobList.tres");
        setIcon(GD.Load<Texture>("res://Art/Job/" + jobImageLocations[rand.Next(jobImageLocations.Length)] + ".png"));
    }

    public void setIcon(Texture icon)
    {
        Icon = icon;
        GetNode<TextureRect>("JobImage").Texture = Icon;
    }
    public void setDescription(String description)
    {
        Description = description;
        GetNode<RichTextLabel>("JobDescr").BbcodeText = Description;
    }
    public void setCorrectApplicant(homeless cApplicant)
    {
        correctApplicant = cApplicant;
        setDescription(correctApplicant.Job.Item1 + "\n" + String.Format(selectedDecriptionFormat, correctApplicant.Job.Item2[0], correctApplicant.Job.Item2[1], correctApplicant.Job.Item2[2]));
    }
    public homeless getCorrectApplicant()
    {
        return correctApplicant;
    }
    public void OnOptionButtonItemSelected(int selected)
    {
        selectedCorrect =optionButton.GetItemText(selected).Equals(correctApplicant.Name) ? true : false;
        if(!selectedAnAnswer && selected != 0)
        {
            EmitSignal(nameof(SelectedAnswersChanged), true);
        }
        else if(selectedAnAnswer && selected == 0)
        {
            EmitSignal(nameof(SelectedAnswersChanged), false);
        }
        selectedAnAnswer = selected != 0 ? true : false;
    }
    public bool SelectedAnAnswer()
    {
        return selectedAnAnswer;
    }
    public bool SelectedCorrect()
    {
        return selectedCorrect;
    }
}
