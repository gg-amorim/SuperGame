using Godot;
using MMO.Core;
using MMO.Scripts.Components;

namespace MMO.Scenes.Player;

public partial class Player : CharacterBody3D
{
    private InputComponent _inputGatherer;
    private PlayerModel _model;
    private PlayerVisuals _visuals;

    public override void _Ready()
    {
        _inputGatherer = GetNode<InputComponent>("Components/InputComponent");
        _model = GetNode<PlayerModel>("Model");
        _visuals = GetNode<PlayerVisuals>("Visuals");

        _visuals.AcceptSkeleton(_model.Skeleton);
        _model.Animator.Play("run");
    }

    public override void _PhysicsProcess(double delta)
    {
        InputPackage input = _inputGatherer.GatherInput();
        _model.UpdateModel(input, (float)delta);
    }
}