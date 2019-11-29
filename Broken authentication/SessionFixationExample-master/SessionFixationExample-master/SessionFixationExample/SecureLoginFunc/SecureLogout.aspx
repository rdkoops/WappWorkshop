<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecureLogout.aspx.cs" Inherits="SessionFixationExample.SecureLogout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />

            <asp:Button ID="btnLogout" runat="server"
                Text="Logout" Visible="false" OnClick="btnLogout_Click" />

        </div>
        <p>
            <asp:Label ID="lblAuthCookie" runat="server" EnableViewState="false" />
        </p>
    </form>
</body>
</html>
