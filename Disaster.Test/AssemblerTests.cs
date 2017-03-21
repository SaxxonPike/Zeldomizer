﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Disaster
{
    public class AssemblerTests : BaseTestFixture<Assembler>
    {
        protected override Assembler GetTestSubject()
        {
            return new Assembler();
        }

        [Test]
        [TestCase(0x00, Opcode.Brk, AddressingMode.Implied)]
        [TestCase(0x02, Opcode.Kil, AddressingMode.Implied)]
        [TestCase(0x08, Opcode.Php, AddressingMode.Implied)]
        [TestCase(0x0A, Opcode.Asl, AddressingMode.Accumulator)]
        [TestCase(0x18, Opcode.Clc, AddressingMode.Implied)]
        [TestCase(0x28, Opcode.Plp, AddressingMode.Implied)]
        [TestCase(0x2A, Opcode.Rol, AddressingMode.Accumulator)]
        [TestCase(0x38, Opcode.Sec, AddressingMode.Implied)]
        [TestCase(0x40, Opcode.Rti, AddressingMode.Implied)]
        [TestCase(0x48, Opcode.Pha, AddressingMode.Implied)]
        [TestCase(0x4A, Opcode.Lsr, AddressingMode.Accumulator)]
        [TestCase(0x58, Opcode.Cli, AddressingMode.Implied)]
        [TestCase(0x60, Opcode.Rts, AddressingMode.Implied)]
        [TestCase(0x68, Opcode.Pla, AddressingMode.Implied)]
        [TestCase(0x6A, Opcode.Ror, AddressingMode.Accumulator)]
        [TestCase(0x78, Opcode.Sei, AddressingMode.Implied)]
        [TestCase(0x88, Opcode.Dey, AddressingMode.Implied)]
        [TestCase(0x8A, Opcode.Txa, AddressingMode.Implied)]
        [TestCase(0x98, Opcode.Tya, AddressingMode.Implied)]
        [TestCase(0x9A, Opcode.Txs, AddressingMode.Implied)]
        [TestCase(0xA8, Opcode.Tay, AddressingMode.Implied)]
        [TestCase(0xAA, Opcode.Tax, AddressingMode.Implied)]
        [TestCase(0xB8, Opcode.Clv, AddressingMode.Implied)]
        [TestCase(0xBA, Opcode.Tsx, AddressingMode.Implied)]
        [TestCase(0xC8, Opcode.Iny, AddressingMode.Implied)]
        [TestCase(0xCA, Opcode.Dex, AddressingMode.Implied)]
        [TestCase(0xD8, Opcode.Cld, AddressingMode.Implied)]
        [TestCase(0xE8, Opcode.Inx, AddressingMode.Implied)]
        [TestCase(0xEA, Opcode.Nop, AddressingMode.Implied)]
        [TestCase(0xF8, Opcode.Sed, AddressingMode.Implied)]
        public void Assemble_ProperlyAssemblesOneByteOpcodes(byte expectedOpcode, Opcode opcode, AddressingMode addressingMode)
        {
            var rom = new Mock<IRom>();
            var codeBlock = new CodeBlock { Rom = rom.Object };
            Subject.Assemble(new Instruction { AddressingMode = addressingMode, Opcode = opcode }, codeBlock, 0);

            rom.VerifySet(x => x[0] = expectedOpcode, Times.Once);
            rom.VerifySet(x => x[1] = It.IsAny<byte>(), Times.Never);
            rom.VerifySet(x => x[2] = It.IsAny<byte>(), Times.Never);
        }

        [Test]
        [TestCase(0x01, Opcode.Ora, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x03, Opcode.Slo, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x04, Opcode.Nop, AddressingMode.ZeroPage)]
        [TestCase(0x05, Opcode.Ora, AddressingMode.ZeroPage)]
        [TestCase(0x06, Opcode.Asl, AddressingMode.ZeroPage)]
        [TestCase(0x07, Opcode.Slo, AddressingMode.ZeroPage)]
        [TestCase(0x09, Opcode.Ora, AddressingMode.Immediate)]
        [TestCase(0x0B, Opcode.Anc, AddressingMode.Immediate)]
        [TestCase(0x10, Opcode.Bpl, AddressingMode.Relative)]
        [TestCase(0x11, Opcode.Ora, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x13, Opcode.Slo, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x14, Opcode.Nop, AddressingMode.ZeroPageX)]
        [TestCase(0x15, Opcode.Ora, AddressingMode.ZeroPageX)]
        [TestCase(0x16, Opcode.Asl, AddressingMode.ZeroPageX)]
        [TestCase(0x17, Opcode.Slo, AddressingMode.ZeroPageX)]
        [TestCase(0x21, Opcode.And, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x23, Opcode.Rla, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x24, Opcode.Bit, AddressingMode.ZeroPage)]
        [TestCase(0x25, Opcode.And, AddressingMode.ZeroPage)]
        [TestCase(0x26, Opcode.Rol, AddressingMode.ZeroPage)]
        [TestCase(0x27, Opcode.Rla, AddressingMode.ZeroPage)]
        [TestCase(0x29, Opcode.And, AddressingMode.Immediate)]
        [TestCase(0x30, Opcode.Bmi, AddressingMode.Relative)]
        [TestCase(0x31, Opcode.And, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x33, Opcode.Rla, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x35, Opcode.And, AddressingMode.ZeroPageX)]
        [TestCase(0x36, Opcode.Rol, AddressingMode.ZeroPageX)]
        [TestCase(0x37, Opcode.Rla, AddressingMode.ZeroPageX)]
        [TestCase(0x41, Opcode.Eor, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x43, Opcode.Sre, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x45, Opcode.Eor, AddressingMode.ZeroPage)]
        [TestCase(0x46, Opcode.Lsr, AddressingMode.ZeroPage)]
        [TestCase(0x47, Opcode.Sre, AddressingMode.ZeroPage)]
        [TestCase(0x49, Opcode.Eor, AddressingMode.Immediate)]
        [TestCase(0x4B, Opcode.Alr, AddressingMode.Immediate)]
        [TestCase(0x50, Opcode.Bvc, AddressingMode.Relative)]
        [TestCase(0x51, Opcode.Eor, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x53, Opcode.Sre, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x55, Opcode.Eor, AddressingMode.ZeroPageX)]
        [TestCase(0x56, Opcode.Lsr, AddressingMode.ZeroPageX)]
        [TestCase(0x57, Opcode.Sre, AddressingMode.ZeroPageX)]
        [TestCase(0x61, Opcode.Adc, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x63, Opcode.Rra, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x65, Opcode.Adc, AddressingMode.ZeroPage)]
        [TestCase(0x66, Opcode.Ror, AddressingMode.ZeroPage)]
        [TestCase(0x67, Opcode.Rra, AddressingMode.ZeroPage)]
        [TestCase(0x69, Opcode.Adc, AddressingMode.Immediate)]
        [TestCase(0x6B, Opcode.Arr, AddressingMode.Immediate)]
        [TestCase(0x70, Opcode.Bvs, AddressingMode.Relative)]
        [TestCase(0x71, Opcode.Adc, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x73, Opcode.Rra, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x75, Opcode.Adc, AddressingMode.ZeroPageX)]
        [TestCase(0x76, Opcode.Ror, AddressingMode.ZeroPageX)]
        [TestCase(0x77, Opcode.Rra, AddressingMode.ZeroPageX)]
        [TestCase(0x80, Opcode.Nop, AddressingMode.Immediate)]
        [TestCase(0x81, Opcode.Sta, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x83, Opcode.Sax, AddressingMode.IndirectZeroPageX)]
        [TestCase(0x84, Opcode.Sty, AddressingMode.ZeroPage)]
        [TestCase(0x85, Opcode.Sta, AddressingMode.ZeroPage)]
        [TestCase(0x86, Opcode.Stx, AddressingMode.ZeroPage)]
        [TestCase(0x87, Opcode.Sax, AddressingMode.ZeroPage)]
        [TestCase(0x8B, Opcode.Xaa, AddressingMode.Immediate)]
        [TestCase(0x90, Opcode.Bcc, AddressingMode.Relative)]
        [TestCase(0x91, Opcode.Sta, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x93, Opcode.Ahx, AddressingMode.IndirectZeroPageY)]
        [TestCase(0x94, Opcode.Sty, AddressingMode.ZeroPageX)]
        [TestCase(0x95, Opcode.Sta, AddressingMode.ZeroPageX)]
        [TestCase(0x96, Opcode.Stx, AddressingMode.ZeroPageY)]
        [TestCase(0x97, Opcode.Sax, AddressingMode.ZeroPageY)]
        [TestCase(0xA0, Opcode.Ldy, AddressingMode.Immediate)]
        [TestCase(0xA1, Opcode.Lda, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xA2, Opcode.Ldx, AddressingMode.Immediate)]
        [TestCase(0xA3, Opcode.Lax, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xA4, Opcode.Ldy, AddressingMode.ZeroPage)]
        [TestCase(0xA5, Opcode.Lda, AddressingMode.ZeroPage)]
        [TestCase(0xA6, Opcode.Ldx, AddressingMode.ZeroPage)]
        [TestCase(0xA7, Opcode.Lax, AddressingMode.ZeroPage)]
        [TestCase(0xA9, Opcode.Lda, AddressingMode.Immediate)]
        [TestCase(0xAB, Opcode.Lax, AddressingMode.Immediate)]
        [TestCase(0xB0, Opcode.Bcs, AddressingMode.Relative)]
        [TestCase(0xB1, Opcode.Lda, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xB3, Opcode.Lax, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xB4, Opcode.Ldy, AddressingMode.ZeroPageX)]
        [TestCase(0xB5, Opcode.Lda, AddressingMode.ZeroPageX)]
        [TestCase(0xB6, Opcode.Ldx, AddressingMode.ZeroPageY)]
        [TestCase(0xB7, Opcode.Lax, AddressingMode.ZeroPageY)]
        [TestCase(0xC0, Opcode.Cpy, AddressingMode.Immediate)]
        [TestCase(0xC1, Opcode.Cmp, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xC3, Opcode.Dcp, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xC4, Opcode.Cpy, AddressingMode.ZeroPage)]
        [TestCase(0xC5, Opcode.Cmp, AddressingMode.ZeroPage)]
        [TestCase(0xC6, Opcode.Dec, AddressingMode.ZeroPage)]
        [TestCase(0xC7, Opcode.Dcp, AddressingMode.ZeroPage)]
        [TestCase(0xC9, Opcode.Cmp, AddressingMode.Immediate)]
        [TestCase(0xCB, Opcode.Axs, AddressingMode.Immediate)]
        [TestCase(0xD0, Opcode.Bne, AddressingMode.Relative)]
        [TestCase(0xD1, Opcode.Cmp, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xD3, Opcode.Dcp, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xD5, Opcode.Cmp, AddressingMode.ZeroPageX)]
        [TestCase(0xD6, Opcode.Dec, AddressingMode.ZeroPageX)]
        [TestCase(0xD7, Opcode.Dcp, AddressingMode.ZeroPageX)]
        [TestCase(0xE0, Opcode.Cpx, AddressingMode.Immediate)]
        [TestCase(0xE1, Opcode.Sbc, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xE3, Opcode.Isc, AddressingMode.IndirectZeroPageX)]
        [TestCase(0xE4, Opcode.Cpx, AddressingMode.ZeroPage)]
        [TestCase(0xE5, Opcode.Sbc, AddressingMode.ZeroPage)]
        [TestCase(0xE6, Opcode.Inc, AddressingMode.ZeroPage)]
        [TestCase(0xE7, Opcode.Isc, AddressingMode.ZeroPage)]
        [TestCase(0xE9, Opcode.Sbc, AddressingMode.Immediate)]
        [TestCase(0xF0, Opcode.Beq, AddressingMode.Relative)]
        [TestCase(0xF1, Opcode.Sbc, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xF3, Opcode.Isc, AddressingMode.IndirectZeroPageY)]
        [TestCase(0xF5, Opcode.Sbc, AddressingMode.ZeroPageX)]
        [TestCase(0xF6, Opcode.Inc, AddressingMode.ZeroPageX)]
        [TestCase(0xF7, Opcode.Isc, AddressingMode.ZeroPageX)]
        public void Assemble_ProperlyAssemblesTwoByteOpcodes(byte expectedOpcode, Opcode opcode, AddressingMode addressingMode)
        {
            var rom = new Mock<IRom>();
            var codeBlock = new CodeBlock { Rom = rom.Object };
            var operand = Random<byte>();
            Subject.Assemble(new Instruction { AddressingMode = addressingMode, Opcode = opcode, Operand = operand }, codeBlock, 0);

            rom.VerifySet(x => x[0] = expectedOpcode, Times.Once);
            rom.VerifySet(x => x[1] = operand, Times.Once);
            rom.VerifySet(x => x[2] = It.IsAny<byte>(), Times.Never);
        }

        [Test]
        [TestCase(0x0C, Opcode.Nop, AddressingMode.Absolute)]
        [TestCase(0x0D, Opcode.Ora, AddressingMode.Absolute)]
        [TestCase(0x0E, Opcode.Asl, AddressingMode.Absolute)]
        [TestCase(0x0F, Opcode.Slo, AddressingMode.Absolute)]
        [TestCase(0x19, Opcode.Ora, AddressingMode.AbsoluteY)]
        [TestCase(0x1B, Opcode.Slo, AddressingMode.AbsoluteY)]
        [TestCase(0x1C, Opcode.Nop, AddressingMode.AbsoluteX)]
        [TestCase(0x1D, Opcode.Ora, AddressingMode.AbsoluteX)]
        [TestCase(0x1E, Opcode.Asl, AddressingMode.AbsoluteX)]
        [TestCase(0x1F, Opcode.Slo, AddressingMode.AbsoluteX)]
        [TestCase(0x20, Opcode.Jsr, AddressingMode.Absolute)]
        [TestCase(0x2C, Opcode.Bit, AddressingMode.Absolute)]
        [TestCase(0x2D, Opcode.And, AddressingMode.Absolute)]
        [TestCase(0x2E, Opcode.Rol, AddressingMode.Absolute)]
        [TestCase(0x2F, Opcode.Rla, AddressingMode.Absolute)]
        [TestCase(0x39, Opcode.And, AddressingMode.AbsoluteY)]
        [TestCase(0x3B, Opcode.Rla, AddressingMode.AbsoluteY)]
        [TestCase(0x3D, Opcode.And, AddressingMode.AbsoluteX)]
        [TestCase(0x3E, Opcode.Rol, AddressingMode.AbsoluteX)]
        [TestCase(0x3F, Opcode.Rla, AddressingMode.AbsoluteX)]
        [TestCase(0x4C, Opcode.Jmp, AddressingMode.Absolute)]
        [TestCase(0x4D, Opcode.Eor, AddressingMode.Absolute)]
        [TestCase(0x4E, Opcode.Lsr, AddressingMode.Absolute)]
        [TestCase(0x4F, Opcode.Sre, AddressingMode.Absolute)]
        [TestCase(0x59, Opcode.Eor, AddressingMode.AbsoluteY)]
        [TestCase(0x5B, Opcode.Sre, AddressingMode.AbsoluteY)]
        [TestCase(0x5D, Opcode.Eor, AddressingMode.AbsoluteX)]
        [TestCase(0x5E, Opcode.Lsr, AddressingMode.AbsoluteX)]
        [TestCase(0x5F, Opcode.Sre, AddressingMode.AbsoluteX)]
        [TestCase(0x6C, Opcode.Jmp, AddressingMode.Indirect)]
        [TestCase(0x6D, Opcode.Adc, AddressingMode.Absolute)]
        [TestCase(0x6E, Opcode.Ror, AddressingMode.Absolute)]
        [TestCase(0x6F, Opcode.Rra, AddressingMode.Absolute)]
        [TestCase(0x79, Opcode.Adc, AddressingMode.AbsoluteY)]
        [TestCase(0x7B, Opcode.Rra, AddressingMode.AbsoluteY)]
        [TestCase(0x7D, Opcode.Adc, AddressingMode.AbsoluteX)]
        [TestCase(0x7E, Opcode.Ror, AddressingMode.AbsoluteX)]
        [TestCase(0x7F, Opcode.Rra, AddressingMode.AbsoluteX)]
        [TestCase(0x8C, Opcode.Sty, AddressingMode.Absolute)]
        [TestCase(0x8D, Opcode.Sta, AddressingMode.Absolute)]
        [TestCase(0x8E, Opcode.Stx, AddressingMode.Absolute)]
        [TestCase(0x8F, Opcode.Sax, AddressingMode.Absolute)]
        [TestCase(0x99, Opcode.Sta, AddressingMode.AbsoluteY)]
        [TestCase(0x9B, Opcode.Tas, AddressingMode.AbsoluteY)]
        [TestCase(0x9C, Opcode.Shy, AddressingMode.AbsoluteX)]
        [TestCase(0x9D, Opcode.Sta, AddressingMode.AbsoluteX)]
        [TestCase(0x9E, Opcode.Shx, AddressingMode.AbsoluteY)]
        [TestCase(0x9F, Opcode.Ahx, AddressingMode.AbsoluteY)]
        [TestCase(0xAC, Opcode.Ldy, AddressingMode.Absolute)]
        [TestCase(0xAD, Opcode.Lda, AddressingMode.Absolute)]
        [TestCase(0xAE, Opcode.Ldx, AddressingMode.Absolute)]
        [TestCase(0xAF, Opcode.Lax, AddressingMode.Absolute)]
        [TestCase(0xB9, Opcode.Lda, AddressingMode.AbsoluteY)]
        [TestCase(0xBB, Opcode.Las, AddressingMode.AbsoluteY)]
        [TestCase(0xBC, Opcode.Ldy, AddressingMode.AbsoluteX)]
        [TestCase(0xBD, Opcode.Lda, AddressingMode.AbsoluteX)]
        [TestCase(0xBE, Opcode.Ldx, AddressingMode.AbsoluteY)]
        [TestCase(0xBF, Opcode.Lax, AddressingMode.AbsoluteY)]
        [TestCase(0xCC, Opcode.Cpy, AddressingMode.Absolute)]
        [TestCase(0xCD, Opcode.Cmp, AddressingMode.Absolute)]
        [TestCase(0xCE, Opcode.Dec, AddressingMode.Absolute)]
        [TestCase(0xCF, Opcode.Dcp, AddressingMode.Absolute)]
        [TestCase(0xD9, Opcode.Cmp, AddressingMode.AbsoluteY)]
        [TestCase(0xDB, Opcode.Dcp, AddressingMode.AbsoluteY)]
        [TestCase(0xDD, Opcode.Cmp, AddressingMode.AbsoluteX)]
        [TestCase(0xDE, Opcode.Dec, AddressingMode.AbsoluteX)]
        [TestCase(0xDF, Opcode.Dcp, AddressingMode.AbsoluteX)]
        [TestCase(0xEC, Opcode.Cpx, AddressingMode.Absolute)]
        [TestCase(0xED, Opcode.Sbc, AddressingMode.Absolute)]
        [TestCase(0xEE, Opcode.Inc, AddressingMode.Absolute)]
        [TestCase(0xEF, Opcode.Isc, AddressingMode.Absolute)]
        [TestCase(0xF9, Opcode.Sbc, AddressingMode.AbsoluteY)]
        [TestCase(0xFB, Opcode.Isc, AddressingMode.AbsoluteY)]
        [TestCase(0xFD, Opcode.Sbc, AddressingMode.AbsoluteX)]
        [TestCase(0xFE, Opcode.Inc, AddressingMode.AbsoluteX)]
        [TestCase(0xFF, Opcode.Isc, AddressingMode.AbsoluteX)]
        public void Assemble_ProperlyAssemblesThreeByteOpcodes(byte expectedOpcode, Opcode opcode, AddressingMode addressingMode)
        {
            var rom = new Mock<IRom>();
            var codeBlock = new CodeBlock { Rom = rom.Object };
            var operandLo = Random<byte>();
            var operandHi = Random<byte>();
            Subject.Assemble(new Instruction { AddressingMode = addressingMode, Opcode = opcode, Operand = operandLo | (operandHi << 8)}, codeBlock, 0);

            rom.VerifySet(x => x[0] = expectedOpcode, Times.Once);
            rom.VerifySet(x => x[1] = operandLo, Times.Once);
            rom.VerifySet(x => x[2] = operandHi, Times.Once);
        }

    }
}