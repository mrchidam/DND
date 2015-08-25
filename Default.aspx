<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Drop files on me</title>
    <style type="text/css">
        #dropOnMe {
            width: 210px;
            height: 136px;
            padding: 10px;
            border: 2px dashed gray;
            background-color: lightgray;
        }
    </style>

   <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script>
        $(document).ready(function () {
            // this function runs when the page loads to set up the drop area
            // Check if window.fileReader exists to make sure the browser 
            // supports file uploads
            if (typeof (window.FileReader) == 'undefined') {
                alert('Browser does not support HTML5 file uploads!');
            }
            dropOnMe.addEventListener("drop", dropHandler, false);
            dropOnMe.addEventListener("dragover", function (ev) {
                $("#dropOnMe").css("background-color", "lightgoldenrodyellow;");
                ev.preventDefault();
            }, false);
            function dropHandler(ev) {
                // Prevent default processing.
                ev.preventDefault();
                // Get the file(s) that are dropped.
                var filelist = ev.dataTransfer.files;
                if (!filelist) return;  // if null, exit now
                //$("#dropOnMe").text(filelist.length +
                //    " file(s) selected for uploading!");
                $("#dropOnMe").text(filelist[0].name);
                document.getElementById('txtFileName').value = filelist[0].name;

                var a = filelist[0].name;
                alert(a);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Default.aspx/validempmail",
                    data: "{'fileName':'" + a + "','company':'" + document.getElementById('ddCompany').value + "'}",
                    dataType: "json",
                    success: function (data) {
                        var b = data.d;
                        var c = b.split(',');
                        alert(c);
                        document.getElementById('txtOrderNo').value = c[0];
						document.getElementById('txtDistributor').value = c[1];
						document.getElementById('txtBranch').value = c[2];
						document.getElementById('txtPONumber').value = c[3];
						document.getElementById('txtDocumentType').value = c[4];
						document.getElementById('txtAdditionalInformation').value = c[5];
                        if (c[6] == 'true')
                        {
                            alert("SuccessFully Loaded Values From DB");
                        }

                        else
                        {
                            alert("Some Error Occured while Fetching from DB");
                        }
                    },
                    async: false
                });


               // $("#txtFileName").text(filelist[0].name);
                $("#upload").click(function () {
                    var data = new FormData();
                    for (var i = 0; i < filelist.length; i++) {
                        data.append(filelist[i].name, filelist[i]);
                    }
                    $.ajax({
                        type: "POST",
                        url: "Handler.ashx",
                        contentType: false,
                        processData: false,
                        data: data,
                        success: function (result) {
                            alert(result);
                        },
                        error: function () {
                            alert("hiiii");
                        },
                    });
                });
            }
            dropOnMe.addEventListener("dragend", function (ev) {
                $("#dropOnMe").css("background-color", "lightgray;");
                $("#dropOnMe").text("");
                $("upload").click(function () { });
                ev.preventDefault();
            }, false);
        });
    </script>
</head>
<body>
    <form runat="server">
    <h3>Drop Files on Me</h3>
    <div id="dropOnMe" draggable="false"></div>
    <div id="fileCount" draggable="false"></div>
   
        <asp:TextBox runat="server" ID="txtFileName"></asp:TextBox>

        <asp:TextBox runat="server" ID="txtOrderNo"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtBranch"></asp:TextBox>

        <asp:DropDownList ID="ddCompany" runat="server" Width="145px">
            <asp:ListItem Value="0">CURR - Curries</asp:ListItem>
            <asp:ListItem Value="1">GRAH - Graham</asp:ListItem>
            <asp:ListItem Value="2">CECO - Ceco</asp:ListItem>
            <asp:ListItem Value="3">FRMW - Frameworks</asp:ListItem>
        </asp:DropDownList>

        <asp:Button runat="server" Text="Process File" ID="upload" OnClick="upload_Click" />
        </form>
</body>
</html>
