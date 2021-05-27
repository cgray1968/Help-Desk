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
                sqlc.CommadSQL(query, false, null);
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
                    string query = "select top 1 NeedsPasswordChange from login where personid = " + userid.ToString();
                    DataTable dt = new DataTable();
                    sqlTypeCommands sql = new sqlTypeCommands();
                    dt = sql.ReturnDatatable(query);
                    bool Change = Convert.ToBoolean(dt.Rows[0].ItemArray[0].ToString());
                    if (Change)
                    {
                        hfid.Value = userid.ToString();
                        changePassword(userid.ToString());
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
            }
            catch (SystemException ex)
            {
            }
        }
        
        protected void changePassword(string userid)
        {
            pnlLogin.Visible = false;
            pnlChangePassword.Visible = true;

        }


        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if ((tbNewPassword.Text == tbNewPasswordConfirm.Text) &&
                ((tbNewPassword.Text.Length > 3) || (tbNewPasswordConfirm.Text.Length > 3)))
            {
                string query = "uspChangePassword";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@pPersonID", Value=hfid.Value.ToString() },
                    new SqlParameter() { ParameterName = "@pPassword", Value=tbNewPassword.Text.ToString() },
                    new SqlParameter() { ParameterName = "@responseMessage", Value=DBNull.Value }
                };
                sqlTypeCommands sql = new sqlTypeCommands();
                string result = sql.CommadSQL(query, true, sp);
                if (result != "")
                {
                    lblChangePasswordError.Text = "Error saving password.";

                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }

            else
            {
                lblChangePasswordError.Text = "Passwords need to match, or password needs to be longer than 3 characters.";
                  
            }

        }
    }
}
