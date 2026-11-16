using CofrinhoDigital.Core;

namespace CofrinhoDigital.Tests;

public sealed class RepositorioJsonTests : IDisposable
{
    private readonly string _pasta = Path.Combine(Path.GetTempPath(), "cofrinho-testes-" + Guid.NewGuid().ToString("N"));

    private string Arquivo => Path.Combine(_pasta, "transacoes.json");

    [Fact]
    public void Arquivo_inexistente_devolve_lista_vazia()
    {
        var repositorio = new RepositorioJson(Arquivo);

        Assert.Empty(repositorio.Carregar());
    }

    [Fact]
    public void Transacoes_salvas_sao_carregadas_iguais()
    {
        var transacoes = new List<Transacao>
        {
            new(new DateTime(2026, 11, 16, 9, 30, 0), TipoTransacao.Deposito, 120.50m, 120.50m),
            new(new DateTime(2026, 11, 16, 10, 0, 0), TipoTransacao.Saque, 20m, 100.50m)
        };

        new RepositorioJson(Arquivo).Salvar(transacoes);
        var carregadas = new RepositorioJson(Arquivo).Carregar();

        Assert.Equal(transacoes, carregadas);
    }

    [Fact]
    public void Cofrinho_continua_com_o_saldo_depois_de_reabrir()
    {
        new Cofrinho(new RepositorioJson(Arquivo)).Depositar(80m);

        var reaberto = new Cofrinho(new RepositorioJson(Arquivo));

        Assert.Equal(80m, reaberto.Saldo);
    }

    public void Dispose()
    {
        if (Directory.Exists(_pasta))
            Directory.Delete(_pasta, recursive: true);
    }
}
