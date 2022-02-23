# Lombiq Orchard Visual Studio Extension readme



## About

Visual Studio extension with many features frequently used by  [Lombiq](https://lombiq.com/) developers. Contains [Orchard CMS](https://www.orchardcore.net/)-related (including Orchard Core and Orchard 1.x) as well as generic goodies. For Orchard developers and for other .NET developers alike!

The extension can be installed from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=LombiqVisualStudioExtension.LombiqOrchardVisualStudioExtension), including installing directly from inside Visual Studio from under Extensions &gt; Manage Extensions.


## Tools

### Dependency Injector
When a class is open in the editor you can inject a dependency with this feature. Type the dependency name, hit Enter and it will be injected.

Can be invoked from under Tools &gt; Inject Dependency. You can also bind a hotkey to it from under Tools &gt; Options &gt; Keyboard, and then search for "Tools.InjectDependency".

### (Orchard) Error Log Watcher
Watches the Orchard error log (or any other error log) and lights up an icon when a new entry was logged. And wait, there's more! This feature also supports [BlinkStick](https://www.blinkstick.com/) USB LED sticks that can blink or light up when an error happens. Check out [this video](https://www.youtube.com/watch?v=MQx5WpJqGi8) for a demo of the whole feature.

If the log file exists, was recently updated and is not empty then this features notifies you by making a button in the Orchard Log Watcher toolbar light up in red. Clicking this button will open the error log using the default application for that file type and turn off the notification (the same will happen if you delete the contents of the log file). You can add this button anywhere in the Visual Studio toolbar from under View &gt; Toolbars &gt; Orchard Log Watcher.

If you have any [BlinkStick](https://www.blinkstick.com/) USB LED stick (all of them should be compatible but only tested with the [Nano](https://www.blinkstick.com/products/blinkstick-nano) and the [Strip](https://www.blinkstick.com/products/blinkstick-strip)) then just plug it in and it'll light up too. This will also work if the button is not placed onto the toolbar.

Settings:
- The feature can be turned on or off on the Options &gt; Orchard Log Watcher page. This also makes the related toolbar visible or hidden on the toolbar strip.
- Patterns for directories where log files are written can be specified there. If your app doesn't use the standard Orchard log file naming conventions then you can also specify custom file name patters there.
- For your BlinkStick LED you can specify the color and whether it should blink (or light up continuously).


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
