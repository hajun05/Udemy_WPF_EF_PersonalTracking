using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_WPF_EF_PersonalTracking.ViewModels
{
    public class EmployeeDetailModel
    {
        public int Id { get; set; }

        public int UserNo { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public int Salary { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime BirthDay { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string ImagePath { get; set; }
    }
}
