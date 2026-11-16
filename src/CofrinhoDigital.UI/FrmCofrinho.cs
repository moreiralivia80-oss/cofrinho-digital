using System.Globalization;
using CofrinhoDigital.Core;

namespace CofrinhoDigital.UI
{
    public partial class FrmCofrinho : Form
    {
        private static readonly CultureInfo PtBr = new("pt-BR");

        private readonly Cofrinho _cofrinho;

        public FrmCofrinho(Cofrinho cofrinho)
        {
            InitializeComponent();
            _cofrinho = cofrinho;
            AtualizarTela();
        }

        private void btnDepositar_Click(object? sender, EventArgs e) => Executar(_cofrinho.Depositar);

        private void btnSacar_Click(object? sender, EventArgs e) => Executar(_cofrinho.Sacar);

        /// <summary>Lê o valor digitado, executa a operação e atualiza a tela.</summary>
        private void Executar(Func<decimal, ResultadoOperacao> operacao)
        {
            if (!ConversorValor.TryConverter(txtValor.Text, out var valor))
            {
                ExibirMensagem("Informe um valor válido, por exemplo 50 ou 12,90.", false);
                txtValor.Focus();
                return;
            }

            ResultadoOperacao resultado;
            try
            {
                resultado = operacao(valor);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                ExibirMensagem($"Não foi possível salvar a operação: {ex.Message}", false);
                return;
            }

            ExibirMensagem(resultado.Mensagem, resultado.Sucesso);
            if (resultado.Sucesso)
                txtValor.Clear();

            txtValor.Focus();
            AtualizarTela();
        }

        private void AtualizarTela()
        {
            lblSaldoValor.Text = _cofrinho.Saldo.ToString("C", PtBr);
            lblLimite.Text = $"Limite de saque restante hoje: {_cofrinho.LimiteRestanteHoje.ToString("C", PtBr)}";

            lstHistorico.BeginUpdate();
            lstHistorico.Items.Clear();
            var transacoes = _cofrinho.Transacoes;
            for (var i = transacoes.Count - 1; i >= 0; i--) // mais recentes primeiro
            {
                lstHistorico.Items.Add(Formatar(transacoes[i]));
            }
            lstHistorico.EndUpdate();
        }

        private static string Formatar(Transacao transacao)
        {
            var (descricao, sinal) = transacao.Tipo switch
            {
                TipoTransacao.Deposito => ("Depósito", "+"),
                TipoTransacao.Saque => ("Saque", "-"),
                _ => ("Taxa", "-")
            };

            return $"{transacao.Data:dd/MM/yyyy HH:mm}   {descricao}   {sinal}{transacao.Valor.ToString("C", PtBr)}";
        }

        private void ExibirMensagem(string texto, bool sucesso)
        {
            lblMensagem.Text = texto;
            lblMensagem.ForeColor = sucesso ? Color.DarkGreen : Color.DarkRed;
        }
    }
}
