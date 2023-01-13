using Foundation;

namespace Selkhound.Client;

/// <summary>
/// The MacOS App Delegate.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    /// <summary>
    /// Retrieves an instance of the shared <see cref="MauiApp"/>.
    /// </summary>
    /// <returns>The shared <see cref="MauiApp"/>.</returns>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
