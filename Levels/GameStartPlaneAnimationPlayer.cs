using Godot;
using System;

public partial class GameStartPlaneAnimationPlayer : AnimationPlayer
{
	public override void _EnterTree()
	{
		GameManager.Instance.AnimationPlayer = this;
	}

	public void OnAnimationFinished()
	{
		GameManager.Instance.OnGameReady();
	}
}
