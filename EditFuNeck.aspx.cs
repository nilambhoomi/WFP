using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class EditFuNeck : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Neck";
    ILog log = log4net.LogManager.GetLogger(typeof(EditFuNeck));


    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Neck";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            checkTP();
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
                // BindROM();
                bindDropdown();
                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpNeck WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpNeck WHERE PatientIE_ID= " + _CurIEid + "");
                SqlCommand cm1 = new SqlCommand(query1, cn);
                SqlDataAdapter IEda = new SqlDataAdapter(cm1);
                cn.Open();
                DataSet IEds = new DataSet();
                IEda.Fill(IEds);
                cn.Close();
                DataRow FUrw = FUds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("FuCount") == 0);
                DataRow IErw = IEds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("IECount") == 0);
                if (FUrw == null)
                {


                    PopulateUI(_FuId);
                    BindDCDataGrid();
                    BindDataGrid();

                }
                else if (IErw == null)
                {
                    PopulateIEUI(_CurIEid);
                    BindDCDataGrid();
                    BindDataGrid();
                }
                else
                {
                    PopulateUIDefaults();
                    BindDataGrid();
                }
            }
            else
            {
                Response.Redirect("EditFU.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuNeck for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }
    [System.Web.Services.WebMethod]
    public string SaveUI_Ajax()
    {
        string ieMode = "New";
        string ieID = Session["PatientIE_ID"].ToString();
        bool bpChecked = true;
        // string result=  SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";

        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpNeck WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

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
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["PainScale"] = txtPainScale.Text.ToString();
            TblRow["Sharp"] = chkSharp.Checked;
            TblRow["Electric"] = chkElectric.Checked;
            TblRow["Shooting"] = chkShooting.Checked;
            TblRow["Throbbling"] = chkThrobbling.Checked;
            TblRow["Pulsating"] = chkPulsating.Checked;
            TblRow["Dull"] = chkDull.Checked;
            TblRow["Achy"] = chkAchy.Checked;
            TblRow["constant"] = chkContent.Checked;
            TblRow["intermittent"] = chkIntermittent.Checked;
            TblRow["Radiates"] = txtRadiates.Text.ToString();
            TblRow["Numbness"] = chkNumbness.Checked;
            TblRow["Tingling"] = chkTingling.Checked;
            TblRow["Burning"] = chkBurning.Checked;
            TblRow["BurningTo"] = txtBurningTo.Text.ToString();
            TblRow["ShoulderLeft1"] = chkShoulderLeft1.Checked;
            TblRow["ShoulderRight1"] = chkShoulderRight1.Checked;
            TblRow["ShoulderBilateral1"] = chkShoulderBilateral1.Checked;
            TblRow["ScapulaLeft1"] = chkScapulaLeft1.Checked;
            TblRow["ScapulaRight1"] = chkScapulaRight1.Checked;
            TblRow["ScapulaBilateral1"] = chkScapulaBilateral1.Checked;
            TblRow["ArmLeft1"] = chkArmLeft1.Checked;
            TblRow["ArmRight1"] = chkArmRight1.Checked;
            TblRow["ArmBilateral1"] = chkArmBilateral1.Checked;
            TblRow["ForearmLeft1"] = chkForearmLeft1.Checked;
            TblRow["ForearmRight1"] = chkForearmRight1.Checked;
            TblRow["ForearmBilateral1"] = chkForearmBilateral1.Checked;
            TblRow["HandLeft1"] = chkHandLeft1.Checked;
            TblRow["HandRight1"] = chkHandRight1.Checked;
            TblRow["HandBilateral1"] = chkHandBilateral1.Checked;
            TblRow["WristLeft1"] = chkWristLeft1.Checked;
            TblRow["WristRight1"] = chkWristRight1.Checked;
            TblRow["WristBilateral1"] = chkWristBilateral1.Checked;
            TblRow["C1stDigitLeft1"] = chk1stDigitLeft1.Checked;
            TblRow["C1stDigitRight1"] = chk1stDigitRight1.Checked;
            TblRow["C1stDigitBilateral1"] = chk1stDigitBilateral1.Checked;
            TblRow["C2ndDigitLeft1"] = chk2ndDigitLeft1.Checked;
            TblRow["C2ndDigitRight1"] = chk2ndDigitRight1.Checked;
            TblRow["C2ndDigitBilateral1"] = chk2ndDigitBilateral1.Checked;
            TblRow["C3rdDigitLeft1"] = chk3rdDigitLeft1.Checked;
            TblRow["C3rdDigitRight1"] = chk3rdDigitRight1.Checked;
            TblRow["C3rdDigitBilateral1"] = chk3rdDigitBilateral1.Checked;
            TblRow["C4thDigitLeft1"] = chk4thDigitLeft1.Checked;
            TblRow["C4thDigitRight1"] = chk4thDigitRight1.Checked;
            TblRow["C4thDigitBilateral1"] = chk4thDigitBilateral1.Checked;
            TblRow["C5thDigitLeft1"] = chk5thDigitLeft1.Checked;
            TblRow["C5thDigitRight1"] = chk5thDigitRight1.Checked;
            TblRow["C5thDigitBilateral1"] = chk5thDigitBilateral1.Checked;
            TblRow["ShoulderLeft2"] = chkShoulderLeft2.Checked;
            TblRow["ShoulderRight2"] = chkShoulderRight2.Checked;
            TblRow["ShoulderBilateral2"] = chkShoulderBilateral2.Checked;
            TblRow["ScapulaLeft2"] = chkScapulaLeft2.Checked;
            TblRow["ScapulaRight2"] = chkScapulaRight2.Checked;
            TblRow["ScapulaBilateral2"] = chkScapulaBilateral2.Checked;
            TblRow["ArmLeft2"] = chkArmLeft2.Checked;
            TblRow["ArmRight2"] = chkArmRight2.Checked;
            TblRow["ArmBilateral2"] = chkArmBilateral2.Checked;
            TblRow["ForearmLeft2"] = chkForearmLeft2.Checked;
            TblRow["ForearmRight2"] = chkForearmRight2.Checked;
            TblRow["ForearmBilateral2"] = chkForearmBilateral2.Checked;
            TblRow["HandLeft2"] = chkHandLeft2.Checked;
            TblRow["HandRight2"] = chkHandRight2.Checked;
            TblRow["HandBilateral2"] = chkHandBilateral2.Checked;
            TblRow["WristLeft2"] = chkWristLeft2.Checked;
            TblRow["WristRight2"] = chkWristRight2.Checked;
            TblRow["WristBilateral2"] = chkWristBilateral2.Checked;
            TblRow["C1stDigitLeft2"] = chk1stDigitLeft2.Checked;
            TblRow["C1stDigitRight2"] = chk1stDigitRight2.Checked;
            TblRow["C1stDigitBilateral2"] = chk1stDigitBilateral2.Checked;
            TblRow["C2ndDigitLeft2"] = chk2ndDigitLeft2.Checked;
            TblRow["C2ndDigitRight2"] = chk2ndDigitRight2.Checked;
            TblRow["C2ndDigitBilateral2"] = chk2ndDigitBilateral2.Checked;
            TblRow["C3rdDigitLeft2"] = chk3rdDigitLeft2.Checked;
            TblRow["C3rdDigitRight2"] = chk3rdDigitRight2.Checked;
            TblRow["C3rdDigitBilateral2"] = chk3rdDigitBilateral2.Checked;
            TblRow["C4thDigitLeft2"] = chk4thDigitLeft2.Checked;
            TblRow["C4thDigitRight2"] = chk4thDigitRight2.Checked;
            TblRow["C4thDigitBilateral2"] = chk4thDigitBilateral2.Checked;
            TblRow["C5thDigitLeft2"] = chk5thDigitLeft2.Checked;
            TblRow["C5thDigitRight2"] = chk5thDigitRight2.Checked;
            TblRow["C5thDigitBilateral2"] = chk5thDigitBilateral2.Checked;
            TblRow["WeeknessIn"] = txtWeeknessIn.Text.ToString();
            TblRow["WorseSitting"] = chkWorseSitting.Checked;
            TblRow["WorseStanding"] = chkWorseStanding.Checked;
            TblRow["WorseLyingDown"] = chkWorseLyingDown.Checked;
            TblRow["WorseMovement"] = chkWorseMovement.Checked;
            TblRow["WorseSeatingtoStandingUp"] = chkWorseSeatingtoStandingUp.Checked;
            TblRow["WorseWalking"] = chkWorseWalking.Checked;
            TblRow["WorseClimbingStairs"] = chkWorseClimbingStairs.Checked;
            TblRow["WorseDescendingStairs"] = chkWorseDescendingStairs.Checked;
            TblRow["WorseDriving"] = chkWorseDriving.Checked;
            TblRow["WorseWorking"] = chkWorseWorking.Checked;
            TblRow["WorseBending"] = chkWorseBending.Checked;
            TblRow["WorseLifting"] = chkWorseLifting.Checked;
            TblRow["WorseTwisting"] = chkWorseTwisting.Checked;
            TblRow["ImprovedResting"] = chkImprovedResting.Checked;
            TblRow["ImprovedMedication"] = chkImprovedMedication.Checked;
            TblRow["ImprovedTherapy"] = chkImprovedTherapy.Checked;
            TblRow["ImprovedSleeping"] = chkImprovedSleeping.Checked;
            TblRow["ImprovedMovement"] = chkImprovedMovement.Checked;
            TblRow["FwdFlexRight"] = txtFwdFlexRight.Text.ToString(); //ROM
            TblRow["FwdFlexLeft"] = txtFwdFlexNormal.Text.ToString();// txtFwdFlexLeft.tex;
            TblRow["ExtensionRight"] = txtExtensionRight.Text.ToString();
            TblRow["ExtensionLeft"] = txtExtensionNormal.Text.ToString();// txtExtensionR;
            TblRow["RotationRight"] = txtRotationRight.Text.ToString();
            TblRow["RotationLeft"] = txtRotationLeft.Text.ToString();
            TblRow["RotationNormal"] = txtRotationNormal.Text.ToString();
            TblRow["LateralFlexRight"] = txtLateralFlexRight.Text.ToString();
            TblRow["LateralFlexLeft"] = txtLateralFlexLeft.Text.ToString();
            TblRow["LateralFlexNormal"] = txtLateralFlexNormal.Text.ToString();
            TblRow["PalpationAt"] = txtPalpationAt.Text.ToString();
            TblRow["Levels"] = ddlLevels.Text;
            //TblRow["Bilaterally"] = chkBilaterally.Checked;
            //TblRow["CSRight"] = chkCSRight.Checked;
            //TblRow["CSLeft"] = chkCSLeft.Checked;
            //TblRow["RightGreaterLeft"] = chkRightGreaterLeft.Checked;
            //TblRow["LeftGreaterRight"] = chkLeftGreaterRight.Checked;
            TblRow["Spurlings"] = cboSpurlings.Text.ToString();
            TblRow["Distraction"] = cboDistraction.Text.ToString();
            TblRow["TPSide1"] = cboTPSide1.Text.ToString();
            TblRow["TPText1"] = txtTPText1.Text.ToString();
            TblRow["TPSide2"] = cboTPSide2.Text.ToString();
            TblRow["TPText2"] = txtTPText2.Text.ToString();
            TblRow["TPSide3"] = cboTPSide3.Text.ToString();
            TblRow["TPText3"] = txtTPText3.Text.ToString();
            TblRow["TPside4"] = cboTPSide4.Text.ToString();
            TblRow["TPText4"] = txtTPText4.Text.ToString();
            TblRow["TPSide5"] = cboTPSide5.Text.ToString();
            TblRow["TPText5"] = txtTPText5.Text.ToString();
            TblRow["TPSide6"] = cboTPSide6.Text.ToString();
            TblRow["TPText6"] = txtTPText6.Text.ToString();
            TblRow["TPSide7"] = cboTPSide7.Text.ToString();
            TblRow["TPText7"] = txtTPTex7t.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["Xrays"] = false;//TBD
            TblRow["TPEpidular"] = false;//TBD
            TblRow["NoOfTPInjections"] = "";// txtPainScale;
            TblRow["ScheduleEpidural"] = false;//txtPainScale;
            TblRow["NewMBB"] = false;// txtPainScale;
            TblRow["SPTPMBB"] = false;//txtPainScale;
            TblRow["ISFirst"] = true;

            TblRow["WorseOther"] = txtWorseOtherText.Text.ToString();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
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

        if (_ieMode == "New")
            return "Neck has been added...";
        else if (_ieMode == "Update")
            return "Neck has been updated...";
        else if (_ieMode == "Delete")
            return "Neck has been deleted...";
        else
            return "";
    }

    public string SaveUI(string ieID, string fuID, string fuMode, bool bpIsChecked)
    {
        long _fuID = Convert.ToInt64(fuID);
        string _fuMode = "";

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + _fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpIsChecked == true)
            _fuMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpIsChecked == false)
            _fuMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpIsChecked == false)
            _fuMode = "Delete";
        else
            _fuMode = "Update";

        if (_fuMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_fuMode == "Update" || _fuMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_fuMode == "Update" || _fuMode == "New")
        {

            TblRow["PatientFU_ID"] = _fuID;
            TblRow["PainScale"] = txtPainScale.Text.ToString();
            TblRow["Sharp"] = chkSharp.Checked;
            TblRow["Electric"] = chkElectric.Checked;
            TblRow["Shooting"] = chkShooting.Checked;
            TblRow["Throbbling"] = chkThrobbling.Checked;
            TblRow["Pulsating"] = chkPulsating.Checked;
            TblRow["Dull"] = chkDull.Checked;
            TblRow["Achy"] = chkAchy.Checked;
            TblRow["constant"] = chkContent.Checked;
            TblRow["intermittent"] = chkIntermittent.Checked;
            TblRow["Radiates"] = txtRadiates.Text.ToString();
            TblRow["Numbness"] = chkNumbness.Checked;
            TblRow["Tingling"] = chkTingling.Checked;
            TblRow["Burning"] = chkBurning.Checked;
            TblRow["BurningTo"] = txtBurningTo.Text.ToString();
            TblRow["ShoulderLeft1"] = chkShoulderLeft1.Checked;
            TblRow["ShoulderRight1"] = chkShoulderRight1.Checked;
            TblRow["ShoulderBilateral1"] = chkShoulderBilateral1.Checked;
            TblRow["ScapulaLeft1"] = chkScapulaLeft1.Checked;
            TblRow["ScapulaRight1"] = chkScapulaRight1.Checked;
            TblRow["ScapulaBilateral1"] = chkScapulaBilateral1.Checked;
            TblRow["ArmLeft1"] = chkArmLeft1.Checked;
            TblRow["ArmRight1"] = chkArmRight1.Checked;
            TblRow["ArmBilateral1"] = chkArmBilateral1.Checked;
            TblRow["ForearmLeft1"] = chkForearmLeft1.Checked;
            TblRow["ForearmRight1"] = chkForearmRight1.Checked;
            TblRow["ForearmBilateral1"] = chkForearmBilateral1.Checked;
            TblRow["HandLeft1"] = chkHandLeft1.Checked;
            TblRow["HandRight1"] = chkHandRight1.Checked;
            TblRow["HandBilateral1"] = chkHandBilateral1.Checked;
            TblRow["WristLeft1"] = chkWristLeft1.Checked;
            TblRow["WristRight1"] = chkWristRight1.Checked;
            TblRow["WristBilateral1"] = chkWristBilateral1.Checked;
            TblRow["C1stDigitLeft1"] = chk1stDigitLeft1.Checked;
            TblRow["C1stDigitRight1"] = chk1stDigitRight1.Checked;
            TblRow["C1stDigitBilateral1"] = chk1stDigitBilateral1.Checked;
            TblRow["C2ndDigitLeft1"] = chk2ndDigitLeft1.Checked;
            TblRow["C2ndDigitRight1"] = chk2ndDigitRight1.Checked;
            TblRow["C2ndDigitBilateral1"] = chk2ndDigitBilateral1.Checked;
            TblRow["C3rdDigitLeft1"] = chk3rdDigitLeft1.Checked;
            TblRow["C3rdDigitRight1"] = chk3rdDigitRight1.Checked;
            TblRow["C3rdDigitBilateral1"] = chk3rdDigitBilateral1.Checked;
            TblRow["C4thDigitLeft1"] = chk4thDigitLeft1.Checked;
            TblRow["C4thDigitRight1"] = chk4thDigitRight1.Checked;
            TblRow["C4thDigitBilateral1"] = chk4thDigitBilateral1.Checked;
            TblRow["C5thDigitLeft1"] = chk5thDigitLeft1.Checked;
            TblRow["C5thDigitRight1"] = chk5thDigitRight1.Checked;
            TblRow["C5thDigitBilateral1"] = chk5thDigitBilateral1.Checked;
            TblRow["ShoulderLeft2"] = chkShoulderLeft2.Checked;
            TblRow["ShoulderRight2"] = chkShoulderRight2.Checked;
            TblRow["ShoulderBilateral2"] = chkShoulderBilateral2.Checked;
            TblRow["ScapulaLeft2"] = chkScapulaLeft2.Checked;
            TblRow["ScapulaRight2"] = chkScapulaRight2.Checked;
            TblRow["ScapulaBilateral2"] = chkScapulaBilateral2.Checked;
            TblRow["ArmLeft2"] = chkArmLeft2.Checked;
            TblRow["ArmRight2"] = chkArmRight2.Checked;
            TblRow["ArmBilateral2"] = chkArmBilateral2.Checked;
            TblRow["ForearmLeft2"] = chkForearmLeft2.Checked;
            TblRow["ForearmRight2"] = chkForearmRight2.Checked;
            TblRow["ForearmBilateral2"] = chkForearmBilateral2.Checked;
            TblRow["HandLeft2"] = chkHandLeft2.Checked;
            TblRow["HandRight2"] = chkHandRight2.Checked;
            TblRow["HandBilateral2"] = chkHandBilateral2.Checked;
            TblRow["WristLeft2"] = chkWristLeft2.Checked;
            TblRow["WristRight2"] = chkWristRight2.Checked;
            TblRow["WristBilateral2"] = chkWristBilateral2.Checked;
            TblRow["C1stDigitLeft2"] = chk1stDigitLeft2.Checked;
            TblRow["C1stDigitRight2"] = chk1stDigitRight2.Checked;
            TblRow["C1stDigitBilateral2"] = chk1stDigitBilateral2.Checked;
            TblRow["C2ndDigitLeft2"] = chk2ndDigitLeft2.Checked;
            TblRow["C2ndDigitRight2"] = chk2ndDigitRight2.Checked;
            TblRow["C2ndDigitBilateral2"] = chk2ndDigitBilateral2.Checked;
            TblRow["C3rdDigitLeft2"] = chk3rdDigitLeft2.Checked;
            TblRow["C3rdDigitRight2"] = chk3rdDigitRight2.Checked;
            TblRow["C3rdDigitBilateral2"] = chk3rdDigitBilateral2.Checked;
            TblRow["C4thDigitLeft2"] = chk4thDigitLeft2.Checked;
            TblRow["C4thDigitRight2"] = chk4thDigitRight2.Checked;
            TblRow["C4thDigitBilateral2"] = chk4thDigitBilateral2.Checked;
            TblRow["C5thDigitLeft2"] = chk5thDigitLeft2.Checked;
            TblRow["C5thDigitRight2"] = chk5thDigitRight2.Checked;
            TblRow["C5thDigitBilateral2"] = chk5thDigitBilateral2.Checked;
            TblRow["WeeknessIn"] = txtWeeknessIn.Text.ToString();
            TblRow["WorseSitting"] = chkWorseSitting.Checked;
            TblRow["WorseStanding"] = chkWorseStanding.Checked;
            TblRow["WorseLyingDown"] = chkWorseLyingDown.Checked;
            TblRow["WorseMovement"] = chkWorseMovement.Checked;
            TblRow["WorseSeatingtoStandingUp"] = chkWorseSeatingtoStandingUp.Checked;
            TblRow["WorseWalking"] = chkWorseWalking.Checked;
            TblRow["WorseClimbingStairs"] = chkWorseClimbingStairs.Checked;
            TblRow["WorseDescendingStairs"] = chkWorseDescendingStairs.Checked;
            TblRow["WorseDriving"] = chkWorseDriving.Checked;
            TblRow["WorseWorking"] = chkWorseWorking.Checked;
            TblRow["WorseBending"] = chkWorseBending.Checked;
            TblRow["WorseLifting"] = chkWorseLifting.Checked;
            TblRow["WorseTwisting"] = chkWorseTwisting.Checked;
            TblRow["ImprovedResting"] = chkImprovedResting.Checked;
            TblRow["ImprovedMedication"] = chkImprovedMedication.Checked;
            TblRow["ImprovedTherapy"] = chkImprovedTherapy.Checked;
            TblRow["ImprovedSleeping"] = chkImprovedSleeping.Checked;
            TblRow["ImprovedMovement"] = chkImprovedMovement.Checked;
            TblRow["FwdFlexRight"] = txtFwdFlexRight.Text.ToString();
            TblRow["FwdFlexLeft"] = txtFwdFlexNormal.Text.ToString();
            TblRow["ExtensionRight"] = txtExtensionRight.Text.ToString();
            TblRow["ExtensionLeft"] = txtExtensionNormal.Text.ToString();
            TblRow["RotationRight"] = txtRotationRight.Text.ToString();
            TblRow["RotationLeft"] = txtRotationLeft.Text.ToString();
            TblRow["RotationNormal"] = txtRotationNormal.Text.ToString();
            TblRow["LateralFlexRight"] = txtLateralFlexRight.Text.ToString();
            TblRow["LateralFlexLeft"] = txtLateralFlexLeft.Text.ToString();
            TblRow["LateralFlexNormal"] = txtLateralFlexNormal.Text.ToString();
            TblRow["PalpationAt"] = txtPalpationAt.Text.ToString();
            TblRow["Levels"] = ddlLevels.Text;
            //TblRow["Bilaterally"] = chkBilaterally.Checked;
            //TblRow["CSRight"] = chkCSRight.Checked;
            //TblRow["CSLeft"] = chkCSLeft.Checked;
            //TblRow["RightGreaterLeft"] = chkRightGreaterLeft.Checked;
            //TblRow["LeftGreaterRight"] = chkLeftGreaterRight.Checked;
            TblRow["Spurlings"] = cboSpurlings.Text.ToString();
            TblRow["Distraction"] = cboDistraction.Text.ToString();
            TblRow["TPSide1"] = cboTPSide1.Text.ToString();
            TblRow["TPText1"] = txtTPText1.Text.ToString();
            TblRow["TPSide2"] = cboTPSide2.Text.ToString();
            TblRow["TPText2"] = txtTPText2.Text.ToString();
            TblRow["TPSide3"] = cboTPSide3.Text.ToString();
            TblRow["TPText3"] = txtTPText3.Text.ToString();
            TblRow["TPside4"] = cboTPSide4.Text.ToString();
            TblRow["TPText4"] = txtTPText4.Text.ToString();
            TblRow["TPSide5"] = cboTPSide5.Text.ToString();
            TblRow["TPText5"] = txtTPText5.Text.ToString();
            TblRow["TPSide6"] = cboTPSide6.Text.ToString();
            TblRow["TPText6"] = txtTPText6.Text.ToString();
            TblRow["TPSide7"] = cboTPSide7.Text.ToString();
            TblRow["TPText7"] = txtTPTex7t.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["Xrays"] = false;//TBD
            TblRow["TPEpidular"] = false;//TBD
            TblRow["NoOfTPInjections"] = "";// txtPainScale;
            TblRow["ScheduleEpidural"] = false;//txtPainScale;
            TblRow["NewMBB"] = false;// txtPainScale;
            TblRow["SPTPMBB"] = false;//txtPainScale;

            TblRow["WorseOther"] = txtWorseOtherText.Text.ToString();

            //string strname = "", strrom = "", strnormal = "";

            //for (int i = 0; i < repROM.Items.Count; i++)
            //{
            //    Label lblname = repROM.Items[i].FindControl("lblname") as Label;
            //    TextBox txtrom = repROM.Items[i].FindControl("txtrom") as TextBox;
            //    TextBox txtnormal = repROM.Items[i].FindControl("txtnormal") as TextBox;

            //    strname = strname + "," + lblname.Text;
            //    strrom = strrom + "," + txtrom.Text;
            //    strnormal = strnormal + "," + txtnormal.Text;
            //}


            //TblRow["ROM"] = strrom.Substring(1);
            //TblRow["NormalROM"] = strnormal.Substring(1);
            //TblRow["NameROM"] = strname.Substring(1);

            if (_fuMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_fuMode == "Delete")
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

        if (_fuMode == "New")
            return "Neck has been added...";
        else if (_fuMode == "Update")
            return "Neck has been updated...";
        else if (_fuMode == "Delete")
            return "Neck has been deleted...";
        else
            return "";
    }

    public void PopulateStrightFwd()
    {
        //tbRomIs.Text = "ROM";
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtensionWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFwdFlexWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal1.Visibility = System.Windows.Visibility.Visible;
        //tbNormal2.Visibility = System.Windows.Visibility.Visible;
        //txtExtensionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFwdFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txtLateralFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txttxtRotationNormal.Visibility = System.Windows.Visibility.Visible;
    }

    public void PopulateUI(string fuid)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + fuid;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtPainScale.Text = TblRow["PainScale"].ToString().Trim();
            chkSharp.Checked = CommonConvert.ToBoolean(TblRow["Sharp"].ToString());
            chkElectric.Checked = CommonConvert.ToBoolean(TblRow["Electric"].ToString());
            chkShooting.Checked = CommonConvert.ToBoolean(TblRow["Shooting"].ToString());
            chkThrobbling.Checked = CommonConvert.ToBoolean(TblRow["Throbbling"].ToString());
            chkPulsating.Checked = CommonConvert.ToBoolean(TblRow["Pulsating"].ToString());
            chkDull.Checked = CommonConvert.ToBoolean(TblRow["Dull"].ToString());
            chkAchy.Checked = CommonConvert.ToBoolean(TblRow["Achy"].ToString());
            if (!string.IsNullOrEmpty(TblRow["constant"].ToString()))
                chkContent.Checked = CommonConvert.ToBoolean(TblRow["constant"]);
            if (!string.IsNullOrEmpty(TblRow["intermittent"].ToString()))
                chkIntermittent.Checked = CommonConvert.ToBoolean(TblRow["intermittent"]);
            txtRadiates.Text = TblRow["Radiates"].ToString().Trim();
            chkNumbness.Checked = CommonConvert.ToBoolean(TblRow["Numbness"].ToString());
            chkTingling.Checked = CommonConvert.ToBoolean(TblRow["Tingling"].ToString());
            chkBurning.Checked = CommonConvert.ToBoolean(TblRow["Burning"].ToString());
            txtBurningTo.Text = TblRow["BurningTo"].ToString().Trim();
            chkShoulderLeft1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderLeft1"].ToString());
            chkShoulderRight1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderRight1"].ToString());
            chkShoulderBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderBilateral1"].ToString());
            chkScapulaLeft1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaLeft1"].ToString());
            chkScapulaRight1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaRight1"].ToString());
            chkScapulaBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaBilateral1"].ToString());
            chkArmLeft1.Checked = CommonConvert.ToBoolean(TblRow["ArmLeft1"].ToString());
            chkArmRight1.Checked = CommonConvert.ToBoolean(TblRow["ArmRight1"].ToString());
            chkArmBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ArmBilateral1"].ToString());
            chkForearmLeft1.Checked = CommonConvert.ToBoolean(TblRow["ForearmLeft1"].ToString());
            chkForearmRight1.Checked = CommonConvert.ToBoolean(TblRow["ForearmRight1"].ToString());
            chkForearmBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ForearmBilateral1"].ToString());
            chkHandLeft1.Checked = CommonConvert.ToBoolean(TblRow["HandLeft1"].ToString());
            chkHandRight1.Checked = CommonConvert.ToBoolean(TblRow["HandRight1"].ToString());
            chkHandBilateral1.Checked = CommonConvert.ToBoolean(TblRow["HandBilateral1"].ToString());
            chkWristLeft1.Checked = CommonConvert.ToBoolean(TblRow["WristLeft1"].ToString());
            chkWristRight1.Checked = CommonConvert.ToBoolean(TblRow["WristRight1"].ToString());
            chkWristBilateral1.Checked = CommonConvert.ToBoolean(TblRow["WristBilateral1"].ToString());
            chk1stDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitLeft1"].ToString());
            chk1stDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitRight1"].ToString());
            chk1stDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitBilateral1"].ToString());
            chk2ndDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitLeft1"].ToString());
            chk2ndDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitRight1"].ToString());
            chk2ndDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitBilateral1"].ToString());
            chk3rdDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitLeft1"].ToString());
            chk3rdDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitRight1"].ToString());
            chk3rdDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitBilateral1"].ToString());
            chk4thDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitLeft1"].ToString());
            chk4thDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitRight1"].ToString());
            chk4thDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitBilateral1"].ToString());
            chk5thDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitLeft1"].ToString());
            chk5thDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitRight1"].ToString());
            chk5thDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitBilateral1"].ToString());
            chkShoulderLeft2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderLeft2"].ToString());
            chkShoulderRight2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderRight2"].ToString());
            chkShoulderBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderBilateral2"].ToString());
            chkScapulaLeft2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaLeft2"].ToString());
            chkScapulaRight2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaRight2"].ToString());
            chkScapulaBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaBilateral2"].ToString());
            chkArmLeft2.Checked = CommonConvert.ToBoolean(TblRow["ArmLeft2"].ToString());
            chkArmRight2.Checked = CommonConvert.ToBoolean(TblRow["ArmRight2"].ToString());
            chkArmBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ArmBilateral2"].ToString());
            chkForearmLeft2.Checked = CommonConvert.ToBoolean(TblRow["ForearmLeft2"].ToString());
            chkForearmRight2.Checked = CommonConvert.ToBoolean(TblRow["ForearmRight2"].ToString());
            chkForearmBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ForearmBilateral2"].ToString());
            chkHandLeft2.Checked = CommonConvert.ToBoolean(TblRow["HandLeft2"].ToString());
            chkHandRight2.Checked = CommonConvert.ToBoolean(TblRow["HandRight2"].ToString());
            chkHandBilateral2.Checked = CommonConvert.ToBoolean(TblRow["HandBilateral2"].ToString());
            chkWristLeft2.Checked = CommonConvert.ToBoolean(TblRow["WristLeft2"].ToString());
            chkWristRight2.Checked = CommonConvert.ToBoolean(TblRow["WristRight2"].ToString());
            chkWristBilateral2.Checked = CommonConvert.ToBoolean(TblRow["WristBilateral2"].ToString());
            chk1stDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitLeft2"].ToString());
            chk1stDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitRight2"].ToString());
            chk1stDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitBilateral2"].ToString());
            chk2ndDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitLeft2"].ToString());
            chk2ndDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitRight2"].ToString());
            chk2ndDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitBilateral2"].ToString());
            chk3rdDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitLeft2"].ToString());
            chk3rdDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitRight2"].ToString());
            chk3rdDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitBilateral2"].ToString());
            chk4thDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitLeft2"].ToString());
            chk4thDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitRight2"].ToString());
            chk4thDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitBilateral2"].ToString());
            chk5thDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitLeft2"].ToString());
            chk5thDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitRight2"].ToString());
            chk5thDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitBilateral2"].ToString());
            txtWeeknessIn.Text = TblRow["WeeknessIn"].ToString().Trim();
            chkWorseSitting.Checked = CommonConvert.ToBoolean(TblRow["WorseSitting"].ToString());
            chkWorseStanding.Checked = CommonConvert.ToBoolean(TblRow["WorseStanding"].ToString());
            chkWorseLyingDown.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingDown"].ToString());
            chkWorseMovement.Checked = CommonConvert.ToBoolean(TblRow["WorseMovement"].ToString());
            chkWorseSeatingtoStandingUp.Checked = CommonConvert.ToBoolean(TblRow["WorseSeatingtoStandingUp"].ToString());
            chkWorseWalking.Checked = CommonConvert.ToBoolean(TblRow["WorseWalking"].ToString());
            chkWorseClimbingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseClimbingStairs"].ToString());
            chkWorseDescendingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseDescendingStairs"].ToString());
            chkWorseDriving.Checked = CommonConvert.ToBoolean(TblRow["WorseDriving"].ToString());
            chkWorseWorking.Checked = CommonConvert.ToBoolean(TblRow["WorseWorking"].ToString());
            chkWorseBending.Checked = CommonConvert.ToBoolean(TblRow["WorseBending"].ToString());
            chkWorseLifting.Checked = CommonConvert.ToBoolean(TblRow["WorseLifting"].ToString());
            chkWorseTwisting.Checked = CommonConvert.ToBoolean(TblRow["WorseTwisting"].ToString());
            chkImprovedResting.Checked = CommonConvert.ToBoolean(TblRow["ImprovedResting"].ToString());
            chkImprovedMedication.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedication"].ToString());
            chkImprovedTherapy.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapy"].ToString());
            chkImprovedSleeping.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleeping"].ToString());
            chkImprovedMovement.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMovement"].ToString());
            txtFwdFlexRight.Text = TblRow["FwdFlexRight"].ToString().Trim();
            txtFwdFlexNormal.Text = TblRow["FwdFlexLeft"].ToString().Trim();
            txtExtensionRight.Text = TblRow["ExtensionRight"].ToString().Trim();
            txtExtensionNormal.Text = TblRow["ExtensionLeft"].ToString().Trim();
            txtRotationRight.Text = TblRow["RotationRight"].ToString().Trim();
            txtRotationLeft.Text = TblRow["RotationLeft"].ToString().Trim();
            txtRotationNormal.Text = TblRow["RotationNormal"].ToString().Trim();
            txtLateralFlexRight.Text = TblRow["LateralFlexRight"].ToString().Trim();
            txtLateralFlexLeft.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtLateralFlexNormal.Text = TblRow["LateralFlexNormal"].ToString().Trim();
            txtPalpationAt.Text = TblRow["PalpationAt"].ToString().Trim();
            ddlLevels.Text = TblRow["Levels"].ToString().Trim();
            cboSpurlings.Text = TblRow["Spurlings"].ToString().Trim();
            cboDistraction.Text = TblRow["Distraction"].ToString().Trim();
            cboTPSide1.Text = TblRow["TPSide1"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText1"].ToString().Trim();
            cboTPSide2.Text = TblRow["TPSide2"].ToString().Trim();
            txtTPText2.Text = TblRow["TPText2"].ToString().Trim();
            cboTPSide3.Text = TblRow["TPSide3"].ToString().Trim();
            txtTPText3.Text = TblRow["TPText3"].ToString().Trim();
            cboTPSide4.Text = TblRow["TPside4"].ToString().Trim();
            txtTPText4.Text = TblRow["TPText4"].ToString().Trim();
            cboTPSide5.Text = TblRow["TPSide5"].ToString().Trim();
            txtTPText5.Text = TblRow["TPText5"].ToString().Trim();
            cboTPSide6.Text = TblRow["TPSide6"].ToString().Trim();
            txtTPText6.Text = TblRow["TPText6"].ToString().Trim();
            cboTPSide7.Text = TblRow["TPSide7"].ToString().Trim();
            txtTPTex7t.Text = TblRow["TPText7"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            txtWorseOtherText.Text = TblRow["WorseOther"].ToString().Trim();
            _fldPop = false;
        }
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }

    public void PopulateIEUI(string ieid)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpNeck WHERE PatientIE_ID = " + ieid;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtPainScale.Text = TblRow["PainScale"].ToString().Trim();
            chkSharp.Checked = CommonConvert.ToBoolean(TblRow["Sharp"].ToString());
            chkElectric.Checked = CommonConvert.ToBoolean(TblRow["Electric"].ToString());
            chkShooting.Checked = CommonConvert.ToBoolean(TblRow["Shooting"].ToString());
            chkThrobbling.Checked = CommonConvert.ToBoolean(TblRow["Throbbling"].ToString());
            chkPulsating.Checked = CommonConvert.ToBoolean(TblRow["Pulsating"].ToString());
            chkDull.Checked = CommonConvert.ToBoolean(TblRow["Dull"].ToString());
            chkAchy.Checked = CommonConvert.ToBoolean(TblRow["Achy"].ToString());
            if (!string.IsNullOrEmpty(TblRow["constant"].ToString()))
                chkContent.Checked = CommonConvert.ToBoolean(TblRow["constant"]);
            if (!string.IsNullOrEmpty(TblRow["intermittent"].ToString()))
                chkIntermittent.Checked = CommonConvert.ToBoolean(TblRow["intermittent"]);
            txtRadiates.Text = TblRow["Radiates"].ToString().Trim();
            chkNumbness.Checked = CommonConvert.ToBoolean(TblRow["Numbness"].ToString());
            chkTingling.Checked = CommonConvert.ToBoolean(TblRow["Tingling"].ToString());
            chkBurning.Checked = CommonConvert.ToBoolean(TblRow["Burning"].ToString());
            txtBurningTo.Text = TblRow["BurningTo"].ToString().Trim();
            chkShoulderLeft1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderLeft1"].ToString());
            chkShoulderRight1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderRight1"].ToString());
            chkShoulderBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ShoulderBilateral1"].ToString());
            chkScapulaLeft1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaLeft1"].ToString());
            chkScapulaRight1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaRight1"].ToString());
            chkScapulaBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ScapulaBilateral1"].ToString());
            chkArmLeft1.Checked = CommonConvert.ToBoolean(TblRow["ArmLeft1"].ToString());
            chkArmRight1.Checked = CommonConvert.ToBoolean(TblRow["ArmRight1"].ToString());
            chkArmBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ArmBilateral1"].ToString());
            chkForearmLeft1.Checked = CommonConvert.ToBoolean(TblRow["ForearmLeft1"].ToString());
            chkForearmRight1.Checked = CommonConvert.ToBoolean(TblRow["ForearmRight1"].ToString());
            chkForearmBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ForearmBilateral1"].ToString());
            chkHandLeft1.Checked = CommonConvert.ToBoolean(TblRow["HandLeft1"].ToString());
            chkHandRight1.Checked = CommonConvert.ToBoolean(TblRow["HandRight1"].ToString());
            chkHandBilateral1.Checked = CommonConvert.ToBoolean(TblRow["HandBilateral1"].ToString());
            chkWristLeft1.Checked = CommonConvert.ToBoolean(TblRow["WristLeft1"].ToString());
            chkWristRight1.Checked = CommonConvert.ToBoolean(TblRow["WristRight1"].ToString());
            chkWristBilateral1.Checked = CommonConvert.ToBoolean(TblRow["WristBilateral1"].ToString());
            chk1stDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitLeft1"].ToString());
            chk1stDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitRight1"].ToString());
            chk1stDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitBilateral1"].ToString());
            chk2ndDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitLeft1"].ToString());
            chk2ndDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitRight1"].ToString());
            chk2ndDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitBilateral1"].ToString());
            chk3rdDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitLeft1"].ToString());
            chk3rdDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitRight1"].ToString());
            chk3rdDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitBilateral1"].ToString());
            chk4thDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitLeft1"].ToString());
            chk4thDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitRight1"].ToString());
            chk4thDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitBilateral1"].ToString());
            chk5thDigitLeft1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitLeft1"].ToString());
            chk5thDigitRight1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitRight1"].ToString());
            chk5thDigitBilateral1.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitBilateral1"].ToString());
            chkShoulderLeft2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderLeft2"].ToString());
            chkShoulderRight2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderRight2"].ToString());
            chkShoulderBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ShoulderBilateral2"].ToString());
            chkScapulaLeft2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaLeft2"].ToString());
            chkScapulaRight2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaRight2"].ToString());
            chkScapulaBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ScapulaBilateral2"].ToString());
            chkArmLeft2.Checked = CommonConvert.ToBoolean(TblRow["ArmLeft2"].ToString());
            chkArmRight2.Checked = CommonConvert.ToBoolean(TblRow["ArmRight2"].ToString());
            chkArmBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ArmBilateral2"].ToString());
            chkForearmLeft2.Checked = CommonConvert.ToBoolean(TblRow["ForearmLeft2"].ToString());
            chkForearmRight2.Checked = CommonConvert.ToBoolean(TblRow["ForearmRight2"].ToString());
            chkForearmBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ForearmBilateral2"].ToString());
            chkHandLeft2.Checked = CommonConvert.ToBoolean(TblRow["HandLeft2"].ToString());
            chkHandRight2.Checked = CommonConvert.ToBoolean(TblRow["HandRight2"].ToString());
            chkHandBilateral2.Checked = CommonConvert.ToBoolean(TblRow["HandBilateral2"].ToString());
            chkWristLeft2.Checked = CommonConvert.ToBoolean(TblRow["WristLeft2"].ToString());
            chkWristRight2.Checked = CommonConvert.ToBoolean(TblRow["WristRight2"].ToString());
            chkWristBilateral2.Checked = CommonConvert.ToBoolean(TblRow["WristBilateral2"].ToString());
            chk1stDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitLeft2"].ToString());
            chk1stDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitRight2"].ToString());
            chk1stDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C1stDigitBilateral2"].ToString());
            chk2ndDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitLeft2"].ToString());
            chk2ndDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitRight2"].ToString());
            chk2ndDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C2ndDigitBilateral2"].ToString());
            chk3rdDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitLeft2"].ToString());
            chk3rdDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitRight2"].ToString());
            chk3rdDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C3rdDigitBilateral2"].ToString());
            chk4thDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitLeft2"].ToString());
            chk4thDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitRight2"].ToString());
            chk4thDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C4thDigitBilateral2"].ToString());
            chk5thDigitLeft2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitLeft2"].ToString());
            chk5thDigitRight2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitRight2"].ToString());
            chk5thDigitBilateral2.Checked = CommonConvert.ToBoolean(TblRow["C5thDigitBilateral2"].ToString());
            txtWeeknessIn.Text = TblRow["WeeknessIn"].ToString().Trim();
            chkWorseSitting.Checked = CommonConvert.ToBoolean(TblRow["WorseSitting"].ToString());
            chkWorseStanding.Checked = CommonConvert.ToBoolean(TblRow["WorseStanding"].ToString());
            chkWorseLyingDown.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingDown"].ToString());
            chkWorseMovement.Checked = CommonConvert.ToBoolean(TblRow["WorseMovement"].ToString());
            chkWorseSeatingtoStandingUp.Checked = CommonConvert.ToBoolean(TblRow["WorseSeatingtoStandingUp"].ToString());
            chkWorseWalking.Checked = CommonConvert.ToBoolean(TblRow["WorseWalking"].ToString());
            chkWorseClimbingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseClimbingStairs"].ToString());
            chkWorseDescendingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseDescendingStairs"].ToString());
            chkWorseDriving.Checked = CommonConvert.ToBoolean(TblRow["WorseDriving"].ToString());
            chkWorseWorking.Checked = CommonConvert.ToBoolean(TblRow["WorseWorking"].ToString());
            chkWorseBending.Checked = CommonConvert.ToBoolean(TblRow["WorseBending"].ToString());
            chkWorseLifting.Checked = CommonConvert.ToBoolean(TblRow["WorseLifting"].ToString());
            chkWorseTwisting.Checked = CommonConvert.ToBoolean(TblRow["WorseTwisting"].ToString());
            chkImprovedResting.Checked = CommonConvert.ToBoolean(TblRow["ImprovedResting"].ToString());
            chkImprovedMedication.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedication"].ToString());
            chkImprovedTherapy.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapy"].ToString());
            chkImprovedSleeping.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleeping"].ToString());
            chkImprovedMovement.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMovement"].ToString());
            txtFwdFlexRight.Text = TblRow["FwdFlexRight"].ToString().Trim();
            txtFwdFlexNormal.Text = TblRow["FwdFlexLeft"].ToString().Trim();
            txtExtensionRight.Text = TblRow["ExtensionRight"].ToString().Trim();
            txtExtensionNormal.Text = TblRow["ExtensionLeft"].ToString().Trim();
            txtRotationRight.Text = TblRow["RotationRight"].ToString().Trim();
            txtRotationLeft.Text = TblRow["RotationLeft"].ToString().Trim();
            txtRotationNormal.Text = TblRow["RotationNormal"].ToString().Trim();
            txtLateralFlexRight.Text = TblRow["LateralFlexRight"].ToString().Trim();
            txtLateralFlexLeft.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtLateralFlexNormal.Text = TblRow["LateralFlexNormal"].ToString().Trim();
            txtPalpationAt.Text = TblRow["PalpationAt"].ToString().Trim();
            ddlLevels.Text = TblRow["Levels"].ToString().Trim();
            cboSpurlings.Text = TblRow["Spurlings"].ToString().Trim();
            cboDistraction.Text = TblRow["Distraction"].ToString().Trim();
            cboTPSide1.Text = TblRow["TPSide1"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText1"].ToString().Trim();
            cboTPSide2.Text = TblRow["TPSide2"].ToString().Trim();
            txtTPText2.Text = TblRow["TPText2"].ToString().Trim();
            cboTPSide3.Text = TblRow["TPSide3"].ToString().Trim();
            txtTPText3.Text = TblRow["TPText3"].ToString().Trim();
            cboTPSide4.Text = TblRow["TPside4"].ToString().Trim();
            txtTPText4.Text = TblRow["TPText4"].ToString().Trim();
            cboTPSide5.Text = TblRow["TPSide5"].ToString().Trim();
            txtTPText5.Text = TblRow["TPText5"].ToString().Trim();
            cboTPSide6.Text = TblRow["TPSide6"].ToString().Trim();
            txtTPText6.Text = TblRow["TPText6"].ToString().Trim();
            cboTPSide7.Text = TblRow["TPSide7"].ToString().Trim();
            txtTPTex7t.Text = TblRow["TPText7"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            txtWorseOtherText.Text = TblRow["WorseOther"].ToString().Trim();
            _fldPop = false;
        }
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }

    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }

        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            if (txtPainScale.Text == "") txtPainScale.Text = node.SelectSingleNode("PainScale") == null ? txtPainScale.Text.ToString().Trim() : node.SelectSingleNode("PainScale").InnerText;
            chkSharp.Checked = node.SelectSingleNode("Sharp") == null ? chkSharp.Checked : Convert.ToBoolean(node.SelectSingleNode("Sharp").InnerText);
            chkElectric.Checked = node.SelectSingleNode("Electric") == null ? chkElectric.Checked : Convert.ToBoolean(node.SelectSingleNode("Electric").InnerText);
            chkShooting.Checked = node.SelectSingleNode("Shooting") == null ? chkShooting.Checked : Convert.ToBoolean(node.SelectSingleNode("Shooting").InnerText);
            chkThrobbling.Checked = node.SelectSingleNode("Throbbling") == null ? chkThrobbling.Checked : Convert.ToBoolean(node.SelectSingleNode("Throbbling").InnerText);
            chkPulsating.Checked = node.SelectSingleNode("Pulsating") == null ? chkPulsating.Checked : Convert.ToBoolean(node.SelectSingleNode("Pulsating").InnerText);
            chkDull.Checked = node.SelectSingleNode("Dull") == null ? chkDull.Checked : Convert.ToBoolean(node.SelectSingleNode("Dull").InnerText);
            chkAchy.Checked = node.SelectSingleNode("Achy") == null ? chkAchy.Checked : Convert.ToBoolean(node.SelectSingleNode("Achy").InnerText);
            if (txtRadiates.Text == "") txtRadiates.Text = node.SelectSingleNode("Radiates") == null ? txtRadiates.Text.ToString().Trim() : node.SelectSingleNode("Radiates").InnerText;
            chkNumbness.Checked = node.SelectSingleNode("Numbness") == null ? chkNumbness.Checked : Convert.ToBoolean(node.SelectSingleNode("Numbness").InnerText);
            chkTingling.Checked = node.SelectSingleNode("Tingling") == null ? chkTingling.Checked : Convert.ToBoolean(node.SelectSingleNode("Tingling").InnerText);
            chkBurning.Checked = node.SelectSingleNode("Burning") == null ? chkBurning.Checked : Convert.ToBoolean(node.SelectSingleNode("Burning").InnerText);
            if (txtBurningTo.Text == "") txtBurningTo.Text = node.SelectSingleNode("BurningTo") == null ? txtBurningTo.Text.ToString().Trim() : node.SelectSingleNode("BurningTo").InnerText;
            chkShoulderLeft1.Checked = node.SelectSingleNode("ShoulderLeft1") == null ? chkShoulderLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderLeft1").InnerText);
            chkShoulderRight1.Checked = node.SelectSingleNode("ShoulderRight1") == null ? chkShoulderRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderRight1").InnerText);
            chkShoulderBilateral1.Checked = node.SelectSingleNode("ShoulderBilateral1") == null ? chkShoulderBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderBilateral1").InnerText);
            chkScapulaLeft1.Checked = node.SelectSingleNode("ScapulaLeft1") == null ? chkScapulaLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaLeft1").InnerText);
            chkScapulaRight1.Checked = node.SelectSingleNode("ScapulaRight1") == null ? chkScapulaRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaRight1").InnerText);
            chkScapulaBilateral1.Checked = node.SelectSingleNode("ScapulaBilateral1") == null ? chkScapulaBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaBilateral1").InnerText);
            chkArmLeft1.Checked = node.SelectSingleNode("ArmLeft1") == null ? chkArmLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmLeft1").InnerText);
            chkArmRight1.Checked = node.SelectSingleNode("ArmRight1") == null ? chkArmRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmRight1").InnerText);
            chkArmBilateral1.Checked = node.SelectSingleNode("ArmBilateral1") == null ? chkArmBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmBilateral1").InnerText);
            chkForearmLeft1.Checked = node.SelectSingleNode("ForearmLeft1") == null ? chkForearmLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmLeft1").InnerText);
            chkForearmRight1.Checked = node.SelectSingleNode("ForearmRight1") == null ? chkForearmRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmRight1").InnerText);
            chkForearmBilateral1.Checked = node.SelectSingleNode("ForearmBilateral1") == null ? chkForearmBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmBilateral1").InnerText);
            chkHandLeft1.Checked = node.SelectSingleNode("HandLeft1") == null ? chkHandLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("HandLeft1").InnerText);
            chkHandRight1.Checked = node.SelectSingleNode("HandRight1") == null ? chkHandRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("HandRight1").InnerText);
            chkHandBilateral1.Checked = node.SelectSingleNode("HandBilateral1") == null ? chkHandBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("HandBilateral1").InnerText);
            chkWristLeft1.Checked = node.SelectSingleNode("WristLeft1") == null ? chkWristLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("WristLeft1").InnerText);
            chkWristRight1.Checked = node.SelectSingleNode("WristRight1") == null ? chkWristRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("WristRight1").InnerText);
            chkWristBilateral1.Checked = node.SelectSingleNode("WristBilateral1") == null ? chkWristBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("WristBilateral1").InnerText);
            chk1stDigitLeft1.Checked = node.SelectSingleNode("C1stDigitLeft1") == null ? chk1stDigitLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitLeft1").InnerText);
            chk1stDigitRight1.Checked = node.SelectSingleNode("C1stDigitRight1") == null ? chk1stDigitRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitRight1").InnerText);
            chk1stDigitBilateral1.Checked = node.SelectSingleNode("C1stDigitBilateral1") == null ? chk1stDigitBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitBilateral1").InnerText);
            chk2ndDigitLeft1.Checked = node.SelectSingleNode("C2ndDigitLeft1") == null ? chk2ndDigitLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitLeft1").InnerText);
            chk2ndDigitRight1.Checked = node.SelectSingleNode("C2ndDigitRight1") == null ? chk2ndDigitRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitRight1").InnerText);
            chk2ndDigitBilateral1.Checked = node.SelectSingleNode("C2ndDigitBilateral1") == null ? chk2ndDigitBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitBilateral1").InnerText);
            chk3rdDigitLeft1.Checked = node.SelectSingleNode("C3rdDigitLeft1") == null ? chk3rdDigitLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitLeft1").InnerText);
            chk3rdDigitRight1.Checked = node.SelectSingleNode("C3rdDigitRight1") == null ? chk3rdDigitRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitRight1").InnerText);
            chk3rdDigitBilateral1.Checked = node.SelectSingleNode("C3rdDigitBilateral1") == null ? chk3rdDigitBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitBilateral1").InnerText);
            chk4thDigitLeft1.Checked = node.SelectSingleNode("C4thDigitLeft1") == null ? chk4thDigitLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitLeft1").InnerText);
            chk4thDigitRight1.Checked = node.SelectSingleNode("C4thDigitRight1") == null ? chk4thDigitRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitRight1").InnerText);
            chk4thDigitBilateral1.Checked = node.SelectSingleNode("C4thDigitBilateral1") == null ? chk4thDigitBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitBilateral1").InnerText);
            chk5thDigitLeft1.Checked = node.SelectSingleNode("C5thDigitLeft1") == null ? chk5thDigitLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitLeft1").InnerText);
            chk5thDigitRight1.Checked = node.SelectSingleNode("C5thDigitRight1") == null ? chk5thDigitRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitRight1").InnerText);
            chk5thDigitBilateral1.Checked = node.SelectSingleNode("C5thDigitBilateral1") == null ? chk5thDigitBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitBilateral1").InnerText);
            chkShoulderLeft2.Checked = node.SelectSingleNode("ShoulderLeft2") == null ? chkShoulderLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderLeft2").InnerText);
            chkShoulderRight2.Checked = node.SelectSingleNode("ShoulderRight2") == null ? chkShoulderRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderRight2").InnerText);
            chkShoulderBilateral2.Checked = node.SelectSingleNode("ShoulderBilateral2") == null ? chkShoulderBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ShoulderBilateral2").InnerText);
            chkScapulaLeft2.Checked = node.SelectSingleNode("ScapulaLeft2") == null ? chkScapulaLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaLeft2").InnerText);
            chkScapulaRight2.Checked = node.SelectSingleNode("ScapulaRight2") == null ? chkScapulaRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaRight2").InnerText);
            chkScapulaBilateral2.Checked = node.SelectSingleNode("ScapulaBilateral2") == null ? chkScapulaBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapulaBilateral2").InnerText);
            chkArmLeft2.Checked = node.SelectSingleNode("ArmLeft2") == null ? chkArmLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmLeft2").InnerText);
            chkArmRight2.Checked = node.SelectSingleNode("ArmRight2") == null ? chkArmRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmRight2").InnerText);
            chkArmBilateral2.Checked = node.SelectSingleNode("ArmBilateral2") == null ? chkArmBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ArmBilateral2").InnerText);
            chkForearmLeft2.Checked = node.SelectSingleNode("ForearmLeft2") == null ? chkForearmLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmLeft2").InnerText);
            chkForearmRight2.Checked = node.SelectSingleNode("ForearmRight2") == null ? chkForearmRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmRight2").InnerText);
            chkForearmBilateral2.Checked = node.SelectSingleNode("ForearmBilateral2") == null ? chkForearmBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ForearmBilateral2").InnerText);
            chkHandLeft2.Checked = node.SelectSingleNode("HandLeft2") == null ? chkHandLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("HandLeft2").InnerText);
            chkHandRight2.Checked = node.SelectSingleNode("HandRight2") == null ? chkHandRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("HandRight2").InnerText);
            chkHandBilateral2.Checked = node.SelectSingleNode("HandBilateral2") == null ? chkHandBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("HandBilateral2").InnerText);
            chkWristLeft2.Checked = node.SelectSingleNode("WristLeft2") == null ? chkWristLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("WristLeft2").InnerText);
            chkWristRight2.Checked = node.SelectSingleNode("WristRight2") == null ? chkWristRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("WristRight2").InnerText);
            chkWristBilateral2.Checked = node.SelectSingleNode("WristBilateral2") == null ? chkWristBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("WristBilateral2").InnerText);
            chk1stDigitLeft2.Checked = node.SelectSingleNode("C1stDigitLeft2") == null ? chk1stDigitLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitLeft2").InnerText);
            chk1stDigitRight2.Checked = node.SelectSingleNode("C1stDigitRight2") == null ? chk1stDigitRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitRight2").InnerText);
            chk1stDigitBilateral2.Checked = node.SelectSingleNode("C1stDigitBilateral2") == null ? chk1stDigitBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("C1stDigitBilateral2").InnerText);
            chk2ndDigitLeft2.Checked = node.SelectSingleNode("C2ndDigitLeft2") == null ? chk2ndDigitLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitLeft2").InnerText);
            chk2ndDigitRight2.Checked = node.SelectSingleNode("C2ndDigitRight2") == null ? chk2ndDigitRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitRight2").InnerText);
            chk2ndDigitBilateral2.Checked = node.SelectSingleNode("C2ndDigitBilateral2") == null ? chk2ndDigitBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("C2ndDigitBilateral2").InnerText);
            chk3rdDigitLeft2.Checked = node.SelectSingleNode("C3rdDigitLeft2") == null ? chk3rdDigitLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitLeft2").InnerText);
            chk3rdDigitRight2.Checked = node.SelectSingleNode("C3rdDigitRight2") == null ? chk3rdDigitRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitRight2").InnerText);
            chk3rdDigitBilateral2.Checked = node.SelectSingleNode("C3rdDigitBilateral2") == null ? chk3rdDigitBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("C3rdDigitBilateral2").InnerText);
            chk4thDigitLeft2.Checked = node.SelectSingleNode("C4thDigitLeft2") == null ? chk4thDigitLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitLeft2").InnerText);
            chk4thDigitRight2.Checked = node.SelectSingleNode("C4thDigitRight2") == null ? chk4thDigitRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitRight2").InnerText);
            chk4thDigitBilateral2.Checked = node.SelectSingleNode("C4thDigitBilateral2") == null ? chk4thDigitBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("C4thDigitBilateral2").InnerText);
            chk5thDigitLeft2.Checked = node.SelectSingleNode("C5thDigitLeft2") == null ? chk5thDigitLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitLeft2").InnerText);
            chk5thDigitRight2.Checked = node.SelectSingleNode("C5thDigitRight2") == null ? chk5thDigitRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitRight2").InnerText);
            chk5thDigitBilateral2.Checked = node.SelectSingleNode("C5thDigitBilateral2") == null ? chk5thDigitBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("C5thDigitBilateral2").InnerText);
            if (txtWeeknessIn.Text == "") txtWeeknessIn.Text = node.SelectSingleNode("WeeknessIn") == null ? txtWeeknessIn.Text.ToString().Trim() : node.SelectSingleNode("WeeknessIn").InnerText;
            chkWorseSitting.Checked = node.SelectSingleNode("WorseSitting") == null ? chkWorseSitting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSitting").InnerText);
            chkWorseStanding.Checked = node.SelectSingleNode("WorseStanding") == null ? chkWorseStanding.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseStanding").InnerText);
            chkWorseLyingDown.Checked = node.SelectSingleNode("WorseLyingDown") == null ? chkWorseLyingDown.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingDown").InnerText);
            chkWorseMovement.Checked = node.SelectSingleNode("WorseMovement") == null ? chkWorseMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovement").InnerText);
            chkWorseSeatingtoStandingUp.Checked = node.SelectSingleNode("WorseSeatingtoStandingUp") == null ? chkWorseSeatingtoStandingUp.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);
            chkWorseWalking.Checked = node.SelectSingleNode("WorseWalking") == null ? chkWorseWalking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWalking").InnerText);
            chkWorseClimbingStairs.Checked = node.SelectSingleNode("WorseClimbingStairs") == null ? chkWorseClimbingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseClimbingStairs").InnerText);
            chkWorseDescendingStairs.Checked = node.SelectSingleNode("WorseDescendingStairs") == null ? chkWorseDescendingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDescendingStairs").InnerText);
            chkWorseDriving.Checked = node.SelectSingleNode("WorseDriving") == null ? chkWorseDriving.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDriving").InnerText);
            chkWorseWorking.Checked = node.SelectSingleNode("WorseWorking") == null ? chkWorseWorking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorking").InnerText);
            chkWorseBending.Checked = node.SelectSingleNode("WorseBending") == null ? chkWorseBending.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseBending").InnerText);
            chkWorseLifting.Checked = node.SelectSingleNode("WorseLifting") == null ? chkWorseLifting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLifting").InnerText);
            chkWorseTwisting.Checked = node.SelectSingleNode("WorseTwisting") == null ? chkWorseTwisting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseTwisting").InnerText);
            chkImprovedResting.Checked = node.SelectSingleNode("ImprovedResting") == null ? chkImprovedResting.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedResting").InnerText);
            chkImprovedMedication.Checked = node.SelectSingleNode("ImprovedMedication") == null ? chkImprovedMedication.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedication").InnerText);
            chkImprovedTherapy.Checked = node.SelectSingleNode("ImprovedTherapy") == null ? chkImprovedTherapy.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapy").InnerText);
            chkImprovedSleeping.Checked = node.SelectSingleNode("ImprovedSleeping") == null ? chkImprovedSleeping.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleeping").InnerText);
            chkImprovedMovement.Checked = node.SelectSingleNode("ImprovedMovement") == null ? chkImprovedMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMovement").InnerText);

            if (txtFwdFlexRight.Text == "") txtFwdFlexRight.Text = node.SelectSingleNode("FwdFlexRight") == null ? txtFwdFlexRight.Text.ToString().Trim() : node.SelectSingleNode("FwdFlexRight").InnerText;
            if (txtFwdFlexNormal.Text == "") txtFwdFlexNormal.Text = node.SelectSingleNode("NeckFwdFlexNormal") == null ? txtFwdFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("NeckFwdFlexNormal").InnerText;

            if (txtExtensionRight.Text == "") txtExtensionRight.Text = node.SelectSingleNode("FwdFlexRight") == null ? txtExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("FwdFlexRight").InnerText;
            if (txtExtensionNormal.Text == "") txtExtensionNormal.Text = node.SelectSingleNode("NeckExtNormal") == null ? txtExtensionNormal.Text.ToString().Trim() : node.SelectSingleNode("NeckExtNormal").InnerText;

            if (txtRotationRight.Text == "") txtRotationRight.Text = node.SelectSingleNode("RotationRight") == null ? txtRotationRight.Text.ToString().Trim() : node.SelectSingleNode("RotationRight").InnerText;
            //if (txtRotationRightWas.Text == "") txtRotationRightWas.Text = node.SelectSingleNode("RotationRight") == null ? txtRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("RotationRight").InnerText;
            if (txtRotationLeft.Text == "") txtRotationLeft.Text = node.SelectSingleNode("RotationLeft") == null ? txtRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("RotationLeft").InnerText;
            if (txtRotationNormal.Text == "") txtRotationNormal.Text = node.SelectSingleNode("NeckRotNormal") == null ? txtRotationNormal.Text.ToString().Trim() : node.SelectSingleNode("NeckRotNormal").InnerText;

            if (txtLateralFlexRight.Text == "") txtLateralFlexRight.Text = node.SelectSingleNode("LateralFlexRight") == null ? txtLateralFlexRight.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexRight").InnerText;
            if (txtLateralFlexNormal.Text == "") txtLateralFlexNormal.Text = node.SelectSingleNode("NeckLatFlex") == null ? txtLateralFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("NeckLatFlex").InnerText;

            if (txtLateralFlexLeft.Text == "") txtLateralFlexLeft.Text = node.SelectSingleNode("LateralFlexLeft") == null ? txtLateralFlexLeft.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexLeft").InnerText;
            // if (txtLateralFlexLeftWas.Text == "") txtLateralFlexLeftWas.Text = node.SelectSingleNode("LateralFlexLeft") == null ? txtLateralFlexLeftWas.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexLeft").InnerText;
            if (txtPalpationAt.Text == "") txtPalpationAt.Text = node.SelectSingleNode("PalpationAt") == null ? txtPalpationAt.Text.ToString().Trim() : node.SelectSingleNode("PalpationAt").InnerText;
            // if (txtLevels.Text == "") txtLevels.Text = node.SelectSingleNode("Levels") == null ? txtLevels.Text.ToString().Trim() : node.SelectSingleNode("Levels").InnerText;
            if (cboSpurlings.Text == "") cboSpurlings.Text = node.SelectSingleNode("Spurlings") == null ? cboSpurlings.Text.ToString().Trim() : node.SelectSingleNode("Spurlings").InnerText;
            if (cboDistraction.Text == "") cboDistraction.Text = node.SelectSingleNode("Distraction") == null ? cboDistraction.Text.ToString().Trim() : node.SelectSingleNode("Distraction").InnerText;
            if (cboTPSide1.Text == "") cboTPSide1.Text = node.SelectSingleNode("TPSide1") == null ? cboTPSide1.Text.ToString().Trim() : node.SelectSingleNode("TPSide1").InnerText;
            if (txtTPText1.Text == "") txtTPText1.Text = node.SelectSingleNode("TPText1") == null ? txtTPText1.Text.ToString().Trim() : node.SelectSingleNode("TPText1").InnerText;
            if (cboTPSide2.Text == "") cboTPSide2.Text = node.SelectSingleNode("TPSide2") == null ? cboTPSide2.Text.ToString().Trim() : node.SelectSingleNode("TPSide2").InnerText;
            if (txtTPText2.Text == "") txtTPText2.Text = node.SelectSingleNode("TPText2") == null ? txtTPText2.Text.ToString().Trim() : node.SelectSingleNode("TPText2").InnerText;
            if (cboTPSide3.Text == "") cboTPSide3.Text = node.SelectSingleNode("TPSide3") == null ? cboTPSide3.Text.ToString().Trim() : node.SelectSingleNode("TPSide3").InnerText;
            if (txtTPText3.Text == "") txtTPText3.Text = node.SelectSingleNode("TPText3") == null ? txtTPText3.Text.ToString().Trim() : node.SelectSingleNode("TPText3").InnerText;
            if (cboTPSide4.Text == "") cboTPSide4.Text = node.SelectSingleNode("TPside4") == null ? cboTPSide4.Text.ToString().Trim() : node.SelectSingleNode("TPside4").InnerText;
            if (txtTPText4.Text == "") txtTPText4.Text = node.SelectSingleNode("TPText4") == null ? txtTPText4.Text.ToString().Trim() : node.SelectSingleNode("TPText4").InnerText;
            if (cboTPSide5.Text == "") cboTPSide5.Text = node.SelectSingleNode("TPSide5") == null ? cboTPSide5.Text.ToString().Trim() : node.SelectSingleNode("TPSide5").InnerText;
            if (txtTPText5.Text == "") txtTPText5.Text = node.SelectSingleNode("TPText5") == null ? txtTPText5.Text.ToString().Trim() : node.SelectSingleNode("TPText5").InnerText;
            if (cboTPSide6.Text == "") cboTPSide6.Text = node.SelectSingleNode("TPSide6") == null ? cboTPSide6.Text.ToString().Trim() : node.SelectSingleNode("TPSide6").InnerText;
            if (txtTPText6.Text == "") txtTPText6.Text = node.SelectSingleNode("TPText6") == null ? txtTPText6.Text.ToString().Trim() : node.SelectSingleNode("TPText6").InnerText;
            if (cboTPSide7.Text == "") cboTPSide7.Text = node.SelectSingleNode("TPSide7") == null ? cboTPSide7.Text.ToString().Trim() : node.SelectSingleNode("TPSide7").InnerText;
            if (txtTPTex7t.Text == "") txtTPTex7t.Text = node.SelectSingleNode("TPText7") == null ? txtTPTex7t.Text.ToString().Trim() : node.SelectSingleNode("TPText7").InnerText;
            if (txtFreeForm.Text == "") txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            if (txtFreeFormCC.Text == "") txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            if (txtFreeFormA.Text == "") txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty) : node.SelectSingleNode("FreeFormA").InnerText.Trim().Replace("      ", string.Empty);
            if (txtFreeFormP.Text == "") txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            if (txtWorseOtherText.Text == "") txtWorseOtherText.Text = node.SelectSingleNode("WorseOther") == null ? txtWorseOtherText.Text.ToString().Trim() : node.SelectSingleNode("WorseOther").InnerText;
            _fldPop = false;
        }
    }

    public void BindDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Standards = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' Order By BodyPart,Heading";
            SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' and IsConsidered=0 Order By BodyPart,Heading";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Standards);
            dgvStandards.DataSource = "";
            dgvStandards.DataSource = Standards.DefaultView;
            dgvStandards.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {

        }
    }

    public string SaveStandards(string ieID)
    {
        string ids = string.Empty;
        try
        {
            foreach (GridViewRow row in dgvStandards.Rows)
            {
                string Procedure_ID, MCODE, BodyPart, Heading, CCDesc, PEDesc, ADesc, PDesc;

                Procedure_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                Heading = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PDesc = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;

                ids += Session["PatientIE_ID"].ToString() + ",";
                SaveStdUI(ieID, Procedure_ID, Heading, PDesc);
            }
        }
        catch (Exception ex)
        {

        }
        if (ids != string.Empty)
            return "Standard(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }

    public void SaveStdUI(string ieID, string iStdID, string heading, string pdesc)
    {
        string[] _Type = iStdID.Split('_');
        int _StdID = Convert.ToInt32(_Type[0]);
        string Part = Convert.ToString(_Type[1]);

        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        //long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + ieID + " AND ProcedureDetail_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        //if (sqlTbl.Rows.Count == 0 && StdChecked == true)
        //    _ieMode = "New";
        //else if (sqlTbl.Rows.Count == 0 && StdChecked == false)
        //    _ieMode = "None";
        //else if (sqlTbl.Rows.Count > 0 && StdChecked == false)
        //    _ieMode = "Delete";
        //else
        _ieMode = "Update";

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
            TblRow["ProcedureDetail_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;

            if (Part.Equals("R"))
            {
                TblRow["Heading"] = heading.ToString().Trim();
                TblRow["PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("S"))
            {
                TblRow["S_Heading"] = heading.ToString().Trim();
                TblRow["S_PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("E"))
            {
                TblRow["E_Heading"] = heading.ToString().Trim();
                TblRow["E_PDesc"] = pdesc.ToString().Trim();
            }

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
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

    protected void btnReset_Click(object sender, EventArgs e)// RoutedEventArgs
    {
        chkShoulderLeft1.Checked = false;
        chkScapulaLeft1.Checked = false;
        chkArmLeft1.Checked = false;
        chkForearmLeft1.Checked = false;
        chkHandLeft1.Checked = false;
        chkWristLeft1.Checked = false;
        chk1stDigitLeft1.Checked = false;
        chk2ndDigitLeft1.Checked = false;
        chk3rdDigitLeft1.Checked = false;
        chk4thDigitLeft1.Checked = false;
        chk5thDigitLeft1.Checked = false;
        chkShoulderRight1.Checked = false;
        chkScapulaRight1.Checked = false;
        chkArmRight1.Checked = false;
        chkForearmRight1.Checked = false;
        chkHandRight1.Checked = false;
        chkWristRight1.Checked = false;
        chk1stDigitRight1.Checked = false;
        chk2ndDigitRight1.Checked = false;
        chk3rdDigitRight1.Checked = false;
        chk4thDigitRight1.Checked = false;
        chk5thDigitRight1.Checked = false;
        chkShoulderBilateral1.Checked = false;
        chkScapulaBilateral1.Checked = false;
        chkArmBilateral1.Checked = false;
        chkForearmBilateral1.Checked = false;
        chkHandBilateral1.Checked = false;
        chkWristBilateral1.Checked = false;
        chk1stDigitBilateral1.Checked = false;
        chk2ndDigitBilateral1.Checked = false;
        chk3rdDigitBilateral1.Checked = false;
        chk4thDigitBilateral1.Checked = false;
        chk5thDigitBilateral1.Checked = false;
        chkShoulderNone1.Checked = false;
        chkScapulaNone1.Checked = false;
        chkArmNone1.Checked = false;
        chkForearmNone1.Checked = false;
        chkHandNone1.Checked = false;
        chkWristNone1.Checked = false;
        chk1stDigitNone1.Checked = false;
        chk2ndDigitNone1.Checked = false;
        chk3rdDigitNone1.Checked = false;
        chk4thDigitNone1.Checked = false;
        chk5thDigitNone1.Checked = false;
    }

    protected void btnReset1_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        chkShoulderLeft2.Checked = false;
        chkScapulaLeft2.Checked = false;
        chkArmLeft2.Checked = false;
        chkForearmLeft2.Checked = false;
        chkHandLeft2.Checked = false;
        chkWristLeft2.Checked = false;
        chk1stDigitLeft2.Checked = false;
        chk2ndDigitLeft2.Checked = false;
        chk3rdDigitLeft2.Checked = false;
        chk4thDigitLeft2.Checked = false;
        chk5thDigitLeft2.Checked = false;
        chkShoulderRight2.Checked = false;
        chkScapulaRight2.Checked = false;
        chkArmRight2.Checked = false;
        chkForearmRight2.Checked = false;
        chkHandRight2.Checked = false;
        chkWristRight2.Checked = false;
        chk1stDigitRight2.Checked = false;
        chk2ndDigitRight2.Checked = false;
        chk3rdDigitRight2.Checked = false;
        chk4thDigitRight2.Checked = false;
        chk5thDigitRight2.Checked = false;
        chkShoulderBilateral2.Checked = false;
        chkScapulaBilateral2.Checked = false;
        chkArmBilateral2.Checked = false;
        chkForearmBilateral2.Checked = false;
        chkHandBilateral2.Checked = false;
        chkWristBilateral2.Checked = false;
        chk1stDigitBilateral2.Checked = false;
        chk2ndDigitBilateral2.Checked = false;
        chk3rdDigitBilateral2.Checked = false;
        chk4thDigitBilateral2.Checked = false;
        chk5thDigitBilateral2.Checked = false;
        chkShoulderNone2.Checked = false;
        chkScapulaNone2.Checked = false;
        chkArmNone2.Checked = false;
        chkForearmNone2.Checked = false;
        chkHandNone2.Checked = false;
        chkWristNone2.Checked = false;
        chk1stDigitNone2.Checked = false;
        chk2ndDigitNone2.Checked = false;
        chk3rdDigitNone2.Checked = false;
        chk4thDigitNone2.Checked = false;
        chk5thDigitNone2.Checked = false;
    }

    private void AddStd_Click(object sender, EventArgs e) //RoutedEventArgs e
    {

        BindDataGrid();

    }

    public string SaveDiagnosis(string ieID)
    {
        string ids = string.Empty;
        try
        {
            ieID = Session["PatientIE_ID"].ToString();
            RemoveDiagCodesDetail(Session["patientFUId"].ToString());
            foreach (GridViewRow row in dgvDiagCodes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string Description, DiagCode, DiagCode_ID;

                    DiagCode_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                    //        DiagCodeDetail_ID = row.Cells[2].Controls.OfType<HiddenField>().FirstOrDefault().Value;

                    Description = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    DiagCode = row.Cells[0].Controls.OfType<TextBox>().FirstOrDefault().Text;

                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        ids += DiagCode_ID + ",";
                        SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    }

                }
            }

        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Diagnosis Code(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }

    public void SaveDiagUI(string ieID, string iDiagID, bool DiagChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID + " AND PatientFu_ID=" + Session["patientFUId"].ToString() + " and BodyPart like '%" + _CurBP + "%' ";
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

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
            TblRow["Diag_Master_ID"] = _DiagID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["PatientFU_ID"] = Session["patientFUId"].ToString();
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
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

    public void BindDCDataGrid()
    {
        try
        {
            if (!IsPostBack)
            {
                if (_FuId == "" || _FuId == "0")
                    return;
                string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
                string SqlStr = "";

                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientFU_ID = " + _FuId + " AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
                oSQLCmd.Connection = oSQLConn;
                oSQLCmd.CommandText = SqlStr;
                oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
                oSQLAdpr.Fill(Diagnosis);
                dgvDiagCodes.DataSource = "";
                dgvDiagCodes.DataSource = Diagnosis.DefaultView;
                dgvDiagCodes.DataBind();
                oSQLAdpr.Dispose();
                oSQLConn.Close();
            }
            else
            {
                if (ViewState["DiagnosisList"] != null)
                {
                    List<Adddiagnosis> objList = (List<Adddiagnosis>)ViewState["DiagnosisList"];

                    dgvDiagCodes.DataSource = objList;
                    dgvDiagCodes.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "Update";
        SaveDiagnosis(_CurIEid);
        SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    protected void AddStd_Click1(object sender, ImageClickEventArgs e)
    {
        string ieMode = "New";
        SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        Response.Redirect("AddStandards.aspx");
        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + "AddStandards.aspx" + "','_blank')", true);
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        //SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        // Response.Redirect("AddDiagnosis.aspx");
    }

    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Results/Result"))
        {
            cboSpurlings.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
            cboDistraction.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }

        ListItemCollection collection = new ListItemCollection();
        collection.Add(new ListItem("on the left"));
        collection.Add(new ListItem("on the right"));
        collection.Add(new ListItem("bilaterally"));
        collection.Add(new ListItem("left greater than right"));
        collection.Add(new ListItem("right greater than left"));

        ddlLevels.DataSource = collection;
        ddlLevels.DataBind();

    }

    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDCDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "TestFU", "closeModelPopup()", true);
    }

    public string SaveStandardsPopup(string ieID)
    {
        List<Adddiagnosis> objList = new List<Adddiagnosis>();
        Adddiagnosis obj = new Adddiagnosis();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvDiagCodesPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    obj = new Adddiagnosis();
                    obj.Diag_Master_ID = dgvDiagCodesPopup.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.DiagCode = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Description = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        ids += obj.DiagCode_ID + ",";
                        //  SaveStdUI(ieID, obj.DiagCode_ID, true, obj.BodyPart, obj.Description, obj.DiagCode);
                        objList.Add(obj);
                    }
                    //else
                    //{ SaveStdUI(ieID, obj.DiagCode_ID, false, obj.BodyPart, obj.Description, obj.DiagCode); }

                }
            }
            ViewState["DiagnosisList"] = objList;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }


    private void bindgridPoup()
    {
        try
        {
            _FuId = Session["patientFUId"].ToString();
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (_FuId != "")
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU(" + _FuId + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            else
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            ds = gDbhelperobj.selectData(SqlStr);

            dgvDiagCodesPopup.DataSource = ds;
            dgvDiagCodesPopup.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void RemoveDiagCodesDetail(string PatientFU_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblDiagCodesDetail WHERE PatientFU_ID=" + PatientFU_ID + " and BodyPart like '%" + _CurBP + "%'";
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }
    public void checkTP()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            if (isTP == false)
                divTP.Attributes.Add("style", "display:none");
            else
                divTP.Attributes.Add("style", "display:block");

        }
    }


    //protected void BindROM()
    //{

    //    _FuId = Session["patientFUId"].ToString();
    //    string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
    //    string SqlStr = "";
    //    oSQLConn.ConnectionString = sProvider;

    //    if (oSQLConn.State == ConnectionState.Closed)
    //        oSQLConn.Open();
    //    SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + _FuId;
    //    SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
    //    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
    //    DataTable sqlTbl = new DataTable();
    //    sqlAdapt.Fill(sqlTbl);
    //    oSQLConn.Close();
    //    if (sqlTbl.Rows.Count > 0)
    //    {
    //        string[] strname, strrom, strnormal;
    //        if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
    //        {
    //            strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
    //            strrom = sqlTbl.Rows[0]["ROM"].ToString().Split(',');
    //            strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


    //            // Create the Table
    //            DataTable OrdersTable = new DataTable("ROM");
    //            // Build the Orders schema
    //            OrdersTable.Columns.Add("name", Type.GetType("System.String"));
    //            OrdersTable.Columns.Add("rom", Type.GetType("System.String"));
    //            OrdersTable.Columns.Add("normal", Type.GetType("System.String"));

    //            DataRow workRow;

    //            for (int i = 0; i < strname.Length; i++)
    //            {

    //                workRow = OrdersTable.NewRow();
    //                workRow[0] = strname[i];
    //                workRow[1] = strrom[i];
    //                workRow[2] = strnormal[i];
    //                OrdersTable.Rows.Add(workRow);
    //            }

    //            if (OrdersTable.Rows.Count != 0)
    //            {
    //                repROM.DataSource = OrdersTable;
    //                repROM.DataBind();
    //            }
    //        }
    //        else
    //            getXMLROMvalue();
    //    }
    //    else
    //    {
    //        getXMLROMvalue();
    //    }
    //}

    //private void getXMLROMvalue()
    //{
    //    //open the tender xml file  
    //    XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Neck.xml"));
    //    //reading the xml data  
    //    DataSet ds = new DataSet();
    //    ds.ReadXml(xmlreader);
    //    xmlreader.Close();
    //    //if ds is not empty  
    //    if (ds.Tables.Count != 0)
    //    {
    //        repROM.DataSource = ds;
    //        repROM.DataBind();
    //    }
    //}
}