using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States.Combos.Sword;

public partial class Combo : Node
{
	public string TriggeredMove {  get; set; }

	public virtual bool IsTriggered(InputPackage _) => false;
}
