using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States;

[GlobalClass]
public partial class Parried : State
{
    // Constante de tempo com o sufixo 'f' (float)
    private const float AnimationEnd = 3f;

    public override string DefaultLifecycle(InputPackage input)
    {
        if (WorksLongerThan(AnimationEnd))
        {
            return BestInputThatCanBePaid(input);
        }

        return "okay";
    }

    public override void OnEnterState()
    {
        Player.AddToGroup("parried_humanoid");
    }

    public override void OnExitState()
    {
        Player.RemoveFromGroup("parried_humanoid");
    }
}