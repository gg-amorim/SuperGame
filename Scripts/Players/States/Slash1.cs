using Godot;
using MMO.Core;
using System;

namespace MMO.Scripts.Players.States;

public partial class Slash1 : State
{
    private const float TRANSITION_TIMING = 0.9f;
    private const float ANIMATION_END = 1.8f;

    private float _hitDamage = 10f;
    private int _rootMotionTrackNumber;

    // Adicionamos uma variável para guardar a animação
    private Animation _slashAnimation;

    public override void _Ready()
    {
        // Salvamos a animação na memória uma única vez ao iniciar o jogo
        _slashAnimation = Animator.GetAnimation(AnimationStr);
        _rootMotionTrackNumber = _slashAnimation.FindTrack("%GeneralSkeleton:Hips", Animation.TrackType.Position3D);
    }

    public override string DefaultLifecycle(InputPackage input)
    {
        string bestInput = BestInputThatCanBePaid(input);

        if (WorksLongerThan(TRANSITION_TIMING))
        {
            if (HasQueuedMove && Resources.CanBePaid(Player.Model.States[QueuedMove]))
            {
                HasQueuedMove = false;
                return QueuedMove;
            }
            else if (bestInput != "idle")
            {
                return bestInput;
            }
            else if (WorksLongerThan(ANIMATION_END))
            {
                return bestInput;
            }
        }

        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        MovePlayer(delta);

        if (WorksBetween(0.5419f, 0.7943f))
        {
            Player.Model.ActiveWeapon.IsAttacking = true;
        }
        else
        {
            Player.Model.ActiveWeapon.IsAttacking = false;
        }
    }

    private void MovePlayer(float delta)
    {
        Player.Velocity = Player.Quaternion * GetDeltaPosition(delta) / delta;

        if (!Player.IsOnFloor())
        {
            float defaultGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

            Vector3 velocity = Player.Velocity;
            velocity.Y -= defaultGravity * delta;
            Player.Velocity = velocity;

            HasForcedMove = true;
            ForcedMove = "midair";
        }

        Player.MoveAndSlide();
    }

    private Vector3 GetDeltaPosition(float deltaTime)
    {
        // Usamos a animação já salva. Fim da alocação de memória a cada frame!
        Vector3 previousPos = (Vector3)_slashAnimation.PositionTrackInterpolate(_rootMotionTrackNumber, GetProgress() - deltaTime);
        Vector3 currentPos = (Vector3)_slashAnimation.PositionTrackInterpolate(_rootMotionTrackNumber, GetProgress());

        Vector3 deltaPos = currentPos - previousPos;
        deltaPos.Y = 0f;

        return deltaPos;
    }

    public override HitData FormHitData(Weapon weapon)
    {
        HitData hit = new HitData();
        hit.Damage = _hitDamage;
        hit.HitMoveAnimation = AnimationStr;
        hit.IsParryable = IsParryable();
        hit.Weapon = Player.Model.ActiveWeapon;

        return hit;
    }

    public override void OnExitState()
    {
        Player.Model.ActiveWeapon.HitboxIgnoreList.Clear();
        Player.Model.ActiveWeapon.IsAttacking = false;
    }
}
