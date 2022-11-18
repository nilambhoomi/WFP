<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>

<%--<%@ Register Assembly="WebSignature" Namespace="RealSignature" TagPrefix="ASP" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="css/signature-pad.css" />
    <script type="text/javascript">
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-39365077-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
  </script>
</head>
<body>
    <form runat="server" id="form1">
        <div id="signature-pad" class="signature-pad">

            <table style="width: 200px">
                <tr>
                    <td>
                        <canvas runat="server" id="can" style="height: 200px"></canvas>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="signature-pad--actions">
                            <div>
                                <button type="button" class="button clear" data-action="clear">Clear</button>
                                <button type="button" class="button" data-action="change-color">Change color</button>
                                <button type="button" class="button" data-action="undo">Undo</button>

                            </div>
                            <div>
                                <button type="button" class="button save" data-action="save-png">Save as PNG</button>
                                <button type="button" class="button save" data-action="save-jpg">Save as JPG</button>
                                <button type="button" class="button save" data-action="save-svg">Save as SVG</button>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img id="imgDemo" />
                    </td>
                </tr>
                <tr>
                    <td>
                       
                    </td>
                </tr>
            </table>

            <div class="signature-pad--body">
            </div>
            <br />
            <div class="signature-pad--footer">
                <div class="description">Sign above</div>


            </div>
        </div>

    </form>


</body>
<script src="js/signature_pad.umd.js"></script>
<script src="js/app.js"></script>
</html>
