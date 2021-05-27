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

    public static class MessageBox
    {
        public static void show(this Page Page, String Message)
        {
            Page.ClientScript.RegisterStartupScript(
               Page.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }
    }

    public class General
    {

        public DropDownList updateDropDownList (string value, string text, string query)
        {
            DropDownList ddl = new DropDownList();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddl.DataSource = dt;
                ddl.DataValueField = value;
                ddl.DataTextField = text;
                ddl.DataBind();
            }
            return ddl;
        }
    }

}