<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="SessionFixationExample.Welcome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

                <asp:Button ID="btnLogout" runat="server"
                    Text="Logout" Visible="false" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>
