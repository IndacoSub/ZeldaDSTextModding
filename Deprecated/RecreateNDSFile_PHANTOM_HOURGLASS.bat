:my_start
@echo off
ECHO _____________________
ECHO 1. Remember to install "ndspy" and its dependencies (https://github.com/RoadrunnerWMC/ndspy) such as Python 3.6 or newer!
ECHO 2. Remember to replace "Japanes3" inside this .bat file AND the Python programs, if you're not editing a JPN ROM!
ECHO 3. Remember to replace the name of the ROM in the beginning of this file, if it isn't "Phantom.nds"!
ECHO 4. Remember to place the ROM file inside the same folder containing this .bat file, alongside the other Python programs!
ECHO 5. Some languages might not be supported, as different languages have different "escape codes" in their text (the known ones are in ReimportBMGFile.py)!
ECHO 6. This program expects you to copy the newly-created .bmg_out files into the "edit" folder (create it, if it doesn't exist), after using SaveAllStringsFromFile.py!
ECHO 7. Do *NOT* pass the "edit" folder as an argument to the randomizer, create another folder instead!
ECHO 8. Files from Spirit Tracks are **NOT COMPATIBLE** with Phantom Hourglass!

type nul > "EditedROM.nds"
XCOPY .\Phantom.nds .\EditedROM.nds /Q /I /Y

:ask_replace_msg
ECHO _____________________
SET /p WRITE_MESSAGES=Would you like to replace text messages? [y/n] 
IF /i %WRITE_MESSAGES% == y GOTO replace_msg
ECHO Ignoring messages.
GOTO ask_replace_graphics
:replace_msg
ECHO Replacing messages (should be 32 files).
ECHO - - - - - - - - - - -

py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\battle.bmg_out /Message/battle.bmg 
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\battleCommon.bmg_out /Message/battleCommon.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\bossLast1.bmg_out /Message/bossLast1.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\bossLast3.bmg_out /Message/bossLast3.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\brave.bmg_out /Message/brave.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\collect.bmg_out /Message/collect.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\demo.bmg_out /Message/demo.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\field.bmg_out /Message/field.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\flame.bmg_out /Message/flame.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\frost.bmg_out /Message/frost.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\ghost.bmg_out /Message/ghost.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\hidari.bmg_out /Message/hidari.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kaitei.bmg_out /Message/kaitei.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kaitei_F.bmg_out /Message/kaitei_F.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kojima1.bmg_out /Message/kojima1.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kojima2.bmg_out /Message/kojima2.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kojima3.bmg_out /Message/kojima3.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\kojima5.bmg_out /Message/kojima5.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\mainselect.bmg_out /Message/mainselect.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\main_isl.bmg_out /Message/main_isl.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\myou.bmg_out /Message/myou.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\power.bmg_out /Message/power.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\regular.bmg_out /Message/regular.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\sea.bmg_out /Message/sea.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\sennin.bmg_out /Message/sennin.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\ship.bmg_out /Message/ship.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\staff.bmg_out /Message/staff.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\system.bmg_out /Message/system.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\torii.bmg_out /Message/torii.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\wind.bmg_out /Message/wind.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\wisdom.bmg_out /Message/wisdom.bmg
py -3 .\ReimportBMGFile.py .\EditedROM.nds .\edit\wisdom_dngn.bmg_out /Message/wisdom_dngn.bmg

:ask_replace_graphics
ECHO _____________________
SET /p WRITE_GRAPHICS=Would you like to replace graphics? [y/n] 
IF /i %WRITE_GRAPHICS% == y GOTO replace_graphics
ECHO Ignoring graphics.
GOTO ask_replace_fonts
:replace_graphics
ECHO Replacing graphics.
ECHO - - - - - - - - - - -

py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlPlBg.bin Japanese/Menu/Bg/btlPlBg.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\Color0.NCLR Japanese/Menu/Bg/Color0.NCLR
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\endlogoS.bin Japanese/Menu/Bg/endlogoS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\frameBg.bin Japanese/Menu/Bg/frameBg.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pslnkDBg.bin Japanese/Menu/Bg/pslnkDBg.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pslnkUBg.bin Japanese/Menu/Bg/pslnkUBg.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\minigame.bin Japanese/Menu/Tex2D/minigame.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\scratch.bin Japanese/Menu/Tex2D/scratch.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\ship.bin Japanese/Menu/Tex2D/ship.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\title.bin Japanese/Menu/Tex2D/title.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlChM.bin Japanese/Menu/UI_main/btlChM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlCtM.bin Japanese/Menu/UI_main/btlCtM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlOpM.bin Japanese/Menu/UI_main/btlOpM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlPlM.bin Japanese/Menu/UI_main/btlPlM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlRcM.bin Japanese/Menu/UI_main/btlRcM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlStM.bin Japanese/Menu/UI_main/btlStM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltFishU.bin Japanese/Menu/UI_main/cltFishU.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltItemU.bin Japanese/Menu/UI_main/cltItemU.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltStU.bin Japanese/Menu/UI_main/cltStU.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\dmHrGsM.bin Japanese/Menu/UI_main/dmHrGsM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\finM.bin Japanese/Menu/UI_main/finM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\fishingM.bin Japanese/Menu/UI_main/fishingM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\grnSwtM.bin Japanese/Menu/UI_main/grnSwtM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\itemShop.bin Japanese/Menu/UI_main/itemShop.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mgM.bin Japanese/Menu/UI_main/mgM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pauseM.bin Japanese/Menu/UI_main/pauseM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pauseMVs.bin Japanese/Menu/UI_main/pauseMVs.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pslnkU.bin Japanese/Menu/UI_main/pslnkU.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\rplExS.bin Japanese/Menu/UI_main/rplExS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\sgnBd.bin Japanese/Menu/UI_main/sgnBd.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\sgnPst.bin Japanese/Menu/UI_main/sgnPst.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\skipM.bin Japanese/Menu/UI_main/skipM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\titleM.bin Japanese/Menu/UI_main/titleM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\touchM.bin Japanese/Menu/UI_main/touchM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\UIM.bin Japanese/Menu/UI_main/UIM.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\UIMField.bin Japanese/Menu/UI_main/UIMField.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\UIMSea.bin Japanese/Menu/UI_main/UIMSea.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\accessStyle.bin Japanese/Menu/UI_sub/accessStyle.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlChS.bin Japanese/Menu/UI_sub/btlChS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlCtS.bin Japanese/Menu/UI_sub/btlCtS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlGmS.bin Japanese/Menu/UI_sub/btlGmS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlOpS.bin Japanese/Menu/UI_sub/btlOpS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlRsRkS.bin Japanese/Menu/UI_sub/btlRsRkS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlRsRrS.bin Japanese/Menu/UI_sub/btlRsRrS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlRsScS.bin Japanese/Menu/UI_sub/btlRsScS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlRsWnS.bin Japanese/Menu/UI_sub/btlRsWnS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlSsD.bin Japanese/Menu/UI_sub/btlSsD.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlSsU.bin Japanese/Menu/UI_sub/btlSsU.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\btlStS.bin Japanese/Menu/UI_sub/btlStS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltComD.bin Japanese/Menu/UI_sub/cltComD.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltFishD.bin Japanese/Menu/UI_sub/cltFishD.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cltItemD.bin Japanese/Menu/UI_sub/cltItemD.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\cnctS.bin Japanese/Menu/UI_sub/cnctS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\codeS.bin Japanese/Menu/UI_sub/codeS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\fileselect.bin Japanese/Menu/UI_sub/fileselect.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\gameoverS.bin Japanese/Menu/UI_sub/gameoverS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\keyboard.bin Japanese/Menu/UI_sub/keyboard.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mapCommon.bin Japanese/Menu/UI_sub/mapCommon.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mapDungeon.bin Japanese/Menu/UI_sub/mapDungeon.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mapField.bin Japanese/Menu/UI_sub/mapField.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mapGhost.bin Japanese/Menu/UI_sub/mapGhost.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\mapSea.bin Japanese/Menu/UI_sub/mapSea.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\option.bin Japanese/Menu/UI_sub/option.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\partySelS.bin Japanese/Menu/UI_sub/partySelS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pauseS.bin Japanese/Menu/UI_sub/pauseS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pauseSVs.bin Japanese/Menu/UI_sub/pauseSVs.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\pslnkD.bin Japanese/Menu/UI_sub/pslnkD.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\routeS.bin Japanese/Menu/UI_sub/routeS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\shipyard.bin Japanese/Menu/UI_sub/shipyard.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\smpS.bin Japanese/Menu/UI_sub/smpS.bin
py -3 .\ReplaceFileInROM.py .\EditedROM.nds .\edit\wifiScrptS.bin Japanese/Menu/UI_sub/wifiScrptS.bin

:ask_replace_fonts
ECHO _____________________
SET /p WRITE_FONTS=Would you like to replace fonts? [y/n] 
IF /i %WRITE_FONTS% == y GOTO replace_fonts
ECHO Ignoring fonts.
goto myend
:replace_fonts
ECHO Replacing fonts.
ECHO - - - - - - - - - - -

py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\LC_Font_m.nftr Font/LC_Font_m.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\zeldaDS_15.nftr Font/zeldaDS_15.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\zeldaDS_15_btl.nftr Font/zeldaDS_15_btl.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\zeldaDS_endL.nftr Font/zeldaDS_endL.nftr
py -3 .\ReplaceFileinROM.py .\EditedROM.nds .\edit\zeldaDS_endS.nftr Font/zeldaDS_endS.nftr

:myend

ECHO _____________________
ECHO All done!
PAUSE
CLS