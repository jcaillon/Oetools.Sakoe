# SAKOE

> This markdown can be generated using the command: `sakoe manual export-md`.
> 
> This version has been generated on 19-04-26 at 19:02:13.

## ABOUT



### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, don't be afraid to use the `--help` option.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value").
  - If you need to use a double quote within a double quote, you can do so by double quoting the double quotes (i.e. "my ""special"" value").
  - If an extra layer is needed, just double the doubling (i.e. -opt "-mysubopt ""my special """"value""""""").
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.

Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.

  `sakoe @responsefile.txt`

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kinds of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

You are invited to STAR the project on github to increase its visibility!

## TABLE OF CONTENT

- [Commands overview](#commands-overview)
- [About the build command](#about-the-build-command)
- [database](#sakoe-database)
  - [admin](#sakoe-database-admin)
    - [run](#sakoe-database-admin-run)
  - [analysis](#sakoe-database-analysis)
    - [advise](#sakoe-database-analysis-advise)
    - [report](#sakoe-database-analysis-report)
  - [backup](#sakoe-database-backup)
  - [before-image](#sakoe-database-before-image)
    - [truncate](#sakoe-database-before-image-truncate)
  - [busy](#sakoe-database-busy)
  - [check](#sakoe-database-check)
  - [connect](#sakoe-database-connect)
  - [copy](#sakoe-database-copy)
  - [create](#sakoe-database-create)
  - [data](#sakoe-database-data)
    - [dump](#sakoe-database-data-dump)
    - [dump-binary](#sakoe-database-data-dump-binary)
    - [dump-sequence](#sakoe-database-data-dump-sequence)
    - [dump-sql](#sakoe-database-data-dump-sql)
    - [load](#sakoe-database-data-load)
    - [load-binary](#sakoe-database-data-load-binary)
    - [load-bulk](#sakoe-database-data-load-bulk)
    - [load-sequence](#sakoe-database-data-load-sequence)
    - [load-sql](#sakoe-database-data-load-sql)
    - [truncate](#sakoe-database-data-truncate)
  - [datadigger](#sakoe-database-datadigger)
    - [install](#sakoe-database-datadigger-install)
    - [remove](#sakoe-database-datadigger-remove)
    - [run](#sakoe-database-datadigger-run)
  - [delete](#sakoe-database-delete)
  - [dictionary](#sakoe-database-dictionary)
    - [run](#sakoe-database-dictionary-run)
  - [index](#sakoe-database-index)
    - [rebuild](#sakoe-database-index-rebuild)
  - [kill](#sakoe-database-kill)
  - [kill-all](#sakoe-database-kill-all)
  - [log](#sakoe-database-log)
    - [truncate](#sakoe-database-log-truncate)
  - [project](#sakoe-database-project)
  - [repair](#sakoe-database-repair)
  - [restore](#sakoe-database-restore)
  - [schema](#sakoe-database-schema)
    - [dump](#sakoe-database-schema-dump)
    - [dump-custom](#sakoe-database-schema-dump-custom)
    - [dump-inc](#sakoe-database-schema-dump-inc)
    - [dump-inc-df](#sakoe-database-schema-dump-inc-df)
    - [dump-sql](#sakoe-database-schema-dump-sql)
    - [load](#sakoe-database-schema-load)
  - [start](#sakoe-database-start)
  - [stop](#sakoe-database-stop)
  - [structure](#sakoe-database-structure)
    - [add](#sakoe-database-structure-add)
    - [generate](#sakoe-database-structure-generate)
    - [remove](#sakoe-database-structure-remove)
    - [update](#sakoe-database-structure-update)
    - [validate](#sakoe-database-structure-validate)
- [manual](#sakoe-manual)
- [progress](#sakoe-progress)
  - [exe-path](#sakoe-progress-exe-path)
  - [generate-ds](#sakoe-progress-generate-ds)
  - [help](#sakoe-progress-help)
    - [list](#sakoe-progress-help-list)
    - [message](#sakoe-progress-help-message)
    - [open](#sakoe-progress-help-open)
    - [search](#sakoe-progress-help-search)
  - [read-ini](#sakoe-progress-read-ini)
  - [read-pf](#sakoe-progress-read-pf)
  - [version](#sakoe-progress-version)
  - [wsdl-doc](#sakoe-progress-wsdl-doc)
- [project](#sakoe-project)
  - [build](#sakoe-project-build)
  - [git-ignore](#sakoe-project-git-ignore)
  - [init](#sakoe-project-init)
  - [list](#sakoe-project-list)
  - [project](#sakoe-project-project)
    - [create](#sakoe-project-project-create)
  - [update](#sakoe-project-update)
- [tool](#sakoe-tool)
  - [datadigger](#sakoe-tool-datadigger)
    - [install](#sakoe-tool-datadigger-install)
    - [remove](#sakoe-tool-datadigger-remove)
    - [run](#sakoe-tool-datadigger-run)
- [update](#sakoe-update)
- [version](#sakoe-version)

## COMMANDS OVERVIEW

| Full command line | Short description |
| --- | --- |
| [sakoe database](#sakoe-database) | Database manipulation tools. |
| [sakoe database admin](#sakoe-database-admin) | The openedge database administration tool. |
| [sakoe database admin run](#sakoe-database-admin-run) | Run an instance of the database administration tool. |
| [sakoe database analysis](#sakoe-database-analysis) | Regroup commands related to database analysis. |
| [sakoe database analysis advise](#sakoe-database-analysis-advise) | Generates an html report which provides pointers to common database configuration issues. |
| [sakoe database analysis report](#sakoe-database-analysis-report) | Displays an analysis report. |
| [sakoe database backup](#sakoe-database-backup) | Backup a database into a single file for a future restore. |
| [sakoe database before-image](#sakoe-database-before-image) | Operate on database before-image file. |
| [sakoe database before-image truncate](#sakoe-database-before-image-truncate) | Truncate the before-image of a database. |
| [sakoe database busy](#sakoe-database-busy) | Check the usage mode of a database (busy mode). |
| [sakoe database check](#sakoe-database-check) | Check if a database is alive. |
| [sakoe database connect](#sakoe-database-connect) | Get the connection string to use to connect to a database. |
| [sakoe database copy](#sakoe-database-copy) | Copy a database. |
| [sakoe database create](#sakoe-database-create) | Creates a new database. |
| [sakoe database data](#sakoe-database-data) | Operate on database data. |
| [sakoe database data dump](#sakoe-database-data-dump) | Dump the database data in plain text files (.d). |
| [sakoe database data dump-binary](#sakoe-database-data-dump-binary) | Dump data in binary format from a database. |
| [sakoe database data dump-sequence](#sakoe-database-data-dump-sequence) | Dump the database sequence data in a plain text file (.d). |
| [sakoe database data dump-sql](#sakoe-database-data-dump-sql) | Dump data in SQL-92 format from a database. |
| [sakoe database data load](#sakoe-database-data-load) | Load the database data from plain text files (.d). |
| [sakoe database data load-binary](#sakoe-database-data-load-binary) | Load the database data from binary format files. |
| [sakoe database data load-bulk](#sakoe-database-data-load-bulk) | Load the database data from plain text files (.d). |
| [sakoe database data load-sequence](#sakoe-database-data-load-sequence) | Load the database sequence data from a plain text file (.d). |
| [sakoe database data load-sql](#sakoe-database-data-load-sql) | Load the database data from SQL-92 format files (.dsql). |
| [sakoe database data truncate](#sakoe-database-data-truncate) | Deletes all the data of a given table. |
| [sakoe database datadigger](#sakoe-database-datadigger) | DataDigger is a tool for exploring and modifying the data of a database. |
| [sakoe database datadigger install](#sakoe-database-datadigger-install) | Install DataDigger in the default installation path. |
| [sakoe database datadigger remove](#sakoe-database-datadigger-remove) | Remove DataDigger from the installation path. |
| [sakoe database datadigger run](#sakoe-database-datadigger-run) | Run a new DataDigger instance. |
| [sakoe database delete](#sakoe-database-delete) | Delete a database. |
| [sakoe database dictionary](#sakoe-database-dictionary) | The openedge database dictionary tool. |
| [sakoe database dictionary run](#sakoe-database-dictionary-run) | Run an instance of the dictionary tool. |
| [sakoe database index](#sakoe-database-index) | Operate on database indexes. |
| [sakoe database index rebuild](#sakoe-database-index-rebuild) | Rebuild the indexes of a database. By default, rebuilds all the active indexes. |
| [sakoe database kill](#sakoe-database-kill) | Kill the broker/server processes running for a particular a database. |
| [sakoe database kill-all](#sakoe-database-kill-all) | Kill all the broker/server processes running on this machine. |
| [sakoe database log](#sakoe-database-log) | Operate on database log files (.lg). |
| [sakoe database log truncate](#sakoe-database-log-truncate) | Truncates the log file. |
| [sakoe database project](#sakoe-database-project) | Operates on databases belonging to a project. |
| [sakoe database repair](#sakoe-database-repair) | Repair/update the database control information file (.db). |
| [sakoe database restore](#sakoe-database-restore) | Restore a database from a backup file. |
| [sakoe database schema](#sakoe-database-schema) | Operate on database schema. |
| [sakoe database schema dump](#sakoe-database-schema-dump) | Dump the schema definition (.df) of a database. |
| [sakoe database schema dump-custom](#sakoe-database-schema-dump-custom) | Dump the schema definition in a custom format. |
| [sakoe database schema dump-inc](#sakoe-database-schema-dump-inc) | Dump an incremental schema definition (.df) by comparing 2 databases. |
| [sakoe database schema dump-inc-df](#sakoe-database-schema-dump-inc-df) | Dump an incremental schema definition (.df) by comparing 2 .df files. |
| [sakoe database schema dump-sql](#sakoe-database-schema-dump-sql) | Dump the SQL-92 schema of a database. |
| [sakoe database schema load](#sakoe-database-schema-load) | Load a schema definition (.df) to a database. |
| [sakoe database start](#sakoe-database-start) | Start a database in order to use it in multi-users mode. |
| [sakoe database stop](#sakoe-database-stop) | Stop a database that was started for multi-users. |
| [sakoe database structure](#sakoe-database-structure) | Operate on a database structure file (.st). |
| [sakoe database structure add](#sakoe-database-structure-add) | Append the extents from a structure file (.st) to a database. |
| [sakoe database structure generate](#sakoe-database-structure-generate) | Generate the structure file (.st) from a definition file (.df). |
| [sakoe database structure remove](#sakoe-database-structure-remove) | Remove storage areas or extents. |
| [sakoe database structure update](#sakoe-database-structure-update) | Create or update a structure file (.st) from the database .db file. |
| [sakoe database structure validate](#sakoe-database-structure-validate) | Validate a structure file (.st) against a given database. |
| [sakoe manual](#sakoe-manual) | The manual of this tool. Learn about the usage and key concepts of sakoe. |
| [sakoe progress](#sakoe-progress) | Progress utilities commands. |
| [sakoe progress exe-path](#sakoe-progress-exe-path) | Returns the full path of the progress executable. |
| [sakoe progress generate-ds](#sakoe-progress-generate-ds) | Generate a dataset definition from a xsd/xml file. |
| [sakoe progress help](#sakoe-progress-help) | Access the Openedge help. |
| [sakoe progress help list](#sakoe-progress-help-list) | List all the .chm files (windows help files) available in $DLC/prohelp. |
| [sakoe progress help message](#sakoe-progress-help-message) | Display the extended error message using an error number. |
| [sakoe progress help open](#sakoe-progress-help-open) | Open an help file (.chm) present in $DLC/prohelp. |
| [sakoe progress help search](#sakoe-progress-help-search) | Search for a keyword in the language windows help file. |
| [sakoe progress read-ini](#sakoe-progress-read-ini) | Get the PROPATH value found in a .ini file. |
| [sakoe progress read-pf](#sakoe-progress-read-pf) | Get a single line argument string from a .pf file. |
| [sakoe progress version](#sakoe-progress-version) | Get the version found for the Openedge installation. |
| [sakoe progress wsdl-doc](#sakoe-progress-wsdl-doc) | Generate an html documentation from a wsdl. |
| [sakoe project](#sakoe-project) | Commands related to an Openedge project (.oe directory). |
| [sakoe project build](#sakoe-project-build) | Build automation for Openedge projects. |
| [sakoe project git-ignore](#sakoe-project-git-ignore) | Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists). |
| [sakoe project init](#sakoe-project-init) | Initialize a new Openedge project file (.oeproj.xml). |
| [sakoe project list](#sakoe-project-list) | List all the project files or list the build configurations in a project file. |
| [sakoe project project](#sakoe-project-project) | Operates on databases belonging to a project. |
| [sakoe project project create](#sakoe-project-project-create) | TODO : repair database |
| [sakoe project update](#sakoe-project-update) | Update the `Project.xsd` file of the project with the latest version embedded in this tool. |
| [sakoe tool](#sakoe-tool) | Manage external tools usable in sakoe. |
| [sakoe tool datadigger](#sakoe-tool-datadigger) | DataDigger is a tool for exploring and modifying the data of a database. |
| [sakoe tool datadigger install](#sakoe-tool-datadigger-install) | Install DataDigger in the default installation path. |
| [sakoe tool datadigger remove](#sakoe-tool-datadigger-remove) | Remove DataDigger from the installation path. |
| [sakoe tool datadigger run](#sakoe-tool-datadigger-run) | Run a new DataDigger instance. |
| [sakoe update](#sakoe-update) | Update this tool with the latest release found on github. |
| [sakoe version](#sakoe-version) | Show the version information of this tool. |

## ABOUT THE BUILD COMMAND



### Overview

With sakoe, you can 'build' your project. A build process is a succession of tasks that (typically) transform your source files into a deliverable format, usually called a release or package.

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


## SAKOE DATABASE

### Synopsis

Database manipulation tools.

### Usage

`sakoe database [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| am | admin | The openedge database administration tool. |
| an | analysis | Regroup commands related to database analysis. |
| ba | backup | Backup a database into a single file for a future restore. |
| bi | before-image | Operate on database before-image file. |
| bu | busy | Check the usage mode of a database (busy mode). |
| ck | check | Check if a database is alive. |
| cn | connect | Get the connection string to use to connect to a database. |
| cp | copy | Copy a database. |
| cr | create | Creates a new database. |
| da | data | Operate on database data. |
| dd | datadigger | DataDigger is a tool for exploring and modifying the data of a database. |
| de | delete | Delete a database. |
| dt | dictionary | The openedge database dictionary tool. |
| id | index | Operate on database indexes. |
| ki | kill | Kill the broker/server processes running for a particular a database. |
| ka | kill-all | Kill all the broker/server processes running on this machine. |
| lg | log | Operate on database log files (.lg). |
| pr | project | Operates on databases belonging to a project. |
| re | repair | Repair/update the database control information file (.db). |
| rs | restore | Restore a database from a backup file. |
| sc | schema | Operate on database schema. |
| sa | start | Start a database in order to use it in multi-users mode. |
| so | stop | Stop a database that was started for multi-users. |
| st | structure | Operate on a database structure file (.st). |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE ADMIN

### Synopsis

The openedge database administration tool.

### Usage

`sakoe database admin [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| rn | run | Run an instance of the database administration tool. |

### Description

The default sub command for this command is run.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE ADMIN RUN

### Synopsis

Run an instance of the database administration tool.

### Usage

`sakoe database admin run [options] [-- <extra pro args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | \--detached | Use this option to immediately return to the prompt instead of waiting the for the program to exit. |
| -c | \--connection \<args> | (Can be used multiple times) A connection string that can be used to connect to one or more openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<path> | (Can be used multiple times) Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to a list of path of all the .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE ANALYSIS

### Synopsis

Regroup commands related to database analysis.

### Usage

`sakoe database analysis [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| ad | advise | Generates an html report which provides pointers to common database configuration issues. |
| re | report | Displays an analysis report. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE ANALYSIS ADVISE

### Synopsis

Generates an html report which provides pointers to common database configuration issues.

### Usage

`sakoe database analysis advise <report-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<report-file\> | Path to the output html report file that will contain pointers to common database configuration issues. The extension is optional but it will be changed to .html if it is incorrectly provided. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -a | \--analysis-file \<file> | The file path to a database analysis output. If empty, the analysis will be carried on automatically if the database is local. |
| -u | \--unattended | Do not open the html report with the default browser after its creation. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

This command uses the Database Advisor which can be found here: http://practicaldba.com/dlmain.html.

The OpenEdge Database Advisor is intended to provide a quick checkup for common database configuration issues. Obviously proper tuning for an application requires much more than any tool can provide, but the advisor should highlight some of the most common low hanging fruit.

For best results you will need a recent database analysis file (proutil -C dbanalys) and you should run this against your production database. A large portion of the suggestions will be based on VST information that will differ greatly between your production and test environments.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE ANALYSIS REPORT

### Synopsis

Displays an analysis report.

### Usage

`sakoe database analysis report [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -o | \--options \<args> | Extra options for the proutil dbanalys command |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

An analysis report is the combination of the output from proutil dbanalys, describe and iostats.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE BACKUP

### Synopsis

Backup a database into a single file for a future restore.

### Usage

`sakoe database backup <backup-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<backup-file\> | File path that will contain the database backup. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -ns | \--no-scan | Prevents the tool from performing an initial scan of the database to display the number of blocks that will be backed up and the amount of media the backup requires. |
| -nc | \--no-compress | Prevents the tool from compressing the backup file. |
| -op | \--options \<args> | Use options for the probkup utility (see the documentation online). |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE BEFORE-IMAGE

### Synopsis

Operate on database before-image file.

### Usage

`sakoe database before-image [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| tr | truncate | Truncate the before-image of a database. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE BEFORE-IMAGE TRUNCATE

### Synopsis

Truncate the before-image of a database.

### Usage

`sakoe database before-image truncate [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Performs the following three functions:
 - Uses the information in the before-image (BI) files to bring the database and after-image (AI) files up to date, waits to verify that the information has been successfully written to the disk, then truncates the before-image file to its original length.
 - Sets the BI cluster size using the Before-image Cluster Size (-bi) parameter.
 - Sets the BI block size using the Before-image Block Size (-biblocksize) parameter.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE BUSY

### Synopsis

Check the usage mode of a database (busy mode).

### Usage

`sakoe database busy [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE CHECK

### Synopsis

Check if a database is alive.

### Usage

`sakoe database check [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Check if a database is alive by trying to connect to it using a openedge client.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE CONNECT

### Synopsis

Get the connection string to use to connect to a database.

### Usage

`sakoe database connect [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -l | \--logical-name \<name> | The logical name to use for the database in the connection string. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE COPY

### Synopsis

Copy a database.

### Usage

`sakoe database copy <source db path> <target db path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<source db path\> | Path to the source database (.db file). The .db extension is optional and the path can be relative to the current directory. |
| \<target db path\> | Path to the target database (.db file). The .db extension is optional and the path can be relative to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -ni | \--new-instance | Specifies that a new GUID be created for the target database. |
| -rp | \--relative-path | Use relative path in the structure file. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE CREATE

### Synopsis

Creates a new database.

### Usage

`sakoe database create [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | File name (physical name) of the database to create (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the name of the current directory. |
| -df | \--df \<file> | Path to the .df file containing the database schema definition. The path can be relative to the current directory. |
| -st | \--st \<file> | Path to the structure file (.st file) containing the database physical structure. The path can be relative to the current directory. |
| -bs | \--block-size \<size> | The block-size to use when creating the database. Defaults to the default block-size for the current platform (linux or windows). |
| -lg | \--lang \<lang> | The codepage/lang to use. Creates the database using an empty database located in the openedge installation $DLC/prolang/(lang). |
| -ni | \--new-instance | Specifies that a new GUID be created for the target database. |
| -rp | \--relative-path | Use relative path in the structure file. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA

### Synopsis

Operate on database data.

### Usage

`sakoe database data [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| du | dump | Dump the database data in plain text files (.d). |
| db | dump-binary | Dump data in binary format from a database. |
| dq | dump-sequence | Dump the database sequence data in a plain text file (.d). |
| ds | dump-sql | Dump data in SQL-92 format from a database. |
| lo | load | Load the database data from plain text files (.d). |
| lb | load-binary | Load the database data from binary format files. |
| lk | load-bulk | Load the database data from plain text files (.d). |
| lq | load-sequence | Load the database sequence data from a plain text file (.d). |
| ls | load-sql | Load the database data from SQL-92 format files (.dsql). |
| tc | truncate | Deletes all the data of a given table. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA DUMP

### Synopsis

Dump the database data in plain text files (.d).

### Usage

`sakoe database data dump <dump-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-directory\> | Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .d file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -t | \--table \<tables> | A list of comma separated table names to dump. Defaults to `ALL` which dumps every table. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA DUMP-BINARY

### Synopsis

Dump data in binary format from a database.

### Usage

`sakoe database data dump-binary <dump-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-directory\> | Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .bd file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -t | \--table \<tables> | A list of comma separated table names to dump. Defaults to dumping every table. |
| -op | \--options \<args> | Use options for the proutil dump utility (see the documentation online). |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA DUMP-SEQUENCE

### Synopsis

Dump the database sequence data in a plain text file (.d).

### Usage

`sakoe database data dump-sequence <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-file\> | File path that will contain the data dumped (usually has the .d extension). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

The data of each sequence of the database is dumped in the output file.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA DUMP-SQL

### Synopsis

Dump data in SQL-92 format from a database.

### Usage

`sakoe database data dump-sql <dump-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-directory\> | Directory path that will contain the data dumped. Each table of the database will be dumped as an individual .dsql file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the sqldump utility (see the documentation online). Defaults to dumping every table. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA LOAD

### Synopsis

Load the database data from plain text files (.d).

### Usage

`sakoe database data load <data-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<data-directory\> | Directory path that contain the data to load. Each table of the database should be stored as an individual .d file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -t | \--table \<tables> | A list of comma separated table names to load. Defaults to `ALL` which loads every table. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA LOAD-BINARY

### Synopsis

Load the database data from binary format files.

### Usage

`sakoe database data load-binary <data-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<data-directory\> | Directory path that contain the data to load. Each table of the database should be stored as an individual .bd file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the proutil load utility (see the documentation online). |
| -nr | \--no-rebuild | Do not rebuild indexes when loading the data. |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA LOAD-BULK

### Synopsis

Load the database data from plain text files (.d).

### Usage

`sakoe database data load-bulk <data-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<data-directory\> | Directory path that contain the data to load. Each table of the database should be stored as an individual .bd file named like the table. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the proutil load utility (see the documentation online). |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA LOAD-SEQUENCE

### Synopsis

Load the database sequence data from a plain text file (.d).

### Usage

`sakoe database data load-sequence <data-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<data-file\> | File path that contains the sequence data to load (usually has the .d extension). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA LOAD-SQL

### Synopsis

Load the database data from SQL-92 format files (.dsql).

### Usage

`sakoe database data load-sql <data-directory> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<data-directory\> | Directory path that contain the data to load. Each table of the database should be stored as an individual .dsql file named like the `owner.table` (e.g. PUB.table1). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the sqlload utility (see the documentation online). Defaults to loading every table. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATA TRUNCATE

### Synopsis

Deletes all the data of a given table.

### Usage

`sakoe database data truncate <tables> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<tables\> | A list of comma separated table names to truncate. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATADIGGER

### Synopsis

DataDigger is a tool for exploring and modifying the data of a database.

### Usage

`sakoe database datadigger [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| in | install | Install DataDigger in the default installation path. |
| rm | remove | Remove DataDigger from the installation path. |
| ru | run | Run a new DataDigger instance. |

### Description

The default sub command for this command is run.

If DataDigger is already installed on your computer, you can use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify the installation location so sakoe knows where to find the tool. Otherwise, simply let sakoe install it in the default location.

DataDigger is maintained by Patrick Tingen: https://github.com/patrickTingen/DataDigger.

Learn more here: https://datadigger.wordpress.com.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATADIGGER INSTALL

### Synopsis

Install DataDigger in the default installation path.

### Usage

`sakoe database datadigger install [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | \--get-pre-release | Accept to install pre-release (i.e. 'beta') versions of the tool. |
| -p | \--proxy \<url> | The http proxy to use for this update. Useful if you are behind a corporate firewall. The expected format is: 'http(s)://[user:password@]host[:port]'. It is also possible to use the environment variables OE_HTTP_PROXY or http_proxy to set this value. |
| -f | \--force | Force the installation even if the tool is already installed. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify a different location.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATADIGGER REMOVE

### Synopsis

Remove DataDigger from the installation path.

### Usage

`sakoe database datadigger remove [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--force | Mandatory option to force the removal and avoid bad manipulation. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DATADIGGER RUN

### Synopsis

Run a new DataDigger instance.

### Usage

`sakoe database datadigger run [options] [-- <extra pro args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -ro | \--read-only | Start DataDigger in read-only mode (records will not modifiable). |
| -d | \--detached | Use this option to immediately return to the prompt instead of waiting the for the program to exit. |
| -c | \--connection \<args> | (Can be used multiple times) A connection string that can be used to connect to one or more openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<path> | (Can be used multiple times) Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to a list of path of all the .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Please note that when running DataDigger, the DataDigger.pf file of the installation path is used.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DELETE

### Synopsis

Delete a database.

### Usage

`sakoe database delete [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -y | \--yes | Automatically answer yes on deletion confirmation. |
| -st | \--delete-st | Also delete the structure file (.st). |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

All the files composing the database are deleted.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DICTIONARY

### Synopsis

The openedge database dictionary tool.

### Usage

`sakoe database dictionary [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| rn | run | Run an instance of the dictionary tool. |

### Description

The default sub command for this command is run.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE DICTIONARY RUN

### Synopsis

Run an instance of the dictionary tool.

### Usage

`sakoe database dictionary run [options] [-- <extra pro args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | \--detached | Use this option to immediately return to the prompt instead of waiting the for the program to exit. |
| -c | \--connection \<args> | (Can be used multiple times) A connection string that can be used to connect to one or more openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<path> | (Can be used multiple times) Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to a list of path of all the .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE INDEX

### Synopsis

Operate on database indexes.

### Usage

`sakoe database index [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| rb | rebuild | Rebuild the indexes of a database. By default, rebuilds all the active indexes. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE INDEX REBUILD

### Synopsis

Rebuild the indexes of a database. By default, rebuilds all the active indexes.

### Usage

`sakoe database index rebuild [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Parameters for the proutil idxbuild command. Defaults to `activeindexes`. |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE KILL

### Synopsis

Kill the broker/server processes running for a particular a database.

### Usage

`sakoe database kill [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE KILL-ALL

### Synopsis

Kill all the broker/server processes running on this machine.

### Usage

`sakoe database kill-all [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

A broker process is generally named: `_mprosrv`.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOG

### Synopsis

Operate on database log files (.lg).

### Usage

`sakoe database log [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| tr | truncate | Truncates the log file. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE LOG TRUNCATE

### Synopsis

Truncates the log file.

### Usage

`sakoe database log truncate [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

If the database is started, it will re-log the database startup parameters to the log file.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE PROJECT

### Synopsis

Operates on databases belonging to a project.

### Usage

`sakoe database project [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE REPAIR

### Synopsis

Repair/update the database control information file (.db).

### Usage

`sakoe database repair [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Update the database control information, usually done after an extent has been moved or renamed.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE RESTORE

### Synopsis

Restore a database from a backup file.

### Usage

`sakoe database restore <backup-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<backup-file\> | File path that contains the database backup. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the prorest utility (see the documentation online). |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA

### Synopsis

Operate on database schema.

### Usage

`sakoe database schema [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| du | dump | Dump the schema definition (.df) of a database. |
| dc | dump-custom | Dump the schema definition in a custom format. |
| di | dump-inc | Dump an incremental schema definition (.df) by comparing 2 databases. |
| dd | dump-inc-df | Dump an incremental schema definition (.df) by comparing 2 .df files. |
| ds | dump-sql | Dump the SQL-92 schema of a database. |
| lo | load | Load a schema definition (.df) to a database. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA DUMP

### Synopsis

Dump the schema definition (.df) of a database.

### Usage

`sakoe database schema dump <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-file\> | Path to the output .df file that will contain the schema definition of the database (extension is optional). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -t | \--table \<tables> | A list of comma separated table names for which we need a schema definition dump. Defaults to `ALL` which dumps every table and sequence. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA DUMP-CUSTOM

### Synopsis

Dump the schema definition in a custom format.

### Usage

`sakoe database schema dump-custom <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-file\> | Path to the output .df file that will contain the schema definition of the database. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA DUMP-INC

### Synopsis

Dump an incremental schema definition (.df) by comparing 2 databases.

### Usage

`sakoe database schema dump-inc <old-db-path> <new-db-path> <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<old-db-path\> | The path to the 'old' database. The .db extension is optional. |
| \<new-db-path\> | The path to the 'new' database. The .db extension is optional. |
| \<dump-file\> | Path to the output incremental .df file (extension is optional). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -rf | \--rename-file \<file> | Path to the 'rename file'. It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.  The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT): - T,old-table-name,new-table-name - F,table-name,old-field-name,new-field-name - S,old-sequence-name,new-sequence-name  Missing entries or entries with an empty new name are considered to have been deleted.  |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Two databases schema definition are compared and the difference is written in a 'delta' `.df`. This `.df` file then allows to upgrade an older database schema to the new schema.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA DUMP-INC-DF

### Synopsis

Dump an incremental schema definition (.df) by comparing 2 .df files.

### Usage

`sakoe database schema dump-inc-df <old df path> <new df path> <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<old df path\> | The path to the 'old' database. |
| \<new df path\> | The path to the 'new' database. |
| \<dump-file\> | Path to the output incremental .df file (extension is optional). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -rf | \--rename-file \<file> | Path to the 'rename file'. It is a plain text file used to identify database tables and fields that have changed names. This allows to avoid having a DROP then ADD table when you changed only the name of said table.  The format of the file is simple (comma separated lines, don't forget to add a final empty line for IMPORT): - T,old-table-name,new-table-name - F,table-name,old-field-name,new-field-name - S,old-sequence-name,new-sequence-name  Missing entries or entries with an empty new name are considered to have been deleted.  |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA DUMP-SQL

### Synopsis

Dump the SQL-92 schema of a database.

### Usage

`sakoe database schema dump-sql <dump-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<dump-file\> | Path to the output .dfsql file that will contain the sql-92 definition of the database (extension is optional). |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -op | \--options \<args> | Use options for the sqlschema utility (see the documentation online). Defaults to dumping everything. |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE SCHEMA LOAD

### Synopsis

Load a schema definition (.df) to a database.

### Usage

`sakoe database schema load <df-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<df-file\> | Path to the .df file that contains the schema definition (or partial schema) of the database. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--connection \<args> | A connection string that can be used to connect to an openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE START

### Synopsis

Start a database in order to use it in multi-users mode.

### Usage

`sakoe database start [options] [-- <extra proserve args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -s | \--service \<port> | Service name that will be used by this database. Usually a port number or a service name declared in /etc/services. |
| -h | \--hostname \<host> | The hostname on which to start the database. Defaults to the current machine. |
| -np | \--next-port \<port> | Port number, the next available port after this number will be used to start the database. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STOP

### Synopsis

Stop a database that was started for multi-users.

### Usage

`sakoe database stop [options] [-- <extra proshut args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE

### Synopsis

Operate on a database structure file (.st).

### Usage

`sakoe database structure [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| ad | add | Append the extents from a structure file (.st) to a database. |
| ge | generate | Generate the structure file (.st) from a definition file (.df). |
| rm | remove | Remove storage areas or extents. |
| up | update | Create or update a structure file (.st) from the database .db file. |
| va | validate | Validate a structure file (.st) against a given database. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE ADD

### Synopsis

Append the extents from a structure file (.st) to a database.

### Usage

`sakoe database structure add <st-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<st-file\> | Path to the structure file (.st) to add. The path can be relative to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE GENERATE

### Synopsis

Generate the structure file (.st) from a definition file (.df).

### Usage

`sakoe database structure generate <df-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<df-file\> | Path to the .df file containing the database schema definition. The path can be relative to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Create all the needed AREA found in the given schema definition file (.df).

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE REMOVE

### Synopsis

Remove storage areas or extents.

### Usage

`sakoe database structure remove <extent-token> <storage-area> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<extent-token\> | Indicates the type of extent to remove. Specify one of the following: d, bi, ai, tl. |
| \<storage-area\> | Specifies the name of the storage area to remove. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE UPDATE

### Synopsis

Create or update a structure file (.st) from the database .db file.

### Usage

`sakoe database structure update [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -rp | \--relative-path | Use relative path in the structure file. |
| -dp | \--directory-path | By default, listing will output file path to each extent, this option allows to output directory path instead. |
| -a | \--access \<args> | Database access/encryption arguments: `[[-userid username [-password passwd ]] | [ -U username -P passwd] ] [-Passphrase]`. |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE DATABASE STRUCTURE VALIDATE

### Synopsis

Validate a structure file (.st) against a given database.

### Usage

`sakoe database structure validate <st-file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<st-file\> | Path to the structure file (.st) to validate against the database. The path can be relative to the current directory. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to the path of the unique .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Validates whether or not the structure is valid to use either for creation (if the database does not exist) or for an update.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE MANUAL

### Synopsis

The manual of this tool. Learn about the usage and key concepts of sakoe.

### Usage

`sakoe manual [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### What is this tool

SAKOE is a collection of tools aimed to simplify your work in Openedge environments.

### About this manual

The goal of this manual is to provide KEY concepts that are necessary to understand to use this tool to its fullest.

Each command is well documented on its own, don't be afraid to use the `--help` option.

### Command line usage

How to use this command line interface tool:
  - You can escape white spaces in argument/option values by using double quotes (i.e. "my value").
  - If you need to use a double quote within a double quote, you can do so by double quoting the double quotes (i.e. "my ""special"" value").
  - If an extra layer is needed, just double the doubling (i.e. -opt "-mysubopt ""my special """"value""""""").
  - In the 'USAGE' help section, arguments between brackets (i.e. []) are optionals.

### Response file parsing

Instead of using a long command line (which is limited in size on every platform), you can use a response file that contains each argument/option that should be used.

Everything that is usually separated by a space in the command line should be separated by a new line in the file.
In response files, you do not have to double quote arguments containing spaces, they will be considered as a whole as long as they are on a separated line.

  `sakoe @responsefile.txt`

### Exit code

The convention followed by this tool is the following.
  - 0 : used when a command completed successfully, without errors nor warnings.
  - 1-8 : used when a command completed but with warnings, the level can be used to pinpoint different kinds of warnings.
  - 9 : used when a command does not complete and ends up in error.

### Website

The official page of this tool is:
  https://jcaillon.github.io/Oetools.Sakoe/.

You are invited to STAR the project on github to increase its visibility!


**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS

### Synopsis

Progress utilities commands.

### Usage

`sakoe progress [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| ep | exe-path | Returns the full path of the progress executable. |
| gd | generate-ds | Generate a dataset definition from a xsd/xml file. |
| hp | help | Access the Openedge help. |
| ri | read-ini | Get the PROPATH value found in a .ini file. |
| rp | read-pf | Get a single line argument string from a .pf file. |
| ve | version | Get the version found for the Openedge installation. |
| wd | wsdl-doc | Generate an html documentation from a wsdl. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS EXE-PATH

### Synopsis

Returns the full path of the progress executable.

### Usage

`sakoe progress exe-path [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -c | \--char-mode | Always return the path of the character mode executable (otherwise prowin is returned by default on windows platform). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS GENERATE-DS

### Synopsis

Generate a dataset definition from a xsd/xml file.

### Usage

`sakoe progress generate-ds <file> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<file\> | Path to an .xml or .xsd file from which to generate the dataset definition. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -o | \--output \<file> | The path to the output include file (.i) that will contain the dataset definition. Defaults to the input file name with the .i extension. |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS HELP

### Synopsis

Access the Openedge help.

### Usage

`sakoe progress help [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| ls | list | List all the .chm files (windows help files) available in $DLC/prohelp. |
| ms | message | Display the extended error message using an error number. |
| on | open | Open an help file (.chm) present in $DLC/prohelp. |
| sr | search | Search for a keyword in the language windows help file. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS HELP LIST

### Synopsis

List all the .chm files (windows help files) available in $DLC/prohelp.

### Usage

`sakoe progress help list [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS HELP MESSAGE

### Synopsis

Display the extended error message using an error number.

### Usage

`sakoe progress help message <message number> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<message number\> | The number of the error message to show. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

This command uses the content of files located in $DLC/prohelp/msgdata to display information.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS HELP OPEN

### Synopsis

Open an help file (.chm) present in $DLC/prohelp.

### Usage

`sakoe progress help open <chm name> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<chm name\> | The file name of the .chm file to display. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -t | \--topic \<topic> | Open the .chm on the given topic. |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS HELP SEARCH

### Synopsis

Search for a keyword in the language windows help file.

### Usage

`sakoe progress help search <keyword> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<keyword\> | The keyword you would like to find in the help. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS READ-INI

### Synopsis

Get the PROPATH value found in a .ini file.

### Usage

`sakoe progress read-ini <ini path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<ini path\> | The file path to the .ini file to read. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | \--directory | The base directory to use to convert to absolute path. Default to the current directory. |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

- This command returns only absolute path.
- Relative path are converted to absolute using the command folder option.
- This command returns only existing directories or .pl files.
- This command expands environment variables like %TEMP% or $DLC.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS READ-PF

### Synopsis

Get a single line argument string from a .pf file.

### Usage

`sakoe progress read-pf <pf path> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<pf path\> | The file path to the parameter file (.pf) to use. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

- This command will skip unnecessary whitespaces and new lines.
- This command will ignore comment lines starting with #.
- Resolves -pf parameters inside the .pf by reading the content of the files.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS VERSION

### Synopsis

Get the version found for the Openedge installation.

### Usage

`sakoe progress version [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROGRESS WSDL-DOC

### Synopsis

Generate an html documentation from a wsdl.

### Usage

`sakoe progress wsdl-doc <wsdl> [options]`

### Arguments

| Argument | Description |
| --- | --- |
| \<wsdl\> | Path to the wsdl file from which to generate the documentation. |

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -o | \--output \<dir> | The directory where to generate the documentation. Defaults to a sub folder named as the .wsdl in the current directory. |
| -u | \--unattended | Do not open the html documentation with the default browser after its creation. |
| -b | \--bindings | Force documentation of WSDL bindings. |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Use this command to generate an html documentation that guides you on how to use the webservice using openedge ABL language.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT

### Synopsis

Commands related to an Openedge project (.oe directory).

### Usage

`sakoe project [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| bd | build | Build automation for Openedge projects. |
| gi | git-ignore | Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists). |
| in | init | Initialize a new Openedge project file (.oeproj.xml). |
| ls | list | List all the project files or list the build configurations in a project file. |
| pr | project | Operates on databases belonging to a project. |
| up | update | Update the `Project.xsd` file of the project with the latest version embedded in this tool. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT BUILD

### Synopsis

Build automation for Openedge projects.

### Usage

`sakoe project build [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<file> | Path or name of the project file. The .oeproj.xml extension is optional. Defaults to the first .oeproj.xml file found. The search is done in the current directory and in the .oe directory when it exists. |
| -c | \--config-name \<config> | The name of the build configuration to use for the build. This name is found in the .oeproj.xml file. Defaults to the first build configuration found in the project file. |
| -e | \--extra-config \<project=config> | (Can be used multiple times) In addition to the base build configuration specified by <project> and --config-name, you can dynamically add a child configuration to the base configuration with this option. This option can be used multiple times, each new configuration will be added as a child of the previously defined configuration. This option allows you to share, with your colleagues, a common project file that holds the property of your application and have an extra configuration in local (just for you) which you can use to build the project in a specific local directory. For each extra configuration, specify the path or the name of the project file and the configuration name to use. If the project file name if empty, the main <project> is used. |
| -p | \--property \<key=value> | (Can be used multiple times) A pair of key/value to dynamically set a property for this build. The value set this way will prevail over the value defined in a project file. Each pair should specify the name of the property to set and the value that should be used. Use the option --property-help to see the full list of properties available as well as their documentation. |
| -v | \--variable \<key=value> | (Can be used multiple times) A pair of key/value to dynamically set a variable for this build. A variable set this way will prevail over a variable with the same name defined in a project file. Each pair should specify the name of the variable to set and the value that should be used. |
| -ph | \--property-help | Shows the list of each build property usable with its full documentation. |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

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
* -p "DatabaseInternationalizationStartupParameters="
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
* -p "OpenedgeIoEncoding="
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

> Display the full documentation of each build property by running `sakoe project build --property-help`.

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

`sakoe project build p1 --config-name Configuration1 --extra-config p2=Configuration6 --extra-config Configuration2`

Below is the equivalent of how build configurations are nested in this scenario:

Configuration1
- Configuration3
   - Configuration4
      - Configuration6
         - Configuration2

This allow a lot of flexibility for organizing and partitioning your build process.

### Notes

Create a new project file using the command: `sakoe project init`.
Get a more in-depth help and learn about the concept of a build (in sakoe) using the command: `sakoe manual command build`.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT GIT-IGNORE

### Synopsis

Initialize a .gitignore file adapted for sakoe projects (or append to, if it exists).

### Usage

`sakoe project git-ignore [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | \--directory \<dir> | The repository base directory (source directory). Defaults to the current directory. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT INIT

### Synopsis

Initialize a new Openedge project file (.oeproj.xml).

### Usage

`sakoe project init [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--file \<name> | The name of the project (project file) to create. Defaults to the current directory name. |
| -d | \--directory \<dir> | The directory in which to initialize the project. Defaults to the current directory. |
| -l | \--local | Create the new project file for local use only. A local project should contain build configurations specific to your machine and thus should not be shared or versioned in your source control system. |
| -o | \--override | Force the creation of the project file by replacing an older project file, if it exists. By default, the command will fail if the project file already exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT LIST

### Synopsis

List all the project files or list the build configurations in a project file.

### Usage

`sakoe project list [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -p | \--path \<path> | The project file in which to list the build configurations or the project base directory (source directory) in which to list the project files. Defaults to the current directory. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT PROJECT

### Synopsis

Operates on databases belonging to a project.

### Usage

`sakoe project project [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| cr | create | TODO : repair database |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT PROJECT CREATE

### Synopsis

TODO : repair database

### Usage

`sakoe project project create [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

TODO : database

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE PROJECT UPDATE

### Synopsis

Update the `Project.xsd` file of the project with the latest version embedded in this tool.

### Usage

`sakoe project update [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -d | \--directory \<dir> | The directory in which the project is located. Defaults to the current directory. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE TOOL

### Synopsis

Manage external tools usable in sakoe.

### Usage

`sakoe tool [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| dd | datadigger | DataDigger is a tool for exploring and modifying the data of a database. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE TOOL DATADIGGER

### Synopsis

DataDigger is a tool for exploring and modifying the data of a database.

### Usage

`sakoe tool datadigger [options] [command]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -h | \--help | Show this help text. |

### Commands

| Short name | Long name | Description |
| --- | --- | --- |
| in | install | Install DataDigger in the default installation path. |
| rm | remove | Remove DataDigger from the installation path. |
| ru | run | Run a new DataDigger instance. |

### Description

The default sub command for this command is run.

If DataDigger is already installed on your computer, you can use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify the installation location so sakoe knows where to find the tool. Otherwise, simply let sakoe install it in the default location.

DataDigger is maintained by Patrick Tingen: https://github.com/patrickTingen/DataDigger.

Learn more here: https://datadigger.wordpress.com.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE TOOL DATADIGGER INSTALL

### Synopsis

Install DataDigger in the default installation path.

### Usage

`sakoe tool datadigger install [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | \--get-pre-release | Accept to install pre-release (i.e. 'beta') versions of the tool. |
| -p | \--proxy \<url> | The http proxy to use for this update. Useful if you are behind a corporate firewall. The expected format is: 'http(s)://[user:password@]host[:port]'. It is also possible to use the environment variables OE_HTTP_PROXY or http_proxy to set this value. |
| -f | \--force | Force the installation even if the tool is already installed. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Use the environment variable `OE_DATADIGGER_INSTALL_PATH` to specify a different location.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE TOOL DATADIGGER REMOVE

### Synopsis

Remove DataDigger from the installation path.

### Usage

`sakoe tool datadigger remove [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -f | \--force | Mandatory option to force the removal and avoid bad manipulation. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE TOOL DATADIGGER RUN

### Synopsis

Run a new DataDigger instance.

### Usage

`sakoe tool datadigger run [options] [-- <extra pro args>...]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -ro | \--read-only | Start DataDigger in read-only mode (records will not modifiable). |
| -d | \--detached | Use this option to immediately return to the prompt instead of waiting the for the program to exit. |
| -c | \--connection \<args> | (Can be used multiple times) A connection string that can be used to connect to one or more openedge database. The connection string will be used in a `CONNECT` statement. |
| -f | \--file \<path> | (Can be used multiple times) Path to a database (.db file). The .db extension is optional and the path can be relative to the current directory. Defaults to a list of path of all the .db file found in the current directory. |
| -cp | \--cp-params \<args> | Database server internationalization startup parameters such as `-cpinternal` codepage and `-cpstream` codepage. This option will be used for openedge commands that support them (_dbutil, _mprosrv, _mprshut, _proutil). |
| -dl | \--dlc \<dir> | The path to the directory containing the Openedge installation. Will default to the path found in the `OE_DLC` or `DLC` environment variable if it exists. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

### Description

Please note that when running DataDigger, the DataDigger.pf file of the installation path is used.

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE UPDATE

### Synopsis

Update this tool with the latest release found on github.

### Usage

`sakoe update [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | \--get-pre-release | Accept to update from new pre-release (i.e. 'beta') versions of the tool. This option will be used by default if the current version of the tool is a pre-release version. Otherwise, only stable releases will be used for updates.  |
| -p | \--proxy \<url> | The http proxy to use for this update. Useful if you are behind a corporate firewall. The expected format is: 'http(s)://[user:password@]host[:port]'. It is also possible to use the environment variables OE_HTTP_PROXY or http_proxy to set this value. |
| -c | \--check-only | Check for new releases but exit the command before actually updating the tool. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**
## SAKOE VERSION

### Synopsis

Show the version information of this tool.

### Usage

`sakoe version [options]`

### Options

| Short name | Long name | Description |
| --- | --- | --- |
| -b | \--bare-version | Output the raw assembly version of the tool, without logo and pre-release tag. |
| -vb | \--verbosity[:level] | Sets the verbosity of this command line tool. To get the 'raw output' of a command (without displaying the log), you can set the verbosity to `none`. Specifying this option without a level value sets the verbosity to `debug`. Not specifying the option defaults to `info`. Optionally, set the verbosity level for all commands using the environment variable `OE_VERBOSITY`. |
| -lo | \--log-output[:file] | Output all the log message in a file, independently of the current verbosity. This allow to have a normal verbosity in the console while still logging everything to a file. Specifying this option without a value will output to the default file `sakoe.log`. |
| -wl | \--with-logo | Always show the logo on start. |
| -pb | \--progress-bar \<mode> | Sets the display mode of progress bars. Specify `off` to hide progress bars and `stay` to make them persistent. Defaults to `on`, which show progress bars but hide them when done. |
| -h | \--help | Show this help text. |

**[\[Go back to the table of content\].](#table-of-content)**