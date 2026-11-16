using System.Globalization;

namespace CofrinhoDigital.Core;

/// <summary>Converte o texto digitado pelo usuário em valor monetário.</summary>
public static class ConversorValor
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    /// <summary>
    /// Aceita vírgula ou ponto como separador decimal ("12,90" ou "12.90") e o prefixo "R$".
    /// Recusa separador de milhar e mais de duas casas decimais: "1.500" é ambíguo
    /// (mil e quinhentos ou um e meio?), então o usuário precisa digitar "1500".
    /// </summary>
    public static bool TryConverter(string? texto, out decimal valor)
    {
        valor = 0m;
        if (string.IsNullOrWhiteSpace(texto))
            return false;

        var normalizado = texto.Trim().Replace("R$", string.Empty).Trim().Replace('.', ',');

        var posicaoVirgula = normalizado.IndexOf(',');
        if (posicaoVirgula >= 0 && normalizado.Length - posicaoVirgula - 1 > 2)
            return false;

        return decimal.TryParse(
            normalizado,
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
            PtBr,
            out valor);
    }
}
