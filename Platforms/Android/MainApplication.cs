using Android.App;
using Android.Runtime;

namespace skps_services
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY",
            Value = "AIzaSyAMGZARJb5j1JY_3s2O97xIPKixBUoMp_U")]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
