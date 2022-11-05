using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public class Main : Control
{
    [Export] private readonly int numHomeless;
    [Export] private readonly int numJobTraits;
    [Export] private readonly string homelessNamePath;
    [Export] private readonly string jobsPath;
    private List<homeless> homelessList;
    public override void _Ready()
    {
        homelessList = GenerateHomeless(numHomeless);
        foreach (homeless homeless in homelessList)
        {
            GD.Print("Name: " + homeless.Name);
            foreach (Trait trait in homeless.TraitList)
            {
                GD.Print("Trait: " + trait);
            }
            GD.Print("Job: " + homeless.Job);
            /*
            foreach (Tuple<String, List<Trait>, String> question in homeless.QuestionList)
            {
                GD.Print("Question: " + question.Item1);
                foreach (Trait trait in question.Item2)
                {
                    GD.Print("Question Trait: " + trait);
                }
                GD.Print("Response: " + question.Item3);
            }
            */
        }
    }

    private List<homeless> GenerateHomeless(int numHomeless)
    {
        List<homeless> homelessListBuilder = new List<homeless>();
        List<String> usedNamesList = new List<String>();
        List<String> usedJobsList = new List<String>();

        String[] HomelessNames = System.IO.File.ReadAllLines(homelessNamePath);
        String[] Jobs = System.IO.File.ReadAllLines(jobsPath);

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
            int jobIndex = rand.Next(Jobs.Length / (numJobTraits+1));
            GD.Print(jobIndex);
            String jobName = Jobs[jobIndex* (numJobTraits + 1)];
            while (usedJobsList.Contains(jobName))
            {
                jobIndex = rand.Next(Jobs.Length / (numJobTraits + 1));
                jobName = Jobs[jobIndex * (numJobTraits + 1)];
            }
            usedJobsList.Add(jobName);
            List<Trait> jobTraits = new List<Trait>();
            for(int j = jobIndex * (numJobTraits + 1)+1; j < jobIndex * (numJobTraits + 1)+numJobTraits+1; j++)
            {
                Enum.TryParse<Trait>(Jobs[j], out Trait trait);
                jobTraits.Add(trait);
            }

            //Randomize Question
            List<String> usedQuestionsList = new List<String>();

            //Add homeless person to list
            homelessListBuilder.Add(new homeless(homelessName, jobTraits, jobName));
        }
        return homelessListBuilder;
    }
}
