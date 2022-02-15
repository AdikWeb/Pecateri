using System;
using Godot;

public class Player : KinematicBody
{

	[Export]
	private float Strength = 4;

	[Export]
	public int JumpImpulse = 7;

	[Export]
	private float Weight =	80;

	[Export]
	private int HAcceleration = 6;


	[Export]
	private float ForwardSpeed = 7.0f;
	
	[Export]
	private float BackwardSpeed = 7.0f;
	
	[Export]
	private float LeftSpeed = 7.0f;
	
	[Export]
	private float RightSpeed = 7.0f;
	
	private float Mass = 1;
	private float JumpPawer;
	private float Speed;
	private Spatial Head;
	private RayCast RayCastDown;
	private Boolean FullContact = false;
	private Vector3 GravityVec = new Vector3();
	private Vector3 HVelocity = new Vector3();
	private Vector3 Movement = new Vector3();

	private Control OverlayMenu;
	private Label FPS;
	public override void _Ready()
	{
		Head = GetNode<Spatial>("Head");
		RayCastDown = GetNode<RayCast>("Head/RayCast");
		OverlayMenu = GetNode<Control>("CanvasLayer/Pause");
		FPS = GetNode<Label>("CanvasLayer/Label");
		Mass = Math.Abs((float)Weight / Global.WorldGravity);
		JumpPawer = Math.Abs(JumpImpulse * Strength - Mass);

		Input.SetMouseMode(Input.MouseMode.Captured);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion && !OverlayMenu.Visible)
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

		if(FPS != null) FPS.Set("text", $"FPS: {Performance.Monitor.RenderVerticesInFrame}");
		FullContact = RayCastDown.IsColliding();
		Vector3 direction = new Vector3();
		if (!IsOnFloor())
		{
			float a = Weight + Global.WorldGravity / 11;
			GravityVec += Vector3.Down * a * delta;
		}
		else if (IsOnFloor() && FullContact)
		{
			GravityVec = -GetFloorNormal() * Global.WorldGravity;
		}
		else
		{
			GravityVec = -GetFloorNormal();
		}

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			OverlayMenu.Visible = !OverlayMenu.Visible;
			Input.SetMouseMode(OverlayMenu.Visible? Input.MouseMode.Visible : Input.MouseMode.Captured);
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			GravityVec = Vector3.Up * JumpPawer;
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
		HVelocity = HVelocity.LinearInterpolate(direction * Speed, HAcceleration * delta);

		Movement.z = HVelocity.z + GravityVec.z;
		Movement.x = HVelocity.x + GravityVec.x;
		Movement.y = GravityVec.y;

		MoveAndSlide(Movement, Vector3.Up);
	}

	private void _on_GoToMainMenu_pressed()
	{
		Root.GoToMainMenu();
	}

	private void _on_ExitGame_pressed()
	{
		GetTree().Quit();
	}
	
	private void _on_lvl1_pressed()
	{
		Root.GoToScene("World/Core");
	}

	private void _on_lvl2_pressed()
	{
		Root.GoToScene("World/Hentai_1");
	}

}







