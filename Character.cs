using Godot;
using System;


public partial class Character : CharacterBody2D
{
    [Export] public int BounceStrength = 5;
    public const float Speed = 450.0f;
    private PlayerInput _playerInput;

    private bool _hasItem = false;
    private ClosetItemType? _itemType = null;

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

        KinematicCollision2D kc = MoveAndCollide(Velocity * (float)delta, true);
        if (kc != null && kc.GetCollider() is Character character)
        {
            velocity += Velocity.Bounce(kc.GetNormal()) * BounceStrength;
            character.Velocity = -Velocity.Bounce(kc.GetNormal()) * BounceStrength;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void ReceiveItem(ClosetItemType item)
    {
        _hasItem = true;
        _itemType = item;

        if (item == ClosetItemType.PILLZ)
        {
            _animationPlayer.Play("pillz");
        }
        if (item == ClosetItemType.SERINGE)
        {
            _animationPlayer.Play("seringe");
        }
        if (item == ClosetItemType.BANDAGE)
        {
            _animationPlayer.Play("bandage");
        }
    }

    public void UseItem(Patient patient)
    {
        if (_itemType.HasValue)
        {
            patient.ReceiveItem(_itemType.Value);
            _hasItem = false;
            _animationPlayer.Stop();
        }
    }
}