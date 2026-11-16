using System.Text.Json;
using CofrinhoDigital.Core;

namespace CofrinhoDigital.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Cofrinho cofrinho;
            try
            {
                cofrinho = new Cofrinho(new RepositorioJson(RepositorioJson.CaminhoPadrao()));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
            {
                // Não abre o app com o cofrinho vazio: isso sobrescreveria o histórico salvo.
                MessageBox.Show(
                    $"Não foi possível carregar o histórico do cofrinho.\n\n{ex.Message}",
                    "Cofrinho Digital",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FrmCofrinho(cofrinho));
        }
    }
}
