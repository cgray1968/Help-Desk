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
    public partial class Settings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getSettings();
        }

        protected void getSettings()
        {
            DataTable dt = new DataTable();
            string query = "select SettingsValue from settings where SettingsName='TechPassword'";
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                tbCurrentPassword.Text = dt.Rows[0].ItemArray[0].ToString();
            }
            else
                tbCurrentPassword.Text = "";
        }

        protected void savePassword()
        {
            string query;
            query = "sp_update_Settings";
            string SettingsName = "TechPassword";
            sqlTypeCommands sql = new sqlTypeCommands();
            List<SqlParameter> sp = new List<SqlParameter>
            {
                new SqlParameter() { ParameterName = "@SettingsName", Value = SettingsName },
                new SqlParameter() { ParameterName = "@settingsValue", Value=tbNewPassword.Text.ToString()}
            };
            string result = sql.CommandSQL(query, true, sp);
            if (result != "")
                MessageBox.show(this.Page, result);
            tbNewPassword.Text = "";
            getSettings();
        }

        protected void btnSavePassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbNewPassword.Text.ToString()))
            {
                if (tbNewPassword.Text != tbCurrentPassword.Text)
                {
                    savePassword();
                }

            }
        }
    }
}