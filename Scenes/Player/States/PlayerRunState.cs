using Godot;
using System;

public partial class PlayerRunState : State
{
    [Export] private InputComponent _input;
    [Export] private MovementComponent _movement;

    public override void PhysicsUpdate(double delta)
    {
        Vector3 direction = _input.GetDirection();
        _movement.ProcessMovement(direction, delta);

        if (_input.IsJumpPressed() && _movement.IsOnFloor())
        {
            EmitSignal(State.SignalName.Transitioned, this, "Airborne");
            return;
        }

        // Se o jogador soltou as teclas, emite o sinal para voltar ao "Idle"
        if (direction == Vector3.Zero)
        {
            EmitSignal(State.SignalName.Transitioned, this, "Idle");
        }
    }
}
