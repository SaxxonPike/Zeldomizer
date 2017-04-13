namespace Breadbox

// 6502/6510 core.
// Ported from Bizhawk's C# core.

[<Sealed>]
type Mos6502Configuration(lxaConstant:int, hasDecimalMode:bool, read:System.Func<int, int>, write:System.Action<int, int>, ready:System.Func<bool>, irq:System.Func<bool>, nmi:System.Func<bool>) =
    member val LxaConstant = lxaConstant
    member val HasDecimalMode = hasDecimalMode
    member val Ready = ready.Invoke
    member val Irq = irq.Invoke
    member val Nmi = nmi.Invoke
    member val Read = read.Invoke
    member val Write = write.Invoke

[<Sealed>]
type Mos6502(config:Mos6502Configuration) =
    [<Literal>]
    let VopFetch1 = 0x100
    [<Literal>]
    let VopRelativeStuff = 0x101
    [<Literal>]
    let VopRelativeStuff2 = 0x102
    [<Literal>]
    let VopRelativeStuff3 = 0x103
    [<Literal>]
    let VopNmi = 0x104
    [<Literal>]
    let VopIrq = 0x105
    [<Literal>]
    let VopReset = 0x106
    [<Literal>]
    let VopFetch1NoInterrupt = 0x107


    [<Literal>]
    let NmiVector = 0xFFFA
    [<Literal>]
    let ResetVector = 0xFFFC
    [<Literal>]
    let IrqVector = 0xFFFE
    [<Literal>]
    let BrkVector = 0xFFFE

    let mutable restart = false
    let mutable opcode = VopReset
    let mutable opcode2 = 0
    let mutable opcode3 = 0
    let mutable ea = 0
    let mutable aluTemp = 0
    let mutable mi = 0
    let mutable myIFlag = false
    let mutable iFlagPending = true
    let mutable interruptPending = false
    let mutable branchIrqHack = false
    let mutable irq = false
    let mutable nmi = false
    let mutable rdy = false
    let mutable lastNmi = false

    let mutable pc = 0
    let mutable a = 0
    let mutable x = 0
    let mutable y = 0
    let mutable s = 0

    let mutable isDecimalMode = false

    let mutable n = 0x80
    let mutable v = 0x00
    let mutable b = 0x00
    let mutable d = false
    let mutable i = true
    let mutable z = false
    let mutable c = 0x00

    let mutable totalCycles = 0UL

    let lxaConstant = config.LxaConstant
    let hasDecimalMode = config.HasDecimalMode
    let readRdy = config.Ready
    let readIrq = config.Irq
    let readNmi = config.Nmi
    let memoryReadRaw = config.Read
    let memoryWriteRaw = config.Write

    let read = memoryReadRaw

    let write = memoryWriteRaw

    let SoftResetInternal () =
        i <- true
        iFlagPending <- true
        mi <- 0
        opcode <- VopReset

    let HardResetInternal () =
        a <- 0x00
        x <- 0x00
        y <- 0x00
        n <- 0x00
        v <- 0x00
        b <- 0x00
        d <- false
        z <- false
        c <- 0x00
        SoftResetInternal()

    let GetP () =
        0x20 |||
        n |||
        v |||
        b |||
        (if d then 0x08 else 0x00) |||
        (if i then 0x04 else 0x00) |||
        (if z then 0x02 else 0x00) |||
        c

    let SetP value =
        n <- (value &&& 0x80)
        v <- (value &&& 0x40)
        b <- (value &&& 0x10)
        d <- (value &&& 0x08) <> 0
        i <- (value &&& 0x04) <> 0
        z <- (value &&& 0x02) <> 0
        c <- (value &&& 0x01)

    let NZ value =
        z <- (value &&& 0xFF) = 0
        n <- (value &&& 0x80)

    let ReadMemoryInternal = read

    let ReadMemory address operation =
        if rdy then
            ReadMemoryInternal address |> operation
            true
        else
            false

    let WriteMemory address value operation =
        write(address, value)
        value |> operation
        true

    let ReadMemoryPcIncrement operation =
        ReadMemory pc <| (fun mem ->
            pc <- pc + 1
            mem |> operation)

    let ReadMemoryS operation =
        ReadMemory (0x100 ||| s) <| operation

    let WriteMemoryS value operation =
        WriteMemory (0x100 ||| s) value <| operation

    let Push value operation =
        WriteMemoryS value <| (fun mem ->
            mem |> operation
            s <- (s - 1 &&& 0xFF))

    let PushDiscard value = Push value <| ignore

    let Pull operation =
        ReadMemoryS <| (fun mem ->
            mem |> operation
            s <- (s + 1 &&& 0xFF))

    let GetPcl () = pc &&& 0xFF
    let GetPch () = pc >>> 8
    let SetPcl value = pc <- (pc &&& 0xFF00) ||| value
    let SetNZA value =
        a <- value
        NZ a
    let SetNZX value =
        x <- value
        NZ x
    let SetNZY value =
        y <- value
        NZ y
    let SetAInternal value =
        a <- value
    let SetAlu value =
        aluTemp <- value
    let SetOpcode2 value =
        opcode2 <- value
    let SetOpcode3 value =
        opcode3 <- value
    let SetLowEa value =
        ea <- value
    let SetHighEa value =
        ea <- ea ||| (value <<< 8)
    let SetPcJump value =
        pc <- opcode2 ||| (value <<< 8)

    let IfReady action =
        if rdy then
            action()
            true
        else
            false


    // ----- ALU -----


    let Cmp register value =
        let result = (register - value)
        aluTemp <- result &&& 0xFF
        c <- ((result >>> 8) &&& 1) ^^^ 1
        NZ result

    let CmpA value = Cmp a value
    let CmpX value = Cmp x value
    let CmpY value = Cmp y value

    let AluANz result =
        aluTemp <- result
        a <- result
        NZ result

    let And value =
        AluANz (a &&& value)

    let Bit value =
        aluTemp <- value
        n <- (value &&& 0x80)
        v <- (value &&& 0x40)
        z <- (a &&& value) = 0

    let Eor value =
        AluANz (a ^^^ value)

    let Ora value =
        AluANz (a ||| value)

    let Anc value =
        let result = (a &&& value)
        AluANz result
        c <- (result &&& 0x80) >>> 7

    let Asr value =
        let andResult = (a &&& value)
        let result = (andResult >>> 1)
        c <- (andResult &&& 0x01)
        AluANz result

    let Axs value =
        Cmp (x &&& a) value
        x <- aluTemp

    let Arr value =
        aluTemp <-
            let initialMask = a &&& value
            let binaryResult = (initialMask >>> 1) ||| (c <<< 7)
            if not isDecimalMode then
                NZ binaryResult
                c <- (binaryResult &&& 0x40) >>> 6
                v <- (binaryResult &&& 0x40) ^^^ ((binaryResult &&& 0x20) <<< 1)
                binaryResult
            else
                n <- (a &&& 0x80)
                z <- binaryResult = 0
                v <- (binaryResult ^^^ initialMask) &&& 0x40
                let lowResult =
                    match ((initialMask &&& 0xf) + (initialMask &&& 0x1)) with
                        | x when x > 0x5 -> (binaryResult &&& 0xf0) ||| ((binaryResult + 0x6) &&& 0xf)
                        | _ -> binaryResult
                if ((initialMask &&& 0xf0) + (initialMask &&& 0x10)) > 0x50 then
                    c <- 1
                    (lowResult &&& 0x0f) ||| ((lowResult + 0x60) &&& 0xf0)
                else
                    c <- 0
                    lowResult
        a <- aluTemp

    let Lxa value =
        let result = ((a ||| lxaConstant) &&& value)
        aluTemp <- result
        a <- result
        x <- result
        NZ result

    let Sbc value =
        let inline setV sum operand =
            v <- ((a ^^^ sum) &&& (a ^^^ operand) &&& 0x80) >>> 1
        let binaryResult = a - value - (c ^^^ 1)
        let alu =
            if not isDecimalMode then
                setV binaryResult value
                binaryResult &&& 0xFF
            else
                let initialSub = (a &&& 0x0F) - (value &&& 0x0F) - (c ^^^ 1)
                let adjustedSub =
                    if (initialSub &&& 0x10) = 0 then
                        (initialSub &&& 0x0F) ||| ((a &&& 0xF0) - (value &&& 0xF0))
                    else
                        ((initialSub - 6) &&& 0x0F) ||| ((a &&& 0xF0) - (value &&& 0xF0) - 0x10)
                let result = (if (adjustedSub &&& 0x100) <> 0 then (adjustedSub - 0x060) else adjustedSub)
                setV binaryResult value
                result &&& 0xFF
        z <- (binaryResult &&& 0xFF) = 0
        n <- (binaryResult &&& 0x80)
        c <- ((binaryResult &&& 0x100) >>> 8) ^^^ 1
        aluTemp <- alu
        a <- alu

    let Adc value =
        let inline setV sum operand =
            v <- (((a ^^^ sum) &&& (a ^^^ operand) &&& 0x80) >>> 1) ^^^ 0x40
        let binaryResult = value + a + c
        let alu =
            if not isDecimalMode then
                NZ binaryResult
                c <- (binaryResult &&& 0x100) >>> 8
                setV binaryResult value
                binaryResult &&& 0xFF
            else
                let initialAdd = (a &&& 0x0F) + (value &&& 0x0F) + c
                let adjustedAdd = initialAdd + (if initialAdd > 9 then 6 else 0)
                let result = (adjustedAdd &&& 0x0F) + (a &&& 0xF0) + (value &&& 0xF0) + (if adjustedAdd > 0x0F then 0x10 else 0x00)
                z <- (binaryResult &&& 0xFF) = 0
                n <- (result &&& 0x80)
                setV result value
                let adjustedResult = result + (if result &&& 0x1F0 > 0x090 then 0x060 else 0x000)
                c <- ((adjustedResult &&& 0x100) >>> 8) ||| ((adjustedResult &&& 0x200) >>> 9)
                adjustedResult &&& 0xFF
        aluTemp <- alu
        a <- alu

    let Slo value =
        let result = ((value <<< 1) &&& 0xFF) ||| a
        c <- (value &&& 0x80) >>> 7
        AluANz result

    let Isc value =
        Sbc ((value + 1) &&& 0xFF)

    let Dcp value =
        c <- (value &&& 0x01)
        CmpA ((value - 1) &&& 0xFF)

    let Sre value =
        let result = ((value >>> 1) &&& 0xFF) ^^^ a
        c <- (value &&& 0x01)
        AluANz result

    let Rra value =
        c <- (value &&& 0x01)
        Adc ((value >>> 1) ||| (c <<< 7))

    let Rla value =
        let result = (((value <<< 1) &&& 0xFF) ||| c) &&& a
        c <- (value &&& 0x80) >>> 7
        AluANz result

    let Lsr value =
        let result = value >>> 1
        c <- (value &&& 0x01)
        aluTemp <- result
        NZ result

    let LsrA () =
        Lsr a
        a <- aluTemp

    let Asl value =
        let result = ((value <<< 1) &&& 0xFF)
        c <- (value &&& 0x80) >>> 7
        aluTemp <- result
        NZ result

    let AslA () =
        Asl a
        a <- aluTemp

    let Inc value =
        let result = (value + 1) &&& 0xFF
        aluTemp <- result
        NZ result

    let Dec value =
        let result = (value - 1) &&& 0xFF
        aluTemp <- result
        NZ result

    let Lda value =
        a <- value
        NZ value

    let Ldx value =
        x <- value
        NZ value

    let Ldy value =
        y <- value
        NZ value

    let Lax value =
        x <- value
        a <- value
        NZ value

    let Rol value =
        let result = ((value <<< 1) &&& 0xFF) ||| c
        c <- (value &&& 0x80) >>> 7
        aluTemp <- result
        NZ result

    let RolA () =
        Rol a
        a <- aluTemp

    let Ror value =
        let result = (value >>> 1) ||| (c <<< 7)
        c <- (value &&& 0x01)
        aluTemp <- result
        NZ result

    let RorA () =
        Ror a
        a <- aluTemp

    let Las value =
        let result = value &&& s
        s <- result
        x <- result
        a <- result
        NZ result


    // ----- uOPS -----


    let FetchDiscard address operation = ReadMemory address <| (ignore >> operation)
    let FetchDummy operation = FetchDiscard pc operation

    let Fetch1RealInternal () =
        let currentPc = pc
        opcode <- ReadMemoryInternal currentPc
        branchIrqHack <- false
        mi <- -1
        pc <- (currentPc + 1) &&& 0xFFFF

    let Fetch1Real () =
        IfReady Fetch1RealInternal

    let Fetch1Internal () =
        myIFlag <- i
        i <- iFlagPending
        match branchIrqHack, nmi, (irq && (not myIFlag)) with
            | false, true, _ ->
                interruptPending <- false
                ea <- NmiVector
                opcode <- VopNmi
                nmi <- false
                mi <- 0
                restart <- true
            | false, false, true ->
                interruptPending <- false
                ea <- IrqVector
                opcode <- VopIrq
                mi <- 0
                restart <- true
            | _ -> Fetch1RealInternal()

    let Fetch1 () =
        IfReady Fetch1Internal

    let Fetch2 () = ReadMemoryPcIncrement <| SetOpcode2
    let Fetch3 () = ReadMemoryPcIncrement <| SetOpcode3
    let PushPch = GetPch >> PushDiscard
    let PushPcl = GetPcl >> PushDiscard

    let DecrementS () =
        s <- (s - 1) &&& 0xFF

    let PushDummy () =
        ReadMemoryS <| (ignore >> DecrementS)

    let PushPInterrupt newB newEa =
        b <- newB
        Push (GetP()) <| fun _ ->
            i <- true
            ea <- newEa

    let PushPBrk () =
        PushPInterrupt 0x10 BrkVector

    let PushPIrq () =
        PushPInterrupt 0x00 IrqVector
            
    let PushPNmi () =
        PushPInterrupt 0x00 NmiVector

    let PushPReset () =
        if PushDummy() then
            b <- 0x00
            i <- true
            ea <- ResetVector
            true
        else
            false

    let FetchPclVectorInternal () =
        if nmi && ((ea = BrkVector && b <> 0) || (ea = IrqVector && (b = 0))) then
            nmi <- false
            ea <- NmiVector
        aluTemp <- ReadMemoryInternal ea
        
    let FetchPclVector () =
        IfReady FetchPclVectorInternal

    let FetchPchVectorInternal () =
        aluTemp <- aluTemp ||| (ReadMemoryInternal(ea + 1) <<< 8)
        pc <- aluTemp
            
    let FetchPchVector () =
        IfReady FetchPchVectorInternal

    let Imp operation = FetchDummy operation
    let ImpIny () = Imp <| fun _ -> (Inc y; y <- aluTemp)
    let ImpDey () = Imp <| fun _ -> (Dec y; y <- aluTemp)
    let ImpInx () = Imp <| fun _ -> (Inc x; x <- aluTemp)
    let ImpDex () = Imp <| fun _ -> (Dec x; x <- aluTemp)
    let ImpTsx () = Imp <| fun _ -> SetNZX s
    let ImpTxs () = Imp <| fun _ -> s <- x
    let ImpTax () = Imp <| fun _ -> SetNZX a
    let ImpTay () = Imp <| fun _ -> SetNZY a
    let ImpTya () = Imp <| fun _ -> SetNZA y
    let ImpTxa () = Imp <| fun _ -> SetNZA x
    let ImpSei () = Imp <| fun _ -> iFlagPending <- true
    let ImpCli () = Imp <| fun _ -> iFlagPending <- false
    let ImpSec () = Imp <| fun _ -> c <- 1
    let ImpClc () = Imp <| fun _ -> c <- 0
    let ImpClv () = Imp <| fun _ -> v <- 0
    let ImpSed () = Imp <| fun _ -> (d <- true; isDecimalMode <- hasDecimalMode)
    let ImpCld () = Imp <| fun _ -> (d <- false; isDecimalMode <- false)

    let AbsWrite value = WriteMemory ((opcode3 <<< 8) ||| opcode2) value <| ignore
    let AbsWriteSta () = AbsWrite <| a
    let AbsWriteStx () = AbsWrite <| x
    let AbsWriteSty () = AbsWrite <| y
    let AbsWriteSax () = AbsWrite <| (x &&& a)

    let ZpWrite value = WriteMemory opcode2 value <| ignore
    let ZpWriteSta () = ZpWrite <| a
    let ZpWriteStx () = ZpWrite <| x
    let ZpWriteSty () = ZpWrite <| y
    let ZpWriteSax () = ZpWrite <| (x &&& a)

    let IndIdxStage3 () = ReadMemory opcode2 <| SetLowEa
    let IndIdxStage4 () =
        IfReady <| fun _ ->
            let result = (ea + y)
            aluTemp <- result
            ea <- ((ReadMemoryInternal <| (opcode2 + 1) &&& 0xFF) <<< 8) ||| (result &&& 0xFF)

    let IndIdxWriteStage5 () =
        IfReady <| fun _ ->
            ReadMemoryInternal ea |> ignore
            ea <- ea + (aluTemp &&& 0xFF00)

    let IndIdxReadStage5 () =
        IfReady <| fun _ ->
            restart <-
                match aluTemp, ea with
                    | alu,_ when alu < 0x100 ->
                        true
                    | _,address ->
                        ReadMemoryInternal address |> ignore
                        ea <- (address + 0x100) &&& 0xFFFF
                        false

    let IndIdxRmwStage5 () =
        IfReady <| fun _ ->
            ReadMemoryInternal ea |> ignore
            ea <- (ea + (aluTemp &&& 0x100)) &&& 0xFFFF

    let IndIdxWriteStage6 value = WriteMemory ea value <| ignore
    let IndIdxWriteStage6Sta () = IndIdxWriteStage6 <| a
    let IndIdxWriteStage6Sha () = IndIdxWriteStage6 <| (a &&& x &&& 7)

    let IndIdxReadStage6 operation = ReadMemory ea <| operation
    let IndIdxReadStage6Lda () = IndIdxReadStage6 <| Lda
    let IndIdxReadStage6Cmp () = IndIdxReadStage6 <| CmpA
    let IndIdxReadStage6And () = IndIdxReadStage6 <| And
    let IndIdxReadStage6Eor () = IndIdxReadStage6 <| Eor
    let IndIdxReadStage6Lax () = IndIdxReadStage6 <| Lax
    let IndIdxReadStage6Adc () = IndIdxReadStage6 <| Adc
    let IndIdxReadStage6Sbc () = IndIdxReadStage6 <| Sbc
    let IndIdxReadStage6Ora () = IndIdxReadStage6 <| Ora

    let IndIdxRmwStage6 () = ReadMemory ea <| SetAlu

    let IndIdxRmwStage7 operation = WriteMemory ea aluTemp <| operation
    let IndIdxRmwStage7Slo () = IndIdxRmwStage7 <| Slo
    let IndIdxRmwStage7Sre () = IndIdxRmwStage7 <| Sre
    let IndIdxRmwStage7Rra () = IndIdxRmwStage7 <| Rra
    let IndIdxRmwStage7Isc () = IndIdxRmwStage7 <| Isc
    let IndIdxRmwStage7Dcp () = IndIdxRmwStage7 <| Dcp
    let IndIdxRmwStage7Rla () = IndIdxRmwStage7 <| Rla

    let IndIdxRmwStage8 () = WriteMemory ea aluTemp <| ignore

    let RelBranchStage2 branchTaken =
        ReadMemoryPcIncrement (fun mem ->
            opcode2 <- mem
            if branchTaken then
                opcode <- VopRelativeStuff
                mi <- -1)
            
    let RelBranchStage2Bvs () = RelBranchStage2 (v <> 0)
    let RelBranchStage2Bvc () = RelBranchStage2 (v = 0)
    let RelBranchStage2Bmi () = RelBranchStage2 (n <> 0)
    let RelBranchStage2Bpl () = RelBranchStage2 (n = 0)
    let RelBranchStage2Bcs () = RelBranchStage2 (c <> 0)
    let RelBranchStage2Bcc () = RelBranchStage2 (c = 0)
    let RelBranchStage2Beq () = RelBranchStage2 z
    let RelBranchStage2Bne () = RelBranchStage2 (not z)

    let RelBranchStage3 () =
        FetchDummy <| fun _ ->
            let address = (pc &&& 0xFF) + (if opcode2 < 0x80 then opcode2 else opcode2 - 256)
            aluTemp <- address
            pc <- (pc &&& 0xFF00) ||| (aluTemp &&& 0xFF)
            if (address &&& 0xFF00) >= 0x100 then
                opcode <- VopRelativeStuff2
                mi <- -1
            else
                if interruptPending then
                    branchIrqHack <- true

    let RelBranchStage4 () =
        FetchDummy <| fun _ ->
            pc <- (pc + (if aluTemp < 0 then -256 else 256)) &&& 0xFFFF

    let IncrementS () = 
        s <- (s + 1) &&& 0xFF
    let IncS () = ReadMemoryS <| (ignore >> IncrementS)
    let Jsr () = ReadMemory pc <| SetPcJump
    let PullP () = Pull <| SetP
    let PullPcl () = Pull <| SetPcl
    let PullPchNoInc () =
        IfReady <| fun _ -> pc <- (pc &&& 0x00FF) ||| (ReadMemoryInternal(0x0100 ||| s) <<< 8)

    let AbsRead operation = ReadMemory ((opcode3 <<< 8) ||| opcode2) <| operation
    let AbsReadLda () = AbsRead <| SetNZA
    let AbsReadLdy () = AbsRead <| SetNZY
    let AbsReadLdx () = AbsRead <| SetNZX
    let AbsReadBit () = AbsRead <| Bit
    let AbsReadLax () = AbsRead <| Lax
    let AbsReadAnd () = AbsRead <| And
    let AbsReadEor () = AbsRead <| Eor
    let AbsReadOra () = AbsRead <| Ora
    let AbsReadAdc () = AbsRead <| Adc
    let AbsReadCmp () = AbsRead <| CmpA
    let AbsReadCpy () = AbsRead <| CmpY
    let AbsReadNop () = AbsRead <| ignore
    let AbsReadCpx () = AbsRead <| CmpX
    let AbsReadSbc () = AbsRead <| Sbc

    let ZpIdxStage3 value =
        ReadMemory opcode2 <|
            fun _ ->
                opcode2 <- (opcode2 + value) &&& 0xFF
    let ZpIdxStage3X () = ZpIdxStage3 <| x
    let ZpIdxStage3Y () = ZpIdxStage3 <| y

    let ZpIdxRmwStage4 () = ReadMemory opcode2 <| SetAlu
    let ZpIdxRmwStage6 () = WriteMemory opcode2 aluTemp <| ignore

    let ZpRead operation = ReadMemory opcode2 <| operation
    let ZpReadEor () = ZpRead <| Eor
    let ZpReadBit () = ZpRead <| Bit
    let ZpReadLda () = ZpRead <| Lda
    let ZpReadLdy () = ZpRead <| Ldy
    let ZpReadLdx () = ZpRead <| Ldx
    let ZpReadLax () = ZpRead <| Lax
    let ZpReadCpy () = ZpRead <| CmpY
    let ZpReadCmp () = ZpRead <| CmpA
    let ZpReadCpx () = ZpRead <| CmpX
    let ZpReadOra () = ZpRead <| Ora
    let ZpReadNop () = ZpRead <| ignore
    let ZpReadSbc () = ZpRead <| Sbc
    let ZpReadAdc () = ZpRead <| Adc
    let ZpReadAnd () = ZpRead <| And

    let Imm operation = ReadMemoryPcIncrement <| operation
    let ImmEor () = Imm <| Eor
    let ImmAnc () = Imm <| Anc
    let ImmAsr () = Imm <| Asr
    let ImmAxs () = Imm <| Axs
    let ImmArr () = Imm <| Arr
    let ImmLxa () = Imm <| Lxa
    let ImmOra () = Imm <| Ora
    let ImmCpy () = Imm <| CmpY
    let ImmCpx () = Imm <| CmpX
    let ImmCmp () = Imm <| CmpA
    let ImmSbc () = Imm <| Sbc
    let ImmAnd () = Imm <| And
    let ImmAdc () = Imm <| Adc
    let ImmLda () = Imm <| Lda
    let ImmLdx () = Imm <| Ldx
    let ImmLdy () = Imm <| Ldy
    let ImmUnsupported () = Imm <| ignore

    let IdxIndStage3 () =
        ReadMemory opcode2 <| fun mem ->
            aluTemp <- (opcode2 + x) &&& 0xFF
    let IdxIndStage4 () = ReadMemory aluTemp <| SetLowEa
    let IdxIndStage5 () = ReadMemory (aluTemp + 1) <| SetHighEa

    let IdxIndReadStage6 operation = ReadMemory ea <| operation
    let IdxIndReadStage6Lda () = IdxIndReadStage6 <| Lda
    let IdxIndReadStage6Ora () = IdxIndReadStage6 <| Ora
    let IdxIndReadStage6Lax () = IdxIndReadStage6 <| Lax
    let IdxIndReadStage6Cmp () = IdxIndReadStage6 <| CmpA
    let IdxIndReadStage6Adc () = IdxIndReadStage6 <| Adc
    let IdxIndReadStage6And () = IdxIndReadStage6 <| And
    let IdxIndReadStage6Eor () = IdxIndReadStage6 <| Eor
    let IdxIndReadStage6Sbc () = IdxIndReadStage6 <| Sbc

    let IdxIndWriteStage6 value = WriteMemory ea value <| ignore
    let IdxIndWriteStage6Sta () = IdxIndWriteStage6 <| a
    let IdxIndWriteStage6Sax () = IdxIndWriteStage6 <| (a &&& x)

    let IdxIndRmwStage6 () = ReadMemory ea <| SetAlu

    let IdxIndRmwStage7 operation = WriteMemory ea aluTemp <| operation
    let IdxIndRmwStage7Slo () = IdxIndRmwStage7 <| Slo
    let IdxIndRmwStage7Sre () = IdxIndRmwStage7 <| Sre
    let IdxIndRmwStage7Rra () = IdxIndRmwStage7 <| Rra
    let IdxIndRmwStage7Isc () = IdxIndRmwStage7 <| Isc
    let IdxIndRmwStage7Dcp () = IdxIndRmwStage7 <| Dcp
    let IdxIndRmwStage7Rla () = IdxIndRmwStage7 <| Rla

    let IdxIndRmwStage8 () = WriteMemory ea aluTemp <| ignore

    let PushP =
        (fun _ -> b <- 0x10) >> GetP >> PushDiscard

    let PushA () = PushDiscard a

    let PullPNoInc () =
        ReadMemoryS <| fun mem ->
            myIFlag <- i
            SetP mem
            iFlagPending <- i
            i <- myIFlag

    let PullANoInc () = ReadMemoryS <| SetAInternal

    let Imp operation = FetchDummy <| operation
    let ImpAslA () = Imp <| AslA
    let ImpRolA () = Imp <| RolA
    let ImpRorA () = Imp <| RorA
    let ImpLsrA () = Imp <| LsrA

    let JmpAbs () = ReadMemory pc <| SetPcJump
    let IncPc () = FetchDummy <| fun _ -> pc <- (pc + 1) &&& 0xFFFF

    let ZpRmwStage3 () = ReadMemory opcode2 <| SetAlu
    let ZpRmwStage5 () = WriteMemory opcode2 aluTemp <| ignore
    let ZpRmw operation = WriteMemory opcode2 aluTemp <| operation

    let ZpRmwInc () = ZpRmw <| Inc
    let ZpRmwDec () = ZpRmw <| Dec
    let ZpRmwAsl () = ZpRmw <| Asl
    let ZpRmwSre () = ZpRmw <| Sre
    let ZpRmwRra () = ZpRmw <| Rra
    let ZpRmwDcp () = ZpRmw <| Dcp
    let ZpRmwLsr () = ZpRmw <| Lsr
    let ZpRmwRor () = ZpRmw <| Ror
    let ZpRmwRol () = ZpRmw <| Rol
    let ZpRmwSlo () = ZpRmw <| Slo
    let ZpRmwIsc () = ZpRmw <| Isc
    let ZpRmwRla () = ZpRmw <| Rla

    let AbsIdxStage3 value =
        ReadMemoryPcIncrement <| (fun mem ->
            opcode3 <- mem
            aluTemp <- opcode2 + value
            ea <- (opcode3 <<< 8) + (aluTemp &&& 0xFF))

    let AbsIdxStage3X () = AbsIdxStage3 x
    let AbsIdxStage3Y () = AbsIdxStage3 y

    let AbsIdxReadStage4 () =
        IfReady <| (fun _ ->
            if aluTemp < 0x100 then
                restart <- true
            else
                aluTemp <- ReadMemoryInternal ea
                ea <- (ea + 0x100) &&& 0xFFFF)
        
    let AbsIdxStage4 () =
        ReadMemory ea <| fun mem ->
            ea <- ea + (aluTemp &&& 0x100)
            aluTemp <- mem

    let AbsIdxWriteStage5 value =
        WriteMemory ea value <| ignore

    let AbsIdxWriteStage5Sh register =
        aluTemp <- register &&& (ea >>> 8)
        ea <- (ea &&& 0xFF) ||| (aluTemp <<< 8)
        AbsIdxWriteStage5 aluTemp

    let AbsIdxWriteStage5Sta () = AbsIdxWriteStage5 a
    let AbsIdxWriteStage5Shy () = AbsIdxWriteStage5Sh y
    let AbsIdxWriteStage5Shx () = AbsIdxWriteStage5Sh x

    let AbsIdxWriteStage5Tas () =
        s <- a &&& x
        AbsIdxWriteStage5 (s &&& (ea >>> 8))

    let AbsIdxRmwStage5 () = ReadMemory ea <| SetAlu
    let AbsIdxRmwStage7 () = WriteMemory ea aluTemp <| ignore

    let AbsIdxRmwStage6 operation = WriteMemory ea aluTemp <| operation
    let AbsIdxRmwStage6Dec () = AbsIdxRmwStage6 <| Dec
    let AbsIdxRmwStage6Dcp () = AbsIdxRmwStage6 <| Dcp
    let AbsIdxRmwStage6Isc () = AbsIdxRmwStage6 <| Isc
    let AbsIdxRmwStage6Inc () = AbsIdxRmwStage6 <| Inc
    let AbsIdxRmwStage6Rol () = AbsIdxRmwStage6 <| Rol
    let AbsIdxRmwStage6Lsr () = AbsIdxRmwStage6 <| Lsr
    let AbsIdxRmwStage6Slo () = AbsIdxRmwStage6 <| Slo
    let AbsIdxRmwStage6Sre () = AbsIdxRmwStage6 <| Sre
    let AbsIdxRmwStage6Rra () = AbsIdxRmwStage6 <| Rra
    let AbsIdxRmwStage6Rla () = AbsIdxRmwStage6 <| Rla
    let AbsIdxRmwStage6Asl () = AbsIdxRmwStage6 <| Asl
    let AbsIdxRmwStage6Ror () = AbsIdxRmwStage6 <| Ror

    let AbsIdxReadStage5 operation = ReadMemory ea <| operation
    let AbsIdxReadStage5Lda () = AbsIdxReadStage5 <| Lda
    let AbsIdxReadStage5Ldx () = AbsIdxReadStage5 <| Ldx
    let AbsIdxReadStage5Lax () = AbsIdxReadStage5 <| Lax
    let AbsIdxReadStage5Ldy () = AbsIdxReadStage5 <| Ldy
    let AbsIdxReadStage5Ora () = AbsIdxReadStage5 <| Ora
    let AbsIdxReadStage5Nop () = AbsIdxReadStage5 <| ignore
    let AbsIdxReadStage5Cmp () = AbsIdxReadStage5 <| CmpA
    let AbsIdxReadStage5Sbc () = AbsIdxReadStage5 <| Sbc
    let AbsIdxReadStage5Adc () = AbsIdxReadStage5 <| Adc
    let AbsIdxReadStage5Eor () = AbsIdxReadStage5 <| Eor
    let AbsIdxReadStage5And () = AbsIdxReadStage5 <| And
    let AbsIdxReadStage5Las () = AbsIdxReadStage5 <| Las

    let AbsIndJmpStage4 () =
        IfReady <| fun _ ->
            ea <- (opcode3 <<< 8) + opcode2
            aluTemp <- ReadMemoryInternal ea

    let AbsIndJmpStage5 () =
        IfReady <| fun _ ->
            ea <- (opcode3 <<< 8) + ((opcode2 + 1) &&& 0xFF)
            pc <- aluTemp ||| (ReadMemoryInternal(ea) <<< 8)

    let AbsRmwStage4 () =
        IfReady <| fun _ ->
            ea <- (opcode3 <<< 8) + opcode2
            aluTemp <- ReadMemoryInternal ea

    let AbsRmwStage5 operation = WriteMemory ea aluTemp <| operation
    let AbsRmwStage5Inc () = AbsRmwStage5 <| Inc
    let AbsRmwStage5Dec () = AbsRmwStage5 <| Dec
    let AbsRmwStage5Dcp () = AbsRmwStage5 <| Dcp
    let AbsRmwStage5Isc () = AbsRmwStage5 <| Isc
    let AbsRmwStage5Asl () = AbsRmwStage5 <| Asl
    let AbsRmwStage5Ror () = AbsRmwStage5 <| Ror
    let AbsRmwStage5Slo () = AbsRmwStage5 <| Slo
    let AbsRmwStage5Rla () = AbsRmwStage5 <| Rla
    let AbsRmwStage5Sre () = AbsRmwStage5 <| Sre
    let AbsRmwStage5Rra () = AbsRmwStage5 <| Rra
    let AbsRmwStage5Rol () = AbsRmwStage5 <| Rol
    let AbsRmwStage5Lsr () = AbsRmwStage5 <| Lsr

    let AbsRmwStage6 () = WriteMemory ea aluTemp <| ignore

    let JamAddress address =
        FetchDiscard address <| ignore

    let JamFFFE () =
        JamAddress 0xFFFE

    let JamFFFF () =
        JamAddress 0xFFFF

    let FetchDummyOp () =
        FetchDummy <| ignore

    let NopOp () =
        ReadMemoryS <| ignore

    let EndISpecial () =
        opcode <- VopFetch1
        mi <- -1
        restart <- true
        true

    let EndSuppressInterrupt () =
        opcode <- VopFetch1NoInterrupt
        mi <- -1
        restart <- true
        true

    let End () =
        opcode <- VopFetch1
        mi <- -1
        iFlagPending <- i
        restart <- true
        true

    let EndBranchSpecial = End

    let Jam () = false

    let jamMicrocodes =
        [|
            Fetch2;
            JamFFFF;
            JamFFFE;
            JamFFFE;
            Jam;
        |]
    
    let microCode =
        [|
            // 00
            [| Fetch2; PushPch; PushPcl; PushPBrk; FetchPclVector; FetchPchVector; EndSuppressInterrupt |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Ora; End |];
            jamMicrocodes;
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Slo; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadNop; End |];
            [| Fetch2; ZpReadOra; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwAsl; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwSlo; ZpRmwStage5; End |];

            // 08
            [| FetchDummyOp; PushP; End |];
            [| ImmOra; End |];
            [| ImpAslA; End |];
            [| ImmAnc; End |];
            [| Fetch2; Fetch3; AbsReadNop; End |];
            [| Fetch2; Fetch3; AbsReadOra; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Asl; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Slo; AbsRmwStage6; End |];

            // 10
            [| RelBranchStage2Bpl; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Ora; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Slo; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadOra; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwAsl; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwSlo; ZpIdxRmwStage6; End |];

            // 18
            [| ImpClc; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Ora; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Slo; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Ora; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Asl; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Slo; AbsIdxRmwStage7; End |];

            // 20
            [| Fetch2; NopOp; PushPch; PushPcl; Jsr; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6And; End |];
            jamMicrocodes;
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Rla; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadBit; End |];
            [| Fetch2; ZpReadAnd; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwRol; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwRla; ZpRmwStage5; End |];

            // 28
            [| FetchDummyOp;  IncS; PullPNoInc; EndISpecial |];
            [| ImmAnd; End |];
            [| ImpRolA; End |];
            [| ImmAnc; End |];
            [| Fetch2; Fetch3; AbsReadBit; End |];
            [| Fetch2; Fetch3; AbsReadAnd; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Rol; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Rla; AbsRmwStage6; End |];

            // 30
            [| RelBranchStage2Bmi; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6And; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Rla; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadAnd; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwRol; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwRla; ZpIdxRmwStage6; End |];

            // 38
            [| ImpSec; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5And; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Rla; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5And; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Rol; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Rla; AbsIdxRmwStage7; End |];

            // 40
            [| FetchDummyOp; IncS; PullP; PullPcl; PullPchNoInc; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Eor; End |];
            jamMicrocodes;
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Sre; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadNop; End |];
            [| Fetch2; ZpReadEor; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwLsr; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwSre; ZpRmwStage5; End |];

            // 48
            [| FetchDummyOp; PushA; End |];
            [| ImmEor; End |];
            [| ImpLsrA; End |];
            [| ImmAsr; End |];
            [| Fetch2; JmpAbs; End |];
            [| Fetch2; Fetch3; AbsReadEor; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Lsr; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Sre; AbsRmwStage6; End |];

            // 50
            [| RelBranchStage2Bvc; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Eor; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Sre; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadEor; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwLsr; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwSre; ZpIdxRmwStage6; End |];

            // 58
            [| ImpCli; EndISpecial |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Eor; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Sre; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Eor; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Lsr; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Sre; AbsIdxRmwStage7; End |];

            // 60
            [| FetchDummyOp; IncS; PullPcl; PullPchNoInc; IncPc; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Adc; End |];
            jamMicrocodes;
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Rra; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadNop; End |];
            [| Fetch2; ZpReadAdc; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwRor; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwRra; ZpRmwStage5; End |];

            // 68
            [| FetchDummyOp; IncS; PullANoInc; End |];
            [| ImmAdc; End |];
            [| ImpRorA; End |];
            [| ImmArr; End |];
            [| Fetch2; Fetch3; AbsIndJmpStage4; AbsIndJmpStage5; End |];
            [| Fetch2; Fetch3; AbsReadAdc; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Ror; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Rra; AbsRmwStage6; End |];

            // 70
            [| RelBranchStage2Bvs; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Adc; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Rra; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadAdc; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwRor; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwRra; ZpIdxRmwStage6; End |];

            // 78
            [| ImpSei; EndISpecial |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Adc; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Rra; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Adc; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Ror; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Rra; AbsIdxRmwStage7; End |];

            // 80
            [| ImmUnsupported; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndWriteStage6Sta; End |];
            [| ImmUnsupported; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndWriteStage6Sax; End |];
            [| Fetch2; ZpWriteSty; End |];
            [| Fetch2; ZpWriteSta; End |];
            [| Fetch2; ZpWriteStx; End |];
            [| Fetch2; ZpWriteSax; End |];

            // 88
            [| ImpDey; End |];
            [| ImmUnsupported; End |];
            [| ImpTxa; End |];
            [| ImmUnsupported; End |];
            [| Fetch2; Fetch3; AbsWriteSty; End |];
            [| Fetch2; Fetch3; AbsWriteSta; End |];
            [| Fetch2; Fetch3; AbsWriteStx; End |];
            [| Fetch2; Fetch3; AbsWriteSax; End |];

            // 90
            [| RelBranchStage2Bcc; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxWriteStage5; IndIdxWriteStage6Sta; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxWriteStage5; IndIdxWriteStage6Sha; End |];
            [| Fetch2; ZpIdxStage3X; ZpWriteSty; End |];
            [| Fetch2; ZpIdxStage3X; ZpWriteSta; End |];
            [| Fetch2; ZpIdxStage3Y; ZpWriteStx; End |];
            [| Fetch2; ZpIdxStage3Y; ZpWriteSax; End |];

            // 98
            [| ImpTya; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxStage4; AbsIdxWriteStage5Sta; End |];
            [| ImpTxs; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxStage4; AbsIdxWriteStage5Tas; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxStage4; AbsIdxWriteStage5Shy; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxStage4; AbsIdxWriteStage5Sta; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxStage4; AbsIdxWriteStage5Shx; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxStage4; AbsIdxWriteStage5Tas; End |];

            // A0
            [| ImmLdy; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Lda; End |];
            [| ImmLdx; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Lax; End |];
            [| Fetch2; ZpReadLdy; End |];
            [| Fetch2; ZpReadLda; End |];
            [| Fetch2; ZpReadLdx; End |];
            [| Fetch2; ZpReadLax; End |];

            // A8
            [| ImpTay; End |];
            [| ImmLda; End |];
            [| ImpTax; End |];
            [| ImmLxa; End |];
            [| Fetch2; Fetch3; AbsReadLdy; End |];
            [| Fetch2; Fetch3; AbsReadLda; End |];
            [| Fetch2; Fetch3; AbsReadLdx; End |];
            [| Fetch2; Fetch3; AbsReadLax; End |];

            // B0
            [| RelBranchStage2Bcs; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Lda; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Lax; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadLdy; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadLda; End |];
            [| Fetch2; ZpIdxStage3Y; ZpReadLdx; End |];
            [| Fetch2; ZpIdxStage3Y; ZpReadLax; End |];

            // B8
            [| ImpClv; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Lda; End |];
            [| ImpTsx; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Las; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Ldy; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Lda; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Ldx; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Lax; End |];

            // C0
            [| ImmCpy; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Cmp; End |];
            [| ImmUnsupported; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Dcp; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadCpy; End |];
            [| Fetch2; ZpReadCmp; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwDec; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwDcp; ZpRmwStage5; End |];

            // C8
            [| ImpIny; End |];
            [| ImmCmp; End |];
            [| ImpDex; End |];
            [| ImmAxs; End |];
            [| Fetch2; Fetch3; AbsReadCpy; End |];
            [| Fetch2; Fetch3; AbsReadCmp; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Dec; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Dcp; AbsRmwStage6; End |];

            // D0
            [| RelBranchStage2Bne; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Cmp; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Dcp; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadCmp; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwDec; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwDcp; ZpIdxRmwStage6; End |];

            // D8
            [| ImpCld; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Cmp; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Dcp; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End|];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Cmp; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Dec; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Dcp; AbsIdxRmwStage7; End |];

            // E0
            [| ImmCpx; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndReadStage6Sbc; End |];
            [| ImmUnsupported; End |];
            [| Fetch2; IdxIndStage3; IdxIndStage4; IdxIndStage5; IdxIndRmwStage6; IdxIndRmwStage7Isc; IdxIndRmwStage8; End |];
            [| Fetch2; ZpReadCpx; End |];
            [| Fetch2; ZpReadSbc; End|];
            [| Fetch2; ZpRmwStage3; ZpRmwInc; ZpRmwStage5; End |];
            [| Fetch2; ZpRmwStage3; ZpRmwIsc; ZpRmwStage5; End |];

            // E8
            [| ImpInx; End |];
            [| ImmSbc; End |];
            [| FetchDummyOp; End |];
            [| ImmSbc; End |];
            [| Fetch2; Fetch3; AbsReadCpx; End|];
            [| Fetch2; Fetch3; AbsReadSbc; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Inc; AbsRmwStage6; End |];
            [| Fetch2; Fetch3; AbsRmwStage4; AbsRmwStage5Isc; AbsRmwStage6; End |];

            // F0
            [| RelBranchStage2Beq; End |];
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxReadStage5; IndIdxReadStage6Sbc; End |];
            jamMicrocodes;
            [| Fetch2; IndIdxStage3; IndIdxStage4; IndIdxRmwStage5; IndIdxRmwStage6; IndIdxRmwStage7Isc; IndIdxRmwStage8; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadNop; End |];
            [| Fetch2; ZpIdxStage3X; ZpReadSbc; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwInc; ZpIdxRmwStage6; End |];
            [| Fetch2; ZpIdxStage3X; ZpIdxRmwStage4; ZpRmwIsc; ZpIdxRmwStage6; End |];

            // F8
            [| ImpSed; End |];
            [| Fetch2; AbsIdxStage3Y; AbsIdxReadStage4; AbsIdxReadStage5Sbc; End |];
            [| FetchDummyOp; End |];
            [| Fetch2; AbsIdxStage3Y;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Isc; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Nop; End |];
            [| Fetch2; AbsIdxStage3X; AbsIdxReadStage4; AbsIdxReadStage5Sbc; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Inc; AbsIdxRmwStage7; End |];
            [| Fetch2; AbsIdxStage3X;  AbsIdxStage4; AbsIdxRmwStage5; AbsIdxRmwStage6Isc; AbsIdxRmwStage7; End |];

            // 100 (VOP_Fetch1)
            [| Fetch1 |];
            // 101 (VOP_RelativeStuff)
            [| RelBranchStage3; EndBranchSpecial |];
            // 102 (VOP_RelativeStuff2)
            [| RelBranchStage4; End |];
            // 103 (VOP_RelativeStuff3)
            [| EndSuppressInterrupt |]
            // 104 (VOP_NMI)
            [| FetchDummyOp; FetchDummyOp; PushPch; PushPcl; PushPNmi; FetchPclVector; FetchPchVector; EndSuppressInterrupt |];
            // 105 (VOP_IRQ)
            [| FetchDummyOp; FetchDummyOp; PushPch; PushPcl; PushPIrq; FetchPclVector; FetchPchVector; EndSuppressInterrupt |];
            // 106 (VOP_RESET)
            [| FetchDummyOp; FetchDummyOp; FetchDummyOp; PushDummy; PushDummy; PushPReset; FetchPclVector; FetchPchVector; EndSuppressInterrupt |];
            // 107 (VOP_Fetch1_NoInterrupt)
            [| Fetch1Real |];
        |]

    let rec ExecuteOneRetryInternal () =
        if microCode.[opcode].[mi]() then
            mi <- mi + 1

        if restart then
            restart <- false
            ExecuteOneRetryInternal()
        else
            totalCycles <- totalCycles + 1UL

    let ExecuteOneRetry () =
        let thisNmi = readNmi()
        rdy <- readRdy()
        irq <- irq || readIrq()
        nmi <- nmi || ((not lastNmi) && thisNmi)
        interruptPending <- nmi || irq
        lastNmi <- thisNmi
        ExecuteOneRetryInternal()

    member this.Clock () =
        ExecuteOneRetry()

    member this.ClockMultiple count =
        let mutable remaining = count
        while remaining > 0 do
            ExecuteOneRetry()
            remaining <- remaining - 1

    member this.ClockToAddress address =
        while not (pc = address) do
            ExecuteOneRetry()

    member this.ClockStep () =
        rdy <- readRdy()
        if rdy then
            while mi <= 0 do
                ExecuteOneRetry()
            while mi > 0 do
                ExecuteOneRetry()            

    member this.A = a
    member this.X = x
    member this.Y = y
    member this.P = GetP()
    member this.S = s
    member this.PC = pc
    member this.N = n <> 0
    member this.V = v <> 0
    member this.B = b <> 0
    member this.I = i
    member this.Z = z
    member this.C = c <> 0
    member this.D = d

    member this.Sync =
        mi >= microCode.[opcode].Length - 1

    member this.SetA value = a <- value &&& 0xFF
    member this.SetX value = x <- value &&& 0xFF
    member this.SetY value = y <- value &&& 0xFF
    member this.SetS value = s <- value &&& 0xFF
    member this.SetPC value = pc <- value &&& 0xFFFF
    member this.SetN value = n <- if value then 0x80 else 0x00
    member this.SetV value = v <- if value then 0x40 else 0x00
    member this.SetB value = b <- if value then 0x10 else 0x00
    member this.SetI value = i <- value; iFlagPending <- value
    member this.SetZ value = z <- value
    member this.SetC value = c <- if value then 0x01 else 0x00
    member this.SetD value = d <- value; isDecimalMode <- d && hasDecimalMode

    member this.SetOpcode value =
        opcode <- value
        branchIrqHack <- false
        mi <- 0
        restart <- false

    member this.ForceOpcodeSync () =
        opcode <- VopFetch1

    member this.TotalCycles = totalCycles

    member this.SoftReset () = SoftResetInternal()

    member this.HardReset () = HardResetInternal()

