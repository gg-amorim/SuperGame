using Godot;
using MMO.Core;
using MMO.Scripts.Players;

namespace MMO.Scripts.Players.States;

public partial class Slash2 : State
{
    private const float COMBO_TIMING = 0.6f;
    private const float TRANSITION_TIMING = 0.65f;

    public override void _Ready()
    {
        Animation = "slash_2";
        StateName = "slash_2";
    }
    public override string CheckRelevance(InputPackage input)
    {
        CheckCombos(input);
        if (WorksLongerThan(COMBO_TIMING) && HasQueuedMove)
        {
            HasQueuedMove = false;
            return QueuedMove;
        }
        else if (WorksLongerThan(TRANSITION_TIMING))
        {
            input.Actions.Sort(MovesPrioritySort);
            return input.Actions[0];
        }
        else return "okay";
    }

    public override void OnEnterState()
    {
        Player.Velocity = Vector3.Zero;
    }
}
