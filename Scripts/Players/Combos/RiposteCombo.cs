using Godot;
using MMO.Core;
using MMO.Scripts.Players.States.Combos.Sword;
using System;

namespace MMO.Scripts.Players.Combos;

[GlobalClass]
public partial class RiposteCombo : Combo
{
    public override bool IsTriggered(InputPackage input)
    {
        if (input.Actions.Contains("slash_1") && HaveTargetForRiposte())
        {
            return true;
        }
        return false;
    }

    private bool HaveTargetForRiposte()
    {
        var parriedVictims = GetTree().GetNodesInGroup("parried_humanoid");

        foreach (Node node in parriedVictims)
        {
            if (node is Node3D humanoid)
            {
                // DistanceSquaredTo é matematicamente mais rápido que DistanceTo
                // 2 metros de distância viram 4 (2 * 2) na checagem ao quadrado
                if (humanoid.GlobalPosition.DistanceSquaredTo(State.Player.GlobalPosition) < 4f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
