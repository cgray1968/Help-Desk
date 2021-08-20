using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;

namespace Help_Desk
{
    public partial class Reports : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getReports();
        }

        protected void getReports()
        {
            gvReports.DataSource = null;
            gvReports.DataBind();

            string query = "select name from catalog where path like '/Reports/%";
            DataTable dt = new DataTable();
            string web = ConfigurationManager.ConnectionStrings["SSRSConnection"].ConnectionString;
            dt.Reset();
            try
            {
                SqlConnection con = new SqlConnection(web);
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {

                    gvReports.DataSource = dt;
                    gvReports.DataBind();
                }
            }
            catch (SystemException ex)
            {
            }


        }

        protected void Report(string ReportName)
        {
            sqlTypeCommands sql = new sqlTypeCommands();
            DataTable dt = new DataTable();
            string query = "select settingsValue from Settings where settingsName = 'ReprotServer*' order by settings_id";
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                string uri = dt.Rows[0].ItemArray[0].ToString();
                string username = dt.Rows[0].ItemArray[1].ToString();
                string password = dt.Rows[0].ItemArray[2].ToString();
                rvReports.ProcessingMode = ProcessingMode.Remote;
                ServerReport serverReport = rvReports.ServerReport;
                serverReport.ReportServerUrl = new Uri(uri);

           }


        }
        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvReports_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(gvReports.SelectedRow.Cells[0].ToString()))
                Report(gvReports.SelectedRow.Cells[0].ToString());
        }
    }
}