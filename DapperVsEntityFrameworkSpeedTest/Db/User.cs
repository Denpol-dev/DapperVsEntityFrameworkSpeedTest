namespace DapperVsEntityFrameworkSpeedTest.Db
{
    public class User
    {
        public User()
        {
            Clients = [];
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
