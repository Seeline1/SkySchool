using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SkySchool.Classes
{
    class Manager
    {
        //private static SkySchoolEntities _context;
        public static Frame MainFrame;
        public static User CurrentUser;

        //public static SkySchoolEntities GetContext()
        //{
        //    if (_context == null)
        //        _context = new SkySchoolEntities();
        //    return _context;
        //}

        public static Frame Mainframe
        {
            get => MainFrame;
            set => MainFrame = value;
        }

        public static User Currentuser
        {
            get => CurrentUser;
            set => CurrentUser = value;
        }
    }
}
