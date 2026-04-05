using Godot;
using System;

public partial class Control_victoire : Control
{
		public void _on_quit_button_pressed()
	{
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
