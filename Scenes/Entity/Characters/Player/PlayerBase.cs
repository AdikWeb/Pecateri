using Godot;
using System;

public class PlayerBase : KinematicBody
{
	private Spatial Head;
	private readonly float MouseSensitivity = 0.3f;

	public override void _Ready()
	{
		Head = GetNode<Spatial>("Head");
	}

	public override void _Input(InputEvent @event)
	{
	   if(@event is InputEventMouseMotion eventMouseMotion)
		{
			Head.RotateX(-Mathf.Deg2Rad(eventMouseMotion.Relative.y * MouseSensitivity));
			RotateY(Mathf.Deg2Rad(-eventMouseMotion.Relative.x * MouseSensitivity));
			Vector3 cameraRot = Head.RotationDegrees;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -85, 85);
			Head.RotationDegrees = cameraRot;
		}
	}
}
