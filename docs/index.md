# SAKOE

> This markdown can be generated using the command: `sakoe manual markdown-file`.

## ABOUT



### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, don't be afraid to use the `--help` option.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value").
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. "my ""special"" value").
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.

Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.

  `sakoe @responsefile.txt`

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

If you want to help, you are welcome to contribute to the github repo.
You are invited to STAR the project on github to increase its visibility!

## TABLE OF CONTENT

- [Commands overview](#commands-overview)
- [About the build command](#about-the-build-command)
- [build](#sakoe-build)
- [database](#sakoe-database)
  - [connect](#sakoe-database-connect)
  - [copy](#sakoe-database-copy)
  - [create](#sakoe-database-create)
  - [delete](#sakoe-database-delete)
  - [dump](#sakoe-database-dump)
    - [data](#sakoe-database-dump-data)
    - [incremental](#sakoe-database-dump-incremental)
    - [incremental-df](#sakoe-database-dump-incremental-df)
    - [schema](#sakoe-database-dump-schema)
    - [seq](#sakoe-database-dump-seq)
  - [kill](#sakoe-database-kill)
  - [load](#sakoe-database-load)
    - [data](#sakoe-database-load-data)
    - [schema](#sakoe-database-load-schema)
    - [seq](#sakoe-database-load-seq)
  - [repair](#sakoe-database-repair)
  - [start](#sakoe-database-start)
  - [stop](#sakoe-database-stop)
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
- [update](#sakoe-update)
- [utilities](#sakoe-utilities)
  - [connectstr](#sakoe-utilities-connectstr)
  - [execpath](#sakoe-utilities-execpath)
  - [propathfromini](#sakoe-utilities-propathfromini)
  - [version](#sakoe-utilities-version)
- [version](#sakoe-version)

## COMMANDS OVERVIEW

| Full command line | Short description |
| --- | --- |
| [sakoe build](#sakoe-build) | Build automation for Openedge projects. This command is the bread and butter of this tool. |
| [sakoe database](#sakoe-database) | Database manipulation tools. |
| [sakoe database connect](#sakoe-database-connect) | Get the connection string to use to connect to a database. |
| [sakoe database copy](#sakoe-database-copy) | Copy a database. |
| [sakoe database create](#sakoe-database-create) | Creates a new database. |
| [sakoe database delete](#sakoe-database-delete) | Delete a database. |
| [sakoe database dump](#sakoe-database-dump) | Dump data or schema definition from a database. |
| [sakoe database dump data](#sakoe-database-dump-data) | Dump the database data in plain text files (.d). |
| [sakoe database dump incremental](#sakoe-database-dump-incremental) | Dump an incremental schema definition (.df) allowing to update a database schema definition. |
| [sakoe database dump incremental-df](#sakoe-database-dump-incremental-df) | Dump an incremental schema definition (.df) by comparing .df files. |
| [sakoe database dump schema](#sakoe-database-dump-schema) | Dump the schema definition (.df) of a database. |
| [sakoe database dump seq](#sakoe-database-dump-seq) | Dump the database sequence values in a plain text file (.d). |
| [sakoe database kill](#sakoe-database-kill) | Kill all the broker processes running on this machine. |
| [sakoe database load](#sakoe-database-load) | Load data or schema definition to a database. |
| [sakoe database load data](#sakoe-database-load-data) | Load the database data from plain text files (.d). |
| [sakoe database load schema](#sakoe-database-load-schema) | Load the schema definition (.df) to a database. |
| [sakoe database load seq](#sakoe-database-load-seq) | Load the database sequence values from a plain text file (.d). |
| [sakoe database repair](#sakoe-database-repair) | Repair the structure of a database. |
| [sakoe database start](#sakoe-database-start) | Start a database in order to use it in multi-users mode. |
| [sakoe database stop](#sakoe-database-stop) | Stop a database that was started for multi-users. |
| [sakoe manual](#sakoe-manual) | The manual of this tool. Learn about its usage and about key concepts of sakoe. |
| [sakoe prohelp](#sakoe-prohelp) | Access the Openedge help. |
| [sakoe prohelp chm](#sakoe-prohelp-chm) | Opens a .chm file (windows help file) present in $DLC/prohelp. |
| [sakoe prohelp keyword](#sakoe-prohelp-keyword) | Look for help on the given Openedge keyword in the language windows help file. |
| [sakoe prohelp listchm](#sakoe-prohelp-listchm) | List all the .chm files (windows help files) available in $DLC/prohelp. |
| [sakoe prohelp promsg](#sakoe-prohelp-promsg) | Displays the extended error message using an error number. |
| [sakoe project](#sakoe-project) | Commands related to an Openedge project (.oe directory). |
| [sakoe project gitignore](#sakoe-project-gitignore) | Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists). |
| [sakoe project init](#sakoe-project-init) | Initialize a new Openedge project file (.oeproj.xml). |
| [sakoe project list](#sakoe-project-list) | List all the project files or list the build configurations in a project file. |
| [sakoe update](#sakoe-update) | Update this tool with the latest release found on github. |
| [sakoe utilities](#sakoe-utilities) | Miscellaneous utility commands. |
| [sakoe utilities connectstr](#sakoe-utilities-connectstr) | Returns a single line connection string from a .pf file. |
| [sakoe utilities execpath](#sakoe-utilities-execpath) | Returns the pro executable full path. |
| [sakoe utilities propathfromini](#sakoe-utilities-propathfromini) | Returns PROPATH value found in a .ini file. |
| [sakoe utilities version](#sakoe-utilities-version) | Returns the version found for the Openedge installation. |
| [sakoe version](#sakoe-version) | Show the version information of this tool. |

## ABOUT THE BUILD COMMAND



### Overview

With sakoe, you can 'build' your application. A build process is a succession of tasks that (typically) transform your source files into a deliverable format, usually called a release or package.

In sakoe, you describe a build process using a 'build configuration'. A build configuration holds 'properties' of the build (for instance, the path to the openedge installation directory $DLC). It also holds the list of 'tasks' that will be executed successively during the build process.

To illustrate this, here is a possible build process:
  - Task 1: compile all your .p files to a `procedures` directory.
  - Task 2: compile all your .w files into a pro-library `client.pl`.
  - Task 3: zip the `procedures` and `client.pl` together into an archive file `release.zip`.

In order to store these build configurations, sakoe uses project files: `.oeproj.xml`.
You can create them with the command: `sakoe project init`.

By default, a build is 'incremental'.
An incremental build is the opposite of a full build. In incremental mode, only the files that were added/modified/deleted since the previous build are taken into account. Unchanged files are simply not rebuilt.

The chapters below contain more details about a project, build configuration, properties and tasks. 

### Project

An Openedge project (typically, your project is your application, they are synonyms).

Some facts:
  - A project is composed of one or several build configurations.
  - A build configuration describe how to build your project.
  - To build your project, you need to specify which build configuration to use.
            
Below is a simplified representation of a project file:
  Project
  - Build configuration 1
    - Child build configuration 2
      - Child build configuration 3
      - Child build configuration x...
    - Child build configuration x...
  - Build configuration x...

There are no upper limit for build configuration nesting.
The idea is that nested build configuration inherit properties from their parent, recursively (more details on the build configuration help).

### Build configuration

A list of build configurations for this project.

Some facts:
  - A build configuration describe how to build your application.
  - A build configuration is essentially a succession of tasks (grouped into steps) that should be carried on in a sequential manner to build your application.
  - Each build configuration also has properties, which are used to customize the build.
  - Each build configuration can also have variables that make your build process dynamic. You can use variables anywhere in this xml. Their value can be changed dynamically without modifying this xml when running the build.
            
Below is a simplified representation of a build configuration:
  Build configuration
  - Variables
  - Properties
  - Build steps
    - Step 1
      - Task 1
      - Task x...
    - Step x...
  - List of children build configuration
  - Child build configuration 1
  - Child build configuration x...

Inheritance of build configurations (think of russian dolls):
  - Each build configuration can define "children" build configurations.
  - Each child inherits its parent properties following these rules:
  - For a given property, the most nested value is used (i.e. child value prioritized over parent value).
  - If the xml element is a list, the elements of the child (if any) will be added to the elements of the parent (if any). For instance, the steps list has this behavior. This means that the steps of a child will be added to the defined steps of its parents.
            
Practical example:
  Given the project below:
 
  Project
  - Build configuration 1
     - Properties
          Property1 (x)
       - Property2
     - Steps
       - Step1 (x)
     - Child build configuration 2
        - Properties
          - Property2 (x)
          - Property3 (x)
        - Steps
           - Step2 (x)
            
  We decide to build 'Child build configuration 2':
  - Property1 of 'Build configuration 1' will be used.
  - Property2 and Property 3 of 'Build configuration 2' will be used.
  - Two steps will be executed: Step1 from 'Build configuration 1' and Step2 from 'Build configuration 2'.

### Build configuration variables

The variables of this build configurations.

Some facts:
  - Variables make your build process dynamic by allowing you to change build options without having to modify this xml.
  - You can use a variable with the syntax {{variable_name}}.
  - Variables will be replaced by their value at run time.
  - If the variable exists as an environment variable, its value will be taken in priority (this allows to overload values using environment variables).
  - Non existing variables will be replaced by an empty string.
  - Variables can be used in any "string type" value (this exclude numbers/booleans).
  - Variables can be used in the build configuration properties and also in the build tasks.
  - You can use variables in the variables definition, simply define them in the correct order.
  - If several variables with the same name exist, the value of the latest defined is used.
  - As this is a list of variables, child configuration will inherit variables from their parents but variable values defined in children prevail.
  - Variable names are case insensitive.
            
Special variables are already defined and available to use:
  - {{SOURCE_DIRECTORY}} the application source directory (defined in properties).
  - {{PROJECT_DIRECTORY}} the project directory ({{SOURCE_DIRECTORY}}/.oe).
  - {{PROJECT_LOCAL_DIRECTORY}} the project local directory ({{SOURCE_DIRECTORY}}/.oe/local).
  - {{DLC}} the dlc path used for the current build.
  - {{OUTPUT_DIRECTORY}} the build output directory (default to {{SOURCE_DIRECTORY}}/.oe/bin).
  - {{CONFIGURATION_NAME}} the build configuration name for the current build.
  - {{CURRENT_DIRECTORY}} the current directory.

### Build configuration properties

The properties of this build configuration.

Some facts:
  - Properties can describe your application (for instance, the database needed to compile).
  - Properties can also describe options to build your application (for instance, if the compilation should also generate the xref files).
  - Properties are inherited from parent build configuration (if any).
  - For instance, this allows to define a DLC (v11) path for the project to use as default, and have a child configuration that overload this value using another DLC (v9) path.

### Build steps

A list of steps to build your application.

Some facts:
  - Each step contains a list of tasks.
  - The sequential execution of these steps/tasks is called a build.
  - Steps (and tasks within them) are executed sequentially in the order they were defined.
  - A child configuration inherit the steps of its parent: they are executed before its own steps.
            
There are 3 kinds of steps:
  - 'BuildSource' with tasks that will handle the files in your source directory (for instance, to compile procedures or copy configuration files).
  - 'BuildOutput' with tasks that will handle the files in the build output directory (for instance, to create a .zip release of compiled files).
  - 'Free' with tasks that will handle files outside of your source or output directory (for instance, to download dependencies before the compilation).


## SAKOE BUILD

### Synopsis

Build automation for Openedge projects. This command is the bread and butter of this tool.

### Usage

`sakoe build [<project>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<project\> | Path or name of the project file. The .oeproj.xml extension is optional. Defaults to the first .oeproj.xml file found. The search is done in the current directory and in the .oe directory when it exists. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | --config-name | The name of the build configuration to use for the build. This name is found in the .oeproj.xml file. Defaults to the first build configuration found in the project file. |
| -e | --extra-config | (Can be used multiple times) In addition to the base build configuration specified by <project> and --config-name, you can dynamically add a child configuration to the base configuration with this option. This option can be used multiple times, each new configuration will be added as a child of the previously defined configuration. This option allows you to share, with your colleagues, a common project file that holds the property of your application and have an extra configuration in local (just for you) which you can use to build the project in a specific local directory. For each extra configuration, specify the path or the name of the project file and the configuration name to use. If the project file name if empty, the main <project> is used. |
| -p | --property | (Can be used multiple times) A pair of key/value to dynamically set a property for this build. The value set this way will prevail over the value defined in a project file. Each pair should specify the name of the property to set and the value that should be used. Use the option --property-help to see the full list of properties available as well as their documentation. |
| -v | --variable | (Can be used multiple times) A pair of key/value to dynamically set a variable for this build. A variable set this way will prevail over a variable with the same name defined in a project file. Each pair should specify the name of the variable to set and the value that should be used. |
| -ph | --property-help | Shows the list of each build property usable with its full documentation. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Build properties

* -p "AddAllSourceDirectoriesToPropath=True"
* -p "AddDefaultOpenedgePropath=True"
* -p "AllowDatabaseShutdownByProcessKill=True"
* -p "AppendMaxConnectionTryToConnectionString=True"
* -p "BuildConfigurationExportFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.oeproj.xml"
* -p "BuildHistoryInputFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.xml"
* -p "BuildHistoryOutputFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.xml"
* -p "CompilableFileExtensionPattern=*.p;*.w;*.t;*.cls"
* -p "CompileOptions="
* -p "CompileStatementExtraOptions="
* -p "CompileWithDebugList=False"
* -p "CompileWithListing=False"
* -p "CompileWithPreprocess=False"
* -p "CompileWithXmlXref=False"
* -p "CompileWithXref=False"
* -p "CurrentBranchName="
* -p "CurrentBranchOriginCommit="
* -p "DlcDirectoryPath=$DLC (openedge installation directory)"
* -p "EnabledIncrementalBuild=True"
* -p "ExtraDatabaseConnectionString="
* -p "ExtraOpenedgeCommandLineParameters="
* -p "ForceSingleProcess=False"
* -p "FullRebuild=False"
* -p "IncludeSourceFilesCommittedOnlyOnCurrentBranch=False"
* -p "IncludeSourceFilesModifiedSinceLastCommit=False"
* -p "IniFilePath="
* -p "MinimumNumberOfFilesPerProcess=10"
* -p "NumberProcessPerCore=1"
* -p "OpenedgeCodePage="
* -p "OpenedgeTemporaryDirectoryPath=$TEMP/.oe_tmp/xxx (temporary folder)"
* -p "OutputDirectoryPath={{SOURCE_DIRECTORY}}\bin"
* -p "ProcedurePathToExecuteAfterAnyProgressExecution="
* -p "ProcedureToExecuteBeforeAnyProgressExecutionFilePath="
* -p "PropathExclude="
* -p "PropathExcludeHiddenDirectories=False"
* -p "PropathExcludeRegex="
* -p "PropathExtraVcsPatternExclusion=.git**;.svn**;.oe**"
* -p "PropathInclude="
* -p "PropathIncludeRegex="
* -p "PropathRecursiveListing=True"
* -p "RebuildFilesWithCompilationErrors=True"
* -p "RebuildFilesWithMissingTargets=False"
* -p "RebuildFilesWithNewTargets=False"
* -p "ReportHtmlFilePath={{PROJECT_LOCAL_DIRECTORY}}\build\latest.html"
* -p "ShutdownCompilationDatabasesAfterBuild=True"
* -p "SourceDirectoryPath=$PWD (current directory)"
* -p "SourceExclude="
* -p "SourceExcludeHiddenDirectories=False"
* -p "SourceExcludeRegex="
* -p "SourceExtraVcsPatternExclusion=.git**;.svn**;.oe**"
* -p "SourceInclude="
* -p "SourceIncludeRegex="
* -p "SourceOverrideFileList="
* -p "SourceRecursiveListing=True"
* -p "StopBuildOnCompilationError=True"
* -p "StopBuildOnCompilationWarning=False"
* -p "StopBuildOnTaskError=True"
* -p "StopBuildOnTaskWarning=False"
* -p "TestMode=False"
* -p "TryToHideProcessFromTaskBarOnWindows=True"
* -p "TryToOptimizeCompilationDirectory=True"
* -p "UseCharacterModeExecutable=False"
* -p "UseCheckSumComparison=False"
* -p "UseCompilerMultiCompile=False"
* -p "UseSimplerAnalysisForDatabaseReference=False"

> Display the full documentation of each build property by running `sakoe build --property-help`.

### Extra config

The example below illustrate the usage of the --extra-config option:
We have 2 project files (p1 and p2) containing build configurations as described below:

p1.oeproj.xml
- Configuration1
- Configuration2

p2.oeproj.xml
- Configuration3
   - Configuration4
     - Configuration5
     - Configuration6
   - Configuration7

We use the following command line to start a build:

`sakoe build p1 --config-name Configuration1 --extra-config p2=Configuration6 --extra-config Configuration2`

Below is the equivalent of how build configurations are nested in this scenario:

Configuration1
- Configuration3
   - Configuration4
      - Configuration6
         - Configuration2

This allow a lot of flexibility for organizing and partitioning your build process.

### Notes

Create a new project file using the command: `sakoe project init`.
Get a more in-depth help and learn about the concept of a build (in sakoe) using the command: `sakoe manual build`.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE

### Synopsis

Database manipulation tools.

### Usage

`sakoe database [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| co | connect | Get the connection string to use to connect to a database. |
| cp | copy | Copy a database. |
| cr | create | Creates a new database. |
| de | delete | Delete a database. |
| du | dump | Dump data or schema definition from a database. |
| ki | kill | Kill all the broker processes running on this machine. |
| lo | load | Load data or schema definition to a database. |
| re | repair | Repair the structure of a database. |
| proserve | start | Start a database in order to use it in multi-users mode. |
| proshut | stop | Stop a database that was started for multi-users. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE CONNECT

### Synopsis

Get the connection string to use to connect to a database.

### Usage

`sakoe database connect [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -l | --logical-name | The logical name to use for the database in the connection string. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE COPY

### Synopsis

Copy a database.

### Usage

`sakoe database copy <source db path> <target db path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<source db path\> | The path to the 'source' database. The .db extension is optional. |
| \<target db path\> | The path to the 'target' database. The .db extension is optional. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -ni | --newinstance | Use -newinstance in the procopy command. |
| -rp | --relativepath | Use -relativepath in the procopy command. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE CREATE

### Synopsis

Creates a new database.

### Usage

`sakoe database create <db path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database to create. The .db extension is optional. Defaults to the name of the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -df | --df | Path to the .df file containing the database schema definition. |
| -st | --st | Path to the .st file containing the database physical structure. |
| -bs | --blocksize | The blocksize to use when creating the database. |
| -cp | --codepage | Existing codepage in the openedge installation $DLC/prolang/(codepage). |
| -ni | --newinstance | Use -newinstance in the procopy command. |
| -rp | --relativepath | Use -relativepath in the procopy command. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DELETE

### Synopsis

Delete a database.

### Usage

`sakoe database delete [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

All the files composing the database are deleted without confirmation.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP

### Synopsis

Dump data or schema definition from a database.

### Usage

`sakoe database dump [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| da | data | Dump the database data in plain text files (.d). |
| inc | incremental | Dump an incremental schema definition (.df) allowing to update a database schema definition. |
| id | incremental-df | Dump an incremental schema definition (.df) by comparing .df files. |
| df | schema | Dump the schema definition (.df) of a database. |
| se | seq | Dump the database sequence values in a plain text file (.d). |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP DATA

### Synopsis

Dump the database data in plain text files (.d).

### Usage

`sakoe database dump data [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | --dump-directory | Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .d file named like the table. |
| -t | --table | A list of comma separated table names to dump. Defaults to `ALL` which dumps every table. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP INCREMENTAL

### Synopsis

Dump an incremental schema definition (.df) allowing to update a database schema definition.

### Usage

`sakoe database dump incremental <old db path> <new db path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<old db path\> | The path to the 'old' database. The .db extension is optional. |
| \<new db path\> | The path to the 'new' database. The .db extension is optional. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -df | --df | Path to the output incremental .df file. |
| -rf | --rename-file | Path to the 'rename file'. It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.  The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT): - T,old-table-name,new-table-name - F,table-name,old-field-name,new-field-name - S,old-sequence-name,new-sequence-name  Missing entries or entries with an empty new name are considered to have been deleted.  |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

Two databases schema definition are compared and the difference is written in a 'delta' `.df`. This `.df` file then allows to upgrade an older database schema to the new schema.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP INCREMENTAL-DF

### Synopsis

Dump an incremental schema definition (.df) by comparing .df files.

### Usage

`sakoe database dump incremental-df <old df path> <new df path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<old df path\> | The path to the 'old' database. |
| \<new df path\> | The path to the 'new' database. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -df | --df | Path to the output incremental .df file. |
| -rf | --rename-file | Path to the 'rename file'. It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.  The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT): - T,old-table-name,new-table-name - F,table-name,old-field-name,new-field-name - S,old-sequence-name,new-sequence-name  Missing entries or entries with an empty new name are considered to have been deleted.  |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP SCHEMA

### Synopsis

Dump the schema definition (.df) of a database.

### Usage

`sakoe database dump schema [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -df | --df | Path to the output .df file that will contain the schema definition of the database. |
| -t | --table | A list of comma separated table names for which we need a schema definition dump. Defaults to `ALL` which dumps every table and sequence. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DUMP SEQ

### Synopsis

Dump the database sequence values in a plain text file (.d).

### Usage

`sakoe database dump seq [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | --dump-file | File path that will contain the data dumped. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE KILL

### Synopsis

Kill all the broker processes running on this machine.

### Usage

`sakoe database kill [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

A broker process is generally named: `_mprosrv`.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOAD

### Synopsis

Load data or schema definition to a database.

### Usage

`sakoe database load [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| da | data | Load the database data from plain text files (.d). |
| df | schema | Load the schema definition (.df) to a database. |
| se | seq | Load the database sequence values from a plain text file (.d). |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOAD DATA

### Synopsis

Load the database data from plain text files (.d).

### Usage

`sakoe database load data [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | --data-directory | Directory path that contain the data to load. Each table of the database should be stored as an individual .d file named like the table. |
| -t | --table | TA list of comma separated table names to load. Defaults to `ALL` which loads every table. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOAD SCHEMA

### Synopsis

Load the schema definition (.df) to a database.

### Usage

`sakoe database load schema [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -df | --df | Path to the .df file that contains the schema definition (or partial schema) of the database. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOAD SEQ

### Synopsis

Load the database sequence values from a plain text file (.d).

### Usage

`sakoe database load seq [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | --data-file | File path that contains the sequence data to load. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE REPAIR

### Synopsis

Repair the structure of a database.

### Usage

`sakoe database repair [<db path>] [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

Update the database control information, usually done after an extent has been moved or renamed.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE START

### Synopsis

Start a database in order to use it in multi-users mode.

### Usage

`sakoe database start [<db path>] [options] [[--] <extra proserve parameters>...]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -s | --service | Service name that will be used by this database. Usually a port number or a service name declared in /etc/services. |
| -h | --hostname | The hostname on which to start the database. Defaults to the current machine. |
| -np | --next-port | Port number, the next available port after this number will be used to start the database. |
| -nu | --nb-users | Number of users that should be able to connect to this database simultaneously. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STOP

### Synopsis

Stop a database that was started for multi-users.

### Usage

`sakoe database stop [<db path>] [options] [[--] <extra proshut parameters>...]`

### Arguments

| Argument | Description |
| --- | --- |
| \<db path\> | Path to the database. The .db extension is optional. Defaults to the first .db file found in the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE MANUAL

### Synopsis

The manual of this tool. Learn about its usage and about key concepts of sakoe.

### Usage

`sakoe manual [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, don't be afraid to use the `--help` option.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value").
  - If you need to use a double quote within a double quote, you can do so by double the double quote (i.e. "my ""special"" value").
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.

Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.

  `sakoe @responsefile.txt`

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kind of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

If you want to help, you are welcome to contribute to the github repo.
You are invited to STAR the project on github to increase its visibility!


**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROHELP

### Synopsis

Access the Openedge help.

### Usage

`sakoe prohelp [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| ch | chm | Opens a .chm file (windows help file) present in $DLC/prohelp. |
| ke | keyword | Look for help on the given Openedge keyword in the language windows help file. |
| li | listchm | List all the .chm files (windows help files) available in $DLC/prohelp. |
| pm | promsg | Displays the extended error message using an error number. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROHELP CHM

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

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROHELP KEYWORD

### Synopsis

Look for help on the given Openedge keyword in the language windows help file.

### Usage

`sakoe prohelp keyword <keyword> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<keyword\> | The keyword you would like to find in the help. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROHELP LISTCHM

### Synopsis

List all the .chm files (windows help files) available in $DLC/prohelp.

### Usage

`sakoe prohelp listchm [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROHELP PROMSG

### Synopsis

Displays the extended error message using an error number.

### Usage

`sakoe prohelp promsg <message number> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<message number\> | The number of the error message to show. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

This command uses the content of files located in $DLC/prohelp/msgdata to display information.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT

### Synopsis

Commands related to an Openedge project (.oe directory).

### Usage

`sakoe project [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| gi | gitignore | Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists). |
| in | init | Initialize a new Openedge project file (.oeproj.xml). |
| li | list | List all the project files or list the build configurations in a project file. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT GITIGNORE

### Synopsis

Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists).

### Usage

`sakoe project gitignore <directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<directory\> | The repository base directory (source directory). Defaults to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT INIT

### Synopsis

Initialize a new Openedge project file (.oeproj.xml).

### Usage

`sakoe project init <directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<directory\> | The directory in which to initialize the project. Defaults to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -p | --project-name | The name of the project to create. Defaults to the current directory name. |
| -l | --local | Create the new project file for local use only. A local project should contain build configurations specific to your machine and thus should not be shared or versioned in your source control system. |
| -f | --force | Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT LIST

### Synopsis

List all the project files or list the build configurations in a project file.

### Usage

`sakoe project list <file or directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file or directory\> | The project file in which to list the build configurations or the project base directory (source directory) in which to list the project files. Defaults to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UPDATE

### Synopsis

Update this tool with the latest release found on github.

### Usage

`sakoe update [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | --get-pre-release | Accept to update from new pre-release (i.e. 'beta') versions of the tool. This option will be used by default if the current version of the tool is a pre-release version. Otherwise, only stable releases will be used for updates.  |
| -p | --proxy | The http proxy to use for this update. Useful if you are behind a corporate firewall. The expected format is: 'http(s)://[user:password@]host[:port]'. It is also possible to use the environment variable HTTP_PROXY to set this value. |
| -c | --check-only | Check for new releases but exit the command before actually updating the tool. |
| -u | --override-github-url | Use an alternative url for the github api. This option is here to allow updates from a different location (a private server for instance) but should not be used in most cases. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UTILITIES

### Synopsis

Miscellaneous utility commands.

### Usage

`sakoe utilities [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | --help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| co | connectstr | Returns a single line connection string from a .pf file. |
| ex | execpath | Returns the pro executable full path. |
| pr | propathfromini | Returns PROPATH value found in a .ini file. |
| ve | version | Returns the version found for the Openedge installation. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UTILITIES CONNECTSTR

### Synopsis

Returns a single line connection string from a .pf file.

### Usage

`sakoe utilities connectstr <.pf path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<.pf path\> | The file path to the parameter file (.pf) to use. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

- This command will skip unnecessary whitespaces and new lines.
- This command will ignore comment lines starting with #.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UTILITIES EXECPATH

### Synopsis

Returns the pro executable full path.

### Usage

`sakoe utilities execpath [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | --char-mode | Specify to return the path of the character mode executable. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UTILITIES PROPATHFROMINI

### Synopsis

Returns PROPATH value found in a .ini file.

### Usage

`sakoe utilities propathfromini <.ini path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<.ini path\> | The file path to the .ini file to read. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | --base-directory | The base directory to use to convert to absolute path. Default to current directory. |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

### Description

- This command returns only absolute path.
- Relative path are converted to absolute using the command folder option.
- This command returns only existing directories or .pl files.
- This command expands environment variables like %TEMP% or $DLC.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UTILITIES VERSION

### Synopsis

Returns the version found for the Openedge installation.

### Usage

`sakoe utilities version [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | --dlc | The path to the directory containing the Openedge installation. Will default to the path found in the DLC environment variable if it exists. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE VERSION

### Synopsis

Show the version information of this tool.

### Usage

`sakoe version [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | --bare-version | Output the raw assembly version of the tool, without logo and pre-release tag. |
| -vb | --verbosity | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. |
| -pm | --progress-mode | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -do | --debug-output | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | --with-logo | Always show the logo on start. |
| -h | --help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**