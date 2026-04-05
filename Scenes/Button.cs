using Godot;
using System;

public partial class Button : Godot.Button
{
	public void _on_pressed()
	{
		var loadingScreen = (PackedScene)ResourceLoader.Load("res://scenes/LoadingScreen.tscn");
		var instance = loadingScreen.Instantiate<LoadingScreen>();
		instance.NextScene = "res://Scenes/niveau_1.tscn";
		GetTree().Root.AddChild(instance);
		GetTree().CurrentScene.QueueFree();
	}
	public void _on_button_3_pressed()
	{
		GetTree().Quit();
	}
}
