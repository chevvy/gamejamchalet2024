using Godot;
using System;

public partial class Character : CharacterBody2D
{
    [Export] public AudioStream BounceAudio;
    [Export] public AudioStream PickupAudio;
    [Export] public AudioStream UseAudio;

    [Export] public int BounceStrength = 2;
    private bool _isBouncing = false;
    private float _bounceLockDuration = 0.1f;
    private Timer _bouceDurationTimer;

    public const float Speed = 850.0f;
    private PlayerInput _playerInput;

    public bool CanReceiveItem = false;
    public ClosetItemType? _itemType = null;
    public ClosetItemType? _lastBumpedItemType = null;

    private AnimationPlayer _animationPlayer;
    private Sprite2D _itemHeld;
    private bool _canCharacterMove = false;

    private AudioStreamPlayer2D _bounceAudioPlayer;

    private CharacterVisual _characterVisual;
    private Vector2? lastDirection = new Vector2(0, 1);

    private float PlaneMovementMalusX = 0;
    private float PlaneMovementMalusY = 0;

    private Vector2 PlaneMovementVectore = Vector2.Zero;

    private PackedScene _player1;
    private PackedScene _player2;
    private PackedScene _player3;
    private PackedScene _player4;

    [ExportGroup("Patient")]
    [Export]
    public InteractArea InteractArea;

    public void SetupPlayer(PlayerID id)
    {
        _playerInput = new(id);
        _canCharacterMove = true;
    }

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _bounceAudioPlayer = GetNode<AudioStreamPlayer2D>("PlayerEffectsAudioPlayer");

        _itemHeld = GetNode<Sprite2D>("Sprite2D/ItemHeld");
        _itemHeld.Visible = false;

        _player1 = ResourceLoader.Load<PackedScene>("res://Assets/Characters/character_visual_01.tscn");
        _player2 = ResourceLoader.Load<PackedScene>("res://Assets/Characters/character_visual_02.tscn");
        _player3 = ResourceLoader.Load<PackedScene>("res://Assets/Characters/character_visual_03.tscn");
        _player4 = ResourceLoader.Load<PackedScene>("res://Assets/Characters/character_visual_04.tscn");

        if (_playerInput.GetID() == PlayerID.P1)
        {
            _characterVisual = _player1.Instantiate<CharacterVisual>();
        }
        else if (_playerInput.GetID() == PlayerID.P2)
        {
            _characterVisual = _player2.Instantiate<CharacterVisual>();
        }
        else if (_playerInput.GetID() == PlayerID.P3)
        {
            _characterVisual = _player3.Instantiate<CharacterVisual>();
        }
        else if (_playerInput.GetID() == PlayerID.P4)
        {
            _characterVisual = _player4.Instantiate<CharacterVisual>();
        }
        else
        {
            _characterVisual = _player1.Instantiate<CharacterVisual>();
        }

        AddChild(_characterVisual);
        MoveChild(_characterVisual, 1);

        SetBounceMovementLockTimer();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.Interact)))
        {
            if (CanReceiveItem && _lastBumpedItemType != null)
            {
                ReceiveItem((ClosetItemType)_lastBumpedItemType);
            }
            // YES WE CAN SIMPLIFY THIS
            if (_itemType.HasValue && InteractArea.Patient != null && InteractArea.Patient.IsPatientAlive() && InteractArea.Patient.ItemNeededByPatient(_itemType.Value))
            {
                UseItem(InteractArea.Patient);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canCharacterMove || _playerInput == null) return;

        Vector2 velocity = Velocity;

        if (!_isBouncing)
        {
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
                HandleMovingAnimation(velocity);
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X - PlaneMovementVectore.X, 0, Speed);
                velocity.Y = Mathf.MoveToward(Velocity.Y - PlaneMovementVectore.Y, 0, Speed);
                HandleIdleAnimation();
            }
        }

        KinematicCollision2D kc = MoveAndCollide(Velocity * (float)delta, true);
        if (kc != null && kc.GetCollider() is Character character)
        {
            velocity += Velocity.Bounce(kc.GetNormal()) * BounceStrength;
            character.Velocity = -Velocity.Bounce(kc.GetNormal()) * BounceStrength;

            LockMovementInput(_bounceLockDuration);
            PlayBounceAudio();
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void HandleMovingAnimation(Vector2 velocity)
    {
        Vector2 oneVector = velocity.Normalized().Snapped(Vector2.One);
        if (oneVector.X == 0 && oneVector.Y == 1)
        {
            _characterVisual.AnimateMoveUp(false);
        }
        else if (oneVector.X == 0 && oneVector.Y == -1)
        {
            _characterVisual.AnimateMoveDown(false);
        }
        else if (oneVector.X == 1 && oneVector.Y == 0)
        {
            _characterVisual.AnimateMoveRight(false);
        }
        else if (oneVector.X == -1 && oneVector.Y == 0)
        {
            _characterVisual.AnimateMoveLeft(false);
        }
        lastDirection = oneVector;
    }

    private void HandleIdleAnimation()
    {
        if (lastDirection.HasValue)
        {
            Vector2 vector = lastDirection.Value;
            if (vector.X == 0 && vector.Y == 1)
            {
                _characterVisual.AnimateMoveUp(true);
            }
            else if (vector.X == 0 && vector.Y == -1)
            {
                _characterVisual.AnimateMoveDown(true);
            }
            else if (vector.X == 1 && vector.Y == 0)
            {
                _characterVisual.AnimateMoveRight(true);
            }
            else if (vector.X == -1 && vector.Y == 0)
            {
                _characterVisual.AnimateMoveLeft(true);
            }
        }
        lastDirection = null;
    }

    public void ReceiveItem(ClosetItemType item)
    {
        _itemType = item;

        _itemHeld.Texture = ItemHelper.TextureFromItem(item);
        _itemHeld.Visible = true;

        PlayPickupAudio();
    }

    public void UseItem(Patient patient)
    {
        if (_itemType.HasValue)
        {
            patient.ReceiveItem(_itemType.Value);
            _itemHeld.Visible = false;
            CanReceiveItem = false;
            _animationPlayer.Stop();
            _itemType = null;
        }

        PlayUseAudio();
    }

    public void ApplyPlaneMovement(Vector2 direction)
    {

        PlaneMovementVectore = direction;
    }

    private void SetBounceMovementLockTimer()
    {
        _bouceDurationTimer = new Timer();
        _bouceDurationTimer.OneShot = false;
        _bouceDurationTimer.Timeout += OnMovementInputLockTimeout;
        AddChild(_bouceDurationTimer);
    }

    private void LockMovementInput(float duration)
    {
        _isBouncing = true;
        _bouceDurationTimer.Start(_bounceLockDuration);
    }

    private void OnMovementInputLockTimeout()
    {
        _isBouncing = false;
    }

    private void PlayBounceAudio()
    {
        _bounceAudioPlayer.Stream = BounceAudio;
        _bounceAudioPlayer.Play();
    }

    private void PlayPickupAudio()
    {
        _bounceAudioPlayer.Stream = PickupAudio;
        _bounceAudioPlayer.Play();
    }

    private void PlayUseAudio()
    {
        _bounceAudioPlayer.Stream = UseAudio;
        _bounceAudioPlayer.Play();
    }
}