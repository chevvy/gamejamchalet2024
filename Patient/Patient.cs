using Godot;
using System;

public partial class Patient : RigidBody2D
{

    [Signal]
    public delegate void OnPatientDeadEventHandler();

    private const int HEALTH_AMT = 1;

    private Node2D bars;
    private LifeBar lifeBar;

    private Timer healthTimer;

    private bool isAlive = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bars = GetNode<Node2D>("Bars");
        lifeBar = GetNode<LifeBar>("Bars/LifeBar");
        healthTimer = GetNode<Timer>("HealingFromItemTimer");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameReady += Initialize;
        }

        lifeBar.OnLifeZero += Die;
    }

    private void Initialize()
    {
        healthTimer.Start();
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
