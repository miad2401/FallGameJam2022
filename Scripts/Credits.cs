using Godot;
using System;

public class Credits : Node2D
{
    void _on_MainButton_pressed(){
        GetTree().ChangeScene("res://Scenes/StartingMenu");
    }
}
