using Godot;
using MMO.Core;
using MMO.Scripts.Players.States.Combos.Sword;


namespace MMO.Scripts.Players.States.Combos.Sword;

public partial class ConsecutiveAttack : Combo
{

    [Export]
    public float PanicClickBlock { get; set; }

    [Export]
    public string PrimaryInput { get; set; }

    public override bool IsTriggered(InputPackage input)
    {
        return input.Actions.Contains(PrimaryInput) &&
            State.WorksLongerThan(PanicClickBlock);
    }
}
