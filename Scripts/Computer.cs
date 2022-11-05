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
    List<String> Questions = new List<String>();
    //Stores todays applicants
    List<homeless> ApplicantsList;
    int currentApplicant = 0;
    int currentQuestionSet = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LogText = GetNode<RichTextLabel>("Browser/Log/VBoxContainer/ScrollContainer/LogText");
        ButtonA = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionA");
        ButtonB = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionB");
        ButtonC = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionC");

        //Generate and set the jobs listings
    }

    // Adds text and formats it depending on if Player or not.
    
    void setApplicantList(List<homeless> applicants){
        ApplicantsList = applicants;
        greetingConversation();
    }

    void greetingConversation(){
        addTextToLog(Username, "Hello, Welcome to No More Drifting! \n What is your name?");
        addTextToLog(ApplicantsList[currentApplicant].Name, "My name is " + ApplicantsList[currentApplicant].Name);
        addTextToLog(Username, "Ok lets get to find you a sutiable job " + ApplicantsList[currentApplicant].Name);
        addPersonToApplicatentList(ApplicantsList[currentApplicant].Name);
        askQuestions();     
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
    public void askQuestions()
    {
        LogText.AppendBbcode("[right][u]---Ask one of the following questions---[/u][/right]\n");
        for(int i = 3* currentQuestionSet; i < currentQuestionSet * 3 + 3; i++){
            GD.Print(ApplicantsList[currentApplicant].QuestionList.Count);
            Questions.Add(ApplicantsList[currentApplicant].QuestionList[i].Item1);
            LogText.AppendBbcode("[right][color=blue]"+ApplicantsList[currentApplicant].QuestionList[i].Item1+"[/color][/right]\n");
        }
        waitForButtonClick();
        currentQuestionSet++;
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
        addTextToLog(Username, Questions[button]);
        addTextToLog(ApplicantsList[currentApplicant].Name, ApplicantsList[currentApplicant].QuestionList[button].Item2[0]);
        if(currentQuestionSet < 3){
            askQuestions();
        }else{
            addTextToLog(Username, "Thank You we will let you know if we find a job in a few hours \n\n Next!");
            currentQuestionSet++;
            currentApplicant++;
            greetingConversation();
        }
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
