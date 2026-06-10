using Godot;
using System;

namespace MMO.Scripts.Players;

public partial class PlayerVisuals : Node3D
{
	private MeshInstance3D _betaJoints;
	private MeshInstance3D _betaSurface;
	private Node3D _swordVisual;
	private PlayerModel _playerModel;
	private Label _staminaLabel;
	private Label _healthLabel;
    private int _lastStamina = -1;
    private int _lastHealth = -1;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        _betaJoints = GetNode<MeshInstance3D>("Beta_Joints");
        _betaSurface = GetNode<MeshInstance3D>("Beta_Surface");
        _swordVisual = GetNode<Node3D>("SwordVisuals");
        _staminaLabel = GetNode<Label>("Stamina_Bar");
        _healthLabel = GetNode<Label>("Health_Bar");
    }

    public override void _Process(double delta)
    {
        UpdateResourcesInterface();
        AdjustWeaponVisuals();

    }

    private void AdjustWeaponVisuals()
    {
        _swordVisual.GlobalTransform = _playerModel.ActiveWeapon.GlobalTransform;
    }

    private void UpdateResourcesInterface()
    {
        if(_playerModel != null && !_playerModel.IsEnemy)
        {
            // Arredondamos para inteiro para a interface não piscar a cada 0.001 de mudança
            int currentStamina = Mathf.RoundToInt(_playerModel.Resources.Stamina);
            int currentHealth = Mathf.RoundToInt(_playerModel.Resources.Health);

            // Só força a UI a recalcular e desenhar de novo se o número visual mudar
            if (currentStamina != _lastStamina)
            {
                _staminaLabel.Text = $"Stamina {currentStamina}";
                _lastStamina = currentStamina;
            }

            if (currentHealth != _lastHealth)
            {
                _healthLabel.Text = $"Health {currentHealth}";
                _lastHealth = currentHealth;
            }
        }
    }

    public void AcceptModel(PlayerModel playerModel)
    {
        _playerModel = playerModel;
        _betaSurface.Skeleton = _playerModel.Skeleton.GetPath();
        _betaJoints.Skeleton = _playerModel.Skeleton.GetPath();

    }
}
