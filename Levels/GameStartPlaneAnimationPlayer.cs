using Godot;
using System;

public partial class GameStartPlaneAnimationPlayer : AnimationPlayer
{
	public override void _EnterTree()
	{
		// GameManager.Instance.AnimationPlayer = this;
	}

	public override void _Ready()
	{
		Play("plane_anim");
	}

	public void OnAnimationFinished()
	{
		GameManager.Instance.OnGameReady();
	}
}
