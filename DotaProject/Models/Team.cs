namespace DotaProject.Models
{
    public class Team
    {
        public String name { get; set; }
        public String url { get; set; }
        public Player[] players { get; set; }
        public String region { get; set; }
        public String location { get; set; }
    }
}
