﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Disaster.Assembly
{
    internal static class OpcodeTables
    {
        public static (AddressingMode AddressingMode, Opcode Opcode) Decode(int data)
        {
            return (AddressingMode: AddressingModes[data], Opcode: Opcodes[data]);
        }

        public static int Encode(AddressingMode addressingMode, Opcode opcode)
        {
            // Find out what possible combination of addressing mode and opcode can exist.
            var valuesByMode = AddressingModes
                .Where(kv => kv.Value == addressingMode)
                .Select(kv => kv.Key);
            var valuesByOpcode = Opcodes
                .Where(kv => kv.Value == opcode)
                .Select(kv => kv.Key);
            var possibleOpcodes = valuesByMode
                .Intersect(valuesByOpcode)
                .ToArray();

            // If there are none, fail.
            if (!possibleOpcodes.Any())
                throw new Exception($"No possible opcodes for {opcode} {addressingMode}.");

            // Prefer to use official opcodes if possible.
            var possiblePreferredOpcodes = PreferredOpcodes
                .Intersect(possibleOpcodes)
                .ToArray();
            if (possiblePreferredOpcodes.Any())
                return possiblePreferredOpcodes.First();

            // It's an undocumented opcode, most times it doesn't matter which is used.
            return possibleOpcodes.First();
        }

        /// <summary>
        /// Opcodes deemed "official" by MOS.
        /// </summary>
        private static readonly HashSet<int> PreferredOpcodes = new HashSet<int>
        {
            0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x90, 0xA0, 0xB0, 0xC0, 0xD0, 0xE0, 0xF0,
            0x01, 0x11, 0x21, 0x31, 0x41, 0x51, 0x61, 0x71, 0x81, 0x91, 0xA1, 0xB1, 0xC1, 0xD1, 0xE1, 0xF1,
            0xA2,
            0x24, 0x84, 0x94, 0xA4, 0xB4, 0xC4, 0xE4,
            0x05, 0x15, 0x25, 0x35, 0x45, 0x55, 0x65, 0x75, 0x85, 0x95, 0xA5, 0xB5, 0xC5, 0xD5, 0xE5, 0xF5,
            0x06, 0x16, 0x26, 0x36, 0x46, 0x56, 0x66, 0x76, 0x86, 0x96, 0xA6, 0xB6, 0xC6, 0xD6, 0xE6, 0xF6,
            0x08, 0x18, 0x28, 0x38, 0x48, 0x58, 0x68, 0x78, 0x88, 0x98, 0xA8, 0xB8, 0xC8, 0xD8, 0xE8, 0xF8,
            0x09, 0x19, 0x29, 0x39, 0x49, 0x59, 0x69, 0x79, 0x99, 0xA9, 0xB9, 0xC9, 0xD9, 0xE9, 0xF9,
            0x0A, 0x2A, 0x4A, 0x6A, 0x8A, 0x9A, 0xAA, 0xBA, 0xCA, 0xEA,
            0x2C, 0x4C, 0x6C, 0x8C, 0xAC, 0xBC, 0xCC, 0xEC,
            0x0D, 0x1D, 0x2D, 0x3D, 0x4D, 0x5D, 0x6D, 0x7D, 0x8D, 0x9D, 0xAD, 0xBD, 0xCD, 0xDD, 0xED, 0xFD,
            0x0E, 0x1E, 0x2E, 0x3E, 0x4E, 0x5E, 0x6E, 0x7E, 0x8E, 0xAE, 0xBE, 0xCE, 0xDE, 0xEE, 0xFE
        };

        public static readonly Dictionary<int, AddressingMode> AddressingModes = new Dictionary<int, AddressingMode>
        {
            { 0x00, AddressingMode.Implied },
            { 0x01, AddressingMode.IndirectZeroPageX },
            { 0x02, AddressingMode.Implied },
            { 0x03, AddressingMode.IndirectZeroPageX },
            { 0x04, AddressingMode.ZeroPage },
            { 0x05, AddressingMode.ZeroPage },
            { 0x06, AddressingMode.ZeroPage },
            { 0x07, AddressingMode.ZeroPage },
            { 0x08, AddressingMode.Implied },
            { 0x09, AddressingMode.Immediate },
            { 0x0A, AddressingMode.Accumulator },
            { 0x0B, AddressingMode.Immediate },
            { 0x0C, AddressingMode.Absolute },
            { 0x0D, AddressingMode.Absolute },
            { 0x0E, AddressingMode.Absolute },
            { 0x0F, AddressingMode.Absolute },
            { 0x10, AddressingMode.Relative },
            { 0x11, AddressingMode.IndirectZeroPageY },
            { 0x12, AddressingMode.Implied },
            { 0x13, AddressingMode.IndirectZeroPageY },
            { 0x14, AddressingMode.ZeroPageX },
            { 0x15, AddressingMode.ZeroPageX },
            { 0x16, AddressingMode.ZeroPageX },
            { 0x17, AddressingMode.ZeroPageX },
            { 0x18, AddressingMode.Implied },
            { 0x19, AddressingMode.AbsoluteY },
            { 0x1A, AddressingMode.Implied },
            { 0x1B, AddressingMode.AbsoluteY },
            { 0x1C, AddressingMode.AbsoluteX },
            { 0x1D, AddressingMode.AbsoluteX },
            { 0x1E, AddressingMode.AbsoluteX },
            { 0x1F, AddressingMode.AbsoluteX },
            { 0x20, AddressingMode.Absolute },
            { 0x21, AddressingMode.IndirectZeroPageX },
            { 0x22, AddressingMode.Implied },
            { 0x23, AddressingMode.IndirectZeroPageX },
            { 0x24, AddressingMode.ZeroPage },
            { 0x25, AddressingMode.ZeroPage },
            { 0x26, AddressingMode.ZeroPage },
            { 0x27, AddressingMode.ZeroPage },
            { 0x28, AddressingMode.Implied },
            { 0x29, AddressingMode.Immediate },
            { 0x2A, AddressingMode.Accumulator },
            { 0x2B, AddressingMode.Immediate },
            { 0x2C, AddressingMode.Absolute },
            { 0x2D, AddressingMode.Absolute },
            { 0x2E, AddressingMode.Absolute },
            { 0x2F, AddressingMode.Absolute },
            { 0x30, AddressingMode.Relative },
            { 0x31, AddressingMode.IndirectZeroPageY },
            { 0x32, AddressingMode.Implied },
            { 0x33, AddressingMode.IndirectZeroPageY },
            { 0x34, AddressingMode.ZeroPageX },
            { 0x35, AddressingMode.ZeroPageX },
            { 0x36, AddressingMode.ZeroPageX },
            { 0x37, AddressingMode.ZeroPageX },
            { 0x38, AddressingMode.Implied },
            { 0x39, AddressingMode.AbsoluteY },
            { 0x3A, AddressingMode.Implied },
            { 0x3B, AddressingMode.AbsoluteY },
            { 0x3C, AddressingMode.AbsoluteX },
            { 0x3D, AddressingMode.AbsoluteX },
            { 0x3E, AddressingMode.AbsoluteX },
            { 0x3F, AddressingMode.AbsoluteX },
            { 0x40, AddressingMode.Implied },
            { 0x41, AddressingMode.IndirectZeroPageX },
            { 0x42, AddressingMode.Implied },
            { 0x43, AddressingMode.IndirectZeroPageX },
            { 0x44, AddressingMode.ZeroPage },
            { 0x45, AddressingMode.ZeroPage },
            { 0x46, AddressingMode.ZeroPage },
            { 0x47, AddressingMode.ZeroPage },
            { 0x48, AddressingMode.Implied },
            { 0x49, AddressingMode.Immediate },
            { 0x4A, AddressingMode.Accumulator },
            { 0x4B, AddressingMode.Immediate },
            { 0x4C, AddressingMode.Absolute },
            { 0x4D, AddressingMode.Absolute },
            { 0x4E, AddressingMode.Absolute },
            { 0x4F, AddressingMode.Absolute },
            { 0x50, AddressingMode.Relative },
            { 0x51, AddressingMode.IndirectZeroPageY },
            { 0x52, AddressingMode.Implied },
            { 0x53, AddressingMode.IndirectZeroPageY },
            { 0x54, AddressingMode.ZeroPageX },
            { 0x55, AddressingMode.ZeroPageX },
            { 0x56, AddressingMode.ZeroPageX },
            { 0x57, AddressingMode.ZeroPageX },
            { 0x58, AddressingMode.Implied },
            { 0x59, AddressingMode.AbsoluteY },
            { 0x5A, AddressingMode.Implied },
            { 0x5B, AddressingMode.AbsoluteY },
            { 0x5C, AddressingMode.AbsoluteX },
            { 0x5D, AddressingMode.AbsoluteX },
            { 0x5E, AddressingMode.AbsoluteX },
            { 0x5F, AddressingMode.AbsoluteX },
            { 0x60, AddressingMode.Implied },
            { 0x61, AddressingMode.IndirectZeroPageX },
            { 0x62, AddressingMode.Implied },
            { 0x63, AddressingMode.IndirectZeroPageX },
            { 0x64, AddressingMode.ZeroPage },
            { 0x65, AddressingMode.ZeroPage },
            { 0x66, AddressingMode.ZeroPage },
            { 0x67, AddressingMode.ZeroPage },
            { 0x68, AddressingMode.Implied },
            { 0x69, AddressingMode.Immediate },
            { 0x6A, AddressingMode.Accumulator },
            { 0x6B, AddressingMode.Immediate },
            { 0x6C, AddressingMode.Indirect },
            { 0x6D, AddressingMode.Absolute },
            { 0x6E, AddressingMode.Absolute },
            { 0x6F, AddressingMode.Absolute },
            { 0x70, AddressingMode.Relative },
            { 0x71, AddressingMode.IndirectZeroPageY },
            { 0x72, AddressingMode.Implied },
            { 0x73, AddressingMode.IndirectZeroPageY },
            { 0x74, AddressingMode.ZeroPageX },
            { 0x75, AddressingMode.ZeroPageX },
            { 0x76, AddressingMode.ZeroPageX },
            { 0x77, AddressingMode.ZeroPageX },
            { 0x78, AddressingMode.Implied },
            { 0x79, AddressingMode.AbsoluteY },
            { 0x7A, AddressingMode.Implied },
            { 0x7B, AddressingMode.AbsoluteY },
            { 0x7C, AddressingMode.AbsoluteX },
            { 0x7D, AddressingMode.AbsoluteX },
            { 0x7E, AddressingMode.AbsoluteX },
            { 0x7F, AddressingMode.AbsoluteX },
            { 0x80, AddressingMode.Immediate },
            { 0x81, AddressingMode.IndirectZeroPageX },
            { 0x82, AddressingMode.Immediate },
            { 0x83, AddressingMode.IndirectZeroPageX },
            { 0x84, AddressingMode.ZeroPage },
            { 0x85, AddressingMode.ZeroPage },
            { 0x86, AddressingMode.ZeroPage },
            { 0x87, AddressingMode.ZeroPage },
            { 0x88, AddressingMode.Implied },
            { 0x89, AddressingMode.Immediate },
            { 0x8A, AddressingMode.Implied },
            { 0x8B, AddressingMode.Immediate },
            { 0x8C, AddressingMode.Absolute },
            { 0x8D, AddressingMode.Absolute },
            { 0x8E, AddressingMode.Absolute },
            { 0x8F, AddressingMode.Absolute },
            { 0x90, AddressingMode.Relative },
            { 0x91, AddressingMode.IndirectZeroPageY },
            { 0x92, AddressingMode.Implied },
            { 0x93, AddressingMode.IndirectZeroPageY },
            { 0x94, AddressingMode.ZeroPageX },
            { 0x95, AddressingMode.ZeroPageX },
            { 0x96, AddressingMode.ZeroPageY },
            { 0x97, AddressingMode.ZeroPageY },
            { 0x98, AddressingMode.Implied },
            { 0x99, AddressingMode.AbsoluteY },
            { 0x9A, AddressingMode.Implied },
            { 0x9B, AddressingMode.AbsoluteY },
            { 0x9C, AddressingMode.AbsoluteX },
            { 0x9D, AddressingMode.AbsoluteX },
            { 0x9E, AddressingMode.AbsoluteY },
            { 0x9F, AddressingMode.AbsoluteY },
            { 0xA0, AddressingMode.Immediate },
            { 0xA1, AddressingMode.IndirectZeroPageX },
            { 0xA2, AddressingMode.Immediate },
            { 0xA3, AddressingMode.IndirectZeroPageX },
            { 0xA4, AddressingMode.ZeroPage },
            { 0xA5, AddressingMode.ZeroPage },
            { 0xA6, AddressingMode.ZeroPage },
            { 0xA7, AddressingMode.ZeroPage },
            { 0xA8, AddressingMode.Implied },
            { 0xA9, AddressingMode.Immediate },
            { 0xAA, AddressingMode.Implied },
            { 0xAB, AddressingMode.Immediate },
            { 0xAC, AddressingMode.Absolute },
            { 0xAD, AddressingMode.Absolute },
            { 0xAE, AddressingMode.Absolute },
            { 0xAF, AddressingMode.Absolute },
            { 0xB0, AddressingMode.Relative },
            { 0xB1, AddressingMode.IndirectZeroPageY },
            { 0xB2, AddressingMode.Implied },
            { 0xB3, AddressingMode.IndirectZeroPageY },
            { 0xB4, AddressingMode.ZeroPageX },
            { 0xB5, AddressingMode.ZeroPageX },
            { 0xB6, AddressingMode.ZeroPageY },
            { 0xB7, AddressingMode.ZeroPageY },
            { 0xB8, AddressingMode.Implied },
            { 0xB9, AddressingMode.AbsoluteY },
            { 0xBA, AddressingMode.Implied },
            { 0xBB, AddressingMode.AbsoluteY },
            { 0xBC, AddressingMode.AbsoluteX },
            { 0xBD, AddressingMode.AbsoluteX },
            { 0xBE, AddressingMode.AbsoluteY },
            { 0xBF, AddressingMode.AbsoluteY },
            { 0xC0, AddressingMode.Immediate },
            { 0xC1, AddressingMode.IndirectZeroPageX },
            { 0xC2, AddressingMode.Immediate },
            { 0xC3, AddressingMode.IndirectZeroPageX },
            { 0xC4, AddressingMode.ZeroPage },
            { 0xC5, AddressingMode.ZeroPage },
            { 0xC6, AddressingMode.ZeroPage },
            { 0xC7, AddressingMode.ZeroPage },
            { 0xC8, AddressingMode.Implied },
            { 0xC9, AddressingMode.Immediate },
            { 0xCA, AddressingMode.Implied },
            { 0xCB, AddressingMode.Immediate },
            { 0xCC, AddressingMode.Absolute },
            { 0xCD, AddressingMode.Absolute },
            { 0xCE, AddressingMode.Absolute },
            { 0xCF, AddressingMode.Absolute },
            { 0xD0, AddressingMode.Relative },
            { 0xD1, AddressingMode.IndirectZeroPageY },
            { 0xD2, AddressingMode.Implied },
            { 0xD3, AddressingMode.IndirectZeroPageY },
            { 0xD4, AddressingMode.ZeroPageX },
            { 0xD5, AddressingMode.ZeroPageX },
            { 0xD6, AddressingMode.ZeroPageX },
            { 0xD7, AddressingMode.ZeroPageX },
            { 0xD8, AddressingMode.Implied },
            { 0xD9, AddressingMode.AbsoluteY },
            { 0xDA, AddressingMode.Implied },
            { 0xDB, AddressingMode.AbsoluteY },
            { 0xDC, AddressingMode.AbsoluteX },
            { 0xDD, AddressingMode.AbsoluteX },
            { 0xDE, AddressingMode.AbsoluteX },
            { 0xDF, AddressingMode.AbsoluteX },
            { 0xE0, AddressingMode.Immediate },
            { 0xE1, AddressingMode.IndirectZeroPageX },
            { 0xE2, AddressingMode.Immediate },
            { 0xE3, AddressingMode.IndirectZeroPageX },
            { 0xE4, AddressingMode.ZeroPage },
            { 0xE5, AddressingMode.ZeroPage },
            { 0xE6, AddressingMode.ZeroPage },
            { 0xE7, AddressingMode.ZeroPage },
            { 0xE8, AddressingMode.Implied },
            { 0xE9, AddressingMode.Immediate },
            { 0xEA, AddressingMode.Implied },
            { 0xEB, AddressingMode.Immediate },
            { 0xEC, AddressingMode.Absolute },
            { 0xED, AddressingMode.Absolute },
            { 0xEE, AddressingMode.Absolute },
            { 0xEF, AddressingMode.Absolute },
            { 0xF0, AddressingMode.Relative },
            { 0xF1, AddressingMode.IndirectZeroPageY },
            { 0xF2, AddressingMode.Implied },
            { 0xF3, AddressingMode.IndirectZeroPageY },
            { 0xF4, AddressingMode.ZeroPageX },
            { 0xF5, AddressingMode.ZeroPageX },
            { 0xF6, AddressingMode.ZeroPageX },
            { 0xF7, AddressingMode.ZeroPageX },
            { 0xF8, AddressingMode.Implied },
            { 0xF9, AddressingMode.AbsoluteY },
            { 0xFA, AddressingMode.Implied },
            { 0xFB, AddressingMode.AbsoluteY },
            { 0xFC, AddressingMode.AbsoluteX },
            { 0xFD, AddressingMode.AbsoluteX },
            { 0xFE, AddressingMode.AbsoluteX },
            { 0xFF, AddressingMode.AbsoluteX }
        };

        private static readonly Dictionary<int, Opcode> Opcodes = new Dictionary<int, Opcode>
        {
            { 0x00, Opcode.Brk },
            { 0x01, Opcode.Ora },
            { 0x02, Opcode.Kil },
            { 0x03, Opcode.Slo },
            { 0x04, Opcode.Nop },
            { 0x05, Opcode.Ora },
            { 0x06, Opcode.Asl },
            { 0x07, Opcode.Slo },
            { 0x08, Opcode.Php },
            { 0x09, Opcode.Ora },
            { 0x0A, Opcode.Asl },
            { 0x0B, Opcode.Anc },
            { 0x0C, Opcode.Nop },
            { 0x0D, Opcode.Ora },
            { 0x0E, Opcode.Asl },
            { 0x0F, Opcode.Slo },
            { 0x10, Opcode.Bpl },
            { 0x11, Opcode.Ora },
            { 0x12, Opcode.Kil },
            { 0x13, Opcode.Slo },
            { 0x14, Opcode.Nop },
            { 0x15, Opcode.Ora },
            { 0x16, Opcode.Asl },
            { 0x17, Opcode.Slo },
            { 0x18, Opcode.Clc },
            { 0x19, Opcode.Ora },
            { 0x1A, Opcode.Nop },
            { 0x1B, Opcode.Slo },
            { 0x1C, Opcode.Nop },
            { 0x1D, Opcode.Ora },
            { 0x1E, Opcode.Asl },
            { 0x1F, Opcode.Slo },
            { 0x20, Opcode.Jsr },
            { 0x21, Opcode.And },
            { 0x22, Opcode.Kil },
            { 0x23, Opcode.Rla },
            { 0x24, Opcode.Bit },
            { 0x25, Opcode.And },
            { 0x26, Opcode.Rol },
            { 0x27, Opcode.Rla },
            { 0x28, Opcode.Plp },
            { 0x29, Opcode.And },
            { 0x2A, Opcode.Rol },
            { 0x2B, Opcode.Anc },
            { 0x2C, Opcode.Bit },
            { 0x2D, Opcode.And },
            { 0x2E, Opcode.Rol },
            { 0x2F, Opcode.Rla },
            { 0x30, Opcode.Bmi },
            { 0x31, Opcode.And },
            { 0x32, Opcode.Kil },
            { 0x33, Opcode.Rla },
            { 0x34, Opcode.Nop },
            { 0x35, Opcode.And },
            { 0x36, Opcode.Rol },
            { 0x37, Opcode.Rla },
            { 0x38, Opcode.Sec },
            { 0x39, Opcode.And },
            { 0x3A, Opcode.Nop },
            { 0x3B, Opcode.Rla },
            { 0x3C, Opcode.Nop },
            { 0x3D, Opcode.And },
            { 0x3E, Opcode.Rol },
            { 0x3F, Opcode.Rla },
            { 0x40, Opcode.Rti },
            { 0x41, Opcode.Eor },
            { 0x42, Opcode.Kil },
            { 0x43, Opcode.Sre },
            { 0x44, Opcode.Nop },
            { 0x45, Opcode.Eor },
            { 0x46, Opcode.Lsr },
            { 0x47, Opcode.Sre },
            { 0x48, Opcode.Pha },
            { 0x49, Opcode.Eor },
            { 0x4A, Opcode.Lsr },
            { 0x4B, Opcode.Alr },
            { 0x4C, Opcode.Jmp },
            { 0x4D, Opcode.Eor },
            { 0x4E, Opcode.Lsr },
            { 0x4F, Opcode.Sre },
            { 0x50, Opcode.Bvc },
            { 0x51, Opcode.Eor },
            { 0x52, Opcode.Kil },
            { 0x53, Opcode.Sre },
            { 0x54, Opcode.Nop },
            { 0x55, Opcode.Eor },
            { 0x56, Opcode.Lsr },
            { 0x57, Opcode.Sre },
            { 0x58, Opcode.Cli },
            { 0x59, Opcode.Eor },
            { 0x5A, Opcode.Nop },
            { 0x5B, Opcode.Sre },
            { 0x5C, Opcode.Nop },
            { 0x5D, Opcode.Eor },
            { 0x5E, Opcode.Lsr },
            { 0x5F, Opcode.Sre },
            { 0x60, Opcode.Rts },
            { 0x61, Opcode.Adc },
            { 0x62, Opcode.Kil },
            { 0x63, Opcode.Rra },
            { 0x64, Opcode.Nop },
            { 0x65, Opcode.Adc },
            { 0x66, Opcode.Ror },
            { 0x67, Opcode.Rra },
            { 0x68, Opcode.Pla },
            { 0x69, Opcode.Adc },
            { 0x6A, Opcode.Ror },
            { 0x6B, Opcode.Arr },
            { 0x6C, Opcode.Jmp },
            { 0x6D, Opcode.Adc },
            { 0x6E, Opcode.Ror },
            { 0x6F, Opcode.Rra },
            { 0x70, Opcode.Bvs },
            { 0x71, Opcode.Adc },
            { 0x72, Opcode.Kil },
            { 0x73, Opcode.Rra },
            { 0x74, Opcode.Nop },
            { 0x75, Opcode.Adc },
            { 0x76, Opcode.Ror },
            { 0x77, Opcode.Rra },
            { 0x78, Opcode.Sei },
            { 0x79, Opcode.Adc },
            { 0x7A, Opcode.Nop },
            { 0x7B, Opcode.Rra },
            { 0x7C, Opcode.Nop },
            { 0x7D, Opcode.Adc },
            { 0x7E, Opcode.Ror },
            { 0x7F, Opcode.Rra },
            { 0x80, Opcode.Nop },
            { 0x81, Opcode.Sta },
            { 0x82, Opcode.Nop },
            { 0x83, Opcode.Sax },
            { 0x84, Opcode.Sty },
            { 0x85, Opcode.Sta },
            { 0x86, Opcode.Stx },
            { 0x87, Opcode.Sax },
            { 0x88, Opcode.Dey },
            { 0x89, Opcode.Nop },
            { 0x8A, Opcode.Txa },
            { 0x8B, Opcode.Xaa },
            { 0x8C, Opcode.Sty },
            { 0x8D, Opcode.Sta },
            { 0x8E, Opcode.Stx },
            { 0x8F, Opcode.Sax },
            { 0x90, Opcode.Bcc },
            { 0x91, Opcode.Sta },
            { 0x92, Opcode.Kil },
            { 0x93, Opcode.Ahx },
            { 0x94, Opcode.Sty },
            { 0x95, Opcode.Sta },
            { 0x96, Opcode.Stx },
            { 0x97, Opcode.Sax },
            { 0x98, Opcode.Tya },
            { 0x99, Opcode.Sta },
            { 0x9A, Opcode.Txs },
            { 0x9B, Opcode.Tas },
            { 0x9C, Opcode.Shy },
            { 0x9D, Opcode.Sta },
            { 0x9E, Opcode.Shx },
            { 0x9F, Opcode.Ahx },
            { 0xA0, Opcode.Ldy },
            { 0xA1, Opcode.Lda },
            { 0xA2, Opcode.Ldx },
            { 0xA3, Opcode.Lax },
            { 0xA4, Opcode.Ldy },
            { 0xA5, Opcode.Lda },
            { 0xA6, Opcode.Ldx },
            { 0xA7, Opcode.Lax },
            { 0xA8, Opcode.Tay },
            { 0xA9, Opcode.Lda },
            { 0xAA, Opcode.Tax },
            { 0xAB, Opcode.Lax },
            { 0xAC, Opcode.Ldy },
            { 0xAD, Opcode.Lda },
            { 0xAE, Opcode.Ldx },
            { 0xAF, Opcode.Lax },
            { 0xB0, Opcode.Bcs },
            { 0xB1, Opcode.Lda },
            { 0xB2, Opcode.Kil },
            { 0xB3, Opcode.Lax },
            { 0xB4, Opcode.Ldy },
            { 0xB5, Opcode.Lda },
            { 0xB6, Opcode.Ldx },
            { 0xB7, Opcode.Lax },
            { 0xB8, Opcode.Clv },
            { 0xB9, Opcode.Lda },
            { 0xBA, Opcode.Tsx },
            { 0xBB, Opcode.Las },
            { 0xBC, Opcode.Ldy },
            { 0xBD, Opcode.Lda },
            { 0xBE, Opcode.Ldx },
            { 0xBF, Opcode.Lax },
            { 0xC0, Opcode.Cpy },
            { 0xC1, Opcode.Cmp },
            { 0xC2, Opcode.Nop },
            { 0xC3, Opcode.Dcp },
            { 0xC4, Opcode.Cpy },
            { 0xC5, Opcode.Cmp },
            { 0xC6, Opcode.Dec },
            { 0xC7, Opcode.Dcp },
            { 0xC8, Opcode.Iny },
            { 0xC9, Opcode.Cmp },
            { 0xCA, Opcode.Dex },
            { 0xCB, Opcode.Axs },
            { 0xCC, Opcode.Cpy },
            { 0xCD, Opcode.Cmp },
            { 0xCE, Opcode.Dec },
            { 0xCF, Opcode.Dcp },
            { 0xD0, Opcode.Bne },
            { 0xD1, Opcode.Cmp },
            { 0xD2, Opcode.Kil },
            { 0xD3, Opcode.Dcp },
            { 0xD4, Opcode.Nop },
            { 0xD5, Opcode.Cmp },
            { 0xD6, Opcode.Dec },
            { 0xD7, Opcode.Dcp },
            { 0xD8, Opcode.Cld },
            { 0xD9, Opcode.Cmp },
            { 0xDA, Opcode.Nop },
            { 0xDB, Opcode.Dcp },
            { 0xDC, Opcode.Nop },
            { 0xDD, Opcode.Cmp },
            { 0xDE, Opcode.Dec },
            { 0xDF, Opcode.Dcp },
            { 0xE0, Opcode.Cpx },
            { 0xE1, Opcode.Sbc },
            { 0xE2, Opcode.Nop },
            { 0xE3, Opcode.Isc },
            { 0xE4, Opcode.Cpx },
            { 0xE5, Opcode.Sbc },
            { 0xE6, Opcode.Inc },
            { 0xE7, Opcode.Isc },
            { 0xE8, Opcode.Inx },
            { 0xE9, Opcode.Sbc },
            { 0xEA, Opcode.Nop },
            { 0xEB, Opcode.Sbc },
            { 0xEC, Opcode.Cpx },
            { 0xED, Opcode.Sbc },
            { 0xEE, Opcode.Inc },
            { 0xEF, Opcode.Isc },
            { 0xF0, Opcode.Beq },
            { 0xF1, Opcode.Sbc },
            { 0xF2, Opcode.Kil },
            { 0xF3, Opcode.Isc },
            { 0xF4, Opcode.Nop },
            { 0xF5, Opcode.Sbc },
            { 0xF6, Opcode.Inc },
            { 0xF7, Opcode.Isc },
            { 0xF8, Opcode.Sed },
            { 0xF9, Opcode.Sbc },
            { 0xFA, Opcode.Nop },
            { 0xFB, Opcode.Isc },
            { 0xFC, Opcode.Nop },
            { 0xFD, Opcode.Sbc },
            { 0xFE, Opcode.Inc },
            { 0xFF, Opcode.Isc }
        };
    }
}