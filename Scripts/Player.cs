using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// mouse look
	public const float MouseSensitivity = 0.1f;
	public float LookAngle = 90.0f;
	private Vector2 mouseDelta = Vector2.Zero;
	private float cameraPitch = 0.0f;

	// movement
	public const float WalkSpeed = 15.0f;
	public const float SprintSpeed = 25.0f;
	public const float JumpVelocity = 20.0f;
	public const float UpGravityMultiplier = 5.0f;
	public const float DownGravityMultiplier = 5.0f;

	private Camera3D camera;
	private RayCast3D ray;
	private Node3D holdPoint;
	public RigidBody3D heldObject = null;
	private DoorBoddy lastDoor = null;

	private Niveau1 niveau;

	private bool haveKey = false;

	private int cpt = 0;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			float gravityMult = velocity.Y > 0 ? UpGravityMultiplier : DownGravityMultiplier;
			velocity += GetGravity() * (float)delta * gravityMult;
		}

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		float currentSpeed = Input.IsActionPressed("sprint") ? SprintSpeed : WalkSpeed;

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");		
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * currentSpeed;
			velocity.Z = direction.Z * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouse)
		{
			mouseDelta += eventMouse.Relative;
		}
	}

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera3D");
		ray = camera.GetNode<RayCast3D>("RayCast3D");
		holdPoint = camera.GetNode<Node3D>("HoldPoint");
		Input.MouseMode = Input.MouseModeEnum.Captured;

		niveau = GetTree().CurrentScene as Niveau1;
	}

	public override void _Process(double delta)
	{
		if(cpt > 0)
		{
			cpt--;
			if(cpt <= 0)
			{
				camera.GetNode<Label>("Verouillé").Visible = false; // hide the "Door Locked" message after the timer ends
			}
		}

		if (GetTree().Paused)
		return;
		RotationDegrees = new Vector3(0, RotationDegrees.Y - mouseDelta.X * MouseSensitivity, 0);

		cameraPitch = Mathf.Clamp(cameraPitch - mouseDelta.Y * MouseSensitivity, -LookAngle, LookAngle);
		camera.RotationDegrees = new Vector3(cameraPitch, 0, 0);

		mouseDelta = Vector2.Zero;
	
		bool isDoorPointed = false;

		if (ray.IsColliding())
		{
			Node collider = ray.GetCollider() as Node;
			if (collider.GetParent().IsInGroup("door") )
			{
				DoorBoddy door = collider.GetParent<DoorBoddy>();
				door.GetNode<Sprite3D>("Sprite3D").Visible = true; 
				door.GetNode<Sprite3D>("Sprite3D2").Visible = true; 
				lastDoor = door;
				isDoorPointed = true;

				if(Input.IsActionJustPressed("interract")) {
					if(haveKey && door.GetParent().Name == "Door2"){
						door.ToggleDoor();
					}else if(door.GetParent().Name == "Door2"){
						niveau.launchGoal(3);
						camera.GetNode<Label>("Verouillé").Visible = true; 
						cpt = 1 * 60; 
					}else{
						camera.GetNode<Label>("Verouillé").Visible = true; 
						cpt = 1 * 60; 
					}
				}
			}
			if (collider.GetParent().IsInGroup("death"))
			{
				if (Input.IsActionJustPressed("interract"))
				{
					GetNode<CanvasLayer>("/root/niveau_1/Defaite").Visible = true;
					Input.MouseMode = Input.MouseModeEnum.Visible;
					GetTree().Paused = true;
				}
			}
			if (collider.GetParent().IsInGroup("victory"))
			{
				if (Input.IsActionJustPressed("interract"))
				{
					GetNode<CanvasLayer>("/root/niveau_1/Victoire").Visible = true;
					Input.MouseMode = Input.MouseModeEnum.Visible;
					GetTree().Paused = true;
				}
			}

			if (heldObject == null && collider.IsInGroup("pickup"))
			{
				
				camera.GetNode<Label>("Ramasser").Visible = true;
			}else
			{
				camera.GetNode<Label>("Ramasser").Visible = false;
			}

			if(collider.GetParent().GetParent().IsInGroup("bac"))
			{
				niveau.validateGoal(1);
				niveau.launchGoal(2);
			}
		}else{
			camera.GetNode<Label>("Ramasser").Visible = false;
		}

		// Masquer les sprites si on ne pointe plus la porte
		if (!isDoorPointed && lastDoor != null)
		{
			lastDoor.GetNode<Sprite3D>("Sprite3D").Visible = false;
			lastDoor.GetNode<Sprite3D>("Sprite3D2").Visible = false;
			lastDoor = null;
		}

		if (Input.IsActionJustPressed("grab"))
		{
			if (heldObject == null)
			{
				TryPickup();
			}
			else
			{
				DropObject();
			}
		}

		if (Input.IsActionJustPressed("launch"))
		{
			if (heldObject != null)
			{
				LaunchObject();
			}
		}
	}

	private void TryPickup()
	{
		if (ray.IsColliding())
		{
			Node collider = ray.GetCollider() as Node;

			if (collider.IsInGroup("pickup"))
			{
				if(collider.Name == "keyCollider" || collider.Name == "PickupObjectKey"){
					haveKey = true;
					collider.QueueFree(); 
					camera.GetNode<TextureRect>("keyIcon").Visible = true; // show the key icon on the UI
					return;
				}

				heldObject = collider as RigidBody3D;

				heldObject.Freeze = true; // stop physique
				heldObject.Reparent(holdPoint);
				heldObject.Position = Vector3.Zero;

				heldObject.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = true; // désactive la collision de l'objet pour éviter les problèmes de physique


				
			}
		}
	}
	public void DropObject()
	{
		Vector3 dropPosition = camera.GlobalPosition;

		String nameOfNode = heldObject.GetChildren()[0].Name;
		heldObject.Reparent(GetTree().CurrentScene.GetNode<Node>(nameOfNode));
		heldObject.Freeze = false;
		heldObject.GlobalPosition = dropPosition + camera.GlobalTransform.Basis.Z * -4.0f; // drop a bit in front of the camera
		
		heldObject.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false; // réactive la collision de l'objet

		heldObject = null;
	}
	public void LaunchObject()
	{
		Vector3 launchDirection = -camera.GlobalTransform.Basis.Z.Normalized();
		float launchForce = 30.0f;

		Vector3 launchOrigin = camera.GlobalPosition;
		launchOrigin += launchDirection * 4.0f; // start a bit in front of the camera

		String nameOfNode = heldObject.GetChildren()[0].Name;

		heldObject.Reparent(GetTree().CurrentScene.GetNode<Node>(nameOfNode)); // reparent to the main scene to avoid issues with physics when launching
		heldObject.GlobalPosition = launchOrigin;
		heldObject.Freeze = false;
		heldObject.LinearVelocity = Vector3.Zero;
		heldObject.AngularVelocity = Vector3.Zero;

		heldObject.ApplyCentralImpulse(launchDirection * launchForce);
		heldObject.GetNode<CollisionShape3D>("CollisionShape3D").Disabled = false; // réactive la collision de l'objet
		heldObject = null;

	}
}
