import sys, os, ndspy.rom, ndspy.bmg, re

# Language variable is *under this function* (tip: go directly to the end of the file)

# Here below is a list of "escape strings" which appear in the games, and we have to - somehow - keep, in the edited BMGs

def get_escape_by_string(string):

  match string:
  
    case "0:0000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x00\x00'))
    case "0:0100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x01\x00'))
    case "0:0200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x02\x00'))
    case "0:0300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x03\x00'))
    case "0:0a000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x00\x00'))
    case "0:0a000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x01\x00'))
    case "0:0a000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x02\x00'))
    case "0:0a000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x03\x00'))
    case "0:0a000400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x04\x00'))
    case "0:0a000500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x05\x00'))
    case "0:0a000600":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x06\x00'))
    case "0:0a000800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x08\x00'))
    case "0:0a000a00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0a\x00\x0a\x00'))
    case "0:0b000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x00\x00'))
    case "0:0b000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x01\x00'))
    case "0:0b000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x02\x00'))
    case "0:0b000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x03\x00'))
    case "0:0b000500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x05\x00'))
    case "0:0b000f00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x0f\x00'))
    case "0:0b000a00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x0a\x00'))
    case "0:0b001400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x14\x00'))
    case "0:0b001800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x18\x00'))
    case "0:0b001900":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x19\x00'))
    case "0:0b001e00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x1e\x00'))
    case "0:0b003c00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x3c\x00'))
    case "0:0b006400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\x64\x00'))
    case "0:0b00f000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0b\x00\xf0\x00'))
    case "0:0e0014000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0e\x00\x14\x00\x00\x00'))
    case "0:0e001e000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0e\x00\x1e\x00\x00\x00'))
    case "0:0e005a000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0e\x00\x5a\x00\x00\x00'))
    case "0:0f000a00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x0a\x00'))
    case "0:0f001400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x14\x00'))
    case "0:0f001900":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x19\x00'))
    case "0:0f001e00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x1e\x00'))
    case "0:0f002800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x28\x00'))
    case "0:0f002d00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x2d\x00'))
    case "0:0f003200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x32\x00'))
    case "0:0f003300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x33\x00'))
    case "0:0f003700":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x37\x00'))
    case "0:0f003c00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x3c\x00'))
    case "0:0f004600":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x46\x00'))
    case "0:0f004b00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x4b\x00'))
    case "0:0f005300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x53\x00'))
    case "0:0f005a00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x5a\x00'))
    case "0:0f005500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x55\x00'))
    case "0:0f006400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x64\x00'))
    case "0:0f006e00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x6e\x00'))
    case "0:0f007800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x78\x00'))
    case "0:0f009600":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\x96\x00'))
    case "0:0f00a000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xa0\x00'))
    case "0:0f00a500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xa5\x00'))
    case "0:0f00aa00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xaa\x00'))
    case "0:0f00af00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xaf\x00'))
    case "0:0f00b400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xb4\x00'))
    case "0:0f00c800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x0f\x00\xc8\x00'))
    case "0:11000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x01\x00'))
    case "0:11000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x02\x00'))
    case "0:11000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x03\x00'))
    case "0:11000400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x04\x00'))
    case "0:11000500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x05\x00'))
    case "0:11000600":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x06\x00'))
    case "0:11000700":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x07\x00'))
    case "0:11000800":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x11\x00\x08\x00'))
    case "0:1400229e":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x22\x9e'))
    case "0:14002396":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x23\x96'))
    case "0:14003453":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x34\x53'))
    case "0:14004a7d":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x4a\x7d'))
    case "0:1400548b":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x54\x8b'))
    case "0:14006c64":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x6c\x64'))
    case "0:14008028":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x80\x28'))
    case "0:14008a7e":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x8a\x7e'))
    case "0:1400b446":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\xb4\x46'))
    case "0:14009b73":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\x9b\x73'))
    case "0:1400c896":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\xc8\x96'))
    case "0:1400d296":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\xd2\x96'))
    case "0:1400dc96":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x14\x00\xdc\x96'))
    case "0:1500":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x15\x00'))
    case "0:1600":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x16\x00'))
    case "0:17000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x17\x00\x00\x00'))
    case "0:17000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x17\x00\x01\x00'))
    case "0:17000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x17\x00\x02\x00'))
    case "0:17000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x17\x00\x03\x00'))
    case "0:17000400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x17\x00\x04\x00'))
    case "0:19000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x19\x00\x00\x00'))
    case "0:19000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x19\x00\x01\x00'))
    case "0:19000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x19\x00\x02\x00'))
    case "0:19000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x19\x00\x03\x00'))
    case "0:1e000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x1e\x00\x01\x00'))
    case "0:1e000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x1e\x00\x02\x00'))
    case "0:1e000900":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x1e\x00\x09\x00'))
    case "0:1e000b00":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x1e\x00\x0b\x00'))
    case "0:28000000":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x28\x00\x00\x00'))
    case "0:28000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x28\x00\x01\x00'))
    case "0:28000200":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x28\x00\x02\x00'))
    case "0:28000300":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x28\x00\x03\x00'))
    case "0:28000400":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x28\x00\x04\x00'))
    case "0:2900":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x29\x00'))
    case "0:32000100":
      return ndspy.bmg.Message.Escape(0, bytearray(b'\x32\x00\x01\x00'))
    case "1:0000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x00\x00'))
    case "1:0100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x01\x00'))
    case "1:0200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x02\x00'))
    case "1:0300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x03\x00'))
    case "1:0a000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x01\x00'))
    case "1:0a000200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x02\x00'))
    case "1:0a000300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x03\x00'))
    case "1:0a000400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x04\x00'))
    case "1:0a000500":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x05\x00'))
    case "1:0a000600":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x06\x00'))
    case "1:0a000800":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x08\x00'))
    case "1:0a000a00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x0a\x00'))
    case "1:0a000c00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x0c\x00'))
    case "1:0a000d00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x0d\x00'))
    case "1:0a000f00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x0f\x00'))
    case "1:0a001000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x10\x00'))
    case "1:0a001400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x14\x00'))
    case "1:0a001900":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x19\x00'))
    case "1:0a001e00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x1e\x00'))
    case "1:0a002400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x24\x00'))
    case "1:0a003c00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x3c\x00'))
    case "1:0a006400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0a\x00\x64\x00'))
    case "1:0d002d000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0d\x00\x2d\x00\x00\x00'))
    case "1:0d0032000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0d\x00\x32\x00\x00\x00'))
    case "1:0d003c000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0d\x00\x3c\x00\x00\x00'))
    case "1:0d0046000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0d\x00\x46\x00\x00\x00'))
    case "1:0d0055000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0d\x00\x55\x00\x00\x00'))
    case "1:0e000401":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x04\x01'))
    case "1:0e000b01":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x0b\x01'))
    case "1:0e000e01":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x0e\x01'))
    case "1:0e001200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x12\x00'))
    case "1:0e001801":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x18\x01'))
    case "1:0e001e00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x1e\x00'))
    case "1:0e002300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x23\x00'))
    case "1:0e002800":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x28\x00'))
    case "1:0e002d00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x2d\x00'))
    case "1:0e003200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x32\x00'))
    case "1:0e003c00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x3c\x00'))
    case "1:0e004b00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x4b\x00'))
    case "1:0e005000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x50\x00'))
    case "1:0e005500":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x55\x00'))
    case "1:0e005a00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x5a\x00'))
    case "1:0e006400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x64\x00'))
    case "1:0e006e00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x6e\x00'))
    case "1:0e007800":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x78\x00'))
    case "1:0e007d00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x7d\x00'))
    case "1:0e008200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x82\x00'))
    case "1:0e008c00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x8c\x00'))
    case "1:0e009600":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x96\x00'))
    case "1:0e009f00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\x9f\x00'))
    case "1:0e00a000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xa0\x00'))
    case "1:0e00aa00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xaa\x00'))
    case "1:0e00b400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xb4\x00'))
    case "1:0e00b900":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xb9\x00'))
    case "1:0e00be00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xbe\x00'))
    case "1:0e00c300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xc3\x00'))
    case "1:0e00c800":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xc8\x00'))
    case "1:0e00d200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xd2\x00'))
    case "1:0e00dc00":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xdc\x00'))
    case "1:0e00e100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xe1\x00'))
    case "1:0e00e600":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x0e\x00\xe6\x00'))
    case "1:14000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x00\x00'))
    case "1:14000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x01\x00'))
    case "1:14000200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x02\x00'))
    case "1:14000300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x03\x00'))
    case "1:14000400":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x04\x00'))
    case "1:14000500":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x05\x00'))
    case "1:14000600":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x14\x00\x06\x00'))
    case "1:1500100e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x10\x0e'))
    case "1:15001010":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x10\x10'))
    case "1:15002053":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x20\x53'))
    case "1:15002c5f":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x2c\x5f'))
    case "1:15003c7e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x3c\x7e'))
    case "1:15004624":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x46\x24'))
    case "1:15004780":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x47\x80'))
    case "1:15004a8c":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x4a\x8c'))
    case "1:15004e5e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x4e\x5e'))
    case "1:15004f69":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x4f\x69')) # nice
    case "1:15005236":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x52\x36'))
    case "1:1500574d":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x57\x4d'))
    case "1:15005d2a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x5d\x2a'))
    case "1:15006a4d":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x6a\x4d'))
    case "1:15006a88":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x6a\x88'))
    case "1:15007275":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x72\x75'))
    case "1:15007676":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x76\x76'))
    case "1:15007e15":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x7e\x15'))
    case "1:1500802d":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x2d'))
    case "1:1500802e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x2e'))
    case "1:15008078":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x78'))
    case "1:1500807a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x7a'))
    case "1:1500807e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x7e'))
    case "1:1500808c":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x8c'))
    case "1:15008090":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x80\x90'))
    case "1:15008ab8":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x8a\xb8'))
    case "1:1500903a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x90\x3a'))
    case "1:1500918a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x91\x8a'))
    case "1:1500935d":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x93\x5d'))
    case "1:15009478":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x94\x78'))
    case "1:15009679":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x96\x79'))
    case "1:15009c40":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x9c\x40'))
    case "1:15009d28":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x9d\x28'))
    case "1:15009e5a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\x9e\x5a'))
    case "1:1500aa6a":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xaa\x6a'))
    case "1:1500ae86":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xae\x86'))
    case "1:1500c05f":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xc0\x5f'))
    case "1:1500c591":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xc5\x91'))
    case "1:1500c76b":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xc7\x6b'))
    case "1:1500c8a0":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xc8\xa0'))
    case "1:1500ca5e":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xca\x5e'))
    case "1:1500d05c":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xd0\x5c'))
    case "1:1500d3a5":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xd3\xa5'))
    case "1:1500eb12":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x15\x00\xeb\x12'))
    case "1:1600":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x16\x00'))
    case "1:17000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x17\x00\x00\x00'))
    case "1:17000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x17\x00\x01\x00'))
    case "1:17000200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x17\x00\x02\x00'))
    case "1:17000300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x17\x00\x03\x00'))
    case "1:1800":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x18\x00'))
    case "1:1900":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x19\x00'))
    case "1:1a000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1a\x00\x00\x00'))
    case "1:1a000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1a\x00\x01\x00'))
    case "1:1a000200":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1a\x00\x02\x00'))
    case "1:1a000300":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1a\x00\x03\x00'))
    case "1:1b000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1b\x00\x00\x00'))
    case "1:1b000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1b\x00\x01\x00'))
    case "1:1c000000":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1c\x00\x00\x00'))
    case "1:1c000100":
      return ndspy.bmg.Message.Escape(1, bytearray(b'\x1c\x00\x01\x00'))
    case "2:00000000":
      return ndspy.bmg.Message.Escape(2, bytearray(b'\x00\x00\x00\x00'))
    case "2:00000100":
      return ndspy.bmg.Message.Escape(2, bytearray(b'\x00\x00\x01\x00'))
    case "2:00000200":
      return ndspy.bmg.Message.Escape(2, bytearray(b'\x00\x00\x02\x00'))
    case "2:0100":
      return ndspy.bmg.Message.Escape(2, bytearray(b'\x01\x00'))
    case "3:00000000":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x00\x00\x00\x00'))
    case "3:00000100":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x00\x00\x01\x00'))
    case "3:0100":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x01\x00'))
    case "3:01000000":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x01\x00\x00\x00'))
    case "3:03000000":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x03\x00\x00\x00'))
    case "3:04000000":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\x04\x00\x00\x00'))
    case "3:fe000700":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\xfe\x00\x07\x00'))
    case "3:ff000000":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\xff\x00\x00\x00'))
    case "3:ff000100":
      return ndspy.bmg.Message.Escape(3, bytearray(b'\xff\x00\x01\x00'))
    case "4:0000":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x00\x00'))
    case "4:00000000":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x00\x00\x00\x00'))
    case "4:00000100":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x00\x00\x01\x00'))
    case "4:00000500":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x00\x00\x05\x00'))
    case "4:00000700":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x00\x00\x07\x00'))
    case "4:0100":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00'))
    case "4:0200":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x02\x00'))
    case "4:0400":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x04\x00'))
    case "4:03000000":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x03\x00\x00\x00'))
    case "4:03000100":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x03\x00\x01\x00'))
    case "4:03000200":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x03\x00\x02\x00'))
    case "4:010000005200750070006500650073000000520075007000650065000000520075007000650065007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x52\x00\x75\x00\x70\x00\x65\x00\x65\x00\x73\x00\x00\x00\x52\x00\x75\x00\x70\x00\x65\x00\x65\x00\x00\x00\x52\x00\x75\x00\x70\x00\x65\x00\x65\x00\x73\x00'))  
    case "4:0100000052007500700069006500000052007500700069006100000052007500700069006500":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x52\x00\x75\x00\x70\x00\x69\x00\x65\x00\x00\x00\x52\x00\x75\x00\x70\x00\x69\x00\x61\x00\x00\x00\x52\x00\x75\x00\x70\x00\x69\x00\x65\x00'))
    case "4:010000006200690067006c006900650074007400690000006200690067006c0069006500740074006f0000006200690067006c0069006500740074006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x62\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x65\x00\x74\x00\x74\x00\x69\x00\x00\x00\x62\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x65\x00\x74\x00\x74\x00\x6f\x00\x00\x00\x62\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x65\x00\x74\x00\x74\x00\x69\x00'))
    case "4:0100000063006f006c0070006900000063006f006c0070006f00000063006f006c0070006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x63\x00\x6f\x00\x6c\x00\x70\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6c\x00\x70\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6c\x00\x70\x00\x69\x00'))
    case "4:0100000063006f006e00690067006c006900000063006f006e00690067006c0069006f00000063006f006e00690067006c006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00'))
    case "4:0100000070006f0069006e0074007300000070006f0069006e007400000070006f0069006e0074007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x73\x00\x00\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x00\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x73\x00'))
    case "4:0100000070006f007300740063006100720064007300000070006f00730074006300610072006400000070006f007300740063006100720064007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x70\x00\x6f\x00\x73\x00\x74\x00\x63\x00\x61\x00\x72\x00\x64\x00\x73\x00\x00\x00\x70\x00\x6f\x00\x73\x00\x74\x00\x63\x00\x61\x00\x72\x00\x64\x00\x00\x00\x70\x00\x6f\x00\x73\x00\x74\x00\x63\x00\x61\x00\x72\x00\x64\x00\x73\x00'))
    case "4:01000000700075006e00740069000000700075006e0074006f000000700075006e0074006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x69\x00\x00\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x6f\x00\x00\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x69\x00'))
    case "4:01000000740069006d00650073000000740069006d0065000000740069006d0065007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x74\x00\x69\x00\x6d\x00\x65\x00\x73\x00\x00\x00\x74\x00\x69\x00\x6d\x00\x65\x00\x00\x00\x74\x00\x69\x00\x6d\x00\x65\x00\x73\x00'))   
    case "4:010000007300740061006d007000730000007300740061006d00700000007300740061006d0070007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x73\x00\x74\x00\x61\x00\x6d\x00\x70\x00\x73\x00\x00\x00\x73\x00\x74\x00\x61\x00\x6d\x00\x70\x00\x00\x00\x73\x00\x74\x00\x61\x00\x6d\x00\x70\x00\x73\x00'))
    case "4:010000007300750062006d0069007300730069006f006e00730000007300750062006d0069007300730069006f006e0000007300750062006d0069007300730069006f006e007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x00\x00\x73\x00\x75\x00\x62\x00\x6d\x00\x69\x00\x73\x00\x73\x00\x69\x00\x6f\x00\x6e\x00\x73\x00\x00\x00\x73\x00\x75\x00\x62\x00\x6d\x00\x69\x00\x73\x00\x73\x00\x69\x00\x6f\x00\x6e\x00\x00\x00\x73\x00\x75\x00\x62\x00\x6d\x00\x69\x00\x73\x00\x73\x00\x69\x00\x6f\x00\x6e\x00\x73\x00'))
    case "4:0100010063006f006e00690067006c006900000063006f006e00690067006c0069006f00000063006f006e00690067006c006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x01\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00'))
    case "4:0100010070006f0069006e0074007300000070006f0069006e007400000070006f0069006e0074007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x01\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x73\x00\x00\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x00\x00\x70\x00\x6f\x00\x69\x00\x6e\x00\x74\x00\x73\x00'))
    case "4:010001007300650063006f006e006400730000007300650063006f006e00640000007300650063006f006e0064007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x01\x00\x73\x00\x65\x00\x63\x00\x6f\x00\x6e\x00\x64\x00\x73\x00\x00\x00\x73\x00\x65\x00\x63\x00\x6f\x00\x6e\x00\x64\x00\x00\x00\x73\x00\x65\x00\x63\x00\x6f\x00\x6e\x00\x64\x00\x73\x00'))
    case "4:01000100700075006e00740069000000700075006e0074006f000000700075006e0074006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x01\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x69\x00\x00\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x6f\x00\x00\x00\x70\x00\x75\x00\x6e\x00\x74\x00\x69\x00'))
    case "4:0100020063006f006e00690067006c006900000063006f006e00690067006c0069006f00000063006f006e00690067006c006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x02\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00'))
    case "4:0100030063006f006e00690067006c006900000063006f006e00690067006c0069006f00000063006f006e00690067006c006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x03\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00'))
    case "4:0100040063006f006e00690067006c006900000063006f006e00690067006c0069006f00000063006f006e00690067006c006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x01\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00\x6f\x00\x00\x00\x63\x00\x6f\x00\x6e\x00\x69\x00\x67\x00\x6c\x00\x69\x00'))  
    case "4:0100050052007500700069006500000052007500700069006100000052007500700069006500":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x05\x00\x52\x00\x75\x00\x70\x00\x69\x00\x65\x00\x00\x00\x52\x00\x75\x00\x70\x00\x69\x00\x61\x00\x00\x00\x52\x00\x75\x00\x70\x00\x69\x00\x65\x00'))
    case "4:01000900730063006f006e007400720069000000730063006f006e00740072006f000000730063006f006e00740072006900":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x09\x00\x73\x00\x63\x00\x6f\x00\x6e\x00\x74\x00\x72\x00\x69\x00\x00\x00\x73\x00\x63\x00\x6f\x00\x6e\x00\x74\x00\x72\x00\x6f\x00\x00\x00\x73\x00\x63\x00\x6f\x00\x6e\x00\x74\x00\x72\x00\x69\x00'))
    case "4:0100090076006f006c0074006500000076006f006c0074006100000076006f006c0074006500":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x09\x00\x76\x00\x6f\x00\x6c\x00\x74\x00\x65\x00\x00\x00\x76\x00\x6f\x00\x6c\x00\x74\x00\x61\x00\x00\x00\x76\x00\x6f\x00\x6c\x00\x74\x00\x65\x00'))  
    case "4:01000900770069006e0073000000770069006e000000770069006e007300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x01\x00\x09\x00\x77\x00\x69\x00\x6e\x00\x73\x00\x00\x00\x77\x00\x69\x00\x6e\x00\x00\x00\x77\x00\x69\x00\x6e\x00\x73\x00'))
    case "4:0300":
      return ndspy.bmg.Message.Escape(4, bytearray(b'\x03\x00'))
    case "5:00000000":
      return ndspy.bmg.Message.Escape(5, bytearray(b'\x00\x00\x00\x00'))
    case "5:00000100":
      return ndspy.bmg.Message.Escape(5, bytearray(b'\x00\x00\x01\x00'))
    case "100:0000":
      return ndspy.bmg.Message.Escape(100, bytearray(b'\x00\x00'))
    case "100:0100":
      return ndspy.bmg.Message.Escape(100, bytearray(b'\x01\x00'))
    case "100:0200":
      return ndspy.bmg.Message.Escape(100, bytearray(b'\x02\x00'))
    case "100:0300":
      return ndspy.bmg.Message.Escape(100, bytearray(b'\x03\x00'))
    case "254:0000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x00\x00'))
    case "254:01000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x01\x00\x00\x00'))
    case "254:02000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x02\x00\x01\x00'))
    case "254:02000200":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x02\x00\x02\x00'))
    case "254:03000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x03\x00\x00\x00'))
    case "254:03000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x03\x00\x01\x00'))
    case "254:06000200":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x06\x00\x02\x00'))
    case "254:08000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x08\x00\x00\x00'))
    case "254:08000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x08\x00\x01\x00'))
    case "254:08000300":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x08\x00\x03\x00'))
    case "254:0a000400":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x04\x00'))
    case "254:0a000500":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x05\x00'))
    case "254:0a000600":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x06\x00'))
    case "254:0a000700":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x07\x00'))
    case "254:0a000800":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x08\x00'))
    case "254:0a000900":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0a\x00\x09\x00'))
    case "254:0b000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x00\x00'))
    case "254:0b000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x01\x00'))
    case "254:0b000200":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x02\x00'))
    case "254:0b000300":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x03\x00'))
    case "254:0b000400":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x04\x00'))
    case "254:0b000600":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x06\x00'))
    case "254:0b000700":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x07\x00'))
    case "254:0b000800":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x08\x00'))
    case "254:0b000900":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0b\x00\x09\x00'))
    case "254:0c000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x00\x00'))
    case "254:0c000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x01\x00'))
    case "254:0c000200":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x02\x00'))
    case "254:0c000300":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x03\x00'))
    case "254:0c000400":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x04\x00'))
    case "254:0c000500":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0c\x00\x05\x00'))
    case "254:0d000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0d\x00\x00\x00'))
    case "254:0d000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0d\x00\x01\x00'))
    case "254:0d000500":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0d\x00\x05\x00'))
    case "254:0d000900":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0d\x00\x09\x00'))
    case "254:0e000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0e\x00\x00\x00'))
    case "254:0e000100":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0e\x00\x01\x00'))
    case "254:0f000000":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x0f\x00\x00\x00'))
    case "254:1500":
      return ndspy.bmg.Message.Escape(254, bytearray(b'\x15\x00'))
    case "255:00000000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00\x00\x00'))
    case "255:00000100":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00\x01\x00'))
    case "255:00000200":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00\x02\x00'))
    case "255:00000300":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00\x03\x00'))
    case "255:00000400":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00\x04\x00'))
    case "255:02000000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x00\x00'))
    case "255:02000146307f3000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x01\x46\x30\x7f\x30\x00'))
    case "255:020001723000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x01\x72\x30\x00'))
    case "255:020001d530a930fc30b93000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x01\xd5\x30\xa9\x30\xfc\x30\xb9\x30\x00'))
    case "255:0200025730853063307130643000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x02\x57\x30\x85\x30\x63\x30\x71\x30\x64\x30\x00'))
    case "255:02000260304430613000":
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x02\x00\x02\x60\x30\x44\x30\x61\x30\x00'))
    
    case _:
      print("Unrecognised string: " + string)
      return ndspy.bmg.Message.Escape(255, bytearray(b'\x00\x00'))
      
    # return ndspy.bmg.Message.Escape(4, bytearray(b'\x'))
  pass

language = "Japanese"

rom_filename = sys.argv[1]
rom = ndspy.rom.NintendoDSRom.fromFile(rom_filename) # Read ROM from argument
filename_in = sys.argv[2] # Read the BMG_Out (txt) filename
to_replace = sys.argv[3]
filename_out = filename_in.strip("_out") # Remove "_out" from the name
messages = [] # Empty list
bmgData = rom.getFileByName(language + to_replace)
bmg = ndspy.bmg.BMG(bmgData)
with open(filename_in, "r", encoding="utf-16") as reader: # Read file as utf-16
  print("file red: " + filename_in)
  all_lines = reader.readlines() # Read lines
  for line in all_lines: # Iterate lines
    #mes_before = line[0 : len(line) - 1] # Remove \n
    messages.append(line) # Add line from file
print(str(len(messages)) + " lines from .bmg_out VS " + str(len(bmg.messages)) + " from the ROM")
last_valid_message = bmg.messages[0]
for msg_index in range (0, len(messages)):

  if msg_index >= len(bmg.messages):
    bmg.messages.append(last_valid_message)
  else:
    last_valid_message = bmg.messages[msg_index]
    
  specific_line = messages[msg_index]
  specific_line = specific_line.strip().strip('﻿').replace("\\n", "\n")
  contains_escapes = specific_line.find("[") != -1 and specific_line.find("]") != -1
  if contains_escapes:
    bmg.messages[msg_index].stringParts.clear()
    while(contains_escapes):
      escape_begin_index = specific_line.find("[")
      if escape_begin_index > 0:
        escape_begin = specific_line[: escape_begin_index]
        bmg.messages[msg_index].stringParts.append(escape_begin)
      specific_line = specific_line[escape_begin_index + 1 :]
      escape_end_index = specific_line.find("]")
      escape_str = specific_line[: escape_end_index]
      converted_escape = get_escape_by_string(escape_str)
      bmg.messages[msg_index].stringParts.append(converted_escape)
      specific_line = specific_line[escape_end_index + 1 :]
      contains_escapes = specific_line.find("[") != -1 and specific_line.find("]") != -1
    if len(specific_line) > 0:
      bmg.messages[msg_index].stringParts.append(specific_line)
  else:
    bmg.messages[msg_index].stringParts = [specific_line]
    
# Debug    
#for b_message in bmg.messages:
  #print("\"" + str(b_message) + "\"")
  #print(str(ord(str(b_message)[0])))
  
if os.path.isfile(filename_out): # Remove file if it existds
  os.remove(filename_out)
rom.setFileByName(language + to_replace, bmg.save())
rom.saveToFile('EditedROM.nds')