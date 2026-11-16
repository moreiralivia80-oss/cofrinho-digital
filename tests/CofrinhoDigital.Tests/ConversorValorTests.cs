using CofrinhoDigital.Core;

namespace CofrinhoDigital.Tests;

public class ConversorValorTests
{
    [Theory]
    [InlineData("50", 50)]
    [InlineData("12,90", 12.90)]
    [InlineData("12.90", 12.90)]
    [InlineData(" R$ 7,5 ", 7.5)]
    public void Converte_valores_validos(string texto, double esperado)
    {
        Assert.True(ConversorValor.TryConverter(texto, out var valor));
        Assert.Equal((decimal)esperado, valor);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("1.500")]
    [InlineData("1.000,00")]
    [InlineData("10,555")]
    public void Recusa_valores_invalidos_ou_ambiguos(string? texto)
    {
        Assert.False(ConversorValor.TryConverter(texto, out _));
    }
}
