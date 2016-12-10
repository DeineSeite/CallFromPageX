
using Android.Content;
using Android.App;
using Android.Util;
using System;
using Com.OneSignal;
using System.Collections.Generic;

namespace CallFromPageApp.Droid
{
    public class Utils
    {
        public static string PATH = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public static int GetScreenDensity(Activity activity)
        {
            DisplayMetrics metrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(metrics);
            int pixelDensityIndex = (int)metrics.Density <= 2 ? 1 : 2;
            return pixelDensityIndex;
        }

        public static SqliteService GetDatabaseService()
        {
            try
            {
                return new SqliteService(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), Utils.PATH);
            }
            catch (Exception e)
            {
                Log.Debug("UTILS: ", e.Message);
                return null;
            }
        }

        public static Boolean SaveNotificationIntoDB(OSNotificationPayload notification)
        {
            try
            {
                var conn = GetDatabaseService();
                ContactNotificationItem item = ParseAdditionalData(notification);
                item.Id = conn.getMaxId() + 1;  // next row in db (could be errorprone, thats why GUID as key)
                List<ContactNotificationItem> newNotifications = new List<ContactNotificationItem>();
                newNotifications.Add(item);

                conn.InsertUpdateProductList<ContactNotificationItem>(newNotifications);

                //String info = conn.InsertUpdateProduct<ContactNotificationItem>(item);
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public static ContactNotificationItem ParseAdditionalData(OSNotificationPayload notification)
        {
            ContactNotificationItem item = new ContactNotificationItem();
            item.Guid = new Guid(notification.notificationID);
            item.Message = notification.body;
            item.Title = notification.title;

            Dictionary<string, object> additionalData = notification.additionalData;
            if (additionalData.Count > 0)
            {
               
                    String key = "when";
                    if (additionalData.ContainsKey(key)) 
                    {
                        try
                        {
                            double unixTimeStamp = Convert.ToDouble(additionalData[key].ToString());
                            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            item.DateOfContact = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                        }
                        catch (System.Exception ex) { }
                    }

                    key = "landingPage";
                    if (additionalData.ContainsKey(key))
                        item.LandingPage = additionalData[key].ToString();

                    key = "isPhone";
                    if (additionalData.ContainsKey(key))
                        item.IsPhone = Convert.ToBoolean(additionalData[key].ToString());

                    key = "phoneOrEmail";
                    if (additionalData.ContainsKey(key))
                        item.PhoneOrEmail = additionalData[key].ToString();
            }

            return item;
        }

        public static void InitDB()
        {
            var conn = Utils.GetDatabaseService();

            if (!conn.TableExists<ContactNotificationItem>())
                conn.CreateTables();
        }
    }
}