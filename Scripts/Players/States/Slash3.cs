using Godot;
using MMO.Core;
using System;


namespace MMO.Scripts.Players.States;

public partial class Slash3 : State
{
    private const float TRANSITION_TIMING = 1.96f;

    private float _hitDamage = 15f;
    private int _rootMotionTrackNumber;

    // Variável para o Cache
    private Animation _slashAnimation;

    public override void _Ready()
    {
        // Cache da animação
        _slashAnimation = Animator.GetAnimation(AnimationStr);
        _rootMotionTrackNumber = _slashAnimation.FindTrack("%GeneralSkeleton:Hips", Animation.TrackType.Position3D);
    }

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(TRANSITION_TIMING))
        {
            return BestInputThatCanBePaid(input);
        }
        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        ManageWeaponAttack();
        MovePlayer(delta);
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
        // Interpolação direto da animação em cache
        Vector3 previousPos = (Vector3)_slashAnimation.PositionTrackInterpolate(_rootMotionTrackNumber, GetProgress() - deltaTime);
        Vector3 currentPos = (Vector3)_slashAnimation.PositionTrackInterpolate(_rootMotionTrackNumber, GetProgress());

        Vector3 deltaPos = currentPos - previousPos;
        deltaPos.Y = 0f;

        return deltaPos;
    }

    private void ManageWeaponAttack()
    {
        if (WorksBetween(0.6816f, 0.7765f))
        {
            Player.Model.ActiveWeapon.IsAttacking = true;
        }
        else
        {
            Player.Model.ActiveWeapon.IsAttacking = false;
        }
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
