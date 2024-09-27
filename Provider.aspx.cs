using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace findWorker2
{
    public partial class Provider : System.Web.UI.Page
    {
        public string welcomeUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadUserName(); 
                BindWorksGrid(); 
                BindRequestsGrid();
            }
        }

        protected void ValidateFutureDate(object source, ServerValidateEventArgs args)
        {
            DateTime selectedDate;
            if (DateTime.TryParse(txtDateOfWork.Text, out selectedDate))
            {
                if (selectedDate >= DateTime.Today)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = false; 
            }
        }

        private void LoadUserName()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT fullName FROM Users WHERE username=@Username AND password=@password AND userType=@type";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", Session["Username"]);
                cmd.Parameters.AddWithValue("@password", Session["Password"]);
                cmd.Parameters.AddWithValue("@type", Session["UserType"]);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        welcomeUser = result.ToString(); 
                    }
                    else
                    {
                        welcomeUser = "User"; 
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnAddWork_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                string title = txtTitle.Text;
                string description = txtDescription.Text;
                string providerUsername = Session["Username"].ToString();
                string workerUsername = "Not applied by anyone yet"; 
                string status = "Open"; 
                DateTime dateOfWork;

                if (!DateTime.TryParse(txtDateOfWork.Text, out dateOfWork))
                {
                    lblMessage.Text = "Please enter a valid date.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

                string query = "INSERT INTO Works (ProviderUsername, WorkerUsername, Title, Description, Status, DateOfWork) " +
                               "VALUES (@ProviderUsername, @WorkerUsername, @Title, @Description, @Status, @DateOfWork)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProviderUsername", providerUsername);
                    cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@DateOfWork", dateOfWork);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            lblMessage.Text = "Work added successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            ClearFields();
                            BindWorksGrid(); 
                        }
                        else
                        {
                            lblMessage.Text = "Failed to add work.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void gvWorks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblWorkerUsername = (Label)e.Row.FindControl("lblWorkerUsername");
                if (lblWorkerUsername != null)
                {
                    if (lblWorkerUsername.Text == "Not applied by anyone yet")
                    {
                        lblWorkerUsername.ForeColor = System.Drawing.Color.Red; 
                    }
                    else
                    {
                        lblWorkerUsername.ForeColor = System.Drawing.Color.Black; 
                    }
                }
            }
        }


        protected void gvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

            if (e.CommandName == "Accept")
            {
                int requestID = Convert.ToInt32(e.CommandArgument);

                string selectQuery = "SELECT WorkID, WorkerUsername FROM Requests WHERE RequestID = @RequestID";
                int workID = 0;
                string workerUsername = "";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                    selectCmd.Parameters.AddWithValue("@RequestID", requestID);
                    con.Open();
                    SqlDataReader reader = selectCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        workID = Convert.ToInt32(reader["WorkID"]);
                        workerUsername = reader["WorkerUsername"].ToString();
                    }
                    reader.Close();
                }

                string updateRequestQuery = "UPDATE Requests SET Status = 'Accepted' WHERE RequestID = @RequestID";
                string updateWorkQuery = "UPDATE Works SET WorkerUsername = @WorkerUsername, Status = 'Assigned' WHERE WorkID = @WorkID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        SqlCommand updateRequestCmd = new SqlCommand(updateRequestQuery, con, transaction);
                        updateRequestCmd.Parameters.AddWithValue("@RequestID", requestID);
                        updateRequestCmd.ExecuteNonQuery();

                        SqlCommand updateWorkCmd = new SqlCommand(updateWorkQuery, con, transaction);
                        updateWorkCmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                        updateWorkCmd.Parameters.AddWithValue("@WorkID", workID);
                        updateWorkCmd.ExecuteNonQuery();

                        transaction.Commit();

                        lblMessage.Text = "Request accepted successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        BindWorksGrid(); 
                        BindRequestsGrid();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "An error occurred: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else if (e.CommandName == "Reject") 
            {
                int requestID = Convert.ToInt32(e.CommandArgument);

                string updateRequestQuery = "UPDATE Requests SET Status = 'Rejected' WHERE RequestID = @RequestID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand updateRequestCmd = new SqlCommand(updateRequestQuery, con);
                    updateRequestCmd.Parameters.AddWithValue("@RequestID", requestID);

                    try
                    {
                        con.Open();
                        updateRequestCmd.ExecuteNonQuery();
                        lblMessage.Text = "Request rejected successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        BindRequestsGrid(); 
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }



        private void BindRequestsGrid()
        {
            string providerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT r.RequestID, r.WorkID, r.WorkerUsername, w.Title, w.Description, r.Status " +
                           "FROM Requests r INNER JOIN Works w ON r.WorkID = w.WorkID " +
                           "WHERE w.ProviderUsername = @ProviderUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProviderUsername", providerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvRequests.DataSource = reader;
                gvRequests.DataBind();
            }
        }





        private void BindWorksGrid()
        {
            string providerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT WorkID, Title, Description, Status, DateOfWork, WorkerUsername FROM Works WHERE ProviderUsername = @ProviderUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProviderUsername", providerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvWorks.DataSource = reader;
                gvWorks.DataBind();
            }
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtDateOfWork.Text = ""; 
        }
    }
}
