using Godot;
using System;

namespace MMO.Scripts;

public partial class Downcast : RayCast3D
{
    private BoneAttachment3D _rootAttachment;

    public override void _Ready()
	{
        _rootAttachment = GetNode<BoneAttachment3D>("../Root");
    }


	public override void _Process(double delta)
	{
		GlobalPosition = _rootAttachment.GlobalPosition;
	}
}
