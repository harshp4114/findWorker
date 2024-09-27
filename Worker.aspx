<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Worker.aspx.cs" Inherits="findWorker2.Worker" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Worker Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Available Works</h1>

            <asp:GridView ID="gvWorks" runat="server" AutoGenerateColumns="False" OnRowCommand="gvWorks_RowCommand">
    <Columns>
        <asp:BoundField DataField="WorkID" HeaderText="ID" />
        <asp:BoundField DataField="ProviderUsername" HeaderText="Provider" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <span style="color:<%# Eval("Status").ToString() == "Open" ? "green" : "red" %>;">
                    <%# Eval("Status") %>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="DateOfWork" HeaderText="Date of Work" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="WorkerUsername" HeaderText="Worker Username" />

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:Button ID="btnApply" runat="server" Text="Apply" CommandName="Apply" CommandArgument='<%# Eval("WorkID") %>' 
                    Enabled='<%# Eval("Status").ToString() == "Open" %>' />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>

            <h2>Your Pending Requests</h2>
<asp:GridView ID="gvPendingRequests" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="RequestID" HeaderText="Request ID" />
        <asp:BoundField DataField="Title" HeaderText="Work Title" />
        <asp:BoundField DataField="Status" HeaderText="Status" />
    </Columns>
</asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <asp:Button ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" />
        </div>
    </form>
</body>
</html>
