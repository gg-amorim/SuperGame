using Godot;

// Não adicionamos [GlobalClass] porque não vamos anexar 'State' puro a um nó, 
// apenas os filhos dele (IdleState, MoveState, etc).
public partial class State : Node
{
    // Sinal para avisar a StateMachine que queremos mudar de estado
    [Signal] public delegate void TransitionedEventHandler(State state, string newStateName);

    // Métodos virtuais que os estados específicos vão sobrescrever (override)
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void ProcessUpdate(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }
}