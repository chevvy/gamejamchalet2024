using Godot;
using System;


public partial class Character : CharacterBody2D
{
    public const float Speed = 450.0f;
    private PlayerInput _playerInput;

    private bool _hasItem = false;

    private AnimationPlayer _animationPlayer;
    private bool _canCharacterMove = false;

    public void SetupPlayer(PlayerID id)
    {
        _playerInput = new(id);
        _canCharacterMove = true;
    }

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
        MoveAndSlide();
        KinematicCollision2D kc = MoveAndCollide(Velocity * (float)delta, true);
        if (kc != null)
        {
            if (_hasItem && kc.GetCollider() is Patient patient)
            {
                UseItem(patient);
            }
            else if (!_hasItem && kc.GetCollider() is ItemSpawner item)
            {
                item.QueueFree();
                ReceiveItem(ItemType.BANDAGE);
            }
        }
    }

    public void ReceiveItem(ItemType item)
    {
        _hasItem = true;
        _animationPlayer.Play("flash");
    }

    public void UseItem(Patient patient)
    {
        patient.ReceiveItem();
        _hasItem = false;
        _animationPlayer.Stop();
    }
}

public enum ItemType
{
    BANDAGE,
    SERINGUE
}