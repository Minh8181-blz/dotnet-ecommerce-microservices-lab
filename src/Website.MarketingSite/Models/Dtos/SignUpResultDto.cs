namespace Website.MarketingSite.Models.Dtos
{
    public class SignUpResultDto
    {
        public bool Succeeded { get; set; }
        public UserDto User { get; set; }
        public string Message { get; set; }
    }
}
