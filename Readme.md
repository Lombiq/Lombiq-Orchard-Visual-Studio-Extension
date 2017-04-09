# Lombiq Orchard Visual Studio Extension readme



Visual Studio extension with many features and templates frequently used by  [Lombiq](http://lombiq.com/) developers. Contains [Orchard](http://orchardproject.net/)-related as well as generic goodies.

The extension can also be installed from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=LombiqVisualStudioExtension.LombiqOrchardVisualStudioExtension), including installing directly from inside Visual Studio from under Tools &gt; Extensions and Updates.


## Tools

- Dependency Injector: when a class is opened in the editor you can inject a dependency with this feature. Type the dependency name, hit Enter and it will be injected. Can be invoked from under the Tools menu as "Inject Dependency".
- Orchard Error Log Watcher: watches the Orchard error log and if the log file exists and is not empty then notifies the user by making a button in the "Orchard Log Watcher" toolbar enabled. Clicking this button will open the error log using the default application. The feature can be turned off in the Options -> Orchard Log Watcher page. Turning the feature on and off also makes the related toolbar visible or hidden on the toolbar strip.


## Templates

Item templates can be found in "Add" &gt; "New Item" and in "Visual C#" &gt; "Orchard" categories. Project templates can be found in "Add" &gt; "New Project" and in "Visual C#" &gt; "Web" categories.

- Content Part templates: IMPORTANT: Always select the project itself before you are planning to add one of this kind of templates. These templates are to help your content part development. Eg. Content Part with or without record, Content Part with a generated Content Type etc. Every necessary file is generated (ie. Drivers, Handlers, Migrations etc.). You can also specify properties right after setting the name of the template, so all of the necessary code will be also generated. NOTE: Placement.info won't be created. :-(
- Empty Shape Template File: generates an empty cshtml file.
- Dependency Template File: generates an interface inherited from IDependency and a class that implements this interface. Those will be in one single file and the file name will be the class' name.
- Orchard 1.9 Module Template: scaffolds an Orchard 1.9 module into your solution. NOTE: make sure that the folder you selected is the Modules folder.
- Orchard 1.10 Module Template: scaffolds an Orchard 1.10 module, see the description above.


## Source repositories

The extension's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/lombiq-visual-studio-extension](https://bitbucket.org/Lombiq/lombiq-visual-studio-extension) (Mercurial repository)
- [https://github.com/Lombiq/Lombiq-Visual-Studio-Extension](https://github.com/Lombiq/Lombiq-Visual-Studio-Extension) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.


## Release notes

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