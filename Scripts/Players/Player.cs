using Godot;
using MMO.Core;
using MMO.Scripts.Components;

namespace MMO.Scripts.Players;

public partial class Player : CharacterBody3D
{
    private InputComponent _inputGatherer;
    private PlayerModel _model;
    private Node3D _cameraMount;
    private PlayerVisuals _visuals;
    public Node3D CameraMount => _cameraMount;
    public PlayerVisuals Visuals => _visuals;
    public PlayerModel Model => _model;
    public override void _Ready()
    {
        _inputGatherer = GetNode<InputComponent>("Components/InputComponent");
        _model = GetNode<PlayerModel>("Model");
        _visuals = GetNode<PlayerVisuals>("Visuals");
        _cameraMount = GetNode<Node3D>("SpringArmoPivot");

        _visuals.AcceptModel(_model);
        _model.Animator.Play("run");
    }

    public override void _PhysicsProcess(double delta)
    {

        InputPackage input = _inputGatherer.GatherInput();
        _model.UpdateModel(input, (float)delta);

        input.QueueFree();
    }
}