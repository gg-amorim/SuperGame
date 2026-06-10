using Godot;
using MMO.Core;
using System.Collections.Generic;

namespace MMO.Scripts.Players;

[GlobalClass] // Adicionado para manter a paridade com class_name HumanoidCombat
public partial class Combat : Node
{
    private PlayerModel _model;

    private static Dictionary<string, int> inputsPriority = new Dictionary<string, int>()
    {
        { "light_attack_pressed", 1 },
        { "heavy_attack_pressed", 2 }
    };

    public override void _Ready()
    {
        // Pega o nó pai ("..") e faz o cast (as PlayerModel)
        _model = GetNode<PlayerModel>("..");
    }

    public InputPackage Contextualize(InputPackage newInput)
    {
        TranslateInputs(newInput);
        FilterWithResources(newInput);
        return newInput;
    }

    private void TranslateInputs(InputPackage input)
    {
        // is_empty() equivale a verificar se o Count é > 0
        if (input.CombatActions.Count > 0)
        {
            // Ordena a lista usando nosso método customizado
            input.CombatActions.Sort(CombatActionPrioritySort);

            string bestInputAction = input.CombatActions[0];

            // Supondo que BasicAttacks seja um Dictionary no C#
            string translatedIntoMoveName = _model.ActiveWeapon.BasicAttacks[bestInputAction];

            // append() equivale ao Add() em C#
            input.Actions.Add(translatedIntoMoveName);
        }
    }

    private void FilterWithResources(InputPackage input)
    {
        if (_model.Resources != null && _model.Resources.Statuses.Contains("fatique"))
        {
            input.Actions.Remove("sprint");
        }
    }

    // O sort_custom no C# exige que retornemos um int (negativo, zero ou positivo)
    public static int CombatActionPrioritySort(string a, string b)
    {
        // Pega os valores do dicionário de prioridade (Usando 0 como fallback)
        int priorityA = inputsPriority.GetValueOrDefault(a, 0);
        int priorityB = inputsPriority.GetValueOrDefault(b, 0);

        // Retornar prioridadeB.CompareTo(prioridadeA) coloca os MAIORES valores primeiro (ordem decrescente)
        // Isso simula exatamente o "if inputs_priority[a] > inputs_priority[b]: return true"
        return priorityB.CompareTo(priorityA);
    }
}