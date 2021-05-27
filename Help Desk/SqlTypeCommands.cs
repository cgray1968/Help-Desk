using System;
using System.Collections.Generic;
 using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Web.UI;

namespace Help_Desk

{
    
    public class sqlTypeCommands
    {
        private DataTable dt = new DataTable();
        private string web = ConfigurationManager.ConnectionStrings["HelpDeskConnection"].ConnectionString;
        
            
        public DataTable ReturnDatatable(string query)
        {dt.Reset();
            SqlConnection con = new SqlConnection(web);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            try
            {
                con.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
                con.Close();
            }
            catch (SystemException ex)
            {

            }
            return dt;        }

        public string returnValueFromSproc(string query, bool sproc, List<SqlParameter> sp, string returnValue)
        {
            string value = "";
            
            
            SqlConnection con = new SqlConnection(web);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sp.ToArray());
                SqlParameter param = new SqlParameter("@result", SqlDbType.VarChar);
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);
                value = (string)cmd.ExecuteScalar();

            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }
            return value;
        }
        public bool getIsTech(string userid)
        {
            DataTable dt = new DataTable();
            string query = "select personid from personrole where personid=" + userid + " and roleid = 1 ";
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public void UpdateSecurityLog(string personid, string log)
        {
            string query = "insert securitylogs (securityLogDate, SecurityInfo, PersonID) values (@securityLogDate, @SecurityInfo, @personID)";
            string web = ConfigurationManager.ConnectionStrings["HelpDeskConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(web);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {

                con.Open();
                cmd.Parameters.AddWithValue("@securityLogDate", System.DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@SecurityInfo", log.ToString());
                cmd.Parameters.AddWithValue("@personID", personid.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }
        }


            public string CommadSQL(string query, bool Sproc, List<SqlParameter> sp = null)
        {

            string success = "";
            string web = ConfigurationManager.ConnectionStrings["HelpDeskConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(web);
            SqlCommand cmd = new SqlCommand(query, con);
            if (Sproc)
                cmd.CommandType = CommandType.StoredProcedure;
            if (sp != null)
                cmd.Parameters.AddRange(sp.ToArray());

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                success = ex.ToString();
            }

            return success;
        }
    }

}