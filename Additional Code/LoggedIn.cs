using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAssignmentPart2.Additional_Code
{
    public static class LoggedIn
    {
        private static bool logIn = false;

        public static bool IsLoggedIn
        {
            get { return logIn; }
            set { logIn = value; }

        }

        public static void  SetLoggedIn(bool a)
        {
             logIn  = a;
        }

        public static bool getLoggedIn()
        {
            return logIn;
        }
    }
}
