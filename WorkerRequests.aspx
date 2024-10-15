<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkerRequests.aspx.cs" Inherits="findWorker2.WorkerRequests" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Worker Requests</title>
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
        <div>
            <h1 class="text-3xl font-bold text-blue-accent text-center mb-6">Your Pending Requests</h1>
            <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="False" 
                          CssClass="w-full table-auto border-collapse text-left text-gray-300">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Request ID" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="Title" HeaderText="Work Title" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                </Columns>
            </asp:GridView>
            <div class="mt-6">
                <asp:Button ID="btnBack" runat="server" Text="Back to Works" OnClick="btnBack_Click" CausesValidation="false" 
                            CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-accent focus:ring-opacity-50" />
            </div>
        </div>
    </form>
</body>
</html>