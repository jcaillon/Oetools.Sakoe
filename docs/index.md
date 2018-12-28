# SAKOE

## About



### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, use the --help option abundantly.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value")
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. "my ""special"" value")
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.
Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.
  sakoe @responsefile.txt

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/

If you want to help, you are welcome to contribute to the github repo. 
You are invited to STAR the project on github to increase its visibility!
## Table of content

- [build](#sakoe-build)
- [database](#sakoe-database)
  - [create](#sakoe-database-create)
  - [delete](#sakoe-database-delete)
  - [kill](#sakoe-database-kill)
  - [project](#sakoe-database-project)
    - [create](#sakoe-database-project-create)
  - [repair](#sakoe-database-repair)
  - [start](#sakoe-database-start)
  - [stop](#sakoe-database-stop)
- [hash](#sakoe-hash)
  - [files](#sakoe-hash-files)
  - [string](#sakoe-hash-string)
- [lint](#sakoe-lint)
- [manual](#sakoe-manual)
- [prohelp](#sakoe-prohelp)
  - [chm](#sakoe-prohelp-chm)
  - [keyword](#sakoe-prohelp-keyword)
  - [listchm](#sakoe-prohelp-listchm)
  - [promsg](#sakoe-prohelp-promsg)
- [project](#sakoe-project)
  - [gitignore](#sakoe-project-gitignore)
  - [init](#sakoe-project-init)
  - [list](#sakoe-project-list)
- [prolib](#sakoe-prolib)
  - [list](#sakoe-prolib-list)
- [selftest](#sakoe-selftest)
  - [consoleformat](#sakoe-selftest-consoleformat)
  - [input](#sakoe-selftest-input)
  - [log](#sakoe-selftest-log)
  - [prompt](#sakoe-selftest-prompt)
  - [responsefile](#sakoe-selftest-responsefile)
  - [wrap](#sakoe-selftest-wrap)
- [update](#sakoe-update)
- [utilities](#sakoe-utilities)
  - [connectstr](#sakoe-utilities-connectstr)
  - [execpath](#sakoe-utilities-execpath)
  - [propathfromini](#sakoe-utilities-propathfromini)
  - [version](#sakoe-utilities-version)
- [version](#sakoe-version)
- [xcode](#sakoe-xcode)
  - [decrypt](#sakoe-xcode-decrypt)
  - [encrypt](#sakoe-xcode-encrypt)
  - [list](#sakoe-xcode-list)


## sakoe build

### Synopsis

Build automation for Openedge projects. This command is the bread and butter of this tool.

### Usage

`sakoe build [<project>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<project\> | Path or name of the project file. The .oeproj.xml extension is optional. Defaults to the .oeproj.xml file found. The search is done in the current directory and in the .oe directory when it exists. |

### Options

| Option | Description |
| --- | --- |
| -c, --config-name <config> | The name of the build configuration to use for the build. This name is found in the .oeproj.xml file. Defaults to the first build configuration found in the project file. |
| -e, --extra-config <project=config> | (Can be used multiple times) In addition to the base build configuration specified by <project> and --config-name, you can dynamically add a child configuration to the base configuration with this option. This option can be used multiple times, each new configuration will be added as a child of the previously defined configuration.
 This option allows you to share, with your colleagues, a common project file that holds the property of your application and have an extra configuration in local (just for you) which you can use to build the project in a specific local directory.
 For each extra configuration, specify the path or the name of the project file and the configuration name to use. If the project file name if empty, the main <project> is used. |
| -p, --property <key=value> | (Can be used multiple times) A pair of key/value to dynamically set a property for this build. The value set this way will prevail over the value defined in a project file.
 Each pair should specify the name of the property to set and the value that should be used.
 Use the option --property-help to see the full list of properties available as well as their documentation. |
| -v, --variable <key=value> | (Can be used multiple times) A pair of key/value to dynamically set a variable for this build. A variable set this way will prevail over a variable with the same name defined in a project file.
 Each pair should specify the name of the variable to set and the value that should be used. |
| -ph, --property-help | Shows the list of each build property usable with its full documentation. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |
```

### Build properties

-p "AddAllSourceDirectoriesToPropath=True"
-p "AddDefaultOpenedgePropath=True"
-p "AllowDatabaseShutdownByProcessKill=True"
-p "AppendMaxConnectionTryToConnectionString=True"
-p "BuildConfigurationExportFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.oeproj.xml"
-p "BuildHistoryInputFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.xml"
-p "BuildHistoryOutputFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.xml"
-p "CompilableFileExtensionPattern=*.p;*.w;*.t;*.cls"
-p "CompileOptions="
-p "CompileStatementExtraOptions="
-p "CompileWithDebugList=False"
-p "CompileWithListing=False"
-p "CompileWithPreprocess=False"
-p "CompileWithXmlXref=False"
-p "CompileWithXref=False"
-p "CurrentBranchName="
-p "CurrentBranchOriginCommit="
-p "DlcDirectoryPath=$DLC (openedge installation directory)"
-p "EnabledIncrementalBuild=True"
-p "ExtraDatabaseConnectionString="
-p "ExtraOpenedgeCommandLineParameters="
-p "ForceSingleProcess=False"
-p "FullRebuild=False"
-p "IncludeSourceFilesCommittedOnlyOnCurrentBranch=False"
-p "IncludeSourceFilesModifiedSinceLastCommit=False"
-p "IniFilePath="
-p "MinimumNumberOfFilesPerProcess=10"
-p "NumberProcessPerCore=1"
-p "OpenedgeCodePage="
-p "OpenedgeTemporaryDirectoryPath=$TEMP/.oe_tmp-xxx (temporary folder)"
-p "OutputDirectoryPath={{SOURCE_DIRECTORY}}\bin"
-p "ProcedurePathToExecuteAfterAnyProgressExecution="
-p "ProcedureToExecuteBeforeAnyProgressExecutionFilePath="
-p "PropathExclude="
-p "PropathExcludeHiddenDirectories=False"
-p "PropathExcludeRegex="
-p "PropathExtraVcsPatternExclusion=.git**;.svn**;.oe**"
-p "PropathInclude="
-p "PropathIncludeRegex="
-p "PropathRecursiveListing=True"
-p "RebuildFilesWithCompilationErrors=True"
-p "RebuildFilesWithMissingTargets=False"
-p "RebuildFilesWithNewTargets=False"
-p "ReportHtmlFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.html"
-p "ShutdownCompilationDatabasesAfterBuild=True"
-p "SourceDirectoryPath=$PWD (current directory)"
-p "SourceExclude="
-p "SourceExcludeHiddenDirectories=False"
-p "SourceExcludeRegex="
-p "SourceExtraVcsPatternExclusion=.git**;.svn**;.oe**"
-p "SourceInclude="
-p "SourceIncludeRegex="
-p "SourceOverrideFileList="
-p "SourceRecursiveListing=True"
-p "StopBuildOnCompilationError=True"
-p "StopBuildOnCompilationWarning=False"
-p "StopBuildOnTaskError=True"
-p "StopBuildOnTaskWarning=False"
-p "TestMode=False"
-p "TryToHideProcessFromTaskBarOnWindows=True"
-p "TryToOptimizeCompilationDirectory=True"
-p "UseCharacterModeExecutable=False"
-p "UseCheckSumComparison=False"
-p "UseCompilerMultiCompile=False"
-p "UseSimplerAnalysisForDatabaseReference=False"

> Display the full documentation of each build property by running 'sakoe build --property-help'.

### Extra config

The example below illustrate the usage of the --extra-config option:
We have 2 project files (p1 and p2) containing build configurations as described below:

p1.oeproj.xml
├─ Configuration1
└─ Configuration2

p2.oeproj.xml
└─ Configuration3
   ├─ Configuration4
   │  ├─ Configuration5
   │  └─ Configuration6
   └─ Configuration7

We use the following command line to start a build:

sakoe build p1 --config-name Configuration1 --extra-config p2=Configuration6 --extra-config Configuration2 

Below is the equivalent of how build configurations are nested in this scenario:

Configuration1
└─ Configuration3
   └─ Configuration4
      └─ Configuration6
         └─ Configuration2

This allow a lot of flexibility for organizing and partitioning your build process.

### Notes

Create a new project file using the command: 'sakoe project init'.
Get a more in-depth help and learn about the concept of a build (in sakoe) using the command: 'sakoe manual build'.
```

## sakoe database

### Synopsis

TODO : db

### Usage

`sakoe database [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| create | TODO : database creation |
| delete | TODO : delete database |
| kill | Kill all the _mprosrv process |
| project | TODO : db |
| repair | TODO : repair database |
| start | TODO : database proserve |
| stop | TODO : stop database |

### Description

TODO : db

## sakoe database create

### Synopsis

TODO : database creation

### Usage

`sakoe database create [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to create (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -df, --df | Path to the .df file containing the database schema definition |
| -st, --st | Path to the .st file containing the database physical structure |
| -bs, --blocksize | The blocksize to use when creating the database |
| -cp, --codepage | Existing codepage in the openedge installation $DLC/prolang/(codepage) |
| -ni, --newinstance | Use -newinstance in the procopy command |
| -rp, --relativepath | Use -relativepath in the procopy command |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : extended database creation

## sakoe database delete

### Synopsis

TODO : delete database

### Usage

`sakoe database delete [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to delete (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : database

## sakoe database kill

### Synopsis

Kill all the _mprosrv process

### Usage

`sakoe database kill [options]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe database project

### Synopsis

TODO : db

### Usage

`sakoe database project [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| create | TODO : repair database |

### Description

TODO : db

## sakoe database project create

### Synopsis

TODO : repair database

### Usage

`sakoe database project create <target database path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<target database path\> | Path to the database to repair (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : database

## sakoe database repair

### Synopsis

TODO : repair database

### Usage

`sakoe database repair [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to repair (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : database

## sakoe database start

### Synopsis

TODO : database proserve

### Usage

`sakoe database start [<db path>] [options] [[--] \<arg\>...]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to start (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -np, --next-port | Port number, the next available port after this number will be used to start the database |
| -p, --port | Port number that will be used by this database |
| -n, --nbusers | Number of users that should be able to connect to this database simultaneously |
| -s, --service | Service name for the database, an alternative to the port number |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : database proserve

## sakoe database stop

### Synopsis

TODO : stop database

### Usage

`sakoe database stop [<db path>] [options] [[--] \<arg\>...]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to stop (.db extension is optional) |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : database

## sakoe hash

### Synopsis

Compute hash values of files or strings using the Openedge ENCODE function.

### Usage

`sakoe hash [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| files | Returns 16 byte hash values computed from the content of input files. |
| string | Returns a 16 byte hash value from a string. |

## sakoe hash files

### Synopsis

Returns 16 byte hash values computed from the content of input files.

### Usage

`sakoe hash files <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The file to process or a directory with files to process. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -f, --file <path> | (Can be used multiple times) File that should be added to the listing. Can be used multiple times. |
| -d, --directory <path> | (Can be used multiple times) Directory containing files that should be added to the listing. Can be used multiple times. |
| -r, --recursive | Recursive listing in the directories. |
| -i, --include <filter> | Include filter for directory listing. Can use wildcards (**,*,?). |
| -e, --exclude <filter> | Exclude filter for directory listing. Can use wildcards (**,*,?). |
| -ir, --include-regex <filter> | Regular expression include filter for directory listing. |
| -er, --exclude-regex <filter> | Regular expression include filter for directory listing.. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe hash string

### Synopsis

Returns a 16 byte hash value from a string.

### Usage

`sakoe hash string <string> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<string\> | The string to hash. |

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe lint

### Synopsis

TODO : prolint

### Usage

`sakoe lint <command> [options] [[--] \<arg\>...]`

### Arguments

| Argument | Description |
| --- | --- |
| \<command\> | Main command  |

### Options

| Option | Description |
| --- | --- |
| -m | Message |
| -t, --trace[:<TRACE>] | Trace |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

TODO : prolint

## sakoe manual

### Synopsis

The manual of this tool. Learn about its usage and about key concepts of sakoe.

### Usage

`sakoe manual [options]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |
```

### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, use the --help option abundantly.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value")
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. "my ""special"" value")
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.
Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.
  sakoe @responsefile.txt

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/

If you want to help, you are welcome to contribute to the github repo. 
You are invited to STAR the project on github to increase its visibility!

```

## sakoe prohelp

### Synopsis

Access the Openedge help.

### Usage

`sakoe prohelp [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| chm | Opens a .chm file (windows help file) present in $DLC/prohelp. |
| keyword | Look for help on the given Openedge keyword in the language windows help file. |
| listchm | List all the .chm files (windows help files) available in $DLC/prohelp. |
| promsg | Displays the extended error message using an error number. |

## sakoe prohelp chm

### Synopsis

Opens a .chm file (windows help file) present in $DLC/prohelp.

### Usage

`sakoe prohelp chm <chm file name> <topic> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<chm file name\> | The file name of the .chm file to display. |
| \<topic\> | Open the .chm on the given topic. |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe prohelp keyword

### Synopsis

Look for help on the given Openedge keyword in the language windows help file.

### Usage

`sakoe prohelp keyword <keyword> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<keyword\> | The keyword you would like to find in the help. |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe prohelp listchm

### Synopsis

List all the .chm files (windows help files) available in $DLC/prohelp.

### Usage

`sakoe prohelp listchm [options]`

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe prohelp promsg

### Synopsis

Displays the extended error message using an error number.

### Usage

`sakoe prohelp promsg <message number> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<message number\> | The number of the error message to show. |

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

This command uses the content of files located in $DLC/prohelp/msgdata to display information.

## sakoe project

### Synopsis

Commands related to an Openedge project (.oe directory).

### Usage

`sakoe project [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| gitignore | Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists). |
| init | Initialize a new Openedge project file (.oeproj.xml). |
| list | List all the project files or list the build configurations in a project file. |

## sakoe project gitignore

### Synopsis

Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists).

### Usage

`sakoe project gitignore <directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<directory\> | The repository base directory (source directory). Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe project init

### Synopsis

Initialize a new Openedge project file (.oeproj.xml).

### Usage

`sakoe project init <directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<directory\> | The directory in which to initialize the project. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -p, --project-name <name> | The name of the project to create. Defaults to the current directory name. |
| -l, --local | Create the new project file for local use only. A local project should contain build configurations specific to your machine and thus should not be shared or versioned in your source control system. |
| -f, --force | Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe project list

### Synopsis

List all the project files or list the build configurations in a project file.

### Usage

`sakoe project list <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The project file in which to list the build configurations or the project base directory (source directory) in which to list the project files. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe prolib

### Synopsis

CRUD operations for pro-libraries (.pl files).

### Usage

`sakoe prolib [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| list | List the content of a prolib file. |

## sakoe prolib list

### Synopsis

List the content of a prolib file.

### Usage

`sakoe prolib list <string> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<string\> | The string to hash. |

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe selftest

### Synopsis

A command to test the behaviour of this tool.

### Usage

`sakoe selftest [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| consoleformat | Subcommand that shows the use of CsConsoleFormat |
| input | Subcommand that shows the usage of options and arguments |
| log | Subcommand that shows the usage of log |
| prompt | Subcommand that shows the usage of prompt |
| responsefile | Subcommand that shows the usage of a response file |
| wrap | Subcommand that shows the word wrap |

### Description

sakoe selftest

## sakoe selftest consoleformat

### Synopsis

Subcommand that shows the use of CsConsoleFormat

### Usage

`sakoe selftest consoleformat [options]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe selftest consoleformat

## sakoe selftest input

### Synopsis

Subcommand that shows the usage of options and arguments

### Usage

`sakoe selftest input FileTruc Folder [options] [[--] <mon truc>...]`

### Arguments

| Argument | Description |
| --- | --- |
| FileTruc |  |
| Folder |  |

### Options

| Option | Description |
| --- | --- |
| --git-dir | GitDir |
| -X, --request | HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post. HTTP Method: GET or POST. Defaults to post. |
| -m, --message <MESSAGE> | Required. The message |
| --to <EMAIL> | Required. The recipient. |
| --attachment <FILE> | (Can be used multiple times) Attachments |
| -i, --importance-fuck <IMPORTANCE_FUCK> | ImportanceFuck |
| -c, --color <COLOR> | The colors should be red or blue |
| --max-size <MB> | The maximum size of the message in MB. |
| -d, --directory |  |
| -b, --block[:<BLOCK>] | Block |
| -b2, --block2 | Block2 |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe st input boom.txt --attachment first --attachment "second with spaces"
sakoe st input -b2 s1024

## sakoe selftest log

### Synopsis

Subcommand that shows the usage of log

### Usage

`sakoe selftest log [options]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe selftest log

## sakoe selftest prompt

### Synopsis

Subcommand that shows the usage of prompt

### Usage

`sakoe selftest prompt [options]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe selftest prompt

## sakoe selftest responsefile

### Synopsis

Subcommand that shows the usage of a response file

### Usage

`sakoe selftest responsefile [options]`

### Options

| Option | Description |
| --- | --- |
| -c, --create | Create the response file |
| -f | (Can be used multiple times) List of files. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe selftest responsefile

## sakoe selftest wrap

### Synopsis

Subcommand that shows the word wrap

### Usage

`sakoe selftest wrap [options]`

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

sakoe selftest wrap

## sakoe update

### Synopsis

Update this tool with the latest release found on github.

### Usage

`sakoe update [options]`

### Options

| Option | Description |
| --- | --- |
| -b, --get-beta | Accept to update from new 'beta' (i.e. pre-release) versions of the tool. This option will be used by default if the current version of the tool is a beta version. Otherwise, only stable releases will be used for updates.  |
| -p, --proxy | The http proxy to use for this update. Useful if you are behind a corporate firewall. The expected format is: 'http(s)://[user:password@]host[:port]'. It is also possible to use the environment variable HTTP_PROXY to set this value. |
| -c, --check-only | Check for new releases but exit the command before actually updating the tool. |
| -u, --override-github-url | Use an alternative url for the github api. This option is here to allow updates from a different location (a private server for instance) but should not be used in most cases. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe utilities

### Synopsis

Miscellaneous utility commands.

### Usage

`sakoe utilities [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| connectstr | Returns a single line connection string from a .pf file. |
| execpath | Returns the pro executable full path. |
| propathfromini | Returns PROPATH value found in a .ini file. |
| version | Returns the version found for the Openedge installation. |

## sakoe utilities connectstr

### Synopsis

Returns a single line connection string from a .pf file.

### Usage

`sakoe utilities connectstr <.pf path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<.pf path\> | The file path to the parameter file (.pf) to use. |

### Options

| Option | Description |
| --- | --- |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

This command will skip unnecessary whitespaces and new lines.
It will also ignore comment lines starting with #.

## sakoe utilities execpath

### Synopsis

Returns the pro executable full path.

### Usage

`sakoe utilities execpath [options]`

### Options

| Option | Description |
| --- | --- |
| -c, --char-mode | Specify to return the path of the character mode executable. |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe utilities propathfromini

### Synopsis

Returns PROPATH value found in a .ini file.

### Usage

`sakoe utilities propathfromini <.ini path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<.ini path\> | The file path to the .ini file to read. |

### Options

| Option | Description |
| --- | --- |
| -rd, --base-directory | The base directory to use to convert to absolute path. Default to current directory. |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

This command returns only absolute path.
Relative path are converted to absolute using the command folder option.
It returns only existing directories or .pl files.
It also expands environment variables like %TEMP% or $DLC.

## sakoe utilities version

### Synopsis

Returns the version found for the Openedge installation.

### Usage

`sakoe utilities version [options]`

### Options

| Option | Description |
| --- | --- |
| -dl, --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe version

### Synopsis

Show the version information of this tool.

### Usage

`sakoe version [options]`

### Options

| Option | Description |
| --- | --- |
| -b, --bare | Only output the version, no logo. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe xcode

### Synopsis

Encrypt and decrypt files using the XCODE utility algorithm.

### Usage

`sakoe xcode [options] [command]`

### Options

| Option | Description |
| --- | --- |
| -?, -h, --help | Show this help text. |

### Commands

| Command | Description |
| --- | --- |
| decrypt | Decrypt files using the XCODE algorithm. Output the list of processed files. |
| encrypt | Encrypt files using the XCODE algorithm. Output the list of processed files. |
| list | List only encrypted (or decrypted) files. |

### Description

About XCODE:
  The original idea of the XCODE utility is to obfuscate your Openedge code before making it available.
  This is an encryption feature which uses a ASCII key/password of a maximum of 8 characters.
  The original XCODE utility uses the default key "Progress" if no custom key is supplied (so does this command).
  The encryption process does not use a standard cryptography method, it uses a 16-bits CRC inside a custom algorithm.

## sakoe xcode decrypt

### Synopsis

Decrypt files using the XCODE algorithm. Output the list of processed files.

### Usage

`sakoe xcode decrypt <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The file to process or a directory with files to process. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -k, --key | The encryption key to use for the process. Defaults to "Progress". |
| -su, --suffix | A suffix to append to each filename processed. |
| -od, --output-directory | Output all processed file in this common directory. |
| -f, --file <path> | (Can be used multiple times) File that should be added to the listing. Can be used multiple times. |
| -d, --directory <path> | (Can be used multiple times) Directory containing files that should be added to the listing. Can be used multiple times. |
| -r, --recursive | Recursive listing in the directories. |
| -i, --include <filter> | Include filter for directory listing. Can use wildcards (**,*,?). |
| -e, --exclude <filter> | Exclude filter for directory listing. Can use wildcards (**,*,?). |
| -ir, --include-regex <filter> | Regular expression include filter for directory listing. |
| -er, --exclude-regex <filter> | Regular expression include filter for directory listing.. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe xcode encrypt

### Synopsis

Encrypt files using the XCODE algorithm. Output the list of processed files.

### Usage

`sakoe xcode encrypt <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The file to process or a directory with files to process. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -k, --key | The encryption key to use for the process. Defaults to "Progress". |
| -su, --suffix | A suffix to append to each filename processed. |
| -od, --output-directory | Output all processed file in this common directory. |
| -f, --file <path> | (Can be used multiple times) File that should be added to the listing. Can be used multiple times. |
| -d, --directory <path> | (Can be used multiple times) Directory containing files that should be added to the listing. Can be used multiple times. |
| -r, --recursive | Recursive listing in the directories. |
| -i, --include <filter> | Include filter for directory listing. Can use wildcards (**,*,?). |
| -e, --exclude <filter> | Exclude filter for directory listing. Can use wildcards (**,*,?). |
| -ir, --include-regex <filter> | Regular expression include filter for directory listing. |
| -er, --exclude-regex <filter> | Regular expression include filter for directory listing.. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

## sakoe xcode list

### Synopsis

List only encrypted (or decrypted) files.

### Usage

`sakoe xcode list <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The file to process or a directory with files to process. Defaults to the current directory. |

### Options

| Option | Description |
| --- | --- |
| -de, --decrypted | List all decrypted files (or default to listing encrypted files). |
| -f, --file <path> | (Can be used multiple times) File that should be added to the listing. Can be used multiple times. |
| -d, --directory <path> | (Can be used multiple times) Directory containing files that should be added to the listing. Can be used multiple times. |
| -r, --recursive | Recursive listing in the directories. |
| -i, --include <filter> | Include filter for directory listing. Can use wildcards (**,*,?). |
| -e, --exclude <filter> | Exclude filter for directory listing. Can use wildcards (**,*,?). |
| -ir, --include-regex <filter> | Regular expression include filter for directory listing. |
| -er, --exclude-regex <filter> | Regular expression include filter for directory listing.. |
| -vb, --verbosity <level> | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to 'none'. Specifying this option without a level value sets the verbosity to 'debug'. Not specifying the option defaults to 'info'. |
| -pm, --progress-mode <mode> | Sets the display mode of progress bars. Specify 'off' to hide progress bars and 'stay' to make them persistent. Defaults to 'on', which show progress bars but hide them when done. |
| -do, --debug-output <file> | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file 'sakoe.log'. |
| -wl, --with-logo | Always show the logo on start. |
| -?, -h, --help | Show this help text. |

### Description

Examples:

  List only the encrypted files in a list of files in argument.
    sakoe xcode list -r -vb none -nop
  Get a raw list of all the encrypted files in the current directory (recursive).
    sakoe xcode list -r -vb none -nop
