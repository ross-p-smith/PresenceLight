﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
//using OSVersionHelper;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace PresenceLight
{
    internal class ThisAppInfo
    {
        internal static string GetDisplayName()
        {
            try
            {
                return Package.Current.DisplayName;
            }
            catch
            {
                return "Not packaged";
            }
        }

        internal static string GetInstallLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        internal static string GetInstallationDate()
        {
            var date = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
            return $"{date.ToShortDateString()} {date.ToShortTimeString()}";
        }

        internal static string GetSettingsLocation()
        {
            string SETTINGS_FILENAME = "settings.json";
            StorageFolder _settingsFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            return $"{_settingsFolder.Path}\\{SETTINGS_FILENAME}";
        }

        internal static string GetPackageVersion()
        {
            try
            {
                return $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
            }
            catch
            {
                return "Not packaged";
            }
        }

        internal static string GetPackageChannel()
        {
            try
            {
                return Package.Current.Id.Name.Substring(Package.Current.Id.Name.LastIndexOf('.') + 1);
            }
            catch
            {
                return null;
            }
        }

        internal static string GetDotNetInfo()
        {
            var runTimeDir = new FileInfo(typeof(string).Assembly.Location);
            var entryDir = new FileInfo(Assembly.GetEntryAssembly().Location);
            var IsSelfContaied = runTimeDir.DirectoryName == entryDir.DirectoryName;

            var result = ".NET Framework - ";
            if (IsSelfContaied)
            {
                result += "Self Contained Deployment";
            }
            else
            {
                result += "Framework Dependent Deployment";
            }

            return result;
        }

        internal static string GetAppInstallerUri()
        {
            string result;

            try
            {

                if (ApiInformation.IsMethodPresent("Windows.ApplicationModel.Package", "GetAppInstallerInfo"))
                {
                    var aiUri = GetAppInstallerInfoUri(Package.Current);
                    if (aiUri != null)
                    {
                        result = aiUri.ToString();
                    }
                    else
                    {
                        result = "not present";
                    }
                }
                else
                {
                    result = "Not Available";
                }

                return result;
            }
            catch
            {
                return "Not packaged";
            }
        }

        internal static string GetDotNetRuntimeInfo()
        {
            return typeof(object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Uri GetAppInstallerInfoUri(Package p)
        {
            var aiInfo = p.GetAppInstallerInfo();
            if (aiInfo != null)
            {
                return aiInfo.Uri;
            }
            return null;
        }
    }
}
