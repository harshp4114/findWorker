using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace findWorker2
{
    public partial class Worker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindWorksGrid(); 
                BindPendingRequestsGrid();
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        private void BindWorksGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT WorkID, ProviderUsername, Title, Description, Status, DateOfWork, WorkerUsername FROM Works";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvWorks.DataSource = reader;
                gvWorks.DataBind();
            }
        }

        private void BindPendingRequestsGrid()
        {
            string workerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT r.RequestID, w.Title, r.Status FROM Requests r INNER JOIN Works w ON r.WorkID = w.WorkID WHERE r.WorkerUsername = @WorkerUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvPendingRequests.DataSource = reader;
                gvPendingRequests.DataBind();
            }
        }

        protected void gvWorks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Apply")
            {
                int workID = Convert.ToInt32(e.CommandArgument);
                string workerUsername = Session["Username"].ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
                string checkQuery = "SELECT COUNT(*) FROM Requests WHERE WorkID = @WorkID AND WorkerUsername = @WorkerUsername";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@WorkID", workID);
                    checkCmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                    con.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMessage.Text = "You have already applied for this work.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                string insertQuery = "INSERT INTO Requests (WorkID, WorkerUsername) VALUES (@WorkID, @WorkerUsername)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@WorkID", workID);
                    cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text = "Request sent successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            BindPendingRequestsGrid(); 
                        }
                        else
                        {
                            lblMessage.Text = "Failed to send request.";
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


        


    }
}
