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
        public bool _isTech = false;
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
            _isAdmin = sql.getIsAdmin(_userID);
           

            if (!IsPostBack)
            {
                getMessage();
                getStatus();
               
                if ((_isAdmin) || (_isTech))
                {
                    setTech();
                }    
                else 
                {
                    
                    menuTicket.Items.Remove(menuTicket.FindItem("AssignTicket"));
                    menuTicket.Items.Remove(menuTicket.FindItem("|2"));
                    menuTicket.Items.Remove(menuTicket.FindItem("Admin"));
                    menuTicket.Items.Remove(menuTicket.FindItem("|3"));
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
           
            if ((!_isTech) || (!_isAdmin))
                query += " where personid = " + _userID + " or createdbyid = " + _userID;
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

            ddlTickets.Items.Insert(0, new ListItem("Select Ticket", "-1"));
            ddlTickets.SelectedIndex = 0;

        }

        protected void getUserID()
        {
            string query = "select personid from person where loginName = '" + _userName + "'";
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
            if ((_isTech) || (_isAdmin))
            {
                if (menuTicket.Items[2].Text == "Assigned Tickets")
                {
                    if (statusid != "6")
                        query += " where assignedID = " + _userID + "  and statusid = " + statusid;
                    else
                        query += " where assignedID = " + _userID ;
                }
                else
                    if (statusid != "6")
                            query += " where statusid = " + statusid;
            }
            else
                query += " where personid = " + _userID + " or CreatedbyID = " + _userID;
            if (statusid != "6")
                query += " and statusid = " + statusid;
            query += " order by statusid, lastupdate";
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gvTickets.DataSource = dt;
                gvTickets.DataBind();
            }

            if ((_isTech) || (_isAdmin))
            {
                string result;
                string message = "<p><span style = 'background: red;' > Red - Ticket is older than 1 week.<br/></span><span style='background: Orange;'>Orange - Ticket is older than 3 days.<br/></span><span style='color:Black;'>Black - Normal ticket.</span></p>";
                lblAdminNotes.Text= message;
                lblAdminNotes.Visible = true;
                DateTime oneWeek = System.DateTime.Today.AddDays(-7).Date;
                DateTime threeDays = System.DateTime.Today.AddDays(-3).Date;
                foreach (GridViewRow row in gvTickets.Rows)
                {
                    int status = Convert.ToInt32(row.Cells[11].Text.ToString());
                    if (status < 3)
                    {
                        result = getDate(row.Cells[1].Text.ToString(), row.Cells[11].Text.ToString());
                        DateTime ticketDate = Convert.ToDateTime(result).Date;

                        if (ticketDate.Date <= System.DateTime.Today.AddDays(-6))
                        {
                            row.BackColor = System.Drawing.Color.Red;
                            row.Cells[11].ForeColor = System.Drawing.Color.Red;
                        }
                        else if (ticketDate.Date <= System.DateTime.Today.AddDays(-3))
                        {
                            row.BackColor = System.Drawing.Color.Orange;
                            row.Cells[11].ForeColor = System.Drawing.Color.Orange;
                        }
                    }
                }
            }
        }

        protected string getDate(string ticketid, string statusid)
        {
            string result = "";
            //select ticketdate from TicketDate where ticketid=43 and ticketstatusid = 1
            string query = "select ticketdate from ticketdate where ticketid = " + ticketid + " and ticketstatusid = " + statusid;
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0].ItemArray[0].ToString();
            }    
            return result;
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
            if ((_isTech) || (_isAdmin))
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