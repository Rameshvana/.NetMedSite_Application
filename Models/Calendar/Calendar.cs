namespace MvcLightphrm_Prject.Models.Calendar
{
    public class Calendar
    {
        public string CalendarUniqueId { get; set; }

        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int Year { get; set; }
        public string ProductName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ReportDate { get; set; }
        public string siteUniqueId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SiteUniqueId { get; set; }


    }
}
