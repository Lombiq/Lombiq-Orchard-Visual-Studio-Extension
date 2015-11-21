# Lombiq Visual Studio Extension readme



Visual Studio extension with many features and templates frequently used by the [Lombiq](http://lombiq.com/) developers. Contains [Orchard](http://orchardproject.net/)-related as well as generic goodies.


## Features

* Dependency Injector: When a class is opened in the editor you can inject a dependency with this feature. You type the dependency name and hit Enter than it will be injected. It can be found in the Tools menu with the name 'Inject Dependency'.


## Templates

Item templates can be found in 'Add' > 'New Item' and in 'Visual C#' > 'Orchard' categories. Project templates can be found in 'Add' > 'New Project' and in 'Visual C#' > 'Web' categories.

* Content Part templates: IMPORTANT: Always select the project itself before you are planning to add one of this kind of templates. These templates are to help your content part development. Eg. Content Part with or without record, Content Part with a generated Content Type etc. Every necessary files are generated (ie. Drivers, Handlers, Migrations etc.). You can also give properties right after giving the name of the template, so all of the necessary code will be also generated. NOTE: Placement.info won't be created. :-(

* Empty Shape Template File: It generates an empty cshtml file.

* Dependency Template File: It generates an interface inherited from IDependency and a class that implements this interface. Those will be in one single file and the file name will be the class' name.

* Orchard 1.9 Module Template: It scaffolds an Orchard 1.9 module to your solution. NOTE: Make sure that the folder you selected is the Modules folder.


## Source repositories

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/lombiq-visual-studio-extension](https://bitbucket.org/Lombiq/lombiq-visual-studio-extension) (Mercurial repository)
- [https://github.com/Lombiq/Lombiq-Visual-Studio-Extension](https://github.com/Lombiq/Lombiq-Visual-Studio-Extension) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.