using Godot;
using System;

public partial class Niveau1 : Node3D
{
	private CanvasLayer _pauseMenu;

	public override void _Ready()
	{
		_pauseMenu = GetNode<CanvasLayer>("PauseMenu");
		_pauseMenu.Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			TogglePause();
		}
	}

	private void TogglePause()
	{
		bool isPaused = !GetTree().Paused;
		GetTree().Paused = isPaused;

		_pauseMenu.Visible = isPaused;

		if (isPaused)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible; // 🔓 libère la souris
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured; // 🔒 reprend contrôle FPS
		}
	}
}
