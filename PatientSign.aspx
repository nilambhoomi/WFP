<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientSign.aspx.cs" Inherits="PatientSign" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tools">
        <a href="#colors_sketch" data-tool="marker">Marker</a> <a href="#colors_sketch" data-tool="eraser">
            Eraser</a>
    </div>
    <br />
    <canvas id="colors_sketch" width="500" height="200" style="border:solid"></canvas>
    <br />
    <br />
    <asp:HiddenField ID="hfImageData" runat="server" />
    <asp:Button ID="btnSave" Text="Save" runat="server" UseSubmitBehavior="false" OnClick="Save"
        OnClientClick="return ConvertToImage(this)" />
    </form>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="https://cdn.rawgit.com/mobomo/sketch.js/master/lib/sketch.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#colors_sketch').sketch();
            $(".tools a").eq(0).attr("style", "color:#000");
            $(".tools a").click(function () {
                $(".tools a").removeAttr("style");
                $(this).attr("style", "color:#000");
            });
        });
        function ConvertToImage(btnSave) {
            var base64 = $('#colors_sketch')[0].toDataURL();
            $("[id*=hfImageData]").val(base64);
            __doPostBack(btnSave.name, "");
        };
    </script>
</body>
</html>
