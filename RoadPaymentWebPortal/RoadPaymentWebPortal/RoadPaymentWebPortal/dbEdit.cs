using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace CSSD_project
{
    public class dbEdit
    {


 
        // Create a new database connection:
        static SQLiteConnection conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");
  
  
        public void CreateTable()
        {
            conn.Open();
            SQLiteCommand sqlite_cmd;
            string CreateUserTable = "CREATE TABLE users(UserId TEXT PRIMARY KEY, Email TEXT UNIQUE NOT NULL, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, Password TEXT NOT NULL, UserType INT NOT NULL)";
            string CreatesBillsTable = "CREATE TABLE bills(UserId TEXT NOT NULL, BillId TEXT NOT NULL, HighwayName TEXT NOT NULL, Distance INT NOT NULL, Price INT NOT NULL, Paid bool NOT NULL, CONSTRAINT fk_users FOREIGN KEY (UserId) References users(UserId), CONSTRAINT fk_highway FOREIGN KEY (HighwayName) References highways(HighwayName), CONSTRAINT UserBillId PRIMARY KEY(UserId, BillId))";
            string HighwayTable = "CREATE TABLE highways(HighwayName TEXT PRIMARY KEY, PricePerKm INT NOT NULL)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = CreateUserTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = CreatesBillsTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = HighwayTable;
            sqlite_cmd.ExecuteNonQuery();
            conn.Close();
        }

        //use to register an account
        public bool InsertUser(string Email, string FirstName, string LastName, string Password, int UserType)
        {
            conn.Open();
            string userId = Guid.NewGuid().ToString();
            string InsertUser = "INSERT INTO users(UserId, Email, FirstName, LastName, Password, UserType) VALUES(@UserId, @Email, @FirstName, @LastName, @Password, @UserType)";

            using (SQLiteCommand cmd = new SQLiteCommand(InsertUser, conn))
            {
                try
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    //if false is returned, the email already exists. No data will be entered into the table.
                    conn.Close();
                    return false;
                }             
            }
            conn.Close();
            return true;
        }


        public bool checkLogIn(string email, string password)
        {

            conn.Open();

            string logIn = "SELECT Email, Password FROM users WHERE Email=@Email AND Password=@Password";

            using (SQLiteCommand cmd = new SQLiteCommand(logIn, conn))
            {

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //if something gets read, then the login exists, otherwise the account doesnt exist with the given username and password.
                    while (reader.Read())
                    {
                        conn.Close();
                        return true;
                    }
                }
            }

            conn.Close();
            return false;
        }

        //gets what type of user the user is, should only be called AFTER checkLogIn() has been used.
        public int getUserTypebyEmail(string email)
        {
            conn.Open();
            int userType = 0;

            string getUserType = "SELECT UserType From users WHERE Email=@Email";
            using (SQLiteCommand cmd = new SQLiteCommand(getUserType, conn))
            {

                cmd.Parameters.AddWithValue("@Email", email);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //reads the userType from the db
                    while (reader.Read())
                    {
                        userType = reader.GetInt32(0);
                    }
                }
            }
            conn.Close();
            return userType;
        }


        public Guid? getUserIdbyEmail(string email)
        {
            conn.Open();
            string getUserType = "SELECT UserId From users WHERE Email=@Email";
            using (SQLiteCommand cmd = new SQLiteCommand(getUserType, conn))
            {

                cmd.Parameters.AddWithValue("@Email", email);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //reads the userType from the db
                    while (reader.Read())
                    {
                        var id = reader[0];
                        conn.Close();
                        return new Guid(id.ToString()!);
                    }
                }
            }
            conn.Close();
            return null;
        }

        public List<string[]> getBillsbyUserId(Guid? id)
        {
            conn.Open();
            string UserId = id.ToString();
            string getBills = "SELECT HighwayName, Distance, Price, Paid FROM bills WHERE bills.UserId=@UserId";
            List<string[]> bills = new List<string[]>();
            using (SQLiteCommand cmd = new SQLiteCommand(getBills, conn))
            { 
                cmd.Parameters.AddWithValue("@UserId", UserId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //reads the userType from the db
                    while (reader.Read())
                    {
                        string[] bill = new string[4];
                        bill[0] = reader[0].ToString();
                        bill[1] = reader[1].ToString();
                        bill[2] = reader[2].ToString();
                        bill[3] = reader[3].ToString();

                        bills.Add(bill);
                    }
                    conn.Close();
                    return bills;
                }               
            }
        }

        public void createBill(Guid? User, string Highway, int Distance, bool Paid)
        {
            conn.Open();
            string insertBill = "INSERT INTO bills(UserId, BillId, HighwayName, Distance, Price, Paid) VALUES(@UserId, @BillId, @HighwayName, @Distance, @Price, @Paid)";
            string BillId = Guid.NewGuid().ToString();
            string UserId = User.ToString();

            int PricePerKm = getKmPricebyHighway(Highway);
            int Price = Distance * PricePerKm;

            using (SQLiteCommand cmd = new SQLiteCommand(insertBill, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@BillId", BillId);
                cmd.Parameters.AddWithValue("@HighwayName", Highway);
                cmd.Parameters.AddWithValue("@Distance", Distance);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@Paid", Paid);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public string getNamebyId(Guid? id)
        {
            conn.Open();
            string getUserType = "SELECT FirstName, LastName From users WHERE UserId=@UserId";
            using (SQLiteCommand cmd = new SQLiteCommand(getUserType, conn))
            {

                cmd.Parameters.AddWithValue("@UserId", id.ToString());
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //reads the userType from the db
                    while (reader.Read())
                    {
                        string Name = reader[0].ToString() + " " + reader[1].ToString();
                        conn.Close();
                        return Name;
                    }
                }
            }
            conn.Close();
            return null;
        }

        public int getKmPricebyHighway(string HighwayName)
        {
            //as this function is called from another query sometimes, we need to check if its already open, if it is we dont want to close it when THIS function ends.
            bool AlreadyOpen = false;
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                AlreadyOpen = true;
            }
            

            string getPricePerKm = "SELECT PricePerKm From highways WHERE HighwayName=@HighwayName";
            using (SQLiteCommand cmd = new SQLiteCommand(getPricePerKm, conn))
            {

                cmd.Parameters.AddWithValue("@HighwayName", HighwayName);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //reads the userType from the db
                    while (reader.Read())
                    {
                        var price = reader[0];
                        if(!AlreadyOpen)
                            conn.Close();
                        return (int)price;
                    }
                }
            }
            if (!AlreadyOpen)
                conn.Close();
            return -1;
        }

        public int getTotalPrice(Guid id)
        {
            List<string[]> bills = getBillsbyUserId(id);
            int total = 0;

            foreach(string[] bill in bills)
            {
                if (bill[3] == "False")
                {
                    total += int.Parse(bill[2]);
                }
                
            }

            return total;
        }

        public void payBills(Guid id)
        {
            conn.Open();
            string updateBills = "UPDATE bills SET Paid=1 WHERE UserId=@UserId AND Paid=0";
            using (SQLiteCommand cmd = new SQLiteCommand(updateBills, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", id.ToString());
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void createHighway(string highway, int pricePerKm)
        {
            conn.Open();
            string createHighway = "INSERT INTO highways(HighwayName, PricePerKm) VALUES(@HighwayName, @PricePerKm)";
            using (SQLiteCommand cmd = new SQLiteCommand(createHighway, conn))
            {
                cmd.Parameters.AddWithValue("@HighwayName", highway);
                cmd.Parameters.AddWithValue("@PricePerKm", pricePerKm);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
