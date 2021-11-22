using System.Collections.Generic;
using Challenge.Application.Business.Policies.Queries;
using Challenge.Core.Response;

namespace Challenge.Backoffice.Models.Policy
{
    public class PolicyIndexVM
    {
        public GetPoliciesQuery Query { get; set; }
        public BaseQueryResult<List<Application.Business.Policies.Entities.Policy>> Result { get; set; }
    }
}