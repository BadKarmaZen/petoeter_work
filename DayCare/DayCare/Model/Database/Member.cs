using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
    public class Member : DatabaseRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid Account_Id { get; set; }

    }
}
