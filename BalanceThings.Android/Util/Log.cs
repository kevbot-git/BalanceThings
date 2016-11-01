namespace BalanceThings
{
    class Log
    {
        private static readonly string TAG = "BALANCE_THINGS";

        public static void I(string message)
        {
            Android.Util.Log.Info(TAG, message);
        }

        public static void D(string message)
        {
            Android.Util.Log.Debug(TAG, message);
        }

        public static void V(string message)
        {
            Android.Util.Log.Verbose(TAG, message);
        }
    }
}