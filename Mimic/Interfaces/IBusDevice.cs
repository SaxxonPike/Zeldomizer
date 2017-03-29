namespace Mimic.Interfaces
{
    public interface IBusDevice
    {
        /// <summary>
        /// Perform a read on the CPU bus.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        int CpuRead(int address);

        /// <summary>
        /// Perform a write on the CPU bus.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        void CpuWrite(int address, int value);

        /// <summary>
        /// Simulate a read on the CPU bus, without triggering side effects.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        int CpuPeek(int address);

        /// <summary>
        /// Simulate a write on the CPU bus, without triggering side effects.
        /// </summary>
        /// <param name="address">Address to write to.</param>
        /// <param name="value">Data to write.</param>
        void CpuPoke(int address, int value);

        /// <summary>
        /// Perform a read on the PPU bus.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        int PpuRead(int address);

        /// <summary>
        /// Simulate a read on the PPU bus, without triggering side effects.
        /// </summary>
        /// <param name="address">Address to read from.</param>
        int PpuPeek(int address);

        /// <summary>
        /// Ready pin output. CPU execution is suspended while this is false.
        /// </summary>
        bool Rdy { get; }

        /// <summary>
        /// Determine if the CPU data bus is asserted when reading from this
        /// device at the specified address.
        /// </summary>
        /// <param name="address">Address to check.</param>
        bool CpuReadChipSelect(int address);

        /// <summary>
        /// Determine if the address is valid when writing to this device on the CPU bus.
        /// </summary>
        /// <param name="address">Address to check.</param>
        bool CpuWriteChipSelect(int address);

        /// <summary>
        /// Determine if the PPU data bus is asserted when reading from this
        /// device at the specified address.
        /// </summary>
        /// <param name="address">Address to check.</param>
        bool PpuReadChipSelect(int address);

        /// <summary>
        /// Determine if the device can control the Ready line.
        /// </summary>
        bool AssertsRdy { get; }

        /// <summary>
        /// Perform a soft reset on the device.
        /// </summary>
        void Reset();

        /// <summary>
        /// Execute one clock on the device.
        /// </summary>
        void Clock();

        /// <summary>
        /// Determine if the device can control the IRQ line.
        /// </summary>
        bool AssertsIrq { get; }

        /// <summary>
        /// Determine if the device can control the NMI line.
        /// </summary>
        bool AssertsNmi { get; }

        /// <summary>
        /// Current output of the device on the IRQ line.
        /// </summary>
        bool Irq { get; }

        /// <summary>
        /// Current output of the device on the NMI line.
        /// </summary>
        bool Nmi { get; }

        /// <summary>
        /// Name of the device.
        /// </summary>
        string Name { get; }
    }
}
