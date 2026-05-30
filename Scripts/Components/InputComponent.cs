using Godot;

public partial class InputComponent : Node
{
    // Usamos [Export] para podermos arrastar a referência do nó no Inspetor
    [Export] private Node3D _cameraPivot;

    public Vector3 GetDirection()
    {
        // 1. Lê as teclas
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y);

        // 2. Converte para a perspectiva da câmera (se ela existir)
        if (_cameraPivot != null && direction != Vector3.Zero)
        {
            direction = direction.Rotated(Vector3.Up, _cameraPivot.Rotation.Y).Normalized();
        }

        return direction;
    }

    public bool IsJumpPressed() => Input.IsActionJustPressed("jump");
}