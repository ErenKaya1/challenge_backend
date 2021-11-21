using System.Collections.Generic;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Business.Users.Queries;
using Challenge.Core.Response;

namespace Challenge.Backoffice.Models.BackofficeUser
{
    public class BackofficeUserIndexVM
    {
        public GetBackofficeUsersQuery Query { get; set; }
        public BaseQueryResult<List<User>> Result { get; set; }
    }
}