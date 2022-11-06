using Godot;
using System;
using System.Collections.Generic;

public class Computer : Control
{
    [Signal] public delegate void moveApplicant(homeless application, bool entered);

    Tabs Settings;

    Slider MusicSlider;
    Slider WhiteNoiseSlider;
    Slider SoundEffectsSlider;
    List<Slider> sliderList = new List<Slider>();

    int prevTab = 0;

    float MusicVolumeDb;
    float WhiteNoiseVolumeDb;
    float SoundEffectsVolumeDb;

    AudioStreamPlayer MusicPlayer;
    AudioStreamPlayer SoundEffectsPlayer;
    AudioStreamPlayer WhiteNoisePlayer;

    String Username = "Mist";

    TextEdit userNameTextEdit;
    private int numAnswers = 0;

    //Stores Log Text Rick Text Label Refrence
    RichTextLabel LogText;
    //Stores Option Buttons Refrences
    Button ButtonA, ButtonB, ButtonC;
    //Stores current questions
    List<String> Questions = new List<String>();
    //Stores todays applicants
    List<homeless> ApplicantsList;
    int[] homelessAnswerSheet = new int[6];
    int currentApplicant = 0;
    int currentQuestionSet = 0;
    Boolean requiredTabSwitch = false;
    bool finished = false;

    int score = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect(nameof(moveApplicant), GetNode("/root/Main"), "moveApplicant");
        userNameTextEdit = GetNode<TextEdit>("Browser/Settings/VBoxContainer/TextEdit");
        LogText = GetNode<RichTextLabel>("Browser/Log/VBoxContainer/ScrollContainer/LogText");
        ButtonA = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionA");
        ButtonB = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionB");
        ButtonC = GetNode<Button>("Browser/Log/VBoxContainer/HBoxContainer/OptionC");

        Settings = GetNode<Tabs>("Browser/Settings");

        MusicPlayer = GetNode<AudioStreamPlayer>("SoundPlayers/MusicPlayer");
        SoundEffectsPlayer = GetNode<AudioStreamPlayer>("SoundPlayers/SoundEffectsPlayer");
        WhiteNoisePlayer = GetNode<AudioStreamPlayer>("SoundPlayers/WhiteNoisePlayer");

        sliderList.Add(MusicSlider = Settings.GetNode<Slider>("SliderHolder/MusicSlider"));
        sliderList.Add(WhiteNoiseSlider = Settings.GetNode<Slider>("SliderHolder/WhiteNoiseSlider"));
        sliderList.Add(SoundEffectsSlider = Settings.GetNode<Slider>("SliderHolder/SoundEffectsSlider"));

        foreach (Slider slider in sliderList)
        {
            slider.MinValue = 0.0001;
            slider.MaxValue = 1.5;
            slider.Step = 0.0001;
            slider.Value = 0.5;
        }

        loadSoundSettings("res://Sounds/soundSettings.tres");
    }

    void _on_username_changed(){
        Username = userNameTextEdit.Text;
    }
    
    void setApplicantList(List<homeless> applicants){
        ApplicantsList = applicants;
        setOptionButtons();
        greetingConversation();
    }

    public void setOptionButtons()
    {
        for (int i = 0; i < ApplicantsList.Count; i++)
        {
            Random random = new Random();
            int selectedIndex = random.Next(homelessAnswerSheet.Length);
            while (homelessAnswerSheet[selectedIndex] != 0)
            {
                selectedIndex = random.Next(homelessAnswerSheet.Length);
            }
            GetNode<JobListingBase>("Browser/Jobs/VBoxContainer/JobListing" + (selectedIndex+1)).setCorrectApplicant(ApplicantsList[i]);
            homelessAnswerSheet[selectedIndex] = 1;
        }
    }
    void greetingConversation(){
        if(currentApplicant == 6)
        {
            addTextToLog(Username, "\n\n[color=white]---Go Select and Submit Your Answers In the Jobs Tab!---[/color]");
            finished = true;
            selectedAnswersChanged(true);
            selectedAnswersChanged(false);
            return;
        }
        MoveApplicant(ApplicantsList[currentApplicant], true);
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
            Questions.Add(ApplicantsList[currentApplicant].QuestionList[i].Item1);
            LogText.AppendBbcode("[right][color=blue]"+ApplicantsList[currentApplicant].QuestionList[i].Item1+"[/color][/right]\n");
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
        addTextToLog(Username, Questions[button]);
        
        Tuple<String,List<String>,List<Trait>> currentQuestionList = ApplicantsList[currentApplicant].QuestionList[currentQuestionSet * 3 + button];
        String formattedResponse = String.Format(currentQuestionList.Item2[0], currentQuestionList.Item3[0], currentQuestionList.Item3[1], currentQuestionList.Item3[2]);
        addTextToLog(ApplicantsList[currentApplicant].Name, formattedResponse);
        
        if(currentQuestionSet < 2){
            currentQuestionSet++;
            askQuestions();
        }else if(currentApplicant == 5){
            addTextToLog(Username, "\n\n[color=white]---Go Select and Submit Your Answers In the Jobs Tab!---[/color]");
            finished = true;
            selectedAnswersChanged(true);
            selectedAnswersChanged(false);
        }else{
            EmitSignal(nameof(moveApplicant), ApplicantsList[currentApplicant], false);
            addTextToLog(Username, "Thank You we will let you know if we find a job in a few hours \n" 
                                + "[color=white][u]---Switch Over To The Jobs Tab To Look For Compatiable Jobs---\n[/u][/color]"
                                +"[rainbow]"
                                +"==========================================================\n"
                                +"=========================================================="
                                +"==========================================================[/rainbow]\n");
            currentQuestionSet = 0;
            currentApplicant++;
            requiredTabSwitch = true;
        }
    }

    void addPersonToApplicatentList(String name){
        for(int i = 1; i <= 6; i++){
            GetNode<OptionButton>("Browser/Jobs/VBoxContainer/JobListing" + i +"/OptionButton").AddItem(name);
        }
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
        soundSettings soundSettings = ResourceLoader.Load<soundSettings>(filePath);
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

    public void OnBrowserTabChanged(int tab)
    {
        if (tab == 2)
        {
            MusicPlayer.Stop();
        }
        else if (prevTab == 2)
        {
            var soundSettings = GD.Load<CSharpScript>("res://Scripts/soundSettings.cs").New(MusicVolumeDb, MusicSlider.Value, WhiteNoiseVolumeDb, WhiteNoiseSlider.Value, SoundEffectsVolumeDb, SoundEffectsSlider.Value);
            ResourceSaver.Save("res://Sounds/soundSettings.tres", soundSettings as soundSettings);
            MusicPlayer.Play();
        } else if (prevTab == 1 && requiredTabSwitch){
            
            requiredTabSwitch = false;
            greetingConversation();
        }
        prevTab = tab;
    }

    public void OnSettingsTabClose(int tab)
    {
        var soundSettings = GD.Load<CSharpScript>("res://Scripts/soundSettings.cs").New(MusicVolumeDb, MusicSlider.Value, WhiteNoiseVolumeDb, WhiteNoiseSlider.Value, SoundEffectsVolumeDb, SoundEffectsSlider.Value);
        ResourceSaver.Save("res://Sounds/soundSettings.tres", soundSettings as soundSettings);
    }

    public void selectedAnswersChanged(bool increased)
    {
        numAnswers = increased ? numAnswers + 1 : numAnswers - 1;
        TextureButton theButton = GetNode<TextureButton>("Browser/Jobs/VBoxContainer/SubmitButton");
        if(numAnswers != 6 || !finished)
        {
            theButton.Disabled = true;
        }
        else
        {
            theButton.Disabled=false;
        }
    }

    void _on_SubmitButton_pressed(){
        for(int i = 1; i <= 6; i++){
            String correctApplicantName = GetNode<JobListingBase>("Browser/Jobs/VBoxContainer/JobListing" + i).getCorrectApplicant().Name;
            OptionButton optionButton = GetNode<OptionButton>("Browser/Jobs/VBoxContainer/JobListing" + i +"/OptionButton");
            String selectedApplicantName = optionButton.GetItemText(optionButton.GetSelectedId());
            if(correctApplicantName == selectedApplicantName){
                score++;
            }
        }
        GetNode<Control>("../../FinishScreen").Visible = true;
        Label label = GetNode<Label>("../../Control/ScoreLabel");
        label.Text = "You changed " + score;
        if(score == 1){
            label.Text += " life out of 6";
        }else{
            label.Text += " lives out of 6";
        }
    }

    public void MoveApplicant(homeless character, bool enter)
    {
        EmitSignal(nameof(moveApplicant), character, enter);
    }
}
