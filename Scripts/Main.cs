using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public class Main : Control
{
    [Signal] public delegate void setApplicantList(List<homeless> homelessList);
    [Export] private readonly int numHomeless;
    [Export] private readonly int numJobTraits;
    [Export] private readonly int numQuestionTraits;
    [Export] private readonly int questionLength;
    [Export] private readonly string homelessNamePath;
    [Export] private readonly string jobsPath;
    [Export] private readonly string questionsPath;
    [Export] private readonly string charactersPath;
    private List<homeless> homelessList;
    public override void _Ready()
    {
        Connect(nameof(setApplicantList), GetNode("/root/Main/HBoxContainer/Computer"), "setApplicantList");

        homelessList = GenerateHomeless(numHomeless);
        
        EmitSignal(nameof(setApplicantList), homelessList);

        //Debug
        /*
        foreach (homeless homeless in homelessList)
        {
            
            GD.Print("Name: " + homeless.Name);
            GD.Print("Job: " + homeless.Job.Item1);
            foreach (Trait trait in homeless.Job.Item2)
            {
                GD.Print("Trait: " + trait);
            }
            foreach (Tuple<String, List<String>, List<Trait>> question in homeless.QuestionList)
            {
                GD.Print("Question: " + question.Item1);
                foreach(String answer in question.Item2)
                {
                    GD.Print("Answer: " + answer);
                }
                foreach(Trait trait in question.Item3)
                {
                    GD.Print("Trait: " + trait);
                }
            }        
        }
        */
        
    }

    private List<homeless> GenerateHomeless(int numHomeless)
    {
        List<homeless> homelessListBuilder = new List<homeless>();
        List<String> usedNamesList = new List<String>();
        List<String> usedJobsList = new List<String>();
        List<String> usedCharacterList = new List<String>();

        String[] HomelessNames = System.IO.File.ReadAllLines(homelessNamePath);
        String[] RawJobs = System.IO.File.ReadAllLines(jobsPath);
        String[] RawQuestions = System.IO.File.ReadAllLines(questionsPath);
        String[] CharacterLocations = System.IO.File.ReadAllLines(charactersPath);
        Trait[] PossibleTraits = (Trait[]) Enum.GetValues(typeof(Trait));

        //Change String[] of jobs and their traits to Jobs
        List<Tuple<String, List<Trait>>> Jobs = new List<Tuple<String, List<Trait>>>();
        String jobName = "";

        List<Trait> jobTraits = new List<Trait>();
        for (int j = 0; j < RawJobs.Length; j++)
        {
            if (j % (numJobTraits+1) == 0)
            {
                jobName = RawJobs[j];
            }
            else
            {
                Enum.TryParse<Trait>(RawJobs[j], out Trait trait);
                jobTraits.Add(trait);
            }
            if(j % (numJobTraits+1) == numJobTraits)
            {
                Jobs.Add(new Tuple<String, List<Trait>>(jobName, jobTraits));
                jobTraits = new List<Trait>();
            }
        }

        //Change String[] of questions and its parts to Questions
        String actualQuestion = "";
        List<String> answerList = new List<string>();
        List<Tuple<String, List<String>, List<Trait>>> Questions = new List<Tuple<String, List<String>, List<Trait>>>();
        for(int j = 0; j < RawQuestions.Length; j++)
        {
            if(j % questionLength == 0)
            {
                actualQuestion = RawQuestions[j];
            }
            else
            {
                answerList.Add(RawQuestions[j]);
            }
            if(j % questionLength == questionLength-1)
            {
                Questions.Add(new Tuple<string, List<string>, List<Trait>>(actualQuestion, answerList, null));
                answerList = new List<string>();
            }
        }

        //Create and randomize each homeless person
        Random rand = new Random();
        for (int i = 0; i < numHomeless; i++)
        {
            //Randomize Name
            String homelessName = HomelessNames[rand.Next(HomelessNames.Length)];
            while (usedNamesList.Contains(homelessName))
            {
                homelessName = HomelessNames[rand.Next(HomelessNames.Length)];
            }
            usedNamesList.Add(homelessName);

            //Randomize Job
            Tuple<String, List<Trait>> selectedJob = Jobs[rand.Next(Jobs.Count-1)];
            while (usedJobsList.Contains(selectedJob.Item1))
            {
                selectedJob = Jobs[rand.Next(Jobs.Count)];
            }
            usedJobsList.Add(selectedJob.Item1);

            //Randomize Question
            List<Tuple<string, List<string>, List<Trait>>> selectedQuestionList = new List<Tuple<string, List<string>, List<Trait>>>();
            List<Trait> traitList = new List<Trait>();
            for (int j = 0; j < numQuestionTraits * 3; j++)
            {
                if(j % numQuestionTraits == 0)
                {
                    traitList.Add(selectedJob.Item2[j / 3]);
                }
                else
                {
                    traitList.Insert(rand.Next(traitList.Count+1), PossibleTraits[rand.Next(PossibleTraits.Length-1)]);
                }
                if(j % numQuestionTraits == numQuestionTraits - 1)
                {
                    Tuple<string, List<string>, List<Trait>> randomQuestion = Questions[rand.Next(Questions.Count)];
                    selectedQuestionList.Add(new Tuple<string, List<string>, List<Trait>>(randomQuestion.Item1, randomQuestion.Item2, traitList));
                    traitList = new List<Trait>();
                }
            }
            //Randomize CharacterTexture
            String characterLocation = CharacterLocations[rand.Next(CharacterLocations.Length)];
            while (usedCharacterList.Contains(characterLocation))
            {
                characterLocation = CharacterLocations[rand.Next(CharacterLocations.Length)];
            }
            Texture characterTexture = GD.Load<Texture>("res://Art/Characters/" + characterLocation + ".png");
            //Add homeless person to list
            homelessListBuilder.Add(new homeless(homelessName, selectedJob, selectedQuestionList, characterTexture));
        }
        return homelessListBuilder;
    }
}
