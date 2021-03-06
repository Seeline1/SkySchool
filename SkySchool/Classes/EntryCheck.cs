using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SkySchool.Classes;
using SkySchool.Pages;

namespace SkySchool.Classes
{
    class EntryCheck
    {
        public static bool Login(string login, string password)
        {
            if (login.Length > 0)
            {
                if (password.Length > 0)
                {
                    using (SkySchoolEntities db = new SkySchoolEntities())
                    {
                        Manager.Currentuser = db.User.Where(p => p.Login.Equals(login) && p.Parol.Equals(password)).FirstOrDefault();
                    }

                    if (Manager.Currentuser != null)
                    {
                        MessageBox.Show("Пользователь успешно авторизовался");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Введите логин");
                return false;
            }
        }
    }
}
