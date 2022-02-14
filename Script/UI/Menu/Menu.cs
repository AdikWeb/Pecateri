using Godot;

public class Menu : Control
{
	public override void _Ready()
	{
		Global.WindowWidth = (int)ProjectSettings.GetSetting("display/window/size/width");
		Global.WindowHeight = (int)ProjectSettings.GetSetting("display/window/size/height");
		ReConstruct();
	}

	private void _on_StartBtn_pressed()
	{
		Root.GoToScene("UI/Menu_start");
	}

	private void _on_QuitBtn_pressed()
	{
		GetTree().Quit();
	}

	private void _on_GoMainMenu_pressed()
	{
		Root.GoToScene("UI/Menu");
	}

	private void ReConstruct()
	{
		if (HasNode("ScrollContainer") && HasNode("ScrollContainer/GridContainer"))
		{
			ScrollContainer ScrollContainer = GetNode<ScrollContainer>("ScrollContainer");
			GridContainer GrContainer = GetNode<GridContainer>("ScrollContainer/GridContainer");
			float ScrollContainerWidth = ScrollContainer.GetRect().Size.x;
			float ScrollContainerHeight = ScrollContainer.GetRect().Size.y;
			GrContainer.Set("columns", (int)ScrollContainerWidth / 141);
		}
	}
	private void _on_MenuStart_resized()
	{
		ReConstruct();
	}
	private void _on_host_pressed()
	{
		Root.GoToScene($"World/{Global.LevelName}");
	}
}








