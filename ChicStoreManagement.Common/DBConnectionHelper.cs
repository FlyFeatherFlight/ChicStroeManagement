using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChicStoreManagement.Common
{
    public class DBConnectionHelper
    {
        protected SqlConnection conn;

        //打开连接

        public bool OpenConnection()

        {

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["chicEntities"].ConnectionString);

            try

            {

                bool result = true;

                if (conn.State.ToString() != "Open")

                {

                    conn.Open();

                }

                return result;

            }

            catch (SqlException ex)

            {

                return false;

            }

        }

        //关闭连接

        public bool CloseConnection()

        {

            try

            {

                conn.Close();

                return true;

            }

            catch (Exception ex)

            {

                return false;

            }

        }

    }

}
