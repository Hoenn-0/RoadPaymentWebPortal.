using System;
using CSSD_project;
using NUnit.Framework;

namespace CSSD_RoadPayment.UnitTests
{
    public class IntegrationTests
    {
        [Test]
        public void UserRegistrationBy_Register_ReturnsRegisterPage()
        {
            //Arrange
            var database = new dbEdit();
            //remove database.db everytime you run test to avoid "table users already exists"
            var url = "https://localhost:44333/";
            //Act
            database.CreateTable();
            database.InsertUser("tom@hotmail.co.uk", "Tom", "Hensby", "Password", 1);
            Guid? testId2 = database.getUserIdbyEmail("tom@hotmail.co.uk");
            database.createBill(testId2, "M41", 43753, false);
            database.createBill(testId2, "M57", 1, false);
            database.createBill(testId2, "M61", 145, false);
            database.createBill(testId2, "M100", 22, true);

            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //Assert
            Assert.AreEqual("text/cshtml; charset=utf-8; C:\Users\Hoenn\Source\Repos\CSSD-project\CSSD-project\Pages\Register.cshtml;",
                response.Content.Headers.ContentType.ToString());
        }


        [Test]
        public void BillingGatewayBy_PayButton_ReturnsPayPal()
        {
            //Arrange
            var database = new dbEdit();
            var url = "https://localhost:44333/";
            var response = await Client.GetAsync(url);

            Guid? testId = database.getUserIdbyEmail("lukeg2001@hotmail.co.uk");

            //Act 

            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2); // 2 = Admin account
            database.createBill(testId, "M1", 100, false);
            database.payBills(testId);

            //Assert
            Assert.AreEqual("text/cshtml; charset=utf-8; C:\Users\Hoenn\Source\Repos\CSSD-project\CSSD-project\Pages\Payment.cshtml;",
               response.Content.Headers.ContentType.ToString());
        }

        [Test]
        public void ShowBillsBy_Dashboard_ReturnsBills()
        {
            //Arrange
            var database = new dbEdit();
            Guid? testId = database.getUserIdbyEmail("lukeg2001@hotmail.co.uk");

            //Act - remove database.db everytime you run test to avoid "table users already exists"
            database.CreateTable();

            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 1); // 2 = Admin account
            database.createBill(testId, "M1", 100, false);
            database.getBillsbyUserId(testId);
            database.getTotalPrice(testId);
            //Assert

            //Assert.AreEqual(Expectedbill, database.getBillsbyUserId(testId).ToArray());
            Assert.AreEqual(2, database.getUserTypebyEmail("lukeg2001@hotmail.co.uk"));
        }

        [Test]
        public void ShowBillsBy_SearchFunction_UserTable()
        {
            //Arrange + Act
            var database = new dbEdit();
            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");

            
            //Assert
            try
            {
                database.CreateTable();
                database.InsertUser("luke@hotmail", "Luke", "Gidley", "Password", 1);
                database.createBill(testId, "M1", 100, false);
                database.getBillsbyUserId(testId);
                database.getTotalPrice(testId); Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }

        }

        [Test]
        public void LogoutBy_User_ReturnsLogIn()
        {
            //Arrange + Act
            var database = new dbEdit();
            var url = "https://localhost:44333/";
            var response = await Client.GetAsync(url);
             
            //Assert

            Assert.AreEqual("text/cshtml; charset=utf-8; C:\Users\Hoenn\Source\Repos\CSSD-project\CSSD-project\Pages\Index.cshtml;",
               response.Content.Headers.ContentType.ToString());
        }

        [Test]
        public void RedirectPagesBy_Buttons_ReturnsPage()
        {
            //Arrange
            var database = new dbEdit();
            var url = "https://localhost:44333/";
            var response = await Client.GetAsync(url);
            //Act - remove database.db everytime you run test to avoid "table users already exists"
            // database.CreateTable();
            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2); // 2 = Admin account
            
            //Assert
            Assert.AreEqual("text/cshtml; charset=utf-8; C:\Users\Hoenn\Source\Repos\CSSD-project\CSSD-project\Pages\*.cshtml;",
                response.Content.Headers.ContentType.ToString());
        }

        [Test]
        public void AuthenticationBy_CorrectForms_ReturnsUser()
        {
            //Arrange
            var database = new dbEdit();
            //Act 
            // database.CreateTable();
            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2); // 2 = Admin account
           
            //Assert
            Assert.AreEqual(true, database.checkLogIn("lukeg2001@hotmail.co.uk", "Password"));

        }

    }
}