using System.Threading.Tasks;
using Challenge.Application.Business.Localizations.Commands;
using Challenge.Application.Business.Localizations.Queries;
using Challenge.Application.Services.Cache;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Common;

namespace Challenge.Application.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dispatcher _dispatcher;
        private readonly IRedisService _redisService;
        private string _language = "tr";

        public LocalizationService(Dispatcher dispatcher, IRedisService redisService)
        {
            _dispatcher = dispatcher;
            _redisService = redisService;
        }

        public async Task<string> ParseAsync(string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
                defaultValue = key;

            var localization = await _redisService.GetAsync<Business.Localizations.Entities.Localization>($"localization-{key}-{_language}", CacheTimes.CACHE_120_DK, async () =>
            {
                var localization = await _dispatcher.Dispatch(new GetLocalizationByKeyAndLanguageCodeQuery { Key = key, LanguageCode = _language });
                if (localization == null)
                {
                    localization = new Business.Localizations.Entities.Localization
                    {
                        Key = key,
                        Value = defaultValue,
                        LanguageCode = _language,
                    };

                    await _dispatcher.Dispatch(new AddUpdateLocalizationCommand { Localization = localization });
                }

                return localization;
            });

            return localization.Value;
        }

        public string CurrentLanguage() => _language;

        public void SetLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                language = "tr";
            else
                _language = language;
        }
    }
}