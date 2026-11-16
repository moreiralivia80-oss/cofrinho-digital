using CofrinhoDigital.Core;

namespace CofrinhoDigital.Tests;

public class CofrinhoTests
{
    private readonly RelogioFalso _relogio = new();
    private readonly RepositorioEmMemoria _repositorio = new();

    private Cofrinho CriarCofrinho() => new(_repositorio, () => _relogio.Agora);

    [Fact]
    public void Deposito_valido_aumenta_o_saldo()
    {
        var cofrinho = CriarCofrinho();

        var resultado = cofrinho.Depositar(150.75m);

        Assert.True(resultado.Sucesso);
        Assert.Equal(150.75m, cofrinho.Saldo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Deposito_zero_ou_negativo_e_recusado(int valor)
    {
        var cofrinho = CriarCofrinho();

        var resultado = cofrinho.Depositar(valor);

        Assert.False(resultado.Sucesso);
        Assert.Equal(0m, cofrinho.Saldo);
    }

    [Fact]
    public void Valor_com_mais_de_duas_casas_decimais_e_recusado()
    {
        var cofrinho = CriarCofrinho();

        Assert.False(cofrinho.Depositar(10.555m).Sucesso);
    }

    [Fact]
    public void Saque_valido_diminui_o_saldo()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(100m);

        var resultado = cofrinho.Sacar(40m);

        Assert.True(resultado.Sucesso);
        Assert.Equal(60m, cofrinho.Saldo);
    }

    [Fact]
    public void Saque_maior_que_o_saldo_e_recusado()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(50m);

        var resultado = cofrinho.Sacar(80m);

        Assert.False(resultado.Sucesso);
        Assert.Equal(50m, cofrinho.Saldo);
    }

    [Fact]
    public void Saque_acima_de_200_cobra_taxa()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(500m);

        var resultado = cofrinho.Sacar(250m);

        Assert.True(resultado.Sucesso);
        Assert.Equal(500m - 250m - Cofrinho.TaxaSaque, cofrinho.Saldo);
        Assert.Equal(TipoTransacao.Taxa, cofrinho.Transacoes[^1].Tipo);
    }

    [Fact]
    public void Saque_de_exatamente_200_nao_cobra_taxa()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(500m);

        cofrinho.Sacar(200m);

        Assert.Equal(300m, cofrinho.Saldo);
        Assert.DoesNotContain(cofrinho.Transacoes, t => t.Tipo == TipoTransacao.Taxa);
    }

    [Fact]
    public void Saque_sem_saldo_para_pagar_a_taxa_e_recusado()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(250m);

        var resultado = cofrinho.Sacar(250m);

        Assert.False(resultado.Sucesso);
        Assert.Equal(250m, cofrinho.Saldo);
    }

    [Fact]
    public void Limite_diario_bloqueia_saques_acima_de_500_no_mesmo_dia()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(1000m);
        cofrinho.Sacar(300m);

        var resultado = cofrinho.Sacar(250m);

        Assert.False(resultado.Sucesso);
        Assert.Equal(200m, cofrinho.LimiteRestanteHoje);
    }

    [Fact]
    public void Limite_diario_volta_a_zero_no_dia_seguinte()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(1000m);
        cofrinho.Sacar(450m);
        Assert.False(cofrinho.Sacar(100m).Sucesso);

        _relogio.Agora = _relogio.Agora.AddDays(1);

        Assert.True(cofrinho.Sacar(100m).Sucesso);
        Assert.Equal(400m, cofrinho.LimiteRestanteHoje);
    }

    [Fact]
    public void Historico_registra_tipo_valor_data_e_saldo_apos_cada_operacao()
    {
        var cofrinho = CriarCofrinho();
        cofrinho.Depositar(300m);
        cofrinho.Sacar(250m);

        Assert.Collection(cofrinho.Transacoes,
            t => Assert.Equal(new Transacao(_relogio.Agora, TipoTransacao.Deposito, 300m, 300m), t),
            t => Assert.Equal(new Transacao(_relogio.Agora, TipoTransacao.Saque, 250m, 50m), t),
            t => Assert.Equal(new Transacao(_relogio.Agora, TipoTransacao.Taxa, 2.50m, 47.50m), t));
    }

    [Fact]
    public void Operacao_bem_sucedida_e_salva_e_operacao_recusada_nao()
    {
        var cofrinho = CriarCofrinho();

        cofrinho.Depositar(100m);
        cofrinho.Sacar(500m);

        Assert.Equal(1, _repositorio.VezesSalvo);
        Assert.Single(_repositorio.Salvas);
    }

    [Fact]
    public void Saldo_e_carregado_a_partir_das_transacoes_salvas()
    {
        _repositorio.Salvar(new List<Transacao>
        {
            new(_relogio.Agora.AddDays(-2), TipoTransacao.Deposito, 200m, 200m),
            new(_relogio.Agora.AddDays(-1), TipoTransacao.Saque, 50m, 150m)
        });

        var cofrinho = CriarCofrinho();

        Assert.Equal(150m, cofrinho.Saldo);
        Assert.Equal(Cofrinho.LimiteDiarioSaque, cofrinho.LimiteRestanteHoje);
    }

    [Fact]
    public void Se_a_gravacao_falhar_a_operacao_e_desfeita()
    {
        var cofrinho = new Cofrinho(new RepositorioComFalha(), () => _relogio.Agora);

        Assert.Throws<IOException>(() => cofrinho.Depositar(100m));
        Assert.Equal(0m, cofrinho.Saldo);
        Assert.Empty(cofrinho.Transacoes);
    }
}
