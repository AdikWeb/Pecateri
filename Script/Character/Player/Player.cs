using Godot;
using System;

public class Player: KinematicBody
{

	[Export]
	private float ForwardSpeed = 10.0f;
	[Export]
	private float BackwardSpeed = 10.0f;
	[Export]
	private float LeftSpeed = 10.0f;
	[Export]
	private float RightSpeed = 10.0f;
	[Export]
	public int JumpImpulse = 20;
	[Export]
	private int HAcceleration = 6;
	[Export]
	private int Mass = 50;

	private float Speed;

	private Spatial Head;

	private RayCast RayCastDown;
	private Boolean FullContact = false;

	private Vector3 GravityVec = new Vector3();
	private Vector3 HVelocity = new Vector3();
	private Vector3 Movement = new Vector3();

	public override void _Ready()
	{
		Head = GetNode<Spatial>("Head");
		RayCastDown = GetNode<RayCast>("Head/RayCast");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			Head.RotateX(-Mathf.Deg2Rad(eventMouseMotion.Relative.y * Global.MouseSensitivity));
			RotateY(Mathf.Deg2Rad(-eventMouseMotion.Relative.x * Global.MouseSensitivity));
			Vector3 cameraRot = Head.RotationDegrees;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -85, 85);
			Head.RotationDegrees = cameraRot;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		FullContact = RayCastDown.IsColliding();
		Vector3 direction = new Vector3();
		if (!IsOnFloor())
		{
			GravityVec += Vector3.Down * Global.WorldGravity * Mass * delta;
		}
		else if (IsOnFloor() && FullContact)
		{
			GravityVec = -GetFloorNormal() * Global.WorldGravity;
		}
		else
		{
			GravityVec = -GetFloorNormal();
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			GravityVec = Vector3.Up * JumpImpulse;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction -= Transform.basis.z;
			Speed = ForwardSpeed;
		}
		else if (Input.IsActionPressed("move_back"))
		{
			direction += Transform.basis.z;
			Speed = BackwardSpeed;
		}

		if (Input.IsActionPressed("strafe_left"))
		{
			direction -= Transform.basis.x;
			Speed = LeftSpeed;
		}
		
		if (Input.IsActionPressed("strafe_right"))
		{
			direction += Transform.basis.x;
			Speed = RightSpeed;
		}

		direction = direction.Normalized();
		HVelocity = HVelocity.LinearInterpolate(direction* Speed, HAcceleration * delta);

		Movement.z = HVelocity.z + GravityVec.z;
		Movement.x = HVelocity.x + GravityVec.x;
		Movement.y = GravityVec.y;

		MoveAndSlide(Movement, Vector3.Up);
	}
}
