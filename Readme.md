# Lombiq Visual Studio Extension readme



Visual Studio extension with many features and templates frequently used by the [Lombiq](http://lombiq.com/) developers. Contains [Orchard](http://orchardproject.net/)-related as well as generic goodies.


## Tools

- Dependency Injector: when a class is opened in the editor you can inject a dependency with this feature. Type the dependency name, hit Enter and it will be injected. Can be invoked from under the Tools menu as "Inject Dependency".


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