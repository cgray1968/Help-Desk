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
using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.AccountManagement;

namespace Help_Desk
{
    public partial class LoginAD : System.Web.UI.Page
    {
        string query;
        int userid;
        DataTable dt = new DataTable();
        sqlTypeCommands sql = new sqlTypeCommands();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getSystemInfo();
            }
        }

        protected void getSystemInfo()
        {
            query = "Select top 1 information, datecreated from systeminfo where active = 1 order by datecreated desc";
            dt.Clear();
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
            sql.UpdateSecurityLog(userid.ToString(), data.ToString());
        }

        protected void updateLastLogon(int userid)
        {
            if (userid >= 0)
            {
                string today = DateTime.Now.ToString();
                query = "update login set lastLogin = '" + today + "' where personid=" + userid;
                sql.CommandSQL(query, false, null);
            }
        }

        protected void loginad_Authenticate(object sender, AuthenticateEventArgs e)
        {
           
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "fbcdallas.local"))
            {
                if (context.ValidateCredentials(login1.UserName, login1.Password))
                {
                    lblSystemInfo.Text = login1.UserName;
                    getUserID();
                    
                }
                else
                {
                    login1.FailureText = "Failed login";
                }
            }
        }
        
        protected void getDepartment()
        {
            query = "select DepartmentID, departmentName from Department order by departmentName";
            dt.Clear();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataBind();
            }
        }
        protected void getUserID()
        {
            int userid =-1;
            string query = "select personid from person where loginname = '" + login1.UserName.ToLower() + "'";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count == 0)
            {
                lblSystemInfo.Text = lblSystemInfo.Text + " FAILED! " ;
                //get more information need first name, last name, and department from some web page(?)
                pnlLogin.Visible = false;
                pnlNewUser.Visible = true;
                tbFirstName.Text = "";
                tbLastName.Text = "";
                getDepartment();
            }
            else
            {
                lblSystemInfo.Text = lblSystemInfo.Text + " SUCCESS! " + dt.Rows[0].ItemArray[0].ToString();
                userid = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                successAuthentication(userid);
            }
        }

        protected void successAuthentication(int userid)
        {
            addSecurityInfo(login1.UserName.TrimEnd().ToLower() + " Login Successful", userid);
            updateLastLogon(userid);
            Tickets ticket = new Tickets();
            Session["userid"] = userid;
            FormsAuthentication.SetAuthCookie(login1.UserName, login1.RememberMeSet);
            FormsAuthentication.RedirectFromLoginPage(login1.UserName, login1.RememberMeSet);

        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(tbFirstName.Text)) || 
                (!string.IsNullOrEmpty(tbLastName.Text)) ||
                (!string.IsNullOrEmpty(tbEmailAddress.Text)) ||
                (ddlDepartment.SelectedIndex != 0) )
            {
                query = "insert into person (firstname, lastname, emailaddress, loginname) values (@firstname, @lastname, @emailaddress, @loginName)";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@firstname",  Value=tbFirstName.Text.ToString()},
                    new SqlParameter() { ParameterName = "@lastname",  Value=tbLastName.Text.ToString()},
                    new SqlParameter() { ParameterName = "@emailaddress",  Value=tbEmailAddress.Text.ToString()},
                    new SqlParameter() { ParameterName = "@loginName",  Value=login1.UserName.ToLower()}
                };
                sql.CommandSQL(query, false, sp);
            }
            query = "select personid from person where loginName = '" + login1.UserName.ToLower() +"'";
            dt.Clear();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                userid = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString());
                query = "insert into departmentuser (departmentID, personID) values(@departmentID, @personID)";
                List<SqlParameter> sp1 = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@departmentID", Value=ddlDepartment.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@personid", Value=userid.ToString()}
                };
                sql.CommandSQL(query, false, sp1);

                query = "insert into Personrole (roleid, personid) values (3, " + userid.ToString() + ")";
                string result = sql.CommandSQL(query, false, null);
                if (result != "")
                {
                    successAuthentication(userid);
                }

            }

        }
    }
}
