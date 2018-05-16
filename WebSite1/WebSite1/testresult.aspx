<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testresult.aspx.cs" Inherits="testresult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Result</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label
            ID="lblTitle"
            BackColor="LightBlue"
            BorderWidth="3px"
            BorderStyle="Groove"
            Width="98%"
            Font-Size="30pt"
            font-name="Calibri"
            Text="<CENTER>Test Results</CENTER>"
            runat="server"></asp:Label>
    </div>
        <div>
            <asp:Table ID="Table1" runat="server" Width="98.5%" Height="100%">
                <asp:TableRow>
                    <asp:TableCell BackColor="White">
                        <div style="width: 15%; float: left; display: inline; height:1000px; border-color:black">
                              <br />
                              <asp:Label ID="Label1" Font-Bold="true" Font-Size="Medium" font-name="Calibri" runat="server" Text="Restult Directories"></asp:Label>
                            <br />
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </div>
                        <div>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                            <br />
                            
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                            <br />
                       </div>
                    </asp:TableCell>
  
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>
