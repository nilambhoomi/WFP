using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;

public partial class SoapReport : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            LoadPatientIE("", 1);

        }
    }

    private void LoadPatientIE(string query, int pageindex)
    {
        try
        {
            int totalcount;
            DataSet dt = new DataSet();

            dt = db.PatientIE_getAll(query, pageindex, 10, out totalcount);
            if (dt.Tables[0].Rows.Count > 0)
            {
                rpview.DataSource = dt;
                rpview.DataBind();
            }
            else
            {
                rpview.DataSource = null;
                rpview.DataBind();
            }
            PopulatePager(totalcount, pageindex);
            //lblcount.Text = totalcount.ToString();
        }
        catch (Exception ex)
        {
        }
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        List<ListItem> pages = new List<ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;

        //Calculate the Start and End Index of pages to be displayed.
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(10));
        int pageCount = (int)Math.Ceiling(dblPageCount);

        startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }

        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }

        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }

        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("First", "1"));
        }

        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        }

        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        }

        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new ListItem("Last", pageCount.ToString()));
        }

        if (recordCount > 0)
        {
            lbl_page_no.InnerText = currentPage.ToString();
            lbl_total_page.InnerText = pageCount.ToString();


            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
        else
        {
            div_page.Style.Add("display", "none");
            rptPager.DataSource = null;
            rptPager.DataBind();
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", pageIndex);
        }
        else
            this.LoadPatientIE("", pageIndex);
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
    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        string filename = Convert.ToString(btn.CommandArgument.Split('~')[1]);
        generatereport(Convert.ToInt32(Convert.ToString(btn.CommandArgument).Split('~')[0]), filename);
        if (filename != "")
        {

            string path = Server.MapPath("~/document/SoapReport/"+filename + ".docx");

            System.IO.FileInfo file = new System.IO.FileInfo(path);

            if (file.Exists)
            {

                Response.Clear();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                Response.WriteFile(file.FullName);

                Response.End();

            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }

    }
    public void generatereport(int IEid, string filenametocreate)
    {
        try
        {
            string query = "select PatientName,DOI,CONVERT(VARCHAR,CONVERT(date,dos),101) 'DOS',Subjective,PrintObjective,PrintTreatment,AssessmentContent,PlansContent,MAProvider from tblsoap where PatientIE_ID=" + IEid;
            string condition = "";
            string condition1 = null;
            condition1 = " AND CONVERT(date,dos) BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101)";
            query += condition1;
            DataTable dt = null;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
            {

                SqlCommand cm = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {

                string fileName = Server.MapPath("~/document/SoapReport/" + filenametocreate + ".docx");
                using (DocX doc = DocX.Create(fileName))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        doc.InsertParagraph("SOAP").Bold().FontSize(10d).SpacingAfter(10d).Font("Times New Roman").Alignment = Alignment.center;
                         doc.InsertParagraph("This session is held by telemedicine video on a secure HIPAA-compliant forum due to the Corona virus (Covid-19). The patient has provided verbal consent to proceed.").FontSize(10d).SpacingAfter(10d).Font("Times New Roman");
                        doc.InsertParagraph("Patient Name: ").Bold().FontSize(10d).Append(Convert.ToString(r["PatientName"]).ToUpper()).Bold().FontSize(10d).Font("Times New Roman");
                        doc.InsertParagraph("DOI: ").Bold().FontSize(10d).Append(Convert.ToString(r["DOI"])).FontSize(10d).Font("Times New Roman");
                        if (!string.IsNullOrEmpty(Convert.ToString(r["DOS"])))
                        {
                            doc.InsertParagraph("DOS: ").Bold().FontSize(10d).Append(Convert.ToString(r["DOS"])).FontSize(10d).SpacingAfter(10d).Font("Times New Roman");
                        }
                        doc.InsertParagraph("Subjective: ").FontSize(10d).Bold().Append(Convert.ToString(r["Subjective"])).FontSize(10d).Font("Times New Roman");
                        doc.InsertParagraph("Objective: ").Bold().FontSize(10d).Append(Convert.ToString(r["PrintObjective"])).FontSize(10d).Font("Times New Roman");
                        if (!string.IsNullOrEmpty(Convert.ToString(r["PrintTreatment"])))
                        {
                            string treatment = string.Empty;
                            foreach (var item in Convert.ToString(r["PrintTreatment"]).Split('$'))
                            {
                                string input = Convert.ToString(item);
                                //string pattern = @"-|";
                                //string replace = "";
                                //string result = Regex.Replace(input, pattern, replace);
                                int Place = input.IndexOf("-|");
                                string result = string.Empty;
                                if (Place > 0)
                                {
                                    result = input.Remove(Place, "-|".Length).Insert(Place, "- ");
                                }
                                if (!string.IsNullOrEmpty(result))
                                {
                                    treatment += Environment.NewLine + result.Replace("|", ", ");
                                }

                                // Convert.ToString(item).Replace('-',' ').Replace('|',',')+" "+ Environment.NewLine;
                            }
                            doc.InsertParagraph("Treatment: ").Bold().FontSize(10d).Append(treatment.TrimEnd('\r', '\n')).FontSize(10d).Font("Times New Roman");
                        }
                        doc.InsertParagraph("Assessment: ").Bold().FontSize(10d).Append(Convert.ToString(r["AssessmentContent"]).Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty)).FontSize(10d).Font("Times New Roman");
                        doc.InsertParagraph("Plans: ").Bold().FontSize(10d).Append(Convert.ToString(r["PlansContent"]).Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty)).FontSize(10d).Font("Times New Roman");

                        // Add an image into the document.  
                        Xceed.Words.NET.Image image = null;
                        if (File.Exists(Server.MapPath("~/img/Sign/" + Convert.ToString(r["MAProvider"]) + ".jpg")))
                        {
                            image = doc.AddImage(Server.MapPath("~/img/Sign/" + Convert.ToString(r["MAProvider"]) + ".jpg"));
                        }
                        else
                        {
                            image = doc.AddImage(Server.MapPath("~/img/Sign/Blank.jpg"));
                        }

                        // Create a picture (A custom view of an Image).
                        Picture picture = image.CreatePicture();

                        doc.InsertParagraph().AppendPicture(picture).UnderlineColor(System.Drawing.Color.Black);

                        doc.InsertParagraph("Physical Therapist").FontSize(10d).Font("Times New Roman");
                    }

                    // Add Headers and Footers to the document.
                    doc.AddHeaders();
                    doc.AddFooters();

                    // Force the first page to have a different Header and Footer.
                    doc.DifferentFirstPage = true;

                    // Force odd & even pages to have different Headers and Footers.
                    doc.DifferentOddAndEvenPages = true;

                    // Insert a Paragraph into the first Header.
                    doc.Headers.First.InsertParagraph("Physical Therapy").Bold().UnderlineColor(System.Drawing.Color.Black).Alignment = Alignment.center;

                    // Insert a Paragraph into the even Header.
                    doc.Headers.Even.InsertParagraph("Physical Therapy").Bold().UnderlineColor(System.Drawing.Color.Black).Alignment = Alignment.center;

                    // Insert a Paragraph into the odd Header.
                    doc.Headers.Odd.InsertParagraph("Physical Therapy").Bold().UnderlineColor(System.Drawing.Color.Black).Alignment = Alignment.center;

                    // Add the page number in the first Footer.
                    doc.Footers.First.InsertParagraph("Page").AppendPageNumber(PageNumberFormat.normal);

                    // Add the page number in the even Footers.
                    doc.Footers.Even.InsertParagraph("Page").AppendPageNumber(PageNumberFormat.normal);

                    // Add the page number in the odd Footers.
                    doc.Footers.Odd.InsertParagraph("Page").AppendPageNumber(PageNumberFormat.normal);
                    doc.Save();


                }
            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID, LastName, FirstName from tblPatientMaster where FirstName like '%" + prefix + "%' OR LastName Like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
            name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }
        return patient.ToArray();
    }
    protected void txt_name_TextChanged(object sender, EventArgs e)
    {
        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", 1);
        }
    }
    public void bindEditData(string PatientIEid)
    {
        try
        {

            string query = "select * from View_PatientIE where PatientIE_ID=" + PatientIEid;

            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["FirstName"].ToString()))
                {
                    Session["fname"] = " ";
                }
                else
                {
                    Session["fname"] = ds.Tables[0].Rows[0]["FirstName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["LastName"].ToString()))
                {
                    Session["lname"] = " ";
                }
                else
                {
                    Session["lname"] = ds.Tables[0].Rows[0]["LastName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MiddleName"].ToString()))
                {
                    Session["mname"] = " ";
                }
                else
                {
                    Session["mname"] = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["eMail"].ToString()))
                {
                    Session["eMail"] = " ";
                }
                else
                {
                    Session["eMail"] = ds.Tables[0].Rows[0]["eMail"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOA"].ToString()))
                {
                    Session["doa"] = " ";
                }
                else
                {
                    Session["doa"] = ds.Tables[0].Rows[0]["DOA"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOE"].ToString()))
                {
                    Session["doe"] = " ";
                }
                else
                {
                    Session["doe"] = ds.Tables[0].Rows[0]["DOE"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["SSN"].ToString()))
                {
                    Session["ssn"] = " ";
                }
                else
                {
                    Session["ssn"] = ds.Tables[0].Rows[0]["SSN"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Address1"].ToString()))
                {
                    Session["Address"] = " ";
                }
                else
                {
                    Session["Address"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsAddress1"].ToString()))
                {
                    Session["InsAddress"] = " ";
                }
                else
                {
                    Session["InsAddress"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone2"].ToString()))
                {
                    Session["mob"] = " ";
                }
                else
                {
                    Session["mob"] = ds.Tables[0].Rows[0]["Phone2"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsPhone"].ToString()))
                {
                    Session["InsPhone"] = " ";
                }
                else
                {
                    Session["InsPhone"] = ds.Tables[0].Rows[0]["InsPhone"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["City"].ToString()))
                {
                    Session["city"] = " ";
                }
                else
                {
                    Session["city"] = ds.Tables[0].Rows[0]["City"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCity"].ToString()))
                {
                    Session["Inscity"] = " ";
                }
                else
                {
                    Session["Inscity"] = ds.Tables[0].Rows[0]["InsCity"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["State"].ToString()))
                {
                    Session["state"] = " ";
                }
                else
                {
                    Session["state"] = ds.Tables[0].Rows[0]["State"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsState"].ToString()))
                {
                    Session["Insstate"] = " ";
                }
                else
                {
                    Session["Insstate"] = ds.Tables[0].Rows[0]["InsState"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    Session["zip"] = " ";
                }
                else
                {
                    Session["zip"] = ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsZip"].ToString()))
                {
                    Session["Inszip"] = " ";
                }
                else
                {
                    Session["Inszip"] = ds.Tables[0].Rows[0]["InsZip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone"].ToString()))
                {
                    Session["Phone"] = " ";
                }
                else
                {
                    Session["Phone"] = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["work_phone"].ToString()))
                {
                    Session["work_phone"] = " ";
                }
                else
                {
                    Session["work_phone"] = ds.Tables[0].Rows[0]["work_phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Sex"].ToString()))
                {
                    Session["sex"] = " ";
                }
                else
                {
                    Session["sex"] = ds.Tables[0].Rows[0]["Sex"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCo"].ToString()))
                {
                    Session["InsCo"] = " ";
                }
                else
                {
                    Session["InsCo"] = ds.Tables[0].Rows[0]["InsCo"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["policy_no"].ToString()))
                {
                    Session["policy_no"] = " ";
                }
                else
                {
                    Session["policy_no"] = ds.Tables[0].Rows[0]["policy_no"].ToString();
                }


                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ClaimNumber"].ToString()))
                {
                    Session["ClaimNumber"] = " ";
                }
                else
                {
                    Session["ClaimNumber"] = ds.Tables[0].Rows[0]["ClaimNumber"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Location"].ToString()))
                {
                    Session["LocationPdf"] = " ";
                }
                else
                {
                    Session["LocationPdf"] = ds.Tables[0].Rows[0]["Location"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Attorney"].ToString()))
                {
                    Session["Attorney"] = " ";
                }
                else
                {
                    Session["Attorney"] = ds.Tables[0].Rows[0]["Attorney"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyAdd"].ToString()))
                {
                    Session["AttorneyAdd"] = " ";
                }
                else
                {
                    Session["AttorneyAdd"] = ds.Tables[0].Rows[0]["AttorneyAdd"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyPhno"].ToString()))
                {
                    Session["AttorneyPhno"] = " ";
                }
                else
                {
                    Session["AttorneyPhno"] = ds.Tables[0].Rows[0]["AttorneyPhno"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Adjuster"].ToString()))
                {
                    Session["Adjuster"] = " ";
                }
                else
                {
                    Session["Adjuster"] = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Compensation"].ToString()))
                {
                    Session["Compensation"] = " ";
                }
                else
                {
                    Session["Compensation"] = ds.Tables[0].Rows[0]["Compensation"].ToString();
                }
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AGE"].ToString()))
                //{
                //    Session["AGE"] = " ";
                //}
                //else
                //{
                //    //Session["AGE"] = ds.Tables[0].Rows[0]["AGE"].ToString();
                //}

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MA_Providers"].ToString()))
                {
                    Session["MA_Providers"] = " ";
                }
                else
                {
                    Session["MA_Providers"] = ds.Tables[0].Rows[0]["MA_Providers"].ToString();
                }

                if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                    Session["dob"] = dob.ToString("MM/dd/yyyy");
                    Session["AGE"] = CalculateAge(dob);
                }
                else
                {
                    Session["dob"] = " ";
                    Session["AGE"] = " ";
                }
            }
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
    }
    private static int CalculateAge(DateTime dateOfBirth)
    {
        int age = 0;
        age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age = age - 1;

        return age;
    }
}