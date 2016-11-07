using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using BalanceThings.Util;

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
            Window.AddFlags(WindowManagerFlags.KeepScreenOn | WindowManagerFlags.Fullscreen);
            Game g = new SinglePlayerGame(GetAccelerometerValues);
            g.GamePause = onGamePause;
            g.GameResume = onGameResume;
            SetContentView((Android.Views.View) g.Services.GetService(typeof(Android.Views.View)));
            g.Run();
        }

        private void onGamePause()
        {
            setImmersive(false);
            Log.D("Success! Paused");
        }

        private void onGameResume()
        {
            setImmersive(true);
            Log.D("Success! Resumed");
        }

        protected override void OnResume()
        {
            base.OnResume();
            setImmersive(true);
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Game);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        private void setImmersive(bool isImmersive)
        {
            if (isImmersive)
                Window.DecorView.SystemUiVisibility |= (StatusBarVisibility)((int)StatusBarVisibility.Hidden |
                    (int)SystemUiFlags.Immersive | (int)SystemUiFlags.HideNavigation);
            else
                Window.DecorView.SystemUiVisibility &= ~(StatusBarVisibility)((int)StatusBarVisibility.Hidden |
                    (int)SystemUiFlags.Immersive | (int)SystemUiFlags.HideNavigation);
        }

        private Microsoft.Xna.Framework.Vector3 GetAccelerometerValues()
        {
            return _lastAccelerometerValue;
        }
    }
}

