<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="findWorker2.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        'dark-bg': '#1F2937',
                        'dark-secondary': '#374151',
                        'blue-accent': '#3B82F6',
                    }
                }
            }
        }
    </script>
</head>
<body class="bg-dark-bg text-gray-200 min-h-screen flex items-center justify-center">
    <form id="form1" runat="server" class="bg-dark-secondary p-6 rounded-lg shadow-lg w-full max-w-md space-y-4 border border-gray-600">
        <h2 class="text-3xl font-bold text-blue-accent text-center mb-4">Sign Up</h2>

        <div>
            <label for="txtUsername" class="block text-sm font-medium text-gray-300 mb-1">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtUsername" ErrorMessage="Username is required." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <div>
            <label for="txtPassword" class="block text-sm font-medium text-gray-300 mb-1">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ErrorMessage="Password is required." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <div>
            <label for="txtEmail" class="block text-sm font-medium text-gray-300 mb-1">Email:</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtEmail" ErrorMessage="Email is required." runat="server" CssClass="text-red-500 text-sm mt-1" />
            <asp:RegularExpressionValidator ControlToValidate="txtEmail" ValidationExpression="\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}" ErrorMessage="Invalid email format." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <div>
            <label for="ddlUserType" class="block text-sm font-medium text-gray-300 mb-1">User Type:</label>
            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2">
                <asp:ListItem Text="Select User Type" Value="" />
                <asp:ListItem Text="Provider" Value="Provider" />
                <asp:ListItem Text="Worker" Value="Worker" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlUserType" InitialValue="" ErrorMessage="Please select a user type." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <div>
            <label for="txtFullName" class="block text-sm font-medium text-gray-300 mb-1">Full Name:</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtFullName" ErrorMessage="Full name is required." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <div>
            <label for="txtPhoneNum" class="block text-sm font-medium text-gray-300 mb-1">Phone Number:</label>
            <asp:TextBox ID="txtPhoneNum" runat="server" CssClass="block w-full rounded-md bg-gray-700 border border-gray-600 text-white focus:border-blue-accent focus:ring focus:ring-blue-accent focus:ring-opacity-50 p-2"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="txtPhoneNum" ErrorMessage="Phone number is required." runat="server" CssClass="text-red-500 text-sm mt-1" />
            <asp:RegularExpressionValidator ControlToValidate="txtPhoneNum" ValidationExpression="^\d{10}$" ErrorMessage="Phone number must be 10 digits." runat="server" CssClass="text-red-500 text-sm mt-1" />
        </div>

        <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" CssClass="w-full bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:ring focus:ring-blue-accent focus:ring-opacity-50" />

        <asp:Label ID="lblMessage" runat="server" CssClass="block mt-4 text-red-500"></asp:Label>

        <asp:Button ID="btnLogin" runat="server" Text="Go to Login" OnClick="btnLogin_Click" CausesValidation="false" CssClass="w-full bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-md focus:ring focus:ring-gray-600 focus:ring-opacity-50" />
    </form>
</body>
</html>
