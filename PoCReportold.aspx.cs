using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;
using ClosedXML.Excel;
using System.Globalization;




public partial class PoCReportold : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtSearchFromdate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void CustomValidator2_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtSearchTodate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void BindProcudureList()
    {
        string query = "select pm.sex,pm.LastName', 'pm.FirstName 'Name',ie.Compensation as 'Case' ,ISNULL(CONVERT(VARCHAR(10),pm.DOB,101),'') as DOB ,ISNULL(CONVERT(VARCHAR(10),ie.DOA,101),'') as DOA ,tp.MCODE,ISnull(tp.Sides,'') AS Sides,ISNULL(tp.Level,'') AS Level,pm.phone+','+pm.Phone2 as Phone,lc.location";
        string condition = " from  tblProceduresDetail tp  inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID";
        string condition1 = null;

        if (chkRequested.Checked)
        {
            query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";
            condition1 = " (CONVERT(VARCHAR(10),tp.Requested,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.createddate,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
        }
        if (chkScheduled.Checked)
        {
            query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
            if (!string.IsNullOrEmpty(condition1))
            {
                condition1 += condition1 = " OR (CONVERT(VARCHAR(10),tp.Scheduled,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.Scheduled,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
            else
            {

                condition1 = " (CONVERT(VARCHAR(10),tp.Scheduled,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.Scheduled,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }

        }
        if (chkExecuted.Checked)
        {
            query += ", ISNULL(CONVERT(VARCHAR(10),tp.Executed,101),'') as Executed ";
            if (!string.IsNullOrEmpty(condition1))
            {
                condition1 += condition1 = " OR (CONVERT(VARCHAR(10),tp.Executed,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.Executed,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
            else
            {

                condition1 = " (CONVERT(VARCHAR(10),tp.Executed,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.Executed,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
        }

        query += condition;
        if (!string.IsNullOrEmpty(condition1))
        {
            condition1 = condition1.Insert(0, " where ");
            query += condition1;
        }


        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
        {

            SqlCommand cm = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                gvProcedureTbl.DataSource = dt;
                Session["Datatableprocedure"] = dt;
                gvProcedureTbl.DataBind();
            }
        }
    }
    protected void lkExportToexcel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Datatableprocedure"];


        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "POCReport");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=POCReport.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }

    }
    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        ViewState["z_sortexpresion"] = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, "DESC");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, "ASC");
        }

    }

    public string SortExpression
    {
        get
        {
            if (ViewState["z_sortexpresion"] == null)
                ViewState["z_sortexpresion"] = this.gvProcedureTbl.DataKeyNames[0].ToString();
            return ViewState["z_sortexpresion"].ToString();
        }
        set
        {
            ViewState["z_sortexpresion"] = value;
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = ((DataTable)Session["Datatableprocedure"]);
        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + " " + direction;
        this.gvProcedureTbl.DataSource = dv;
        gvProcedureTbl.DataBind();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearchFromdate.Text = string.Empty;
        txtSearchTodate.Text = string.Empty;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProcudureList();
    }



}