using Godot;
using System;

namespace MMO.Scripts.Players;

public partial class PlayerVisuals : Node3D
{
	private MeshInstance3D _betaJoints;
	private MeshInstance3D _betaSurface;
	private Node3D _swordVisual;
	private PlayerModel _playerModel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

        _betaJoints = GetNode<MeshInstance3D>("Beta_Joints");
        _betaSurface = GetNode<MeshInstance3D>("Beta_Surface");
        _swordVisual = GetNode<Node3D>("SwordVisuals");
    }

    public override void _Process(double delta)
    {

        AdjustWeaponVisuals();

    }

    private void AdjustWeaponVisuals()
    {
        _swordVisual.GlobalTransform = _playerModel.ActiveWeapon.GlobalTransform;
    }

    public void AcceptModel(PlayerModel playerModel)
    {
        _playerModel = playerModel;
        _betaSurface.Skeleton = _playerModel.Skeleton.GetPath();
        _betaJoints.Skeleton = _playerModel.Skeleton.GetPath();

    }
}
