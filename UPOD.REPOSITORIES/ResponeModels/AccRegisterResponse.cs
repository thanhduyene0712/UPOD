namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AccRegisterResponse
    {
        public Guid id { get; set; }
        public string role_name { get; set; }
        public string user_name { get; set; }
        public DateTime? create_date { get; set; }
    }
}
