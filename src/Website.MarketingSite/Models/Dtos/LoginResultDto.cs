namespace Website.MarketingSite.Models.Dtos
{
    public class LoginResultDto
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int TokenExpiresIn { get; set; }
        public string Message { get; set; }
    }
}
