using Godot;
using MMO.Core;
using MMO.Scripts.Players.States.Combos.Sword;


namespace MMO.Scripts.Players.States.Combos.Sword;

public partial class ConsecutiveAttack : Combo
{
    [Export]
    public State RootSate { get; set; }

    [Export]
    public float PanicClickBlock { get; set; }

    [Export]
    public string PrimaryInput { get; set; }

    [Export]
    public string NextAttack { get; set; }
    public override void _Ready()
    {
        TriggeredMove = NextAttack;
    }

    public override bool IsTriggered(InputPackage input)
    {
        return input.Actions.Contains(PrimaryInput) &&
            RootSate.WorksLongerThan(PanicClickBlock);
    }
}
