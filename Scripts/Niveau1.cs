using Godot;
using System;

public partial class Niveau1 : Node3D
{
	private CanvasLayer _pauseMenu;

	public int currentDay = 5;
	private const int maxDays = 5;
	private Node uiLevel;
	private Label dayLabel;
	private HBoxContainer goalDoneContainer;

	private int cpt = 0;
	private AudioStreamPlayer _music;



	public override void _Ready()
	{
		_pauseMenu = GetNode<CanvasLayer>("PauseMenu");
		_pauseMenu.Visible = false;
		uiLevel = GetNode<Player>("Player").GetNode<Camera3D>("Camera3D").GetNode<VBoxContainer>("UI_level");
		dayLabel = uiLevel.GetNode<Label>("Day") as Label;
		goalDoneContainer = uiLevel.GetNode<HBoxContainer>("GoalDone") as HBoxContainer;
		_music = GetNode<AudioStreamPlayer>("MusicPlayer");
		_music.Play();
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
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Captured; 
		}
	}

	public void ChangeDay()
	{
		if(currentDay >= maxDays) // évite de dépasser le nombre de jours disponibles
		{
			GD.Print("Maximum number of days reached.");
			GetNode<Node>("Door1").GetNode<Node>("DoorBoddy").AddToGroup("Victory");
			return;
		}

		switch (currentDay)
		{
			case 1:{

				MeshInstance3D cube = GetNode<Node>("salle_principale").GetNode<Node>("passerel").GetNode<MeshInstance3D>("Cube");
				cube.Visible = true; 
				cube.GetNode<StaticBody3D>("StaticBody3D").GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
				break;
			}
			case 2:{

				MeshInstance3D cube = GetNode<Node>("salle_principale").GetNode<Node>("passerel").GetNode<MeshInstance3D>("Cube_003");
				cube.Visible = true; 
				cube.GetNode<StaticBody3D>("StaticBody3D").GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
				break;
			}
			case 3:{
				MeshInstance3D cube = GetNode<Node>("salle_principale").GetNode<Node>("passerel").GetNode<MeshInstance3D>("Cube_001");
				cube.Visible = true; 
				cube.GetNode<StaticBody3D>("StaticBody3D").GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
				break;
			}
			case 4:{
				MeshInstance3D cube = GetNode<Node>("salle_principale").GetNode<Node>("passerel").GetNode<MeshInstance3D>("Cube_002");
				cube.Visible = true; 
				cube.GetNode<StaticBody3D>("StaticBody3D").GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false;
				break;
			}
			default:
				GD.Print("Unknown day: " + currentDay);
				break;
		}

		//gestion des plantes
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).Visible = false;
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true; // désactive la collision de l'objet actuel
		currentDay++;
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).Visible = true; 
		GetNode<Node>("plante").GetNode<RigidBody3D>("PickupObject"+currentDay).GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false; // active la collision du nouvel objet


		//reset de la position du joueur
		GetNode<Player>("Player").GlobalPosition = new Vector3(0, 0, 0);


		//libere l'objet tenu
		Player player = GetNode<Player>("Player");
		if(player.heldObject != null){
			player.DropObject();

		}

		//reset les portes
		if(GetNode<Node3D>("Door1").GetNode<DoorBoddy>("DoorBoddy").IsOpen()){
			GetNode<Node3D>("Door1").GetNode<DoorBoddy>("DoorBoddy").ToggleDoor();
		}
		if(GetNode<Node3D>("Door2").GetNode<DoorBoddy>("DoorBoddy").IsOpen()){
			GetNode<Node3D>("Door2").GetNode<DoorBoddy>("DoorBoddy").ToggleDoor();
		}

		//reset la position des objets à ramasser et leur vitesse
		GetNode<Node3D>("pierre").GetNode<RigidBody3D>("PickupObject").GlobalPosition = new Vector3(-46f, 15.2f, -27f);
		GetNode<Node3D>("pierre").GetNode<RigidBody3D>("PickupObject").LinearVelocity = Vector3.Zero;
		GetNode<Node3D>("pierre").GetNode<RigidBody3D>("PickupObject").AngularVelocity = Vector3.Zero;

		GetNode<Node3D>("balle").GetNode<RigidBody3D>("PickupObject").GlobalPosition = new Vector3(-27f, 0.1f, -3f);
		GetNode<Node3D>("balle").GetNode<RigidBody3D>("PickupObject").LinearVelocity = Vector3.Zero;
		GetNode<Node3D>("balle").GetNode<RigidBody3D>("PickupObject").AngularVelocity = Vector3.Zero;

		GetNode<Node3D>("plante").GetNode<RigidBody3D>("PickupObject" + currentDay).GlobalPosition = new Vector3(20.5f, 3.1f, 37.5f);
		GetNode<Node3D>("plante").GetNode<RigidBody3D>("PickupObject" + currentDay).LinearVelocity = Vector3.Zero;
		GetNode<Node3D>("plante").GetNode<RigidBody3D>("PickupObject" + currentDay).AngularVelocity = Vector3.Zero;

		GetNode<Node3D>("bois").GetNode<RigidBody3D>("PickupObject").GlobalPosition = new Vector3(18f, 4.1f, -89f);
		GetNode<Node3D>("bois").GetNode<RigidBody3D>("PickupObject").LinearVelocity = Vector3.Zero;
		GetNode<Node3D>("bois").GetNode<RigidBody3D>("PickupObject").AngularVelocity = Vector3.Zero;

		if(currentDay == 3){
			GetNode<Node3D>("key").Visible = true;
			launchGoal(7);
		}
		

		dayLabel.Text = "Jour " + currentDay; // update the day label
	}

	public void validateGoal(int goal)
	{
		HBoxContainer goalContainer = uiLevel.GetNode<HBoxContainer>("Goal"+goal);
		if(goalContainer.Visible == true && goalContainer.GetNode<CheckBox>("CheckBox").IsPressed() == false){
			// if(goal == 1 ){

			// }

			// check the checkbox
			goalContainer.GetNode<CheckBox>("CheckBox").ButtonPressed = true;
			goalContainer.Visible = false; // hide the goal container
			goalDoneContainer.GetNode<Label>("Label").Text = goalContainer.GetNode<Label>("Label").Text;
			goalDoneContainer.Visible = true;
			cpt = 5 * 60; // show the goalDoneContainer for 5 seconds

			//debloquage
			switch (goal)
			{
				case 1:
					GD.Print("Goal 1 validated!");
					break;
				case 2:
					GD.Print("Goal 2 validated!");
					
					break;
				case 3:
					GD.Print("Goal 3 validated!");
					break;
				case 4:
					GD.Print("Goal 4 validated!");
					break;
				case 5:
					GD.Print("Goal 5 validated!");
					break;
				default:
					GD.Print("Unknown goal validated: " + goal);
					break;
			}
		}
		
	}

	public void launchGoal(int goal)
	{
		HBoxContainer goalContainer = uiLevel.GetNode<HBoxContainer>("Goal"+goal);
		if(goalContainer.Visible == false && goalContainer.GetNode<CheckBox>("CheckBox").IsPressed() == false){
			goalContainer.Visible = true;
		}
	}

	public bool isGoalValidated(int goal)
	{
		HBoxContainer goalContainer = uiLevel.GetNode<HBoxContainer>("Goal"+goal);
		return goalContainer.GetNode<CheckBox>("CheckBox").IsPressed();
	}

	public override void _Process(double delta)
	{
		// hide goalDoneContainer after 5 seconds
		if(cpt > 0){
			cpt--;
			if(cpt <= 0){
				goalDoneContainer.Visible = false;
				cpt = 0;
			}
		}
		
	}

	
}
