using Godot;

public partial class MovementComponent : Node
{
    [Export] public float Speed = 5.0f;
    [Export] public float Acceleration = 10.0f;
    [Export] public float RotationSpeed = 10.0f;
    [Export] public float Gravity = 9.8f;
    [Export] public float JumpForce = 5.0f;

    // Precisamos de referência ao corpo físico e ao visual para movê-los
    [Export] private CharacterBody3D _body;
    [Export] private Node3D _visuals;

    public void ProcessMovement(Vector3 targetDirection, double delta)
    {
        Vector3 currentVelocity = _body.Velocity;

        // 1. Aplica Gravidade
        if (!_body.IsOnFloor())
        {
            currentVelocity.Y -= Gravity * (float)delta;
        }

        // 2. Aplica Aceleração/Movimentação
        currentVelocity.X = Mathf.Lerp(currentVelocity.X, targetDirection.X * Speed, Acceleration * (float)delta);
        currentVelocity.Z = Mathf.Lerp(currentVelocity.Z, targetDirection.Z * Speed, Acceleration * (float)delta);

        _body.Velocity = currentVelocity;
        _body.MoveAndSlide();

        // 3. Aplica Rotação da Malha Visual
        if (targetDirection != Vector3.Zero && _visuals != null)
        {
            float targetAngle = Mathf.Atan2(targetDirection.X, targetDirection.Z);
            Quaternion targetRotation = new Quaternion(Vector3.Up, targetAngle);
            _visuals.Quaternion = _visuals.Quaternion.Slerp(targetRotation, RotationSpeed * (float)delta);
        }
    }

    // Função de conveniência para quando ele estiver parado (desacelerando)
    public void ProcessIdle(double delta)
    {
        ProcessMovement(Vector3.Zero, delta);
    }

    // Verifica se a física da Godot detectou chão
    public bool IsOnFloor() => _body.IsOnFloor();

    // Aplica o impulso vertical
    public void ApplyJump()
    {
        Vector3 currentVelocity = _body.Velocity;
        currentVelocity.Y = JumpForce; // Injeta força para cima
        _body.Velocity = currentVelocity;
    }
}