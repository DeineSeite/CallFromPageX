using Android.App;
using Android.Widget;
using Android.OS;
using Com.OneSignal;
using System.Collections.Generic;
using CallFromPageApp.Droid;

namespace CallFromPageApp.Droid
{
	[Activity (Label = "CallFromPage", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
        ListView _listView;
        CardViewAdapter _cardViewAdapter;
        
        protected override void OnCreate (Bundle savedInstanceState)
		{
            Utils.InitDB();
			base.OnCreate(savedInstanceState);
            InitOneSignalAndDelegates();
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
            _listView = FindViewById<ListView>(Resource.Id.listview);
            _cardViewAdapter = new CardViewAdapter(this);
            _listView.Adapter = _cardViewAdapter;
        }

        private void InitOneSignalAndDelegates()
        {
            // Notification Received Delegate
            OneSignal.NotificationReceived exampleNotificationReceivedDelegate = delegate (OSNotification notification)
            {
                try
                {
                    System.Console.WriteLine("OneSignal Notification Received:\nMessage: {0}", notification.payload.body);

                    Utils.SaveNotificationIntoDB(notification.payload);
                    RunOnUiThread(() => _cardViewAdapter.Add());

                    Dictionary<string, object> additionalData = notification.payload.additionalData;

                    if (additionalData.Count > 0)
                        System.Console.WriteLine("additionalData: {0}", additionalData);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            };

            // Notification Opened Delegate
            OneSignal.NotificationOpened exampleNotificationOpenedDelegate = delegate (OSNotificationOpenedResult result)
            {
                try
                {
                    System.Console.WriteLine("OneSignal Notification opened:\nMessage: {0}", result.notification.payload.body);
                    Dictionary<string, object> additionalData = result.notification.payload.additionalData;
                    if (additionalData.Count > 0)
                        System.Console.WriteLine("additionalData: {0}", additionalData);


                    List<Dictionary<string, object>> actionButtons = result.notification.payload.actionButtons;
                    if (actionButtons.Count > 0)
                        System.Console.WriteLine("actionButtons: {0}", actionButtons);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            };

            // Initialize OneSignal
            //OneSignal.StartInit("4ba9ec31-b65a-4f5f-b210-a5077a245b3d", "703322744261")
            OneSignal.StartInit("58a9c9c8-8e59-4e31-b07f-ef32028a9688", "485609922492")
                        .HandleNotificationReceived(exampleNotificationReceivedDelegate)
                     .HandleNotificationOpened(exampleNotificationOpenedDelegate)
                     .InFocusDisplaying(OneSignal.OSInFocusDisplayOption.Notification)
                     .Settings(new Dictionary<string, bool> { { OneSignal.kOSSettingsKeyAutoPrompt, true }, { OneSignal.kOSSettingsKeyInAppLaunchURL, false } })
                     .EndInit();

            OneSignal.IdsAvailable((playerID, pushToken) =>
            {
                try
                {
                    System.Console.WriteLine("Player ID: " + playerID);
                    if (pushToken != null)
                        System.Console.WriteLine("Push Token: " + pushToken);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            });
        }

	}
}


