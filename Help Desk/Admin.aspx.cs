using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace Help_Desk
{
    public partial class Admin : System.Web.UI.Page
    {
        public int LoginID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            hfLoginID.Value = Request.QueryString["uid"];
            LoginID = Convert.ToInt16(hfLoginID.Value.ToString());

            if (!IsPostBack)
            {
                HideAllPanels();
            }
        }

        protected void HideAllPanels()
        {

            pnlCategory.Visible = false;
            pnlDepartment.Visible = false;
            pnlComputers.Visible = false;
            pnlMessages.Visible = false;
            pnlUser.Visible = false;
            pnlSystemInfo.Visible = false;
            pnlSettings.Visible = false;
            pnlReport.Visible = false;

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
                    pnlUser.Visible = true;
                    break;
                case "System Logs":
                    HideAllPanels();
                    pnlMessages.Visible = true;
                    break;
                case "Messages":
                    HideAllPanels();
                    pnlMessages.Visible = true;
                    break;
                case "Department":
                    HideAllPanels();
                    pnlDepartment.Visible = true;
                    break;
                case "Computers":
                    HideAllPanels();
                    pnlComputers.Visible = true;
                    break;
                case "Categories":
                    HideAllPanels();
                    pnlCategory.Visible = true;
                    break;
                case "Settings":
                    HideAllPanels();
                    pnlSettings.Visible = true;
                    break;
                case "Reports":
                    HideAllPanels();
                    pnlReport.Visible = true;
                    break;
            }
        }

        public void setMessagelbl(string message)
        {
            lblMessage.Text = message.ToString();
            if (message != "")
                lblMessage.Visible = true;
            else
                lblMessage.Visible = false;
        }

    }
}