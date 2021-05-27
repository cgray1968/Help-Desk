using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace Help_Desk
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            hfLoginID.Value = Request.QueryString["uid"];
            

            if (!IsPostBack)
            {
                pnlCategory.Visible = false;
                pnlCategoryInput.Visible = false;
                pnlComputerEdit.Visible = false;
                pnlComputers.Visible = false;
                pnlDepartment.Visible = false;
                pnlDepartmentEdit.Visible = false;
                pnlMessages.Visible = false;
                pnlUsersEdit.Visible = false;
                pnlUsersGrid.Visible = false;
                getNewDepartment();
                getNewUserDDL();
            }

        }

        protected void menuTicket_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

        protected void HideAllPanels()
        {
            pnlCategory.Visible = false;
            pnlCategoryInput.Visible = false;
            pnlComputerEdit.Visible = false;
            pnlComputers.Visible = false;
            pnlDepartment.Visible = false;
            pnlDepartmentEdit.Visible = false;
            pnlMessages.Visible = false;
            pnlUsersEdit.Visible = false;
            pnlUsersGrid.Visible = false;
        }

        protected void getNewUserDDL()
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            string query = "Select personid, firstName + ' ' + lastName as personName from v_users";

            if (ddlNewDepartment.SelectedValue.ToString() != "-1")
            {
                query += " where departmentid = " + ddlNewDepartment.SelectedValue.ToString();
            }
            dt.Clear();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlGetUser.DataSource = null;
                ddlGetUser.DataBind();
                ddlGetUser.DataSource = dt;
                ddlGetUser.DataValueField = "Personid";
                ddlGetUser.DataTextField = "personName";
                ddlGetUser.DataBind();
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

        protected void getUserGrid()
        {
            // string query = "select * from V_users";
            //updateGridview(gvUsers, query);
            getNewDepartment();
            getNewUserDDL();


        }

        protected void getMessages()
        {
            string query = "Select * from V_Systeminfo";
            updateGridview(gvMessages, query);
        }

        protected void getDepartment()
        {
            string query = "select * from department";
            updateGridview(gvDepartment, query);
        }

        protected void getComputers()
        {
            string query = "select * from v_computer";
            updateGridview(gvComputers, query);
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

        protected void getCategories()
        {
            string query = "select * from category";
            updateGridview(gvcategory, query);

        }

        protected void menuAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (menuAdmin.SelectedItem.Text)
            {
                case "Return":
                    Response.Redirect("Tickets.aspx");
                    break;
                case "Users":
                    HideAllPanels();
                    pnlUsersGrid.Visible = true;
                    pnlUsersGrid.Visible = true;
                    getUserGrid();
                    break;
                case "New User":
                    HideAllPanels();
                    pnlUsersGrid.Visible = true;
                    pnlUsersEdit.Visible = true;
                    getUserGrid();
                    getDDLUserComputer();
                    break;
                case "Import Users from A/D":
                    break;
                case "System Logs":
                    HideAllPanels();
                    pnlMessages.Visible = true;
                    getMessages();
                    break;
                case "Create New Info":
                    HideAllPanels();
                    pnlMessages.Visible = true;
                    getMessages();
                    break;
                case "Department":
                    HideAllPanels();
                    pnlDepartment.Visible = true;
                    getDepartment();
                    getDDLUserDepartment();
                    break;
                case "New Department":
                    HideAllPanels();
                    pnlDepartment.Visible = true;
                    getDepartment();
                    pnlDepartmentEdit.Visible = true;
                    break;
                case "Computer":
                    HideAllPanels();
                    pnlComputers.Visible = true;
                    getComputers();
                    break;
                case "New Computer":
                    HideAllPanels();
                    pnlComputers.Visible = true;
                    pnlComputerEdit.Visible = true;
                    getComputers();
                    break;
                case "Import Computers from A/D":
                    break;
                case "Category":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    getCategories();
                    break;
                case "New Category":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    HFType.Value = "category";
                    lbltypecategory.Text = "Category: ";
                    getCategories();
                    pnlCategoryInput.Visible = true;
                    break;
                case "SubCategory":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    getCategories();
                    break;
                case "New SubCategory":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    HFType.Value = "subcategory";
                    lbltypecategory.Text = "SubCategory: ";
                    getCategories();
                    pnlCategoryInput.Visible = true;
                    break;
                case "Selection":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    getCategories();
                    break;
                case "New Selection":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    HFType.Value = "selection";
                    lbltypecategory.Text = "Selection: ";
                    getCategories();
                    pnlCategoryInput.Visible = true;
                    break;
            }
        }

        protected void getDDLUserDepartment()
        {
            string query = "select departmentID, departmentname from department";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlUserDepartment.DataSource = dt;
                ddlUserDepartment.DataValueField = "DepartmentID";
                ddlUserDepartment.DataTextField = "DepartmentName";
                ddlUserDepartment.DataBind();
                ddlUserDepartment.Items.Insert(0, new ListItem("All Department", "-1"));
                ddlUserDepartment.SelectedIndex = 0;
            }
        }

        protected void getDDLUserComputer()
        {
            string query = "select departmentID, departmentname from department";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlUserComputer.DataSource = dt;
                ddlUserComputer.DataValueField = "DepartmentID";
                ddlUserComputer.DataTextField = "DepartmentName";
                ddlUserComputer.DataBind();
                ddlUserComputer.Items.Insert(0, new ListItem("No Ticket", "-1"));
                ddlUserComputer.SelectedIndex = 0;
            }

        }


        protected void editUsers(string UserID)
        {
            string query = "select * from v_users where personid = " + UserID.ToString();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                tbUserFirstName.Text = dt.Rows[0].ItemArray[1].ToString();
                tbUserLastName.Text = dt.Rows[0].ItemArray[2].ToString();
                tbUserEmailAddress.Text = dt.Rows[0].ItemArray[3].ToString();
                hfLoginID.Value = dt.Rows[0].ItemArray[4].ToString();
                tbUserLoginName.Text = dt.Rows[0].ItemArray[5].ToString();
                getDDLUserDepartment();

            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;
            int UserID;
            switch (e.CommandName.ToString())
            {

                case "btnUserEdit":
                    index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = gvUsers.Rows[index];
                    UserID = Convert.ToInt32(selectedRow.Cells[1].Text.ToString());
                    hfPersonID.Value = UserID.ToString();
                    editUsers(UserID.ToString());
                    break;
                case "btnUserDelete":
                    index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedrow = gvUsers.Rows[index];
                    UserID = Convert.ToInt32(selectedrow.Cells[1].Text.ToString());
                    deleteUser(UserID.ToString());
                    break;
            }

        }

        protected void saveUser()
        {
            if (hfPersonID.Value == "-1")
            {
                if (tbUserPassword.Text.ToString() != tbUserPasswordConfirm.Text.ToString())
                {
                    MessageBox.Show(this.Page, "Passwords do not match!");

                }
                else
                if ((tbUserFirstName.Text.ToString() == "") ||
                    (tbUserLastName.Text.ToString() == "") ||
                    (tbUserLoginName.Text == "") ||
                    (tbUserPassword.Text == "") ||
                    (tbUserEmailAddress.Text == ""))
                {
                    MessageBox.Show(this.Page, "Missing Data");
                }
                else
                {
                    String query = "uspadduser";
                    List<SqlParameter> sp = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName = "@pLogin",  Value=tbUserLoginName.Text.ToString()},
                        new SqlParameter() { ParameterName = "@pPassword", Value = tbUserPassword.ToString()},
                        new SqlParameter() { ParameterName = "@pFirstName", Value = tbUserFirstName.Text.ToString()},
                        new SqlParameter() { ParameterName = "@pLastName", Value = tbUserLastName.Text.ToString()},
                        new SqlParameter() { ParameterName = "@pEmailAddress", Value=tbUserEmailAddress.Text.ToString()},
                        new SqlParameter() { ParameterName = "@responseMessage", Value=DBNull.Value}
                    };

                    sqlTypeCommands sql = new sqlTypeCommands();
                    sql.CommadSQL(query, true, sp);
                }
            }
            else
            {

            }
        }

        protected void GetNewUserTickets()
        {
            //get DDL Tickets
            string query = "select ticketID, " + hfPersonID.Value.ToString() + " as personID, description from ticket where personid = " + ddlGetUser.SelectedValue.ToString();
            updateGridview(gvNewUserTickets, query);
        }


        protected void getNewUserInfo()
        {
            DataTable user = new DataTable();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            string query;

            //get user info
            query = "select * from v_users where personid = " + ddlGetUser.SelectedValue.ToString();
            user = sql.ReturnDatatable(query);
            if (user.Rows.Count > 0)
            {
                //get DDL For Department
                if (ddlNewUserDepartment.Items.Count == 0)
                {
                    query = "select departmentid, departmentname from department";
                    dt = sql.ReturnDatatable(query);
                    ddlNewUserDepartment.DataSource = dt;
                    ddlNewUserDepartment.DataValueField = "departmentID";
                    ddlNewUserDepartment.DataTextField = "departmentName";
                    ddlNewUserDepartment.DataBind();
                    ddlNewUserDepartment.Items.Insert(0, new ListItem("No Department", "-1"));
                }


                ddlNewUserDepartment.SelectedValue = user.Rows[0].ItemArray[7].ToString();
                //get DDL Tickets
                query = "select ticketID, " + hfLoginID.Value.ToString() + " as PersonID,  description from ticket where personid = " + ddlGetUser.SelectedValue.ToString();
                updateGridview(gvNewUserTickets, query);
                query = "select * from v_users where personID = " + ddlGetUser.SelectedValue.ToString();
                dt.Clear();
                dt = sql.ReturnDatatable(query);
                if (dt.Rows.Count > 0)
                {
                    tbNewUserFirstName.Text = dt.Rows[0].ItemArray[1].ToString();
                    tbNewUserLastName.Text = dt.Rows[0].ItemArray[2].ToString();
                    tbNewUserEmail.Text = dt.Rows[0].ItemArray[3].ToString();
                }
                query = "select * from ";
            }
        }

        protected void deleteUser(string uid)
        {
            string query = "delete from person where person_id = '" + uid.ToString() + "'";
            sqlTypeCommands sql = new sqlTypeCommands();
            string result;
            result = sql.CommadSQL(query, false, null);
            if (result.ToString() != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + result.ToString() + "');", true);
            }

        }
    
        protected void btnUserSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnUserCancel_Click(object sender, EventArgs e)
        {

        }

        protected void gvCompters_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ddlComputerDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlComputerPerson_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnComputerSave_Click(object sender, EventArgs e)
        {

        }

        protected void BtnComputerCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnDepartmentSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnDepartmentCancel_Click(object sender, EventArgs e)
        {

        }

        protected void gvMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnSaveMessage_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvMessages_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvSubCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {

        }

        protected void ddlNewDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            getNewUserDDL();
        } 

        protected void ddlGetUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            getNewUserInfo();
        }

        protected void gvNewUserTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvNewRecentActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvNewUserTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNewUserTickets.PageIndex = e.NewPageIndex;
            //GetNewUserTickets();
        }
    }
}