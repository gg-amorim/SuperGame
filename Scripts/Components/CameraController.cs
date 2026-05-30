using Godot;

public partial class CameraController : Node3D
{
    [Export] public Node3D Target; // Quem a câmera deve seguir
    [Export] public float SmoothSpeed = 8.0f; // Quão "elástica" é a câmera

    public override void _PhysicsProcess(double delta)
    {
        if (Target != null)
        {
            // A posição global da câmera tenta alcançar a posição global do alvo suavemente
            GlobalPosition = GlobalPosition.Lerp(Target.GlobalPosition, SmoothSpeed * (float)delta);
        }
    }
}