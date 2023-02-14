namespace Translate.Google;
public interface IGetTranslateGoogle
{
    Task<string?> GetTranslateAsync(string texto, string idiomaOrigem, string idiomaDestino);
}
