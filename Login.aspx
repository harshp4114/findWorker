<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="findWorker2.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login</h2>

            <label for="txtUsername">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtUsername" ErrorMessage="Username is required." runat="server" ForeColor="Red" /><br /><br />

            <label for="txtPassword">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password is required." runat="server" ForeColor="Red" /><br /><br />

            <label for="ddlUserType">User Type:</label>
            <asp:DropDownList ID="ddlUserType" runat="server">
                <asp:ListItem Text="Select User Type" Value="" />
                <asp:ListItem Text="Provider" Value="Provider" />
                <asp:ListItem Text="Worker" Value="Worker" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlUserType" InitialValue="" ErrorMessage="Please select a user type." runat="server" ForeColor="Red" /><br /><br />

            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br /><br />

            <asp:Button ID="btnSignUp" runat="server" Text="Go to Sign Up" OnClick="btnSignUp_Click" CausesValidation="false" />
        </div>
    </form>
</body>
</html>
