﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_WPF_EF_PersonalTracking.ViewModels
{
    public class TaskDetailModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime TaskStartDate { get; set; }

        public DateTime TaskDeliveryDate { get; set; }

        public int TaskState { get; set; }

        public string TaskTitle { get; set; }

        public string TaskContent { get; set; }

        public int UserNo { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string StateName { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }
    }
}
