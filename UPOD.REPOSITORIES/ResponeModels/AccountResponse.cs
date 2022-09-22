namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AccountResponse
    {
        public Guid id { get; set; }
        public Guid? role_id { get; set; }
        public string username { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
