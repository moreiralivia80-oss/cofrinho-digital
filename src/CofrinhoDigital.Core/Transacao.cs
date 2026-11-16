namespace CofrinhoDigital.Core;

public enum TipoTransacao
{
    Deposito,
    Saque,
    Taxa
}

/// <summary>Uma movimentação do cofrinho. O saldo é sempre calculado a partir delas.</summary>
public sealed record Transacao(DateTime Data, TipoTransacao Tipo, decimal Valor, decimal SaldoApos);
