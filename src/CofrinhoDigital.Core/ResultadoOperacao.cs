namespace CofrinhoDigital.Core;

/// <summary>Resultado de um depósito ou saque: diz se deu certo e qual mensagem mostrar.</summary>
public sealed record ResultadoOperacao(bool Sucesso, string Mensagem)
{
    public static ResultadoOperacao Ok(string mensagem) => new(true, mensagem);

    public static ResultadoOperacao Falha(string mensagem) => new(false, mensagem);
}
