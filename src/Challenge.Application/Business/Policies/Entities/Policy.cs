using Challenge.Common;
using Challenge.Core.Attributes;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Policies.Entities
{
    public class Policy : AggregateRoot<string>
    {
        [CustomRequired]
        public string Title { get; set; }
        
        public string Slug { get; set; }

        [CustomRequired]
        public string Content { get; set; }
        
        public PolicyType PolicyType { get; set; }
    }
}