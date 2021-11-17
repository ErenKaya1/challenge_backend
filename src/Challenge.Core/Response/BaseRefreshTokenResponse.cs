namespace Challenge.Core.Response
{
    public class BaseRefreshTokenResponse<T> : BaseResponse<T>
    {
        public string RefreshToken { get; set; }
    }
}