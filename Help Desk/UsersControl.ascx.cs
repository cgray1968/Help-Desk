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

namespace Help_Desk
{
    public partial class UsersControl : System.Web.UI.UserControl
    {
        public bool _recordChange;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) || (ddlGetUser.Items.Count == 0))
            {
                hfRecordChange.Value = "true";
                pnlUserInfo.Visible = false;
                getUserDDL();
                getUserRole();
                hflNewUserID.Value = "-2";
                hfNewRecord.Value = "false";
                hfRecordChange.Value = "false";
                HideAllPanels();
            }
        }

        protected void deleteUser(string uid)
        {
            string query = "delete from person where person_id = '" + uid.ToString() + "'";
            sqlTypeCommands sql = new sqlTypeCommands();
            string result;
            result = sql.CommandSQL(query, false, null);
            if (result.ToString() != "")
            {
               //warn deletion not successful!
            }

        }

        protected void updateGridview(GridView gv, string query)
        {
            gv.DataSource = null;
            gv.DataBind();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.DataBind();
            }
        }

        protected void HideAllPanels()
        {
            pnlUserInfo.Visible = false;
            pnlUser.Visible = true;
            pnlSelectComputer.Visible = false;
            gvNewUserTickets.DataSource = null;
            gvNewUserTickets.DataBind();
            gvNewUserComputer.DataSource = null;
            gvNewUserComputer.DataBind();
            gvNewRecentActivity.DataSource = null;
            gvNewRecentActivity.DataBind();
            tbNewUserFirstName.Text = "";
            tbNewUserLastName.Text = "";
            tbNewUserEmail.Text = "";
            tbNewUserLogin.Text = "";
            if (ddlNewUserRole.Items.Count == 0)
                getUserRole();
            ddlNewUserRole.SelectedIndex = 0;
            if (ddlNewUserDepartment.Items.Count == 0)
                getNewUserDepartment();
            ddlNewUserDepartment.SelectedIndex = 0;

        }

        protected void getUserDDL()
        {
            getNewDepartment();
            getNewUserDDL();
        }

        protected void getNewUserDepartment()
        {
            string query = "select departmentid, departmentname from department order by departmentname";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlNewUserDepartment.DataSource = null;
                ddlNewUserDepartment.DataBind();
                ddlNewUserDepartment.DataSource = dt;
                ddlNewUserDepartment.DataTextField = "DepartmentName";
                ddlNewUserDepartment.DataValueField = "DepartmentID";
                ddlNewUserDepartment.DataBind();
                ddlNewUserDepartment.Items.Insert(0, new ListItem("No Department", "-1"));
            }
        }

        protected void getUserComputer()
        {
            string query;
            DataTable dt = new DataTable();
            query = "select * from V_Computer where personid=" + ddlGetUser.SelectedValue.ToString();
            updateGridview(gvNewUserComputer, query);
        }


        protected void setMessagelbl(string message)
        {
            lblMessage.Text = message.ToString();
            if (message != "")
                lblMessage.Visible = true;
            else
                lblMessage.Visible = false;
        }


        protected void getDDLSelectedComputers()
        {
            if (hfRecordChange.Value == "false")
                {
                string query = "select distinct computerid, computername from V_User_Computers where personID not in(" + ddlGetUser.SelectedValue.ToString() + ")";
                DataTable dt = new DataTable();
                sqlTypeCommands sql = new sqlTypeCommands();
                dt = sql.ReturnDatatable(query);
                if (dt.Rows.Count > 0)
                {
                    ddlSelectComputer.Items.Clear();
                    ddlSelectComputer.DataSource = dt;
                    ddlSelectComputer.DataTextField = "ComputerName";
                    ddlSelectComputer.DataValueField = "ComputerID";
                    ddlSelectComputer.DataBind();
                    ddlSelectComputer.Items.Insert(0, new ListItem("Select Computer...", "-1"));
                }
            }

        }

        protected void AddComputer()
        {
            if ((ddlSelectComputer.SelectedValue != "-1") ||
                (hflNewUserID.Value != "-1"))
            {
                string query = "UpdateUserComputer";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
            {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlGetUser.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@Computerid", Value = ddlSelectComputer.SelectedValue.ToString()}
            };

                string result = sql.CommandSQL(query, true, sp);
                setMessagelbl(result);
            }
        }

        protected void getUserRole()
        {
            string query = "select roleid, role from role";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlNewUserRole.DataSource = null;
                ddlNewUserRole.DataBind();
                ddlNewUserRole.DataSource = dt;
                ddlNewUserRole.DataValueField = "roleid";
                ddlNewUserRole.DataTextField = "role";
                ddlNewUserRole.DataBind();
            }
            ddlNewUserRole.Items.Insert(0, new ListItem("Select Role", "-1"));
            ddlNewUserRole.SelectedIndex = 0;
        }

        protected void deleteUserComputer()
        {
            String query = "delete from computerUser where computerid = " + hfComputerID.Value.ToString() + " and personid = " + ddlGetUser.SelectedValue.ToString();
            sqlTypeCommands sql = new sqlTypeCommands();
            string result = sql.CommandSQL(query, false, null);
            lblMessage.Text = result.ToString();
            setMessagelbl(result);

        }


        protected void getNewUserInfo()
        {
            if (ddlGetUser.SelectedIndex.ToString() != "0")
            {
                hfRecordChange.Value = "true";
                hfNewRecord.Value = "false";
                DataTable user = new DataTable();
                DataTable dt = new DataTable();
                sqlTypeCommands sql = new sqlTypeCommands();
                string query;
                pnlUserInfo.Visible = true;
                //get user info
                query = "select * from v_users where personid = " + ddlGetUser.SelectedValue.ToString();
                user = sql.ReturnDatatable(query);
                if (user.Rows.Count > 0)
                {
                    //get DDL For Department
                    if (ddlNewUserDepartment.Items.Count == 0)
                    {
                        getNewUserDepartment();

                    }

                    //Get DDL Roles
                    if (ddlNewUserRole.Items.Count == 0)
                    {
                        getUserRole();
                    }

                    ddlNewUserDepartment.SelectedValue = user.Rows[0].ItemArray[5].ToString();
                    //get DDL Tickets
                    query = "select ticketID, " + hflNewUserID.Value.ToString() + " as PersonID,  description from ticket where personid = " + ddlGetUser.SelectedValue.ToString();
                    updateGridview(gvNewUserTickets, query);
                    query = "select * from v_users where personID = " + ddlGetUser.SelectedValue.ToString();
                    dt.Reset();
                    dt = sql.ReturnDatatable(query);
                    if (dt.Rows.Count > 0)
                    {
                        tbNewUserFirstName.Text = dt.Rows[0].ItemArray[1].ToString();
                        tbNewUserLastName.Text = dt.Rows[0].ItemArray[2].ToString();
                        tbNewUserEmail.Text = dt.Rows[0].ItemArray[3].ToString();
                        tbNewUserLogin.Text = dt.Rows[0].ItemArray[4].ToString();
                    }

                    query = "select top 1 roleid from PersonRole where personid = " + ddlGetUser.SelectedValue.ToString();
                    dt.Reset();
                    dt = sql.ReturnDatatable(query);
                    if (dt.Rows.Count > 0)
                    {
                        ddlNewUserRole.SelectedValue = dt.Rows[0].ItemArray[0].ToString();
                    }
                    else
                        ddlNewUserRole.SelectedValue = "-1";
                    //Get Comuters
                    getUserComputer();
                    //Get Last 5 Activity
                    query = "select top 5 securitylogdate, securityinfo from securityLogs where personid = " + ddlGetUser.SelectedValue.ToString() + " order by securitylogdate desc";
                    updateGridview(gvNewRecentActivity, query);
                   
                   
                }
                getDDLSelectedComputers();
                hfRecordChange.Value = "false";
            }
        }

        protected void GetNewUserTickets()
        {
            //get DDL Tickets
            {
                string query = "select ticketID, " + hflNewUserID.Value.ToString() + " as personID, description from ticket where personid = " + ddlGetUser.SelectedValue.ToString() + " order by ticketid";
                updateGridview(gvNewUserTickets, query);
            }
        }

        protected void getNewUserDDL()
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            
            string query = "select p.personid,	lastname + ', ' + firstname as PersonName from person p left outer join DepartmentUser du on p.personid = du.personid ";


            if (ddlNewDepartment.SelectedValue.ToString() != "-1")
            {
                query += " where p.personid = 1 or departmentid = " + ddlNewDepartment.SelectedValue.ToString();
            }
           
            query += " order by (case when p.personid = 1 then 0 else 1 end), LastName, firstname";
            dt.Reset();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                string changeMeBack = "false";
                if (hfRecordChange.Value == "false")
                {
                    changeMeBack = "true";
                    hfRecordChange.Value = "true";
                }
                ddlGetUser.DataSource = null;
                ddlGetUser.DataBind();
                ddlGetUser.DataSource = dt;
                ddlGetUser.DataValueField = "Personid";
                ddlGetUser.DataTextField = "personName";
                ddlGetUser.DataBind();
                if (changeMeBack == "true")
                    hfRecordChange.Value = "false";
            }
        }

        protected void getNewDepartment()
        {
            String query = "select * from department";
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlNewDepartment.DataSource = null;
                ddlNewDepartment.DataBind();
                ddlNewDepartment.DataSource = dt;
                ddlNewDepartment.DataValueField = "DepartmentID";
                ddlNewDepartment.DataTextField = "DepartmentName";
                ddlNewDepartment.DataBind();
                ddlNewDepartment.Items.Insert(0, new ListItem("All Departments", "-1"));
                ddlNewDepartment.SelectedIndex = 0;
            }
        }


        protected void ddlNewDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfRecordChange.Value == "false")
                getNewUserDDL();
        }

        protected void ddlGetUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfRecordChange.Value == "false")
                getNewUserInfo();
        }

        protected void tbNewUserFirstName_TextChanged(object sender, EventArgs e)
        {
            if ((hfNewRecord.Value == "false") && (tbNewUserFirstName.Text != ""))
            {
                sqlTypeCommands sql = new sqlTypeCommands();
                string query = "update person set FirstName = @firstname where personid = @personid";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                      new SqlParameter() { ParameterName = "@firstname", Value = tbNewUserFirstName.Text.ToString() },
                      new SqlParameter() { ParameterName = "@personid", Value= ddlGetUser.SelectedValue.ToString()}
                };
                string result = sql.CommandSQL(query, false, sp);
                setMessagelbl(result);
            }
        }

        protected void tbNewUserLastName_TextChanged(object sender, EventArgs e)
        {
            if ((hfNewRecord.Value == "false") && (tbNewUserLastName.Text != ""))
            {
                sqlTypeCommands sql = new sqlTypeCommands();
                string query = "update person set lastname = @lastname where personid=@personid";
                List<SqlParameter> sp = new List<SqlParameter>
                    {
                    new SqlParameter() { ParameterName = "@lastname", Value = tbNewUserLastName.Text.ToString()},
                    new SqlParameter() { ParameterName = "@personid", Value = ddlGetUser.SelectedValue.ToString()}
                };
                string result = sql.CommandSQL(query, false, sp);
                setMessagelbl(result);
            }

        }

        protected void tbNewUserEmail_TextChanged(object sender, EventArgs e)
        {
            if ((hfNewRecord.Value == "false") && (tbNewUserEmail.Text != ""))
            {
                string query = "update person set emailaddress = @emailaddress where personid = @personid";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
                    {
                    new SqlParameter() { ParameterName = "@emailaddress", Value = tbNewUserEmail.Text.ToString()},
                    new SqlParameter() { ParameterName = "@personid", Value = ddlGetUser.SelectedValue.ToString() }
                };
                string result = sql.CommandSQL(query, false, sp);
                setMessagelbl(result);
            }
        }

        protected void ddlNewUserDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfNewRecord.Value == "false")
            {
                string query = "sp_updateDepartmentUser";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
            {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlGetUser.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@departmentid", Value = ddlNewUserDepartment.SelectedValue.ToString()}
            };

                string result = sql.CommandSQL(query, true, sp);
                lblMessage.Text = result.ToString();
                if (result != "")
                {
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Visible = false;
                }
            }
  
        }

        protected void ddlNewUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlNewUserRole.SelectedIndex.ToString() != "-1") && (hfNewRecord.Value == "false"))
            {
                string query = "sp_updateRole";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
            {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlGetUser.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@roleid", Value = ddlNewUserRole.SelectedValue.ToString()}
            };

                string result = sql.CommandSQL(query, true, sp);
                setMessagelbl(result);
            }
        }

   
          protected void gvNewUserTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvNewUserTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNewUserTickets.PageIndex = e.NewPageIndex;
        }

        protected void gvNewUserComputer_RowCommand(object sender, GridViewCommandEventArgs e)
        {   //deleteComputer
            //btnAddUserComputer_Command
            switch (e.CommandName)
            {
                case "btnDeleteUserComputer_Command":
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = gvNewUserComputer.Rows[index];
                    int computerid = Convert.ToInt32(selectedRow.Cells[0].Text.ToString());
                    hfComputerID.Value = computerid.ToString();
                    deleteUserComputer();
                    break;
            }
        }

        protected void btnAddComputer_Click(object sender, ImageClickEventArgs e)
        {
            getDDLSelectedComputers();
            HideAllPanels();
            pnlSelectComputer.Visible = true;
        }

          protected void ddlSelectComputer_Load(object sender, EventArgs e)
        {
            getDDLSelectedComputers();
        }

        protected void btnOKSelecteComputer_Click(object sender, EventArgs e)
        {
            if (ddlSelectComputer.SelectedValue != "-1")
            {
                string query = "UpdateUserComputer";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@ComputerID", Value = ddlSelectComputer.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@PersonID", Value = ddlGetUser.SelectedValue.ToString()}
                };
                string result = sql.CommandSQL(query, true, sp);
                getUserComputer();
                getNewUserInfo();
                pnlSelectComputer.Visible = false;
                pnlUserInfo.Visible = true;
            }
        }

        protected void btnCancelSelectComputer_Click(object sender, EventArgs e)
        {
            hflNewUserID.Value = "-2";
            HideAllPanels();
            pnlUser.Visible = true;
            pnlUserInfo.Visible = false;
        }

    }
}