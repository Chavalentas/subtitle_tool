# subtitle_tool
This is a simple tool written in C# that manipulates subtitle files (.SRT).
It can either delay/rush them or merge several subtitle files.

## Used technologies
![.NET](https://img.shields.io/badge/.NET-violet?style=for-the-badge&logo=.NET) ![C#](https://img.shields.io/badge/C%23-green?style=for-the-badge)

## Problem
Everyone who is a movie fan and needs to follow the subtitles from time to time has surely 
encountered the problem of possible delay in the subtitles.
This console tool is supposed to delay/rush them and store them in a new file.
In some cases of movies with more foreign languages, there are subtitle files only for one language.
In this case, you also need to merge the subtitle files for more languages.
This tool makes sure of that.

## How to use
This section describes how to use the different commands:

### Delay subtitles

Navigate to the folder with the executable and run it with the following command:
* **--delay** (mandatory): This is a mandatory command that needs some parameters in order to run properly.
</br></br>
Parameters:
</br></br>
* **-source** (mandatory): The path to the source subtitle file that needs to be rushed/delayed. </br>
E.g.: -source "./mission.srt"
* **-target** (mandatory): The path where the new rushed/delayed subtitle file should be created. </br>
E.g.: -target "./mission_final.srt" </br></br>
**NOTE:** At least one of the following parameters have to specified!
</br></br>
* **-hours** (optional): The amount of hours by which the subtitles should be rushed/delayed. 
Negative number rushes, positive number delays. </br>
E.g.: -hours 2 (delays by 2 hours), -hours -2 (rushes by 2 hours)
* **-minutes** (optional): The amount of minutes by which the subtitles should be rushed/delayed. 
Negative number rushes, positive number delays. </br>
E.g.: -minutes 2 (delays by 2 minutes), -minutes -2 (rushes by 2 minutes)
* **-seconds** (optional): The amount of seconds by which the subtitles should be rushed/delayed. 
Negative number rushes, positive number delays. </br>
E.g.: -seconds 2 (delays by 2 seconds), -seconds -2 (rushes by 2 seconds)
* **-milliseconds** (optional): The amount of milliseconds by which the subtitles should be rushed/delayed. 
Negative number rushes, positive number delays. </br>
E.g.: -milliseconds 2 (delays by 2 milliseconds), -milliseconds -2 (rushes by 2 milliseconds)

### Merge subtitles
Navigate to the folder with the executable and run it with the following command:
* **--merge** (mandatory): This is a mandatory command that needs some parameters to run properly.
</br></br>
Parameters:
</br></br>
* **-sources** (mandatory): The paths to the source subtitle files that need to be merged. </br>
E.g.: -source "./mission_eng.srt" "./mission_esp.srt"  
* **-target** (mandatory): The path where the new merged subtitle file should be created. </br>
E.g.: -target "./merged_final.srt" 

## Example
The following example takes a source file under the path "./mission.srt", delays it by
2 hours, 1 minute, 2 seconds and 1 millisecond.
The delayed file is stored as a new file under "./mission2.srt".

```plaintext
"subtitles_console.exe" --delay -source  "./mission.srt" -target "./mission2.srt"
-minutes 1 -hours 2 -seconds 2 -milliseconds 1
```

The following exampple takes two source files (one only for the English dialogues and one only for the Spanish dialogues)
and merges them together.
The merged file is stored as a new file under "./final.srt".
```plaintext
"subtitles_console.exe" --merge -sources ".\titles_eng.srt" ".\titles_esp.srt"  -target ".\final.srt"
```
It is also possible the chain different commands.
Commands are executed in chain.
When some error is encountered, the execution stops.

```plaintext
"subtitles_console.exe" --delay -seconds 2 -source ".\title_1.srt" -target ".\title_1_delayed.srt" 
--merge -sources ".\title_1_delayed.srt" ".\title_2.srt"  -target ".\final.srt"
```

