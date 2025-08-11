import sys, os, ndspy.rom, ndspy.bmg

# Usage: python SaveAllStringsFromFile.py <rom.nds> [output_folder] [language]
if len(sys.argv) < 2:
    print("Usage: SaveAllStringsFromFile.py <rom.nds> [output_folder] [language]")
    sys.exit(1)

rom_filename = sys.argv[1]
output_dir = os.path.abspath(sys.argv[2]) if len(sys.argv) > 2 and sys.argv[2] else os.path.join(os.getcwd(), "Extracted")
language = sys.argv[3] if len(sys.argv) > 3 and sys.argv[3] else 'Japanese'

os.makedirs(output_dir, exist_ok=True)

zelda_st_locations = [  # List of files containing text
    'Message/battle_common.bmg',
    'Message/battle_parent.bmg',
    'Message/castle.bmg',
    'Message/castle_town.bmg',
    'Message/collect.bmg',
    'Message/demo.bmg',
    'Message/demo01_05.bmg',
    'Message/demo06_10.bmg',
    'Message/demo11_15.bmg',
    'Message/demo16_20.bmg',
    'Message/demo21_25.bmg',
    'Message/desert.bmg',
    'Message/dungeon.bmg',
    'Message/field.bmg',
    'Message/flame.bmg',
    'Message/flame_fld.bmg',
    'Message/forest.bmg',
    'Message/intrain.bmg',
    'Message/maingame.bmg',
    'Message/post.bmg',
    'Message/regular.bmg',
    'Message/select.bmg',
    'Message/shop.bmg',
    'Message/snow.bmg',
    'Message/tower.bmg',
    'Message/tower_lobby.bmg',
    'Message/train.bmg',
    'Message/train_extra.bmg',
    'Message/village.bmg',
    'Message/water.bmg'
]

zelda_ph_locations = [  # List of files containing text
    'Message/battle.bmg',
    'Message/battleCommon.bmg',
    'Message/bossLast1.bmg',
    'Message/bossLast3.bmg',
    'Message/brave.bmg',
    'Message/collect.bmg',
    'Message/demo.bmg',
    'Message/field.bmg',
    'Message/flame.bmg',
    'Message/frost.bmg',
    'Message/ghost.bmg',
    'Message/hidari.bmg',
    'Message/kaitei.bmg',
    'Message/kaitei_F.bmg',
    'Message/kojima1.bmg',
    'Message/kojima2.bmg',
    'Message/kojima3.bmg',
    'Message/kojima5.bmg',
    'Message/mainselect.bmg',
    'Message/main_isl.bmg',
    'Message/myou.bmg',
    'Message/power.bmg',
    'Message/regular.bmg',
    'Message/sea.bmg',
    'Message/sennin.bmg',
    'Message/ship.bmg',
    'Message/staff.bmg',
    'Message/system.bmg',
    'Message/torii.bmg',
    'Message/wind.bmg',
    'Message/wisdom.bmg',
    'Message/wisdom_dngn.bmg',
]

possible_locations = []

rom = ndspy.rom.NintendoDSRom.fromFile(rom_filename)  # Read ROM from argument
# print(rom.name)
if 'SPIRITTRACKS' in str(rom.name):
    possible_locations = zelda_st_locations  # Spirit Tracks
else:
    possible_locations = zelda_ph_locations  # Phantom Hourglass

my_encoding = "utf-16"  # Default
is_japanese = (language == "Japanese")  # DO NOT TOUCH

# Dump all text files
for file_index in range(0, len(possible_locations)):
    after_message = possible_locations[file_index].split("Message/", 1)[1]  # drop 'Message/'
    filename_out = os.path.join(output_dir, after_message + "_out")

    if os.path.isfile(filename_out):
        os.remove(filename_out)

    bmgData = rom.getFileByName(language + '/' + possible_locations[file_index])  # e.g., Japanese/Message/file.bmg
    bmg = ndspy.bmg.BMG(bmgData)  # Automatically recognised as UTF-16

    for x in range(0, len(bmg.messages)):
        my_string = str(bmg.messages[x])
        my_string = my_string.replace('\r', '')
        my_string = my_string.replace('\n', '\\n')
        my_string = my_string + '\n'

        if is_japanese:
            encoded = my_string.encode(my_encoding)  # Encode to bytes
            with open(filename_out, mode="ab") as writer:  # Write as binary
                writer.write(encoded)
        else:
            with open(filename_out, mode="a", encoding=my_encoding) as writer:  # Write as text
                writer.write(my_string)
