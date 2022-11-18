using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {
            if (Request["id"] != null)
            {
                string id = Request.QueryString["id"];
                BindUserDetails(id);
            }
        }
    }

    protected void BindUserDetails(string userId = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("GetAllUser", con);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtEmail.Text = ds.Tables[0].Rows[0]["eMailID"].ToString();
                txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txtLoginID.Text = ds.Tables[0].Rows[0]["LoginID"].ToString();
                txtMiddleName.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                txtDesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();
                txtPassowrd.Attributes.Add("value", ds.Tables[0].Rows[0]["Password"].ToString());
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            if (Request["id"] != null)
            {
                query = "update tblUserMaster set LoginID='" + txtLoginID.Text + "',Password='" + txtPassowrd.Text + "',";
                query = query + " Designation='" + txtDesignation.Text + "',FirstName='" + txtFirstName.Text + "',";
                query = query + " LastName='" + txtLastName.Text + "',MiddleName='" + txtMiddleName.Text + "',";
                query = query + " eMailID='" + txtEmail.Text + "' where User_ID=" + Request["id"].ToString();
            }
            else
            {
                query = "insert into tblUserMaster values('" + txtLoginID.Text + "','" + txtPassowrd.Text + "','" + txtDesignation.Text + "',";
                query = query + " '" + txtFirstName.Text + "','" + txtLastName.Text + "','" + txtMiddleName.Text + "','" + txtEmail.Text + "',Null,'admin',GETDATE()) ";
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            Response.Redirect("ManageUser.aspx");
        }
        catch (Exception ex)
        {
        }
    }

 

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetDesignations(string prefix)
    {
        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select distinct Designation from tblUserMaster";
                cmd.CommandType = CommandType.Text;
              
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["Designation"].ToString());
                    }
                }
                conn.Close();
            }
            return _patients.ToArray();
        }
    }
}