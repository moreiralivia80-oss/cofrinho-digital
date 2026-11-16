namespace CofrinhoDigital.Core;

/// <summary>Onde as transações ficam guardadas entre uma execução e outra.</summary>
public interface ICofrinhoRepositorio
{
    IReadOnlyList<Transacao> Carregar();

    void Salvar(IReadOnlyList<Transacao> transacoes);
}
