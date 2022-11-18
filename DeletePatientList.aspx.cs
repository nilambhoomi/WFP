using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeletePatientList : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        //if (Convert.ToString(Session["pageAccess"]).Contains("11"))
        //{
            if (!IsPostBack)
            {
                Session["patientFUId"] = "";
                BindPatientIEDetails();
               // bindLocation();
                txtSearch.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
            }
       // }
    }

    protected void BindPatientIEDetails(string patientId = null, string searchText = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetPatientIEDetails", con);

            if (!string.IsNullOrEmpty(patientId))
            {
                cmd.Parameters.AddWithValue("@Patient_Id", hfPatientId.Value);
            }
            else if (!string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(patientId))
            {
                string keyword = searchText.TrimStart(("Mrs. ").ToCharArray());
                cmd.Parameters.AddWithValue("@SearchText", keyword);
            }
            //else
            //{
            //    if (Session["Location"] != null)
            //    {
            //        cmd.Parameters.AddWithValue("@LocationId", Convert.ToString(Session["Location"]));
            //    }
            //}

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            string _query = "";
            DataRow row;

            if (ddl_location.SelectedIndex > 0)
            {
                if (string.IsNullOrEmpty(_query))
                {
                    _query = " Location_ID=" + ddl_location.SelectedItem.Value;
                }
                else
                    _query = _query + " and Location_ID=" + ddl_location.SelectedItem.Value;
            }
            //else
            //{
            //    if (string.IsNullOrEmpty(_query))
            //        _query = " Location_ID in (" + Session["Locations"].ToString() + ")";
            //    else
            //        _query = _query + " and Location_ID in (" + Session["Locations"].ToString() + ")";
            //}



            try
            {
                dt = dt.Select(_query).CopyToDataTable();
                DataView dv = dt.DefaultView;
                dv.Sort = "LastTestDate desc";
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                dt = null;
            }


            con.Close();
            Session["iedata"] = dt;


            gvPatientDetails.DataSource = dt;
            gvPatientDetails.DataBind();
            hfPatientId.Value = null;
        }
    }

    private void bindLocation()
    {
        DataSet ds = new DataSet();

        string query = "select Location,Location_ID from tblLocations ";
        if (!string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " where Is_Active = 1";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- All --", "0"));


        }

    }
    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        //BindPatientIEDetails();
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }
    protected void gvPatientFUDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvPatientFUDetails = (sender as GridView);
        hfCurrentlyOpened.Value = gvPatientFUDetails.ToolTip;
        gvPatientFUDetails.PageIndex = e.NewPageIndex;
        bindFUDetails(gvPatientFUDetails);
    }

    protected void bindFUDetails(GridView gvPatientFUDetails)
    {
        BusinessLogic bl = new BusinessLogic();
        gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(gvPatientFUDetails.ToolTip));
        gvPatientFUDetails.DataBind();
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string patientIEId = gvPatientDetails.DataKeys[e.Row.RowIndex].Value.ToString();
            BusinessLogic bl = new BusinessLogic();
            GridView gvPatientFUDetails = e.Row.FindControl("gvPatientFUDetails") as GridView;
            gvPatientFUDetails.ToolTip = patientIEId;
            gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(patientIEId));
            gvPatientFUDetails.DataBind();
        }
    }

    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        //BindPatientIEDetails();
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DeletePatientList.aspx");
    }

    protected void lnkDelete_FU_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientFUID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientFU", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientIEID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientIE", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}