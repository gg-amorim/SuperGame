using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States; // Ajuste para a sua pasta de estados

[GlobalClass]
public partial class Parry : State
{

    private const float ParryWindowStart = 0.2f;
    private const float ParryWindowEnd = 1f;
    private const float AnimationEnd = 1.3667f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(AnimationEnd))
        {
            // Nota: No seu PlayerModel.cs, o dicionário foi nomeado como "States",
            // então usamos Player.Model.States ao invés do ".moves" do GDScript.
            if (HasQueuedMove && Resources.CanBePaid(Player.Model.States[QueuedMove]))
            {
                HasQueuedMove = false;
                return QueuedMove;
            }

            return BestInputThatCanBePaid(input);
        }

        return "okay";
    }

    public override void ReactOnHit(HitData hit)
    {
        if (WorksBetween(ParryWindowStart, ParryWindowEnd) && hit.IsParryable)
        {
            hit.Weapon.Holder.CurrentState.ReactOnParry(hit);

            GD.Print("parry kong");
        }
        else
        {
            TryForceMove("staggered");
        }
    }
}