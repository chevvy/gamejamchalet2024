using Godot;
using System;

public partial class CharacterVisual : Node2D
{

	private AnimationPlayer _player;

	public override void _Ready()
	{
		_player = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void AnimateMoveLeft(bool isIdle)
	{
		if (isIdle)
		{
			_player.Play("Idle_Left");
		}
		else
		{
			_player.Play("Walk_Left");
		}
	}

	public void AnimateMoveRight(bool isIdle)
	{
		if (isIdle)
		{
			_player.Play("Idle_Right");
		}
		else
		{
			_player.Play("Walk_Right");
		}
	}

	public void AnimateMoveUp(bool isIdle)
	{
		if (isIdle)
		{
			_player.Play("Idle_Front");
		}
		else
		{
			_player.Play("Walk_Front");
		}
	}

	public void AnimateMoveDown(bool isIdle)
	{
		if (isIdle)
		{
			_player.Play("Idle_Back");
		}
		else
		{
			_player.Play("Walk_Back");
		}
	}
}
