# Lombiq Orchard Visual Studio Extension readme



Visual Studio extension with many features frequently used by  [Lombiq](https://lombiq.com/) developers. Contains [Orchard](https://orchardproject.net/)-related (including Orchard Core) as well as generic goodies.

The extension can also be installed from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=LombiqVisualStudioExtension.LombiqOrchardVisualStudioExtension), including installing directly from inside Visual Studio from under Tools &gt; Extensions and Updates.


## Tools

- Dependency Injector: when a class is opened in the editor you can inject a dependency with this feature. Type the dependency name, hit Enter and it will be injected. Can be invoked from under the Tools menu as "Inject Dependency".
- Orchard Error Log Watcher: watches the Orchard error log and if the log file exists and is not empty then notifies the user by making a button in the "Orchard Log Watcher" toolbar enabled. Clicking this button will open the error log using the default application. The feature can be turned off in the Options -> Orchard Log Watcher page. Turning the feature on and off also makes the related toolbar visible or hidden on the toolbar strip.


## Source repositories

The extension's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/lombiq-visual-studio-extension](https://bitbucket.org/Lombiq/lombiq-visual-studio-extension) (Mercurial repository)
- [https://github.com/Lombiq/Lombiq-Visual-Studio-Extension](https://github.com/Lombiq/Lombiq-Visual-Studio-Extension) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.


## Release notes

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
    - `IEnumerable&lt;T&gt;` and other generic types are handled when generating injected dependency names; e.g. for `IEnumerable&lt;IDependency&gt;` the field name `_dependencies` will be generated.
- 1.0, 24.05.2016
    - Dependency Injector tool
    - Templates for content part, shape template, injected dependency, Orchard 1.9 and 1.10 module