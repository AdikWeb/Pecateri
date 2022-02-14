using Godot;

public class Root : Godot.Node
{
    static private ResourceInteractiveLoader Loader;
    static public Node CurrentScene;
    static public int WaitFrame = 0;
    static public ulong TimeMax = 100;
    static public Node RootNode;
    static public bool state = false;
    public override void _Ready()
    {
        InitRoot();
        GoToMainMenu();
    }

    static public void GoToScene(string SceneName)
    {
        string path = $"res://Scenes/{SceneName}.tscn";
        GD.Print(CurrentScene);
        if (ResourceLoader.HasCached(path))
        {
        }
        else
        {
            Loader = ResourceLoader.LoadInteractive(path);
            if (Loader == null) return;

            RootNode.SetProcess(true);
            CurrentScene.QueueFree();
            WaitFrame = 1;
        }
    }

    static public void GoToMainMenu()
    {
        GoToScene("UI/Menu");
    }

    public void InitRoot()
    {
        RootNode = GetTree().Root;
        CurrentScene = RootNode.GetChild(RootNode.GetChildCount() - 1);
    }
    public void SetNewScene(PackedScene Res)
    {
        CurrentScene = Res.Instance();
        RootNode.CallDeferred("add_child", CurrentScene);
    }

    public override void _Process(float delta)
    {
        if (Loader == null)
        {
            RootNode.SetProcess(false);
            return;
        }
        if (WaitFrame > 0)
        {
            WaitFrame -= 1;
            return;
        }

        var t = OS.GetTicksMsec();
        while (OS.GetTicksMsec() < t + TimeMax)
        {
            var err = Loader.Poll();
            if (err == Error.FileEof)
            {
                Resource Resource = Loader.GetResource();
                Loader = null;
                SetNewScene((PackedScene)Resource);
                break;
            }
            else if (err == Error.Ok)
            {
                var progress = (float)Loader.GetStage() / Loader.GetStageCount();
            }
            else
            {
                Loader = null;
                break;
            }
        }
    }
}
