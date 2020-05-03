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


## Release notes

- 1.5.1
    - Fixing potential UI deadlocks when the Orchard Error Log Watcher icon was visible on the toolbar, causing VS freezes frequently on build.
- 1.5.1, 24.04.2020
    - Fixing startup performance issues. If you've previously seen a message like "Visual Studio stopped responding for x seconds. Disabling the extension Lombiq Orchard Visual Studio Extension 1.5.0 might help." when launching Visual Studio then that's gone now.
    - Updating Lombiq logo to the current one.
- 1.5.0, 04.04.2020
    - Support for [BlinkStick](https://www.blinkstick.com/) USB LED sticks so they can light up when the Orchard Error Log Watcher detects a new log entry.
    - Added support for custom log file name patters for the Orchard Error Log Watcher so files not following Orchard naming conventions will be detected too.
- 1.4.2, 06.11.2019
    - Fixing Visual Studio 2017 incompatibility issues.
- 1.4.1, 20.10.2019
    - Fixing incompatibility issues with Visual Studio 2019.
- 1.4, 17.06.2019
    - Visual Studio 2019 compatibility added.
- 1.3.1, 23.01.2019
    - Fixing that the short field name check-box can't be unchecked if the dependencies are `IWorkContextAccessor` or `IHttpContextAccessor`.
- 1.3, 11.01.2019
    - "Inject Dependency" moved back to "Tools" so keyboard shortcuts can be assigned to it.
    - Dependency Injector dialog provide more options to edit injected property and the private field.
    - Dependency Injector will suggest the commonly used dependency names.
    - Dependency Injector dialog contains a code preview.
    - Dependency Injector will recognize some dependencies and will use short name automatically.
    - Dependency Injector will suggest field and constructor types and names in a more intelligent way (e.g. field name will be `T` if the `IStringLocalizer` is being injected).
    - Dependency Injector will use the current class name as generic parameter in some suggested dependencies (e.g. `IStringLocalizer<T>`).
    - Orchard Log Watcher log paths can be updated to contain multiple paths (e.g. Orchard Core).
    - Orchard Log Watcher paths can contain wildcards (e.g. src/*.Web/App_Data/logs).
    - Orchard module templates have been removed.
    - Orchard item templates have been removed.
- 1.2.2, 09.11.2017
    - Visual Studio 2017 compatibility added.
    - "Inject Dependency" moved from under "Tools" to a separate toolbar called "Dependency Injector".
    - The toolbar "Orchard Log Watcher Toolbar" is renamed to "Orchard Log Watcher".
- 1.2.1, 09.04.2017
    - Fixing short dependency name generation in the Dependency Injector feature.
- 1.2, 09.04.2017
    - New Orchard Error Log Watcher feature.
- 1.1, 18.03.2017
    - The Dependency Injector tool now creates the constructor if it doesn't exist (i.e. it can be used even if there's no constructor already).
    - `IEnumerable<T>` and other generic types are handled when generating injected dependency names; e.g. for `IEnumerable<IDependency>` the field name `_dependencies` will be generated.
- 1.0, 24.05.2016
    - Dependency Injector tool
    - Templates for content part, shape template, injected dependency, Orchard 1.9 and 1.10 module