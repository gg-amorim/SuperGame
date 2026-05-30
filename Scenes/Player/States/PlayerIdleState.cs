using Godot;

public partial class PlayerIdleState : State
{
    // Precisamos referenciar os componentes para ler input e mover
    [Export] private InputComponent _input;
    [Export] private MovementComponent _movement;

    public override void PhysicsUpdate(double delta)
    {
        _movement.ProcessIdle(delta);

        if (_input.IsJumpPressed() && _movement.IsOnFloor())
        {
            EmitSignal(State.SignalName.Transitioned, this, "Airborne");
            return; // Interrompe o resto da função
        }

        // Verifica o teclado
        Vector3 direction = _input.GetDirection();

        // Se o jogador pressionou algo, emite o sinal para a StateMachine trocar para "Run"
        if (direction != Vector3.Zero)
        {
            EmitSignal(State.SignalName.Transitioned, this, "Run");
        }
    }
}
