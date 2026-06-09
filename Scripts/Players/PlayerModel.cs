using Godot;
using MMO.Core;
using MMO.Scripts.Weapons.Models;
using System.Collections.Generic;

namespace MMO.Scripts.Players;

public partial class PlayerModel : Node3D
{
    private Player _player;
    private Skeleton3D _skeleton;
    private SkeletonAnimator _animator;
    private State _currentState;
    private Weapon _activeWeapon;
    private Combat _combat;
    private Dictionary<string, State> _states;

    public Skeleton3D Skeleton => _skeleton;
    public SkeletonAnimator Animator => _animator;


    public Weapon ActiveWeapon => _activeWeapon;
    public override void _Ready()
    {
        _player = GetParent<Player>();
        _skeleton = GetNode<Skeleton3D>("GeneralSkeleton");
        _animator = GetNode<SkeletonAnimator>("SkeletonAnimator");
        _combat = GetNode<Combat>("Combat");
        _activeWeapon = GetNode<Weapon>("RightWrist/WeaponSocket/Sword");
      
        _states = new Dictionary<string, State>()
        {
            { "idle", GetNode<State>("States/Idle") },
            { "run",  GetNode<State>("States/Run") },
            { "sprint",  GetNode<State>("States/Sprint") },
            { "jump_run",  GetNode<State>("States/JumpRun") },
            { "midair",  GetNode<State>("States/Midair") },
            { "landing_run",  GetNode<State>("States/LandingRun") },
            { "jump_sprint",  GetNode<State>("States/JumpSprint") },
            { "landing_sprint",  GetNode<State>("States/LandingSprint") },
            { "slash_1",  GetNode<State>("States/Slash1") },
            { "slash_2",  GetNode<State>("States/Slash2") },
            { "slash_3",  GetNode<State>("States/Slash3") }
        };

        _currentState = _states["idle"];

        foreach (var state in _states.Values)
        {
            state.Player = _player;
        }


        // Dá o tranco inicial
        _currentState.MarkEnterState();
        _currentState.OnEnterState();
        _animator.CallDeferred(AnimationPlayer.MethodName.Play, _currentState.Animation);

    }

    public void UpdateModel(InputPackage input, float delta)
    {
        input = _combat.TranslateCombatActions(input);
        string relevance = _currentState.CheckRelevance(input);
        if (relevance != "okay")
        {
            SwitchTo(relevance);
        }
        _currentState.Update(input, delta);
    }

    public void SwitchTo(string state)
    {
        if (!_states.ContainsKey(state)) return;
        _currentState.OnExitState();
        _currentState = _states[state];
        _currentState.OnEnterState();
        _currentState.MarkEnterState();
        _animator.Play(_currentState.Animation);
    }
}
