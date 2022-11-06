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
    public List<Tuple<String,List<String>,List<Trait>>> QuestionList { get; set; }

    [Export]
    public Texture Body { get; set; }

    public homeless(String name = "", Tuple<String, List<Trait>> job = null, List<Tuple<String, List<String>, List<Trait>>> questionList = null, Texture body = null)
    {
        Name = name;
        Job = job;
        QuestionList = questionList;
        Body = body;
    }
}