namespace Breadbox.System
open Breadbox

type Commodore64SystemCpuPort (cpuPort:IPort) =
    interface ILoRamSignal with
        member this.ReadLoRam () =
            (cpuPort.ReadPort >> (&&&) 0x01 >> (<>) 0)()
    interface IHiRamSignal with
        member this.ReadHiRam () =
            (cpuPort.ReadPort >> (&&&) 0x02 >> (<>) 0)()
    interface ICharenSignal with
        member this.ReadCharen () =
            (cpuPort.ReadPort >> (&&&) 0x04 >> (<>) 0)()
            
