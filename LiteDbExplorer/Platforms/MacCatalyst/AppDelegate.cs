using Foundation;
using UIKit;

namespace LiteDbExplorer;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void BuildMenu(IUIMenuBuilder builder)
    {
        base.BuildMenu(builder);
        var formatMenuIdentifier = UIMenuIdentifier.Format.GetConstant();
        builder.RemoveMenu(formatMenuIdentifier);
        var editMenuIdentifier = UIMenuIdentifier.Edit.GetConstant();
        builder.RemoveMenu(editMenuIdentifier);
    }

}

[Register("SceneDelegate")]
public class SceneDelegate : MauiUISceneDelegate
{
}