using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Net.NetworkInformation;

namespace LatinHelper
{
    public class StringOperations
    {
        public static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
         }
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd-HH-mm-ss-ffff");
        }
        public static string GetTimestampHEX(DateTime value)
        {
            ulong timestamp = ulong.Parse(value.ToString("yyyyMMddHHmmssffff"));
            return timestamp.ToString("X");
        }
        public static string StringAb(string needle, string haystack)
        {
            int index = haystack.IndexOf(needle);
            int indexend = index + needle.Length;
            String newString = haystack.Substring(indexend);
            return newString;
        }
        public static string StringBis(string needle, string haystack)
        {
            int index = haystack.IndexOf(needle);
            int indexend = index;
            String newString = haystack.Substring(0, indexend);
            return newString;
        }
        public static string GetApplicationsPath()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            return fi.DirectoryName;
        }
        public static string GetApplicationsFilePath()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            return fi.FullName;
        }
        public static int CountOf(string input, string substring)
        {
            string[] seperator = { substring };
            int count = 0;
            try
            {
                count = input.Split(seperator, StringSplitOptions.None).Count() - 1;
            }
            catch
            {
            }
            return count;
        }
    }
}
