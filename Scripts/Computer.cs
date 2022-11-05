using Godot;
using System;
using System.Collections.Generic;

public class Computer : Control
{

    [Export] String Username;

    //Stores Log Text Rick Text Label Refrence
    RichTextLabel LogText;
    //Stores Option Buttons Refrences
    Button ButtonA, ButtonB, ButtonC;
    //Stores current questions
    String[] Questions;
    //Stores todays applicants
    List<homeless> ApplicantsList;
    int currentApplicant = 0;
    int chatPhase = 1;
    String[] questions32 = new String[] {"A","B", "C"};

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LogText = GetNode<RichTextLabel>("Browser/Log/VBoxContainer/ScrollContainer/LogText");
        ButtonA = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionA");
        ButtonB = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionB");
        ButtonC = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionC");

        //Generate and set the jobs listings
    }

     //Called every frame. 'delta' is the elapsed time since the previous frame.
     public override void _Process(float delta)
     {
        if(currentApplicant < ApplicantsList.Count){
            if(chatPhase == 1){
                GD.Print(ApplicantsList.Count);
                //Move character in
                addTextToLog(Username, "Hello, Welcome to No More Drifting! \n What is your name?");
                addTextToLog(ApplicantsList[currentApplicant].Name, "My name is " + ApplicantsList[currentApplicant].Name);
                addTextToLog(Username, "Ok lets get to find you a sutiable job " + ApplicantsList[currentApplicant].Name);
                addPersonToApplicatentList(ApplicantsList[currentApplicant].Name);

                    askQuestions(questions32);
                    //Give three questions
                    //wait for user to pick one
                    //Get response
                    
                addTextToLog(Username, "Thank You we will let you know if we find a job in a few hours");
                currentApplicant++;
            }
        }
     }

    // Adds text and formats it depending on if Player or not.
    
    void setApplicantList(List<homeless> applicants){
        ApplicantsList = applicants;
    }

    void Conversation(){

    }

    public void addTextToLog(String name, String text)
    {
        //First line prints name
        //Second prints the given text
        if (name == Username)
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
        addTextToLog("Social Worker", Questions[button]);
    }

    void addPersonToApplicatentList(String name){
        for(int i = 1; i <= 6; i++){
            GetNode<OptionButton>("Browser/Jobs/VBoxContainer/JobListing" + i +"/OptionButton").AddItem(name);
        }
    }

    void changeJobListingDescription(String desc, int index){
        RichTextLabel listingDescr = GetNode<RichTextLabel>("Browser/Jobs/VBoxContainer/JobListing" + index +"/JobDescr");
        listingDescr.Clear();
        listingDescr.AppendBbcode(desc);
    }

    void changeMusicVolume(float value){
        GetNode<AudioStreamPlayer>("../../AudioStreamPlayer").VolumeDb = value;
    }
}
