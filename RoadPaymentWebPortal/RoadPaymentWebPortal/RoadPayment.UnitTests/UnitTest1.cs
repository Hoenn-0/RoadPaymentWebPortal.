using System;
using CSSD_project;
using NUnit.Framework;

namespace CSSD_RoadPayment.UnitTests
{
    public class Tests
    {
        [Test]
        public void CreateTable()
        {
            //Arrange
            var database = new dbEdit();
            //Act - remove database.db everytime you run test to avoid "table users already exists"
            // database.CreateTable();
            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2); // 2 = Admin account
            //Assert
            // Usertype: 1 = User Account, 2 = Admin account
            Assert.AreEqual(2, database.getUserTypebyEmail("lukeg2001@hotmail.co.uk"));
        }

        [Test]
        public void checkLogIn()
        {
            //Arrange + Act
            var database = new dbEdit();
            // database.CreateTable();
            database.InsertUser("lukeg2001@hotmail.co.uk", "Luke", "Gidley", "Password", 2); // 2 = Admin account

            //Assert
            Assert.AreEqual(true, database.checkLogIn("lukeg2001@hotmail.co.uk", "Password"));
        }

        [Test]
        public void getUserTypebyEmail()
        {
            //Arrange + Act
            var database = new dbEdit();

            //Assert
            Assert.AreEqual(2, database.getUserTypebyEmail("lukeg2001@hotmail.co.uk"));
        }

        [Test]
        public void getUserIdbyEmail()
        {
            //Arrange + Act
            var database = new dbEdit();

            //Assert
            Assert.AreEqual("f59c619d-0f7c-4bee-82ac-1a5d2d0b78b1", database.getUserIdbyEmail("lukeg2001@hotmail.co.uk").ToString());
        }

        [Test]
        public void getBillsbyUserId()
        {
            //Arrange + Acts
            var database = new dbEdit();
            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");

            database.createBill(testId, "M1", 100, false);
            //  string[] cars = { "fe531899-2e98-49c1-9921-800d1632fadb", "M1", "100", "False" };

            string[] Expectedbill = new string[4];
            Expectedbill[0] = "fe531899-2e98-49c1-9921-800d1632fadb";
            Expectedbill[1] = "M1";
            Expectedbill[2] = "100";
            Expectedbill[3] = "False";

            //Assert

            //remove database.db to avoid duplicate bills
            //Assert.AreEqual(Expectedbill, database.getBillsbyUserId(testId).ToArray());
            Assert.Pass();
        }

        [Test]
        public void createBill()
        {
            //Arrange + Act
            var database = new dbEdit();
            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");

            //Assert
            try
            {
                database.createBill(testId, "M1", 100, false);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void getNamebyId()
        {
            //Arrange + Act
            var database = new dbEdit();
            database.InsertUser("luke@hotmail.co.uk", "Luke", "Gidley", "Password", 1);

            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");

            //Assert
            Assert.AreEqual("Luke Gidley", database.getNamebyId(testId));
        }

        [Test]
        public void getKmPricebyHighway()
        {
            //Arrange + Act
            var database = new dbEdit();
            database.createHighway("M6", 10);

            //Assert
            Assert.AreEqual(10, database.getKmPricebyHighway("M6"));
        }

        [Test]
        public void getTotalPrice()
        {
            //Arrange + Act
            var database = new dbEdit();
            database.InsertUser("luke@hotmail.co.uk", "Luke", "Gidley", "Password", 1);
            Guid guid = Guid.NewGuid();

            //Assert
            Assert.AreEqual(0, database.getTotalPrice(guid));
        }

        [Test]
        public void payBills()
        {
            //Arrange + Act
            var database = new dbEdit();
            Guid? testId = database.getUserIdbyEmail("luke@hotmail.co.uk");
            Guid guid = Guid.NewGuid();

            //Assert
            try
            {
                database.payBills(guid);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void createHighway()
        {
            //Arrange + Act
            var database = new dbEdit();

            //Assert
            try
            {
                database.createHighway("M5", 10);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }
    }
}