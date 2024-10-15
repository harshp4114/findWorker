<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Provider.aspx.cs" Inherits="findWorker2.Provider" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Provider Dashboard</title>
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
        <h1 class="text-3xl font-bold text-blue-accent text-center mb-6">Welcome, <%= welcomeUser %>!</h1>
        
        <!-- Works Grid -->
        <h2 class="text-2xl font-semibold text-blue-accent">Your Works:</h2>
        <asp:GridView ID="gvWorks" runat="server" AutoGenerateColumns="False" CssClass="w-full table-auto border-collapse text-left text-gray-300">
            <Columns>
                <asp:BoundField DataField="WorkID" HeaderText="ID" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:BoundField DataField="DateOfWork" HeaderText="Date of Work" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                <asp:TemplateField HeaderText="Worker Username" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2">
                    <ItemTemplate>
                        <asp:Label ID="lblWorkerUsername" runat="server" Text='<%# Eval("WorkerUsername") %>' CssClass="p-2 truncate w-40"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle CssClass="border border-gray-600 p-2" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Form to Add New Work -->
        <h2 class="text-2xl font-semibold text-blue-accent mt-6">Add New Work</h2>
        <div class="space-y-4">
            <div>
                <label for="txtTitle" class="block text-gray-400">Title:</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="w-full bg-dark-bg text-gray-200 border border-gray-600 rounded-md p-2 focus:outline-none focus:ring-2 focus:ring-blue-accent"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" 
                    ErrorMessage="Title is required." ForeColor="Red" CssClass="text-red-500 text-sm"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label for="txtDescription" class="block text-gray-400">Description:</label>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="w-full bg-dark-bg text-gray-200 border border-gray-600 rounded-md p-2 focus:outline-none focus:ring-2 focus:ring-blue-accent"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                    ErrorMessage="Description is required." ForeColor="Red" CssClass="text-red-500 text-sm"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label for="txtDateOfWork" class="block text-gray-400">Date of Work:</label>
                <asp:TextBox ID="txtDateOfWork" runat="server" TextMode="Date" CssClass="w-full bg-dark-bg text-gray-200 border border-gray-600 rounded-md p-2 focus:outline-none focus:ring-2 focus:ring-blue-accent"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDateOfWork" runat="server" ControlToValidate="txtDateOfWork" 
                    ErrorMessage="Date of work is required." ForeColor="Red" CssClass="text-red-500 text-sm"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvDateOfWork" runat="server" ControlToValidate="txtDateOfWork" 
                    Type="Date" Operator="DataTypeCheck" 
                    ErrorMessage="Please enter a valid date." ForeColor="Red" CssClass="text-red-500 text-sm"></asp:CompareValidator>
                <asp:CustomValidator ID="cvFutureDate" runat="server" ControlToValidate="txtDateOfWork"
                    ErrorMessage="Date of work cannot be earlier than today's date." 
                    OnServerValidate="ValidateFutureDate" ForeColor="Red" CssClass="text-red-500 text-sm"></asp:CustomValidator>
            </div>
            <asp:Button ID="btnAddWork" runat="server" Text="Add Work" OnClick="btnAddWork_Click" CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-accent focus:ring-opacity-50" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="block mt-2 text-red-500"></asp:Label>
        </div>

        <!-- Button to View Requests on Separate Page -->
        <h2 class="text-2xl font-semibold text-blue-accent mt-6">Worker Requests</h2>
        <div class="flex justify-between mt-4">
            <asp:Button ID="btnViewRequests" runat="server" Text="View Worker Requests" CausesValidation="false" OnClick="btnViewRequests_Click" CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-accent focus:ring-opacity-50" />
            <asp:Button ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" CausesValidation="false" CssClass="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-600 focus:ring-opacity-50" />
        </div>
    </form>
</body>
</html>
