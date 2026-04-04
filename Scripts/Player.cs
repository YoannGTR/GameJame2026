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
	public const float Speed = 15.0f;
	public const float JumpVelocity = 14.0f;
	public const float UpGravityMultiplier = 4.0f;
	public const float DownGravityMultiplier = 4.0f;

	private Camera3D camera;

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

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
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
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		if (GetTree().Paused)
		return;
		RotationDegrees = new Vector3(0, RotationDegrees.Y - mouseDelta.X * MouseSensitivity, 0);

		cameraPitch = Mathf.Clamp(cameraPitch - mouseDelta.Y * MouseSensitivity, -LookAngle, LookAngle);
		camera.RotationDegrees = new Vector3(cameraPitch, 0, 0);

		mouseDelta = Vector2.Zero;
	}
}
