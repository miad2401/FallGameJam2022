using Godot;
using System;
using System.Collections.Generic;

public class homeless : Resource
{
    [Export]
    public String Name { get; set; }

    [Export]
    public List<Trait> TraitList { get; set; }

    [Export]
    public String Job { get; set; }

    [Export]
    public List<Tuple<String,List<Trait>>> QuestionList { get; set; }

    public homeless(String name = "", List<Trait> traitList = null, String job = "", List<Tuple<String,List<Trait>>> questionList = null)
    {
        Name = name;
        TraitList = traitList;
        Job = job;
        QuestionList = questionList;
    }
}
