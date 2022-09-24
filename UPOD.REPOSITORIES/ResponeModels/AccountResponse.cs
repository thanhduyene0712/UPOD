namespace UPOD.REPOSITORIES.ResponeModels
{
    public class AccountResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public RoleResponse role { get; set; } = null!;
        public string? username { get; set; }
        public string? password { get; set; }
        public bool? is_delete { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
