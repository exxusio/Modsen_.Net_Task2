namespace BusinessLogicLayer.Dtos.Users
{
    public class AuthenticationResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
