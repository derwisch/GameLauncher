# GameLauncher
Easy to configure game launcher/updater template

## Content
There are two projects in this repository, the main GameLauncher project and the ManifestGenerator which builds the XML file needed by the launcher to patch your game.

## Setup
To get the launcher up and running you need the following things set up beforehand:
- A ftp server with a seperate user with readonly access
- All required gamefiles
- A changelog (or other plain text or html file to display in the launcher)

### The LauncherSetup class
The first step in setting up the launcher project would be to set up all needed information in the `LauncherSetup` class inside the Program.cs

It contains almost all settings needed for the launcher. The settings are broken down into three segments. While everything is commented in the code I'll break all of them down here.

#### UI Settings
| Setting | Description |
|---|---|
| LAUNCHER_UI_TITLE | The title bar of the launcher window |
| LauncherLinks | A collection of links to be displayed in the sidebar of the launcher |
| IS_CHANGELOG_HTML | A switch to determine whether the provvided changelog is a html file or plain text |
| TEXT_CHANGELOG_STYLE | A html style for the changelog when in plain text mode and the error message displayed when the changelog could not be loaded |
| ADDITIONAL_HTML_REPLACEMENTS | Enables very basic formatting for plaintext files using a syntax vaguely simmilar to markdown |

#### FTP Settings
| Setting | Description |
|---|---|
| FTP_READONLY_USER_NAME | Username of the **readonly** ftp user used to download the launcher and game files |
| FTP_READONLY_USER_PASS | Password of the **readonly** ftp user used to download the launcher and game files |
| FTP_BASE_URL | Base URL of your ftp server + base directory of your game update files.<br>*Must contain tailing '/'* |
| FTP_GAME_FILE_UPDATE_FOLDER | Folder in which the game files defined in the content manifest are located on the server |
| FTP_GAME_CONTENT_FILE | Name of the content manifest files |
| FTP_GAME_VERSION_FILE | Name of the game version file |
| FTP_GAME_CHANGES_FILE | Name of the changelog file |
| FTP_LAUNCHER_VERSION_FILE | Name of the launcher version file |

#### Launch Settings
| Setting | Description |
|---|---|
| GAME_EXE_FILE_NAME | Name of your game's executeable file |
| GAME_EXE_ARGUMENTS | Command line arguments to launch your game with (if any) |

### Setting up the required files using the ManifestGenerator project
Simply compile the ManifestGenerator project and run the application with the following command line arguments:

``` batch
ManifestGenerator <game-dir> <output-dir>\content.xml <game.exe> <overwrite-manifest:true|false>

:: for example like this:
ManifestGenerator D:\projects\MyGame\bin D:\projects\MyGame\content.xml MyGame.exe true
```

This will generate two nessecary files, content.xml (or what you named it) and content.version. If the generator was not able to determine the game version you'll get the error message `Could not determine game version. Please enter yourself.`. If that happens you need to manually enter it in the content.version file

``` ini
GameVersion=0.1.0
```

This information is currently only used for display in the launcher as to what the current version of your game is as the updater checks for file size, last write and the file hash to determine if the file still is up to date.

You now have three of the nessecary four files, the two content manifest files and your previously prepared changelog. The last nessecary file is the launcher.version file. There is an example prepared inside the AncillaryFiles directory. The file contains the current version of the launcher as well as the URL where the new launcher can be downloaded.

``` ini
VERSION=0.1.0
FILENAME=ftp://your.ftp.server/GameLauncher.exe
```

### Further setup and customization 
With these settings in place the launcher is up and running, patching your files and itself if you choose to update the launcher itself.

While that is all you *need* to do for a working launcher you can change a few things more.

One thing you absolutely should do is changing the launchers window and executeable icon. The executeable icon is easily changed in the project properties. The window icon can be changed using the forms designer as a setting on the FormLauncher form itself. There you could also change the form design, colors and layout.

If you want your launcher to support more languages (or have localized sidebar links and link titles) you can change the `localization.xml`. The layout is quite simple, every language has a language block (with one language nessecarily marked as `isFallback="true"`) that contains multiple nodes for each available text.

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<localization>
  <lang key="en-US" isFallback="true">
    <entry key="Key.Of.This.Entry">Text of this entry</entry>
  </lang>
  <lang key="de-DE">
    <entry key="Key.Of.This.Entry">Text dieses Eintrags</entry>
  </lang>
  <lang key="es-ES">
    <entry key="Key.Of.This.Entry">Texto de esta entrada</entry>
  </lang>
  <lang key="fr-FR">
    <entry key="Key.Of.This.Entry">Texte de cette entrée</entry>
  </lang>
  <lang key="zh-CN">
    <entry key="Key.Of.This.Entry">該條目的文字</entry>
  </lang>
  <lang key="ru-RU">
    <entry key="Key.Of.This.Entry">Текст этой записи</entry>
  </lang>
</localization>
```

### Finishing the setup
After all that is done, build the game launcher and upload the update information files, the game files and the launcher to your ftp server and give it a testing run in your IDE. if anything is configured wrong the application should run into exceptions telling you what went wrong.

## Have Questions?
If you have any questions, feel free to message me at der_wisch ( at ) outlook.com or send me a message on reddit ([/u/der_wisch](https://old.reddit.com/message/compose/?to=der_wisch))

## Want to contribute?
Found a bug? Want your language in the default translations/texts? Just create a pull request with your changes to the localization.xml

## ToDo/Ideas/Future Vision
- Merge setup of settings and icons into one step/place
  - Move configuration/setup to xml(?) file and add custom build step generating the nessecary code before build
- Add missing default translations
- Abstract the update mechanism further to allow implementation into other launchers or games themselves
- Add the launcher update credentials to the launcher.version file to allow downloads from other servers (for example when the server moves to another domain)