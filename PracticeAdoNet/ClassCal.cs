using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAdoNet
{
    public class ClassCal
    {
        //private string connectionString = @"Data Source=USEDEV27038\MSSQLSERVER1;Database=ExploreCalifornia;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private string connectionString = @"Server=USEDEV27038\MSSQLSERVER1;Database=ExploreCalifornia;User Id=ciqdev\m_boyini;Password=Tracker@999;Trusted_Connection=True;";

        //send table as input into astored proc
        public void ExecuteStoredProc()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("sp_AllPosts", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parms = new SqlParameter();
            parms.ParameterName = "@postsExport";
            parms.Value = GetPosts();

            cmd.Parameters.Add(parms);

            cmd.Connection.Open();
            var output = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private DataTable GetPosts()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Key");
            dt.Columns.Add("Title");
            dt.Columns.Add("Author");
            dt.Columns.Add("Section");
            dt.Columns.Add("TimeOfPost");
            dt.Columns.Add("Year");
            dt.Columns.Add("Month");

            dt.Rows.Add(1,"test-my-block", "Testblock1", "mboin", "This is the section place", DateTime.Now, 1993, 2);

            dt.Rows.Add(2,"test-my-block", "Testblock2", "mboin", "This is the section place", DateTime.Now, 1994, 2);
            dt.Rows.Add(3,"test-my-block", "Testblock3", "mboin", "This is the section place", DateTime.Now, 1995, 2);
            dt.Rows.Add(4,"test-my-block", "Testblock4", "mboin", "This is the section place", DateTime.Now, 1996, 2);
            dt.Rows.Add(5,"test-my-block", "Testblock5", "mboin", "This is the section place", DateTime.Now, 1997, 2);
            dt.Rows.Add(6,"test-my-block", "Testblock6", "mboin", "This is the section place", DateTime.Now, 1998, 2);

            return dt;
        }

        public PostModel ReadRecords(int id)
        {
            SqlConnection comm = new SqlConnection(connectionString);
            string sql = @"Select * from posts where id="+id.ToString();
            SqlCommand cmd = new SqlCommand(sql, comm);
            PostModel post = new PostModel();

            cmd.Connection.Open();
            IDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                post.Id = Convert.ToInt32(reader["Id"]);
                post.KeyVal = Convert.ToString(reader["KeyVal"]);
                post.Section = Convert.ToString(reader["Section"]);
                post.Author = Convert.ToString(reader["Author"]);
                post.Year = Convert.ToInt32(Convert.ToString(reader["Year"])==string.Empty?-1: reader["Year"]);
                post.Month = Convert.ToInt32(Convert.ToString(reader["Month"]) == string.Empty ? -1 : reader["Month"]);
            }
            return post;
        }

        public int DeleteRecords(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = @"Delete from posts where id=" + id.ToString();
            SqlCommand cmd = new SqlCommand(sql,conn);

            cmd.Connection.Open();
            int output =cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return output;
        }

        public int UpdateRecords(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = @"update posts set year=2003 where id=" + id.ToString();
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Connection.Open();
            int output = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return output;
        }

        public int CreateRecords(PostModel post)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = @"Insert into posts(keyval,title,author,section,timeofpost,year,month) values(@keyVal,@title,@author,@section,@yearOfPost,@year,@month)";
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@KeyVal", post.KeyVal);
            cmd.Parameters.Add("@title", post.Title);
            cmd.Parameters.Add("@author", post.Author);
            cmd.Parameters.Add("@section", post.Section);
            cmd.Parameters.Add("@yearOfPost", post.YearOfPost);
            cmd.Parameters.Add("@year", post.Year);
            cmd.Parameters.Add("@month", post.Month);

            cmd.Connection.Open();
            int output = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return output;
        }

        //disconnected architecture
        public void ReturnStoredProc()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                List<PostModel> posts = new List<PostModel>();
                using (var cmd =new SqlDataAdapter())
                {
                    cmd.SelectCommand = new SqlCommand("sp_GetAllPostsInTableFormat", conn);
                    cmd.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataTable da = new DataTable();
                    cmd.Fill(da);

                    foreach(DataRow dr in da.Rows)
                    {
                        PostModel post = new PostModel();
                        post.Id = Convert.ToInt32(dr["id"]);
                        post.Author = Convert.ToString(dr["Author"]);
                        post.KeyVal = Convert.ToString(dr["KeyVal"]);
                        post.Section = Convert.ToString(dr["Section"]);
                        post.YearOfPost = Convert.ToString(dr["TimeOfPost"]);
                        post.Month = Convert.ToInt32(dr["month"]);
                        post.Year = Convert.ToInt32(dr["year"]);
                        posts.Add(post);
                    }
                }
            }
        }

        public string ReturnScalrValFunction(int postID)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlDataAdapter())
                {
                    cmd.SelectCommand = new SqlCommand();
                    cmd.SelectCommand.Connection = conn;
                    cmd.SelectCommand.CommandText = $"select [dbo].[GetPostTitle]({postID})";

                    cmd.SelectCommand.Connection.Open();

                    var value =cmd.SelectCommand.ExecuteScalar();
                    cmd.SelectCommand.Connection.Close();
                    if(value!=null)
                    {
                        return value.ToString();
                    }
                }
            }
            return "";
        }

        public bool ReturnTableFromFunc(int postId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlDataAdapter())
                {
                    cmd.SelectCommand = new SqlCommand($"select * from [dbo].[GetPostPerId]({postId})", conn);
                    cmd.SelectCommand.Connection.Open();
                    IDataReader reader= cmd.SelectCommand.ExecuteReader();
                    cmd.SelectCommand.Connection.Close();

                    if(reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}