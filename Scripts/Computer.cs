using Godot;
using System;

public class Computer : Control
{
    //Stores Log Text Rick Text Label Refrence
    RichTextLabel LogText;
    //Stores Option Buttons Refrences
    Button ButtonA, ButtonB, ButtonC;
    //Stories current questions
    String[] Questions;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LogText = GetNode<RichTextLabel>("Browser/Log/VBoxContainer/LogText");
        ButtonA = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionA");
        ButtonB = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionB");
        ButtonC = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionC");

        addTextToLog("Social Worker", "Cookie");
        addTextToLog("No", "No Cookie");
        Questions = new String[] {"How are you?", "Do you like cookies?", "Go Away?"};
        askQuestions(Questions);
        addTextToLog("No", "No Cooasdgf");
    }

    // //Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {

    // }

    // Adds text and formats it depending on if Player or not.
    public void addTextToLog(String name, String text)
    {
        //First line prints name
        //Second prints the given text
        if (name == "Social Worker")
        {
            LogText.AppendBbcode("[right][u][color=blue]" + name + ":[/color][/u][/right]");
            LogText.AppendBbcode("[right][color=blue]" + text + "[/color][/right]\n");
        }
        else
        {
            LogText.AppendBbcode("[u]" + name + ":[/u]\n");
            LogText.AppendBbcode(text + "\n");
        }
    }

    //Clears previous questions and adds new questions
    public void askQuestions(String[] questions)
    {
        Questions = questions;
        LogText.AppendBbcode("[right][u]---Ask one of the following questions---[/u][/right]\n");
        foreach(String question in questions){
            LogText.AppendBbcode("[right][color=blue]"+question+"[/color][/right]\n");
        }
        waitForButtonClick();
    }

    //Code for when game is waiting for user to selected a question
    public void waitForButtonClick(){
        ButtonA.Disabled = false;
        ButtonB.Disabled = false;
        ButtonC.Disabled = false;
    }

    void _on_OptionButton_pressed(int button){
        
        ButtonA.Disabled = true;
        ButtonB.Disabled = true;
        ButtonC.Disabled = true;

        addTextToLog("Social Worker", Questions[button-1]);
    }
}
