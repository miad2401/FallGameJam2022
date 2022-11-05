using Godot;
using System;
using System.Collections.Generic;

public class homeless : Resource
{
    [Export]
    public String Name { get; set; }

    [Export]
    public Tuple<String,List<Trait>> Job { get; set; }

    [Export]
    public List<Tuple<String,List<Trait>,String>> QuestionList { get; set; }

    public homeless(String name = "", Tuple<String, List<Trait>> job = null, List<Tuple<String,List<Trait>,String>> questionList = null)
    {
        Name = name;
        Job = job;
        QuestionList = questionList;
    }
}
