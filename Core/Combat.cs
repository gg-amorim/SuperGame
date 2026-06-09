using Godot;
using MMO.Core;
using System;
using System.Collections.Generic;


namespace MMO.Scripts.Players;

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

    public InputPackage TranslateCombatActions(InputPackage newInput)
    {
        // is_empty() equivale a verificar se o Count é 0
        if (newInput.CombatActions.Count > 0)
        {
            // Ordena a lista usando nosso método customizado
            newInput.CombatActions.Sort(CombatActionPrioritySort);

            string bestInputAction = newInput.CombatActions[0];

            // Supondo que BasicAttacks seja um Dictionary no C#
            string translatedIntoMoveName = _model.ActiveWeapon.BasicAttacks[bestInputAction];

            // append() equivale ao Add() em C# (seja List<T> ou Godot.Collections.Array<T>)
            newInput.Actions.Add(translatedIntoMoveName);
        }

        return newInput;
    }

    // O sort_custom no C# exige que retornemos um int (negativo, zero ou positivo)
    public static int CombatActionPrioritySort(string a, string b)
    {
        // Pega os valores do dicionário de prioridade
        // (Usando 0 como padrão (fallback) caso a chave não exista, para evitar erros)
        int priorityA = inputsPriority.ContainsKey(a) ? inputsPriority[a] : 0;
        int priorityB = inputsPriority.ContainsKey(b) ? inputsPriority[b] : 0;

        // Retornar prioridadeB.CompareTo(prioridadeA) coloca os MAIORES valores primeiro (ordem decrescente)
        // Isso simula exatamente o "if inputs_priority[a] > inputs_priority[b]: return true"
        return priorityB.CompareTo(priorityA);
    }
}
