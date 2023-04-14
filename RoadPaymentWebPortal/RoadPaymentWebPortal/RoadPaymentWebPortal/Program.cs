using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CSSD_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            dbEdit database = new dbEdit();

            //all lines below to do with the database are for inital creation of tables and records ONLY RUN THEM IF YOU KNOW THEY DONT ALREADY EXIST IN THE DATABASE
            //IF YOU ARE UNSURE DELETE THE DATABASE.DB FILE BEFORE RUNNING THE PROGRAM.

            Setup();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void Setup()
        {
            dbEdit database = new dbEdit();
            database.CreateTable();

            //admin account
            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2);

            //highway creation
            database.createHighway("M1", 1);
            database.createHighway("M5", 1);
            database.createHighway("M6", 2);
            database.createHighway("M41", 3);
            database.createHighway("M57", 1);
            database.createHighway("M61", 7);
            database.createHighway("M100", 1);

            //1st user creation 
            database.InsertUser("luke@hotmail.co.uk", "Luke", "Gidley", "Password", 1);
            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");
            database.createBill(testId, "M1", 100, false);
            database.createBill(testId, "M5", 10, false);
            database.createBill(testId, "M6", 15, false);
            database.createBill(testId, "M1", 20, true);

            //2nd user creation
            database.InsertUser("tom@hotmail.co.uk", "Tom", "Hensby", "Password", 1);
            Guid? testId2 = database.getUserIdbyEmail("tom@hotmail.co.uk");
            database.createBill(testId2, "M41", 43753, false);
            database.createBill(testId2, "M57", 1, false);
            database.createBill(testId2, "M61", 145, false);
            database.createBill(testId2, "M100", 22, true);
           
        }
    }





}
