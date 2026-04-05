using Godot;
using System;

public partial class Niveau1 : Node3D
{
	private CanvasLayer _pauseMenu;

	private int currentDay = 1;

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
		if (@event.IsActionPressed("test"))
		{
			ChangeDay();
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

	private void ChangeDay()
	{
		if(currentDay >= 5) // reset to day 1 after day 4
		{
			return;
		}

		GD.Print("Changing to day " + (currentDay + 1));
		GD.Print(GetNode<Node>("plante").Name);
		//parcours les enfants de "plante" et affiche leurs noms
		foreach (Node child in GetNode<Node>("plante").GetChildren())
		{
			GD.Print("Child: " + child.Name);
		}

		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).Visible = false;
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true; // désactive la collision de l'objet actuel
		currentDay++;
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).Visible = true; 
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false; // active la collision du nouvel objet

		Player player = GetNode<Player>("Player");
		player.GetNode<Camera3D>("Camera3D").GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("Label").Text = "Jour " + currentDay; // update the day label
	}

	
}
