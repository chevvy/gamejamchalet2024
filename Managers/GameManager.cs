using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	public Node CurrentScene { get; set; }

	[Signal] public delegate void GameReadyEventHandler();
	[Signal] public delegate void GameWonEventHandler();
	[Signal] public delegate void GameLostEventHandler();

	[Export] public int PatientSaved = 0;
	[Export] public int PatientDead = 0;
	[Export] public int PatientSavedGoal = 8;
	[Export] public int PatientDeadGoal = 6;

	public bool IsGameReady = false;
	public AnimationPlayer AnimationPlayer { get; set; }
	[Export] public AudioStreamPlayer2D MortStreamPlayer;

	public override void _EnterTree()
	{
		GD.Print("GameManager entering tree");
		base._EnterTree();
		if (Instance != null)
			QueueFree();
		else
			Instance = this;
	}

    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionJustPressed("p5_go_to_menu"))
		{
			LoadScene(Scenes.MENU);
		}

		if (Input.IsActionJustPressed("p5_go_to_game"))
		{
			LoadScene(Scenes.GAME);
		}

		if (Input.IsActionJustPressed("p5_go_to_credit"))
		{
			LoadScene(Scenes.CREDIT);
		}
    }

    public override void _Ready()
	{
		base._Ready();
		GD.Print("_Ready on GameManager");
		
		var root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
	}

	public void LoadScene(string path)
	{
		CallDeferred(MethodName.DeferredGotoScene, path);
	}

	public void DeferredGotoScene(string path)
	{
		CurrentScene.Free();

		var nextScene = GD.Load<PackedScene>(path);

		CurrentScene = nextScene.Instantiate();

		GetTree().Root.AddChild(CurrentScene);

		GetTree().CurrentScene = CurrentScene;
	}

    public override void _Process(double delta)
    {
		base._Process(delta);
		
		CheckGameState();
    }

    public void OnGameReady()
	{
		IsGameReady = true;
		EmitSignal(SignalName.GameReady);
		GD.Print("Game ready");

		SubscribeToPatientEvents();
	}

	private void SubscribeToPatientEvents()
	{
		var patients = GetTree().GetNodesInGroup("Patients");
		foreach (Patient patient in patients)
		{
			patient.OnPatientDead += OnPatientDead;
			patient.OnPatientSaved += OnPatientSaved;
		}
	}

	private void OnPatientDead()
	{
		PatientDead++;
	}

	private void OnPatientSaved()
	{
		PatientSaved++;
	}

	private void CheckGameState()
	{
		if (PatientSaved >= PatientSavedGoal)
		{
			OnGameWon();
		}
		else if (PatientDead >= PatientDeadGoal)
		{
			OnGameLost();
		}
	}

	private void ResetGameState()
	{
		PatientSaved = 0;
		PatientDead = 0;
	}

	private void OnGameWon()
	{
		GD.Print("Game won");

		ResetGameState();
		LoadScene(Scenes.CREDIT);
	}

	private void OnGameLost()
	{
		GD.Print("Game lost");
		
		ResetGameState();
		LoadScene(Scenes.CREDIT);
	}

	public void OnMortSounds()
	{
		if (!MortStreamPlayer.Playing)
		{
			MortStreamPlayer.Play();
		}
	}
}
