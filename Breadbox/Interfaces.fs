namespace Breadbox

type IMemory =
    abstract member Read: int -> int
    abstract member Write: int * int -> unit
    abstract member Peek: int -> int
    abstract member Poke: int * int -> unit

type IReadySignal =
    abstract member ReadRdy: unit -> bool

type MemoryNull () =
    interface IMemory with
        member this.Read (address) = 0
        member this.Write (address, value) = ()
        member this.Peek (address) = 0
        member this.Poke (address, value) = ()

type ReadySignalNull () =
    interface IReadySignal with
        member this.ReadRdy () = true

type MemoryTrace (memory:IMemory, onRead:System.Action<int>, onWrite:System.Action<int,int>) =
    interface IMemory with
        member this.Read (address) =
            onRead.Invoke(address)
            memory.Read address
        member this.Write (address, value) =
            onWrite.Invoke(address, value)
            memory.Write(address, value)
        member this.Peek (address) =
            memory.Peek address
        member this.Poke (address, value) =
            memory.Poke(address, value)

type MemoryMapSlim (read:int->int, write:int*int->unit) =
    interface IMemory with
        member this.Read (address) = read(address)
        member this.Write (address, value) = write(address, value)
        member this.Peek (address) = read(address)
        member this.Poke (address, value) = write(address, value)

type MemoryMap (read:int->int, write:int*int->unit, peek:int->int, poke:int*int->unit) =
    interface IMemory with
        member this.Read (address) = read(address)
        member this.Write (address, value) = write(address, value)
        member this.Peek (address) = peek(address)
        member this.Poke (address, value) = poke(address, value)
