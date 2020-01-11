using NeoDataLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoDataLayer
{
    public class Store
    {
        public User loggedUser { get; set; }
        private static Store instance;

        public static Store GetInstance()
        {
            if (instance == null)
            {
                instance = new Store();
            }
            return instance;
        }

        private Store()
        {
            loggedUser = null;
        }
        public User GetUser()
        {
            return loggedUser;
        }
        public bool SetUser(NeoDataLayer.DomainModel.User u)
        {
            try
            {
                loggedUser = u;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
