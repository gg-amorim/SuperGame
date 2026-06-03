using Godot;
using System.Collections.Generic;

namespace MMO.Core;

public partial class PlayerModel : Node3D
{
    private CharacterBody3D _player;
    private Skeleton3D _skeleton;
    private AnimationPlayer _animator;
    private State _currentState;
    private Dictionary<string, State> _states;

    public Skeleton3D Skeleton => _skeleton;
    public AnimationPlayer Animator => _animator;

    public override void _Ready()
    {
        GD.Print("[DEBUG] A iniciar o PlayerModel _Ready()...");

        _player = GetParent<CharacterBody3D>();
        _skeleton = GetNode<Skeleton3D>("GeneralSkeleton");
        _animator = GetNode<AnimationPlayer>("AnimationPlayer");

        // DEBUG 1: Verifica se o Animator foi encontrado na árvore de nós
        GD.Print($"[DEBUG] Animator encontrado? {_animator != null}");

        _states = new Dictionary<string, State>()
        {
            { "idle", GetNode<State>("States/Idle") },
            { "run",  GetNode<State>("States/Run") },
            { "sprint",  GetNode<State>("States/Sprint") },
            { "jump_run",  GetNode<State>("States/JumpRun") },
            { "midair",  GetNode<State>("States/Midair") },
            { "landing_run",  GetNode<State>("States/LandingRun") },
            { "jump_sprint",  GetNode<State>("States/JumpSprint") },
            { "landing_sprint",  GetNode<State>("States/LandingSprint") }
        };

        _currentState = _states["idle"];

        foreach (var state in _states.Values)
        {
            state.Player = _player;
        }

        // DEBUG 2: Verifica que nome de animação o estado Idle está a pedir
        GD.Print($"[DEBUG] Estado atual definido para: '{_currentState.StateName}'");
        GD.Print($"[DEBUG] Animação a ser pedida: '{_currentState.Animation}'");

        // DEBUG 3: Verifica se a animação realmente existe dentro do AnimationPlayer
        if (_animator != null)
        {
            bool temAnimacao = _animator.HasAnimation(_currentState.Animation);
            GD.Print($"[DEBUG] A animação '{_currentState.Animation}' existe no Animator? {temAnimacao}");

            if (!temAnimacao)
            {
                GD.PrintErr($"[ERRO GRAVE] A animação '{_currentState.Animation}' não existe! Verifique o nome no editor.");
            }
        }

        // Dá o tranco inicial
        _currentState.MarkEnterState();
        _currentState.OnEnterState();
        _animator.CallDeferred(AnimationPlayer.MethodName.Play, _currentState.Animation);

        GD.Print("[DEBUG] Fim do _Ready(). O comando Play() foi chamado com sucesso.");
    }

    public void UpdateModel(InputPackage input, float delta)
    {
        string relevance = _currentState.CheckRelevance(input);
        if (relevance != "okay")
        {
            SwitchTo(relevance);
        }
        _currentState.Update(input, delta);
    }

    public void SwitchTo(string state)
    {

        // ADICIONE ESTA LINHA:
        GD.Print($"[State Machine] Mudando de '{_currentState.StateName}' para '{state}'");

        if (!_states.ContainsKey(state))
        {
            GD.PrintErr($"[ERRO] Tentou mudar para o estado '{state}', mas ele não existe no dicionário!");
            return;
        }

        _currentState.OnExitState();
        _currentState = _states[state];
        _currentState.OnEnterState();
        _currentState.MarkEnterState();
        _animator.Play(_currentState.Animation);
    }
}
