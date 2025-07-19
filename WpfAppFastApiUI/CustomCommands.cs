using System.Windows.Input;

namespace WpfAppFastApiUI;

public class CustomCommands
{
    public static readonly RoutedUICommand GetCategories = new RoutedUICommand(
        "Get Categories",
        "GetCategories",
        typeof(CustomCommands),
        new InputGestureCollection
        {
            new KeyGesture(Key.C, ModifierKeys.Control)
        }
    );
    
    public static readonly RoutedUICommand BackToMain = new RoutedUICommand(
        "Back To Main",
        "BackToMain",
        typeof(CustomCommands),
        new InputGestureCollection
        {
            new KeyGesture(Key.B, ModifierKeys.Control)
        }
    );
    
    public static readonly RoutedUICommand GetCart = new RoutedUICommand(
        "Get Cart",
        "GetCart",
        typeof(CustomCommands),
        new InputGestureCollection
        {
            new KeyGesture(Key.G, ModifierKeys.Control)
        }
    );
}