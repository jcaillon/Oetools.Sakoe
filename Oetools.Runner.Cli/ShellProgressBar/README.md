## About ShellProgressBar

Cross platform simple and complex progressbars on the command line!
Author : Martijn Laarman

This project is taken from this repo :

https://github.com/Mpdreamz/shellprogressbar

I didn't add it as a package dependency because the nuget package is only available for .netstandard. If I build the project targetting .net461, a ton of .dll will be added to the resulting bin/. This is a known issue of .net :

https://github.com/jcaillon/3P/blob/yamuiframework/BUILD.md#additionnal-remarks
