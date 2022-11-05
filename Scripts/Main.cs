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
    [Export] private readonly string questionsPath;
    private List<homeless> homelessList;
    public override void _Ready()
    {
        homelessList = GenerateHomeless(numHomeless);
        foreach (homeless homeless in homelessList)
        {
            GD.Print("Name: " + homeless.Name);
            GD.Print("Job: " + homeless.Job.Item1);
            foreach (Trait trait in homeless.Job.Item2)
            {
                GD.Print("Trait: " + trait);
            }
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
        String[] RawJobs = System.IO.File.ReadAllLines(jobsPath);
        String[] Questions = System.IO.File.ReadAllLines(questionsPath);

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
            List <String> usedQuestionsList = new List<String>();
            int questionIndex = rand.Next(Questions.Length / (numJobTraits + 1));
            //Add homeless person to list
            homelessListBuilder.Add(new homeless(homelessName, selectedJob));
        }
        return homelessListBuilder;
    }
}
