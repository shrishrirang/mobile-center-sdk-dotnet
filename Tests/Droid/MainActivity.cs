﻿using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Azure.Mobile;
using Com.Microsoft.Azure.Mobile.Analytics.Channel;
using Com.Microsoft.Azure.Mobile.Ingestion.Models;
using Com.Microsoft.Azure.Mobile.Analytics.Ingestion.Models;
using Com.Microsoft.Azure.Mobile.Analytics;
using System.Collections.Generic;

namespace Contoso.Forms.Test.Droid
{
    [Activity(Label = "Contoso.Forms.Test.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            MobileCenter.LogLevel = LogLevel.Verbose;
            AndroidAnalytics.SetListener(new AndroidAnalyticsListener());

            MobileCenter.Configure("cc684d08-3240-4eb7-a748-e7ddd846a8b1");

            LoadApplication(new App());
        }
    }

    public class AndroidAnalyticsListener : Java.Lang.Object, IAnalyticsListener
    {
        public void OnSendingFailed(ILog log, Java.Lang.Exception e)
        {
            EventSharer.InvokeFailedToSendEvent(LogToEventData(log));
        }

        public void OnSendingSucceeded(ILog log)
        {
            EventSharer.InvokeSentEvent(LogToEventData(log));
        }

        public void OnBeforeSending(ILog log)
        {
            EventSharer.InvokeSendingEvent(LogToEventData(log));
        }

        private EventData LogToEventData(ILog log)
        {
            EventLog eventlog = log as EventLog;
            EventData data = new EventData();
            if (eventlog != null)
            {
                data.Name = eventlog.Name;
                data.Properties = new Dictionary<string, string>();
                if (eventlog.Properties != null)
                {
                    foreach (string key in eventlog.Properties.Keys)
                    {
                        data.Properties.Add(key, eventlog.Properties[key]);
                    }
                }
            }
            return data;
        }
    }
}
