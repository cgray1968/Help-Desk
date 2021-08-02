using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Help_Desk
{

    public partial class ComputerControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getComputers();
                pnlComputerEdit.Visible = false;
                btnComputerSave.Visible = false;
            }
        }

        protected void getComputers()
        {
            string query;
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            query = "select * from v_computer";
            dt.Reset();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gvComputers.DataSource = null;
                gvComputers.DataBind();
                gvComputers.DataSource = dt;
                gvComputers.DataBind();
            }
        }

        protected void getDepartments()
        {
            string query;
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            query = "select departmentID, departmentName from department order by DepartmentName";
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlDepartments.DataSource = dt;
                ddlDepartments.DataValueField = "departmentID";
                ddlDepartments.DataTextField = "departmentName";
                ddlDepartments.DataBind();

            }
        }

        protected void deleteComputer()
        {
            string query;
            sqlTypeCommands sql = new sqlTypeCommands();
            query = "delete from computer where computerid=" + hfComputerID.Value.ToString();
            string results = sql.CommandSQL(query, false, null);
            if (results.ToString() != "")
            {
                MessageBox.show(this.Page, results);
            }
            getComputers();
        }

        protected void editComputer()
        {
            string query;
            sqlTypeCommands sql = new sqlTypeCommands();
            if (ddlDepartments.Items.Count == 0)
            {
                getDepartments();
            }
            updateComputerUsers();
            pnlComputerUsers.Visible = true;

            pnlComputers.Visible = false;
            pnlComputerEdit.Visible = true;
            btnComputerSave.Visible = false;
            query = "select computerName, coalesce(DepartmentID, '1') from Computer where computerid=" + hfComputerID.Value.ToString();

            DataTable dt = new DataTable();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                tbComputerName.Text = dt.Rows[0].ItemArray[0].ToString();
                ddlDepartments.SelectedValue = dt.Rows[0].ItemArray[1].ToString();
            }
        }

        protected void gvComputers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvComputers.Rows[index];
            hfComputerID.Value = selectedRow.Cells[2].Text.ToString();

            if (e.CommandName == "ComputerEdit")
            {
                if (ddlDepartments.Items.Count == 0)
                    getDepartments();
                editComputer();

            }
            if (e.CommandName == "ComputerDelete")
            {
                deleteComputer();
            }

        }

        protected void btnAddNewComputer_Click(object sender, ImageClickEventArgs e)
        {
            hfComputerID.Value = "-1";
            pnlComputers.Visible = false;
            pnlComputerEdit.Visible = true;
            tbComputerName.Text = "";
            if (ddlDepartments.Items.Count == 0)
                getDepartments();
            btnComputerSave.Visible = true;
        }

        protected void btnComputerSave_Click(object sender, EventArgs e)
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            if ((hfComputerID.Value == "-1") || ((tbComputerName.Text != "") || (ddlDepartments.SelectedValue != "-1")))
            {
                string query = "sp_insert_update_computer";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerid", Value=hfComputerID.Value.ToString()},
                    new SqlParameter () { ParameterName="@departmentID", Value=ddlDepartments.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName= "@ComputerName", Value = tbComputerName.Text.ToUpper()},
                    new SqlParameter() { ParameterName = "@type", Value = "InsertComputer"}
                };
                string result = sql.CommandSQL(query, true, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
                else
                {
                    closeEditForm();
                }
            }
        }

        protected void BtnComputerCancel_Click(object sender, EventArgs e)
        {
            closeEditForm();
        }

        protected void ddlDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            if (hfComputerID.Value != "-1")
            {
                string query = "sp_insert_update_computer";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerid", Value=hfComputerID.Value.ToString()},
                    new SqlParameter() { ParameterName="@departmentID", Value=ddlDepartments.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName= "@ComputerName", Value = tbComputerName.Text.ToUpper()},
                    new SqlParameter() { ParameterName = "@type", Value = "UpdateDepartment"}
                };
                string result = sql.CommandSQL(query, true, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
                closeEditForm();

            }
        }

        protected void tbComputerName_TextChanged(object sender, EventArgs e)
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            if (hfComputerID.Value != "-1")
            {
                string query = "sp_insert_update_computer";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerid", Value=hfComputerID.Value.ToString()},
                    new SqlParameter() { ParameterName="@ComputerName", Value=tbComputerName.Text.ToString()},
                    new SqlParameter() { ParameterName= "@ComputerName", Value = tbComputerName.Text.ToUpper()},
                    new SqlParameter() { ParameterName = "@type", Value = "UpdateComputer"}
                };
                string result = sql.CommandSQL(query, true, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
                closeEditForm();
            }

        }

        protected void closeEditForm()
        {
            getComputers();
            pnlComputerEdit.Visible = false;
            pnlComputerUsers.Visible = false;
            gvComputersUsers.DataSource = null;
            gvComputersUsers.DataBind();
            pnlComputers.Visible = true;
            tbComputerName.Text = "";
            ddlDepartments.SelectedIndex = 0;
            
        }

        protected void deleteComputerUser()
        {
            string query = "sp_delete_user_computer" + hfComputerUserID.Value.ToString();
            sqlTypeCommands sql = new sqlTypeCommands();
            string result = sql.CommandSQL(query, false, null);
            if (result != "")
            {
                MessageBox.show(this.Page, result);
            }
        }

        protected void updateComputerUsers()
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            string query = "select computeruserid, cu.personID, firstname + ' ' + lastname as PersonName,  computerID from computeruser cu join person p on cu.personid=p.personid where computerid = " + hfComputerID.Value.ToString();
            DataTable dtComputerUsers = new DataTable();
            dtComputerUsers = sql.ReturnDatatable(query);
            if (dtComputerUsers.Rows.Count > 0)
            {
                gvComputersUsers.DataSource = dtComputerUsers;
                gvComputersUsers.DataBind();
            }
        }

        protected void gvComputersUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvComputersUsers.Rows[index];
            hfComputerUserID.Value = selectedRow.Cells[1].Text.ToString();
            if (e.CommandName == "DeleteUser")
            {
                deleteComputerUser();
                updateComputerUsers();
            }
        }
    }
}