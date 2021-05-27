using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;
using System.Net;

namespace Help_Desk
{
    public partial class ThisTicket : System.Web.UI.Page
    {
        public bool _statusChange;
        public bool _isDDLChanging;
        public string notes;
        public string sidenotes;
        public Boolean newnote = false;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();

            if (!IsPostBack)
            {

                if ((Session["userid"] == null))
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    lblticketClosed.Value = "false";
                    lblisTech.Value = "false";
                    lblisAdmin.Value = "false";
                    _statusChange = false;
                    lblNewTicket.Value = "true";
                    if (string.IsNullOrEmpty(Request.QueryString["ReturnTo"]))
                        hfReturn.Value = "";
                    else
                        hfReturn.Value = Request.QueryString["ReturnTo"].ToString();

                    if (string.IsNullOrEmpty(Request.QueryString["userid"]))
                        lbluserID.Value = Session["userid"].ToString();
                    else
                        lbluserID.Value = Request.QueryString["userid"];

                    if (string.IsNullOrEmpty(Request.QueryString["TID"]))
                        lblMyTicketID.Value = Session["ticketID"].ToString();
                    else
                        lblMyTicketID.Value = Request.QueryString["TID"].ToString();
                    
                    string _ticketid = lblMyTicketID.Value.ToString();
                    if ((_ticketid == null) || (_ticketid == ""))
                        lblMyTicketID.Value = Session["ticketID"].ToString();
                    else
                        lblMyTicketID.Value = _ticketid;
                    sqlTypeCommands sql = new sqlTypeCommands();
                    lblisTech.Value = sql.getIsTech(lbluserID.Value) ? "true" : "false";
                    if (lblisTech.Value.ToString() == "false")
                        ddlTechs.Enabled = false;
                    else
                        ddlTechs.Enabled = true;
                    getUserName();
                    getComputerName();
                    getDDLCategory();
                    getDDLTech();
                    getTicketDDLStatus();

                    if (lblMyTicketID.Value != "-1")
                    {
                        getTicket();
                    }
                    
                    else
                    {
                        tbTicketID.Text = "-1";
                        tbDateCreated.Text = System.DateTime.Now.ToString();
                        hfcreatedid.Value = lbluserID.Value.ToString();
                        tbCreatedby.Text = lbluserName.Value.ToString();
                        getDDLComputer();
                        ddlComputers.SelectedValue = lblComputerID.Value.ToString();
                        getDDLCategory();
                        getTicketDDLStatus();
                        getTicketPerson();
                        ddlTicketPersons.SelectedValue = lbluserID.Value.ToString();
                    }
                }
            }
        }

        protected void getComputerName()
        {
            string ComputerName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
            string[] a = ComputerName.ToString().Split('.');
            ComputerName = a[0];
            DataTable dt = new DataTable();
            string query = "Select computerid from computer where computername like '" + ComputerName.ToString() + "'";
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count == 0)
            {
                saveComputer(ComputerName);
                getComputerName();
            }
            else
               lblComputerID.Value = dt.Rows[0].ItemArray[0].ToString();
            

        }

        protected void saveComputer(string computerName)
        {
            string query = "select departmentid from DepartmentUser where personid = " + lbluserID.Value.ToString();
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
                query = "insert computer (computername, departmentid) values ('" + computerName.ToString() + "' , '" + dt.Rows[0].ItemArray[0].ToString() + "')";
             else
               query = "insert computer (computername) values ('" + computerName.ToString() + "')";
            string success = sql.CommadSQL(query, false, null);

            //Save computer to user
            //get computerid
            query = "select computerid from computer where computername = '" + computerName.ToString() + "'";
            dt.Reset();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                query = "insert into computeruser (computerid, personid) values('" + dt.Rows[0].ItemArray[0].ToString() + "', '" + lbluserID.Value.ToString() + "')";
                string Success = sql.CommadSQL(query, false, null);
            }
            
        }

        protected void getUserName()
        {
            string query = "select firstname + ' ' + lastName from person where personid=" + lbluserID.Value.ToString();
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                lbluserName.Value = dt.Rows[0].ItemArray[0].ToString();
            }
        }

        protected void getTicket()
        {
            lblNewTicket.Value = "false";
            DataTable dt = new DataTable();
            String query = "select	t.ticketid,	t.createdbyID, t.createdby,	t.personid,	t.DateCreated, t.description, t.statusid, t.status, t.categoryid, t.subcategoryid, t.selectionid, t.category, t.subcategory, t.selection, t.assignedto, t.assignedid, t.computerid from v_Tickets	t where ticketID = " + lblMyTicketID.Value.ToString();

            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                _isDDLChanging = true;
                getDDLCategory();
                ddlCategory.SelectedValue = dt.Rows[0].ItemArray[8].ToString();

                if (ddlCategory.SelectedValue != "-1")
                {
                    getddlSubCategory();
                    ddlSubCategory.SelectedValue = dt.Rows[0].ItemArray[9].ToString();
                }
                if (ddlSubCategory.SelectedValue != "-1")
                {

                    if (ddlSubCategory.SelectedValue != "")
                    {
                        getddlSelection();
                        ddlSelection.SelectedValue = dt.Rows[0].ItemArray[10].ToString();
                    }
                }
                if (ddlComputers.Items.Count == 0)
                    getDDLComputer();
                ddlComputers.SelectedValue = dt.Rows[0].ItemArray[16].ToString();

                if (ddlTicketPersons.Items.Count == 0)
                    getTicketPerson();
                ddlTicketPersons.SelectedValue = dt.Rows[0].ItemArray[1].ToString();

                if (ddlTicketStatus.Items.Count == 0)
                    getTicketDDLStatus();
                ddlTicketStatus.SelectedValue = dt.Rows[0].ItemArray[6].ToString();

                if (ddlTechs.Items.Count == 0)
                    getDDLTech();
                ddlTechs.SelectedValue = dt.Rows[0].ItemArray[15].ToString();
                
                tbDateCreated.Text = dt.Rows[0].ItemArray[4].ToString();
                tbTicketID.Text = lblMyTicketID.Value.ToString();
                hfcreatedid.Value = dt.Rows[0].ItemArray[1].ToString();
                tbCreatedby.Text = dt.Rows[0].ItemArray[2].ToString();
                tbNotes.Text = dt.Rows[0].ItemArray[5].ToString();
                tbNotesHidden.Text = dt.Rows[0].ItemArray[5].ToString();
                lblNewTicket.Value = "false";
                _isDDLChanging = false;
                if ((ddlTicketStatus.SelectedValue == "4") || (ddlTicketStatus.SelectedValue == "5"))
                    lblticketClosed.Value = "true";

                getTicketNotes();

            }
        }

        protected void getTicketNotes()
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            String query = "select datecreated + ' ' + createdby + ' - ' + notes as info from v_ticketnotes where ticketid = " + lblMyTicketID.Value.ToString();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                gvNotes.DataSource = dt;
                gvNotes.DataBind();
            }
                
        }

        protected void getDDLTech()
        {
            string query;
            query = "select * from v_techs order by Person_id";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlTechs.DataSource = null;
                ddlTechs.DataBind();
                ddlTechs.DataSource = dt;
                ddlTechs.DataTextField = "Tech";
                ddlTechs.DataValueField = "Person_ID";
                ddlTechs.DataBind();
                ddlTechs.SelectedIndex = 0;
            }
            else
            {
                ddlTechs.Items.Insert(0, new ListItem("System Attendant", "0"));
                ddlTechs.SelectedIndex = 0;
            }

        }


        protected void getddlSelection()
        {
            if (ddlSubCategory.SelectedValue != "-1")
                {
                if (ddlCategory.SelectedValue != "")
                {
                    string query = "select selectionName, selectionID from selection where subcategoryId = " + ddlSubCategory.SelectedValue.ToString();
                    DataTable dt = new DataTable();
                    sqlTypeCommands sql = new sqlTypeCommands();
                    dt = sql.ReturnDatatable(query);
                    if (dt.Rows.Count > 0)
                    {
                        ddlSelection.DataSource = null;
                        ddlSelection.DataBind();
                        ddlSelection.DataSource = dt;
                        ddlSelection.DataTextField = "selectionName";
                        ddlSelection.DataValueField = "selectionID";
                        ddlSelection.DataBind();
                        ddlSelection.Items.Insert(0, new ListItem("New SubCategory", "-1"));
                        ddlSelection.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlSelection.Items.Insert(0, new ListItem("New SubCategory", "-1"));
                        ddlSelection.SelectedIndex = 0;

                    }
                }
            }
        }


        protected void getddlSubCategory()
        {
            if (ddlCategory.SelectedValue != "-1")
            {
                string query = "select subcategoryName, subcategoryID from subcategory where categoryId = " + ddlCategory.SelectedValue.ToString();
                DataTable dt = new DataTable();
                sqlTypeCommands sql = new sqlTypeCommands();
                dt = sql.ReturnDatatable(query);
                if (dt.Rows.Count > 0)
                {
                    ddlSubCategory.DataSource = null;
                    ddlSubCategory.DataBind();
                    ddlSubCategory.DataSource = dt;
                    ddlSubCategory.DataTextField = "subcategoryName";
                    ddlSubCategory.DataValueField = "subcategoryID";
                    ddlSubCategory.DataBind();
                    ddlSubCategory.Items.Insert(0, new ListItem("New SubCategory", "-1"));
                    ddlSubCategory.SelectedIndex = 0;
                }
                else
                {
                    ddlSubCategory.Items.Insert(0, new ListItem("New SubCategory", "-1"));
                    ddlSubCategory.SelectedIndex = 0;
                }
            }
        }



        protected void getDDLCategory()
        {
            if (ddlCategory.Items.Count == 0)
            {
                string query = "select categoryName, categoryID from category";
                DataTable dt = new DataTable();
                sqlTypeCommands sql = new sqlTypeCommands();
                dt = sql.ReturnDatatable(query);
                if (dt.Rows.Count > 0)
                {
                    ddlCategory.DataSource = null;
                    ddlCategory.DataBind();
                    ddlCategory.DataSource = dt;
                    ddlCategory.DataTextField = "categoryName";
                    ddlCategory.DataValueField = "categoryID";
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("New Category", "-1"));
                    ddlCategory.SelectedIndex = 0;
                }
                else
                {
                    ddlCategory.Items.Insert(0, new ListItem("New Category", "-1"));
                    ddlCategory.SelectedIndex = 0;
                }
            }
        }

        protected void getDDLComputer()
        {
            string query = "select computername, computerId from computer";
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlComputers.DataSource = null;
                ddlComputers.DataBind();
                ddlComputers.DataSource = dt;
                ddlComputers.DataTextField = "computername";
                ddlComputers.DataValueField = "computerID";
                ddlComputers.DataBind();
                ddlComputers.Items.Insert(0, new ListItem("New Computer", "-1"));
                ddlComputers.SelectedIndex = 0;

            }
            else
            {
                ddlComputers.Items.Insert(0, new ListItem("New Computer", "-1"));
                ddlComputers.SelectedIndex = 0;
            }

        }
    


        protected void getTicketPerson()
        {
            
            string _departmentID = "-1";
            string query;
            DataTable dtdID = new DataTable();
            DataSet ds = new DataSet();
            sqlTypeCommands sql = new sqlTypeCommands();
            if ((lblisTech.Value == "true") || (lblisAdmin.Value == "true"))
            {
                _departmentID = "-1";
            }
            else
            {

                query = "Select departmentID from departmentUser where personID = " + lbluserID.Value.ToString();
                dtdID = sql.ReturnDatatable(query);
                ds.Tables.Add(dtdID);
                try
                {
                    _departmentID = string.Join(", ", ds.Tables[0].Select().Select(r => r["departmentID"].ToString()));
                }
                catch (SystemException ex)
                {

                }
            }

            DataTable dt = new DataTable();
            if (_departmentID != "-1")
            {
                query = "select distinct p.PersonID, firstName + ' ' + lastName as PersonName from person p join departmentuser du on p.personid = du.personid where du.departmentid in (" + _departmentID + ")";
            }
            else
                query = "select personid, firstname + ' ' + lastname as personname from person";

            dt = sql.ReturnDatatable(query);

            if (dt.Rows.Count > 0)
            {
                ddlTicketPersons.DataSource = null;
                ddlTicketPersons.DataBind();
                ddlTicketPersons.DataSource = dt;
                ddlTicketPersons.DataTextField = "PersonName";
                ddlTicketPersons.DataValueField = "PersonID";
                ddlTicketPersons.DataBind();
                ddlTicketPersons.Items.Insert(0, new ListItem("New Employee", "-1"));
            }
        }

        protected void getTicketDDLStatus()
        {
            string query = "select statusID, StatusName from status where technician=0";
            DataTable dt = new DataTable();
            if ((lblisTech.Value == "true") || (lblisAdmin.Value == "true"))
            {
                query = "select statusid, statusname from status where statusid < 6 ";
            }

            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                ddlTicketStatus.DataSource = null;
                ddlTicketStatus.DataBind();
                ddlTicketStatus.DataSource = dt;
                ddlTicketStatus.DataTextField = "StatusName";
                ddlTicketStatus.DataValueField = "StatusID";
                ddlTicketStatus.DataBind();
            }

        }

        protected void saveTicket()
        {
            string _computerID = null ;
            string _catid = null;
            string _subcatid = null;
            string _selectionid = null;
            string _assginedtoid = null;
            string _notes = "";
            
            string query;
            string success;
            if (tbTicketID.Text != "-1")
            {
                int _status = Convert.ToInt16(ddlTicketStatus.SelectedValue.ToString());

                if ((lblticketClosed.Value == "true") && (_status < 4))
                    ddlTicketStatus.SelectedValue = "2";
                if (ddlComputers.SelectedValue.ToString() == "-1")
                {
                    _computerID = null;
                }
                else
                    _computerID = ddlComputers.SelectedValue.ToString();
                if (ddlCategory.SelectedValue.ToString() == "")
                    ddlCategory.SelectedValue = "-1";
                if (ddlSubCategory.SelectedValue.ToString() == "")
                    ddlSubCategory.SelectedValue = "-1";
                if (ddlSelection.SelectedValue.ToString() == "")
                    ddlSelection.SelectedValue = "-1";

                if (ddlCategory.SelectedValue.ToString() == "-1")
                    _catid = null;
                else
                    _catid = ddlCategory.SelectedValue.ToString();
                if (ddlSubCategory.SelectedValue.ToString() == "-1")
                    _subcatid = null;
                else
                    _subcatid = ddlSubCategory.SelectedValue.ToString();

                if (ddlSelection.SelectedValue.ToString() == "-1")
                    _selectionid = null;
                else
                    _selectionid = ddlSelection.SelectedValue.ToString();
                if (ddlTechs.SelectedValue.ToString() == "-1")
                    _assginedtoid = null;
                else
                    _assginedtoid = ddlTechs.SelectedValue.ToString();


                query = "update ticket set personid=@personid, description=@description, categoryid=@categoryid, subcategoryid=@subcategoryid, selectionid=@selectionid, computerid=@computerid, assignedto=@assignedto, statusid =@statusid, lastupdated = @lastupdated, updatedbyid = @updatedbyid where ticketID = @ticketid ";
                lblMyTicketID.Value = tbTicketID.Text.ToString();

                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlTicketPersons.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@description",  Value=tbNotes.Text.ToString()},
                    new SqlParameter() { ParameterName = "@categoryID", Value = _catid == null ?  (object)DBNull.Value : _catid.ToString() },
                    new SqlParameter() { ParameterName = "@subcategoryID",Value = _subcatid == null ? (object)DBNull.Value : _subcatid.ToString()  },
                    new SqlParameter() { ParameterName = "@selectionid", Value = _selectionid == null ? (object)DBNull.Value : _selectionid.ToString() },
                    new SqlParameter() { ParameterName = "@assignedto",  Value = _assginedtoid == null ? (object)DBNull.Value: _assginedtoid.ToString() },
                    new SqlParameter() { ParameterName = "@statusid",  Value = ddlTicketStatus.SelectedValue.ToString() },
                    new SqlParameter() { ParameterName = "@Ticketid",  Value = tbTicketID.Text == null ? (object)DBNull.Value: tbTicketID.Text.ToString()},
                    new SqlParameter() { ParameterName = "@ComputerID", Value = _computerID == null ? (object)DBNull.Value:_computerID.ToString() },
                    new SqlParameter() { ParameterName = "@lastupdated", Value = System.DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@updatedbyid", Value = lbluserID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@DateClosed", Value = _computerID == null ? (object)DBNull.Value:tbDateClosed.Text.ToString() }
                };
                sqlTypeCommands sql = new sqlTypeCommands();
                success = sql.CommadSQL(query, false, sp);
                if (success != "")
                    lblMessage.Text = success.ToString();
                else
                {
                    _notes = "Ticket ID " + lblMyTicketID.Value.ToString() + " changed by : " + lbluserName.Value.ToString() + " changed : " + hfChanges.Value.ToString();
                }
            }
            else
            {
                if (ddlComputers.SelectedValue.ToString() == "-1")
                {
                    _computerID = null;
                }
                else
                    _computerID = ddlComputers.SelectedValue.ToString();

                if (ddlCategory.SelectedValue.ToString() == "")
                    _catid = null;
                if (ddlSubCategory.SelectedValue == "")
                    _subcatid = null;
                if (ddlSelection.SelectedValue == "")
                    _selectionid = null;


                if ((ddlCategory.SelectedValue.ToString() == "-1") || (ddlCategory.SelectedValue.ToString() == ""))
                    _catid = null;
                else
                    _catid = ddlCategory.SelectedValue.ToString();

                if ((ddlSubCategory.SelectedValue.ToString() == "-1") || (ddlSubCategory.SelectedValue == ""))
                    _subcatid = null;
                else
                    _subcatid = ddlSubCategory.SelectedValue.ToString();

                if ((ddlSelection.SelectedValue.ToString() == "-1") || (ddlSelection.SelectedValue == ""))
                    _selectionid = null;
                else
                    _selectionid = ddlSelection.SelectedValue.ToString();

                if (ddlTechs.SelectedValue.ToString() == "-1")
                    _assginedtoid = null;
                else
                    _assginedtoid = ddlTechs.SelectedValue.ToString();

                query = "insert ticket (datecreated,createdby, personid, description, categoryid, subcategoryid, selectionid, computerid, assignedto, statusid) values (@datecreated, @createdby, @personid, @description, @categoryid, @subcategoryid, @selectionid, @computerid, @assignedto, @statusid)";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlTicketPersons.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@createdby", Value=hfcreatedid.Value.ToString()},
                    new SqlParameter() { ParameterName = "@description",  Value=tbNotes.Text.ToString()},
                    new SqlParameter() { ParameterName = "@categoryID", Value = _catid == null ? (object)DBNull.Value : _catid.ToString()},
                    new SqlParameter() { ParameterName = "@subcategoryID",Value = _subcatid == null ? (object)DBNull.Value : _subcatid.ToString()  },
                    new SqlParameter() { ParameterName = "@selectionid", Value = _selectionid == null ? (object)DBNull.Value : _selectionid.ToString() },
                    new SqlParameter() { ParameterName = "@assignedto",  Value = _assginedtoid == null ? (object)DBNull.Value: _assginedtoid.ToString() },
                    new SqlParameter() { ParameterName = "@statusid",  Value = ddlTicketStatus.SelectedValue.ToString() },
                    new SqlParameter() { ParameterName = "@ComputerID", Value = _computerID == null ? (object)DBNull.Value:_computerID.ToString() },
                    new SqlParameter() { ParameterName = "@DateCreated", Value = System.DateTime.Now.ToString()}
                                   };
                sqlTypeCommands sql = new sqlTypeCommands();
                success = sql.CommadSQL(query, false, sp);
                if (success != "")
                {

                    lblMessage.Text = success.ToString();

                }
                else
                {
                    if (lblMyTicketID.Value == "-1")
                    {
                        DataTable dt = new DataTable();
                        string getID = "select top 1 ticketid from ticket where createdby = " + lbluserID.Value.ToString() + " order by datecreated desc";
                        dt = sql.ReturnDatatable(getID);
                        if (dt.Rows.Count > 0)
                        {
                            lblMyTicketID.Value = dt.Rows[0].ItemArray[0].ToString();
                            tbTicketID.Text = dt.Rows[0].ItemArray[0].ToString();
                        }
                    }

                    _notes = "New ticket #" + lblMyTicketID.Value.ToString() + " was created by " + lbluserName.Value.ToString() + " on " + System.DateTime.Now.ToString();
                }
            }

            if (_notes != "")
            {
                sqlTypeCommands sql = new sqlTypeCommands();
                if (lblMyTicketID.Value == "1")
                {
                    DataTable dt = new DataTable();
                    string getID = "select top 1 ticketid from ticket where createdby = " + lbluserID.Value.ToString() + " order by datecreated desc";
                    dt = sql.ReturnDatatable(getID);
                    if (dt.Rows.Count > 0)
                    {
                        lblMyTicketID.Value = dt.Rows[0].ItemArray[0].ToString();
                        tbTicketID.Text = dt.Rows[0].ItemArray[0].ToString();
                    }
                }
                query = "insert into  ticketnotes (ticketid, personid, datecreated, notes) values ('" + lblMyTicketID.Value.ToString() + "' , '" + lbluserID.Value.ToString() + "', '" + System.DateTime.Now.ToString() + "', '" + _notes + "')";
                sql.CommadSQL(query, false, null);

                Email email = new Email();

                if (ddlTicketPersons.SelectedValue != hfcreatedid.Value.ToString())
                    email.sendEmail(ddlTicketPersons.SelectedValue.ToString(), "Ticket ID # " + lblMyTicketID.Value.ToString(), _notes.ToString());
                email.sendEmail(lbluserID.Value.ToString(), "Ticket ID # " + lblMyTicketID.Value.ToString(), _notes.ToString());

                email.sendEmail(ddlTechs.SelectedValue.ToString(), "Ticket ID # " + lblMyTicketID.Value.ToString(), _notes.ToString());

                sql.UpdateSecurityLog(lbluserID.Value.ToString(), _notes.ToString());   
            }
        }

        protected void saveNotes(string _Notes)
        {

        }

        protected void returnToTicket()
        {
            if (hfReturn.Value == "admin")
            {
                string s = "admin.aspx?uid=" + lbluserID;
                Response.Redirect(s, true);
            }
            else
                Response.Redirect("Tickets.aspx", true);
        }

        
        protected void menuTicket_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (menuTicket.SelectedItem.Text)
            {
                case "Save":
                    saveTicket();
                    Response.Redirect("Tickets.aspx", true);
                    break;
                case "Cancel":
                    returnToTicket();                    
                    break;
            }
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!_isDDLChanging) || (ddlSubCategory.SelectedValue != "-1"))
            {
                lblchangeMade.Value = "true";
                getddlSelection();
                hfChanges.Value += " subcategory changed to " + ddlSubCategory.SelectedItem.ToString();
                sidenotes = " subcategory changed to " + ddlSubCategory.SelectedItem.ToString();
            }
              
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!_isDDLChanging) || (ddlCategory.SelectedValue != "-1"))
            {
                lblchangeMade.Value = "true";
                getddlSubCategory();
                hfChanges.Value += " category changed to " + ddlCategory.SelectedItem.ToString();
                sidenotes = " category changed to " + ddlCategory.SelectedItem.ToString();
            }

            ddlSelection.Items.Clear();
        }

        protected void ddlTicketStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (lblNewTicket.Value == "false")
            {
                lblchangeMade.Value = "true";
                if ((ddlTicketStatus.SelectedValue == "4") || (ddlTicketStatus.SelectedValue == "5"))
                {

                    tbDateClosed.Text = System.DateTime.Now.ToString();
                    hfChanges.Value += " ticket status changed to " + ddlTicketStatus.SelectedItem.ToString();
                    sidenotes = " ticket status changed to " + ddlTicketStatus.SelectedItem.ToString();
                }
                
            }
        }

        protected void ddlTechs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblNewTicket.Value == "false")
            {
                hfChanges.Value += " Tech assigned changed to " + ddlTechs.SelectedItem.ToString();
                sidenotes = " Tech assigned changed to " + ddlTechs.SelectedItem.ToString();
                if (ddlTicketStatus.SelectedValue == "0")
                    ddlTicketStatus.SelectedValue = "1";
                lblchangeMade.Value = "true";

            }

        }

        protected void ddlTicketPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblNewTicket.Value == "false")
            {
                hfChanges.Value += " Ticket user changed to " + ddlTicketPersons.SelectedItem.ToString();
                sidenotes = " Ticket user changed to " + ddlTicketPersons.SelectedItem.ToString();
                lblchangeMade.Value = "true";
            }

        }

        protected void ddlComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblNewTicket.Value == "false")
            {
                lblchangeMade.Value = "true";
                hfChanges.Value += " computer changed to " + ddlComputers.SelectedItem.ToString();
                sidenotes = " computer changed to " + ddlComputers.SelectedItem.ToString();
            }

        }

        protected void tbNotes_TextChanged(object sender, EventArgs e)
        {
            if ((lblNewTicket.Value == "false") || (tbNotes.Text != tbNotesHidden.Text))
            {
                lblchangeMade.Value = "true";
                hfChanges.Value += " description changed.";
                sidenotes = " description changed.";
            }
        }

        protected void bnAddNote_Click(object sender, EventArgs e)
        {
            if (tbNewNote.Text != "") 
            {
                String query = "insert into ticketnotes (TicketID, PersoniD, datecreated, Notes) values (@ticketid, @personid, @datecreated, @notes)";
                sqlTypeCommands sql = new sqlTypeCommands();
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=lbluserID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@ticketID", Value = lblMyTicketID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@datecreated", Value = DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@notes", Value = tbNewNote.Text.ToString()}
                };
               string result  = sql.CommadSQL(query, false, sp);
                if (result != "")
                {
                    lblMessage.Text = result.ToString();
                }
                else
                {
                    query = "";
                    if ((lblisTech.Value.ToString() == "true") || (lblisAdmin.Value.ToString() == "true"))
                    {

                        if (((ddlTicketStatus.SelectedValue.ToString() == "0") || (ddlTicketStatus.SelectedValue.ToString() == "1")) &&
                        (ddlTechs.SelectedValue == "1"))
                        {
                            ddlTicketStatus.SelectedValue = "2";
                            ddlTechs.SelectedValue = lbluserID.Value.ToString();
                            query = "update ticket set statusID = 1, assignedto = " + lbluserID.Value.ToString() + " where ticketid = " + lblMyTicketID.Value.ToString();
                            sql.CommadSQL(query, false, null);
                        }
                        if (ddlTicketPersons.SelectedValue != hfcreatedid.Value.ToString())
                            sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), ddlTicketPersons.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), tbNewNote.Text.ToString());
                        sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), hfcreatedid.Value.ToString(), lblMyTicketID.Value.ToString(), tbNewNote.Text.ToString());
                        sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), ddlTechs.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), tbNewNote.Text.ToString());
                        sql.UpdateSecurityLog(lbluserID.Value.ToString(), "New note on Ticket ID " + lblMyTicketID.Value.ToString());
                        tbNewNote.Text = "";
                        newnote = false;
                        getTicket();
                        getTicketNotes();
                    }
                }
            }
        }

        protected void tbNewNote_TextChanged(object sender, EventArgs e)
        {
        }

        protected void sendEmail(string subject, string toid, string ticketid, string data)
        {
            Email email = new Email();
            bool result = email.sendEmail(toid, subject, data);
            if (!result)
                lblMessage.Text = "Email failed";

        }

    }
}