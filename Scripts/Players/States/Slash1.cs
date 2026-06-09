using Godot;
using MMO.Core;
using System;

namespace MMO.Scripts.Players.States;

public partial class Slash1 : State
{
    private const float COMBO_TIMING = 0.97f;
    private const float TRANSITION_TIMING = 1.1333f;

    public override void _Ready()
    {
        Animation = "slash_1";
        StateName = "slash_1";
    }
    public override string CheckRelevance(InputPackage input)
    {
        CheckCombos(input);
        // Agora o tempo vai avançar e ele conseguirá sair daqui!
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
