using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace BalanceThings.Core
{
    [Activity(Label = "Balance Things"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    internal class AndroidActivity : Microsoft.Xna.Framework.AndroidGameActivity, ISensorEventListener
    {
        private static readonly object _syncLock = new object();
        private SensorManager _sensorManager;
        private Microsoft.Xna.Framework.Vector3 _lastAccelerometerValue;

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock) // Ensure single modification at any one time
            {
                _lastAccelerometerValue = new Microsoft.Xna.Framework.Vector3(e.Values[0], e.Values[1], e.Values[2]);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _sensorManager = (SensorManager) GetSystemService(SensorService);
            _lastAccelerometerValue = new Microsoft.Xna.Framework.Vector3();
            Game g = new SinglePlayerGame(GetAccelerometerValues);
            SetContentView((Android.Views.View) g.Services.GetService(typeof(Android.Views.View)));
            g.Run();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Game);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        private Microsoft.Xna.Framework.Vector3 GetAccelerometerValues()
        {
            return _lastAccelerometerValue;
        }
    }
}

