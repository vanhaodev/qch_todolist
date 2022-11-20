using education_project_backend.Models.SQL;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace education_project_backend.Modules
{
    public enum IDType
    {
        UserID, WorkspaceID
    }
    public class GenerateUniqueID
    {
        public static string GenerateID(IDType idType)
        {
            //26 chữ cái hoa và 10 số
            //36^10
            Random random = new Random();
            int loop = 0;
            char[] id = new char[11];
            id[0] = (idType == IDType.UserID ? 'U' : 'W'); //set tiền tố U (user)

            for (int i = 1; i < id.Length; i++) //i = 1 vì bỏ qua chữ "u"
            {
                //0 = chữ; 1 = số
                int index = random.Next(0, 2) == 0 ? random.Next(65, 91) : random.Next(48, 58);
                id[i] = (char)(index);
            }

            while (IDCheck(new string(id), idType))
            {
                loop++;
                for (int i = 1; i < id.Length; i++) //i = 1 vì bỏ qua chữ "u"
                {
                    //0 = chữ; 1 = số
                    int index = random.Next(0, 2) == 0 ? random.Next(65, 91) : random.Next(48, 58);
                    id[i] = (char)(index);
                }
            }

            return new string(id);
        }
        static bool IDCheck(string value, IDType idType)
        {
            SqlConnection con = new SqlConnection(ConnectStrings.main);
            SqlCommand cmd = new SqlCommand($"select uid from {(idType == IDType.UserID ? "UserAccount where uid" : "Workspace where WorkspaceID")} = '{value}'", con);

            bool contains = false;
            try
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["uid"].ToString() == value)
                        {
                            contains = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("================UIDIsContains====================" + ex);
            }
            finally
            {
                con.Close();
            }

            return contains;
        }
    }
}

