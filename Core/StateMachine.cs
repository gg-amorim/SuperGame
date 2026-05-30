using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    [Export] public State InitialState;
    private State _currentState;
    private Dictionary<string, State> _states = new Dictionary<string, State>();

    public override void _Ready()
    {
        // 1. Vasculha todos os nós filhos procurando pelos Estados
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name.ToString().ToLower()] = state;
                // Conecta o sinal do estado ao método de transição da máquina
                state.Transitioned += OnStateTransitioned;
            }
        }

        // 2. Inicia o primeiro estado
        if (InitialState != null)
        {
            InitialState.Enter();
            _currentState = InitialState;
        }
    }

    public override void _Process(double delta)
    {
        _currentState?.ProcessUpdate(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState?.PhysicsUpdate(delta);
    }

    // Método que é chamado quando um Estado emite o sinal "Transitioned"
    private void OnStateTransitioned(State state, string newStateName)
    {
        // Se quem pediu a transição não for o estado atual, ignora (evita bugs)
        if (state != _currentState) return;

        string newStateKey = newStateName.ToLower();
        if (!_states.ContainsKey(newStateKey)) return;

        // Sai do estado antigo e entra no novo
        _currentState.Exit();
        _currentState = _states[newStateKey];
        _currentState.Enter();

        // Opcional: Descomente para ver as transições no console
        // GD.Print($"Entrou no estado: {newStateName}");
    }
}