using Godot;
using MMO.Core;
using MMO.Scripts.Players.States;
using MMO.Scripts.Weapons.Models;
using System.Collections.Generic;

namespace MMO.Scripts.Players;

public partial class PlayerModel : Node3D
{
    [Export] public bool IsEnemy { get; set; } = false;

    private Player _player;
    private Skeleton3D _skeleton;
    private SkeletonAnimator _animator;
    private Combat _combat;
    public ResourcesComponent Resources;
    private MovesData _movesData;
    private Weapon _activeWeapon;

    public State CurrentState;
    public Dictionary<string, State> States;

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


        Resources = _player.GetNode<ResourcesComponent>("Components/ResourcesComponent");
        _movesData = GetNode<MovesData>("MovesData");
        States = new Dictionary<string, State>()
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
            { "slash_3",  GetNode<State>("States/Slash3") },
            { "staggered",  GetNode<State>("States/Staggered") },
            { "parry",  GetNode<State>("States/Parry") },
            { "riposte",  GetNode<State>("States/Riposte") },
            { "parried",  GetNode<State>("States/Parried") },
            { "death",  GetNode<State>("States/Death") },
        };

        CurrentState = States["idle"];

        foreach (var state in States.Values)
        {
            state.Player = _player;
            state.Resources = Resources;
            state.MovesData = _movesData;
            state.AssignCombos();
        }


        // Dá o tranco inicial
        CurrentState.MarkEnterState();
        CurrentState.OnEnterState();
        _animator.CallDeferred(AnimationPlayer.MethodName.Play, CurrentState.AnimationStr);

    }

    public void UpdateModel(InputPackage input, float delta)
    {
        // Atualizado para o novo nome do GDScript. Se a sua classe Combat em C# 
        // ainda usar "TranslateCombatActions", basta renomear lá ou aqui.
        input = _combat.Contextualize(input);

        string relevance = CurrentState.CheckRelevance(input);
        if (relevance != "okay")
        {
            SwitchTo(relevance);
        }

        // Chamadas atualizadas exigidas pelo GDScript
        CurrentState.UpdateResources(delta);
        CurrentState.Update(input, delta);
    }

    public void SwitchTo(string state)
    {
        if (!States.ContainsKey(state)) return;

        CurrentState.OnExitState();
        CurrentState = States[state];
        CurrentState.OnEnterState();
        CurrentState.MarkEnterState();

        // Desconta os custos da ação no momento exato em que o estado é trocado
        Resources.PayResourceCost(CurrentState);

        _animator.Play(CurrentState.AnimationStr);
    }
}
