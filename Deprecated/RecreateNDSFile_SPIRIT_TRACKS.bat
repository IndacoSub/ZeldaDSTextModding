:my_start
@echo off
ECHO _____________________
ECHO 1. Remember to install "ndspy" and its dependencies (https://github.com/RoadrunnerWMC/ndspy) such as Python 3.6 or newer!
ECHO 2. Remember to replace "Japanes3" inside this .bat file AND the Python programs, if you're not editing a JPN ROM!
ECHO 3. Remember to replace the name of the ROM in the beginning of this file, if it isn't a JPN ROM called "SPIRITTRACKS_BKIJ01_00.nds"!
ECHO 4. Remember to place the ROM file inside the same folder containing this .bat file, alongside the other Python programs!
ECHO 5. Some languages might not be supported, as different languages have different "escape codes" in their text (the known ones are in ReimportBMGFile.py)!
ECHO 6. This program expects you to copy the newly-created .bmg_out files into the "edit" folder (create it, if it doesn't exist), after using SaveAllStringsFromFile.py!
ECHO 7. Do *NOT* pass the "edit" folder as an argument to the randomizer, create another folder instead!
ECHO 8. Files from Phantom Hourglass are **NOT COMPATIBLE** with Spirit Tracks!

type nul > "EditedROM.nds"
XCOPY .\SPIRITTRACKS_BKIJ01_00.nds .\EditedROM.nds /Q /I /Y

:ask_replace_msg
ECHO _____________________
SET /p WRITE_MESSAGES=Would you like to replace text messages? [y/n] 
IF /i %WRITE_MESSAGES% == y GOTO replace_msg
ECHO Ignoring messages.
GOTO ask_replace_graphics
:replace_msg
ECHO Replacing messages (should be 30 files).
ECHO - - - - - - - - - - -

py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\battle_common.bmg_out /Message/battle_common.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\battle_parent.bmg_out /Message/battle_parent.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\castle.bmg_out /Message/castle.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\castle_town.bmg_out /Message/castle_town.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\collect.bmg_out /Message/collect.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo.bmg_out /Message/demo.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo01_05.bmg_out /Message/demo01_05.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo06_10.bmg_out /Message/demo06_10.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo11_15.bmg_out /Message/demo11_15.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo16_20.bmg_out /Message/demo16_20.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo21_25.bmg_out /Message/demo21_25.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\desert.bmg_out /Message/desert.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\dungeon.bmg_out /Message/dungeon.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\field.bmg_out /Message/field.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\flame.bmg_out /Message/flame.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\flame_fld.bmg_out /Message/flame_fld.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\forest.bmg_out /Message/forest.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\intrain.bmg_out /Message/intrain.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\maingame.bmg_out /Message/maingame.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\post.bmg_out /Message/post.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\regular.bmg_out /Message/regular.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\select.bmg_out /Message/select.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\shop.bmg_out /Message/shop.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\snow.bmg_out /Message/snow.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\tower.bmg_out /Message/tower.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\tower_lobby.bmg_out /Message/tower_lobby.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\train.bmg_out /Message/train_extra.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\village.bmg_out /Message/village.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\water.bmg_out /Message/water.bmg

:ask_replace_graphics
ECHO _____________________
SET /p WRITE_GRAPHICS=Would you like to replace graphics? [y/n] 
IF /i %WRITE_GRAPHICS% == y GOTO replace_graphics
ECHO Ignoring graphics.
GOTO ask_replace_fonts
:replace_graphics
ECHO Replacing graphics.
ECHO - - - - - - - - - - -

py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mixedLarge.bin Japanese/libASR/mixedLarge.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\tex2d.bin Japanese/Screen/tex2d.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\KeyboardBG.bin Japanese/Screen/Bg/KeyboardBG.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\BattleM.bin Japanese/Screen/Layout/BattleM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\BselM_C.bin Japanese/Screen/Layout/BselM_C.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\BselS_A.bin Japanese/Screen/Layout/BselS_A.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Cargo.ncgr Japanese/Screen/Layout/Cargo.ncgr
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\CmnM.bin Japanese/Screen/Layout/CmnM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\CmnM_L.bin Japanese/Screen/Layout/CmnM_L.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\CmnS.bin Japanese/Screen/Layout/CmnS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\CollectM.bin Japanese/Screen/Layout/CollectM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\CollectS.bin Japanese/Screen/Layout/CollectS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Face.ncgr Japanese/Screen/Layout/Face.ncgr
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Fin.bin Japanese/Screen/Layout/Fin.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Item_L.ncgr Japanese/Screen/Layout/Item_L.ncgr
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Keyboard.bin Japanese/Screen/Layout/Keyboard.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\LandM.bin Japanese/Screen/Layout/LandM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\LandM_L.bin Japanese/Screen/Layout/LandM_L.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\LetterM.bin Japanese/Screen/Layout/LetterM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\ListS.bin Japanese/Screen/Layout/ListS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\MiniM.bin Japanese/Screen/Layout/MiniM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\OverS.bin Japanese/Screen/Layout/OverS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Rabbit.bin Japanese/Screen/Layout/Rabbit.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\ResultS.bin Japanese/Screen/Layout/ResultS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\StampM.bin Japanese/Screen/Layout/StampM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\TitleM.bin Japanese/Screen/Layout/TitleM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\TrainS.bin Japanese/Screen/Layout/TrainS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\WXCS.bin Japanese/Screen/Layout/WXCS.bin

:ask_replace_fonts
ECHO _____________________
SET /p WRITE_FONTS=Would you like to replace fonts? [y/n] 
IF /i %WRITE_FONTS% == y GOTO replace_fonts
ECHO Ignoring fonts.
goto myend
:replace_fonts
ECHO Replacing fonts.
ECHO - - - - - - - - - - -

py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\DSZ2_endL.nftr Font/DSZ2_endL.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\DSZ2_endS.nftr Font/DSZ2_endS.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\DSZ2_msg.nftr Font/DSZ2_msg.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\LC_Font_s_Name.nftr Font/LC_Font_s_Name.nftr

:myend

ECHO _____________________
ECHO All done!
PAUSE
CLS