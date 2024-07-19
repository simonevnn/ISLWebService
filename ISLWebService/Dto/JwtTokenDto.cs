namespace ISLWebService.Dto
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public JwtTokenDto(string token)
        {
            Token = token;
        }
    }
}
