using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Breadbox.Test.Cpu6502.Opcode
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class Cpu6502AddressingTests : Cpu6502ExecutionBaseTestFixture
    {
        private enum AccessType
        {
            Read,
            Write
        }

        private class AccessEntry
        {
            public AccessEntry(AccessType accessType, int address)
            {
                AccessType = accessType;
                Address = address;
            }
            public AccessType AccessType { get; private set; }
            public int Address { get; private set; }
        }

        private void Verify(IEnumerable<AccessEntry> accesses)
        {
            Cpu.ForceOpcodeSync();
            MemoryMock.ResetCalls();
            foreach (var access in accesses)
            {
                var address = access.Address;
                Cpu.Clock();
                switch (access.AccessType)
                {
                    case AccessType.Read:
                        if (address >= 0)
                        {
                            Console.WriteLine("Verifying READ at ${0:x4}", address);
                            MemoryMock.Verify(m => m.Read(It.IsIn(address)), Times.Once);
                            MemoryMock.Verify(m => m.Read(It.IsNotIn(address)), Times.Never);
                        }
                        else
                        {
                            Console.WriteLine("Verifying READ at any address");
                            MemoryMock.Verify(m => m.Read(It.IsAny<int>()), Times.Once);
                        }
                        MemoryMock.Verify(m => m.Write(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
                        break;
                    case AccessType.Write:
                        if (address >= 0)
                        {
                            Console.WriteLine("Verifying WRITE at ${0:x4}", address);
                            MemoryMock.Verify(m => m.Write(It.IsIn(address), It.IsAny<int>()), Times.Once);
                            MemoryMock.Verify(m => m.Write(It.IsNotIn(address), It.IsAny<int>()), Times.Never);
                        }
                        else
                        {
                            Console.WriteLine("Verifying WRITE at any address");
                            MemoryMock.Verify(m => m.Write(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
                        }
                        MemoryMock.Verify(m => m.Read(It.IsAny<int>()), Times.Never);
                        break;
                }
                MemoryMock.ResetCalls();
            }
            Cpu.Sync.Should().BeTrue("Sync must occur after opcode finishes.");
        }

        [Test]
        public void Break([Values(0x00)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int interruptVector)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFD);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(0xFFFE)).Returns(interruptVector & 0xFF);
            MemoryMock.Setup(m => m.Read(0xFFFF)).Returns((interruptVector >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Write, 0x01FD),
                new AccessEntry(AccessType.Write, 0x01FC),
                new AccessEntry(AccessType.Write, 0x01FB),
                new AccessEntry(AccessType.Read, 0xFFFE),
                new AccessEntry(AccessType.Read, 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(interruptVector);
        }

        [Test]
        public void Implied([Values(0x0A, 0x18, 0x1A, 0x2A, 0x38, 0x3A, 0x4A, 0x58, 0x5A, 0x6A, 0x78, 0x7A, 0x88, 0x8A, 0x98, 0x9A, 0xA8, 0xAA, 0xB8, 0xBA, 0xC8, 0xCA, 0xD8, 0xDA, 0xE8, 0xEA, 0xF8, 0xFA)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 1);
        }

        [Test]
        public void Immediate([Values(0x09, 0x0B, 0x29, 0x2B, 0x49, 0x4B, 0x69, 0x6B, 0x80, 0x82, 0x89, 0x8B, 0xA0, 0xA2, 0xA9, 0xAB, 0xC0, 0xC2, 0xC9, 0xCB, 0xE0, 0xE2, 0xE9, 0xEB)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpRead([Values(0x04, 0x05, 0x24, 0x25, 0x44, 0x45, 0x64, 0x65, 0xA4, 0xA5, 0xA6, 0xA7, 0xC4, 0xC5, 0xE4, 0xE5)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpWrite([Values(0x84, 0x85, 0x86, 0x87)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Write, zpAddress),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpRmw([Values(0x06, 0x07, 0x26, 0x27, 0x46, 0x47, 0x66, 0x67, 0xC6, 0xC7, 0xE6, 0xE7)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Write, zpAddress),
                new AccessEntry(AccessType.Write, zpAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpxRead([Values(0x15, 0x35, 0x55)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int x)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + x) & 0xFF),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpxWrite([Values(0x94, 0x95)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int x)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Write, (zpAddress + x) & 0xFF),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpxRmw([Values(0x16, 0x36, 0x56)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int x)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + x) & 0xFF),
                new AccessEntry(AccessType.Write, (zpAddress + x) & 0xFF),
                new AccessEntry(AccessType.Write, (zpAddress + x) & 0xFF),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpyRead([Values(0xB6, 0xB7)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int y)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + y) & 0xFF),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void ZpyWrite([Values(0x96, 0x97)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int y)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Write, (zpAddress + y) & 0xFF),
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IndRead([Values(0x6C)] int opcode, [Random(0x0000, 0xFFFD, 1)] int address, [Values(0x0000, 0x07FF, 0xFFFF)] int absAddress, [Random(0x0000, 0xFFFF, 1)] int targetAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            MemoryMock.Setup(m => m.Read(absAddress)).Returns(targetAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((absAddress & 0xFF00) | ((absAddress + 1) & 0xFF))).Returns((targetAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, absAddress),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + 1) & 0xFF))
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(targetAddress);
        }

        [Test]
        public void Push([Values(0x08, 0x48)] int opcode, [Random(0x0000, 0xFFFF, 1)] int address)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFE);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Write, 0x01FE)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 1);
        }

        [Test]
        public void Pull([Values(0x28, 0x68)] int opcode, [Random(0x0000, 0xFFFF, 1)] int address)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFE);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, 0x01FE),
                new AccessEntry(AccessType.Read, 0x01FF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 1);
        }

        [Test]
        public void Rti([Values(0x40)] int opcode, [Random(0x0000, 0xFFFF, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int returnAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFE);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(0x0100)).Returns(returnAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(0x0101)).Returns((returnAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, 0x01FE),
                new AccessEntry(AccessType.Read, 0x01FF),
                new AccessEntry(AccessType.Read, 0x0100),
                new AccessEntry(AccessType.Read, 0x0101)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(returnAddress);
        }

        [Test]
        public void Rts([Values(0x60)] int opcode, [Random(0x0000, 0xFFFF, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int returnAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFE);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(0x01FF)).Returns(returnAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(0x0100)).Returns((returnAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, 0x01FE),
                new AccessEntry(AccessType.Read, 0x01FF),
                new AccessEntry(AccessType.Read, 0x0100),
                new AccessEntry(AccessType.Read, returnAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(returnAddress + 1);
        }

        [Test]
        public void Jsr([Values(0x20)] int opcode, [Random(0x0000, 0xFFFF, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            Cpu.SetS(0xFE);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, 0x01FE),
                new AccessEntry(AccessType.Write, 0x01FE),
                new AccessEntry(AccessType.Write, 0x01FD),
                new AccessEntry(AccessType.Read, address + 2)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(absAddress);
        }

        [Test]
        public void AbsRead([Values(0x0D, 0x2D, 0x4D, 0x6D, 0xAD, 0xCD, 0xED)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbsWrite([Values(0x8C, 0x8D, 0x8E, 0x8F)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Write, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbsRmw([Values(0x0E, 0x0F, 0x2E, 0x2F, 0x4E, 0x4F, 0x6E, 0x6F, 0xCE, 0xCF, 0xEE, 0xEF)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress)
        {
            // Arrange
            Cpu.SetPC(address);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new[]
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, absAddress),
                new AccessEntry(AccessType.Write, absAddress),
                new AccessEntry(AccessType.Write, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbxRead([Values(0x1C, 0x1D, 0x3C, 0x3D, 0x5C, 0x5D, 0x7C, 0x7D, 0xBC, 0xBD, 0xDC, 0xDD, 0xFC, 0xFD)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + x) & 0xFF))
            };
            if ((absAddress & 0xFF) + x >= 0x100)
            {
                accesses.Add(new AccessEntry(AccessType.Read, (absAddress + x) & 0xFFFF));
            }

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbxWrite([Values(0x9D)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + x) & 0xFF)),
                new AccessEntry(AccessType.Write, (absAddress + x) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void ShyAbx([Values(0x9C)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + x) & 0xFF)),
                new AccessEntry(AccessType.Write, -1)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbxRmw([Values(0x1E, 0x1F, 0x3E, 0x3F, 0x5E, 0x5F, 0x7E, 0x7F, 0xDE, 0xDF, 0xFE, 0xFF)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + x) & 0xFF)),
                new AccessEntry(AccessType.Read, (absAddress + x) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + x) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + x) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbyRead([Values(0x19, 0x39, 0x59, 0x79, 0xB9, 0xBB, 0xBE, 0xBF, 0xD9, 0xF9)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF))
            };
            if ((absAddress & 0xFF) + y >= 0x100)
            {
                accesses.Add(new AccessEntry(AccessType.Read, (absAddress + y) & 0xFFFF));
            }

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbyWrite([Values(0x99, 0x9B)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbyWriteIllegal([Values(0x9E, 0x9F)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Write, -1)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void AbyRmw([Values(0x1B, 0x3B, 0x5B, 0x7B, 0xDB, 0xFB)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read(address + 2)).Returns((absAddress >> 8) & 0xFF);
            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, address + 2),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Read, (absAddress + y) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 3);
        }

        [Test]
        public void IzxRead([Values(0x01, 0x21, 0x41, 0x61, 0xA1, 0xA3, 0xC1, 0xE1)] int opcode, [Random(0x0100, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read((zpAddress + x) & 0xFF)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + x + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + x) & 0xFF),
                new AccessEntry(AccessType.Read, (zpAddress + x + 1) & 0xFF),
                new AccessEntry(AccessType.Read, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IzxWrite([Values(0x81, 0x83)] int opcode, [Random(0x0100, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x0100, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read((zpAddress + x) & 0xFF)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + x + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + x) & 0xFF),
                new AccessEntry(AccessType.Read, (zpAddress + x + 1) & 0xFF),
                new AccessEntry(AccessType.Write, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IzxRmw([Values(0x03, 0x23, 0x43, 0x63, 0xC3, 0xE3)] int opcode, [Random(0x0100, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x0000, 0xFFFF, 1)] int absAddress, [Values(0x00, 0xFF)] int x)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetX(x);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read((zpAddress + x) & 0xFF)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + x + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + x) & 0xFF),
                new AccessEntry(AccessType.Read, (zpAddress + x + 1) & 0xFF),
                new AccessEntry(AccessType.Read, absAddress),
                new AccessEntry(AccessType.Write, absAddress),
                new AccessEntry(AccessType.Write, absAddress)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IzyRead([Values(0x11, 0x31, 0x51, 0x71, 0xB1, 0xB3, 0xD1, 0xF1)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read(zpAddress)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + 1) & 0xFF),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF))
            };
            if ((absAddress & 0xFF) + y >= 0x100)
            {
                accesses.Add(new AccessEntry(AccessType.Read, (absAddress + y) & 0xFFFF));
            }

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IzyWrite([Values(0x91)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read(zpAddress)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + 1) & 0xFF),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void AhxIzy([Values(0x93)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read(zpAddress)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + 1) & 0xFF),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Write, -1)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }

        [Test]
        public void IzyRmw([Values(0x13, 0x33, 0x53, 0x73, 0xD3, 0xF3)] int opcode, [Random(0x0000, 0xFFFE, 1)] int address, [Random(0x00, 0xFF, 1)] int zpAddress, [Random(0x00, 0xFF, 1)] int absAddress, [Values(0x00, 0xFF)] int y)
        {
            // Arrange
            address |= 0x80;
            Cpu.SetPC(address);
            Cpu.SetY(y);
            MemoryMock.Setup(m => m.Read(address)).Returns(opcode);
            MemoryMock.Setup(m => m.Read(address + 1)).Returns(zpAddress);
            MemoryMock.Setup(m => m.Read(zpAddress)).Returns(absAddress & 0xFF);
            MemoryMock.Setup(m => m.Read((zpAddress + 1) & 0xFF)).Returns((absAddress >> 8) & 0xFF);

            var accesses = new List<AccessEntry>
            {
                new AccessEntry(AccessType.Read, address),
                new AccessEntry(AccessType.Read, address + 1),
                new AccessEntry(AccessType.Read, zpAddress),
                new AccessEntry(AccessType.Read, (zpAddress + 1) & 0xFF),
                new AccessEntry(AccessType.Read, (absAddress & 0xFF00) | ((absAddress + y) & 0xFF)),
                new AccessEntry(AccessType.Read, (absAddress + y) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF),
                new AccessEntry(AccessType.Write, (absAddress + y) & 0xFFFF)
            };

            // Assert
            Verify(accesses);
            Cpu.PC.Should().Be(address + 2);
        }
    }
}
