using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Help_Desk
{
    public partial class CategoryControl : System.Web.UI.UserControl
    {
        private string query;
        sqlTypeCommands sql = new sqlTypeCommands();
        private string spType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ddlCategory.Items.Count == 0)
                getCategory();
        }

        protected void getCategory()
        {
            tbCategory.Text = "";
            DataTable dt = new DataTable();
            query = "select categoryID, categoryName from category";
            ddlSubCategory.Items.Clear();
            ddlSelection.Items.Clear();
            ddlCategory.Items.Clear();
            tbSubCategory.Text = "";
            tbSelection.Text = "";

            dt = sql.ReturnDatatable(query);

            if (dt.Rows.Count > 0)
            {
                ddlCategory.DataSource = dt;
                ddlCategory.DataValueField = "categoryID";
                ddlCategory.DataTextField = "categoryName";
                ddlCategory.DataBind();
            }
            ddlCategory.Items.Insert(0, new ListItem("New Entry", "-1"));
            ddlCategory.SelectedIndex = 0;
            tbCategory.Text = "";
        }


        protected void getSubCategory()
        {
            ddlSubCategory.Items.Clear();
            ddlSelection.Items.Clear();
            tbSelection.Text = "";
            tbSubCategory.Text = "";
            if (ddlCategory.SelectedValue != "-1")
            {
                  DataTable dt = new DataTable();
                query = "select subcategoryID, SubCategoryName from subcategory where categoryID = " + ddlCategory.SelectedValue.ToString();
                ddlSubCategory.DataSource = null;
                ddlSubCategory.DataBind();
                dt = sql.ReturnDatatable(query);
                if (dt.Rows.Count > 0)
                {
                    ddlSubCategory.DataSource = dt;
                    ddlSubCategory.DataValueField = "subcategoryID";
                    ddlSubCategory.DataTextField = "SubCategoryName";
                    ddlSubCategory.DataBind();

                }
                ddlSubCategory.Items.Insert(0, new ListItem("New Entry", "-1"));
                ddlSubCategory.SelectedIndex = 0;
              }
        }

        protected void getSelection()
        {
            ddlSelection.Items.Clear();
            ddlSelection.DataSource = null;
            ddlSelection.DataBind();
            tbSelection.Text = "";
            if (ddlSubCategory.SelectedValue != "-1")
            {

                DataTable dt = new DataTable();
                query = "select selectionid, selectionName from selection where subcategoryid = " + ddlSubCategory.SelectedValue.ToString();
                dt = sql.ReturnDatatable(query);
                if(dt.Rows.Count > 0)
                {
                    ddlSelection.DataSource = dt;
                    ddlSelection.DataValueField = "SelectionID";
                    ddlSelection.DataTextField = "selectionName";
                    ddlSelection.DataBind();

                }
                ddlSelection.Items.Insert(0, new ListItem("New Entry", "-1"));
                ddlSelection.SelectedIndex = 0;

            }
        }


        protected void tbCategory_TextChanged(object sender, EventArgs e)
        {
            tbCategory.Text = tbCategory.Text.ToUpper();
            string category;
            if (!string.IsNullOrEmpty(tbCategory.Text))
            {
                category = tbCategory.Text.ToUpper().TrimEnd();
                if (ddlCategory.Items.FindByText(category) != null)
                    return;
                query = "sp_add_update_category";
                if (ddlCategory.SelectedValue == "-1")
                    spType = "NewCategory";
                else
                    spType = "UpdateCategory;";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@pName", Value = category},
                    new SqlParameter() { ParameterName = "@pID", Value = ddlCategory.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@pType", Value = spType}
                };
                string result = sql.CommandSQL(query, true, sp);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.show(this.Page,result);
                }
                getCategory();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCategory.Text = "";
            tbSubCategory.Text = "";
            ddlSubCategory.Items.Clear();
            ddlSubCategory.DataSource = null;
            ddlSubCategory.DataBind();
            tbSelection.Text = "";
            ddlSelection.Items.Clear();
            ddlSelection.DataSource = null;
            ddlSelection.DataBind();
            if (ddlCategory.SelectedValue != "-1")
            {
                tbCategory.Text = ddlCategory.SelectedItem.Text.ToString();
                getSubCategory();
            }
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSubCategory.Text = "";
            tbSelection.Text = "";
            ddlSelection.Items.Clear();
            ddlSelection.DataSource = null;
            ddlSelection.DataBind();
            if(ddlSubCategory.SelectedValue !="-1")
            {
                tbSubCategory.Text = ddlSubCategory.SelectedItem.Text.ToString();
                getSelection();
                taEmail.Value = "";
            }
        }

        protected void tbSubCategory_TextChanged(object sender, EventArgs e)
        {
            tbSubCategory.Text = tbSubCategory.Text.ToUpper();
            string subCategory;
            if (!string.IsNullOrEmpty(tbSubCategory.Text))
            {
                subCategory = tbCategory.Text.ToString().ToUpper();
                if (ddlSubCategory.Items.FindByText(subCategory) != null)
                    return;
                if (ddlSubCategory.SelectedValue == "-1")
                    spType = "NewSubCategory";
                else
                    spType = "UpdateSubCategory;";

                query = "sp_add_update_Subcategory";
                  List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@pName", Value = tbSubCategory.Text.ToString()},
                    new SqlParameter() { ParameterName = "@pCategoryID", Value = ddlCategory.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@pID", Value = ddlSubCategory.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@pType", Value = spType}
                };
                string result = sql.CommandSQL(query, true, sp);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.show(this.Page, result);
                }
                tbSubCategory.Text = "";
                getSubCategory();
                taEmail.Value = "";
            }
        }
 


        protected void getEmailBody()
        {
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            string query = "select emailbody from selection where selectionid = " + ddlSelection.SelectedValue.ToString();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                taEmail.Value = dt.Rows[0].ItemArray[0].ToString();
                

            }
        }

        protected void ddlSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            tbSelection.Text = "";
            if (ddlSelection.SelectedValue != "-1")
            {
                tbSelection.Text = ddlSelection.SelectedItem.Text.ToString();
                getEmailBody();
            }
        }

        protected void tbSelection_TextChanged(object sender, EventArgs e)
        {
            query = "sp_add_update_Selection";
            string selection;
            if (!string.IsNullOrEmpty(tbSelection.Text))
            {
                selection = tbSelection.Text.ToUpper().TrimEnd();
                if (ddlSelection.Items.FindByText(selection) != null)
                    return;
                if (ddlSelection.SelectedValue == "-1")
                {
                    spType = "NewSelection";
                }
                else
                    spType = "UpdateSelection";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@pName", Value = tbSelection.Text.ToString()},
                    new SqlParameter() { ParameterName = "@pSubCategoryID", Value = ddlSubCategory.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@pID", Value = ddlSelection.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@pType", Value = spType}
                };


                string result = sql.CommandSQL(query, true, sp);
                getSelection();
            }
        }

        protected void btnDeleteCategory_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCategory.SelectedValue != "-1")
            {
                query = "delete from category where categoryid = " + ddlCategory.SelectedValue.ToString();
                string result = sql.CommandSQL(query, false, null);
                if (!string.IsNullOrEmpty(result))
                    MessageBox.show(this.Page, result);
                getCategory();
            }
        }

        protected void btnDeleteSubCategory_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCategory.SelectedValue != "-1")
            {
                query = "delete from subcategory where subcategoryid = " + ddlSubCategory.SelectedValue.ToString();
                string result = sql.CommandSQL(query, false, null);
                if (!string.IsNullOrEmpty(result))
                    MessageBox.show(this.Page, result);
                getCategory();
            }
        }

        protected void btnDeleteSelection_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCategory.SelectedValue != "-1")
            {
                query = "delete from selection where selectionid = " + ddlSelection.SelectedValue.ToString();
                string result = sql.CommandSQL(query, false, null);
                if (!string.IsNullOrEmpty(result))
                    MessageBox.show(this.Page, result);
                getCategory();
            }
        }

        protected void tbDescription_TextChanged(object sender, EventArgs e)
        {

        }
      protected void btnSaveEmailBody_Click(object sender, EventArgs e)
        {

            string query = "update selection set emailbody = '" + taEmail.Value.ToString() + "' where  SelectionID = " + ddlSelection.SelectedValue.ToString();
            sqlTypeCommands sql = new sqlTypeCommands();
            sql.CommandSQL(query, false, null);

        }
    }
}