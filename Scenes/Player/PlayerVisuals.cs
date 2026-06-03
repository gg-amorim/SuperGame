using Godot;
using System;

namespace MMO.Scenes.Player;

public partial class PlayerVisuals : Node3D
{
	private MeshInstance3D _betaJoints;
	private MeshInstance3D _betaSurface;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_betaJoints = GetNode<MeshInstance3D>("Beta_Joints");
        _betaSurface = GetNode<MeshInstance3D>("Beta_Surface");

    }

	public  void AcceptSkeleton(Skeleton3D skeleton)
	{
        _betaSurface.Skeleton = skeleton.GetPath();
        _betaJoints.Skeleton = skeleton.GetPath();

    }
}
