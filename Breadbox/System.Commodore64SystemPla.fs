namespace Breadbox.System
open Breadbox

// Data source:
// http://www.zimmers.net/anonftp/pub/cbm/firmware/computers/c64/C64_PLA_Dissected.pdf

type Commodore64SystemPlaConfiguration(loRamInput:ILoRamSignal, hiRamInput:IHiRamSignal, charenInput:ICharenSignal, gameInput:IGameSignal, exromInput:IExRomSignal, ram:IMemory, color:IMemory, basic:IMemory, kernal:IMemory, char:IMemory, roml:IMemory, romh:IMemory, io:IMemory, vicBank:IVicBank) =
    member val LoRam = loRamInput
    member val HiRam = hiRamInput
    member val Charen = charenInput
    member val Game = gameInput
    member val ExRom = exromInput
    member val Ram = ram
    member val Kernal = kernal
    member val Basic = basic
    member val Char = char
    member val RomL = roml
    member val RomH = romh
    member val Io = io
    member val Color = color
    member val VicBank = vicBank

type Commodore64SystemPlaVoidMemory (getLastData:unit->int) =
    interface IMemory with
        member this.Read (address) = getLastData()
        member this.Write (address, value) = ()
        member this.Peek (address) = getLastData()
        member this.Poke (address, value) = ()

type Commodore64SystemPla (config:Commodore64SystemPlaConfiguration) =
    let mutable lastAddress = 0xFFFF
    let mutable lastData = 0xFF

    let readLoRam = config.LoRam.ReadLoRam
    let readHiRam = config.HiRam.ReadHiRam
    let readGame = config.Game.ReadGame
    let readExRom = config.ExRom.ReadExRom
    let readCharen = config.Charen.ReadCharen
    let readVicBank = config.VicBank.ReadVicBank

    let ram = config.Ram
    let kernal = config.Kernal
    let basic = config.Basic
    let char = config.Char
    let romL = config.RomL
    let romH = config.RomH
    let io = config.Io
    let color = config.Color
    let none = new Commodore64SystemPlaVoidMemory(fun _ -> lastData) :> IMemory

    let vicReadTarget address =
        match address &&& 0x3000, readGame(), readExRom() with
            | 0x3000, false, true -> romH
            | 0x1000, true, _ -> char
            | 0x1000, _, false -> char
            | _ -> ram

    let mode1111Read address =
        match address >>> 12 with
            | 0xA | 0xB -> basic
            | 0xD -> if readCharen() then io else char
            | 0xE | 0xF -> kernal
            | _ -> ram

    let mode011xRead address =
        match address >>> 12 with
            | 0xD -> if readCharen() then io else char
            | 0xE | 0xF -> kernal
            | _ -> ram

    let mode1000Read address =
        if (address >>> 12 = 0xD && readCharen()) then io else ram

    let mode101xRead address =
        if (address >>> 12 = 0xD && readCharen()) then io else char

    let mode1100Read address =
        match address >>> 12 with
            | 0x8 | 0x9 -> romL
            | 0xA | 0xB -> romH
            | 0xD -> if readCharen() then io else char
            | 0xE | 0xF -> kernal
            | _ -> ram

    let mode0100Read address =
        match address >>> 12 with
            | 0xA | 0xB -> romH
            | 0xD -> if readCharen() then io else char
            | 0xE | 0xF -> kernal
            | _ -> ram

    let mode1110Read address =
        match address >>> 12 with
            | 0x8 | 0x9 -> romL
            | 0xA | 0xB -> basic
            | 0xD -> if readCharen() then io else char
            | 0xE | 0xF -> kernal
            | _ -> ram

    let modexx01Read address =
        match address >>> 12 with
            | 0x0 -> ram
            | 0x8 | 0x9 -> romL
            | 0xD -> io
            | 0xE | 0xF -> romH
            | _ -> none

    let defaultRead _ =
        ram

    let modexx01Write address =
        match address >>> 12 with
            | 0x0 -> ram
            | 0x8 | 0x9 -> romL
            | 0xD -> io
            | 0xE | 0xF -> romH
            | _ -> none

    let mode001xWrite address =
        ram

    let defaultWrite address =
        if (address >>> 12 = 0xD && readCharen()) then io else ram

    let readTarget address =
        address |>
            match readLoRam(), readHiRam(), readGame(), readExRom() with
                | _, _, false, true -> modexx01Read
                | true, false, true, _ -> mode101xRead
                | false, true, true, _ -> mode011xRead
                | true, true, true, true -> mode1111Read
                | true, true, true, false -> mode1110Read
                | true, true, false, false -> mode1100Read
                | true, false, false, false -> mode1000Read
                | false, true, false, false -> mode0100Read
                | _ -> defaultRead
                    
    let writeTarget address =
        address |>
            match readLoRam(), readHiRam(), readGame(), readExRom() with
                | false, false, true, _ -> mode001xWrite
                | false, false, false, false -> mode001xWrite
                | _, _, false, true -> modexx01Write
                | _ -> defaultWrite
    
    let vicRead (address) = vicReadTarget(address).Read(address)
    let vicPeek (address) = vicReadTarget(address).Peek(address)

    let read (address) = readTarget(address).Read(address)
    let write (address, value) = writeTarget(address).Write(address, value)
    let peek (address) = readTarget(address).Peek(address)
    let poke (address, value) = writeTarget(address).Write(address, value)

    member this.VicBus = new MemoryMap(vicRead, ignore, vicPeek, ignore) :> IMemory
    member this.SystemBus = new MemoryMap(read, write, peek, poke) :> IMemory
