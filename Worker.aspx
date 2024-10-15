<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Worker.aspx.cs" Inherits="findWorker2.Worker" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Worker Dashboard</title>
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
    <form id="form1" runat="server" class="bg-dark-secondary p-8 rounded-lg shadow-lg w-full max-w-5xl border border-gray-600 space-y-6">
        <h1 class="text-3xl font-bold text-blue-accent text-center mb-6">Available Works</h1>

        <asp:GridView ID="gvWorks" runat="server" AutoGenerateColumns="False" CssClass="w-full table-auto border-collapse text-left text-gray-300" OnRowCommand="gvWorks_RowCommand">
            <Columns>
                <asp:BoundField DataField="WorkID" HeaderText="ID" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="ProviderUsername" HeaderText="Provider" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2">
                    <ItemTemplate>
                        <span class='<%# Eval("Status").ToString() == "Open" ? "text-green-500" : "text-red-500" %>'>
                            <%# Eval("Status") %>
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="border border-gray-600 p-2" />
                </asp:TemplateField>
                <asp:BoundField DataField="DateOfWork" HeaderText="Date of Work" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="WorkerUsername" HeaderText="Worker Username" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2">
                    <ItemTemplate>
                        <asp:Button ID="btnApply" runat="server" Text="Apply" CommandName="Apply" CommandArgument='<%# Eval("WorkID") %>' CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-1 px-2 rounded-md focus:ring focus:ring-blue-accent focus:ring-opacity-50"  Enabled='<%# Eval("Status").ToString() == "Open" %>' />
                    </ItemTemplate>
                    <ItemStyle CssClass="border border-gray-600 p-2" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:Label ID="lblMessage" runat="server" CssClass="block mt-4 text-red-500"></asp:Label>

        <div class="flex justify-between mt-6">
            <asp:Button ID="btnViewRequests" runat="server" Text="View Pending Requests" OnClick="btnViewRequests_Click" CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:ring focus:ring-blue-accent focus:ring-opacity-50" />
            <asp:Button ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" CssClass="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-md focus:ring focus:ring-gray-600 focus:ring-opacity-50" />
        </div>
    </form>
</body>
</html>
