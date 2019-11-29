<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SessionFixationExample.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset runat="server">
                <legend>Login</legend>
                <p>
                    Username :
                    <asp:TextBox ID="txtUsername" runat="server" />
                </p>
                <p>
                    Password :
                    <asp:TextBox ID="txtPassword" runat="server" />
                </p>
                <p>
                    <asp:Button ID="btnSubmit" runat="server"
                        Text="Login" OnClick="btnSubmit_Click" />

                </p>
            </fieldset>
              <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
        </div>
       
    </form>
</body>
</html>
