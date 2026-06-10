using MMO.Core;
using Godot;

namespace MMO.Scripts.Players.States;

public partial class Idle : State
{
    public override string DefaultLifecycle(InputPackage input)
    {
        if (!Player.IsOnFloor()) return "midair";


        if (HasQueuedMove && Resources.CanBePaid(Player.Model.States[QueuedMove]))
        {
            HasQueuedMove = false;
            return QueuedMove;
        }
        return BestInputThatCanBePaid(input);


    }

    public override void OnEnterState()
    {
        Player.Velocity = Vector3.Zero;
    }
}
