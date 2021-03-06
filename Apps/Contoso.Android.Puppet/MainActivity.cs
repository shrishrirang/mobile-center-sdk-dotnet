﻿using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Crashes;
using System.Collections.Generic;
using System.Threading;

namespace Contoso.Android.Puppet
{
    [Activity(Label = "SXPuppet", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string LogTag = "MobileCenterXamarinPuppet";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate
            {
                // Crash
                button.Text = button.Text.Substring(42);
            };

            // Mobile Center integration
            MobileCenterLog.Assert(LogTag, "MobileCenter.LogLevel=" + MobileCenter.LogLevel);
            MobileCenter.LogLevel = LogLevel.Verbose;
            MobileCenterLog.Info(LogTag, "MobileCenter.LogLevel=" + MobileCenter.LogLevel);
            MobileCenter.SetServerUrl("https://in-integration.dev.avalanch.es");
            MobileCenter.Start("7f222d3c-0f5e-421b-93e7-f862c462e07e", typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("myEvent", new Dictionary<string, string> { { "someKey", "someValue" } });
            MobileCenterLog.Info(LogTag, "MobileCenter.InstallId=" + MobileCenter.InstallId);
            MobileCenterLog.Info(LogTag, "MobileCenter.HasCrashedInLastSession=" + Crashes.HasCrashedInLastSession);
            Crashes.GetLastSessionCrashReportAsync().ContinueWith(report =>
            {
                MobileCenterLog.Info(LogTag, "MobileCenter.LastSessionCrashReport=" + report.Result?.AndroidDetails?.Throwable);
            });
        }

        protected override void OnDestroy()
        {
            // This will crash with super not called exception, pure java exception that we want to test
        }
    }
}