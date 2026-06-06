using MMO.Core;
using Godot;

namespace MMO.Scripts.Players.States;

public partial class Idle : State
{

    public override void _Ready()
    {
        Animation = "idle";
        StateName = "idle";
    }
    public override string CheckRelevance(InputPackage input)
    {
        input.Actions.Sort(MovesPrioritySort);
        if (input.Actions[0] == "idle")
        {
            return "okay";
        }
        return input.Actions[0];


    }

    public override void OnEnterState()
    {
        Player.Velocity = Vector3.Zero;
    }
}
