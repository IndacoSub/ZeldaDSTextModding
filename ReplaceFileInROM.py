import sys, os, ndspy.rom, ndspy.bmg, re

# All languages "supported", no need to change anything here

rom_filename = sys.argv[1] # First argument
rom = ndspy.rom.NintendoDSRom.fromFile(rom_filename) # Read ROM from argument
filename_in = sys.argv[2] # Read the filename of the file that replaces the other file
to_replace = sys.argv[3] # The file to be replaced inside the ROM
filename_out = "EditedROM.nds"
red_bytes = b'\x00' # I don't know how to initialize it
with open(filename_in, "rb") as reader: # Read as binary
  red_bytes = reader.read() # The read operation
rom.setFileByName(to_replace, red_bytes) # Replace the file in the ROM
#if os.path.isfile(filename_out): # Remove file if it exists
  #os.remove(filename_out)
rom.saveToFile(filename_out) # Save the ROM
print(to_replace + " replaced with " + filename_in + "!")