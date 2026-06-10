using Godot;
using MMO.Core;

namespace MMO.Scripts.Players.States.Combos.Sword;

[GlobalClass] 
public partial class Combo : Node
{
    // A referência ao estado pai (no GDScript era var move : Move)
    // Usamos o tipo State pois foi assim que você renomeou a classe base de movimentos
    public State State { get; set; }

    // O [Export] garante que você possa definir qual golpe será trigado pelo Inspetor do Godot
    [Export]
    public string TriggeredMove { get; set; }

    public virtual bool IsTriggered(InputPackage input)
    {
        return false;
    }
}
// Como os combos básicos têm apenas uma função simples, que é sempre chamada na 
// primeira linha do CheckRelevance de algum State, pode parecer que estamos 
// abstraindo as coisas um pouco demais. Por que não colocar simplesmente o código 
// que decide se o "slash_1" avança para o "slash_2" direto no "slash_1" e chamar 
// isso de "funcionalidade local do slash_1"?
//
// Bem, para um exemplo básico como encadear golpes consecutivos em uma série, 
// isso até pode ser verdade. Mas o propósito dos Combos é dividir ainda mais a 
// lógica de transição do State para melhorar a escalabilidade.
//
// Muitos fatores diferentes podem regular a transição de um State. Imagine o 
// nível de adrenalina, nível de fadiga, status de mana/stamina, alguns itens 
// únicos no inventário, uma chance aleatória de ativar um finisher que decepa 
// membros, o tipo de inimigo, buffs diferentes... tudo isso pode influenciar o 
// fluxo dos nossos estados...
//
// Imagina ter que modificar isso tudo adicionando mais um "else if" no 
// CheckRelevance do State toda vez que quiser adicionar uma mecânica nova.
//
// Com essa arquitetura de combos, o "você-do-futuro" pode trabalhar no projeto 
// por um ano e, de repente, decidir que precisa de uns finishers aleatórios de 
// arrancar cabeças. Teria algo mais fácil do que simplesmente criar um script 
// Combo com umas 7 linhas de lógica e jogar ele (como nó filho) nos seus States?
//
// E nós ainda consultamos o funcionamento dos nossos combos iterando sobre os 
// filhos com GetChildren() a partir de um State, o que cria um sistema fantasma 
// de prioridade de combos alimentado puramente pela ordem dos nós Combo na árvore 
// do editor. Genial!
//
// O código do State é focado em lógicas de ação super básicas. Pense nele como: 
// "como seria o seu jogo com inputs lineares, onde toda ação tem uma tecla de 
// atalho própria e nenhum input secundário?" — a implementação mais simples e 
// plana possível da sua máquina de estados, sem condições complexas.
//
// O código dos Combos é um módulo para criar camadas adicionais de transições 
// condicionais que podem ser adicionadas, misturadas, copiadas e deletadas sem 
// que a sua função base CheckRelevance precise mudar uma única vírgula.