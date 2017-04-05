# Zeldomizer

The Legend of Zelda ROM manipulation library.

- Zeldomizer
  - Modifies aspects of the Legend of Zelda ROM
- Breadbox
  - Emulates a 6502 processor at the cycle level
- Disaster
  - Assembles and disassembles 6502 code
- Mimic
  - Emulates the game's NES cartridge mapper, MMC1

## Implemented

- Zeldomizer
  - Edit character dialog
  - Edit ending text
  - Edit dungeons
  - Edit overworld map
  - Render sprites and tiles
- Breadbox
  - Fully emulated
    - Interrupts
	- Undocumented opcodes
- Disaster
  - Assembly
    - Raw opcodes only
  - Disassembly
    - Raw opcodes only
    - Basic static analysis
- Mimic
  - Basic emulation
    - Runs game code headlessly
	- MMC1 implemented minimally

## Todo

- A whole lot more, stay tuned
