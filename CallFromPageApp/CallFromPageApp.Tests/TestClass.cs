using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallFromPageApp.Droid;
using Com.OneSignal;

namespace CallFromPageApp.Tests
{
    [TestFixture]
    public class TestParseNotificationAdditionalData
    {
        [Test]
        public void TestDateOfNotificationMethod()
        {
            String date = "1481369367"; 
            Guid guid = Guid.NewGuid();
            String landingPage = "meineseite.at";
            Boolean isPhone = true;
            String phoneOrEmail = "06602334234";
            String body = "body";
            String title = "title";

            Dictionary<string, object> additionalData = new Dictionary<string, object>();
            additionalData.Add("when", date);
            additionalData.Add("landingPage", landingPage);
            additionalData.Add("isPhone", isPhone);
            additionalData.Add("phoneOrEmail", phoneOrEmail);

            ContactNotificationItem item = new ContactNotificationItem();
            item.DateOfContact = new DateTime(2016, 12, 10, 12, 29, 27);
            item.Guid = guid;
            item.LandingPage = landingPage;
            item.IsPhone = isPhone;
            item.PhoneOrEmail = phoneOrEmail;
            item.Message = body;
            item.Title = title;


            OSNotificationPayload payload = new OSNotificationPayload();
            payload.additionalData = additionalData;
            payload.notificationID = guid.ToString();
            payload.body = body;
            payload.title = title;

            ContactNotificationItem testItem = Utils.ParseAdditionalData(payload);

            Assert.AreEqual(item.DateOfContact, testItem.DateOfContact);

        }
    }

}
