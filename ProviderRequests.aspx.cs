using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace findWorker2
{
    public partial class ProviderRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindRequestsGrid();
            }
        }

        protected void BindRequestsGrid()
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

                        // Rebind the grid to reflect changes
                        BindRequestsGrid();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
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
                        // Rebind the grid to reflect changes
                        BindRequestsGrid();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

       

        protected void btnBackToProvider_Click(object sender, EventArgs e)
        {
            Response.Redirect("Provider.aspx");
        }
    }
}
