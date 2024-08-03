using Godot;
using System;


public partial class Character : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;

    private bool _hasItem = false;

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
        KinematicCollision2D kc = MoveAndCollide(Velocity * (float)delta);
        if (kc != null)
        {
            if (_hasItem && kc.GetCollider() is Patient patient)
            {
                UseItem(patient);
            }
            else if (!_hasItem && kc.GetCollider() is ItemSpawner item)
            {
                item.QueueFree();
                ReceiveItem();
            }
        }
    }

    public void ReceiveItem()
    {
        _hasItem = true;
    }

    public void UseItem(Patient patient)
    {
        patient.ReceiveItem();
        _hasItem = false;
    }
}
