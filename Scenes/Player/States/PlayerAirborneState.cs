using Godot;
using System;

public partial class PlayerAirborneState : State
{
    [Export] private InputComponent _input;
    [Export] private MovementComponent _movement;

    public override void Enter()
    {
        // Assim que entra no estado, aplica a força do pulo!
        _movement.ApplyJump();
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector3 direction = _input.GetDirection();

        // Continua processando movimento e gravidade enquanto está no ar
        _movement.ProcessMovement(direction, delta);

        // Verifica se tocou o chão novamente
        if (_movement.IsOnFloor())
        {
            if (direction != Vector3.Zero)
            {
                EmitSignal(State.SignalName.Transitioned, this, "Run");
            }
            else
            {
                EmitSignal(State.SignalName.Transitioned, this, "Idle");
            }
        }
    }
}