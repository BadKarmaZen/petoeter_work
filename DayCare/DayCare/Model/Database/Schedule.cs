using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
    public class Schedule : DatabaseRecord
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value.Date; }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set 
            {
                _endDate = value.Date;
                _endDate.Add(new TimeSpan(23,59,59));
            }
        }

        public Guid Child_Id { get; set; }
        public Guid Group_Id { get; set; }

        public bool MondayMorning { get; set; }
        public bool MondayAfternoon { get; set; }
        public bool TuesdayMorning { get; set; }
        public bool TuesdayAfternoon { get; set; }
        public bool WednesdayMorning { get; set; }
        public bool WednesdayAfternoon { get; set; }
        public bool ThursdayMorning { get; set; }
        public bool ThursdayAfternoon { get; set; }
        public bool FridayMorning { get; set; }
        public bool FridayAfternoon { get; set; }

        public Schedule()
        {
            Group_Id = Guid.Empty;
        }

        public bool InPeriod(Schedule schedule)
        {
            if (this.StartDate < schedule.StartDate && this.EndDate > schedule.EndDate)
	        {
                return true;		 
	        }

            return false;
        }

        public override DatabaseRecord Copy()
        {
            return new Schedule 
            {
                Id = this.Id,
                Child_Id = this.Child_Id,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Group_Id = this.Group_Id,
                MondayMorning = this.MondayMorning,
                MondayAfternoon = this.MondayAfternoon,
                TuesdayMorning = this.TuesdayMorning,
                TuesdayAfternoon = this.TuesdayAfternoon,
                WednesdayMorning = this.WednesdayMorning,
                WednesdayAfternoon = this.WednesdayAfternoon,
                ThursdayMorning = this.ThursdayMorning,
                ThursdayAfternoon = this.ThursdayAfternoon,
                FridayMorning = this.FridayMorning,
                FridayAfternoon = this.FridayAfternoon
            };
        }
    }
}
