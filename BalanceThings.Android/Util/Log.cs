using System;

namespace BalanceThings.Util
{
    internal class Log
    {
        private static readonly string TAG = "BALANCE_THINGS";

        internal static void I(string message)
        {
            Android.Util.Log.Info(TAG, message);
        }

        internal static void D(string message)
        {
            DateTime now = DateTime.Now;
            Android.Util.Log.Debug(TAG, message + " (Time: " + now.Hour + "h" + now.Minute + "m" + now.Second + "s" + now.Millisecond + "ms)");
        }

        internal static void V(string message)
        {
            Android.Util.Log.Verbose(TAG, message);
        }
    }
}