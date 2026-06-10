using Godot;
using MMO.Core;
using MMO.Scripts.Players;
using System;
using System.Collections.Generic;

public partial class ResourcesComponent : Node
{
    [Export]
    public bool GodMode { get; set; } = false;

    [ExportGroup("Health")]
    [Export]
    public float Health { get; set; } = 100f;
    [Export]
    public float MaxHealth { get; set; } = 100f;

    [ExportGroup("Stamina")]
    [Export]
    public float Stamina { get; set; } = 100f;
    [Export]
    public float MaxStamina { get; set; } = 100f;
    [Export]
    public float StaminaRegenRate { get; set; } = 10f;

    public List<string> Statuses { get; set; } = [];

    private const int FATIQUE_TRESHOLD = 20;

    private PlayerModel _model;
    public override void _Ready()
    {
        _model = GetParent<Node>().GetParent<Player>().Model;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void Update(float delta) => GainStamina(StaminaRegenRate * delta);
    public void PayResourceCost(State state) => LoseStamina(state.StaminaCost);
    public bool CanBePaid(State state) => Stamina > 0 || state.StaminaCost == 0;



    public void GainHealth(float amount) =>    
        Health = Health + amount <= MaxHealth ? Health + amount : MaxHealth;

        
    

    public void LoseHealth(float amount)
    {
        if (GodMode) return;

        Health = Health - amount > 0 ? Health - amount : 0;

        if (Health <= 0) _model.CurrentState.TryForceMove("death"); // Placeholder for actual death state
    }

    public void GainStamina(float amount)
    {
        Stamina = Stamina + amount <= MaxStamina ? Stamina + amount : MaxStamina;

        if(Stamina > FATIQUE_TRESHOLD) Statuses.Remove("fatique");
    }

    public void LoseStamina(float amount)
    {
        if(GodMode) return;

        Stamina = Stamina - amount > 0 ? Stamina - amount : 0;
        if (Stamina < 1) Statuses.Add("fatique");
    }

}
