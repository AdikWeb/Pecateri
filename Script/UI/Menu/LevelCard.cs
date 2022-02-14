using Godot;
using System;

public class LevelCard : Control
{
	[Export]
	private string LevelName = "";
	[Export] 
	private Texture Preview;

	private TextureButton ButtonPanel;
	private Label LabelPanel;
	private ColorRect ColorRect;
	private bool Active = false;

	public override void _Ready()
	{
		ButtonPanel = GetNode<TextureButton>("TextureButton");
		LabelPanel = GetNode<Label>("TextureButton/Label");
		
		UpdateCard();
	}

	public override void _Process(float delta)
	{
		Active = Global.LevelName == LevelName;
		UpdateCard();
	}

	private void UpdateCard()
	{
		if (Preview != null) ButtonPanel.TextureNormal = Preview;
		if (LabelPanel != null) LabelPanel.Set("text", $"{LevelName} {Active}");
	}

	private void _on_TextureButton_pressed()
	{
		Global.LevelName = LevelName;
	}
}
