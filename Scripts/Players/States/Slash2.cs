using Godot;
using MMO.Core;
using MMO.Scripts.Players;

namespace MMO.Scripts.Players.States;

public partial class Slash2 : State
{
    private const float TRANSITION_TIMING = 0.65f;
    private const float COMBO_TIMING = 0.6f;

    private float _hitDamage = 15f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(COMBO_TIMING) && HasQueuedMove)
        {
            HasQueuedMove = false;
            return QueuedMove;
        }
        else if (WorksLongerThan(TRANSITION_TIMING))
        {
            return BestInputThatCanBePaid(input);
        }

        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        if (WorksBetween(0.25f, 0.44f))
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
