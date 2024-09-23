<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Provider.aspx.cs" Inherits="findWorker2.Provider" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Provider Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Welcome, <%= welcomeUser %>!</h1> 
            <h2>Your Works:</h2>
            <asp:GridView ID="gvWorks" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvWorks_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="WorkID" HeaderText="ID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="DateOfWork" HeaderText="Date of Work" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField HeaderText="Worker Username">
                        <ItemTemplate>
                            <asp:Label ID="lblWorkerUsername" runat="server" Text='<%# Eval("WorkerUsername") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <h2>Add New Work</h2>
            <label for="txtTitle">Title:</label>
            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" 
                ErrorMessage="Title is required." ForeColor="Red"></asp:RequiredFieldValidator>
            <br /><br />

            <label for="txtDescription">Description:</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                ErrorMessage="Description is required." ForeColor="Red"></asp:RequiredFieldValidator>
            <br /><br />

            <label for="txtDateOfWork">Date of Work:</label>
            <asp:TextBox ID="txtDateOfWork" runat="server" TextMode="Date"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDateOfWork" runat="server" ControlToValidate="txtDateOfWork" 
                ErrorMessage="Date of work is required." ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvDateOfWork" runat="server" ControlToValidate="txtDateOfWork" 
                Type="Date" Operator="DataTypeCheck" 
                ErrorMessage="Please enter a valid date." ForeColor="Red"></asp:CompareValidator>

            <asp:CustomValidator ID="cvFutureDate" runat="server" ControlToValidate="txtDateOfWork"
                ErrorMessage="Date of work cannot be earlier than today's date." 
                OnServerValidate="ValidateFutureDate" ForeColor="Red"></asp:CustomValidator>
            <br /><br />

            <asp:Button ID="btnAddWork" runat="server" Text="Add Work" OnClick="btnAddWork_Click" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <asp:Button ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" CausesValidation="false" />
        </div>
    </form>
</body>
</html>
