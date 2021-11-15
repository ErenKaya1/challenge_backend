using System;
using Challenge.Common;

namespace Challenge.Application.Business.Localizations.Entities
{
    public class Localization : AggregateRoot<string>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string LanguageCode { get; set; }
        public Guid ContentId { get; set; }
    }
}