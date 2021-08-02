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
using System.Text.RegularExpressions;

namespace Help_Desk
{
    public partial class ThisTicket : System.Web.UI.Page
    {
        public bool _statusChange;
        public bool _isDDLChanging;
        public string notes;
        public string sidenotes;
        public Boolean newnote = false;
        public Dictionary<string, string> emailParameters = new Dictionary<string, string>();

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
                    lblIsTech.Value = "false";
                    lblIsAdmin.Value = "false";
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
                    lblIsTech.Value = sql.getIsTech(lbluserID.Value) ? "true" : "false";
                    lblIsAdmin.Value = sql.getIsAdmin(lbluserID.Value) ? "true" : "false";
                    //if (lblisTech.Value.ToString() == "false")
                    if ((lblIsAdmin.Value == "false" ) || (lblIsTech.Value == "false"))
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
                        pnlTicketNotes.Visible = false;
                        pnlTechNotes.Visible = false;
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
                    if ((lblIsTech.Value.ToString() == "true") || (lblIsAdmin.Value.ToString() == "true"))
                    {
                        pnlTechNotes.Visible = true;
                        tbTechNotespassword.Visible = false;
                        btnTechNotes.Visible = false;
                        hfTechNotes.Value = "true";
                        ddlTechs.Enabled = true;
                        getTechNotes();
                    }
                    else
                    {
                        tbTechNotespassword.Visible = true;
                        pnlTechNotes.Visible = false;
                        btnTechNotes.Visible = true;
                        hfTechNotes.Value = "false";
                    }
                }
            }
        }
        protected string getEmailParameters(string _notes)
        {
            string newMessage = "";
            Regex re = new Regex(@"\##(\w+)\##", RegexOptions.Compiled);
            try
            {

                emailParameters.Clear();
                //##firstName##, ##LastName##, ##TicketID##, ##TechFirstName##, ##TechLastName##, ##TicketChange##
                string names = ddlTicketPersons.SelectedItem.ToString();
                string techNames = ddlTechs.SelectedItem.ToString();
                string[] _names = names.Split(' ');
                string[] _techNames = techNames.Split(' ');
                emailParameters.Add("##FirstName##", _names[0]);
                emailParameters.Add("##LastName##", _names[1]);
                emailParameters.Add("##TicketID##", tbTicketID.Text.ToString());
                emailParameters.Add("##TechFirstName##", _techNames[0]);
                emailParameters.Add("##TechLastName##", _techNames[1]);
                emailParameters.Add("##TicketChange##", _notes);
                newMessage = re.Replace(_notes, match => emailParameters[match.Groups[1].Value]);
            }
            catch
            {

            }
            return newMessage;

             
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
            string success = sql.CommandSQL(query, false, null);

            //Save computer to user
            //get computerid
            query = "select computerid from computer where computername = '" + computerName.ToString() + "'";
            dt.Reset();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                query = "insert into computeruser (computerid, personid) values('" + dt.Rows[0].ItemArray[0].ToString() + "', '" + lbluserID.Value.ToString() + "')";
                string Success = sql.CommandSQL(query, false, null);
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
            String query = "select	t.ticketid,	t.createdbyID, t.createdby,	t.personid,	t.DateCreated, t.description, t.statusid, t.status, t.categoryid, t.subcategoryid, t.selectionid, t.category, t.subcategory, t.selection, t.assignedto, t.assignedid, t.computerid, t.LastUpdate, t.LastUpdateby from v_Tickets t where ticketID = " + lblMyTicketID.Value.ToString();

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
                ddlTicketPersons.SelectedValue = dt.Rows[0].ItemArray[3].ToString();

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
                taDescription.Value = dt.Rows[0].ItemArray[5].ToString();
                tbDescriptionHidden.Text = dt.Rows[0].ItemArray[5].ToString();
                tbLastUpdate.Text = dt.Rows[0].ItemArray[17].ToString();
                tbupdatedby.Text = dt.Rows[0].ItemArray[18].ToString();
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
            String query = "select datecreated + ' ' + createdby + ' - ' + notes as info from v_ticketnotes where ticketid = " + lblMyTicketID.Value.ToString() + " order by datecreated desc";
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
            ddlTechs.Items.Insert(0, new ListItem("System Attendant", "1"));
            ddlTechs.SelectedIndex = 0;
        }


        protected void getddlSelection()
        {
            string query = "select selectionName, selectionID from selection where subcategoryId = " + ddlSubCategory.SelectedValue.ToString() + " order by case when SelectionName='UNKNOWN' then 0 else 1 end "; ;
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
            }
        }


        protected void getddlSubCategory()
        {
                string query = "select subcategoryName, subcategoryID from subcategory where categoryId = " + ddlCategory.SelectedValue.ToString() + " order by case when SubCategoryName='UNKNOWN' then 0 else 1 end ";
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
                getddlSelection();
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
                    getddlSubCategory();
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
            if ((lblIsTech.Value == "true") || (lblIsAdmin.Value == "true"))
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
            string query = "select statusid, statusname from status where statusid < 6 ";
            DataTable dt = new DataTable();
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
            if ((lblIsTech.Value != "true") || (lblIsAdmin.Value != "true"))
            {
                ddlTicketStatus.Items.FindByValue("2").Attributes.Add("disabled", "disabled");
                ddlTicketStatus.Items.FindByValue("3").Attributes.Add("disabled", "disabled");
                ddlTicketStatus.Items.FindByValue("4").Attributes.Add("disabled", "disabled");
            }


        }

        protected void saveTicket()
        {
            string _computerID = null;
            string _catid = null;
            string _subcatid = null;
            string _selectionid = null;
            string _assginedtoid = null;
            string _notes = "";
            string _closedbyDate = "";
            string query;
            string success;
            string _closedbyID;
            if ((string.IsNullOrEmpty(tbDateClosed.Text)) && (ddlTicketStatus.SelectedItem.ToString() == "Closed"))
            {
                tbClosedby.Text = System.DateTime.Now.ToString();
                _closedbyID = lbluserID.Value.ToString();
            }
            if (tbTicketID.Text != "-1")
            {
                int _status = Convert.ToInt16(ddlTicketStatus.SelectedValue.ToString());

                if ((lblticketClosed.Value == "true") && (_status < 4))
                    ddlTicketStatus.SelectedValue = "2";

                if ((string.IsNullOrEmpty(tbDateClosed.Text)) && (_status == 4))
                {
                    _closedbyDate = System.DateTime.Now.ToString();
                    _closedbyID = lbluserID.Value.ToString();
                }
                    
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
                    _catid = "10";
                else
                    _catid = ddlCategory.SelectedValue.ToString();
                if (ddlSubCategory.SelectedValue.ToString() == "-1")
                    _subcatid = "7";
                else
                    _subcatid = ddlSubCategory.SelectedValue.ToString();

                if (ddlSelection.SelectedValue.ToString() == "-1")
                    _selectionid = "2";
                else
                    _selectionid = ddlSelection.SelectedValue.ToString();
                if (ddlTechs.SelectedValue.ToString() == "-1")
                    _assginedtoid = null;
                else
                    _assginedtoid = ddlTechs.SelectedValue.ToString();


                //query = "update ticket set personid=@personid, description=@description, categoryid=@categoryid, subcategoryid=@subcategoryid, selectionid=@selectionid, computerid=@computerid, assignedto=@assignedto, statusid =@statusid, lastupdated = @lastupdated, updatedbyid = @updatedbyid where ticketID = @ticketid ";
                query = "save_Ticket";
                lblMyTicketID.Value = tbTicketID.Text.ToString();

                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlTicketPersons.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@description",  Value=taDescription.Value.ToString()},
                    new SqlParameter() { ParameterName = "@categoryID", Value = string.IsNullOrEmpty(_catid) ?  (object)DBNull.Value : _catid.ToString() },
                    new SqlParameter() { ParameterName = "@subcategoryID",Value = string.IsNullOrEmpty(_subcatid) ? (object)DBNull.Value : _subcatid.ToString()  },
                    new SqlParameter() { ParameterName = "@selectionid", Value = string.IsNullOrEmpty(_selectionid) ? (object)DBNull.Value : _selectionid.ToString() },
                    new SqlParameter() { ParameterName = "@assignedto",  Value = string.IsNullOrEmpty(_assginedtoid) ? (object)DBNull.Value: _assginedtoid.ToString() },
                    new SqlParameter() { ParameterName = "@statusid",  Value = ddlTicketStatus.SelectedValue.ToString() },
                    new SqlParameter() { ParameterName = "@Ticketid",  Value = string.IsNullOrEmpty(tbTicketID.Text) ? (object)DBNull.Value: tbTicketID.Text.ToString()},
                    new SqlParameter() { ParameterName = "@ComputerID", Value = string.IsNullOrEmpty(_computerID) ? (object)DBNull.Value:_computerID.ToString()},
                    new SqlParameter() { ParameterName = "@lastupdated", Value = System.DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@Createdby", Value = hfcreatedid.Value.ToString()},
                    new SqlParameter() { ParameterName = "@updatedbyid", Value = lbluserID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@DateCreated", Value=tbDateCreated.Text.ToString()},
                    new SqlParameter() { ParameterName = "@type", Value = "Update"},
                    new SqlParameter() { ParameterName = "@ClosedByID", Value = string.IsNullOrEmpty(hfClosedbyID.Value) ?(object)DBNull.Value: hfClosedbyID.Value.ToString() },
                    new SqlParameter() { ParameterName = "@DateClosed", Value = string.IsNullOrEmpty(_closedbyDate.ToString()) ? (object)DBNull.Value:tbDateClosed.Text.ToString()}
                };
                sqlTypeCommands sql = new sqlTypeCommands();
                success = sql.CommandSQL(query, true, sp);
                if (success != "")
                    lblMessage.Text = success.ToString();
                else
                {
                    _notes = "Ticket ID " + lblMyTicketID.Value.ToString() + " changed by : " + lbluserName.Value.ToString() + " changed : " + hfChanges.Value.ToString();
                }
            }
            else
            {
                hfIsTicketNew.Value = "true";
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

                query = "save_ticket";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=ddlTicketPersons.SelectedValue.ToString()},
                    new SqlParameter() { ParameterName = "@description",  Value=taDescription.Value.ToString()},
                    new SqlParameter() { ParameterName = "@categoryID", Value = _catid == null ? (object)DBNull.Value : _catid.ToString()},
                    new SqlParameter() { ParameterName = "@subcategoryID",Value = _subcatid == null ? (object)DBNull.Value : _subcatid.ToString()  },
                    new SqlParameter() { ParameterName = "@selectionid", Value = _selectionid == null ? (object)DBNull.Value : _selectionid.ToString() },
                    new SqlParameter() { ParameterName = "@assignedto",  Value = _assginedtoid == null ? (object)DBNull.Value: _assginedtoid.ToString() },
                    new SqlParameter() { ParameterName = "@statusid",  Value = ddlTicketStatus.SelectedValue.ToString() },
                    new SqlParameter() { ParameterName = "@Ticketid", Value = "-1"},
                    new SqlParameter() { ParameterName = "@ComputerID", Value = _computerID == null ? (object)DBNull.Value:_computerID.ToString() },
                    new SqlParameter() { ParameterName = "@lastupdated", Value = (object)DBNull.Value},
                    new SqlParameter() { ParameterName = "@Createdby", Value=hfcreatedid.Value.ToString()},
                    new SqlParameter() { ParameterName = "@updatedbyid", Value =(object)DBNull.Value},
                    new SqlParameter() { ParameterName = "@DateCreated", Value = System.DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@type", Value = "Insert"},
                    new SqlParameter() { ParameterName = "@ClosedByID", Value = (object)DBNull.Value},
                    new SqlParameter() { ParameterName = "@DateClosed", Value = (object)DBNull.Value },

                };
                sqlTypeCommands sql = new sqlTypeCommands();
                success = sql.CommandSQL(query, true, sp);
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

                    updateTicketDate();
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

                DataTable dt2 = new DataTable();
                string url = "http://" + HttpContext.Current.Request.Url.Host.ToString() + "/ThisTicket.aspx?TID=" + lblMyTicketID.Value.ToString();
                string TicketURL = string.Format("\n To update your ticket, click {0}", "<a href=" + url + ">here</a>");

                if ((ddlSelection.SelectedValue.ToString() != null) || (ddlSelection.SelectedValue.ToString() == "-1"))
                {
                    query = "select emailbody from selection where selectionid = " + ddlSelection.SelectedValue.ToString();
                    dt2 = sql.ReturnDatatable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        string NewNote = getEmailParameters(dt2.Rows[0].ItemArray[0].ToString());

                        if (!string.IsNullOrEmpty(dt2.Rows[0].ItemArray[0].ToString()))
                            _notes = _notes + "\n" + dt2.Rows[0].ItemArray[0].ToString();
                    }
                }
                //_notes += "\n Click the following update your ticket: /n http://" + 
                //    HttpContext.Current.Request.Url.Host.ToString() + "/ThisTicket.aspx?TID=" + lblMyTicketID.Value.ToString();
                query = "insert into  ticketnotes (ticketid, personid, datecreated, notes) values ('" + lblMyTicketID.Value.ToString() + "' , '" + lbluserID.Value.ToString() + "', '" + System.DateTime.Now.ToString() + "', '" + _notes + "')";
                sql.CommandSQL(query, false, null);
    
                _notes = _notes + TicketURL.ToString();

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
                case "Save Ticket":
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
                updateTicketDate();
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

        protected void updateTicketDate()
        {
            string query = "sp_insert_update_Ticket_dates";
            sqlTypeCommands sql = new sqlTypeCommands();
            List<SqlParameter> sp2 = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName ="@Ticketstatusid", Value = ddlTicketStatus.SelectedValue.ToString() },
                        new SqlParameter() { ParameterName ="@personid", Value = lbluserID.Value.ToString() },
                        new SqlParameter() { ParameterName = "@ticketid", Value=lblMyTicketID.Value.ToString() },
                        new SqlParameter() { ParameterName = "@ticketdate", Value = System.DateTime.Now.ToString()}
                    };
            string success = sql.CommandSQL(query, true, sp2);
            if (success != "")
            {
                MessageBox.show(this.Page, success);
            }

        }

        protected void taDescription_TextChanged(object sender, EventArgs e)
        {
            if ((lblNewTicket.Value == "false") || (taDescription.Value != tbDescriptionHidden.Text))
            {
                lblchangeMade.Value = "true";
                hfChanges.Value += " description changed.";
                sidenotes = " description changed.";
            }
        }
        protected void tbDescription_TextChanged(object sender, EventArgs e)
        {
            if ((lblNewTicket.Value == "false") || (taDescription.Value != tbDescriptionHidden.Text))
            {
                lblchangeMade.Value = "true";
                hfChanges.Value += " description changed.";
                sidenotes = " description changed.";
            }
        }

        protected void bnAddNote_Click(object sender, EventArgs e)
        {
            sqlTypeCommands sql = new sqlTypeCommands();

            if (taNewNote.Value != "") 
            {
                string update2 = "update ticket set lastupdated = @lastupdated, updatedbyid = @updatedbyid where ticketid = @ticketid";
                List<SqlParameter> sp2 = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@lastupdated", Value = DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@updatedbyid", Value = lbluserID.Value.ToString() },
                    new SqlParameter() { ParameterName = "@ticketid", Value = tbTicketID.Text.ToString()}
                    };
                string result = sql.CommandSQL(update2, false, sp2);
                if (result !="")
                {
                    lblMessage.Text = result.ToString();
                    lblMessage.Visible = true;
                }

                String query = "insert into ticketnotes (TicketID, PersoniD, datecreated, Notes) values (@ticketid, @personid, @datecreated, @notes)";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName = "@personid",  Value=lbluserID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@ticketID", Value = lblMyTicketID.Value.ToString()},
                    new SqlParameter() { ParameterName = "@datecreated", Value = DateTime.Now.ToString()},
                    new SqlParameter() { ParameterName = "@notes", Value = taNewNote.Value }
                };
               result  = sql.CommandSQL(query, false, sp);
                if (result != "")
                {
                    lblMessage.Text = result.ToString();
                    lblMessage.Visible = true;
                }
                else
                {
                    query = "";
                    if (lblIsTech.Value.ToString() == "true")
                    {

                        string url = "http://" + HttpContext.Current.Request.Url.Host.ToString() + "/ThisTicket.aspx?TID=" + lblMyTicketID.Value.ToString();
                        string TicketURL = string.Format("\n To update your ticket, click {0}", "<a href=" + url + ">here</a>");
                        if ((ddlTicketStatus.SelectedValue.ToString() == "0") || (ddlTicketStatus.SelectedValue.ToString() == "1"))
                        {
                            ddlTicketStatus.SelectedValue = "2";
                            query = "update ticket set statusID = 2 where ticketid = " + lblMyTicketID.Value.ToString();
                            if (ddlTicketStatus.SelectedValue.ToString() == "0")
                            {
                                ddlTechs.SelectedValue = lbluserID.Value.ToString();
                                query = "update ticket set statusID = 2, assignedto = " + lbluserID.Value.ToString() + " where ticketid = " + lblMyTicketID.Value.ToString();
                            }
                            sql.CommandSQL(query, false, null);
                            sendEmail("Ticket ID " + lblMyTicketID.Value.ToString() + " assigned", ddlTicketPersons.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), "The Ticket is assigned to " + ddlTechs.SelectedItem.ToString() +  TicketURL);
                            sendEmail("Ticket ID " + lblMyTicketID.Value.ToString() + " assigned", ddlTechs.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), "The Ticket is assigned to " + ddlTechs.SelectedItem.ToString() +  TicketURL);
                            updateTicketDate();
                        }

                        
                        if (ddlTicketPersons.SelectedValue != hfcreatedid.Value.ToString())
                            sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), ddlTicketPersons.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), taNewNote.Value.ToString() +  TicketURL );
                        

                        sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), hfcreatedid.Value.ToString(), lblMyTicketID.Value.ToString(), taNewNote.Value.ToString() +  TicketURL);
                        sendEmail("New note on Ticket ID " + lblMyTicketID.Value.ToString(), ddlTechs.SelectedValue.ToString(), lblMyTicketID.Value.ToString(), taNewNote.Value.ToString() + TicketURL);
                        sql.UpdateSecurityLog(lbluserID.Value.ToString(), "New note on Ticket ID " + lblMyTicketID.Value.ToString());

                        taNewNote.Value  = "";
                        newnote = false;
                        getTicket();
                        getTicketNotes();
                    }
                }
            }
        }


        protected void sendEmail(string subject, string toid, string ticketid, string data)
        {
            Email email = new Email();
            bool result = email.sendEmail(toid, subject, data);
            if (!result)
                lblMessage.Text = "Email failed";
        }

        protected void getTechNotes()
        {
            hfTechNotesID.Value = "-1";
            string query;
            query = "select * from V_technotes where ticketid = " + lblMyTicketID.Value.ToString() + " order by datecreated desc";
            if (lblMyTicketID.Value != "-1")
            {
                DataTable dt = new DataTable();
                sqlTypeCommands sql = new sqlTypeCommands();
                dt = sql.ReturnDatatable(query);
                gvTechNotes.DataSource = null;
                gvTechNotes.DataBind();
                if (dt.Rows.Count > 0)
                {
                    gvTechNotes.DataSource = dt;
                    gvTechNotes.DataBind();
                }
            }
        }

        protected void SQLTechNotes(string spType)
        {
            string query = "sp_add_update_delete_TechNotes";
            int techid;
            sqlTypeCommands sql = new sqlTypeCommands();
            if (lblIsTech.Value == "false")
                techid = 0;
            else
                techid = Convert.ToInt16(lbluserID.Value.ToString());
            List<SqlParameter> sp = new List<SqlParameter>
            {
                new SqlParameter() { ParameterName = "@Ticketid",  Value=lblMyTicketID.Value.ToString()},
                new SqlParameter() { ParameterName = "@spType", Value = spType.ToString()},
                new SqlParameter() { ParameterName = "@techNotes", Value = taTechNotes.Value.ToString()},
                new SqlParameter() { ParameterName = "@techID", Value = techid.ToString()},
                new SqlParameter() { ParameterName = "@dateCreated", Value =System.DateTime.Now.ToString()},
                new SqlParameter() { ParameterName = "@techNotesID", Value = hfTechNotesID.Value.ToString()}
            };
            string result = sql.CommandSQL(query, true, sp);
            if (result != "")
                MessageBox.show(this.Page,result);
            taTechNotes.Value = "";
            getTechNotes();
        }

        protected void editTechNote(int index)
        {
            taTechNotes.Value = gvTechNotes.Rows[index].Cells[5].Text.ToString();
        }

        protected void saveTechNotes()
        {
            if (hfTechNotesID.Value == "-1")
                SQLTechNotes("Insert");
            else
                SQLTechNotes("Update");

         }

        protected void btnTechNotes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTechNotespassword.Text))
                if (btnTechNotes.Text == "Logout")
                {
                    gvTechNotes.DataSource = null;
                    gvTechNotes.DataBind();
                    hfTechNotes.Value = "false";
                    pnlTechNotes.Visible = false;
                    btnSaveTechNotes.Text = "Save Tech Notes";
                }
                else
                {

                    DataTable dt = new DataTable();
                    sqlTypeCommands sql = new sqlTypeCommands();
                    string query = "select techpassword from settings";
                    dt = sql.ReturnDatatable(query);
                    string techpass = dt.Rows[0].ItemArray[0].ToString();
                    if (techpass == tbTechNotespassword.Text.ToString())
                    {
                        hfTechNotes.Value = "true";
                        btnTechNotes.Text = "Logout";
                        getTechNotes();
                        pnlTechNotes.Visible = true;
                    }
                }
        }

        protected void tbTechNotes_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnSaveTechNotes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(taTechNotes.Value))
            {
                saveTechNotes();
            }
        }

        protected void gvTechNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvTechNotes.Rows[index];
            hfTechNotesID.Value = selectedRow.Cells[2].Text.ToString();
            if (e.CommandName == "TechNoteDelete")
            {
                SQLTechNotes("Delete");
            }
            if (e.CommandName == "TechNoteEdit")
            {
                editTechNote(index);
            }
        }
    }
}