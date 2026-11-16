using CofrinhoDigital.Core;

namespace CofrinhoDigital.Tests;

/// <summary>Relógio controlado pelo teste, para simular a passagem dos dias.</summary>
internal sealed class RelogioFalso
{
    public DateTime Agora { get; set; } = new DateTime(2026, 11, 16, 10, 0, 0);
}

/// <summary>Repositório em memória: guarda o que foi salvo e quantas vezes.</summary>
internal sealed class RepositorioEmMemoria : ICofrinhoRepositorio
{
    public List<Transacao> Salvas { get; private set; } = new();

    public int VezesSalvo { get; private set; }

    public IReadOnlyList<Transacao> Carregar() => Salvas;

    public void Salvar(IReadOnlyList<Transacao> transacoes)
    {
        Salvas = transacoes.ToList();
        VezesSalvo++;
    }
}

/// <summary>Repositório que sempre falha ao salvar, como um disco cheio.</summary>
internal sealed class RepositorioComFalha : ICofrinhoRepositorio
{
    public IReadOnlyList<Transacao> Carregar() => new List<Transacao>();

    public void Salvar(IReadOnlyList<Transacao> transacoes) => throw new IOException("Disco cheio");
}
