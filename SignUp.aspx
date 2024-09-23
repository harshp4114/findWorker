<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="findWorker2.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Sign Up</h2>

            <label for="txtUsername">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtUsername" ErrorMessage="Username is required." runat="server" ForeColor="Red" /><br /><br />

            <label for="txtPassword">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password is required." runat="server" ForeColor="Red" /><br /><br />

            <label for="txtEmail">Email:</label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtEmail" ErrorMessage="Email is required." runat="server" ForeColor="Red" /><br />
            <asp:RegularExpressionValidator ControlToValidate="txtEmail" ValidationExpression="\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}" ErrorMessage="Invalid email format." runat="server" ForeColor="Red" /><br /><br />

            <label for="ddlUserType">User Type:</label>
            <asp:DropDownList ID="ddlUserType" runat="server">
                <asp:ListItem Text="Select User Type" Value="" />
                <asp:ListItem Text="Provider" Value="Provider" />
                <asp:ListItem Text="Worker" Value="Worker" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlUserType" InitialValue="" ErrorMessage="Please select a user type." runat="server" ForeColor="Red" /><br /><br />

            <label for="txtFullName">Full Name:</label>
            <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtFullName" ErrorMessage="Full name is required." runat="server" ForeColor="Red" /><br /><br />

            <label for="txtPhoneNum">Phone Number:</label>
            <asp:TextBox ID="txtPhoneNum" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPhoneNum" ErrorMessage="Phone number is required." runat="server" ForeColor="Red" /><br />
            <asp:RegularExpressionValidator ControlToValidate="txtPhoneNum" ValidationExpression="^\d{10}$" ErrorMessage="Phone number must be 10 digits." runat="server" ForeColor="Red" /><br /><br />

            <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br /><br />

            <asp:Button ID="btnLogin" runat="server" Text="Go to Login" OnClick="btnLogin_Click" CausesValidation="false" />
        </div>
    </form>
</body>
</html>
