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

    public partial class Computers : System.Web.UI.Page
    {
        static sqlTypeCommands sql = new sqlTypeCommands();
        private DataTable dt = new DataTable();
        private string query = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getComputers();
            }
        }

        protected void getComputers()
        {
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

        protected void btnAddNewComputer_Click(object sender, ImageClickEventArgs e)
        {
            hfComputerID.Value = "-1";
        }

        protected void editComputer()
        {

        }

        protected void deleteComputer()
        {

        }

        protected void getDepartments()
        {
            ddlComputerDepartment.DataSource = null;
            ddlComputerDepartment.DataBind();
            query = "select * from department order by DepartmentName";
            dt.Reset();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlComputerDepartment.DataSource = dt;
                ddlComputerDepartment.DataValueField = "DepartmentID";
                ddlComputerDepartment.DataTextField = "DepartmentName";
                ddlComputerDepartment.DataBind();
            }
        }
    
        protected void gvComputers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvComputers.Rows[index];
            hfComputerID.Value = selectedRow.Cells[2].Text.ToString();

            if (e.CommandName == "ComputerEdit")
            {
                if (ddlComputerDepartment.Items.Count == 0)
                    getDepartments();
                editComputer();
                //Here belongs where I add to open the jquery popups for computer edit.
            }
            if (e.CommandName == "ComputerDelete")
            {
                deleteComputer();
            }
        }

        protected void ddlComputerDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfComputerID.Value != "-1")
            {
                string query ="update Computer set DepartmentID = @departmentID where computerid = @computerid";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerid", Value=hfComputerID.Value.ToString()},
                    new SqlParameter () { ParameterName="@departmentID", Value=ddlComputerDepartment.SelectedValue.ToString()}
                };
                string result = sql.CommadSQL(query, false, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
            }
        }

        protected void btnComputerSave_Click(object sender, EventArgs e)
        {
            if ((hfComputerID.Value == "-1") || ((tbComputerName.Text != "") || (ddlComputerDepartment.SelectedValue != "-1")))
            {
                string query = "insert into computer (ComputerName, DepartmentID) values (@computerName, @departmentID)";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerName", Value=hfComputerID.Value.ToString()},
                    new SqlParameter () { ParameterName="@departmentID", Value=ddlComputerDepartment.SelectedValue.ToString()}
                };
                string result = sql.CommadSQL(query, false, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
            }
        }

        protected void BtnComputerCancel_Click(object sender, EventArgs e)
        {
            //close java
        }

        protected void tbComputerName_TextChanged(object sender, EventArgs e)
        {
            if(hfComputerID.Value != "-1")
            {
                string query = "update Computer set ComputerName = @ComputerName where computerid = @computerid";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName="@computerid", Value=hfComputerID.Value.ToString()},
                    new SqlParameter () { ParameterName="@departmentID", Value=ddlComputerDepartment.SelectedValue.ToString()}
                };
                string result = sql.CommadSQL(query, false, sp);
                if (result != "")
                    MessageBox.show(this.Page, result);
            }
        }
    }
    
}