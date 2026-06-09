using Godot;
using MMO.Core;
using System;


namespace MMO.Scripts.Players.States;

public partial class Slash3 : State
{
    private const float TRANSITION_TIMING = 1.96f;

    public override void _Ready()
    {
        Animation = "slash_3";
        StateName = "slash_3";
    }
    public override string CheckRelevance(InputPackage input)
    {

        if (WorksLongerThan(TRANSITION_TIMING))
        {
            input.Actions.Sort(MovesPrioritySort);
            return input.Actions[0];
        }
        return "okay";
    }

    public override void OnEnterState()
    {
        Player.Velocity = Vector3.Zero;
    }
}
