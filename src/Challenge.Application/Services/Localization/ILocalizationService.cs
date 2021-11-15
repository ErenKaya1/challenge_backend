using System.Threading.Tasks;

namespace Challenge.Application.Services.Localization
{
    public interface ILocalizationService
    {
        Task<string> ParseAsync(string key, string defaultValue);
        string CurrentLanguage();
        void SetLanguage(string language);
    }
}