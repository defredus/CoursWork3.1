using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursWorkUI.Interfaces;

namespace CoursWorkUI.Users
{
    public class ManagerMenu : IShowMenu
    {
        private readonly string _id;
        private readonly ServiceStorage _serviceStorage;
        public ManagerMenu(string id, ServiceStorage serviceStorage) {
            _id = id;
            _serviceStorage = serviceStorage;
        }
        public void ShowMenu()
        {

        }
    }
}
