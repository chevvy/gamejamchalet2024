using Godot;
using Godot.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Patient : RigidBody2D
{

    [Signal]
    public delegate void OnPatientDeadEventHandler();

    [Signal]
    public delegate void OnPatientSavedEventHandler();

    [Export]
    private const int HOW_MANY_DEMANDS = 5;

    [Export]
    private Texture2D BED_EMPTY_TEXT;

    [Export]
    private int DEMAND_RATE = 5;

    private Timer demandsTimer;
    private Timer timeUntilDeadTimer;
    private Timer timeUntilShakeTimer;

    private Node2D itemHolder;
    private Sprite2D itemDemanded;
    private Sprite2D patientSprite;

    private List<ClosetItemType> demands = new();
    private ClosetItemType? _currentDemand = null;

    private bool isAlive = true;

    private AnimationPlayer player;
    private AudioStreamPlayer2D audioDeathEffectPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        demandsTimer = GetNode<Timer>("DemandsTimer");
        timeUntilDeadTimer = GetNode<Timer>("TimeUntilDeadTimer");
        timeUntilShakeTimer = GetNode<Timer>("TimeUntilShakeTimer");
        itemHolder = GetNode<Node2D>("ItemHolder");
        itemHolder.Visible = false;
        itemDemanded = GetNode<Sprite2D>("ItemHolder/ItemDemanded");
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        patientSprite = GetNode<Sprite2D>("PatientSprite");
        audioDeathEffectPlayer = GetNode<AudioStreamPlayer2D>("SonsMort");

        GetNode<GameManager>("/root/GameManager").GameReady += Initialize;
    }

    private void Initialize()
    {
        CreateRandomDemands();

        demandsTimer.WaitTime = new Random().Next(5, 15);
        demandsTimer.Start();
    }

    private void OnTreeExiting()
    {
        GetNode<GameManager>("/root/GameManager").GameReady -= Initialize;
    }

    public bool IsPatientAlive()
    {
        return isAlive;
    }

    public bool ItemNeededByPatient(ClosetItemType item)
    {
        return _currentDemand.HasValue && _currentDemand.Value == item;
    }

    public bool CanBeHealed(ClosetItemType? item)
    {
        if (item is ClosetItemType validItemType)
        {
            return IsPatientAlive() && ItemNeededByPatient(validItemType);
        };
        return false;
    }

    public void ReceiveItem(ClosetItemType item)
    {
        if (_currentDemand.HasValue && item == _currentDemand)
        {
            DemandMet();
        }
    }

    private void CreateRandomDemands()
    {
        Random r = new Random();
        for (int i = 0; i < HOW_MANY_DEMANDS; ++i)
        {
            demands.Add((ClosetItemType)r.Next(3));
        }
    }

    private void OnDemandTimerTimeout()
    {
        if (demands.Count == 0)
        {
            StartPatientSaveEvent();
            return;
        }
        _currentDemand = demands.First();
        demands.RemoveAt(0);
        demandsTimer.WaitTime = DEMAND_RATE;
        demandsTimer.Stop();

        StartDemand();
    }

    private void StartDemand()
    {
        if (!_currentDemand.HasValue)
        {
            GD.PrintErr("Incorrect patient demand ...");
            return;
        }

        if (!isAlive)
        {
            GD.PrintErr("Shouldn't create new patient demands while dead ...");
            return;
        }

        itemHolder.Visible = true;
        itemDemanded.Texture = ItemHelper.TextureFromItem(_currentDemand.Value);

        timeUntilDeadTimer.Start();
        timeUntilShakeTimer.Start();
        player.Play("hurry");
    }

    private void DemandMet()
    {
        timeUntilDeadTimer.Stop();
        timeUntilShakeTimer.Stop();

        _currentDemand = null;
        itemHolder.Visible = false;
        player.Stop();
        demandsTimer.Start();
    }

    private void OnDeathTimer()
    {
        StartPatientDeathEvent();
    }

    private void OnTimerUntilShake()
    {
        player.Play("shake");

        audioDeathEffectPlayer.Play();
    }

    private void StartPatientSaveEvent()
    {
        if (isAlive)
        {
            GD.Print("Patient is saved");
            EmitSignal(SignalName.OnPatientSaved);
            GetParent().RemoveChild(this);
            QueueFree(); //TODO Win animation
        }
    }

    private void StartPatientDeathEvent()
    {
        if (isAlive)
        {
            isAlive = false;
            GD.Print("Patient dying");
            EmitSignal(SignalName.OnPatientDead);

            patientSprite.Texture = BED_EMPTY_TEXT;
            itemHolder.Visible = false;

            //GetParent().RemoveChild(this);
            //QueueFree(); //TODO Dead animation or remove the sprite ???
        }
    }
}
