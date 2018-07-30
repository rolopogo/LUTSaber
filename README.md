# LUTSaber
An IPA Plugin for BeatSaber to add lookup texture colour correction and hue adjustment
It allows you to radically change the appearance of the game using lookup textures stored in .png files.

# Installation

Ensure you have the latest verion of the Beat Saber Mod Installer installed
https://github.com/Umbranoxio/BeatSaberModInstaller/releases

Copy the contents of the Zip into your Steam\steamapps\common\Beat Saber Folder

Your Beat Saber folder should then look like this:
```
| Beat Saber
  | Plugins
    | LUTSaber.dll      <-- 
  | PluginsContent
    | LUTSaber.bundle   <--
  | CustomLUT		<--
    | <png files>	<--
  | IPA
  | Beat Saber.exe
  | (other files and folders)
```
# Controls

Press Z to cycle through Lookup textures
Press X or C to cycle through hue shifts

# Adding more LUT files

New LUT files go in /CustomLUT The game will detect any png files in this folder

LUT textures must be 256x16 PNG files.

# Creating new LUT files

1. Make a copy of /CustomLUT/_Default.png this is a neutral LUT (lookup texture)

2. Take a screenshot of your game

3. Import into a graphics editor (such as Photoshop) and apply color adjustments 
(such as contrast, brightness, color levels adjustments) until a satisfying result 
has been reached

4. Perform the same steps to your neutral LUT that you copied in step 1

5. Save your modified LUT with a new name into /CustomLUT
