# Lombiq Orchard Visual Studio Extension readme



Visual Studio extension with many features frequently used by  [Lombiq](https://lombiq.com/) developers. Contains [Orchard](http://orchardproject.net/)-related as well as generic goodies.

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

- 1.3, 01..2019
    - "Inject Dependency" moved back to "Tools" so keyboard shortcuts can be assigned to it.
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


## How to add a new (Orchard) project template

For adding project templates the [SideWaffle](https://github.com/ligershark/side-waffle) library is used in this Visual Studio extension. If you want to create a new one just follow the steps below.

1. Create the project skeleton (use the Orchard.CodeGeneration feature) and give a unique name to the csproj file (eg. Orchard.ModuleTemplate.1.10.csproj).
2. Copy the skeleton to the /OrchardTemplates subfolder.
3. Add the project as an existing item to the Orchard Templates solution folder.
4. Open the build configuration manager and make this project not to be built when building the solution.
5. Right click on the main extension project and under the Add menu select the SideWaffle project. Select your project template in the form that has recently appeared.
6. Now you got 3 files created in your project skeleton (and one phantom project file in the main extension project, don't remove it).
	- \_Definitions/_project.vstemplate.xml: it contains all the information needed to be shown when adding a new project later. It's recommended that you replace this file with a similar one from the existing project templates and update the Name, Description and the ProjectTemplate/Project tags and the other tags if necessary.
	- _preprocess.xml: it is described here what to do right before creating the project from the template like replacing strings. It is also recommended to replace this file with one of the existing ones. You can update TemplateInfo/Path attribute and the Replacements if necessary.
	- sw-file-icon.png: the icon of the template shown when adding the project. You should replace it (eg. with an Orchard logo found in other templates).
7. Finally we need to edit some project files.
	- Module.txt/Theme.txt: you can use $safeprojectname$ or the string you've just specified in a Replacement list - both will make the project generator replace it with the project name (that is actualy the module's/theme's name). See one of the existing similar file.
	- Csproj file (right click on the project and select Properties): specify the value you've just given in the Replacement list to the Assembly name and Default namespace (eg. Orchard.ModuleTemplate) so this will be also replaced after the project has been created from this template.
	- Properties/AssemblyInfo.cs: edit the AssemblyTitle attribute and specify either the value you've just given in the Replacement list or the $safeprojectname$ string. Also the GUID needs to be auto-generated so replace the GUID value with $guid1$.

These were the commonly used modifications but make sure that all the replacements can be done if you have some special files (eg. if you have a code file then replace the root namespace with the one given in the csproj properties).

You can also use other template parameters, see the [MSDN documentation](https://msdn.microsoft.com/en-us/library/eehb4faa.aspx).

Optionally you can use [wizards](https://msdn.microsoft.com/en-us/library/ms185301.aspx) to implement some operations in the different phases of the project/item generation.

IMPORTANT: don't remove the added csproj file in the main project. It is being created by the SideWaffle template generator.