﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_WPF_EF_PersonalTracking
{
    public static class UserStatic
    {
        public static int EmployeeId { get; set; }

        public static bool IsAdmin { get; set; }

        public static int UserNo { get; set; }

        public static string Name { get; set; }

        public static string Surname { get; set; }

        public static bool isShutdownCalled { get; set; }
    }
}
