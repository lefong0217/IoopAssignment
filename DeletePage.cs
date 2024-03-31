using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment
{
    public partial class DeletePage : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\User\\Documents\\APU Y1SEM2\\Introduction To Object Oriented Programming\\Assignment\\Database.mdf\";Integrated Security=True";
        public DeletePage()
        {
            InitializeComponent();

        }


        private void RefreshUserInfoDataGridView()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [dbo].[UserInfo]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataUserForDelete.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 检查输入的 userID 是否为空
            if (string.IsNullOrWhiteSpace(txtDeleteUser.Text))
            {
                MessageBox.Show("Please enter the userID you want to delete!");
                return;
            }

            // 获取用户输入的 userID
            string userIdToDelete = txtDeleteUser.Text;

            // 使用参数化查询来避免 SQL 注入攻击
            string query = "DELETE FROM UserInfo WHERE userID = @userid";
            string query1 = "SELECT role FROM UserInfo WHERE userID = @userid";
            string query2 = "DELETE FROM CoachSalary WHERE userID = @userid";
            string query3 = "DELETE FROM CoachMember WHERE userID = @userid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command1 = new SqlCommand(query1, connection))
                {
                    command1.Parameters.AddWithValue("userid", userIdToDelete);

                    string role = command1.ExecuteScalar().ToString();

                    if (role == "Coach")
                    {
                        SqlCommand command2 = new SqlCommand(query2, connection);
                        command2.Parameters.AddWithValue("userid", userIdToDelete);
                        command2.ExecuteNonQuery();
                        SqlCommand command3 = new SqlCommand(query3, connection);
                        command3.Parameters.AddWithValue("userid", userIdToDelete);
                        command3.ExecuteNonQuery();
                    }
                    if (role == "Member")
                    {
                        SqlCommand command4 = new SqlCommand(query3, connection);
                        command4.Parameters.AddWithValue("userid", userIdToDelete);
                        command4.ExecuteNonQuery();
                    }
                }
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userid", userIdToDelete);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Successfully deleted UserID for {userIdToDelete}!");
                    }
                    else
                    {
                        MessageBox.Show($"No UserID {userIdToDelete} is found!");
                    }
                }
                RefreshUserInfoDataGridView();
            }

                // 清空输入框
                txtDeleteUser.Text = "";
        }

        private void DeletePage_Load(object sender, EventArgs e)
        {
            RefreshUserInfoDataGridView();
        }
    }
}
