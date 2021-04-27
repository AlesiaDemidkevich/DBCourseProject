using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Connection
{
    public static class OracleDatabaseConnection
    {
        public static string connection = "Data Source = " +
                                          "(DESCRIPTION = " +
                                          "(ADDRESS_LIST =  " +
                                          "(ADDRESS = " +
                                          "(PROTOCOL = TCP)" +
                                          "(HOST = 192.168.56.105)" +
                                          "(PORT = 1521)))" +
                                          "(CONNECT_DATA = " +
                                          "(SERVER = DEDICATED)" +
                                          "(SERVICE_NAME = CINEMA.be.by)));" +
                                          "User Id=admin;Password=Alesya2001;";

    }
}
