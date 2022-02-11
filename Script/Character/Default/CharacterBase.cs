using Godot;
using System;

public class CharacterBase : KinematicBody
{
	[Export]
	private float ForwardSpeed = 1.0f;
	[Export]
	private float BackwardSpeed = 1.0f;
	[Export]
	private float LeftSpeed = 1.0f;
	[Export]
	private float RightSpeed = 1.0f;
	[Export]
	private float JumpPower = 10.0f;
}
