using Godot;
using System;

namespace MMO.Scripts.Components;

public partial class CameraController : Node3D
{
    // Cria um grupo principal no Inspector
    [ExportGroup("Configurações da Câmara")]
    [Export]
    public float MouseSensibility { get; set; } = 0.005f;

    // Cria um subgrupo dentro do grupo anterior para variáveis relacionadas
    [ExportSubgroup("Limites de Rotação Vertical")]
    [Export(PropertyHint.Range, "-90,0,0.1,radians_as_degrees")]
    public float MinVerticalAngle { get; set; } = -Mathf.Pi / 2;

    [Export(PropertyHint.Range, "0,90,0.1,radians_as_degrees")]
    public float MaxVerticalAngle { get; set; } = Mathf.Pi / 4;

    // Inicia um novo grupo principal
    [ExportGroup("Configurações de Zoom")]
    [Export]
    public float MinZoomLength { get; set; } = 1.5f;

    [Export]
    public float MaxZoomLength { get; set; } = 6.0f;

    [Export]
    public float ZoomStep { get; set; } = 0.5f;

    private SpringArm3D _springArm;

    public override void _Ready()
    {
        _springArm = GetNode<SpringArm3D>("SpringArm3D");
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _springArm.SpringLength = Mathf.Clamp(_springArm.SpringLength, MinZoomLength, MaxZoomLength);

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            Vector3 rot = Rotation;

            rot.Y -= mouseMotion.Relative.X * MouseSensibility;
            rot.Y = Mathf.Wrap(rot.Y, 0.0f, Mathf.Tau);

            rot.X -= mouseMotion.Relative.Y * MouseSensibility;
            rot.X = Mathf.Clamp(rot.X, MinVerticalAngle, MaxVerticalAngle);

            Rotation = rot;
        }

        if (@event.IsActionPressed("wheel_up"))
        {
            _springArm.SpringLength = Mathf.Clamp(_springArm.SpringLength - ZoomStep, MinZoomLength, MaxZoomLength);
        }
        if (@event.IsActionPressed("wheel_down"))
        {
            _springArm.SpringLength = Mathf.Clamp(_springArm.SpringLength + ZoomStep, MinZoomLength, MaxZoomLength);
        }

        if (@event.IsActionPressed("toggle_mouse_capture"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }
    }
}