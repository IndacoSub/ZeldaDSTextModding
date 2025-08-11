import sys, os, ndspy.rom, ndspy.bmg, re

# All languages "supported", no need to change anything here

# --- Usage Help ---
if len(sys.argv) < 4:
    print("Usage: ReplaceFileInROM.py <ROM.nds> <replacement_file> <ROM path in ROM> [output.nds]")
    sys.exit(1)

# --- Arguments ---
rom_filename = sys.argv[1]
filename_in = sys.argv[2]
to_replace = sys.argv[3]
filename_out = sys.argv[4] if len(sys.argv) > 4 else "EditedROM.nds"

print(f"[INFO] Loading ROM: {rom_filename}")
print(f"[INFO] Replacing in ROM path: {to_replace}")
print(f"[INFO] Replacement file: {filename_in}")
print(f"[INFO] Output ROM: {filename_out}")

# --- Check file existence ---
if not os.path.isfile(filename_in):
    print(f"[SKIP] File not found: {filename_in} — skipping replacement of {to_replace}")
    sys.exit(0)

# --- Read replacement data ---
try:
    with open(filename_in, "rb") as reader:
        red_bytes = reader.read()

    # Load ROM and apply replacement
    rom = ndspy.rom.NintendoDSRom.fromFile(rom_filename)
    rom.setFileByName(to_replace, red_bytes)
    rom.saveToFile(filename_out)

    print(f"[DONE] ROM saved with replacement: {filename_out}")

except Exception as e:
    print(f"[ERROR] Failed to replace {to_replace}: {str(e)}")
    sys.exit(1)