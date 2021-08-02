using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Help_Desk
{
    public partial class Departments : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (gvDepartment.Rows.Count == 0)
                getDepartment();
        }

        protected void getDepartment()
        {
            string query = "select DepartmentID, DepartmentName from Department where DepartmentID != '1'";
            gvDepartment.DataSource = null;
            gvDepartment.DataBind();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gvDepartment.DataSource = dt;
                gvDepartment.DataBind();
            }
        }


        protected void saveDepartment()
        {
            if (hfDeparmentID.Value == "-1")
            {
                if (tbDepartmentName.Text != "")
                {
                    string query;
                    sqlTypeCommands sql = new sqlTypeCommands();
                    query = "sp_add_department";
                    List<SqlParameter> sp = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName = "@departmentName", Value = tbDepartmentName.Text.ToString()},
                        new SqlParameter() { ParameterName = "@departmentID", Value = hfDeparmentID.Value.ToString()}
                    };
                    string result = sql.CommandSQL(query, true, sp);
                    if ((result != "Success") || (string.IsNullOrEmpty(result)))
                        MessageBox.show(this.Page, result);
                    else
                    {
                        pnlDepartmentEdit.Visible = false;
                        pnlDepartment.Visible = true;
                        tbDepartmentName.Text = "";
                    }
                }
            }
        }

        protected void editDepartment(int index)
        {
            btnDepartmentSave.Visible = false;
            pnlDepartment.Visible = false;
            pnlDepartmentEdit.Visible = true;
            tbDepartmentName.Text = gvDepartment.Rows[index].Cells[3].Text.ToString();
          
        }

        protected void deleteDepartment()
        {
            string query;
            sqlTypeCommands sql = new sqlTypeCommands();
            query = "sp_delete_department";
            List<SqlParameter> sp = new List<SqlParameter>
            {
                new SqlParameter() { ParameterName = "@departmentID", Value = hfDeparmentID.Value.ToString()}
            };
            string result = sql.CommandSQL(query, true, sp);
            if (!string.IsNullOrEmpty(result))
                MessageBox.show(this.Page, result);
            getDepartment();
        }

        protected void btnDepartmentSave_Click(object sender, EventArgs e)
        {
            saveDepartment();
        }

        protected void btnDepartmentCancel_Click(object sender, EventArgs e)
        {
            pnlDepartmentEdit.Visible = false;
            tbDepartmentName.Text = "";
            pnlDepartment.Visible = true;
        }

        protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;
            GridViewRow selectedRow;
            index = Convert.ToInt32(e.CommandArgument);
            selectedRow = gvDepartment.Rows[index];
            int computerid = Convert.ToInt32(selectedRow.Cells[2].Text.ToString());
            hfDeparmentID.Value = computerid.ToString();

            switch (e.CommandName)
            {
                case "DepartmentDelete":
                    deleteDepartment();
                    break;
                case "DepartmentEdit":
                    editDepartment(index);
                    break;
            }
        }

        protected void tbDepartmentName_TextChanged(object sender, EventArgs e)
        {
            if ((hfDeparmentID.Value !="-1") || (tbDepartmentName.Text != ""))
            {
                
                string query = "sp_update_DepartmentName";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@departmentName", Value = tbDepartmentName.Text.ToString()},
                    new SqlParameter() { ParameterName = "@departmentID", Value =hfDeparmentID.Value.ToString()}
                };
                sqlTypeCommands sql = new sqlTypeCommands();
                string result = sql.CommandSQL(query, true, sp);
                if (result != "")           
                {
                    MessageBox.show(this.Page, result);
                }
            }

        }

        protected void ibAddDepartment_Click(object sender, ImageClickEventArgs e)
        {
            pnlDepartmentEdit.Visible = true;
            hfDeparmentID.Value = "-1";
            pnlDepartment.Visible = false;
            tbDepartmentName.Text = "";
            btnDepartmentSave.Visible = true;
        }
    }
}