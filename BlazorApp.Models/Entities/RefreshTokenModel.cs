namespace BlazorApp.Models.Entities
{
    public class RefreshTokenModel
    {
        public int ID { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
