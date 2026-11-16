using System.Text.Json;
using System.Text.Json.Serialization;

namespace CofrinhoDigital.Core;

/// <summary>Guarda as transações num arquivo JSON.</summary>
public sealed class RepositorioJson : ICofrinhoRepositorio
{
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _caminhoArquivo;

    public RepositorioJson(string caminhoArquivo)
    {
        _caminhoArquivo = caminhoArquivo;
    }

    /// <summary>%LOCALAPPDATA%\CofrinhoDigital\transacoes.json</summary>
    public static string CaminhoPadrao() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CofrinhoDigital",
            "transacoes.json");

    public IReadOnlyList<Transacao> Carregar()
    {
        if (!File.Exists(_caminhoArquivo))
            return new List<Transacao>();

        var json = File.ReadAllText(_caminhoArquivo);
        return JsonSerializer.Deserialize<List<Transacao>>(json, Opcoes) ?? new List<Transacao>();
    }

    public void Salvar(IReadOnlyList<Transacao> transacoes)
    {
        var pasta = Path.GetDirectoryName(_caminhoArquivo);
        if (!string.IsNullOrEmpty(pasta))
            Directory.CreateDirectory(pasta);

        // Grava primeiro num arquivo temporário e depois troca: se o programa fechar
        // no meio da gravação, o arquivo anterior continua inteiro.
        var arquivoTemporario = _caminhoArquivo + ".tmp";
        File.WriteAllText(arquivoTemporario, JsonSerializer.Serialize(transacoes, Opcoes));
        File.Move(arquivoTemporario, _caminhoArquivo, overwrite: true);
    }
}
