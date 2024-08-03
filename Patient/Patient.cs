using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Patient : RigidBody2D
{

    [Signal]
    public delegate void OnPatientDeadEventHandler();

    [Signal]
    public delegate void OnPatientSavedEventHandler();

    private const int HEALTH_AMT = 1;

    [Export]
    private const int HOW_MANY_DEMANDS = 3;

    private Node2D bars;
    private LifeBar lifeBar;

    private Timer healthTimer;
    private Timer demandsTimer;
    private Timer timeUntilDeadTimer;

    private Node2D itemHolder;
    private Sprite2D itemDemanded;

    private List<ClosetItemType> demands = new();
    private ClosetItemType? currentDemand = null;

    private bool isAlive = true;

    private AnimationPlayer player;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bars = GetNode<Node2D>("Bars");
        lifeBar = GetNode<LifeBar>("Bars/LifeBar");
        healthTimer = GetNode<Timer>("HealingFromItemTimer");

        demandsTimer = GetNode<Timer>("DemandsTimer");
        timeUntilDeadTimer = GetNode<Timer>("TimeUntilDeadTimer");
        itemHolder = GetNode<Node2D>("ItemHolder");
        itemHolder.Visible = false;
        itemDemanded = GetNode<Sprite2D>("ItemHolder/ItemDemanded");
        player = GetNode<AnimationPlayer>("AnimationPlayer");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameReady += Initialize;
        }

        //lifeBar.OnLifeZero += Die;
    }

    private void Initialize()
    {
        CreateRandomDemands();
        healthTimer.Start();
        demandsTimer.Start();
    }

    private void OnTreeExiting()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameReady -= Initialize;
        }
    }

    public void ReceiveItem(ClosetItemType item)
    {
        if (currentDemand.HasValue && item == currentDemand)
        {
            DemandMet();
        }

        // Old mechanic ...
        const int ITEM_VALUE = 25;
        foreach (Node child in bars.GetChildren())
        {
            if (child is DecrementingBar bar && bar.itemType == item)
            {
                if (item == ClosetItemType.BANDAGE)
                {
                    bar.Increase(ITEM_VALUE);
                }
                if (item == ClosetItemType.PILLZ)
                {
                    bar.Increase(ITEM_VALUE);
                }
                if (item == ClosetItemType.SERINGE)
                {
                    bar.Increase(ITEM_VALUE);
                }
            }
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
            GD.Print("Patient saved!");
            return;
        }
        currentDemand = demands.First();
        demands.RemoveAt(0);
        demandsTimer.Stop();

        StartDemand();
    }

    private void StartDemand()
    {
        if (!currentDemand.HasValue)
        {
            GD.PrintErr("Incorrect patient demand ...");
            return;
        }

        itemHolder.Visible = true;
        itemDemanded.Texture = ItemHelper.TextureFromItem(currentDemand.Value);

        timeUntilDeadTimer.Start();
        player.Play("hurry");
    }

    private void DemandMet()
    {
        currentDemand = null;
        itemHolder.Visible = false;
        player.Stop();
        demandsTimer.Start();
    }

    private void OnDeathTimer()
    {
        Die();
    }

    private void OnHealthTickTimeout()
    {
        foreach (Node child in bars.GetChildren())
        {
            if (child is DecrementingBar bar)
            {
                if (!bar.IsEmpty())
                {
                    GainHealthFromTick();
                    break;
                }
            }
        }
    }

    private void GainHealthFromTick()
    {
        lifeBar.Increase(HEALTH_AMT);
    }

    private void PatientSaved()
    {
        if (isAlive)
        {
            EmitSignal(SignalName.OnPatientSaved);
            GetParent().RemoveChild(this);
            QueueFree(); //TODO Win animation
        }
    }

    private void Die()
    {
        if (isAlive)
        {
            isAlive = false;
            GD.Print("Patient dying");
            EmitSignal(SignalName.OnPatientDead);

            GetParent().RemoveChild(this);
            QueueFree(); //TODO Dead animation or remove the sprite ???
        }
    }
}
