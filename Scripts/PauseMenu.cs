using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public void OnResumePressed()
	{
		GetTree().Paused = false;
		Visible = false;

		Input.MouseMode = Input.MouseModeEnum.Captured; // 🔒 remet la souris en jeu
	}
	public void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
