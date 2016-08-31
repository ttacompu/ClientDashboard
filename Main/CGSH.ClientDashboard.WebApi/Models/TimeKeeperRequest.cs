using System;
using System.ComponentModel.DataAnnotations;

namespace CGSH.ClientDashboard.WebApi.Models
{
    public class TimeKeeperRequest
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Please enter positive interger")]
        public int Threshold { get; set; }
    }
}