using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace Help_Desk
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getSystemInfo();
            }
        }

        protected void getSystemInfo()
        {
            string query = "Select top 1 information, datecreated from systeminfo where active = 1 order by datecreated desc";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                lblSystemInfo.Visible = true;
                lblSystemInfo.BackColor = System.Drawing.Color.LightSkyBlue;
                lblSystemInfo.ForeColor = System.Drawing.Color.Firebrick;

                lblSystemInfo.Text = dt.Rows[0][1].ToString() + ' ' + dt.Rows[0][0].ToString();
            }
            else
            {
                lblSystemInfo.Visible = false;
                lblSystemInfo.Text = "";
            }

        }

        protected void addSecurityInfo(string data, int userid)
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            sql.UpdateSecurityLog(userid.ToString(), data.ToString());
        }

        protected void updateLastLogon(int userid)
        {
            if (userid >= 0)
            {
                string today = DateTime.Now.ToString();
                string query = "update login set lastLogin = '" + today + "' where personid=" + userid;
                sqlTypeCommands sqlc = new sqlTypeCommands();
                sqlc.CommandSQL(query, false, null);
            }
        }

 

        protected void login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            int userid = -1;
            string web = ConfigurationManager.ConnectionStrings["HelpDeskConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(web);
            try
            {
                SqlCommand com = new SqlCommand("uspLogin", con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@pLoginName", login1.UserName.ToString());
                com.Parameters.AddWithValue("@pPassword", login1.Password.ToString());
                con.Open();

                userid = Convert.ToInt32(com.ExecuteScalar());
                if (userid == -1)
                {
                    addSecurityInfo(login1.UserName.TrimEnd().ToLower() + " login Failed", userid);
                    login1.FailureText = "Failed login";
                }

                else
                {
                    addSecurityInfo(login1.UserName.TrimEnd().ToLower() + " Login Successful", userid);
                    updateLastLogon(userid);
                    Tickets ticket = new Tickets();
                    Session["userid"] = userid;
                    FormsAuthentication.RedirectFromLoginPage(login1.UserName, login1.RememberMeSet);

                }
            }
            catch (SystemException ex)
            {
            }
        }
    }
}
