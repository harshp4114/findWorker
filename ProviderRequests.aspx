<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProviderRequests.aspx.cs" Inherits="findWorker2.ProviderRequests" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Provider Worker Requests</title>
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
            <h1 class="text-3xl font-bold text-blue-accent text-center mb-6">Worker Requests for Your Works</h1>
            <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="False" OnRowCommand="gvRequests_RowCommand" 
                          CssClass="w-full table-auto border-collapse text-left text-gray-300">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Request ID" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="Title" HeaderText="Work Title" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="WorkerUsername" HeaderText="Worker Username" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-CssClass="border border-gray-600 p-2" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2" />
                    <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="bg-dark-secondary text-gray-400 p-2">
                        <ItemTemplate>
                            <asp:Button ID="btnAccept" runat="server" CausesValidation="false" Text="Accept" CommandName="Accept" CommandArgument='<%# Eval("RequestID") %>' 
                                Enabled='<%# Eval("Status").ToString() == "Pending" %>'
                                CssClass='<%# Eval("Status").ToString() == "Pending" ? "bg-green-600 hover:bg-green-700 text-white font-bold py-1 px-2 rounded-md mx-2 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-opacity-50" : "bg-gray-400 text-gray-700 font-bold py-1 px-2 rounded-md mx-2 cursor-not-allowed" %>' />
                            <asp:Button ID="btnReject" runat="server" CausesValidation="false" Text="Reject" CommandName="Reject" CommandArgument='<%# Eval("RequestID") %>' 
                                Enabled='<%# Eval("Status").ToString() == "Pending" %>'
                                CssClass='<%# Eval("Status").ToString() == "Pending" ? "bg-red-600 hover:bg-red-700 text-white font-bold py-1 px-2 rounded-md focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-opacity-50" : "bg-gray-400 text-gray-700 font-bold py-1 px-2 rounded-md cursor-not-allowed" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="mt-6">
                <asp:Button ID="btnBackToProvider" runat="server" Text="Back to Dashboard" OnClick="btnBackToProvider_Click" 
                            CssClass="bg-blue-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-accent focus:ring-opacity-50" />
            </div>
        </div>
    </form>
</body>
</html>