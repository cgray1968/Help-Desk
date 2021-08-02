using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Help_Desk
{
    public partial class Reports : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ddlChartType.Items.Count == 0)
                getddlChartTypes();
        }


        protected void getddlChartTypes()
        {
            ddlChartType.Items.Insert(0, new ListItem("Select Type", "-1"));

        }
        protected void getSummaryData()
        {
            gvSummaryTechs.DataSource = null;
            gvSummaryTechs.DataBind();
            sqlTypeCommands sql = new sqlTypeCommands();
            string query;
            DataTable dt1 = new DataTable();
            query = "select * from v_ticket_summary";
            dt1 = sql.ReturnDatatable(query);
            tbNew.Text = "";
            tbOpen.Text = "";
            tbInProgress.Text = "";
            tbCompleted.Text = "";
            tbClosed.Text = "";
            tbCanceled.Text = "";
            tbTotal.Text = "";
            if (dt1.Rows.Count > 0)
            {
                tbNew.Text = dt1.Rows[0].ItemArray[0].ToString();
                tbOpen.Text = dt1.Rows[0].ItemArray[1].ToString();
                tbInProgress.Text = dt1.Rows[0].ItemArray[2].ToString();
                tbCompleted.Text = dt1.Rows[0].ItemArray[3].ToString();
                tbClosed.Text = dt1.Rows[0].ItemArray[4].ToString();
                tbCanceled.Text = dt1.Rows[0].ItemArray[5].ToString();
                tbTotal.Text = dt1.Rows[0].ItemArray[6].ToString();
            }
            dt1.Reset();
            query = "select * from v_Tickets_per_tech";
            dt1 = sql.ReturnDatatable(query);
            if (dt1.Rows.Count > 0)
            {
                gvSummaryTechs.DataSource = dt1;
                gvSummaryTechs.DataBind();
            }
        }

        protected void getTechGraphicsData()
        {
  
        }

        protected void hideAllPanels()
        {
            pnlGraphics.Visible = false;
            pnlSummary.Visible = false;
        }

        protected void menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (menu1.SelectedItem.Value)
            {
                case "Return":
                    Response.Redirect("Tickets.aspx");
                    break;
                case "GraphicsTech":

                    break;
                case "Graphics":
                    hideAllPanels();
                    pnlGraphics.Visible = true;
                    break;
                case "Summary":
                    hideAllPanels();
                    pnlSummary.Visible = true;
                    getSummaryData();
                    break;
            }
        }

        protected void TwoGraph(string query, string ColumnA, string ColumnB)
        {
            Chart1.Visible = false;
            BarChart1.Visible = true;
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);
            if (dt.Rows.Count > 0)
            {
                string[] x = new string[dt.Rows.Count];
                decimal[] y = new decimal[dt.Rows.Count];
                for (int i=0; i< dt.Rows.Count;i++)
                {
                    x[i] = dt.Rows[i][0].ToString();
                    y[i] = Convert.ToInt32(dt.Rows[i][1]);
                }
                BarChart1.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y });
                BarChart1.CategoriesAxis = string.Join(",", x);
                BarChart1.ChartTitle = string.Format("{0} Graph", ddlChartType.SelectedItem.Text.ToString());
       
                if (x.Length > 3)
                {
                    BarChart1.ChartWidth = (x.Length * 100).ToString();
                }
                BarChart1.Visible = ddlChartType.SelectedItem.Value != "";
                BarChart1.CategoriesAxis.PadLeft(30);
                   
            }


            //List<string> Category = (from p in dt.AsEnumerable() select p.Field<string>(ColumnA)).Distinct().ToList();

            //foreach (string s in Category)
            //{
            //    string[] x = (from p in dt.AsEnumerable() where p.Field<string>(ColumnA) == s
            //                  orderby p.Field<string>(ColumnA) ascending
            //                  select p.Field<string>(ColumnA)).ToArray(); 

            //    int[] y = (from p in dt.AsEnumerable()
            //                  where p.Field<string>(ColumnA) == s
            //                  orderby p.Field<string>(ColumnA) ascending
            //                  select p.Field<int>(ColumnB)).ToArray();

            //    Chart1.Series.Add(new Series(s));
            //    Chart1.Series[s].IsValueShownAsLabel = true;
            //    Chart1.Series[s].ChartType = SeriesChartType.Column;
            //    Chart1.Series[s].Points.DataBindXY(x, y);
            //}
            //Chart1.Legends[0].Enabled = true;
            //Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }

        protected void ThreeGraph(string query)
        {
            Chart1.Visible = true;
            BarChart1.Visible = false;
            DataTable dt = new DataTable();
            sqlTypeCommands sql = new sqlTypeCommands();
            dt = sql.ReturnDatatable(query);

            List<string> Status = (from p in dt.AsEnumerable() select p.Field<string>("StatusName")).Distinct().ToList();

            foreach (string s in Status)
            {
                string[] x = (from p in dt.AsEnumerable()
                              where p.Field<string>("StatusName") == s
                              orderby p.Field<string>("StatusName") ascending
                              select p.Field<string>("Tech")).ToArray();


                int[] y = (from p in dt.AsEnumerable()
                           where p.Field<string>("StatusName") == s
                           orderby p.Field<string>("Tech") ascending
                           select p.Field<int>("Total")).ToArray();

                Chart1.Series.Add(new Series(s));
                Chart1.Series[s].IsValueShownAsLabel = true;
                Chart1.Series[s].ChartType = SeriesChartType.Column;
                Chart1.Series[s].Points.DataBindXY(x, y);
                Chart1.Series[s].CustomProperties = "PointWidth=.6";
            }
            Chart1.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

        }

        protected void graphSelection()
        {
            string query;
            query = "select combined, Total from v_Total_Ticket_per_Categories order by categoryID, SubCategoryID, SelectionID";
            TwoGraph(query, "Combined", "Total");

        }
        protected void graphTechs()
        {
            string query;
            query = "select	tech, statusname, total from v_tickets_status_tech ORDER BY StatusID, Tech";
            ThreeGraph(query);
        }

        protected void graphDepartment()
        {
            string query = "select * from v_Total_Ticket_Per_Department order by [Department Name]";
            TwoGraph(query, "Department Name", "Total");
        }

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlChartType.SelectedValue)
            {
            //"Selections", "0"
            //"Techs", "1"
            //"Department", "2"

                case "0":
                    graphSelection();
                    break;
                case "1":
                    graphTechs();
                    break;
                case "2":
                    graphDepartment();
                    break;
            }
        }
    }
}