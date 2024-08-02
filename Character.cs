using Godot;
using System;


public partial class Character : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private PlayerInput _playerInput;

	private bool _canCharacterMove = false;

	public void SetupPlayer(PlayerID id)
	{
		_playerInput = new(id);
		_canCharacterMove = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_canCharacterMove || _playerInput == null) return;

		Vector2 velocity = Velocity;
		GD.Print(velocity);

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector(
			_playerInput.GetInputKey(InputAction.MoveLeft),
			_playerInput.GetInputKey(InputAction.MoveRight),
			_playerInput.GetInputKey(InputAction.MoveUp),
			_playerInput.GetInputKey(InputAction.MoveDown)
		);
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
