using Godot;
public class ScenesManager : Node
{
	public string CurrentSceneName = Global.LevelName;
	public Node CurrentScene;
	public Viewport _root;
	public ResourceInteractiveLoader Loader;
	public int WaitFrame = 0;
	public ulong TimeMax = 100;

	public Viewport Root { get { return _root; } set { _root = value; } }
	
	public void GoToMenu(string MenuName)
	{
		CallDeferred(nameof(DeferredGotoScene), $"res://Scenes/UI/{MenuName}.tscn");
	}

	public void DeferredGotoScene(string path) {
		CurrentScene = Root.GetChild(Root.GetChildCount() - 1);
		CurrentScene.Free();
		var nextScene = (PackedScene)GD.Load(path);
		SetNewScene(nextScene);
		Root.GetTree().CurrentScene = CurrentScene;
	}

	public void SetNewScene(PackedScene Res)
	{
		CurrentScene = Res.Instance();
		Root.AddChild(CurrentScene);
	}

}
