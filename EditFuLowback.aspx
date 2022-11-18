<%@ Page Title="" Language="C#" MasterPageFile="~/FollowUpMaster.master" AutoEventWireup="true" CodeFile="EditFuLowback.aspx.cs" Inherits="EditFuLowback" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="js/images/bootstrap.min.js"></script>
    <style>
        /*.table{
    display:table;
    width:100%;
    table-layout:fixed;
}*/
        .table_cell {
            /*display:table-cell;*/
            width: 100px;
            /*border:solid black 1px;*/
        }
    </style>
    <style>
        .ui-button ui-widget ui-state-default ui-button-icon-only ui-corner-right ui-button-icon {
        }

        .ui-menu-item {
            /*display: none;*/
            position: absolute;
            background-color: #f9f9f9;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }

        .ui-corner-all a {
            color: black;
            /*padding: 12px 16px;*/
            text-decoration: none;
            display: block;
        }
    </style>
    <style>
        button.ui-button-icon-only {
            width: 1.4em;
            height: 27px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                document.getElementById('<%= btnSave.ClientID %>').click();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
    <div id="mymodelmessage" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMessage" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label runat="server" id="lblMessage"></label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- start: Header -->
    <%--    <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>--%>
    <div class="container">
        <div class="row">
            <div class="col-lg-10" id="content">
                <%--    <ul class="breadcrumb">
                                <li>
                                    <i class="icon-home"></i>
                                    <a href="Page1.aspx"><span class="label">Page1</span></a>
                                </li>
                                <li id="lipage2">
                                    <i class="icon-edit"></i>
                                    <a href="Page2.aspx"><span class="label label-success">Page2</span></a>
                                </li>
                                <li id="li1" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page3.aspx"><span class="label">Page3</span></a>
                                </li>
                                <li id="li2" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page4.aspx"><span class="label">Page4</span></a>
                                </li>
                            </ul>--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>CHIEF COMPLAINT</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label class="control-label">The patient complaints of Lower back pain that is&nbsp;</label>
                        <asp:TextBox runat="server" ID="txtPainScale" Style="width: 40px;"></asp:TextBox>
                        <label class="control-label">&nbsp;/10, with 10 being the worst, which is</label>
                        <asp:CheckBox ID="chkContent" runat="server" Text="constant" />
                        <asp:CheckBox ID="chkIntermittent" runat="server" Text="intermittent." />
                        <asp:CheckBox ID="chkSharp" runat="server" Text="sharp" />
                        <asp:CheckBox ID="chkelectric" runat="server" Text="electric" />
                        <asp:CheckBox ID="chkshooting" runat="server" Text="shooting" />
                        <asp:CheckBox ID="chkthrobbing" runat="server" Text="throbbing" />
                        <asp:CheckBox ID="chkpulsating" runat="server" Text="pulsating" />
                        <asp:CheckBox ID="chkdull" runat="server" Text="dull" />
                        <asp:CheckBox ID="chkachy" runat="server" Text="achy in nature." />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:Label Font-Bold="false" runat="server" Text="The Lower back pain radiates to:"></asp:Label><br />
                        <asp:TextBox ID="txtRadiates" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtburningto" runat="server" Visible="false"></asp:TextBox>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:ImageButton ID="btnReset" runat="server" Height="30px" Width="30px" ImageUrl="~/img/reset.png" OnClick="btnReset_Click" /></th>
                                    <th class="table_cell">
                                        <label>Side</label></th>
                                    <th class="table_cell">
                                        <label>Buttock</label></th>
                                    <th class="table_cell">
                                        <label>Groin</label></th>
                                    <th class="table_cell">
                                        <label>Hip</label></th>
                                    <th class="table_cell">
                                        <label>Thigh</label></th>
                                    <th class="table_cell">
                                        <label>Leg</label></th>
                                    <th class="table_cell">
                                        <label>Knee</label></th>
                                    <th class="table_cell">
                                        <label>Ankle</label></th>
                                    <th class="table_cell">
                                        <label>Feet</label></th>
                                    <th class="table_cell">
                                        <label>Toe</label></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="">Left</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideLeft1" runat="server" GroupName="RS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockLeft1" runat="server" GroupName="RB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinLeft1" runat="server" GroupName="RG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipLeft1" runat="server" GroupName="RH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighLeft1" runat="server" GroupName="RTH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegLeft1" runat="server" GroupName="RL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKneeLeft1" runat="server" GroupName="RK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkleLeft1" runat="server" GroupName="RA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeetLeft1" runat="server" GroupName="RF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToeLeft1" runat="server" GroupName="RTO" /></td>
                                </tr>
                                <tr>
                                    <td style="">Right</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideRight1" runat="server" GroupName="RS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockRight1" runat="server" GroupName="RB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinRight1" runat="server" GroupName="RG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipRight1" runat="server" GroupName="RH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighRight1" runat="server" GroupName="RTH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegRight1" runat="server" GroupName="RL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKneeRight1" runat="server" GroupName="RK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkleRight1" runat="server" GroupName="RA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeetRight1" runat="server" GroupName="RF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToeRight1" runat="server" GroupName="RTO" /></td>

                                </tr>
                                <tr>
                                    <td style="">Bilateral</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideBilateral1" runat="server" GroupName="RS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockBilateral1" runat="server" GroupName="RB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinBilateral1" runat="server" GroupName="RG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipBilateral1" runat="server" GroupName="RH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighBilateral1" runat="server" GroupName="RTH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegBilateral1" runat="server" GroupName="RL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKneeBilateral1" runat="server" GroupName="RK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkleBilateral1" runat="server" GroupName="RA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeetBilateral1" runat="server" GroupName="RF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToeBilateral1" runat="server" GroupName="RTO" /></td>
                                </tr>
                                <tr>
                                    <td style="">None</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSide1None" runat="server" GroupName="RS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtock1None" runat="server" GroupName="RB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroin1None" runat="server" GroupName="RG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHip1None" runat="server" GroupName="RH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThigh1None" runat="server" GroupName="RTH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLeg1None" runat="server" GroupName="RL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKnee1None" runat="server" GroupName="RK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkle1None" runat="server" GroupName="RA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeet1None" runat="server" GroupName="RF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToe1None" runat="server" GroupName="RTO" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:Label Font-Bold="false" ID="Label2" runat="server" Text="The lower back pain is associated with "></asp:Label>
                        <asp:CheckBox ID="chknumbness" runat="server" Text="numbness" Checked="true" />
                        <asp:CheckBox ID="chktingling" runat="server" Text="tingling and  " Checked="true" />
                        <asp:CheckBox ID="chkBurning" runat="server" Text="burning sensation to " /><br />
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:ImageButton ID="btnReset1" Height="30px" Width="30px" runat="server" ImageUrl="~/img/reset.png" OnClick="btnReset1_Click" /></th>
                                    <th class="table_cell">
                                        <label>Side/</label></th>
                                    <th class="table_cell">
                                        <label>Buttock</label></th>
                                    <th class="table_cell">
                                        <label>Groin</label></th>
                                    <th class="table_cell">
                                        <label>Hip</label></th>
                                    <th class="table_cell">
                                        <label>Thigh</label></th>
                                    <th class="table_cell">
                                        <label>Leg</label></th>
                                    <th class="table_cell">
                                        <label>Knee</label></th>
                                    <th class="table_cell">
                                        <label>Ankle</label></th>
                                    <th class="table_cell">
                                        <label>Feet</label></th>
                                    <th class="table_cell">
                                        <label>Toe</label></th>
                                    <th class="table_cell"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="">Left</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideLeft2" runat="server" GroupName="AS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockLeft2" runat="server" GroupName="AB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinLeft2" runat="server" GroupName="AG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipLeft2" runat="server" GroupName="AH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighLeft2" runat="server" GroupName="ATH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegLeft2" runat="server" GroupName="AL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKneeLeft2" runat="server" GroupName="AK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkleLeft2" runat="server" GroupName="AA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeetLeft2" runat="server" GroupName="AF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToeLeft2" runat="server" GroupName="ATO" /></td>
                                </tr>
                                <tr>
                                    <td style="">Right</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideRight2" runat="server" GroupName="AS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockRight2" runat="server" GroupName="AB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinRight2" runat="server" GroupName="AG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipRight2" runat="server" GroupName="AH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighRight2" runat="server" GroupName="ATH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegRight2" runat="server" GroupName="AL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitRight2" runat="server" GroupName="AK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitRight2" runat="server" GroupName="AA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitRight2" runat="server" GroupName="AF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitRight2" runat="server" GroupName="ATO" /></td>
                                </tr>
                                <tr>
                                    <td style="">Bilateral</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSideBilateral2" runat="server" GroupName="AS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtockBilateral2" runat="server" GroupName="AB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroinBilateral2" runat="server" GroupName="AG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHipBilateral2" runat="server" GroupName="AH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThighBilateral2" runat="server" GroupName="ATH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLegBilateral2" runat="server" GroupName="AL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKneeBilateral2" runat="server" GroupName="AK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkleBilateral2" runat="server" GroupName="AA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeetBilateral2" runat="server" GroupName="AF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToeBilateral2" runat="server" GroupName="ATO" /></td>
                                </tr>
                                <tr>
                                    <td style="">None</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkSide2None" runat="server" GroupName="AS" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkButtock2None" runat="server" GroupName="AB" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkGroin2None" runat="server" GroupName="AG" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHip2None" runat="server" GroupName="AH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkThigh2None" runat="server" GroupName="ATH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkLeg2None" runat="server" GroupName="AL" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkKnee2None" runat="server" GroupName="AK" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkAnkle2None" runat="server" GroupName="AA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkFeet2None" runat="server" GroupName="AF" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkToe2None" runat="server" GroupName="ATO" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label class="control-label">Lower back pain is associated with weakness in</label>
                        <asp:TextBox ID="txtWeeknessIn" runat="server"></asp:TextBox>
                        <label class="control-label">.  The Lower back pain is worsened with</label>
                        <asp:CheckBox ID="chkWorseSitting" runat="server" Checked="true" Text="sitting,  " />
                        <asp:CheckBox ID="chkWorseStanding" runat="server" Checked="true" Text="standing,  " />
                        <asp:CheckBox ID="chkWorseLyingDown" runat="server" Checked="true" Text="lying down,  " />
                        <asp:CheckBox ID="chkWorseMovement" runat="server" Text="movement,  " />
                        <asp:CheckBox ID="chkWorseBending" runat="server" Text="bending,  " />
                        <asp:CheckBox ID="chkWorseLifting" runat="server" Text="lifting,  " />
                        <asp:CheckBox ID="chkWorseSeatingtoStandingUp" runat="server" Text="going from seating to standing up,  " />
                        <asp:CheckBox ID="chkWorseWalking" runat="server" Text="walking,  " />
                        <asp:CheckBox ID="chkWorseClimbingStairs" runat="server" Text="Climbing stairs,  " />
                        <asp:CheckBox ID="chkWorseDescendingStairs" runat="server" Text="descending stairs " />
                        <asp:CheckBox ID="chkWorseDriving" runat="server" Text="driving, " />

                        <asp:CheckBox ID="chkWorseWorking" runat="server" Text="working,  " />
                        <asp:CheckBox ID="chkWorseOther" runat="server" Text="and" />
                        <asp:TextBox ID="txtWorseOtherText" runat="server"></asp:TextBox>
                        <label class="control-label">. The lower back pain is improved with</label>
                        <asp:CheckBox ID="chkImprovedResting" runat="server" Text="resting, " />
                        <asp:CheckBox ID="chkImprovedMedication" runat="server" Text="medication, " />
                        <asp:CheckBox ID="chkImprovedTherapy" runat="server" Text="therapy, " />
                        <asp:CheckBox ID="chkImprovedSleeping" runat="server" Text="sleeping, " />
                        <asp:CheckBox ID="chkImprovedMovement" runat="server" Text="movement. " />

                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:TextBox runat="server" ID="txtFreeFormCC" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                            <button type="button" id="start_button1" onclick="startButton1(event)">
                                <img src="images/mic.gif" alt="start" /></button>
                            <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PHYSICAL EXAM:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <th>
                                    <table style="max-width: 350px">
                                        <tr>
                                            <td style="text-align: left;">
                                                <label>lumbar spine exam</label>
                                            </td>
                                            <td style="">
                                                <label>ROM</label>
                                            </td>
                                            <%--  <td style=""><label>Right</label>
                                            </td>--%>
                                            <td style="">
                                                <label>Normal</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <label>Forward flexion</label></td>
                                            <%-- <td>
                                                <asp:TextBox ID="txtFwdFlexWas" Text="30" Width="40px" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtFwdFlex" runat="server" Width="50px" Text="30"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtFwdFlexNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <label>Extension</label></td>
                                            <%--<td>
                                                <asp:TextBox ID="txtExtensionWas" Text="10" Width="40px" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtExtension" Text="10" Width="50px" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtExtensionNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </th>
                                <th>

                                    <%-- <asp:Repeater runat="server" ID="repROM">
                                            <HeaderTemplate>
                                                <table style="width: 40%;">

                                                    <thead>
                                                        <tr>
                                                            <td style="text-align: left;"></td>
                                                            <td></td>
                                                            <td></td>


                                                        </tr>
                                                    </thead>
                                                    <tr>
                                                        <td></td>
                                                        <td style="">ROM
                                                        </td>
                                                        <td style="">Normal
                                                        </td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        <asp:Label runat="server" ID="lblname" Text='<%# Eval("name") %>'></asp:Label></td>

                                                    <td>
                                                        <asp:TextBox ID="txtrom" runat="server" Width="50px" onkeypress="return onlyNumbers(event);" Text='<%# Eval("rom") %>'></asp:TextBox></td>
                                                    <td>
                                                        <asp:TextBox ID="txtnormal" ReadOnly="true" Text='<%# Eval("normal") %>' Width="50px" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>--%>

                                    <table style="max-width: 400px;">
                                        <tr>
                                            <td style="text-align: left;"></td>
                                            <%--<td><label>Left</label>--%>
                                            </td>
                                            <td style="">
                                                <label>Left</label>
                                            </td>
                                            <%-- <td style=""><label>Right</label>--%>
                                            </td>
                                            <td style="">
                                                <label>Right</label>
                                            </td>
                                            <td style="">
                                                <label>Normal</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <label>Rotation</label></td>
                                            <%-- <td>
                                               <asp:TextBox ID="txtRotationLeftWas" Width="40px" Text="10" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtRotationLeft" Width="50px" Text="10" runat="server"></asp:TextBox></td>
                                            <%--<td>
                                                <asp:TextBox ID="txtRotationRightWas" Width="40px" Text="10" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtRotationRight" Width="50px" runat="server" Text="10"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txttxtRotationNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">
                                                <label>Lateral flexion</label></td>
                                            <%--<td>
                                                <asp:TextBox ID="txtLateralFlexLeftWas" Width="40px" runat="server" Text="10"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexLeft" Width="50px" runat="server" Text="10"></asp:TextBox></td>
                                            <%--<td>
                                                <asp:TextBox ID="txtLateralFlexRightWas" Width="40px" runat="server" Text="10"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexRight" Width="50px" runat="server" Text="10"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label class="control-label">The lumbar spine exam reveals tenderness upon palpation at &nbsp;</label>
                        <asp:TextBox ID="txtPalpationAt" Text="L1-S1" runat="server"></asp:TextBox>
                        <label class="control-label">levels</label>
                        <editable:EditableDropDownList runat="server" ID="cboLevels" Width="257px" CssClass="inline">
                        </editable:EditableDropDownList>
                        <label class="control-label" style="display: none">with muscle spasm present.</label><br />
                        <br />

                        <div class="table-responsive">
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <td></td>
                                        <td colspan="1">Strainght leg raise exam</td>
                                        <td colspan="1">Braggard's test</td>
                                        <td style="">Kernig's sign</td>
                                        <td style="">Brudzinski's test</td>
                                        <td style="">Sacroiliac compression</td>
                                        <td style="">Sacral notch tenderness</td>
                                        <td style="">Ober's test causing pain at the SI joint</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td style="">Left</td>
                                        <td colspan="1">
                                            <asp:CheckBox ID="chkLegRaisedExamLeft" runat="server" />
                                            <asp:TextBox ID="txtLegRaisedExamLeft" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="1">
                                            <asp:CheckBox ID="chkBraggardLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberLeft" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="">Right</td>
                                        <td>
                                            <asp:CheckBox ID="chkLegRaisedExamRight" runat="server" />
                                            <asp:TextBox ID="txtchkLegRaisedExamRight" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkBraggardRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberRight" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="">Bilateral</td>
                                        <td>
                                            <asp:CheckBox ID="chkLegRaisedExamBilateral" runat="server" />
                                            <asp:TextBox ID="txtLegRaisedExamBilateral" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkBraggardBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberBilateral" runat="server" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>


                <div class="row" runat="server" id="divTP">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>Trigger Point:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide1" DataSourceID="TPSide1XML" DataTextField="name" runat="server" Style="height: 30px; width: 200px"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide1XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText1" Style="margin-left: 20px;" runat="server" Text="para spinal level L3-S1 with referral patterns laterally to the region in a fan-like pattern" Width="556px"></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" Style="" ID="txtFreeForm" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upMedicine">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"><b><u>ASSESSMENT/DIAGNOSIS:</u></b></label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Notes:</label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:TextBox runat="server" Style="float: left;" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                <%-- <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="basicPopup();" OnClick="AddDiag_Click" />--%>
                                <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />
                                <%-- <asp:GridView ID="dgvDiagCodes" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" ReadOnly="true" runat="server" Width="400" Text='<%# Eval("Description") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>--%>
                                <asp:GridView ID="dgvDiagCodes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Width="100" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hidDiagCodeID" Value='<%# Eval("Diag_Master_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="700">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtpe" ReadOnly="true" runat="server" Width="700" Text='<%# Eval("Description") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="50">
                                            <ItemTemplate>
                                                <%--    <asp:HiddenField runat="server" ID="hidDiagCodeDetailID" Value='<%# Eval("DiagCodeDetail_ID") %>' />--%>
                                                <asp:CheckBox runat="server" ID="chkRemove" Checked="true" />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PLAN:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%-- <asp:CheckBox ID="chkCervicalSpine" Style="" Text="MRI" runat="server" />--%>
                        <%-- <asp:DropDownList ID="cboScanType" Style=" height: 25px;" runat="server"></asp:DropDownList>
                                <asp:Label ID="Label7" Style="" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style="" Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="display: none;" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:GridView ID="dgvStandards" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDesc" ItemStyle-Width="600">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPDesc" runat="server" CssClass="form-control" Width="600px" TextMode="MultiLine" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="IsChkd">

                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" value='<%# Convert.ToBoolean(Eval("IsChkd")) %>' AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="MCODE" ItemStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="mcode" runat="server" Text='<%# Eval("MCODE") %>'></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%--<asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
                    </ItemTemplate>
                                      </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>--%>
                                <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>--%>
                                <%--   <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px"  TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="CC" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("CCDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("PEDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AD" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtadesc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("ADesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PD" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdesc" Width="95" ReadOnly="true" runat="server" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="PN" ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                <%--<asp:TemplateField HeaderText="IsChkd">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox4" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row"></div>
                <div class="row" style="margin-top: 15px">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--<asp:ImageButton ID="LoadDV" Style="" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn blue" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>

    <div class="modal fade" id="MedicinePopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; width: 950px; margin-right: 20%">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    Select Diag 
                    <b id="CatHeading"></b>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMedice">
                        <ContentTemplate>

                            <div class="row" style="margin: 5px">
                                <div class="col-md-3">
                                    <asp:TextBox ID="txDesc" runat="server" Style="margin-bottom: 0px" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnSearch" Text="Filter" CssClass="btn btn-info" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" OnClick="btnDaigSave_Click" />
                                </div>
                                <br />

                                <div class="col-md-12">
                                    <asp:GridView ID="dgvDiagCodesPopup" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                                    <%--<asp:TextBox ID="txtbodypart" runat="server" Text='<%# Eval("BodyPart") %>'></asp:TextBox>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DiagCode") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="550">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" Width="550" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PN" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PreSelect") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //function basicPopup() {
        //    popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        //};
        //function basicPopup1() {
        //    popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        //}
        function OnSuccess(response) {
            //debugger;
            popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        }
        function OnSuccess_q(response) {
            popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');

        }
        function basicPopup() {
            document.forms[0].target = "_blank";
        };
    </script>
    <script>
        $.noConflict();
        function openModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('show');

            })(jQuery);
        }

        function closeModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('hide');

            })(jQuery);
        }

          var $j = jQuery.noConflict();
        $j('#MedicinePopup').on('hidden.bs.modal', function (e) {
            $('#ctl00_lnkbtn_lowback').addClass('active');
        });

        $(document).ready(function () {
            $('#rbl_in_past input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_injur_past_bp").prop('disabled', true);
                    $("#txt_injur_past_how").prop('disabled', true);
                }
                else {
                    $("#txt_injur_past_bp").prop('disabled', false);
                    $("#txt_injur_past_how").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rbl_seen_injury input').change(function () {
                if ($(this).val() == 'False') {
                    $("#txt_docname").prop('disabled', true);
                }
                else {
                    $("#txt_docname").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rep_wenttohospital input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_day").prop('disabled', true);
                    $("#txt_day").prop('value', "0");
                }
                else {
                    $("#txt_day").prop('disabled', false);
                    $("#txt_day").select();
                    $("#txt_day").focus();
                }
            });
        });

        $(document).ready(function () {
            $('#rep_hospitalized input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_hospital").prop('disabled', true);
                    $("#txt_day").prop('disabled', true);
                    $("#chk_mri").prop('disabled', true);
                    $("#txt_mri").prop('disabled', true);
                    $("#chk_CT").prop('disabled', true);
                    $("#txt_CT").prop('disabled', true);
                    $("#chk_xray").prop('disabled', true);
                    $("#txt_x_ray").prop('disabled', true);
                    $("#txt_prescription").prop('disabled', true);
                    $("#txt_which_what").prop('disabled', true);
                }
                else {
                    $("#txt_hospital").prop('disabled', false);
                    $("#ddl_via").prop('disabled', false);
                    $("#txt_day").prop('disabled', false);
                    $("#chk_mri").prop('disabled', false);
                    $("#txt_mri").prop('disabled', false);
                    $("#chk_CT").prop('disabled', false);
                    $("#txt_CT").prop('disabled', false);
                    $("#chk_xray").prop('disabled', false);
                    $("#txt_x_ray").prop('disabled', false);
                    $("#txt_prescription").prop('disabled', false);
                    $("#txt_which_what").prop('disabled', false);
                }
            });
        });
    </script>
    <script>
        var controlname = null;
        var final_transcript = '';
        var recognizing = false;
        var ignore_onend;
        var start_timestamp;

        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = true;

            recognition.onstart = function () {
                recognizing = true;
            };

            recognition.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend = true;
                }
            };

            recognition.onend = function () {
                recognizing = false;
                if (ignore_onend) {
                    return;
                }
                if (!final_transcript) {
                    //showInfo('info_start');
                    return;
                }
                if (!final_transcript1) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition.onresult = function (event) {
                var interim_transcript = '';
                if (typeof (event.results) == 'undefined') {
                    recognition.onend = null;
                    recognition.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript += event.results[i][0].transcript;
                    } else {
                        interim_transcript += event.results[i][0].transcript;
                    }
                }
                final_transcript = capitalize(final_transcript);
                //finalrecord = linebreak(final_transcript);
                //$('#ctl00_ContentPlaceHolder1_txtFreeForm').text(linebreak(final_transcript));
                $(controlname).text(linebreak(final_transcript));
                interim_span.innerHTML = linebreak(interim_transcript);
            };
        }



        var two_line = /\n\n/g;
        var one_line = /\n/g;
        function linebreak(s) {
            return s.replace(two_line, '<p></p>').replace(one_line, '<br>');
        }

        var first_char = /\S/;
        function capitalize(s) {
            return s.replace(first_char, function (m) { return m.toUpperCase(); });
        }

        function startButton(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeForm";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span.innerHTML = '';
            interim_span.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }

        function startButton1(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeFormCC";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span1.innerHTML = '';
            interim_span1.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }
    </script>
</asp:Content>


