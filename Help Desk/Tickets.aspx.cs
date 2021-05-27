using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data;
using System.Web.Security;

namespace Help_Desk
{
    public partial class Tickets : System.Web.UI.Page
    {
        public bool _isTech;
        public bool _isAdmin = false;
        public bool _statusChange = false;
        public bool _newTicket = false;
        public string _userID;
        public string _userName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            _userName = this.Page.User.Identity.Name.ToString();
            getUserID();
            sqlTypeCommands sql = new sqlTypeCommands();
            _isTech = sql.getIsTech(_userID);

            if (!IsPostBack)
            {
                getMessage();
                getStatus();
                if (_isTech)
                {
                    menuTicket.Items[4].Selectable = true;
                    menuTicket.Items[2].Selectable = true;
                    _isTech = true;
                    setTech();
                }

                getTicketIDs();
                getTickets();
            }
        }

        public DropDownList updateDropDownList(string value, string text, string query)
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


        protected void getTicketIDs()
        {
            DataTable dt = new DataTable();
            DropDownList ddl1 = new DropDownList();
            ddlTickets.DataSource = null;
            ddlTickets.DataBind();
            sqlTypeCommands sql = new sqlTypeCommands();
            String query = "Select TicketID from v_tickets";
           
            if (!_isTech)
                query += " where personid = " + _userID;
            query += " order by ticketid";
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlTickets.DataSource = "";
                ddlTickets.DataBind();
                ddlTickets.DataSource = dt;
                ddlTickets.DataTextField = "TicketID";
                ddlTickets.DataValueField = "TicketID";
                ddlTickets.DataBind();
            }

            ddlTickets = updateDropDownList("ticketID", "TicketID", query);
            ddlTickets.Items.Insert(0, new ListItem("Select Ticket", "-1"));
            ddlTickets.SelectedIndex = 0;

        }

        protected void getUserID()
        {
            string query = "select personid from login where loginName = '" + _userName + "'";
            DataTable dt1 = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt1 = sql.ReturnDatatable(query);
            if (dt1.Rows.Count > 0)
            {
                _userID = dt1.Rows[0][0].ToString();
            }
            else
                FormsAuthentication.RedirectToLoginPage();

        }

        protected void setTech()
        {
            menuTicket.Items[1].Enabled = true;
        }

        protected void getMessage()
        {
            string query = "Select top 1 information, datecreated from systeminfo where active = 1 order by datecreated desc";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Firebrick;

                lblMessage.Text = dt.Rows[0][1].ToString() + ' ' + dt.Rows[0][0].ToString();
            }
            else
            {
                lblMessage.Visible = false;
                lblMessage.Text = "";
            }
        }

        protected void getStatus()
        {
            _statusChange = true;
            DataTable dt = new DataTable();
            string query = "select statusID, StatusName from status";
            sqlTypeCommands cmd = new sqlTypeCommands();
            dt = cmd.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlStatus.DataSource = dt;
                ddlStatus.DataValueField = "statusID";
                ddlStatus.DataTextField = "StatusName";
                ddlStatus.SelectedValue = "0";
                ddlStatus.DataBind();
            }
            _statusChange = false;
        }

        protected void getTickets()
        {
            DataTable dt = new DataTable();
            gvTickets.DataSource = null;
            gvTickets.DataBind();
            sqlTypeCommands sql = new sqlTypeCommands();
            String query = "Select * from v_tickets ";
            string statusid = ddlStatus.SelectedValue.ToString();
            ;
            if (_isTech)
            {

                if (menuTicket.Items[2].Text == "Assigned Tickets")
                {
                    if (statusid == "6")
                        query += " where assignedID = " + _userID;
                    else
                        query += " where assignedID = " + _userID +  "  and statusid = " + statusid;
                }
                else
                    if (statusid != "6")
                            query += " where statusid = " + statusid;
            }
            else
                if (statusid == "6")
                query += " where personid = " + _userID;
            else
                query += " where personid = " + _userID + " and statusid = " + statusid;
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gvTickets.DataSource = dt;
                gvTickets.DataBind();
               
            }
        }


        protected void NewTicket()
        {
            ThisTicket ticket = new ThisTicket();
            ticket.Session["TicketID"] = "-1";
            Server.Transfer("ThisTicket.aspx");
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_statusChange)
            {
                getTickets();
            }

        }

 
        protected void AssignedTickets()
        {
            if (_isTech)
            {

                if (menuTicket.Items[2].Text == "Assigned Tickets")

                    menuTicket.Items[2].Text = "All Tickets";
                else
                {
                    menuTicket.Items[2].Text = "Assigned Tickets";
                }
                getTickets();
            }
        }

        protected void Logout()
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void bnImage_Click(object sender, EventArgs e)
        {

        }

        protected void Admin()
        {
            Server.Transfer("admin.aspx?uid=" + _userID.ToString());
        }

        protected void menuTicket_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (menuTicket.SelectedItem.Text)
            {
                case "New Ticket":
                    NewTicket();
                    break;
                case "Admin":
                    Admin();
                    break;
                case "Assigned Tickets":
                    AssignedTickets();
                    break;
                case "All Tickets":
                    AssignedTickets();
                    break;
                case "Logoff":
                    Logout();
                    break;
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }


        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editTicket")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gvTickets.Rows[index];
                int ticketID = Convert.ToInt32(selectedRow.Cells[1].Text.ToString());
                editticket(ticketID);
            }
        }

        protected void editticket(int TicketID)
        {
            ThisTicket ticket = new ThisTicket();
            ticket.Session["TicketID"] = TicketID.ToString();
            Server.Transfer("ThisTicket.aspx");

        }


        protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ddlTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTickets.SelectedValue != "-1")
            {
                int ticketid = Convert.ToInt32(ddlTickets.SelectedValue.ToString());
                editticket(ticketid);

            }
        }
    }
}