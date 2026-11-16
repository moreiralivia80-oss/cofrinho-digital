using System.Globalization;

namespace CofrinhoDigital.Core;

/// <summary>
/// Regras do cofrinho: depósitos, saques com limite diário e taxa, e histórico de transações.
/// </summary>
public sealed class Cofrinho
{
    public const decimal LimiteDiarioSaque = 500m;
    public const decimal ValorSaqueSemTaxa = 200m; // saques acima deste valor pagam taxa
    public const decimal TaxaSaque = 2.50m;

    private static readonly CultureInfo PtBr = new("pt-BR");

    private readonly List<Transacao> _transacoes;
    private readonly ICofrinhoRepositorio? _repositorio;
    private readonly Func<DateTime> _agora;

    /// <param name="repositorio">Onde salvar as transações. Sem repositório, os dados ficam só na memória.</param>
    /// <param name="relogio">Fonte da data e hora atual. Os testes usam um relógio falso para simular a virada do dia.</param>
    public Cofrinho(ICofrinhoRepositorio? repositorio = null, Func<DateTime>? relogio = null)
    {
        _repositorio = repositorio;
        _agora = relogio ?? (() => DateTime.Now);
        _transacoes = repositorio?.Carregar().ToList() ?? new List<Transacao>();
    }

    public IReadOnlyList<Transacao> Transacoes => _transacoes;

    /// <summary>Calculado a partir do histórico: depósitos somam, saques e taxas subtraem.</summary>
    public decimal Saldo => _transacoes.Sum(t => t.Tipo == TipoTransacao.Deposito ? t.Valor : -t.Valor);

    /// <summary>Soma dos saques feitos na data de hoje (o limite volta a zero a cada dia).</summary>
    public decimal TotalSacadoHoje
    {
        get
        {
            var hoje = _agora().Date;
            return _transacoes
                .Where(t => t.Tipo == TipoTransacao.Saque && t.Data.Date == hoje)
                .Sum(t => t.Valor);
        }
    }

    public decimal LimiteRestanteHoje => LimiteDiarioSaque - TotalSacadoHoje;

    public ResultadoOperacao Depositar(decimal valor)
    {
        var erro = ValidarValor(valor, "depósito");
        if (erro != null)
            return ResultadoOperacao.Falha(erro);

        Registrar((TipoTransacao.Deposito, valor));
        return ResultadoOperacao.Ok($"Depósito de {Moeda(valor)} realizado com sucesso!");
    }

    public ResultadoOperacao Sacar(decimal valor)
    {
        var erro = ValidarValor(valor, "saque");
        if (erro != null)
            return ResultadoOperacao.Falha(erro);

        if (valor > Saldo)
            return ResultadoOperacao.Falha("Saldo insuficiente para realizar o saque.");

        if (valor > LimiteRestanteHoje)
            return ResultadoOperacao.Falha(
                $"Limite diário de saque excedido. Limite restante hoje: {Moeda(LimiteRestanteHoje)}.");

        var taxa = valor > ValorSaqueSemTaxa ? TaxaSaque : 0m;
        if (valor + taxa > Saldo)
            return ResultadoOperacao.Falha($"Saldo insuficiente para pagar a taxa de saque ({Moeda(taxa)}).");

        if (taxa > 0)
        {
            Registrar((TipoTransacao.Saque, valor), (TipoTransacao.Taxa, taxa));
            return ResultadoOperacao.Ok($"Saque de {Moeda(valor)} realizado com sucesso! (Taxa de {Moeda(taxa)} aplicada)");
        }

        Registrar((TipoTransacao.Saque, valor));
        return ResultadoOperacao.Ok($"Saque de {Moeda(valor)} realizado com sucesso!");
    }

    private static string? ValidarValor(decimal valor, string operacao)
    {
        if (valor <= 0)
            return $"O valor do {operacao} deve ser maior que zero.";

        if (decimal.Round(valor, 2) != valor)
            return "Informe o valor com no máximo duas casas decimais.";

        return null;
    }

    /// <summary>
    /// Adiciona as transações e salva. Se a gravação falhar, desfaz as transações
    /// para a memória continuar igual ao que está salvo no arquivo.
    /// </summary>
    private void Registrar(params (TipoTransacao Tipo, decimal Valor)[] movimentos)
    {
        var agora = _agora();
        var saldo = Saldo;
        var novas = new List<Transacao>();

        foreach (var (tipo, valor) in movimentos)
        {
            saldo += tipo == TipoTransacao.Deposito ? valor : -valor;
            novas.Add(new Transacao(agora, tipo, valor, saldo));
        }

        _transacoes.AddRange(novas);
        try
        {
            _repositorio?.Salvar(_transacoes);
        }
        catch
        {
            _transacoes.RemoveRange(_transacoes.Count - novas.Count, novas.Count);
            throw;
        }
    }

    private static string Moeda(decimal valor) => valor.ToString("C", PtBr);
}
