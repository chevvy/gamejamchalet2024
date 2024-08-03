using Godot;
using System;

public partial class Patient : RigidBody2D
{

    private DecrementingBar decrementingBar;
    private LifeBar lifeBar;

    private Timer healthTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        decrementingBar = GetNode<DecrementingBar>("Bars/DecrementingBar");
        lifeBar = GetNode<LifeBar>("Bars/LifeBar");
        healthTimer = GetNode<Timer>("HealingFromItemTimer");

        GameManager.Instance.GameReady += Initialize;
    }

    private void Initialize()
    {
        healthTimer.Start();
    }

    private void OnTreeExiting()
    {
        GameManager.Instance.GameReady -= Initialize;
    }

    public void ReceiveItem()
    {
        const int ITEM_VALUE = 25;
        decrementingBar.Increase(ITEM_VALUE);
    }

    private void OnHealthTickTimeout()
    {
        if (!decrementingBar.IsEmpty())
        {
            GainHealthFromTick();
        }
    }

    private void GainHealthFromTick()
    {
        lifeBar.Increase(5);
    }
}
