# GDD Técnico & Diário de Bordo: MMORPG 3D Action (Side Project)

Este documento centraliza a visão conceitual, as diretrizes de arquitetura e as decisões de engenharia de software tomadas para o desenvolvimento do nosso MMORPG 3D Sandbox. O objetivo principal deste projeto é criar um jogo com progressão vertical focada em grind (estilo RuneScape e Albion Online) e combate action cadenciado (inspirado em World of Warcraft e Guild Wars 2), utilizando a Godot Engine 4 com C#.

---

## 1. Pilares do Escopo e Referências

O design do jogo foi estruturado com base em experiências consagradas do gênero MMORPG:

* Tibia: Ciclo de grind implacável, valorização do tempo investido e mundo aberto perigoso.
* RuneScape & Albion Online: Sistemas de progressão horizontal e vertical baseados em uso. Não existem classes fixas na criação do personagem; a "classe" do jogador é definida dinamicamente pelas armas e equipamentos que ele escolhe usar, dependendo do seu nível de proficiência treinado.
* World of Warcraft, Guild Wars 2 & Albion: Combate dinâmico, ritmado e responsivo. A câmera adota uma perspectiva isométrica travada (estilo MOBA/Albion), garantindo clareza tática durante o combate action, enquanto o movimento é controlado fisicamente pelo teclado (WASD).

---

## 2. Decisões Arquiteturais e Engenharia de Software

Para mitigar a complexidade intrínseca de um MMORPG (mesmo na fase de prototipagem single-player), estabelecemos padrões rigorosos de desenvolvimento que evitam o acoplamento excessivo de código (God Classes).

### Composição sobre Herança (Component-Based Design)
A herança clássica de orientação a objetos cria nós rígidos e difíceis de modificar a longo prazo. Em vez disso, adotamos a Composição Baseada em Nós:
* O nó `Player` funciona apenas como um orquestrador central e contêiner físico (CharacterBody3D) com zero linhas de código de lógica.
* Comportamentos específicos são isolados em nós filhos especialistas, como `InputComponent` (lê teclado) e `MovementComponent` (aplica física e gravidade).

### Padrão de Projeto: Máquina de Estados Baseada em Nós (Node-Based FSM)
Em vez de utilizar condicionais (`if/else`) ou `enums` rígidos que inflam os scripts, o comportamento de entidades é governado por uma Máquina de Estados onde cada estado é um Nó na árvore da Godot.
* A raiz `Core/StateMachine.cs` gerencia as transições.
* Estados concretos como `PlayerIdleState` e `PlayerRunState` assinam um contrato abstrato (`Core/State.cs`) e contêm apenas a lógica pertinente àquele exato momento.

### Desenvolvimento Baseado em Dados (Data-Driven Architecture)
Parâmetros de balanceamento, dados de experiência e definições de itens não ficam hardcoded.
* Utilizaremos `Resource` nativos para encapsular dados puramente estáticos. Isso isola a lógica de cálculo (scripts) das tabelas de dados.

### Convenção de Organização de Diretórios
Os arquivos seguem o padrão PascalCase para pastas e scripts C#, sendo rigorosamente agrupados por domínio:

res://
├── Core/                  # Sistemas globais e contratos abstratos (State.cs, StateMachine.cs)
├── Data/                  # Recursos customizados (.tres) para tabelas de XP, itens e classes
├── Scenes/
│   ├── Player/            # Cenas (.tscn) e malhas exclusivas do jogador
│   └── Shared/            # Componentes reutilizáveis (ex: zonas de colisão, chão)
└── Scripts/
	├── Components/        # Nós puramente comportamentais (InputComponent, MovementComponent)
	├── Player/
	│   └── States/        # Estados concretos da FSM (PlayerIdleState, PlayerRunState)
	└── UI/                # Controladores de interfaces gráficas e menus

---

## 3. Aprendizados Conceituais e Técnicos na Godot 4 com C#

Durante a implementação do núcleo de locomoção, absorvemos conceitos vitais da matemática de jogos e das peculiaridades do C# dentro da engine:

### I. Espaço Local vs. Espaço Global e Perspectiva Isométrica
Os inputs brutos capturados do teclado (vetor bidimensional X e Y) não são injetados diretamente na física. Eles são transformados em um vetor de direção global preciso (X e Z) rotacionado em relação ao eixo Y (Rotation.Y) do pivô da câmera. Isso garante que apertar "W" mova o personagem na diagonal visual da tela isométrica, mantendo a sensação tátil correta para o jogador.

### II. Suavização Vetorial (Interpolação de Velocidade)
Para afastar o jogo de uma movimentação robótica, a velocidade física obedece a um cálculo de aproximação (Lerp) ponderado pelo tempo de quadro (delta) entre o "Vetor Alvo" (Input) e o "Vetor Atual" (Velocity do corpo).

### III. Peculiaridades de C# e Godot 4
* Injeção de Dependência pelo Inspetor: A comunicação entre estados e componentes é feita usando o atributo `[Export]`. O esquecimento de linkar graficamente essas dependências no Inspetor gera `System.NullReferenceException`.
* Sinais (Signals) e Herança: Devido aos Source Generators da Godot 4, acessar sinais declarados em uma classe pai exige explicitar a origem do sinal para evitar o erro CS0117 (ex: `EmitSignal(State.SignalName.Transitioned)` ao invés de apenas `SignalName.Transitioned`).
* Sobrescrita (Override): Métodos do contrato base `State.cs` exigem a palavra-chave `virtual` para permitir que estados filhos utilizem `override` em métodos como `PhysicsUpdate`.

---

## 4. Próximos Passos do Desenvolvimento

Com a fundação de movimento, a câmera isométrica, os componentes e a Máquina de Estados operando em perfeita sintonia e livres de bugs, a esteira técnica avançará para:

1. Suavização da Câmera: Implementar um script para que o pivô da câmera isométrica siga o `Player` pelo cenário com interpolação suave, em vez de ser um nó filho engessado que copia travamentos do modelo.
2. Fundação de Dados (Data-Driven): Criar os recursos nativos (`WeaponClassData`) na pasta `Data/` para dar início ao sistema dinâmico de proficiência de armas.
3. Integração de Animações: Vincular a FSM ao `AnimationPlayer`/`AnimationTree` para reproduzir as animações correspondentes aos estados de transição (Idle e Run).
