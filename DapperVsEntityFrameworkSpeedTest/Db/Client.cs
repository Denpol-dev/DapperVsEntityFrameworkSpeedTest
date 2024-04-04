namespace DapperVsEntityFrameworkSpeedTest.Db
{
    public class Client
    {
        public Guid ClientId { get; set; }
        public int CountSell { get; set; }
        public int CountBuy { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
