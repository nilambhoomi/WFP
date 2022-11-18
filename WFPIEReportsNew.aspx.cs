using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class WFPIEReportsNew : System.Web.UI.Page
{
    private static int PageSize = 10;
    private static string fmdt = "10/01/2019";
    private static string todt = "10/31/2019";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            txtFromDate.Text = "10/01/2019";
            txtToDate.Text = "10/31/2019";
            string frmdt = txtFromDate.Text;
            string toDate = txtToDate.Text;
            BindGrid(frmdt, toDate);
        }
    }
    [WebMethod]
    public static string GetIEReport(int pageIndex)
    {
        //string format = "mm/dd/yyyy";
        //DateTime fd = DateTime.ParseExact(fmdt, format, CultureInfo.InvariantCulture);
        //string fmd = fd.ToString("yyyy-mm-dd");
        //DateTime td = DateTime.ParseExact(todt, format, CultureInfo.InvariantCulture);
        //string tod = td.ToString("yyyy-mm-dd");

        string frmDate = DateTime.Parse(fmdt).ToString("yyyy-MM-dd");
        string ToDate = DateTime.Parse(todt).ToString("yyyy-MM-dd");
        string query = "[GetWFP_IEReport_Pager]";
        SqlCommand cmd = new SqlCommand(query);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FromDate", frmDate);
        cmd.Parameters.AddWithValue("@ToDate", ToDate);
        cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
        cmd.Parameters.AddWithValue("@PageSize", PageSize);
        cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
        return GetData(cmd, pageIndex).GetXml();
    }
    private static DataSet GetData(SqlCommand cmd, int pageIndex)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds, "Customers");
                    DataTable dt = new DataTable("Pager");
                    dt.Columns.Add("PageIndex");
                    dt.Columns.Add("PageSize");
                    dt.Columns.Add("RecordCount");
                    dt.Rows.Add();
                    dt.Rows[0]["PageIndex"] = pageIndex;
                    dt.Rows[0]["PageSize"] = PageSize;
                    dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
        }
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        string SearchValue = "";
        //SearchValue = Convert.ToString(txtSearch.Text);
        string frmDate = DateTime.Parse(txtFromDate.Text).ToString("yyyy-MM-dd");
        string ToDate = DateTime.Parse(txtToDate.Text).ToString("yyyy-MM-dd");
        fmdt = DateTime.Parse(txtFromDate.Text).ToString("yyyy-MM-dd");
        todt = DateTime.Parse(txtToDate.Text).ToString("yyyy-MM-dd");
        //DateTime dt = DateTime.ParseExact(txtToDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //string op = dt.ToString("yyyy-MM-dd");
        BindGrid(frmDate, ToDate);
    }

    private void BindGrid(string frmDt, string toDt)
    {



        string constr = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        SqlCommand cmd = new SqlCommand();

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.CommandText = "usp_IEReport";

        cmd.Connection = con;

        try
        {

            con.Open();
            if (frmDt != null)
            {

                //// string dateString = "25/06/2012";
                //string format = "mm/dd/yyyy";
                //DateTime fd = DateTime.ParseExact(frmDt, format, CultureInfo.InvariantCulture);
                //string fmd = fd.ToString("yyyy-mm-dd");
                //DateTime td = DateTime.ParseExact(toDt, format, CultureInfo.InvariantCulture);
                //string tod = td.ToString("yyyy-mm-dd");

                SqlParameter FromDate = cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(fmdt));
                SqlParameter ToDate = cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(toDt));
            }
            gvPocReport.EmptyDataText = "No Records Found";
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            Session["Datatableprocedure"] = dt;
            gvPocReport.DataSource = cmd.ExecuteReader();
            gvPocReport.DataBind();
            // con.Close();

        }

        catch (Exception ex)
        {



        }

        //finally
        //{

        //    con.Close();

        //    con.Dispose();

        //}
    }



    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Datatableprocedure"];


        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "IEReports");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=IEReport.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}