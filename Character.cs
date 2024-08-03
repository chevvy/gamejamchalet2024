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

    private bool _hasItem = false;
    private ClosetItemType? _itemType = null;

    private AnimationPlayer _animationPlayer;
    private bool _canCharacterMove = false;

    private AudioStreamPlayer2D _bounceAudioPlayer;

    private float PlaneMovementMalusX = 0;
    private float PlaneMovementMalusY = 0;

    private Vector2 PlaneMovementVectore = Vector2.Zero;
    
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
        _bounceAudioPlayer = GetNode<AudioStreamPlayer2D>("BounceAudioPlayer");

        SetBounceMovementLockTimer();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canCharacterMove || _playerInput == null) return;

        if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.Interact)) && _hasItem && InteractArea.Patient != null)
        {
            UseItem(InteractArea.Patient);
        }

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
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X - PlaneMovementVectore.X, 0, Speed);
                velocity.Y = Mathf.MoveToward(Velocity.Y - PlaneMovementVectore.Y, 0, Speed);
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

        PlayPickupAudio();
    }

    public void UseItem(Patient patient)
    {
        if (_itemType.HasValue)
        {
            patient.ReceiveItem(_itemType.Value);
            _hasItem = false;
            _animationPlayer.Stop();
        }

        PlayUseAudio();
    }

    public void ApplyPlaneMovement(Vector2 direction){

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