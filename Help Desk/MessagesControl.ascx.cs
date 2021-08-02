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
    public partial class MessagesControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                getMessages();
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

        protected void getMessages()
        {
            string query = "Select * from V_Systeminfo order by active desc, datecreated";
            updateGridview(gvMessages, query);
            hfMessageID.Value = "-1";
            tbNewMessage.Text = "";
            cbNewMessage.Checked = false;

        }

        protected void gvMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvMessages.Rows[index];
            hfMessageID.Value = gvMessages.Rows[index].Cells[2].Text.ToString();

            if (e.CommandName == "MessageEdit")
            {
                editMessage(index);

            }
            if (e.CommandName == "MessageDelete")
            {
                deleteMessage();
            }

        }
        
        protected void ChangeMessage(string procedure)
        {
            string query = "sp_save_delete_update_message";
            sqlTypeCommands sql = new sqlTypeCommands();
            string result;
            string LoginId;
            int ischecked = 0;
            if (cbNewMessage.Checked)
                ischecked = 1;
                   
            LoginId = Request.QueryString["uid"];
            List<SqlParameter> sp = new List<SqlParameter>
            {
                new SqlParameter() { ParameterName="@messageid", Value=hfMessageID.Value.ToString()},
                new SqlParameter() { ParameterName="@Message", Value=tbNewMessage.Text.ToString()},
                new SqlParameter() { ParameterName="@Active", Value=ischecked},
                new SqlParameter() { ParameterName="@pType", Value=procedure.ToString()},
                new SqlParameter() { ParameterName="@DateCreated", Value =  System.DateTime.Now.ToString()},
                new SqlParameter() { ParameterName="@createdby", Value=LoginId.ToString()}

            };
            result = sql.CommandSQL(query, true, sp);
            if (result != "")
            {
                MessageBox.show(this.Page, result);
            }

            getMessages();
        }


        protected void editMessage(int index)
        {
            int ActiveChecked = Convert.ToInt16(gvMessages.Rows[index].Cells[4].Text.ToString());
            tbNewMessage.Text = gvMessages.Rows[index].Cells[3].Text.ToString();

            if (ActiveChecked == 1)
                cbNewMessage.Checked = true;
            else
                cbNewMessage.Checked = false;
        }

        protected void deleteMessage()
        {
            ChangeMessage("Delete");
        }



        protected void btnSaveMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbNewMessage.Text))
            {
                if (hfMessageID.Value == "-1")
                {
                    ChangeMessage("Insert");
                }
                else
                {
                    ChangeMessage("Update");
                }
            }
        }

        protected void btnClearMessage_Click(object sender, EventArgs e)
        {
            tbNewMessage.Text = "";
            cbNewMessage.Checked = false ;
        }
    }
}