# Cofrinho Digital

Aplicativo desktop em C# (Windows Forms) para controlar um cofrinho: depósitos, saques com limite diário e taxa, histórico de transações e saldo salvo entre uma execução e outra. As regras de negócio ficam numa biblioteca separada da interface e são cobertas por testes automatizados.

## Funcionalidades

- **Depósitos e saques** com validação do valor (maior que zero e no máximo duas casas decimais)
- **Limite diário de saque** de R$ 500,00, que volta a zero a cada dia
- **Taxa de R$ 2,50** em saques acima de R$ 200,00
- **Histórico de transações** com data, tipo, valor e saldo após cada operação
- **Persistência em arquivo JSON**: o saldo e o histórico continuam ao fechar e abrir o aplicativo
- Entrada de valores com vírgula ou ponto (`12,90` ou `12.90`), recusando valores ambíguos como `1.500`

## Regras de negócio

| Regra | Valor |
|---|---|
| Limite de saque por dia | R$ 500,00 |
| Saques que pagam taxa | Acima de R$ 200,00 |
| Taxa por saque | R$ 2,50 |
| Saque recusado quando | O valor, ou o valor mais a taxa, for maior que o saldo, ou ultrapassar o limite do dia |

O saldo não é guardado separadamente: ele é **calculado a partir do histórico** (depósitos somam, saques e taxas subtraem). Assim, saldo e histórico nunca ficam inconsistentes.

## Arquitetura

```
src/
├── CofrinhoDigital.Core/     Regras de negócio, sem dependência de interface
│   ├── Cofrinho.cs           Depósito, saque, limite diário, taxa e saldo
│   ├── Transacao.cs          Registro de cada movimentação
│   ├── ResultadoOperacao.cs  Sucesso ou falha, com a mensagem para o usuário
│   ├── ICofrinhoRepositorio.cs
│   ├── RepositorioJson.cs    Persistência em arquivo JSON
│   └── ConversorValor.cs     Leitura do valor digitado
└── CofrinhoDigital.UI/       Interface Windows Forms
tests/
└── CofrinhoDigital.Tests/    Testes automatizados (xUnit)
```

Algumas decisões do projeto:

- **A interface não contém regras.** O formulário só lê o valor, chama o `Cofrinho` e mostra o resultado. As mesmas regras poderiam ser usadas por uma API ou um app web sem mudanças.
- **O resultado de cada operação é um objeto** (`ResultadoOperacao`), não um texto. A interface sabe se deu certo pelo campo `Sucesso`, sem procurar palavras na mensagem.
- **A persistência fica atrás de uma interface** (`ICofrinhoRepositorio`). O aplicativo usa JSON e os testes usam um repositório em memória.
- **A data atual é injetada** (`Func<DateTime>`). Assim os testes simulam a virada do dia e confirmam que o limite diário é zerado.
- **A gravação é segura:** o arquivo é escrito primeiro em um temporário e depois substituído, e, se a gravação falhar, a operação é desfeita na memória.

## Tecnologias

- **C# / .NET 10**
- **Windows Forms**
- **System.Text.Json** (persistência)
- **xUnit** (testes)

## Como executar

Pré-requisitos: Windows e [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/moreiralivia80-oss/cofrinho-digital.git
cd cofrinho-digital
dotnet run --project src/CofrinhoDigital.UI
```

Também é possível abrir o arquivo `CofrinhoDigital.slnx` no Visual Studio e executar o projeto `CofrinhoDigital.UI`.

Os dados ficam em `%LOCALAPPDATA%\CofrinhoDigital\transacoes.json`. Para começar do zero, basta apagar esse arquivo.

## Testes

```bash
dotnet test
```

Os testes cobrem depósitos e saques válidos e inválidos, a cobrança da taxa, o limite diário (incluindo a virada do dia), o histórico, a persistência em JSON, a reversão quando a gravação falha e a conversão dos valores digitados.

## Autora

**Lívia Moreira Teixeira**, estudante de Análise e Desenvolvimento de Sistemas.
[LinkedIn](https://www.linkedin.com/in/liviamoreira-software/) · [GitHub](https://github.com/moreiralivia80-oss)
