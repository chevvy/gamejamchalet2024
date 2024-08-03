using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerManager : Node2D
{
    [Export] public PackedScene Character;
    [Export] public Marker2D[] PlayerSpawnPoint;

    // TODO convert to array
    private Dictionary<string, PlayerID> _playerIDbyStartKey = new()
    {
        { "p1_start", PlayerID.P1 },
        { "p2_start", PlayerID.P2 },
        { "p3_start", PlayerID.P3 },
        { "p4_start", PlayerID.P4 },
        { "p5_start", PlayerID.P5 }
    };


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Character == null)
        {
            GD.PrintErr("Missing character packed scene on PlayerManager");
        }

        if (PlayerSpawnPoint == null)
        {
            GD.PrintErr("Missing PlayerSpawnPoints on PlayerManager");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (var item in _playerIDbyStartKey.ToList())
        {
            if (!Input.IsActionJustPressed(item.Key)) continue;

            var character = Character.Instantiate() as Character;
            character.GlobalTransform = item.Value == PlayerID.P5
                ? PlayerSpawnPoint[(int)PlayerID.P1].GlobalTransform
                : PlayerSpawnPoint[(int)item.Value].GlobalTransform;

            character.SetupPlayer(item.Value);
            AddChild(character);
        }
    }
}