using Godot;
using System;

public class JobListingBase : GridContainer
{
    [Signal] public delegate void SelectedAnswersChanged(bool increased);

    private Texture Icon;
    private String Description;
    private homeless correctApplicant;
    private bool selectedCorrect = false;
    private bool selectedAnAnswer = false;

    private OptionButton optionButton;
    public override void _Ready()
    {
        optionButton = GetNode<OptionButton>("OptionButton");
        Connect(nameof(SelectedAnswersChanged), GetNode("/root/Main/HBoxContainer/Computer"), "selectedAnswersChanged");
    }

    public void setIcon(Texture icon)
    {
        Icon = icon;
    }
    public void setDescription(String description)
    {
        Description = description;
        GetNode<RichTextLabel>("JobDescr").BbcodeText = Description;
    }
    public void setCorrectApplicant(homeless cApplicant)
    {
        correctApplicant = cApplicant;
        setDescription(correctApplicant.Name);
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
