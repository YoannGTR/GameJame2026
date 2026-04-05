using Godot;
using System;

public partial class Niveau1 : Node3D
{
	private CanvasLayer _pauseMenu;

	public int currentDay = 1;
	private const int maxDays = 5;
	private Node uiLevel;

	//map des objectifs (string = nom de l'objectif, bool = si l'objectif est accompli ou pas)
	private Godot.Collections.Dictionary goalsDone = new Godot.Collections.Dictionary()
	{
		{"Où suis-je ?", false},
		{"A quoi sert le bac ?", false},
		{"Qu'y a-t-il derrière la porte ?", false},
		{"A quoi sert la clé ?", false},
		{"Qu'y a-t-il à l'étage ?", false}
	};
	private Godot.Collections.Dictionary goalsOnDoing = new Godot.Collections.Dictionary()
	{
		{"Où suis-je ?", false},
		{"A quoi sert le bac ?", false},
		{"Qu'y a-t-il derrière la porte ?", false},
		{"A quoi sert la clé ?", false},
		{"Qu'y a-t-il à l'étage ?", false}
	};

	public override void _Ready()
	{
		_pauseMenu = GetNode<CanvasLayer>("PauseMenu");
		_pauseMenu.Visible = false;
		uiLevel = GetNode<Player>("Player").GetNode<Camera3D>("Camera3D").GetNode<VBoxContainer>("UI_level");
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
		if(currentDay >= maxDays) // évite de dépasser le nombre de jours disponibles
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


		uiLevel.GetNode<Label>("Day").Text = "Jour " + currentDay; // update the day label
	}

	public void validateGoal(string goal)
	{
		switch (goal)
		{
			case "Où suis-je ?":
				goalsDone["Où suis-je ?"] = true;
				break;
			case "A quoi sert le bac ?":
				goalsDone["A quoi sert le bac ?"] = true;
				break;
			case "Qu'y a-t-il derrière la porte ?":
				goalsDone["Qu'y a-t-il derrière la porte ?"] = true;
				break;
			case "A quoi sert la clé ?":
				goalsDone["A quoi sert la clé ?"] = true;
				break;
			case "Qu'y a-t-il à l'étage ?":
				goalsDone["Qu'y a-t-il à l'étage ?"] = true;
				break;
		}
	}

	public void launchGoal(string goal)
	{
		switch (goal)
		{
			case "Où suis-je ?":
				goalsOnDoing["Où suis-je ?"] = true;
				break;
			case "A quoi sert le bac ?":
				goalsOnDoing["A quoi sert le bac ?"] = true;
				break;
			case "Qu'y a-t-il derrière la porte ?":
				goalsOnDoing["Qu'y a-t-il derrière la porte ?"] = true;
				break;
			case "A quoi sert la clé ?":
				goalsOnDoing["A quoi sert la clé ?"] = true;
				break;
			case "Qu'y a-t-il à l'étage ?":
				goalsOnDoing["Qu'y a-t-il à l'étage ?"] = true;
				break;
		}
	}

	
}
