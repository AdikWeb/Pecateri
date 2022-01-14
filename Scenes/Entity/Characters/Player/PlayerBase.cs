using Godot;
using System;

public class PlayerBase : KinematicBody
{
	private Spatial Head;
	private RayCast RayCastDown;
	private readonly float MouseSensitivity = 0.3f;
	
	private readonly float Speed = 5.0f;
	private int HAcceleration = 6;
	
	private float JumpPower = 1.2f;
	private float Gravity = 0.98f;

	private Boolean FullContact = false;
	private Vector3 HVelocity = new Vector3();
	private Vector3 Movement = new Vector3();
	private Vector3 GravityVec = new Vector3();
	public override void _Ready()
	{
		Head = GetNode<Spatial>("Head");
		RayCastDown = GetNode<RayCast>("Head/RayCast");
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			Head.RotateX(-Mathf.Deg2Rad(eventMouseMotion.Relative.y * MouseSensitivity));
			RotateY(Mathf.Deg2Rad(-eventMouseMotion.Relative.x * MouseSensitivity));
			Vector3 cameraRot = Head.RotationDegrees;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -85, 85);
			Head.RotationDegrees = cameraRot;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 direction = new Vector3();
		FullContact = RayCastDown.IsColliding();
		GD.Print(FullContact);

		if (!IsOnFloor())
			GravityVec += Vector3.Down * Gravity * delta;
		else if(IsOnFloor() && FullContact)
			GravityVec = -GetFloorNormal() * Gravity;
		else
			GravityVec = -GetFloorNormal();

		if (IsOnFloor() && Input.IsActionJustPressed("jump") && (IsOnFloor() || RayCastDown.IsColliding()))
			GravityVec = Vector3.Up * JumpPower;
		if (Input.IsActionPressed("move_forward"))
			direction -= Transform.basis.z;
		else if (Input.IsActionPressed("move_back"))
			direction += Transform.basis.z;
		if (Input.IsActionPressed("strafe_left"))
			direction -= Transform.basis.x;
		else if (Input.IsActionPressed("strafe_right"))
			direction += Transform.basis.x;

		direction = direction.Normalized();
		HVelocity = HVelocity.LinearInterpolate(direction * Speed, HAcceleration * delta);
		Movement.z = HVelocity.z + GravityVec.z;
		Movement.x = HVelocity.x + GravityVec.x;
		Movement.y = GravityVec.y;
		MoveAndSlide(Movement, Vector3.Up);
	}
}

