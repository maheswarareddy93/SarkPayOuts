using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models
{
    public class BookingNewViewModel
    {
        public List<AgentModel> lstAgents { get; set; }
        public List<NewBookingViewModel> lstBookings {get;set;}
        public LayOutViewModel projectStatus { get; set; }
    }
}
