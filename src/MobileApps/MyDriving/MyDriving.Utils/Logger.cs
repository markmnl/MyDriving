// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.

using System;
using System.Collections.Generic;
using MyDriving.Utils.Interfaces;
using System.Collections;
using System.Threading.Tasks;

namespace MyDriving.Utils
{
    public class Logger : ILogger
    {
        static ILogger _instance;
        public static string HockeyAppiOS => "TODO";
        public static string HockeyAppAndroid => "592d6d0d817e41cfb8ceb0b1a0066f0a ";
        public static string HockeyAppUWP => "b519fc3cd78c4bbf8697aa307f4e64dd";
        /// <summary>
        /// You can generate a key from: www.bingmapsportal.com
        /// This enables maps to show on the Past Trips screen
        /// </summary>
        public static string BingMapsAPIKey => "Au3KEWwJJgaNq5K4JQEaf5-rKMmObUBQvcTzA-nZ1bKckm1BmeOpDhArN90i_LSH";

        public static ILogger Instance => _instance ?? (_instance = ServiceLocator.Instance.Resolve<ILogger>());

        #region ILogger implementation

        public virtual void Identify(string uid, IDictionary<string, string> table = null)
        {
          
        }

        public virtual void Identify(string uid, string key, string value)
        {
            
        }

        public virtual void Track(string trackIdentifier, IDictionary<string, string> table = null)
        {
           
        }

        public virtual void Track(string trackIdentifier, string key, string value)
        {
           
        }

        public virtual ITrackHandle TrackTime(string identifier, IDictionary<string, string> table = null)
        {
            return null;
        }

        public virtual ITrackHandle TrackTime(string identifier, string key, string value)
        {
            return null;

        }

        public virtual void Report(Exception exception)
        {
 
        }

        public virtual Task Save()
        {
            return Task.FromResult(true);
        }

        public virtual Task PurgePendingCrashReports()
        {
            return Task.FromResult(true);
        }

        

        #endregion
    }
    
}