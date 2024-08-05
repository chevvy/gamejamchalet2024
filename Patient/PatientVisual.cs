using Godot;
using System;

public partial class PatientVisual : Node2D
{

    private AnimationTree player;

    public override void _Ready()
    {
        player = GetNode<AnimationTree>("AnimationTree");
    }

    public void AnimatePatientFeelingGood()
    {
        player.Set("parameters/conditions/STABLE", true);
        player.Set("parameters/conditions/HURRY", false);
    }

    public void AnimatePatientFeelingBad()
    {
        player.Set("parameters/conditions/STABLE", false);
        player.Set("parameters/conditions/HURRY", true);
    }

    public void AnimatePatientDead()
    {
        player.Set("parameters/conditions/DEAD", true);
    }

    public void AnimatePatientFullyHealed()
    {
        player.Set("parameters/conditions/SURVIVE", true);
    }
}
