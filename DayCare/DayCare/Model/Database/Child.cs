using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
    public class Child : DatabaseRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public Guid Account_Id { get; set; }
        public bool Present { get; set; }

        public override DatabaseRecord Copy()
        {
            return new Child
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Account_Id = this.Account_Id,
                BirthDay = this.BirthDay,
                Present = this.Present,
                Deleted = this.Deleted
            };
        }
    }
}
