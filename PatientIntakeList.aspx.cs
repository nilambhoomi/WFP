﻿using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO.Compression;
using System.Xml;
using System.Reflection;
using System.Globalization;
using IntakeSheet.DAL;
using IntakeSheet.Entity;

public partial class PatientIntakeList : System.Web.UI.Page
{
    public int iCounter = 1;
    public int iCounterSoap = 1;
    public int neck;
    public int Midback;
    public int lowback;
    public int Shoulder;
    public int Keen;
    public int Elbow;
    public int Wrist;
    public int ankle;
    public int Hip;
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["patientFUId"] = "";
            BindPatientIEDetails();
        
        }
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
            else
            {
                if (Session["Location"] != null)
                {
                    cmd.Parameters.AddWithValue("@LocationId", Convert.ToString(Session["Location"]));
                }
            }

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            Session["iedata"] = dt;
            gvPatientDetails.DataSource = dt;
            gvPatientDetails.DataBind();
            hfPatientId.Value = null;
        }
    }

    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails();
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
        gvPatientFUDetails.DataSource = Session["dtfu"] = bl.GetFUDetails(Convert.ToInt32(gvPatientFUDetails.ToolTip));
        gvPatientFUDetails.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string patientIEId = gvPatientDetails.DataKeys[e.Row.RowIndex].Value.ToString();
            BusinessLogic bl = new BusinessLogic();
            GridView gvPatientFUDetails = e.Row.FindControl("gvPatientFUDetails") as GridView;

            Image img = e.Row.FindControl("plusimg") as Image;



            gvPatientFUDetails.ToolTip = patientIEId;
            gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(patientIEId));
            gvPatientFUDetails.DataBind();

            if (gvPatientFUDetails.Rows.Count == 0)
                img.Attributes.Add("style", "display:none");
            else
                img.Attributes.Add("style", "display:block");
        }
    }

    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails();
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Session["PatientIE_ID"] = null;
        Response.Redirect("Page1.aspx");
    }

    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        Response.Redirect("Page1.aspx?id=" + btn.CommandArgument);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PatientIntakeList.aspx");
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatus(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        //if (filePaths.Length <= 0)
        //    return "";
        //using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        //{
        //    foreach (string filePath in filePaths)
        //    {
        //        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        //        archive.CreateEntryFromFile(filePath, filename);
        //    }
        //}

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownload(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatusRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownloadRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString() + "/ROD";
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatusSoap(string flag, Int64 id, Int64 soapid)
    {
        string tempFileName = "_" + id + "_" + flag + "_Soap";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusSoap";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@soapID", soapid);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownloadSoap(string flag, Int64 id, Int64 soapid)
    {
        string tempFileName = "_" + soapid + "_Soap";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString() + "/Soap";
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + soapid + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + soapid + "_*.*");
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusSoap";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@soapID", soapid);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }

    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvPatientDetails.PageSize = Convert.ToInt16(ddlPage.SelectedItem.Value);
        BindPatientIEDetails();
    }
    private void BindRODDeafultValues(DataView dv, bool IsFromFU = false)
    {
        try
        {
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Default_Rod.xml"));
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();
            if (dv != null)
            {

                string clause = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/XML/Clause.xml"));
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Clauses");
                foreach (XmlNode node in nodeList)
                {
                    if (!IsFromFU)
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])).InnerText;
                    }
                    else
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])).InnerText;
                    }

                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Name"].ToString().Contains("##Name##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[1]) + " " + Convert.ToString(dv[0].Row.ItemArray[3]) + " " + Convert.ToString(dv[0].Row.ItemArray[2])).Replace("##IEdate##", Convert.ToString(!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[9]))?Convert.ToDateTime(dv[0].Row.ItemArray[9]).ToString("MM/dd/yyyy"):string.Empty)).Replace("##DOA##", Convert.ToString(!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[5]))?Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy"):string.Empty)).Replace("##cause##", clause).Replace("##FUVisitdate##", " ___(last DOS)___ ");
                            }
                            else
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[6]) + " " + Convert.ToString(dv[0].Row.ItemArray[4]) + " " + Convert.ToString(dv[0].Row.ItemArray[5])).Replace("##FUVisitdate##", Convert.ToString(!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[7]))?Convert.ToDateTime(dv[0].Row.ItemArray[7]).ToString("MM/dd/yyyy"):string.Empty)).Replace("##DOA##", Convert.ToString(!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[11]))?Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy"):string.Empty)).Replace("##cause##", clause).Replace("##IEdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[12]).ToString("MM/dd/yyyy")));
                            }
                            row.SetField("Name", temp);
                        }
                        else if (row["Name"].ToString().Contains("##DOA##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                if(!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[5])))
                                {
                                    temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy")));
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[11])))
                                {
                                    temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy")));
                                }
                            }
                            row.SetField("Name", temp);
                        }
                    }

                    repRoD.DataSource = ds.Tables[0];
                    repRoD.DataBind();

                }
            }
        }
        catch (Exception)
        {

            throw;
        }


    }
    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtrodcreatedate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }

    protected void CustomValidator4_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtCreateSoapDate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void btnrodsave_Click(object sender, EventArgs e)
    {
        DateTime d;
        DateTime.TryParseExact(txtrodcreatedate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        //string id = hdnrodieid.Value;
        SqlConnection oSQLConn = new SqlConnection();
        SqlCommand oSQLCmd = new SqlCommand();
        string _ieID = Convert.ToString(hdnrodieid.Value);
        string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
        string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + _ieID + " and Patiend_FUID IS NULL";
        }
        else
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + Convert.ToInt64(_fuieid) + " and Patiend_FUID = " + Convert.ToInt64(_fufuid);
        }

        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0)
            _ieMode = "Update";
        else
            _ieMode = "Delete";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["patietn_IE"] = !string.IsNullOrEmpty(_ieID) ? _ieID : _fuieid;

            if (!string.IsNullOrEmpty(_fufuid))
            {
                TblRow["Patiend_FUID"] = _fufuid;
            }

            TblRow["Content"] = txtrodFulldetails.Text;
            TblRow["Contentdelimit"] = bindRodPrintvalue();
            TblRow["Bodypartdetails"] = hdbodyparts.Value;
            TblRow["Newlinedetails"] = hdnewline.Value;
            TblRow["Plandetails"] = "test";
            TblRow["Plandelimit"] = "test";
            TblRow["Clientnote"] = "test";
            TblRow["Signpath"] = "test";
            TblRow["MAProvider"] = txtMAProviderrod.SelectedItem.Text;
            TblRow["CreateDate"] = Convert.ToDateTime(d);

            if (_ieMode == "New")
            {
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();





        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _ieID + "-" + txtMAProviderrod.SelectedItem.Text;
            lnkierod_Click(btn, e);

        }
        else
        {

            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _fufuid + "-" + _fuieid + "~" + txtMAProviderrod.SelectedItem.Text;
            lnkfurod_Click(btn, e);
        }

    }
    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    protected void txtRod_TextChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    private string bindRodPrintvalue()
    {
        string str = "";
        string strDelimit = "";
        string bodypart = string.Empty;
        string strbp = string.Empty, strnewline = string.Empty;
        for (int i = 0; i < repRoD.Items.Count; i++)
        {

            TextBox txt = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
            CheckBox chk = repRoD.Items[i].FindControl("chk") as CheckBox;
            HiddenField hdbodypart = repRoD.Items[i].FindControl("bodypart") as HiddenField;
            HiddenField hdisnewline = repRoD.Items[i].FindControl("isnewline") as HiddenField;
            if (chk.Checked)
            {
                if (hdisnewline.Value == "1")
                    str = str + @"\n" + txt.Text;
                else if (hdisnewline.Value == "2")
                    str = str + @"\n\n" + txt.Text;
                else
                    str = str + txt.Text;

                strDelimit = strDelimit + "^" + txt.Text;
                bodypart += hdbodypart.Value + ",";
            }
            else
            {

                // str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "^@" + txt.Text;
            }


            if (string.IsNullOrEmpty(strbp))
                strbp = hdbodypart.Value + ",";
            else
                strbp += hdbodypart.Value + ",";

            if (string.IsNullOrEmpty(strnewline))
                strnewline = hdisnewline.Value + ",";
            else
                strnewline += hdisnewline.Value + ",";

        }
        //foreach (var v in bodypart.Split(','))
        //{
        //    for (int i = 0; i < repRoD.Items.Count; i++)
        //    {
        //        TextBox txt1 = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
        //        CheckBox chk1 = repRoD.Items[i].FindControl("chk") as CheckBox;
        //        HiddenField hdbodypart1 = repRoD.Items[i].FindControl("bodypart") as HiddenField;

        //        if (v.Split('-')[0].Equals(Convert.ToString(hdbodypart1.Value).Split('-')[0]) && !chk1.Checked && v.Split('-').Count() > 1)
        //        {
        //            if (v.Split('-')[1] != hdbodypart1.Value.Split('-')[1] && hdbodypart1.Value.Split('-')[1].Equals("p"))
        //            {
        //                chk1.Checked = !chk1.Checked;
        //            }

        //        }
        //        if (v.Equals(hdbodypart1.Value) && chk1.Checked)
        //        {
        //            chk1.Checked = chk1.Checked;
        //        }
        //    }
        //}

        txtrodFulldetails.Text = str;

        strDelimit = strDelimit.TrimStart('^');
        hdbodyparts.Value = strbp;
        hdnewline.Value = strnewline;

        return strDelimit;
    }
    protected void lnkiesoap_Click1(object sender, EventArgs e)
    {
        try
        {
            btnCreatnewFu.Visible = false;
            btnCreatnew.Visible = true;

            LinkButton btn = (LinkButton)(sender);
            btnCreatnew.CommandArgument = Convert.ToString(btn.CommandArgument) + "|" + "0";
            //check for the value available or not in the soap table.
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select LEFT(DOS,9) AS DOS,* from tblSoap where PatientIE_ID = " + btn.CommandArgument + " and PatientFU_ID is null");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEditSoap.DataSource = ds;
                gvEditSoap.DataBind();
                gvEditSoap.Visible = true;
                lblRecordnotfound.Visible = false;
            }
            else
            {
                gvEditSoap.Visible = false;
                lblRecordnotfound.Visible = true;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoapEditSoap();", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void lnkierod_Click(object sender, EventArgs e)
    {
        try
        {

            hdnrodeditedfuid.Value = "";
            hdnrodeditedfuieid.Value = "";
            hdnrodieid.Value = "";

            string maprovider = string.Empty;
            LinkButton btn = (LinkButton)(sender);
            DataTable dt = (DataTable)(Session["iedata"]);
            if (btn.CommandArgument.Split('-').Count() > 0)
            {
                hdnrodieid.Value = btn.CommandArgument.Split('-')[0];
            }
            if (btn.CommandArgument.Split('-').Count() > 1)
            {
                maprovider = btn.CommandArgument.Split('-')[1];
            }

            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientIE_ID=" + Convert.ToInt32(hdnrodieid.Value); // query example = "id = 10"

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where patietn_IE= " + hdnrodieid.Value + " and Patiend_FUID is null");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";


            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv);
                btnRODDelete.Visible = false;
                txtrodcreatedate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                GetMAandProviders();
                if (!string.IsNullOrEmpty(Convert.ToString(txtMAProviderrod.Items.FindByText(maprovider))))
                {
                    txtMAProviderrod.Items.FindByText(maprovider).Selected = true;
                }

            }
            else
            {

                txtrodcreatedate.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CreateDate"])) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateDate"]).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();
                //txtMAProviderrod.Text = Convert.ToString(ds.Tables[0].Rows[0]["MAProvider"]);
                GetMAandProviders();
                if (!string.IsNullOrEmpty(Convert.ToString(txtMAProviderrod.Items.FindByText(ds.Tables[0].Rows[0]["MAProvider"].ToString()))))
                {
                    txtMAProviderrod.Items.FindByText(ds.Tables[0].Rows[0]["MAProvider"].ToString()).Selected = true;
                }

                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }

            bindRodPrintvalue();
            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(hdnrodieid.Value) + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(hdnrodieid.Value) + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception ex)
        {
            throw;
        }

    }
    protected void lnkfurod_Click(object sender, EventArgs e)
    {
        try
        {
            string maprovider = string.Empty;

            hdnrodeditedfuid.Value = "";
            hdnrodeditedfuieid.Value = "";

            BusinessLogic bl = new BusinessLogic();
            LinkButton btn = (LinkButton)(sender);
            if (btn.CommandArgument.Split('-').Count() > 0)
            {
                hdnrodeditedfuid.Value = btn.CommandArgument.Split('-')[0];
            }
            if (btn.CommandArgument.Split('-').Count() > 1)
            {
                hdnrodeditedfuieid.Value = btn.CommandArgument.Split('-')[1].Split('~')[0];
            }
            if (btn.CommandArgument.Split('~').Count() > 1)
            {
                maprovider = btn.CommandArgument.Split('~')[1];
            }



            DataTable dt = ToDataTable(bl.GetFUDetails(Convert.ToInt32(hdnrodeditedfuieid.Value)));
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientFUId=" + Convert.ToInt32(hdnrodeditedfuid.Value); // query example = "id = 10"
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where Patiend_FUID= " + hdnrodeditedfuid.Value + " and patietn_ie=" + hdnrodeditedfuieid.Value);
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";

            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv, true);
                btnRODDelete.Visible = false;
                txtrodcreatedate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                GetMAandProviders();
                if (!string.IsNullOrEmpty(Convert.ToString(txtMAProviderrod.Items.FindByText(maprovider))))
                {
                    txtMAProviderrod.Items.FindByText(maprovider).Selected = true;
                }
            }
            else
            {
                txtrodcreatedate.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CreateDate"])) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateDate"]).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();
                //txtMAProviderrod.Text = Convert.ToString(ds.Tables[0].Rows[0]["MAProvider"]);
                GetMAandProviders();
                if (!string.IsNullOrEmpty(Convert.ToString(txtMAProviderrod.Items.FindByText(ds.Tables[0].Rows[0]["MAProvider"].ToString()))))
                {
                    txtMAProviderrod.Items.FindByText(ds.Tables[0].Rows[0]["MAProvider"].ToString()).Selected = true;
                }
                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }


            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Defining type of data column gives proper data table 
            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name, type);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }
    private void BindRODEditValues(string val, string bpstr, string newlinestr)
    {
        try
        {
            if (!string.IsNullOrEmpty(val))
            {
                string[] str = val.Split('^');
                string[] strbp = bpstr.Split(',');
                string[] strnl = newlinestr.Split(',');

                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)),
            new DataColumn("bodypart", typeof(string)),
              new DataColumn("isnewline", typeof(string))});

                for (int i = 0; i < str.Length; i++)
                {
                    dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'), strbp[i], strnl[i]);
                    // dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", string.IsNullOrEmpty(str[i]) ? str[i] : str[i].TrimStart('@'));
                }

                repRoD.DataSource = dt;
                repRoD.DataBind();
                bindRodPrintvalue();
                //  bindTeratMentPrintvalue();

            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnRODDelete_Click(object sender, EventArgs e)
    {
        DBHelperClass dB = new DBHelperClass();
        int val = dB.executeQuery("delete from tblROD where id=" + ViewState["rodid"].ToString());
        if (val > 0)
        {
            string _ieID = Convert.ToString(hdnrodieid.Value);
            string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
            string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

            //if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
            //{
            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _ieID;
            //    lnkierod_Click(btn, e);

            //}
            //else
            //{

            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _fufuid + "-" + _fuieid;
            //    lnkfurod_Click(btn, e);
            //}
        }
    }
    public List<string> getFUInjuredParts(Int64 PatientFU_ID)
    {
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientFU_ID", PatientFU_ID));
        DataTable _dt = _dal.getDataTable("nusp_GetFUInjuredBodyParts", param);
        IntakeSheet.Entity.BodyParts _bodyparts = new IntakeSheet.Entity.BodyParts();
        DataRow _newdr = _dt.NewRow();
        foreach (DataRow dr in _dt.Rows)
        {
            _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
            _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
            _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
            _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
            _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
            _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
            _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
            _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
            _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
            _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
            _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
            _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
            _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
            _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
            _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
            _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
            _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
            _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
            _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
            _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
            _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
            _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
            _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
            _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
            _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
            _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
            _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
            _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
            _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
        }
        _dt.Rows.Clear();
        if (_bodyparts != null)
        {
            _newdr["Neck"] = _bodyparts.Neck;
            _newdr["MidBack"] = _bodyparts.MidBack;
            _newdr["LowBack"] = _bodyparts.LowBack;
            _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
            _newdr["RightShoulder"] = _bodyparts.RightShoulder;
            _newdr["LeftKnee"] = _bodyparts.LeftKnee;
            _newdr["RightKnee"] = _bodyparts.RightKnee;
            _newdr["LeftElbow"] = _bodyparts.LeftElbow;
            _newdr["RightElbow"] = _bodyparts.RightElbow;
            _newdr["LeftWrist"] = _bodyparts.LeftWrist;
            _newdr["RightWrist"] = _bodyparts.RightWrist;
            _newdr["LeftHip"] = _bodyparts.LeftHip;
            _newdr["RightHip"] = _bodyparts.RightHip;
            _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
            _newdr["RightAnkle"] = _bodyparts.RightAnkle;
            _newdr["LeftArm"] = _bodyparts.LeftArm;
            _newdr["RightArm"] = _bodyparts.RightArm;
            _newdr["LeftHand"] = _bodyparts.LeftHand;
            _newdr["RightHand"] = _bodyparts.RightHand;
            _newdr["LeftLeg"] = _bodyparts.LeftLeg;
            _newdr["RightLeg"] = _bodyparts.RightLeg;
            _newdr["LeftFoot"] = _bodyparts.LeftFoot;
            _newdr["RightFoot"] = _bodyparts.RightFoot;
            _newdr["Face"] = _bodyparts.Face;
            _newdr["Ribs"] = _bodyparts.Ribs;
            _newdr["Chest"] = _bodyparts.Chest;
            _newdr["Abdomen"] = _bodyparts.Abdomen;
            _newdr["Pelvis"] = _bodyparts.Pelvis;
            _newdr["Others"] = _bodyparts.Other;

        }
        _dt.Rows.Add(_newdr);
        List<string> _injuredParts = new List<string>();
        foreach (DataRow dr in _dt.Rows)
        {
            foreach (DataColumn dc in _dt.Columns)
            {
                if (Convert.ToBoolean(dr[dc]))
                {
                    _injuredParts.Add(dc.ColumnName);
                }
            }
        }


        return _injuredParts;

    }
    public List<string> getIEInjuredParts(Int64 PatientIE_ID)
    {
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientIE_ID", PatientIE_ID));
        DataTable _dt = _dal.getDataTable("nusp_GetIEInjuredBodyParts", param);
        IntakeSheet.Entity.BodyParts _bodyparts = new IntakeSheet.Entity.BodyParts();
        DataRow _newdr = _dt.NewRow();
        foreach (DataRow dr in _dt.Rows)
        {
            _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
            _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
            _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
            _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
            _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
            _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
            _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
            _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
            _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
            _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
            _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
            _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
            _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
            _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
            _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
            _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
            _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
            _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
            _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
            _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
            _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
            _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
            _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
            _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
            _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
            _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
            _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
            _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
            _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
        }
        _dt.Rows.Clear();
        if (_bodyparts != null)
        {
            _newdr["Neck"] = _bodyparts.Neck;
            _newdr["MidBack"] = _bodyparts.MidBack;
            _newdr["LowBack"] = _bodyparts.LowBack;
            _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
            _newdr["RightShoulder"] = _bodyparts.RightShoulder;
            _newdr["LeftKnee"] = _bodyparts.LeftKnee;
            _newdr["RightKnee"] = _bodyparts.RightKnee;
            _newdr["LeftElbow"] = _bodyparts.LeftElbow;
            _newdr["RightElbow"] = _bodyparts.RightElbow;
            _newdr["LeftWrist"] = _bodyparts.LeftWrist;
            _newdr["RightWrist"] = _bodyparts.RightWrist;
            _newdr["LeftHip"] = _bodyparts.LeftHip;
            _newdr["RightHip"] = _bodyparts.RightHip;
            _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
            _newdr["RightAnkle"] = _bodyparts.RightAnkle;
            _newdr["LeftArm"] = _bodyparts.LeftArm;
            _newdr["RightArm"] = _bodyparts.RightArm;
            _newdr["LeftHand"] = _bodyparts.LeftHand;
            _newdr["RightHand"] = _bodyparts.RightHand;
            _newdr["LeftLeg"] = _bodyparts.LeftLeg;
            _newdr["RightLeg"] = _bodyparts.RightLeg;
            _newdr["LeftFoot"] = _bodyparts.LeftFoot;
            _newdr["RightFoot"] = _bodyparts.RightFoot;
            _newdr["Face"] = _bodyparts.Face;
            _newdr["Ribs"] = _bodyparts.Ribs;
            _newdr["Chest"] = _bodyparts.Chest;
            _newdr["Abdomen"] = _bodyparts.Abdomen;
            _newdr["Pelvis"] = _bodyparts.Pelvis;
            _newdr["Others"] = _bodyparts.Other;

        }
        _dt.Rows.Add(_newdr);
        List<string> _injuredParts = new List<string>();
        foreach (DataRow dr in _dt.Rows)
        {
            foreach (DataColumn dc in _dt.Columns)
            {
                if (Convert.ToBoolean(dr[dc]))
                {
                    _injuredParts.Add(dc.ColumnName);
                }
            }
        }


        return _injuredParts;

    }
    protected void lnkiesoap_Click(object sender, EventArgs e)
    {
        try
        {
            hdnrodeditedfuid.Value = string.Empty;
            hdnrodeditedfuieid.Value = string.Empty;
            hdnSoapId.Value = string.Empty;

            string SoapId = hdnrodieid.Value = hdnSoapId.Value = string.Empty;
            LinkButton btn = (LinkButton)(sender);
            DataTable dt = (DataTable)(Session["iedata"]);

            if (btn.CommandArgument.Split('|').Count() > 1)
            {
                hdnrodieid.Value = btn.CommandArgument.Split('|')[0];
                hdnSoapId.Value = btn.CommandArgument.Split('|')[1];
            }

            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientIE_ID=" + Convert.ToInt32(hdnrodieid.Value); // query example = "id = 10"

            Session["ieid"] = btn.CommandArgument;
            List<string> _injured = getIEInjuredParts(Convert.ToInt64(hdnrodieid.Value));
            ViewState["Injuredbodypart"] = _injured;
            //check for the value available or not in the soap table.

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblSoap where PatientIE_ID= " + hdnrodieid.Value + " and PatientFU_ID is null and ID =" + hdnSoapId.Value);
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            txtothertext.Text = string.Empty;

            if (ds.Tables[0].Rows.Count == 0)
            {
                if (dv != null)
                {
                    lblName.Text = Convert.ToString(dv[0].Row.ItemArray[2]) + " " + Convert.ToString(dv[0].Row.ItemArray[3]);//Last name +First Name;
                    lblDOI.Text = !String.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[5])) ? Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy") : string.Empty;//DOA
                    txtAdjective.Text = Convert.ToString(dv[0].Row.ItemArray[13]);

                }
                txtCreateSoapDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                bindSoapBodyPart(_injured, true);
                BindSoapAsstPlan();

                bool ShowInitial = false;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/xml/Default_Soap.xml"));
                XmlNodeList nodeList;
                nodeList = xmlDoc.DocumentElement.SelectNodes("/Soaps");
                foreach (XmlNode node in nodeList)
                {
                    ShowInitial = Convert.ToBoolean(node.SelectSingleNode("ShowInitial").InnerText);
                }
                if (ShowInitial)
                {
                    string MP = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["Providers"])))
                    {
                        if (Convert.ToString(Session["Providers"]).Split(' ').Count() > 0)
                        {
                            MP = Convert.ToString(Session["Providers"]).Split(' ')[0].Substring(0, 1);
                        }

                        if (Convert.ToString(Session["Providers"]).Split(' ').Count() > 1)
                        {
                            MP += Convert.ToString(Session["Providers"]).Split(' ')[1].Substring(0, 1);
                        }
                    }
                    txtMAProvider.Text = MP;
                }
                else
                {
                    txtMAProvider.Text = Convert.ToString(Session["Providers"]);
                }
            }
            else
            {
                clearcontrolSoap();
                bindSoapBodyPart(_injured, false);
                txtCreateSoapDate.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CreationDate"])) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
                lblName.Text = Convert.ToString(ds.Tables[0].Rows[0]["PatientName"]);
                lblDOI.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DOI"])) ? Convert.ToString(ds.Tables[0].Rows[0]["DOI"]) : string.Empty;
                txtAdjective.Text = Convert.ToString(ds.Tables[0].Rows[0]["Subjective"]);
                txtMAProvider.Text = Convert.ToString(ds.Tables[0].Rows[0]["MAProvider"]);
                string temp = Convert.ToString(ds.Tables[0].Rows[0]["Objective"]);

                foreach (var item in temp.Split('$'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        foreach (var item1 in item.Split('|'))
                        {
                            if (item1.Contains('-'))
                            {
                                string chk1;
                                if (item1.Contains("-chk"))
                                {
                                    chk1 = item1.Split('-')[1] + item1.Split('-')[0];
                                }
                                else
                                {
                                    chk1 = "chk" + item1.Split('-')[1] + item1.Split('-')[0];
                                }


                                //foreach (Control child in pnlCheckbox.Controls)
                                //{
                                //    if (child is RadioButton)
                                //    {
                                //        RadioButton chk = child as RadioButton;
                                //        if (chk.ID == chk1)
                                //        {
                                //            chk.Checked = true;
                                //        }
                                //    }
                                //}
                                selectbodypart(chk1);
                            }
                        }
                    }
                }

                string Treatment = Convert.ToString(ds.Tables[0].Rows[0]["Treatment"]);

                foreach (var item in Treatment.Split('$'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {

                        if (item.Split('-')[0].Equals("Other10"))
                        {
                            txtothertext.Text = item.Split('-')[1];
                        }
                        else
                        {
                            string checkboxlist = "chk" + item.Split('-')[0];
                            string selectedvalues = item.Split('-')[1];
                            bindEditTreatment(checkboxlist, selectedvalues);
                        }
                    }

                }

                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Assessment"])))
                {
                    BindSoapAssestPlanEditValues(repAssestment, Convert.ToString(ds.Tables[0].Rows[0]["Assessment"]));
                }
                else
                { bindSoapAssestPlanPrintvalue(repAssestment, "txtAssestment", hdnAssestment); }
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Plans"])))
                {
                    BindSoapAssestPlanEditValues(repPlan, Convert.ToString(ds.Tables[0].Rows[0]["Plans"]));
                }
                else
                { bindSoapAssestPlanPrintvalue(repPlan, "txtPlan", hdnPlan); }

            }

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoap();", true);
        }
        catch (Exception ex)
        {
            db.LogError(ex);
            throw;
        }
    }
    public void bindEditTreatment(string CheckBoxname, string selectedvalues)
    {
        try
        {
            if (!string.IsNullOrEmpty(selectedvalues))
            {
                if (chkModalities1.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkModalities1.Items.FindByText(item1) != null)
                            {
                                chkModalities1.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }

                if (chkModalities2.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkModalities2.Items.FindByText(item1) != null)
                            {
                                chkModalities2.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }

                if (chkModalities3.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkModalities3.Items.FindByText(item1) != null)
                            {
                                chkModalities3.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkModalities4.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkModalities4.Items.FindByText(item1) != null)
                            {
                                chkModalities4.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                //Manualtreatment
                if (chkManualtreatment1.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkManualtreatment1.Items.FindByText(item1) != null)
                            {
                                chkManualtreatment1.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkManualtreatment2.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkManualtreatment2.Items.FindByText(item1) != null)
                            {
                                chkManualtreatment2.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkManualtreatment3.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkManualtreatment3.Items.FindByText(item1) != null)
                            {
                                chkManualtreatment3.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkManualtreatment4.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkManualtreatment4.Items.FindByText(item1) != null)
                            {
                                chkManualtreatment4.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }

                //Other
                if (chkOther1.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther1.Items.FindByText(item1) != null)
                            {
                                chkOther1.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther2.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther2.Items.FindByText(item1) != null)
                            {
                                chkOther2.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther3.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther3.Items.FindByText(item1) != null)
                            {
                                chkOther3.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther4.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther4.Items.FindByText(item1) != null)
                            {
                                chkOther4.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther5.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther5.Items.FindByText(item1) != null)
                            {
                                chkOther5.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther6.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther6.Items.FindByText(item1) != null)
                            {
                                chkOther6.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther7.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther7.Items.FindByText(item1) != null)
                            {
                                chkOther7.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther8.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther8.Items.FindByText(item1) != null)
                            {
                                chkOther8.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
                if (chkOther9.ID.Equals(CheckBoxname))
                {
                    foreach (var item1 in selectedvalues.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            if (chkOther9.Items.FindByText(item1) != null)
                            {
                                chkOther9.Items.FindByText(item1).Selected = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public void selectbodypart(string chk1)
    {
        try
        {
            //neck
            if (chkIncreaseflexionNeck.ID.Equals(chk1))
            { chkIncreaseflexionNeck.Checked = true; }

            if (chkDecreaseflexionNeck.ID.Equals(chk1))
            { chkDecreaseflexionNeck.Checked = true; }

            if (chkIncreaseextensionNeck.ID.Equals(chk1))
            { chkIncreaseextensionNeck.Checked = true; }

            if (chkDecreaseextensionNeck.ID.Equals(chk1))
            { chkDecreaseextensionNeck.Checked = true; }

            if (chkIncreasetilttoleftNeck.ID.Equals(chk1))
            { chkIncreasetilttoleftNeck.Checked = true; }

            if (chkDecreasetilttoleftNeck.ID.Equals(chk1))
            { chkDecreasetilttoleftNeck.Checked = true; }

            if (chkIncreasetilttorightNeck.ID.Equals(chk1))
            { chkIncreasetilttorightNeck.Checked = true; }

            if (chkDecreasetilttorightNeck.ID.Equals(chk1))
            { chkDecreasetilttorightNeck.Checked = true; }

            if (chkIncreaseleftrotationNeck.ID.Equals(chk1))
            { chkIncreaseleftrotationNeck.Checked = true; }

            if (chkDecreaseleftrotationNeck.ID.Equals(chk1))
            { chkDecreaseleftrotationNeck.Checked = true; }

            if (chkIncreaseRightrotationNeck.ID.Equals(chk1))
            { chkIncreaseRightrotationNeck.Checked = true; }

            if (chkDecreaseRightrotationNeck.ID.Equals(chk1))
            { chkDecreaseRightrotationNeck.Checked = true; }

            if (chkspasmNeck.ID.Equals(chk1))
            { chkspasmNeck.Checked = true; }

            if (chkIncreaseflexionLumber.ID.Equals(chk1))
            { chkIncreaseflexionLumber.Checked = true; }
            if (chkDecreaseflexionLumber.ID.Equals(chk1))
            { chkDecreaseflexionLumber.Checked = true; }

            if (chkIncreaseextensionLumber.ID.Equals(chk1))
            { chkIncreaseextensionLumber.Checked = true; }

            if (chkDecreaseextensionLumber.ID.Equals(chk1))
            { chkDecreaseextensionLumber.Checked = true; }

            if (chkIncreasetilttoleftLumber.ID.Equals(chk1))
            { chkIncreasetilttoleftLumber.Checked = true; }

            if (chkDecreasetilttoleftLumber.ID.Equals(chk1))
            { chkDecreasetilttoleftLumber.Checked = true; }

            if (chkIncreasetilttorightLumber.ID.Equals(chk1))
            { chkIncreasetilttorightLumber.Checked = true; }

            if (chkDecreasetilttorightLumber.ID.Equals(chk1))
            { chkDecreasetilttorightLumber.Checked = true; }

            if (chkspasmLumber.ID.Equals(chk1))
            { chkspasmLumber.Checked = true; }



            if (chkIncreasemildlyTrousic.ID.Equals(chk1))
            { chkIncreasemildlyTrousic.Checked = true; }
            if (chkDecreasemildlyTrousic.ID.Equals(chk1))
            { chkDecreasemildlyTrousic.Checked = true; }
            if (chkIncreasemoderatelyTrousic.ID.Equals(chk1))
            { chkIncreasemoderatelyTrousic.Checked = true; }
            if (chkDecreasemoderatelyTrousic.ID.Equals(chk1))
            { chkDecreasemoderatelyTrousic.Checked = true; }
            if (chkincreaseseverelyTrousic.ID.Equals(chk1))
            { chkincreaseseverelyTrousic.Checked = true; }
            if (chkDecreaseseverelyTrousic.ID.Equals(chk1))
            { chkDecreaseseverelyTrousic.Checked = true; }
            if (chkspasmTrousic.ID.Equals(chk1))
            { chkspasmTrousic.Checked = true; }

            if (chkIncreaseflexionRK.ID.Equals(chk1))
            { chkIncreaseflexionRK.Checked = true; }
            if (chkDecreaseflexionRK.ID.Equals(chk1))
            { chkDecreaseflexionRK.Checked = true; }
            if (chkIncreaseextensionRK.ID.Equals(chk1))
            { chkIncreaseextensionRK.Checked = true; }
            if (chkDecreaseextensionRK.ID.Equals(chk1))
            { chkDecreaseextensionRK.Checked = true; }

            if (chkIncreaseflexionLK.ID.Equals(chk1))
            { chkIncreaseflexionLK.Checked = true; }
            if (chkDecreaseflexionLK.ID.Equals(chk1))
            { chkDecreaseflexionLK.Checked = true; }
            if (chkIncreaseextensionLK.ID.Equals(chk1))
            { chkIncreaseextensionLK.Checked = true; }
            if (chkDecreaseextensionLK.ID.Equals(chk1))
            { chkDecreaseextensionLK.Checked = true; }



            if (chkIncreaseflexionRSh.ID.Equals(chk1))
            { chkIncreaseflexionRSh.Checked = true; }
            if (chkDecreaseflexionRSh.ID.Equals(chk1))
            { chkDecreaseflexionRSh.Checked = true; }
            if (chkIncreaseabductionRSh.ID.Equals(chk1))
            { chkIncreaseabductionRSh.Checked = true; }
            if (chkDecreaseabductionRSh.ID.Equals(chk1))
            { chkDecreaseabductionRSh.Checked = true; }
            if (chkIncreaseintrotationRSh.ID.Equals(chk1))
            { chkIncreaseintrotationRSh.Checked = true; }
            if (chkDecreaseintrotationRSh.ID.Equals(chk1))
            { chkDecreaseintrotationRSh.Checked = true; }
            if (chkIncreaseextrotationRSh.ID.Equals(chk1))
            { chkIncreaseextrotationRSh.Checked = true; }
            if (chkDecreaseextrotationRSh.ID.Equals(chk1))
            { chkDecreaseextrotationRSh.Checked = true; }

            if (chkIncreaseflexionLSh.ID.Equals(chk1))
            { chkIncreaseflexionLSh.Checked = true; }
            if (chkDecreaseflexionLSh.ID.Equals(chk1))
            { chkDecreaseflexionLSh.Checked = true; }
            if (chkIncreaseabductionLSh.ID.Equals(chk1))
            { chkIncreaseabductionLSh.Checked = true; }
            if (chkDecreaseabductionLSh.ID.Equals(chk1))
            { chkDecreaseabductionLSh.Checked = true; }
            if (chkIncreaseintrotationLSh.ID.Equals(chk1))
            { chkIncreaseintrotationLSh.Checked = true; }
            if (chkDecreaseintrotationLSh.ID.Equals(chk1))
            { chkDecreaseintrotationLSh.Checked = true; }
            if (chkIncreaseextrotationLSh.ID.Equals(chk1))
            { chkIncreaseextrotationLSh.Checked = true; }
            if (chkDecreaseextrotationLSh.ID.Equals(chk1))
            { chkDecreaseextrotationLSh.Checked = true; }



            if (chkIncreaseflexionRE.ID.Equals(chk1)) { chkIncreaseflexionRE.Checked = true; }
            if (chkDecreaseflexionRE.ID.Equals(chk1)) { chkDecreaseflexionRE.Checked = true; }
            if (chkIncreaseextensionRE.ID.Equals(chk1)) { chkIncreaseextensionRE.Checked = true; }
            if (chkDecreaseextensionRE.ID.Equals(chk1)) { chkDecreaseextensionRE.Checked = true; }

            if (chkIncreaseflexionLE.ID.Equals(chk1)) { chkIncreaseflexionLE.Checked = true; }
            if (chkDecreaseflexionLE.ID.Equals(chk1)) { chkDecreaseflexionLE.Checked = true; }
            if (chkIncreaseextensionLE.ID.Equals(chk1)) { chkIncreaseextensionLE.Checked = true; }
            if (chkDecreaseextensionLE.ID.Equals(chk1)) { chkDecreaseextensionLE.Checked = true; }


            if (chkIncreasepalmarflexionRW.ID.Equals(chk1)) { chkIncreasepalmarflexionRW.Checked = true; }
            if (chkDecreasepalmarflexionRW.ID.Equals(chk1)) { chkDecreasepalmarflexionRW.Checked = true; }
            if (chkIncreasedorsiflexionRW.ID.Equals(chk1)) { chkIncreasedorsiflexionRW.Checked = true; }
            if (chkDecreasedorsiflexionRW.ID.Equals(chk1)) { chkDecreasedorsiflexionRW.Checked = true; }
            if (chkIncreaseulnardeviationRW.ID.Equals(chk1)) { chkIncreaseulnardeviationRW.Checked = true; }
            if (chkDecreaseulnardeviationRW.ID.Equals(chk1)) { chkDecreaseulnardeviationRW.Checked = true; }
            if (chkIncreaseradialdeviationRW.ID.Equals(chk1)) { chkIncreaseradialdeviationRW.Checked = true; }
            if (chkDecreaseradialdeviationRW.ID.Equals(chk1)) { chkDecreaseradialdeviationRW.Checked = true; }


            if (chkIncreasepalmarflexionLW.ID.Equals(chk1)) { chkIncreasepalmarflexionLW.Checked = true; }
            if (chkDecreasepalmarflexionLW.ID.Equals(chk1)) { chkDecreasepalmarflexionLW.Checked = true; }
            if (chkIncreasedorsiflexionLW.ID.Equals(chk1)) { chkIncreasedorsiflexionLW.Checked = true; }
            if (chkDecreasedorsiflexionLW.ID.Equals(chk1)) { chkDecreasedorsiflexionLW.Checked = true; }
            if (chkIncreaseulnardeviationLW.ID.Equals(chk1)) { chkIncreaseulnardeviationLW.Checked = true; }
            if (chkDecreaseulnardeviationLW.ID.Equals(chk1)) { chkDecreaseulnardeviationLW.Checked = true; }
            if (chkIncreaseradialdeviationLW.ID.Equals(chk1)) { chkIncreaseradialdeviationLW.Checked = true; }
            if (chkDecreaseradialdeviationLW.ID.Equals(chk1)) { chkDecreaseradialdeviationLW.Checked = true; }

            if (chkIncreaseflexionRHIP.ID.Equals(chk1)) { chkIncreaseflexionRHIP.Checked = true; }
            if (chkDecreaseflexionRHIP.ID.Equals(chk1)) { chkDecreaseflexionRHIP.Checked = true; }
            if (chkIncreaseextensionRHIP.ID.Equals(chk1)) { chkIncreaseextensionRHIP.Checked = true; }
            if (chkDecreaseextensionRHIP.ID.Equals(chk1)) { chkDecreaseextensionRHIP.Checked = true; }
            if (chkIncreaseabductionRHIP.ID.Equals(chk1)) { chkIncreaseabductionRHIP.Checked = true; }
            if (chkDecreaseabductionRHIP.ID.Equals(chk1)) { chkDecreaseabductionRHIP.Checked = true; }
            if (chkIncreaseintrotationRHIP.ID.Equals(chk1)) { chkIncreaseintrotationRHIP.Checked = true; }
            if (chkDecreaseintrotationRHIP.ID.Equals(chk1)) { chkDecreaseintrotationRHIP.Checked = true; }

            if (chkIncreaseflexionLHIP.ID.Equals(chk1)) { chkIncreaseflexionLHIP.Checked = true; }
            if (chkDecreaseflexionLHIP.ID.Equals(chk1)) { chkDecreaseflexionLHIP.Checked = true; }
            if (chkIncreaseextensionLHIP.ID.Equals(chk1)) { chkIncreaseextensionLHIP.Checked = true; }
            if (chkDecreaseextensionLHIP.ID.Equals(chk1)) { chkDecreaseextensionLHIP.Checked = true; }
            if (chkIncreaseabductionLHIP.ID.Equals(chk1)) { chkIncreaseabductionLHIP.Checked = true; }
            if (chkDecreaseabductionLHIP.ID.Equals(chk1)) { chkDecreaseabductionLHIP.Checked = true; }
            if (chkIncreaseintrotationLHIP.ID.Equals(chk1)) { chkIncreaseintrotationLHIP.Checked = true; }
            if (chkDecreaseintrotationLHIP.ID.Equals(chk1)) { chkDecreaseintrotationLHIP.Checked = true; }


            if (chkIncreasedorsiflexionRA.ID.Equals(chk1)) { chkIncreasedorsiflexionRA.Checked = true; }
            if (chkDecreasedorsiflexionRA.ID.Equals(chk1)) { chkDecreasedorsiflexionRA.Checked = true; }
            if (chkIncreaseplantarflexionRA.ID.Equals(chk1)) { chkIncreaseplantarflexionRA.Checked = true; }
            if (chkDecreaseplantarflexionRA.ID.Equals(chk1)) { chkDecreaseplantarflexionRA.Checked = true; }
            if (chkIncreaseinversionRA.ID.Equals(chk1)) { chkIncreaseinversionRA.Checked = true; }
            if (chkDecreaseinversionRA.ID.Equals(chk1)) { chkDecreaseinversionRA.Checked = true; }
            if (chkIncreaseeversionRA.ID.Equals(chk1)) { chkIncreaseeversionRA.Checked = true; }
            if (chkDecreaseeversionRA.ID.Equals(chk1)) { chkDecreaseeversionRA.Checked = true; }

            if (chkIncreasedorsiflexionLA.ID.Equals(chk1)) { chkIncreasedorsiflexionLA.Checked = true; }
            if (chkDecreasedorsiflexionLA.ID.Equals(chk1)) { chkDecreasedorsiflexionLA.Checked = true; }
            if (chkIncreaseplantarflexionLA.ID.Equals(chk1)) { chkIncreaseplantarflexionLA.Checked = true; }
            if (chkDecreaseplantarflexionLA.ID.Equals(chk1)) { chkDecreaseplantarflexionLA.Checked = true; }
            if (chkIncreaseinversionLA.ID.Equals(chk1)) { chkIncreaseinversionLA.Checked = true; }
            if (chkDecreaseinversionLA.ID.Equals(chk1)) { chkDecreaseinversionLA.Checked = true; }
            if (chkIncreaseeversionLA.ID.Equals(chk1)) { chkIncreaseeversionLA.Checked = true; }
            if (chkDecreaseeversionLA.ID.Equals(chk1)) { chkDecreaseeversionLA.Checked = true; }
            //
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string GetSelectedValue(CheckBoxList chk, string bodyPart, string ltrtext)
    {
        try
        {
            string Selected = string.Empty;
            string textPrint = string.Empty;
            int count = 0;
            foreach (ListItem li in chk.Items)
            {
                var value = li.Value;
                var text = li.Text;
                bool isChecked = li.Selected;
                if (isChecked)
                {
                    if (count == 0)
                    {
                        Selected = "$" + bodyPart + "-";
                        textPrint = "$" + ltrtext + "-";
                    }
                    Selected = Selected + "|" + value;
                    textPrint = textPrint + "|" + value;
                    count++;
                }
            }
            return Selected + "~" + textPrint;
        }
        catch (Exception ex)
        {

            throw;
            return string.Empty;
        }
    }
    protected void btnsavesoap_Click(object sender, EventArgs e)
    {
        try
        {
            //string otherselected = hdnOtherValue.Value;
            //Request.Form["Other_1"].ToString();
            List<string> _injuredBodyParts = (List<string>)ViewState["_injuredBodyParts"];
            List<string> groupbodypart = (List<string>)ViewState["groupbodypart"];

            DateTime d;
            DateTime.TryParseExact(txtCreateSoapDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
            string BodyPart = string.Empty;
            string BodyPartPrint = string.Empty;
            string seelctedspams = string.Empty;

            foreach (var t in _injuredBodyParts)
            {
                switch (t)
                {
                    case "Neck":
                        string selected = string.Empty;
                        string selectedPrintIncrease = string.Empty;
                        string selectedPrintDecrease = string.Empty;
                        selected += (chkIncreaseflexionNeck.Checked ? "|flexionNeck-Increase" : String.Empty) + (chkDecreaseflexionNeck.Checked ? "|flexionNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreaseflexionNeck.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionNeck.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionNeck.Checked ? "|extensionNeck-Increase" : String.Empty) + (chkDecreaseextensionNeck.Checked ? "|extensionNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreaseextensionNeck.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionNeck.Checked ? " extension," : string.Empty);

                        selected += (chkIncreasetilttoleftNeck.Checked ? "|tilttoleftNeck-Increase" : String.Empty) + (chkDecreasetilttoleftNeck.Checked ? "|tilttoleftNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreasetilttoleftNeck.Checked ? " tilt to left," : String.Empty);
                        selectedPrintDecrease += (chkDecreasetilttoleftNeck.Checked ? " tilt to left," : string.Empty);

                        selected += (chkIncreasetilttorightNeck.Checked ? "|tilttorightNeck-Increase" : String.Empty) + (chkDecreasetilttorightNeck.Checked ? "|tilttorightNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreasetilttorightNeck.Checked ? " tilt to right," : String.Empty);
                        selectedPrintDecrease += (chkDecreasetilttorightNeck.Checked ? " tilt to right," : string.Empty);

                        selected += (chkIncreaseleftrotationNeck.Checked ? "|leftrotationNeck-Increase" : String.Empty) + (chkDecreaseleftrotationNeck.Checked ? "|leftrotationNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreaseleftrotationNeck.Checked ? " left rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseleftrotationNeck.Checked ? " left rotation," : string.Empty);

                        selected += (chkIncreaseRightrotationNeck.Checked ? "|RightrotationNeck-Increase" : String.Empty) + (chkDecreaseRightrotationNeck.Checked ? "|RightrotationNeck-Decrease" : string.Empty);

                        selectedPrintIncrease += (chkIncreaseRightrotationNeck.Checked ? " right rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseRightrotationNeck.Checked ? " right rotation," : string.Empty);

                        selected += (chkspasmNeck.Checked ? "|spasmNeck-chk" : String.Empty);
                        if (chkspasmNeck.Checked)
                        { seelctedspams = "spasm present "; }
                        //selectedPrintIncrease += (chkspasmNeck.Checked ? " spasm present ,," : String.Empty);
                        //selectedPrintDecrease += (chkspasmNeck.Checked ? " spasm present ,," : String.Empty);

                        BodyPart += "$Cervical" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "C/S: ";
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                            if (chkspasmNeck.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += " Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                            if (chkspasmNeck.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (string.IsNullOrEmpty(selectedPrintDecrease) && string.IsNullOrEmpty(selectedPrintIncrease))
                        { BodyPartPrint += System.Environment.NewLine + "C/S: " + seelctedspams; }

                        //groupbodypart.Add("Cervical");
                        break;
                    case "LowBack":
                        selected = string.Empty;
                        seelctedspams = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionLumber.Checked ? "|flexionLumber-Increase" : String.Empty) + (chkDecreaseflexionLumber.Checked ? "|flexionLumber-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionLumber.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionLumber.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionLumber.Checked ? "|extensionLumber-Increase" : String.Empty) + (chkDecreaseextensionLumber.Checked ? "|extensionLumber-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionLumber.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionLumber.Checked ? " extension," : string.Empty);

                        selected += (chkIncreasetilttoleftLumber.Checked ? "|tilttoleftLumber-Increase" : String.Empty) + (chkDecreasetilttoleftLumber.Checked ? "|tilttoleftLumber-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasetilttoleftLumber.Checked ? " tilt to left," : String.Empty);
                        selectedPrintDecrease += (chkDecreasetilttoleftLumber.Checked ? " tilt to left," : string.Empty);

                        selected += (chkIncreasetilttorightLumber.Checked ? "|tilttorightLumber-Increase" : String.Empty) + (chkDecreasetilttorightLumber.Checked ? "|tilttorightLumber-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasetilttorightLumber.Checked ? " tilt to right," : String.Empty);
                        selectedPrintDecrease += (chkDecreasetilttorightLumber.Checked ? " tilt to right," : string.Empty);

                        selected += (chkspasmLumber.Checked ? "|spasmLumber-chk" : String.Empty);
                        if (chkspasmLumber.Checked)
                        { seelctedspams = "spasm present "; }
                        //selectedPrintIncrease += (chkspasmLumber.Checked ? " spasm present ,," : String.Empty);
                        //selectedPrintDecrease += (chkspasmLumber.Checked ? " spasm present ,," : String.Empty);

                        BodyPart += "$lumber" + selected;

                        //BodyPartPrint += "$L/S:Increase:" + selectedPrintIncrease.Replace(","," and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L/S: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                            if (chkspasmLumber.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += " Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                            if (chkspasmLumber.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (string.IsNullOrEmpty(selectedPrintDecrease) && string.IsNullOrEmpty(selectedPrintIncrease))
                        { BodyPartPrint += System.Environment.NewLine + "L/S: " + seelctedspams; }
                        //groupbodypart.Add("lumber");
                        break;
                    case "MidBack":
                        selected = string.Empty;
                        seelctedspams = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreasemildlyTrousic.Checked ? "|mildlyTrousic-Increase" : String.Empty) + (chkDecreasemildlyTrousic.Checked ? "|mildlyTrousic-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasemildlyTrousic.Checked ? " ROM is mildly increase," : String.Empty);
                        selectedPrintDecrease += (chkDecreasemildlyTrousic.Checked ? " ROM is mildly decreased," : string.Empty);

                        selected += (chkIncreasemoderatelyTrousic.Checked ? "|moderatelyTrousic-Increase" : String.Empty) + (chkDecreasemoderatelyTrousic.Checked ? "|moderatelyTrousic-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasemoderatelyTrousic.Checked ? " ROM is moderately increase," : String.Empty);
                        selectedPrintDecrease += (chkDecreasemoderatelyTrousic.Checked ? " ROM is moderately decreased," : string.Empty);

                        selected += (chkincreaseseverelyTrousic.Checked ? "|severelyTrousic-Increase" : String.Empty) + (chkDecreaseseverelyTrousic.Checked ? "|severelyTrousic-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkincreaseseverelyTrousic.Checked ? " ROM is severely increase," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseseverelyTrousic.Checked ? " ROM is severely decreased," : string.Empty);

                        selected += (chkspasmTrousic.Checked ? "|spasmTrousic-chk" : String.Empty);
                        if (chkspasmTrousic.Checked)
                        { seelctedspams = "spasm present "; }

                        //selectedPrintIncrease += (chkspasmTrousic.Checked ? " spasm present ,, " : String.Empty);
                        //selectedPrintDecrease += (chkspasmTrousic.Checked ? " spasm present ,, " : String.Empty);

                        BodyPart += "$thoracic" + selected;

                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "T/S: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += selectedPrintIncrease.Replace(",", ", ");
                            if (chkspasmTrousic.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ").Trim();
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += selectedPrintDecrease.Replace(",", ", ");
                            if (chkspasmTrousic.Checked)
                            { BodyPartPrint += "spasm present,"; }
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ").Trim();
                        }
                        if (string.IsNullOrEmpty(selectedPrintDecrease) && string.IsNullOrEmpty(selectedPrintIncrease))
                        { BodyPartPrint += System.Environment.NewLine + "T/S: " + seelctedspams; }
                        //BodyPartPrint += "$T/S:Increase:" + selectedPrintIncrease.Replace(","," and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();

                        //groupbodypart.Add("trousic");
                        break;
                    case "RightShoulder":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionRSh.Checked ? "|flexionRSh-Increase" : String.Empty) + (chkDecreaseflexionRSh.Checked ? "|flexionRSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionRSh.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionRSh.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseabductionRSh.Checked ? "|abductionRSh-Increase" : String.Empty) + (chkDecreaseabductionRSh.Checked ? "|abductionRSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseabductionRSh.Checked ? " abduction," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseabductionRSh.Checked ? " abduction," : string.Empty);

                        selected += (chkIncreaseintrotationRSh.Checked ? "|introtationRSh-Increase" : String.Empty) + (chkDecreaseintrotationRSh.Checked ? "|introtationRSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseintrotationRSh.Checked ? " internal rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseintrotationRSh.Checked ? " internal rotation," : string.Empty);

                        selected += (chkIncreaseextrotationRSh.Checked ? "|extrotationRSh-Increase" : String.Empty) + (chkDecreaseextrotationRSh.Checked ? "|extrotationRSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextrotationRSh.Checked ? " external rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextrotationRSh.Checked ? " external rotation," : string.Empty);

                        BodyPart += "$R. Shoulder" + selected;

                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Shoulder: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }



                        //BodyPartPrint += "$R. Shoulder :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("LSh");
                        break;
                    case "LeftShoulder":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionLSh.Checked ? "|flexionRLh-Increase" : String.Empty) + (chkDecreaseflexionLSh.Checked ? "|flexionLSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionLSh.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionLSh.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseabductionLSh.Checked ? "|abductionLSh-Increase" : String.Empty) + (chkDecreaseabductionLSh.Checked ? "|abductionLSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseabductionLSh.Checked ? " abduction," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseabductionLSh.Checked ? " abduction," : string.Empty);

                        selected += (chkIncreaseintrotationLSh.Checked ? "|introtationLSh-Increase" : String.Empty) + (chkDecreaseintrotationLSh.Checked ? "|introtationLSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseintrotationLSh.Checked ? " internal rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseintrotationLSh.Checked ? " internal rotation," : string.Empty);

                        selected += (chkIncreaseextrotationLSh.Checked ? "|extrotationLSh-Increase" : String.Empty) + (chkDecreaseextrotationLSh.Checked ? "|extrotationLSh-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextrotationLSh.Checked ? " external rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextrotationLSh.Checked ? " external rotation," : string.Empty);

                        BodyPart += "$L. Shoulder" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Shoulder: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Shoulder :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("RSh");
                        break;
                    case "LeftKnee":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionLK.Checked ? "|flexionLK-Increase" : String.Empty) + (chkDecreaseflexionLK.Checked ? "|flexionLK-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionLK.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionLK.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionLK.Checked ? "|extensionLK-Increase" : String.Empty) + (chkDecreaseextensionLK.Checked ? "|extensionLK-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionLK.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionLK.Checked ? " extension," : string.Empty);

                        BodyPart += "$L. Knee" + selected;

                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Knee: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        //BodyPartPrint += "$L. Knee :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        // groupbodypart.Add("LK");
                        break;
                    case "RightKnee":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionRK.Checked ? "|flexionRK-Increase" : String.Empty) + (chkDecreaseflexionRK.Checked ? "|flexionRK-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionRK.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionRK.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionRK.Checked ? "|extensionRK-Increase" : String.Empty) + (chkDecreaseextensionLK.Checked ? "|extensionRK-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionRK.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionRK.Checked ? " extension," : string.Empty);

                        BodyPart += "$R. Knee" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Knee: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$R. Knee :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //   groupbodypart.Add("RK");
                        break;
                    case "LeftElbow":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionLE.Checked ? "|flexionLE-Increase" : String.Empty) + (chkDecreaseflexionLE.Checked ? "|flexionLE-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionLE.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionLE.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionLE.Checked ? "|extensionLE-Increase" : String.Empty) + (chkDecreaseextensionLE.Checked ? "|extensionLE-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionLE.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionLE.Checked ? " extension," : string.Empty);

                        BodyPart += "$L. Elbow" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Elbow: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Elbow :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //  groupbodypart.Add("LE");
                        break;
                    case "RightElbow":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionRE.Checked ? "|flexionRE-Increase" : String.Empty) + (chkDecreaseflexionRE.Checked ? "|flexionRE-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionRE.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionRE.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionRE.Checked ? "|extensionRE-Increase" : String.Empty) + (chkDecreaseextensionRE.Checked ? "|extensionRE-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionRE.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionRE.Checked ? " extension," : string.Empty);

                        BodyPart += "$R. Elbow" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Elbow: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        //BodyPartPrint += "$R. Elbow :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("RE");
                        break;
                    case "LeftWrist":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreasepalmarflexionLW.Checked ? "|palmarflexionLW-Increase" : String.Empty) + (chkDecreasepalmarflexionLW.Checked ? "|palmarflexionLW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasepalmarflexionLW.Checked ? " palmar flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasepalmarflexionLW.Checked ? " palmar flexion," : string.Empty);

                        selected += (chkIncreasedorsiflexionLW.Checked ? "|dorsiflexionLW-Increase" : String.Empty) + (chkDecreasedorsiflexionLW.Checked ? "|dorsiflexionLW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasedorsiflexionLW.Checked ? " dorsiflexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasedorsiflexionLW.Checked ? " dorsiflexion," : string.Empty);

                        selected += (chkIncreaseulnardeviationLW.Checked ? "|ulnardeviationLW-Increase" : String.Empty) + (chkDecreaseulnardeviationLW.Checked ? "|ulnardeviationLW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseulnardeviationLW.Checked ? " ulnar deviation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseulnardeviationLW.Checked ? " ulnar deviation," : string.Empty);

                        selected += (chkIncreaseradialdeviationLW.Checked ? "|radialdeviationLW-Increase" : String.Empty) + (chkDecreaseradialdeviationLW.Checked ? "|radialdeviationLW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseradialdeviationLW.Checked ? " radial deviation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseradialdeviationLW.Checked ? " radial deviation," : string.Empty);

                        BodyPart += "$L. Wrist" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Wrist: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Wrist :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        // groupbodypart.Add("LW");
                        break;
                    case "RightWrist":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreasepalmarflexionRW.Checked ? "|palmarflexionRW-Increase" : String.Empty) + (chkDecreasepalmarflexionRW.Checked ? "|palmarflexionRW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasepalmarflexionRW.Checked ? " palmar flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasepalmarflexionRW.Checked ? " palmar flexion," : string.Empty);

                        selected += (chkIncreasedorsiflexionRW.Checked ? "|dorsiflexionRW-Increase" : String.Empty) + (chkDecreasedorsiflexionRW.Checked ? "|dorsiflexionRW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasedorsiflexionRW.Checked ? " dorsiflexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasedorsiflexionRW.Checked ? " dorsiflexion," : string.Empty);

                        selected += (chkIncreaseulnardeviationRW.Checked ? "|ulnardeviationRW-Increase" : String.Empty) + (chkDecreaseulnardeviationRW.Checked ? "|ulnardeviationRW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseulnardeviationRW.Checked ? " ulnar deviation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseulnardeviationRW.Checked ? " ulnar deviation," : string.Empty);

                        selected += (chkIncreaseradialdeviationRW.Checked ? "|radialdeviationRW-Increase" : String.Empty) + (chkDecreaseradialdeviationRW.Checked ? "|radialdeviationRW-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseradialdeviationRW.Checked ? " radial deviation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseradialdeviationRW.Checked ? " radial deviation," : string.Empty);

                        BodyPart += "$R. Wrist" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Wrist:";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        //BodyPartPrint += "$R. Wrist :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("RW");
                        break;
                    case "LeftHip":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionLHIP.Checked ? "|flexionLHIP-Increase" : String.Empty) + (chkDecreaseflexionLHIP.Checked ? "|flexionLHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionLHIP.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionLHIP.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionLHIP.Checked ? "|extensionLHIP-Increase" : String.Empty) + (chkDecreaseextensionLHIP.Checked ? "|extensionLHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionLHIP.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionLHIP.Checked ? " extension," : string.Empty);

                        selected += (chkIncreaseabductionLHIP.Checked ? "|abductionLHIP-Increase" : String.Empty) + (chkDecreaseabductionLHIP.Checked ? "|abductionLHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseabductionLHIP.Checked ? " abduction," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseabductionLHIP.Checked ? " abduction," : string.Empty);

                        selected += (chkIncreaseintrotationLHIP.Checked ? "|introtationLHIP-Increase" : String.Empty) + (chkDecreaseintrotationLHIP.Checked ? "|introtationLHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseintrotationLHIP.Checked ? " internal rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseintrotationLHIP.Checked ? " internal rotation," : string.Empty);

                        BodyPart += "$L. Hip" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Hip: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Hip :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("LHIP");
                        break;
                    case "RightHip":
                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreaseflexionRHIP.Checked ? "|flexionRHIP-Increase" : String.Empty) + (chkDecreaseflexionRHIP.Checked ? "|flexionRHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseflexionRHIP.Checked ? " flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseflexionRHIP.Checked ? " flexion," : string.Empty);

                        selected += (chkIncreaseextensionRHIP.Checked ? "|extensionRHIP-Increase" : String.Empty) + (chkDecreaseextensionRHIP.Checked ? "|extensionRHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseextensionRHIP.Checked ? " extension," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseextensionRHIP.Checked ? " extension," : string.Empty);

                        selected += (chkIncreaseabductionRHIP.Checked ? "|abductionRHIP-Increase" : String.Empty) + (chkDecreaseabductionRHIP.Checked ? "|abductionRHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseabductionRHIP.Checked ? " abduction," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseabductionRHIP.Checked ? " abduction," : string.Empty);

                        selected += (chkIncreaseintrotationRHIP.Checked ? "|introtationRHIP-Increase" : String.Empty) + (chkDecreaseintrotationRHIP.Checked ? "|introtationRHIP-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseintrotationRHIP.Checked ? " internal rotation," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseintrotationRHIP.Checked ? " internal rotation," : string.Empty);

                        BodyPart += "$R. Hip" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Hip: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Hip :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("RHIP");

                        break;
                    case "LeftAnkle":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreasedorsiflexionLA.Checked ? "|dorsiflexionLA-Increase" : String.Empty) + (chkDecreasedorsiflexionLA.Checked ? "|dorsiflexionLA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasedorsiflexionLA.Checked ? " dorsiflexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasedorsiflexionLA.Checked ? " dorsiflexion," : string.Empty);

                        selected += (chkIncreaseplantarflexionLA.Checked ? "|plantarflexionLA-Increase" : String.Empty) + (chkDecreaseplantarflexionLA.Checked ? "|plantarflexionLA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseplantarflexionLA.Checked ? " plantar flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseplantarflexionLA.Checked ? " plantar flexion," : string.Empty);

                        selected += (chkIncreaseinversionLA.Checked ? "|inversionLA-Increase" : String.Empty) + (chkDecreaseinversionLA.Checked ? "|inversionLA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseinversionLA.Checked ? " inversion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseinversionLA.Checked ? " inversion," : string.Empty);

                        selected += (chkIncreaseeversionLA.Checked ? "|eversionLA-Increase" : String.Empty) + (chkDecreaseeversionLA.Checked ? "|eversionLA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseeversionLA.Checked ? " eversion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseeversionLA.Checked ? " eversion," : string.Empty);

                        BodyPart += "$L. Ankle" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "L. Ankle: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$L. Ankle :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("LA");
                        break;
                    case "RightAnkle":

                        selected = string.Empty;
                        selectedPrintIncrease = string.Empty;
                        selectedPrintDecrease = string.Empty;

                        selected += (chkIncreasedorsiflexionRA.Checked ? "|dorsiflexionRA-Increase" : String.Empty) + (chkDecreasedorsiflexionRA.Checked ? "|dorsiflexionRA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreasedorsiflexionRA.Checked ? " dorsiflexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreasedorsiflexionRA.Checked ? " dorsiflexion," : string.Empty);

                        selected += (chkIncreaseplantarflexionRA.Checked ? "|plantarflexionRA-Increase" : String.Empty) + (chkDecreaseplantarflexionRA.Checked ? "|plantarflexionRA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseplantarflexionRA.Checked ? " plantar flexion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseplantarflexionRA.Checked ? " plantar flexion," : string.Empty);

                        selected += (chkIncreaseinversionRA.Checked ? "|inversionRA-Increase" : String.Empty) + (chkDecreaseinversionRA.Checked ? "|inversionRA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseinversionRA.Checked ? " inversion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseinversionRA.Checked ? " inversion," : string.Empty);

                        selected += (chkIncreaseeversionRA.Checked ? "|eversionRA-Increase" : String.Empty) + (chkDecreaseeversionRA.Checked ? "|eversionRA-Decrease" : string.Empty);
                        selectedPrintIncrease += (chkIncreaseeversionRA.Checked ? " eversion," : String.Empty);
                        selectedPrintDecrease += (chkDecreaseeversionRA.Checked ? " eversion," : string.Empty);

                        BodyPart += "$R. Ankle" + selected;
                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += System.Environment.NewLine + "R. Ankle: ";
                        }

                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                        }
                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                        {
                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                        }

                        //BodyPartPrint += "$R. Ankle :Increase:" + selectedPrintIncrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", " and ");
                        //BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, "and", " ").Trim();
                        //groupbodypart.Add("RA");
                        break;
                }
            }



            string Treatment = string.Empty;
            string PrintText = string.Empty;
            if (GetSelectedValue(chkModalities1, "Modalities1", Modalities1.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkModalities1, "Modalities1", Modalities1.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkModalities1, "Modalities1", Modalities1.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkModalities2, "Modalities2", Modalities2.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkModalities2, "Modalities2", Modalities2.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkModalities2, "Modalities2", Modalities2.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkModalities3, "Modalities3", Modalities3.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkModalities3, "Modalities3", Modalities3.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkModalities3, "Modalities3", Modalities3.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkModalities4, "Modalities4", Modalities4.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkModalities4, "Modalities4", Modalities4.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkModalities4, "Modalities4", Modalities4.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkManualtreatment1, "Manualtreatment1", Manualtreatment1.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkManualtreatment1, "Manualtreatment1", Manualtreatment1.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkManualtreatment1, "Manualtreatment1", Manualtreatment1.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkManualtreatment2, "Manualtreatment2", Manualtreatment2.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkManualtreatment2, "Manualtreatment2", Manualtreatment2.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkManualtreatment2, "Manualtreatment2", Manualtreatment2.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkManualtreatment3, "Manualtreatment3", Manualtreatment3.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkManualtreatment3, "Manualtreatment3", Manualtreatment3.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkManualtreatment3, "Manualtreatment3", Manualtreatment3.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkManualtreatment4, "Manualtreatment4", Manualtreatment4.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkManualtreatment4, "Manualtreatment4", Manualtreatment4.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkManualtreatment4, "Manualtreatment4", Manualtreatment4.Text).Split('~')[1];
            }

            if (GetSelectedValue(chkOther1, "Other1", Other1.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther1, "Other1", Other1.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther1, "Other1", Other1.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther2, "Other2", Other2.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther2, "Other2", Other2.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther2, "Other2", Other2.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther3, "Other3", Other3.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther3, "Other3", Other3.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther3, "Other3", Other3.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther4, "Other4", Other4.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther4, "Other4", Other4.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther4, "Other4", Other4.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther5, "Other5", Other5.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther5, "Other5", Other5.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther5, "Other5", Other5.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther6, "Other6", Other6.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther6, "Other6", Other6.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther6, "Other6", Other6.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther7, "Other7", Other7.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther7, "Other7", Other7.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther7, "Other7", Other7.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther8, "Other8", Other8.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther8, "Other8", Other8.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther8, "Other8", Other8.Text).Split('~')[1];
            }
            if (GetSelectedValue(chkOther9, "Other9", Other9.Text).Split('~').Count() > 1)
            {
                Treatment += GetSelectedValue(chkOther9, "Other9", Other9.Text).Split('~')[0];
                PrintText += GetSelectedValue(chkOther9, "Other9", Other9.Text).Split('~')[1];
            }
            if (!string.IsNullOrEmpty(txtothertext.Text))
            {
                Treatment += "$Other10" + "-" + txtothertext.Text;
                PrintText += "$" + Other10.Text + "-|" + txtothertext.Text;
            }

            SqlConnection oSQLConn = new SqlConnection();
            SqlCommand oSQLCmd = new SqlCommand();
            string _soapId = Convert.ToString(hdnSoapId.Value);
            string _ieID = Convert.ToString(hdnrodieid.Value);
            string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
            string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

            string _ieMode = "";
            string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
            {
                SqlStr = "Select * from tblSoap WHERE PatientIE_ID = " + _ieID + " and PatientFU_ID IS NULL and Id=" + Convert.ToInt64(_soapId);
            }
            else
            {
                _ieID = string.Empty;
                SqlStr = "Select * from tblSoap WHERE PatientIE_ID = " + Convert.ToInt64(_fuieid) + " and PatientFU_ID = " + Convert.ToInt64(_fufuid) + " and Id=" + Convert.ToInt64(_soapId);
            }

            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count == 0)
                _ieMode = "New";
            else if (sqlTbl.Rows.Count == 0)
                _ieMode = "None";
            else if (sqlTbl.Rows.Count > 0)
                _ieMode = "Update";
            else
                _ieMode = "Delete";

            if (_ieMode == "New")
                TblRow = sqlTbl.NewRow();
            else if (_ieMode == "Update" || _ieMode == "Delete")
            {
                TblRow = sqlTbl.Rows[0];
                TblRow.AcceptChanges();
            }
            else
                TblRow = null;

            if (_ieMode == "Update" || _ieMode == "New")
            {
                TblRow["PatientIE_ID"] = !string.IsNullOrEmpty(_ieID) ? _ieID : _fuieid;

                if (!string.IsNullOrEmpty(_fufuid))
                {
                    TblRow["PatientFU_ID"] = _fufuid;
                }

                TblRow["CreationDate"] = Convert.ToDateTime(d);
                TblRow["PatientName"] = lblName.Text;
                TblRow["DOI"] = lblDOI.Text;
                TblRow["DOS"] = Convert.ToDateTime(d);
                TblRow["Subjective"] = txtAdjective.Text;
                TblRow["Objective"] = BodyPart;
                TblRow["PrintObjective"] = BodyPartPrint;
                TblRow["Treatment"] = Treatment;
                TblRow["PrintTreatment"] = PrintText;
                TblRow["Assessment"] = bindSoapAssestPlanPrintvalue(repAssestment, "txtAssestment", hdnAssestment);
                TblRow["Plans"] = bindSoapAssestPlanPrintvalue(repPlan, "txtPlan", hdnPlan);
                TblRow["AssessmentContent"] = hdnAssestment.Value;
                TblRow["PlansContent"] = hdnPlan.Value;

                TblRow["MAProvider"] = txtMAProvider.Text;

                if (_ieMode == "New")
                {
                    sqlTbl.Rows.Add(TblRow);
                }
                sqlAdapt.Update(sqlTbl);
            }
            else if (_ieMode == "Delete")
            {
                TblRow.Delete();
                sqlAdapt.Update(sqlTbl);
            }
            if (TblRow != null)
                TblRow.Table.Dispose();
            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    public void bindSoapBodyPart(List<string> _injuredBodyParts, bool binddefault)
    {
        ViewState["_injuredBodyParts"] = _injuredBodyParts;

        if (binddefault)
        { clearcontrolSoap(); }

        if (_injuredBodyParts.Contains("Neck"))
        {
            dvbp_Neck.Style.Add("display", "block");
        }
        else
        { dvbp_Neck.Style.Add("display", "none"); }


        if (_injuredBodyParts.Contains("MidBack"))
        {
            dvbp_MidBack.Style.Add("display", "block");
        }
        else
        { dvbp_MidBack.Style.Add("display", "none"); }


        if (_injuredBodyParts.Contains("LowBack"))
        {
            dvbp_LowBack.Style.Add("display", "block");
        }
        else
        { dvbp_LowBack.Style.Add("display", "none"); }


        if (_injuredBodyParts.Contains("RightElbow"))
        {
            dvbp_RightElbow.Style.Add("display", "block");
        }
        else
        { dvbp_RightElbow.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftElbow"))
        {
            dvbp_LeftElbow.Style.Add("display", "block");
        }
        else
        { dvbp_LeftElbow.Style.Add("display", "none"); }


        if (_injuredBodyParts.Contains("RightKnee"))
        {
            dvbp_RightKnee.Style.Add("display", "block");
        }
        else
        { dvbp_RightKnee.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftKnee"))
        {
            dvbp_LeftKnee.Style.Add("display", "block");
        }
        else
        { dvbp_LeftKnee.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("RightShoulder"))
        {
            dvbp_RightShoulder.Style.Add("display", "block");
        }
        else
        { dvbp_RightShoulder.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftShoulder"))
        {
            dvbp_LeftShoulder.Style.Add("display", "block");
        }
        else
        { dvbp_LeftShoulder.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("RightWrist"))
        {
            dvbp_RightWrist.Style.Add("display", "block");
        }
        else
        { dvbp_RightWrist.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftWrist"))
        {
            dvbp_LeftWrist.Style.Add("display", "block");
        }
        else
        { dvbp_LeftWrist.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("RightHip"))
        {
            dvbp_RightHip.Style.Add("display", "block");
        }
        else
        { dvbp_RightHip.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftHip"))
        {
            dvbp_LeftHip.Style.Add("display", "block");
        }
        else
        { dvbp_LeftHip.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("RightAnkle"))
        {
            dvbp_RightAnkle.Style.Add("display", "block");
        }
        else
        { dvbp_RightAnkle.Style.Add("display", "none"); }

        if (_injuredBodyParts.Contains("LeftAnkle"))
        {
            dvbp_LeftAnkle.Style.Add("display", "block");
        }
        else
        { dvbp_LeftAnkle.Style.Add("display", "none"); }


        List<string> groupbodypart = new List<string>();

        foreach (var t in _injuredBodyParts)
        {
            switch (t)
            {

                case "Neck":
                    groupbodypart.Add("C/S");
                    break;
                case "MidBack":
                    groupbodypart.Add("T/S");
                    break;
                case "LowBack":
                    groupbodypart.Add("L/S");
                    break;
                case "LeftShoulder":
                    groupbodypart.Add("L. Shoulder");
                    break;
                case "RightShoulder":
                    groupbodypart.Add("R. Shoulder");
                    break;
                case "LeftKnee":
                    groupbodypart.Add("L. Knee");
                    break;
                case "RightKnee":
                    groupbodypart.Add("R. Knee");
                    break;
                case "LeftElbow":
                    groupbodypart.Add("L. Elbow");
                    break;
                case "RightElbow":
                    groupbodypart.Add("R. Elbow");
                    break;
                case "LeftWrist":
                    groupbodypart.Add("L. Wrist");
                    break;
                case "RightWrist":
                    groupbodypart.Add("R. Wrist");
                    break;
                case "LeftHip":
                    groupbodypart.Add("L. Hip");
                    break;
                case "RightHip":
                    groupbodypart.Add("R. Hip");
                    break;
                case "LeftAnkle":
                    groupbodypart.Add("L. Ankle");
                    break;
                case "RightAnkle":
                    groupbodypart.Add("R. Ankle");
                    break;
            }
            ViewState["groupbodypart"] = groupbodypart;
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/Default_Soap.xml"));

        chkModalities1.Items.Clear();
        chkModalities2.Items.Clear();
        chkModalities3.Items.Clear();
        chkModalities4.Items.Clear();
        foreach (var item in groupbodypart)
        {
            chkModalities1.Items.Add(new ListItem(item, item));
            chkModalities2.Items.Add(new ListItem(item, item));
            chkModalities3.Items.Add(new ListItem(item, item));
            chkModalities4.Items.Add(new ListItem(item, item));
        }
        chkModalities1.DataBind();
        chkModalities2.DataBind();
        chkModalities3.DataBind();
        chkModalities4.DataBind();
        foreach (XmlNode node in doc.SelectNodes("//Soaps/Modalitiess/Modalities"))
        {
            if (node.Attributes["id"].InnerText.Equals("1") && binddefault)
            {
                foreach (ListItem item in chkModalities1.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("2") && binddefault)
            {
                foreach (ListItem item in chkModalities2.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("3") && binddefault)
            {
                foreach (ListItem item in chkModalities3.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("4") && binddefault)
            {
                foreach (ListItem item in chkModalities4.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }

            var literals = Page.Master.FindControl("cpMain").Controls.OfType<Literal>();
            foreach (Literal literal in literals)
            {
                if (literal.ID.Equals(node.Attributes["name"].InnerText))
                {
                    literal.Text = node.Attributes["text"].InnerText;
                }
            }
        }



        chkManualtreatment1.Items.Clear();
        chkManualtreatment2.Items.Clear();
        chkManualtreatment3.Items.Clear();
        chkManualtreatment4.Items.Clear();
        foreach (var item in groupbodypart)
        {
            chkManualtreatment1.Items.Add(new ListItem(item, item));
            chkManualtreatment2.Items.Add(new ListItem(item, item));
            chkManualtreatment3.Items.Add(new ListItem(item, item));
            chkManualtreatment4.Items.Add(new ListItem(item, item));

        }
        chkManualtreatment1.DataBind();
        chkManualtreatment2.DataBind();
        chkManualtreatment3.DataBind();
        chkManualtreatment4.DataBind();
        foreach (XmlNode node in doc.SelectNodes("//Soaps/Manualtreatmentss/Manualtreatments"))
        {
            if (node.Attributes["id"].InnerText.Equals("1") && binddefault)
            {
                foreach (ListItem item in chkManualtreatment1.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("2") && binddefault)
            {
                foreach (ListItem item in chkManualtreatment2.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("3") && binddefault)
            {
                foreach (ListItem item in chkManualtreatment3.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("4") && binddefault)
            {
                foreach (ListItem item in chkManualtreatment4.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }

            var literals = Page.Master.FindControl("cpMain").Controls.OfType<Literal>();
            foreach (Literal literal in literals)
            {
                if (literal.ID.Equals(node.Attributes["name"].InnerText))
                {
                    literal.Text = node.Attributes["text"].InnerText;
                }
            }
        }


        chkOther1.Items.Clear();
        chkOther2.Items.Clear();
        chkOther3.Items.Clear();
        chkOther4.Items.Clear();
        chkOther5.Items.Clear();
        chkOther6.Items.Clear();
        chkOther7.Items.Clear();
        chkOther8.Items.Clear();
        chkOther9.Items.Clear();
        foreach (var item in groupbodypart)
        {
            chkOther1.Items.Add(new ListItem(item, item));
            chkOther2.Items.Add(new ListItem(item, item));
            chkOther3.Items.Add(new ListItem(item, item));
            chkOther4.Items.Add(new ListItem(item, item));
            chkOther5.Items.Add(new ListItem(item, item));
            chkOther6.Items.Add(new ListItem(item, item));
            chkOther7.Items.Add(new ListItem(item, item));
            chkOther8.Items.Add(new ListItem(item, item));
            chkOther9.Items.Add(new ListItem(item, item));
        }
        chkOther1.DataBind();
        chkOther2.DataBind();
        chkOther3.DataBind();
        chkOther4.DataBind();
        chkOther5.DataBind();
        chkOther6.DataBind();
        chkOther7.DataBind();
        chkOther8.DataBind();
        chkOther9.DataBind();
        foreach (XmlNode node in doc.SelectNodes("//Soaps/Others/Other"))
        {
            if (node.Attributes["id"].InnerText.Equals("1") && binddefault)
            {
                foreach (ListItem item in chkOther1.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("2") && binddefault)
            {
                foreach (ListItem item in chkOther2.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("3") && binddefault)
            {
                foreach (ListItem item in chkOther3.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("8") && binddefault)
            {
                foreach (ListItem item in chkOther8.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }

            if (node.Attributes["id"].InnerText.Equals("4") && binddefault)
            {
                foreach (ListItem item in chkOther4.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("9") && binddefault)
            {
                foreach (ListItem item in chkOther9.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("5") && binddefault)
            {
                foreach (ListItem item in chkOther5.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("6") && binddefault)
            {
                foreach (ListItem item in chkOther6.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }
            if (node.Attributes["id"].InnerText.Equals("7") && binddefault)
            {
                foreach (ListItem item in chkOther7.Items)
                {
                    item.Selected = Convert.ToBoolean(node.Attributes["selected"].InnerText);
                }
            }


            var literals = Page.Master.FindControl("cpMain").Controls.OfType<Literal>();
            foreach (Literal literal in literals)
            {
                if (literal.ID.Equals(node.Attributes["name"].InnerText))
                {
                    literal.Text = node.Attributes["text"].InnerText;
                }
            }
        }

    }
    public void BindSoapAsstPlan()
    {
        try
        {

            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/Xml/Default_soap_treatment_assestment.xml"));
            DataSet main = new DataSet();
            main.ReadXml(xmlreader);
            xmlreader.Close();
            DataTable dt = main.Tables[0];
            DataView dvplan = new DataView(dt);
            dvplan.RowFilter = "type='plan'"; // query example = "id = 10"

            DataView dvassestment = new DataView(dt);
            dvassestment.RowFilter = "type='Assessment'"; // query example = "id = 10"

            if (dvassestment.Count > 0)
            {
                repAssestment.DataSource = dvplan;
                repAssestment.DataBind();
            }
            if (dvplan.Count > 0)
            {
                repPlan.DataSource = dvassestment;
                repPlan.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    private string bindSoapAssestPlanPrintvalue(Repeater rep, string textbox, HiddenField hdnfield)
    {
        string str = "";
        string strDelimit = "";
        for (int i = 0; i < rep.Items.Count; i++)
        {
            TextBox txt = rep.Items[i].FindControl(textbox) as TextBox;
            CheckBox chk = rep.Items[i].FindControl("chk") as CheckBox;

            if (chk.Checked)
            {
                str = str + txt.Text + System.Environment.NewLine;
                strDelimit = strDelimit + "`" + txt.Text;
            }
            else
            {

                str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "`@" + txt.Text;
            }

        }
        strDelimit = strDelimit.TrimStart('`');
        hdnfield.Value = str;
        return strDelimit;

    }
    private void BindSoapAssestPlanEditValues(Repeater rep, string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            string[] str = val.Split('`');

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)) });

            for (int i = 0; i < str.Length; i++)
            {
                dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
            }

            rep.DataSource = dt;
            rep.DataBind();
        }

    }
    protected void chk_CheckedAssestment(object sender, EventArgs e)
    {
        bindSoapAssestPlanPrintvalue(repAssestment, "txtAssestment", hdnAssestment);

        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoap();", true);
    }
    protected void chk_CheckedPlan(object sender, EventArgs e)
    {
        bindSoapAssestPlanPrintvalue(repPlan, "txtPlan", hdnPlan);

        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoap();", true);
    }
    protected void lnkfusoap_Click(object sender, EventArgs e)
    {
        try
        {

            hdnrodeditedfuid.Value = string.Empty;
            hdnrodeditedfuieid.Value = string.Empty;
            hdnSoapId.Value = string.Empty;
            BusinessLogic bl = new BusinessLogic();
            LinkButton btn = (LinkButton)(sender);

            if (btn.CommandArgument.Split('|').Count() > 0)
            {
                hdnrodeditedfuid.Value = btn.CommandArgument.Split('|')[0];
            }
            if (btn.CommandArgument.Split('|').Count() > 1)
            {
                hdnrodeditedfuieid.Value = btn.CommandArgument.Split('|')[1];
            }
            if (btn.CommandArgument.Split('|').Count() > 2)
            {
                hdnSoapId.Value = btn.CommandArgument.Split('|')[2];
            }
            DataTable dt = ToDataTable(bl.GetFUDetails(Convert.ToInt32(hdnrodeditedfuieid.Value)));
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientFUId=" + Convert.ToInt32(hdnrodeditedfuid.Value); // query example = "id = 10"

            Session["ieid"] = btn.CommandArgument;
            List<string> _injured = getFUInjuredParts(Convert.ToInt64(hdnrodeditedfuid.Value));
            ViewState["Injuredbodypart"] = _injured;

            //check for the value available or not in the soap table.

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblSoap where PatientIE_ID= " + hdnrodeditedfuieid.Value + " and PatientFU_ID =" + hdnrodeditedfuid.Value + " and Id=" + hdnSoapId.Value);
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            txtothertext.Text = string.Empty;

            if (ds.Tables[0].Rows.Count == 0)
            {
                if (dv != null)
                {

                    lblName.Text = Convert.ToString(dv[0].Row.ItemArray[5]) + " " + Convert.ToString(dv[0].Row.ItemArray[4]);//Last name +First Name;
                    lblDOI.Text = !String.IsNullOrEmpty(Convert.ToString(dv[0].Row.ItemArray[11])) ? Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy") : string.Empty;//DOA
                    txtAdjective.Text = Convert.ToString(dv[0].Row.ItemArray[16]);

                }
                txtCreateSoapDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                bindSoapBodyPart(_injured, true);
                BindSoapAsstPlan();


                bool ShowInitial = false;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/xml/Default_Soap.xml"));
                XmlNodeList nodeList;
                nodeList = xmlDoc.DocumentElement.SelectNodes("/Soaps");
                foreach (XmlNode node in nodeList)
                {
                    ShowInitial = Convert.ToBoolean(node.SelectSingleNode("ShowInitial").InnerText);
                }
                if (ShowInitial)
                {
                    string MP = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["Providers"])))
                    {
                        if (Convert.ToString(Session["Providers"]).Split(' ').Count() > 0)
                        {
                            MP = Convert.ToString(Session["Providers"]).Split(' ')[0].Substring(0, 1);
                        }

                        if (Convert.ToString(Session["Providers"]).Split(' ').Count() > 1)
                        {
                            MP += Convert.ToString(Session["Providers"]).Split(' ')[1].Substring(0, 1);
                        }
                    }
                    txtMAProvider.Text = MP;
                }
                else
                {
                    txtMAProvider.Text = Convert.ToString(Session["Providers"]);
                }
            }
            else
            {
                clearcontrolSoap();
                bindSoapBodyPart(_injured, false);
                txtCreateSoapDate.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CreationDate"])) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CreationDate"]).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
                lblName.Text = Convert.ToString(ds.Tables[0].Rows[0]["PatientName"]);
                lblDOI.Text = !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["DOI"])) ? Convert.ToString(ds.Tables[0].Rows[0]["DOI"]) : string.Empty;
                txtAdjective.Text = Convert.ToString(ds.Tables[0].Rows[0]["Subjective"]);
                txtMAProvider.Text = Convert.ToString(ds.Tables[0].Rows[0]["MAProvider"]); ;
                string temp = Convert.ToString(ds.Tables[0].Rows[0]["Objective"]);
                foreach (var item in temp.Split('$'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        foreach (var item1 in item.Split('|'))
                        {
                            if (item1.Contains('-'))
                            {
                                string chk1 = "chk" + item1.Split('-')[1] + item1.Split('-')[0];

                                //foreach (Control child in pnlCheckbox.Controls)
                                //{
                                //    if (child is RadioButton)
                                //    {
                                //        RadioButton chk = child as RadioButton;
                                //        if (chk.ID == chk1)
                                //        {
                                //            chk.Checked = true;
                                //        }
                                //    }
                                //}
                                selectbodypart(chk1);
                            }
                        }
                    }
                }

                string Treatment = Convert.ToString(ds.Tables[0].Rows[0]["Treatment"]);

                foreach (var item in Treatment.Split('$'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Split('-')[0].Equals("Other10"))
                        {
                            txtothertext.Text = item.Split('-')[1];
                        }
                        else
                        {
                            string checkboxlist = "chk" + item.Split('-')[0];
                            string selectedvalues = item.Split('-')[1];
                            bindEditTreatment(checkboxlist, selectedvalues);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Assessment"])))
                {
                    BindSoapAssestPlanEditValues(repAssestment, Convert.ToString(ds.Tables[0].Rows[0]["Assessment"]));
                }
                else
                { bindSoapAssestPlanPrintvalue(repAssestment, "txtAssestment", hdnAssestment); }
                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Plans"])))
                {
                    BindSoapAssestPlanEditValues(repPlan, Convert.ToString(ds.Tables[0].Rows[0]["Plans"]));
                }
                else
                { bindSoapAssestPlanPrintvalue(repPlan, "txtPlan", hdnPlan); }

            }

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoap();", true);
        }
        catch (Exception ex)
        {
            db.LogError(ex);
            throw;
        }
    }
    protected void btnCreatnew_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btncreate = (LinkButton)(sender);
            LinkButton btn = new LinkButton();
            btn.CommandArgument = btncreate.CommandArgument;
            lnkiesoap_Click(btn, e);
        }
        catch (Exception ex)
        {
            db.LogError(ex);
            throw;
        }
    }
    protected void soapdelete_Click(object sender, EventArgs e)
    {
        try
        {
            string ieid = string.Empty;
            string fuid = string.Empty;
            string soapid = string.Empty;
            LinkButton btn = (LinkButton)(sender);
            string type = btn.CommandArgument.Split('-')[0];
            if (type.Equals("IE"))
            {
                ieid = btn.CommandArgument.Split('-')[1].Split('|')[0];
                soapid = btn.CommandArgument.Split('-')[1].Split('|')[1];
            }
            else if (type.Equals("FU"))
            {
                ieid = btn.CommandArgument.Split('-')[1].Split('|')[0];
                fuid = btn.CommandArgument.Split('-')[1].Split('|')[1];
                soapid = btn.CommandArgument.Split('-')[1].Split('|')[2];
            }



            DBHelperClass dB = new DBHelperClass();
            //string ieid = string.Empty, soapid = string.Empty;

            int val = dB.executeQuery("delete from tblsoap where id=" + soapid);


            if (type.Equals("IE"))
            {
                LinkButton btn1 = new LinkButton();
                btn1.CommandArgument = ieid;
                lnkiesoap_Click1(btn1, e);
            }
            else if (type.Equals("FU"))
            {
                LinkButton btn1 = new LinkButton();
                btn1.CommandArgument = fuid + "|" + ieid;
                lnkfusoap_Click1(btn1, e);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void lnkfusoap_Click1(object sender, EventArgs e)
    {
        try
        {
            btnCreatnewFu.Visible = true;
            btnCreatnew.Visible = false;
            LinkButton btn = (LinkButton)(sender);
            btnCreatnewFu.CommandArgument = Convert.ToString(btn.CommandArgument) + "|" + "0";

            hdnrodeditedfuid.Value = string.Empty;
            hdnrodeditedfuieid.Value = string.Empty;
            hdnSoapId.Value = string.Empty;

            //BusinessLogic bl = new BusinessLogic();
            //LinkButton btn = (LinkButton)(sender);

            hdnrodeditedfuid.Value = btn.CommandArgument.Split('|')[0];
            hdnrodeditedfuieid.Value = btn.CommandArgument.Split('|')[1];

            //DataTable dt = ToDataTable(bl.GetFUDetails(Convert.ToInt32(hdnrodeditedfuieid.Value)));
            //DataView dv = new DataView(dt);
            //dv.RowFilter = "PatientFUId=" + Convert.ToInt32(hdnrodeditedfuid.Value); // query example = "id = 10"

            //Session["ieid"] = btn.CommandArgument;
            List<string> _injured = getFUInjuredParts(Convert.ToInt64(hdnrodeditedfuid.Value));
            ViewState["Injuredbodypart"] = _injured;

            //check for the value available or not in the soap table.

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblSoap where PatientIE_ID= " + hdnrodeditedfuieid.Value + " and PatientFU_ID =" + hdnrodeditedfuid.Value);
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEditSoap.DataSource = ds;
                gvEditSoap.DataBind();
                gvEditSoap.Visible = true;
                lblRecordnotfound.Visible = false;
            }
            else
            {
                gvEditSoap.Visible = false;
                lblRecordnotfound.Visible = true;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoapEditSoap();", true);

            //LinkButton btn = (LinkButton)(sender);
            //btnCreatnew.CommandArgument = Convert.ToString(btn.CommandArgument) + "|" + "0";
            ////check for the value available or not in the soap table.
            //SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
            //DBHelperClass db = new DBHelperClass();
            //string query = ("select * from tblSoap where PatientIE_ID = " + btn.CommandArgument + " and PatientFU_ID is null");
            //SqlCommand cm = new SqlCommand(query, cn);
            //SqlDataAdapter da = new SqlDataAdapter(cm);
            //cn.Open();
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    gvEditSoap.DataSource = ds;
            //    gvEditSoap.DataBind();
            //    gvEditSoap.Visible = true;
            //    lblRecordnotfound.Visible = false;
            //}
            //else
            //{
            //    gvEditSoap.Visible = false;
            //    lblRecordnotfound.Visible = true;
            //}
            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopupSoapEditSoap();", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public void clearcontrolSoap()
    {
        try
        {
            chkIncreaseflexionNeck.Checked = false;
            chkDecreaseflexionNeck.Checked = false;
            chkIncreaseextensionNeck.Checked = false;
            chkDecreaseextensionNeck.Checked = false;
            chkIncreasetilttoleftNeck.Checked = false;
            chkDecreasetilttoleftNeck.Checked = false;
            chkIncreasetilttorightNeck.Checked = false;
            chkDecreasetilttorightNeck.Checked = false;
            chkIncreaseleftrotationNeck.Checked = false;
            chkDecreaseleftrotationNeck.Checked = false;
            chkIncreaseRightrotationNeck.Checked = false;
            chkDecreaseRightrotationNeck.Checked = false;
            //chkspasmNeck.Checked = false;
            chkIncreaseflexionLumber.Checked = false;
            chkDecreaseflexionLumber.Checked = false;
            chkIncreaseextensionLumber.Checked = false;
            chkDecreaseextensionLumber.Checked = false;
            chkIncreasetilttoleftLumber.Checked = false;
            chkDecreasetilttoleftLumber.Checked = false;
            chkIncreasetilttorightLumber.Checked = false;
            chkDecreasetilttorightLumber.Checked = false;
            //chkspasmLumber.Checked = false;
            chkIncreasemildlyTrousic.Checked = false;
            chkDecreasemildlyTrousic.Checked = false;
            chkIncreasemoderatelyTrousic.Checked = false;
            chkDecreasemoderatelyTrousic.Checked = false;
            chkincreaseseverelyTrousic.Checked = false;
            chkDecreaseseverelyTrousic.Checked = false;
            //chkspasmTrousic.Checked = false;
            chkIncreaseflexionRE.Checked = false;
            chkDecreaseflexionRE.Checked = false;
            chkIncreaseextensionRE.Checked = false;
            chkDecreaseextensionRE.Checked = false;
            chkIncreaseflexionLE.Checked = false;
            chkDecreaseflexionLE.Checked = false;
            chkIncreaseextensionLE.Checked = false;
            chkDecreaseextensionLE.Checked = false;
            chkIncreaseflexionRK.Checked = false;
            chkDecreaseflexionRK.Checked = false;
            chkIncreaseextensionRK.Checked = false;
            chkDecreaseextensionRK.Checked = false;
            chkIncreaseflexionLK.Checked = false;
            chkDecreaseflexionLK.Checked = false;
            chkIncreaseextensionLK.Checked = false;
            chkDecreaseextensionLK.Checked = false;
            chkIncreaseflexionRSh.Checked = false;
            chkDecreaseflexionRSh.Checked = false;
            chkIncreaseabductionRSh.Checked = false;
            chkDecreaseabductionRSh.Checked = false;
            chkIncreaseintrotationRSh.Checked = false;
            chkDecreaseintrotationRSh.Checked = false;
            chkIncreaseextrotationRSh.Checked = false;
            chkDecreaseextrotationRSh.Checked = false;
            chkIncreaseflexionLSh.Checked = false;
            chkDecreaseflexionLSh.Checked = false;
            chkIncreaseabductionLSh.Checked = false;
            chkDecreaseabductionLSh.Checked = false;
            chkIncreaseintrotationLSh.Checked = false;
            chkDecreaseintrotationLSh.Checked = false;
            chkIncreaseextrotationLSh.Checked = false;
            chkDecreaseextrotationLSh.Checked = false;
            chkIncreasepalmarflexionRW.Checked = false;
            chkDecreasepalmarflexionRW.Checked = false;
            chkIncreasedorsiflexionRW.Checked = false;
            chkDecreasedorsiflexionRW.Checked = false;
            chkIncreaseulnardeviationRW.Checked = false;
            chkDecreaseulnardeviationRW.Checked = false;
            chkIncreaseradialdeviationRW.Checked = false;
            chkDecreaseradialdeviationRW.Checked = false;
            chkIncreasepalmarflexionLW.Checked = false;
            chkDecreasepalmarflexionLW.Checked = false;
            chkIncreasedorsiflexionLW.Checked = false;
            chkDecreasedorsiflexionLW.Checked = false;
            chkIncreaseulnardeviationLW.Checked = false;
            chkDecreaseulnardeviationLW.Checked = false;
            chkIncreaseradialdeviationLW.Checked = false;
            chkDecreaseradialdeviationLW.Checked = false;
            chkIncreaseflexionRHIP.Checked = false;
            chkDecreaseflexionRHIP.Checked = false;
            chkIncreaseextensionRHIP.Checked = false;
            chkDecreaseextensionRHIP.Checked = false;
            chkIncreaseabductionRHIP.Checked = false;
            chkDecreaseabductionRHIP.Checked = false;
            chkIncreaseintrotationRHIP.Checked = false;
            chkDecreaseintrotationRHIP.Checked = false;
            chkIncreaseflexionLHIP.Checked = false;
            chkDecreaseflexionLHIP.Checked = false;
            chkIncreaseextensionLHIP.Checked = false;
            chkDecreaseextensionLHIP.Checked = false;
            chkIncreaseabductionLHIP.Checked = false;
            chkDecreaseabductionLHIP.Checked = false;
            chkIncreaseintrotationLHIP.Checked = false;
            chkDecreaseintrotationLHIP.Checked = false;
            chkIncreasedorsiflexionRA.Checked = false;
            chkDecreasedorsiflexionRA.Checked = false;
            chkIncreaseplantarflexionRA.Checked = false;
            chkDecreaseplantarflexionRA.Checked = false;
            chkIncreaseinversionRA.Checked = false;
            chkDecreaseinversionRA.Checked = false;
            chkIncreaseeversionRA.Checked = false;
            chkDecreaseeversionRA.Checked = false;
            chkIncreasedorsiflexionLA.Checked = false;
            chkDecreasedorsiflexionLA.Checked = false;
            chkIncreaseplantarflexionLA.Checked = false;
            chkDecreaseplantarflexionLA.Checked = false;
            chkIncreaseinversionLA.Checked = false;
            chkDecreaseinversionLA.Checked = false;
            chkIncreaseeversionLA.Checked = false;
            chkDecreaseeversionLA.Checked = false;

        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void btnCreatnewFu_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btncreate = (LinkButton)(sender);
            LinkButton btn = new LinkButton();
            btn.CommandArgument = btncreate.CommandArgument;
            lnkfusoap_Click(btn, e);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void GetMAandProviders()
    {
        BusinessLogic bl = new BusinessLogic();
        txtMAProviderrod.Items.Clear();
        txtMAProviderrod.Items.Insert(0, new ListItem("", " "));
        foreach (User user in bl.getAllProvidersAndMA())
        {
            if (user.Designation != null && user.Designation.Equals("Provider"))
            {
                txtMAProviderrod.Items.Add(new ListItem(user.FirstName + ' ' + user.LastName, user.UserId.ToString()));
            }
        }
    }

}