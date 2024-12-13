namespace DotaProject.Models
{
    public class Player
    {
        public int Id { get; set; }
        public String PlayerIgn {  get; set; }
        public String PlayerName { get; set; }
        public String Nationality { get; set; }
        public int Earnings { get; set; }
        public DateTime DateofBirth { get; set; }
        public String Region { get; set; }
        public String YearsActive { get; set; }
        public String[] CurrentRoles { get; set; }
        public Team? Team { get; set; }
        public String Url { get; set; }


    }
}
