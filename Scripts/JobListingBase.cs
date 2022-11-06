using Godot;
using System;

public class JobListingBase : GridContainer
{
    private Texture Icon;
    private String Description;
    private homeless correctApplicant;
    private bool selectedCorrect = false;
    private bool selectedAnAnswer = false;

    public override void _Ready()
    {
    }

    public void setIcon(Texture icon)
    {
        Icon = icon;
    }
    public void setDescription(String description)
    {
        Description = description;
    }
    public void setCorrectApplicant(homeless cApplicant)
    {
        correctApplicant = cApplicant;
    }
    public void OnOptionButtonItemSelected(int selected)
    {
        selectedCorrect = GetNode<OptionButton>("OptionButton").GetItemText(selected).Equals(correctApplicant.Name) ? true : false;
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
