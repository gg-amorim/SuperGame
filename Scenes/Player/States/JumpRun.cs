using Godot;
using MMO.Core;

namespace MMO.Scenes.Player.States;

public partial class JumpRun : State
{
    private const float VERTICAL_SPEED_ADDED = 2.5f;
    private const float TRANSITION_TIMING = 0.44f;
    private const float JUMP_TIMING = 0.1f;

    private bool _jumped = false;

    public override void _Ready()
    {
        Animation = "jump_run";
        StateName = "jump_run";
    }

    // Boa prática: Sempre zerar variáveis de controle quando ENTRAR no estado
    public override void OnEnterState()
    {
        _jumped = false;
    }

    public override string CheckRelevance(InputPackage input)
    {
        // Agora o tempo vai avançar e ele conseguirá sair daqui!
        if (WorksLongerThan(TRANSITION_TIMING))
        {
            return "midair";
        }

        return "okay";
    }

    public override void Update(InputPackage input, float delta)
    {
        // A LINHA MÁGICA: Chama o Update da classe base (State)
        // Isso faz o cronômetro interno (TimeInState) do WorksLongerThan avançar!
        base.Update(input, delta);

        if (WorksLongerThan(JUMP_TIMING))
        {
            if (!_jumped)
            {
                Vector3 velocity = Player.Velocity;
                velocity.Y = VERTICAL_SPEED_ADDED;
                Player.Velocity = velocity;
                _jumped = true;
            }
        }

        Player.MoveAndSlide();
    }
}