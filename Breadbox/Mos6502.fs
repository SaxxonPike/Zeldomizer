namespace Breadbox

// 6502/6510 core.
// Ported from Bizhawk's C# core.

type private Uop =
    | Fetch1
    | Fetch1Real
    | Fetch2
    | Fetch3
    | FetchDummy
    | Nop
    | Jsr
    | IncPc
    | AbsWriteSta
    | AbsWriteStx
    | AbsWriteSty
    | AbsWriteSax
    | AbsReadBit
    | AbsReadLda
    | AbsReadLdy
    | AbsReadOra
    | AbsReadLdx
    | AbsReadCmp
    | AbsReadAdc
    | AbsReadCpx
    | AbsReadSbc
    | AbsReadAnd
    | AbsReadEor
    | AbsReadCpy
    | AbsReadNop
    | AbsReadLax
    | AbsRmwStage4
    | AbsRmwStage6
    | AbsRmwStage5Inc
    | AbsRmwStage5Dec
    | AbsRmwStage5Lsr
    | AbsRmwStage5Rol
    | AbsRmwStage5Asl
    | AbsRmwStage5Ror
    | AbsRmwStage5Slo
    | AbsRmwStage5Rla
    | AbsRmwStage5Sre
    | AbsRmwStage5Rra
    | AbsRmwStage5Dcp
    | AbsRmwStage5Isc
    | JmpAbs
    | ZpIdxStage3X
    | ZpIdxStage3Y
    | ZpIdxRmwStage4
    | ZpIdxRmwStage6
    | ZpWriteSta
    | ZpWriteStx
    | ZpWriteSty
    | ZpWriteSax
    | ZpRmwStage3
    | ZpRmwStage5
    | ZpRmwDec
    | ZpRmwInc
    | ZpRmwAsl
    | ZpRmwLsr
    | ZpRmwRor
    | ZpRmwRol
    | ZpRmwSlo
    | ZpRmwRla
    | ZpRmwSre
    | ZpRmwRra
    | ZpRmwDcp
    | ZpRmwIsc
    | ZpReadEor
    | ZpReadBit
    | ZpReadOra
    | ZpReadLda
    | ZpReadLdy
    | ZpReadLdx
    | ZpReadCpx
    | ZpReadSbc
    | ZpReadCpy
    | ZpReadNop
    | ZpReadAdc
    | ZpReadAnd
    | ZpReadCmp
    | ZpReadLax
    | IdxIndStage3
    | IdxIndStage4
    | IdxIndStage5
    | IdxIndReadStage6Ora
    | IdxIndReadStage6Sbc
    | IdxIndReadStage6Lda
    | IdxIndReadStage6Eor
    | IdxIndReadStage6Cmp
    | IdxIndReadStage6Adc
    | IdxIndReadStage6And
    | IdxIndReadStage6Lax
    | IdxIndWriteStage6Sta
    | IdxIndWriteStage6Sax
    | IdxIndRmwStage6
    | IdxIndRmwStage7Slo
    | IdxIndRmwStage7Rla
    | IdxIndRmwStage7Sre
    | IdxIndRmwStage7Rra
    | IdxIndRmwStage7Isc
    | IdxIndRmwStage7Dcp
    | IdxIndRmwStage8
    | AbsIdxStage3X
    | AbsIdxStage3Y
    | AbsIdxStage4
    | AbsIdxWriteStage5Sta
    | AbsIdxWriteStage5Shy
    | AbsIdxWriteStage5Shx
    | AbsIdxWriteStage5Tas
    | AbsIdxReadStage4
    | AbsIdxReadStage5Lda
    | AbsIdxReadStage5Cmp
    | AbsIdxReadStage5Sbc
    | AbsIdxReadStage5Adc
    | AbsIdxReadStage5Eor
    | AbsIdxReadStage5Ldx
    | AbsIdxReadStage5And
    | AbsIdxReadStage5Ora
    | AbsIdxReadStage5Ldy
    | AbsIdxReadStage5Nop
    | AbsIdxReadStage5Lax
    | AbsIdxReadStage5Las
    | AbsIdxRmwStage5
    | AbsIdxRmwStage7
    | AbsIdxRmwStage6Ror
    | AbsIdxRmwStage6Dec
    | AbsIdxRmwStage6Inc
    | AbsIdxRmwStage6Asl
    | AbsIdxRmwStage6Lsr
    | AbsIdxRmwStage6Rol
    | AbsIdxRmwStage6Slo
    | AbsIdxRmwStage6Rla
    | AbsIdxRmwStage6Sre
    | AbsIdxRmwStage6Rra
    | AbsIdxRmwStage6Dcp
    | AbsIdxRmwStage6Isc
    | IncS
    | PushPcl
    | PushPch
    | PushP
    | PullP
    | PullPcl
    | PullPchNoInc
    | PushA
    | PullANoInc
    | PullPNoInc
    | PushPBrk
    | PushPNmi
    | PushPIrq
    | PushPReset
    | PushDummy
    | FetchPclVector
    | FetchPchVector
    | ImpAslA
    | ImpRolA
    | ImpRorA
    | ImpLsrA
    | ImpSec
    | ImpCli
    | ImpSei
    | ImpCld
    | ImpClc
    | ImpClv
    | ImpSed
    | ImpIny
    | ImpDey
    | ImpInx
    | ImpDex
    | ImpTsx
    | ImpTxs
    | ImpTax
    | ImpTay
    | ImpTya
    | ImpTxa
    | ImmCmp
    | ImmAdc
    | ImmAnd
    | ImmSbc
    | ImmOra
    | ImmEor
    | ImmCpy
    | ImmCpx
    | ImmAnc
    | ImmAsr
    | ImmArr
    | ImmLxa
    | ImmAxs
    | ImmLda
    | ImmLdx
    | ImmLdy
    | ImmUnsupported
    | NzX
    | NzY
    | NzA
    | RelBranchStage2Bne
    | RelBranchStage2Bpl
    | RelBranchStage2Bcc
    | RelBranchStage2Bcs
    | RelBranchStage2Beq
    | RelBranchStage2Bmi
    | RelBranchStage2Bvc
    | RelBranchStage2Bvs
    | RelBranchStage3
    | RelBranchStage4
    | AbsIndJmpStage4
    | AbsIndJmpStage5
    | IndIdxStage3
    | IndIdxStage4
    | IndIdxReadStage5
    | IndIdxWriteStage5
    | IndIdxWriteStage6Sta
    | IndIdxWriteStage6Sha
    | IndIdxReadStage6Lda
    | IndIdxReadStage6Cmp
    | IndIdxReadStage6Ora
    | IndIdxReadStage6Sbc
    | IndIdxReadStage6Adc
    | IndIdxReadStage6And
    | IndIdxReadStage6Eor
    | IndIdxReadStage6Lax
    | IndIdxRmwStage5
    | IndIdxRmwStage6
    | IndIdxRmwStage7Slo
    | IndIdxRmwStage7Rla
    | IndIdxRmwStage7Sre
    | IndIdxRmwStage7Rra
    | IndIdxRmwStage7Isc
    | IndIdxRmwStage7Dcp
    | IndIdxRmwStage8
    | End
    | EndISpecial
    | EndBranchSpecial
    | EndSuppressInterrupt
    | Jam
    | JamFFFF
    | JamFFFE

type Mos6502Configuration(lxaConstant:int, hasDecimalMode:bool, port:IPort, memory:IMemory, ready:IReadySignal) =
    member val LxaConstant = lxaConstant
    member val HasDecimalMode = hasDecimalMode
    member val Memory = memory
    member val Ready = ready
    member val Port = port
    member val HasPort =
        match box port with
            | null -> false
            | _ -> true

type Mos6502(config:Mos6502Configuration) =
    let jamMicrocodes =
        [|
            Uop.Fetch2;
            Uop.JamFFFF;
            Uop.JamFFFE;
            Uop.JamFFFE;
            Uop.Jam;
        |]
    
    let microCode =
        [|
            // 00
            [| Uop.Fetch2; Uop.PushPch; Uop.PushPcl; Uop.PushPBrk; Uop.FetchPclVector; Uop.FetchPchVector; Uop.EndSuppressInterrupt |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Ora; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Slo; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadOra; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwAsl; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwSlo; Uop.ZpRmwStage5; Uop.End |];

            // 08
            [| Uop.FetchDummy; Uop.PushP; Uop.End |];
            [| Uop.ImmOra; Uop.End |];
            [| Uop.ImpAslA; Uop.End |];
            [| Uop.ImmAnc; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadOra; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Asl; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Slo; Uop.AbsRmwStage6; Uop.End |];

            // 10
            [| Uop.RelBranchStage2Bpl; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Ora; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Slo; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadOra; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwAsl; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwSlo; Uop.ZpIdxRmwStage6; Uop.End |];

            // 18
            [| Uop.ImpClc; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Ora; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Slo; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Ora; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Asl; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Slo; Uop.AbsIdxRmwStage7; Uop.End |];

            // 20
            [| Uop.Fetch2; Uop.Nop; Uop.PushPch; Uop.PushPcl; Uop.Jsr; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6And; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Rla; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadBit; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadAnd; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwRol; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwRla; Uop.ZpRmwStage5; Uop.End |];

            // 28
            [| Uop.FetchDummy;  Uop.IncS; Uop.PullPNoInc; Uop.EndISpecial |];
            [| Uop.ImmAnd; Uop.End |];
            [| Uop.ImpRolA; Uop.End |];
            [| Uop.ImmAnc; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadBit; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadAnd; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Rol; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Rla; Uop.AbsRmwStage6; Uop.End |];

            // 30
            [| Uop.RelBranchStage2Bmi; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6And; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Rla; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadAnd; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwRol; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwRla; Uop.ZpIdxRmwStage6; Uop.End |];

            // 38
            [| Uop.ImpSec; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5And; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Rla; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5And; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Rol; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Rla; Uop.AbsIdxRmwStage7; Uop.End |];

            // 40
            [| Uop.FetchDummy; Uop.IncS; Uop.PullP; Uop.PullPcl; Uop.PullPchNoInc; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Eor; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Sre; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadEor; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwLsr; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwSre; Uop.ZpRmwStage5; Uop.End |];

            // 48
            [| Uop.FetchDummy; Uop.PushA; Uop.End |];
            [| Uop.ImmEor; Uop.End |];
            [| Uop.ImpLsrA; Uop.End |];
            [| Uop.ImmAsr; Uop.End |];
            [| Uop.Fetch2; Uop.JmpAbs; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadEor; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Lsr; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Sre; Uop.AbsRmwStage6; Uop.End |];

            // 50
            [| Uop.RelBranchStage2Bvc; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Eor; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Sre; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadEor; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwLsr; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwSre; Uop.ZpIdxRmwStage6; Uop.End |];

            // 58
            [| Uop.ImpCli; Uop.EndISpecial |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Eor; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Sre; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Eor; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Lsr; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Sre; Uop.AbsIdxRmwStage7; Uop.End |];

            // 60
            [| Uop.FetchDummy; Uop.IncS; Uop.PullPcl; Uop.PullPchNoInc; Uop.IncPc; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Adc; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Rra; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadAdc; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwRor; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwRra; Uop.ZpRmwStage5; Uop.End |];

            // 68
            [| Uop.FetchDummy; Uop.IncS; Uop.PullANoInc; Uop.End |];
            [| Uop.ImmAdc; Uop.End |];
            [| Uop.ImpRorA; Uop.End |];
            [| Uop.ImmArr; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsIndJmpStage4; Uop.AbsIndJmpStage5; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadAdc; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Ror; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Rra; Uop.AbsRmwStage6; Uop.End |];

            // 70
            [| Uop.RelBranchStage2Bvs; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Adc; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Rra; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadAdc; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwRor; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwRra; Uop.ZpIdxRmwStage6; Uop.End |];

            // 78
            [| Uop.ImpSei; Uop.EndISpecial |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Adc; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Rra; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Adc; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Ror; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Rra; Uop.AbsIdxRmwStage7; Uop.End |];

            // 80
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndWriteStage6Sta; Uop.End |];
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndWriteStage6Sax; Uop.End |];
            [| Uop.Fetch2; Uop.ZpWriteSty; Uop.End |];
            [| Uop.Fetch2; Uop.ZpWriteSta; Uop.End |];
            [| Uop.Fetch2; Uop.ZpWriteStx; Uop.End |];
            [| Uop.Fetch2; Uop.ZpWriteSax; Uop.End |];

            // 88
            [| Uop.ImpDey; Uop.End |];
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.ImpTxa; Uop.End |];
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsWriteSty; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsWriteSta; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsWriteStx; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsWriteSax; Uop.End |];

            // 90
            [| Uop.RelBranchStage2Bcc; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxWriteStage5; Uop.IndIdxWriteStage6Sta; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxWriteStage5; Uop.IndIdxWriteStage6Sha; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpWriteSty; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpWriteSta; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3Y; Uop.ZpWriteStx; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3Y; Uop.ZpWriteSax; Uop.End |];

            // 98
            [| Uop.ImpTya; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Sta; Uop.End |];
            [| Uop.ImpTxs; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Tas; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Shy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Sta; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Shx; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxStage4; Uop.AbsIdxWriteStage5Tas; Uop.End |];

            // A0
            [| Uop.ImmLdy; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Lda; Uop.End |];
            [| Uop.ImmLdx; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Lax; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadLdy; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadLda; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadLdx; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadLax; Uop.End |];

            // A8
            [| Uop.ImpTay; Uop.End |];
            [| Uop.ImmLda; Uop.End |];
            [| Uop.ImpTax; Uop.End |];
            [| Uop.ImmLxa; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadLdy; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadLda; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadLdx; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadLax; Uop.End |];

            // B0
            [| Uop.RelBranchStage2Bcs; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Lda; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Lax; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadLdy; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadLda; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3Y; Uop.ZpReadLdx; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3Y; Uop.ZpReadLax; Uop.End |];

            // B8
            [| Uop.ImpClv; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Lda; Uop.End |];
            [| Uop.ImpTsx; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Las; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Ldy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Lda; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Ldx; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Lax; Uop.End |];

            // C0
            [| Uop.ImmCpy; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Cmp; Uop.End |];
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Dcp; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadCpy; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadCmp; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwDec; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwDcp; Uop.ZpRmwStage5; Uop.End |];

            // C8
            [| Uop.ImpIny; Uop.End |];
            [| Uop.ImmCmp; Uop.End |];
            [| Uop.ImpDex; Uop.End |];
            [| Uop.ImmAxs; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadCpy; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadCmp; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Dec; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Dcp; Uop.AbsRmwStage6; Uop.End |];

            // D0
            [| Uop.RelBranchStage2Bne; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Cmp; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Dcp; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadCmp; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwDec; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwDcp; Uop.ZpIdxRmwStage6; Uop.End |];

            // D8
            [| Uop.ImpCld; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Cmp; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Dcp; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End|];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Cmp; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Dec; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Dcp; Uop.AbsIdxRmwStage7; Uop.End |];

            // E0
            [| Uop.ImmCpx; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndReadStage6Sbc; Uop.End |];
            [| Uop.ImmUnsupported; Uop.End |];
            [| Uop.Fetch2; Uop.IdxIndStage3; Uop.IdxIndStage4; Uop.IdxIndStage5; Uop.IdxIndRmwStage6; Uop.IdxIndRmwStage7Isc; Uop.IdxIndRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadCpx; Uop.End |];
            [| Uop.Fetch2; Uop.ZpReadSbc; Uop.End|];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwInc; Uop.ZpRmwStage5; Uop.End |];
            [| Uop.Fetch2; Uop.ZpRmwStage3; Uop.ZpRmwIsc; Uop.ZpRmwStage5; Uop.End |];

            // E8
            [| Uop.ImpInx; Uop.End |];
            [| Uop.ImmSbc; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.ImmSbc; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadCpx; Uop.End|];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsReadSbc; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Inc; Uop.AbsRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.Fetch3; Uop.AbsRmwStage4; Uop.AbsRmwStage5Isc; Uop.AbsRmwStage6; Uop.End |];

            // F0
            [| Uop.RelBranchStage2Beq; Uop.End |];
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxReadStage5; Uop.IndIdxReadStage6Sbc; Uop.End |];
            jamMicrocodes;
            [| Uop.Fetch2; Uop.IndIdxStage3; Uop.IndIdxStage4; Uop.IndIdxRmwStage5; Uop.IndIdxRmwStage6; Uop.IndIdxRmwStage7Isc; Uop.IndIdxRmwStage8; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadNop; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpReadSbc; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwInc; Uop.ZpIdxRmwStage6; Uop.End |];
            [| Uop.Fetch2; Uop.ZpIdxStage3X; Uop.ZpIdxRmwStage4; Uop.ZpRmwIsc; Uop.ZpIdxRmwStage6; Uop.End |];

            // F8
            [| Uop.ImpSed; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Sbc; Uop.End |];
            [| Uop.FetchDummy; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3Y;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Isc; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Nop; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X; Uop.AbsIdxReadStage4; Uop.AbsIdxReadStage5Sbc; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Inc; Uop.AbsIdxRmwStage7; Uop.End |];
            [| Uop.Fetch2; Uop.AbsIdxStage3X;  Uop.AbsIdxStage4; Uop.AbsIdxRmwStage5; Uop.AbsIdxRmwStage6Isc; Uop.AbsIdxRmwStage7; Uop.End |];

            // 100 (VOP_Fetch1)
            [| Uop.Fetch1 |];
            // 101 (VOP_RelativeStuff)
            [| Uop.RelBranchStage3; Uop.EndBranchSpecial |];
            // 102 (VOP_RelativeStuff2)
            [| Uop.RelBranchStage4; Uop.End |];
            // 103 (VOP_RelativeStuff3)
            [| Uop.EndSuppressInterrupt |]
            // 104 (VOP_NMI)
            [| Uop.FetchDummy; Uop.FetchDummy; Uop.PushPch; Uop.PushPcl; Uop.PushPNmi; Uop.FetchPclVector; Uop.FetchPchVector; Uop.EndSuppressInterrupt |];
            // 105 (VOP_IRQ)
            [| Uop.FetchDummy; Uop.FetchDummy; Uop.PushPch; Uop.PushPcl; Uop.PushPIrq; Uop.FetchPclVector; Uop.FetchPchVector; Uop.EndSuppressInterrupt |];
            // 106 (VOP_RESET)
            [| Uop.FetchDummy; Uop.FetchDummy; Uop.FetchDummy; Uop.PushDummy; Uop.PushDummy; Uop.PushPReset; Uop.FetchPclVector; Uop.FetchPchVector; Uop.EndSuppressInterrupt |];
            // 107 (VOP_Fetch1_NoInterrupt)
            [| Uop.Fetch1Real |];
        |]

    [<Literal>]
    let vopFetch1 = 0x100
    [<Literal>]
    let vopRelativeStuff = 0x101
    [<Literal>]
    let vopRelativeStuff2 = 0x102
    [<Literal>]
    let vopRelativeStuff3 = 0x103
    [<Literal>]
    let vopNmi = 0x104
    [<Literal>]
    let vopIrq = 0x105
    [<Literal>]
    let vopReset = 0x106
    [<Literal>]
    let vopFetch1NoInterrupt = 0x107


    [<Literal>]
    let nmiVector = 0xFFFA
    [<Literal>]
    let resetVector = 0xFFFC
    [<Literal>]
    let irqVector = 0xFFFE
    [<Literal>]
    let brkVector = 0xFFFE

    let mutable restart = false
    let mutable opcode = vopReset
    let mutable opcode2 = 0
    let mutable opcode3 = 0
    let mutable ea = 0
    let mutable aluTemp = 0
    let mutable mi = 0
    let mutable myIFlag = false
    let mutable iFlagPending = true
    let mutable rdyFreeze = false
    let mutable interruptPending = false
    let mutable branchIrqHack = false
    let mutable irq = false
    let mutable nmi = false
    let mutable rdy = false

    let mutable pc = 0
    let mutable a = 0
    let mutable x = 0
    let mutable y = 0
    let mutable s = 0

    let mutable isDecimalMode = false

    let mutable n = false
    let mutable v = false
    let mutable b = false
    let mutable d = false
    let mutable i = true
    let mutable z = false
    let mutable c = false

    let mutable ioLatch = 0xFF
    let mutable ioDirection = 0x00

    let mutable totalCycles = 0UL

    let lxaConstant = config.LxaConstant
    let hasDecimalMode = config.HasDecimalMode
    let readRdy = config.Ready.ReadRdy
    let memoryReadRaw = config.Memory.Read
    let memoryWriteRaw = config.Memory.Write

    let read =
        if config.HasPort then
            let readPort =
                if config.HasPort then
                    config.Port.ReadPort
                else
                    fun _ ->
                        0xFF
            fun address ->
                match address with
                    | 0x00 ->
                        memoryReadRaw(address) |> ignore
                        ioDirection
                    | 0x01 ->
                        memoryReadRaw(address) |> ignore
                        (ioLatch ||| ((~~~ioDirection) &&& readPort())) &&& 0xFF
                    | _ -> memoryReadRaw(address)
        else
            memoryReadRaw

    let write =
        if config.HasPort then
            fun (address, value) ->
                match address with
                    | 0x00 ->
                        memoryReadRaw(address) |> ignore
                        ioDirection <- value
                    | 0x01 ->
                        memoryReadRaw(address) |> ignore
                        ioLatch <- value
                    | _ -> memoryWriteRaw(address, value)
        else
            memoryWriteRaw

    let SoftReset () =
        i <- true
        iFlagPending <- true
        mi <- 0
        opcode <- vopReset

    let HardReset () =
        a <- 0x00
        x <- 0x00
        y <- 0x00
        n <- false
        v <- false
        b <- false
        d <- false
        z <- false
        c <- false
        SoftReset()

    let GetP () =
        0x20 |||
        (if n then 0x80 else 0x00) |||
        (if v then 0x40 else 0x00) |||
        (if b then 0x10 else 0x00) |||
        (if d then 0x08 else 0x00) |||
        (if i then 0x04 else 0x00) |||
        (if z then 0x02 else 0x00) |||
        (if c then 0x01 else 0x00)

    let SetP value =
        n <- (value &&& 0x80) <> 0
        v <- (value &&& 0x40) <> 0
        b <- (value &&& 0x10) <> 0
        d <- (value &&& 0x08) <> 0
        i <- (value &&& 0x04) <> 0
        z <- (value &&& 0x02) <> 0
        c <- (value &&& 0x01) <> 0

    let NZ value =
        z <- (value &&& 0xFF) = 0
        n <- (value &&& 0x80) <> 0

    let NZA () =
        NZ a

    let NZX () =
        NZ x

    let NZY () =
        NZ y

    let ReadMemoryInternal address =
        read address

    let ReadMemory address operation =
        if rdy then
            ReadMemoryInternal address |> operation
        rdy

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
    let SetPc low high = pc <- (high <<< 8) ||| low
    let SetPcl value = pc <- (pc &&& 0xFF00) ||| value
    let SetPch value = pc <- (pc &&& 0x00FF) ||| (value <<< 8)
    let SetNZA value =
        a <- value
        NZ a
    let SetNZX value =
        x <- value
        NZ x
    let SetNZY value =
        y <- value
        NZ y
    let SetAlu value =
        aluTemp <- value
    let SetNZAlu value =
        SetAlu value
        NZ aluTemp
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
        match rdy with
            | true -> action(); true
            | _ -> false


    // ----- ALU -----


    let Cmp register value =
        match (register - value) &&& 0xFF with
            | result ->
                aluTemp <- result
                c <- register >= result
                NZ result

    let CmpA value = Cmp a value
    let CmpX value = Cmp x value
    let CmpY value = Cmp y value

    let And value =
        match (a &&& value) with
            | result ->
                aluTemp <- result
                a <- result
                NZ result

    let Bit value =
        aluTemp <- value
        n <- (value &&& 0x80) <> 0
        v <- (value &&& 0x40) <> 0
        z <- (a &&& value) = 0

    let Eor value =
        match (a ^^^ value) with
            | result ->
                aluTemp <- result
                a <- result
                NZ result

    let Ora value =
        match (a ||| value) with
            | result ->
                aluTemp <- result
                a <- result
                NZ result

    let Anc value =
        match (a &&& value) with
            | result ->
                aluTemp <- result
                c <- (result &&& 0x80) <> 0
                a <- result
                NZ result

    let Asr value =
        match (a &&& value) with
            | andResult ->
                c <- (andResult &&& 0x01) <> 0
                match (andResult >>> 1) with
                    | result ->
                        aluTemp <- result
                        a <- result
                        NZ result

    let Axs value =
        Cmp (x &&& a) value
        x <- aluTemp

    let Arr value =
        aluTemp <-
            let initialMask = a &&& value
            let binaryResult = (initialMask >>> 1) ||| (if c then 0x80 else 0x00)
            if isDecimalMode then
                n <- (a &&& 0x80) <> 0
                z <- binaryResult = 0
                v <- (binaryResult ^^^ initialMask) &&& 0x40 <> 0
                let lowResult =
                    match ((initialMask &&& 0xf) + (initialMask &&& 0x1)) with
                        | x when x > 0x5 -> (binaryResult &&& 0xf0) ||| ((binaryResult + 0x6) &&& 0xf)
                        | _ -> binaryResult
                if ((initialMask &&& 0xf0) + (initialMask &&& 0x10)) > 0x50 then
                    c <- true
                    (lowResult &&& 0x0f) ||| ((lowResult + 0x60) &&& 0xf0)
                else
                    c <- false
                    lowResult
            else
                NZ binaryResult
                c <- (binaryResult &&& 0x40) <> 0
                v <- (binaryResult &&& 0x40) <> ((binaryResult &&& 0x20) <<< 1)
                binaryResult
        a <- aluTemp

    let Lxa value =
        match ((a ||| lxaConstant) &&& value) with
            | result ->
                aluTemp <- result
                a <- result
                x <- result
                NZ result

    let Sbc value =
        let inline setV sum operand =
            v <- (a ^^^ sum) &&& (a ^^^ operand) &&& 0x80 <> 0
        aluTemp <-
            let binaryResult = a - value - (if c then 0 else 1)
            if isDecimalMode then
                let initialSub = (a &&& 0x0F) - (value &&& 0x0F) - (if c then 0 else 1)
                let adjustedSub =
                    match (initialSub &&& 0x10) with
                        | 0 -> (initialSub &&& 0x0F) ||| ((a &&& 0xF0) - (value &&& 0xF0))
                        | _ -> ((initialSub - 6) &&& 0x0F) ||| ((a &&& 0xF0) - (value &&& 0xF0) - 0x10)
                let result = (if (adjustedSub &&& 0x100) <> 0 then (adjustedSub - 0x060) else adjustedSub)
                z <- (binaryResult &&& 0xFF) = 0
                n <- (binaryResult &&& 0x80) <> 0
                c <- binaryResult >= 0
                setV binaryResult value
                result &&& 0xFF
            else
                NZ binaryResult
                c <- binaryResult >= 0
                setV binaryResult value
                binaryResult &&& 0xFF
        a <- aluTemp

    let Adc value =
        let inline setV sum operand =
            v <- (a ^^^ sum) &&& (a ^^^ operand) &&& 0x80 = 0
        aluTemp <-
            let binaryResult = value + a + (if c then 1 else 0)
            if isDecimalMode then
                let initialAdd = (a &&& 0x0F) + (value &&& 0x0F) + (if c then 1 else 0)
                let adjustedAdd = initialAdd + (if initialAdd > 9 then 6 else 0)
                let result = (adjustedAdd &&& 0x0F) + (a &&& 0xF0) + (value &&& 0xF0) + (if adjustedAdd > 0x0F then 0x10 else 0x00)
                z <- (binaryResult &&& 0xFF) = 0
                n <- (result &&& 0x80) <> 0
                setV result value
                let adjustedResult = result + (if result &&& 0x1F0 > 0x090 then 0x060 else 0x000)
                c <- (adjustedResult &&& 0xFF0) > 0x0F0
                adjustedResult &&& 0xFF
            else
                NZ binaryResult
                c <- binaryResult > 0xFF
                setV binaryResult value
                binaryResult &&& 0xFF
        a <- aluTemp

    let Slo value =
        match ((value <<< 1) &&& 0xFF) ||| a, (value &&& 0x80) <> 0 with
            | result, newCarry ->
                c <- newCarry
                aluTemp <- result
                a <- result
                NZ result

    let Isc value =
        Sbc ((value + 1) &&& 0xFF)

    let Dcp value =
        match (value - 1) &&& 0xFF, (value &&& 0x01) <> 0 with
            | cmpValue, newCarry ->
                c <- newCarry
                CmpA cmpValue

    let Sre value =
        match ((value >>> 1) &&& 0xFF) ^^^ a, (value &&& 0x01) <> 0 with
            | result, newCarry ->
                c <- newCarry
                aluTemp <- result
                a <- result
                NZ result

    let Rra value =
        match (value >>> 1) ||| (if c then 0x80 else 0x00), (value &&& 0x01) <> 0 with
            | adcValue, newCarry ->
                c <- newCarry
                Adc adcValue

    let Rla value =
        match (((value <<< 1) &&& 0xFF) ||| (if c then 0x01 else 0x00)) &&& a, (value &&& 0x80) <> 0 with
            | result, newCarry ->
                c <- newCarry
                aluTemp <- result
                a <- result
                NZ result

    let Lsr value =
        match value >>> 1 with
            | result ->
                c <- (value &&& 0x01) <> 0
                aluTemp <- result
                NZ result

    let LsrA () =
        Lsr a
        a <- aluTemp

    let Asl value =
        match ((value <<< 1) &&& 0xFF) with
            | result ->
                c <- (value &&& 0x80) <> 0
                aluTemp <- result
                NZ result

    let AslA () =
        Asl a
        a <- aluTemp

    let Inc value =
        match (value + 1) &&& 0xFF with
            | result ->
                aluTemp <- result
                NZ result

    let Dec value =
        match (value - 1) &&& 0xFF with
            | result ->
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
        match ((value <<< 1) &&& 0xFF) ||| (if c then 0x01 else 0x00), ((value &&& 0x80) <> 0) with
            | result, newCarry ->
                c <- newCarry
                aluTemp <- result
                NZ result

    let RolA () =
        Rol a
        a <- aluTemp

    let Ror value =
        match (value >>> 1) ||| (if c then 0x80 else 0x00), ((value &&& 0x01) <> 0) with
            | result, newCarry ->
                c <- newCarry
                aluTemp <- result
                NZ result

    let RorA () =
        Ror a
        a <- aluTemp

    let Las value =
        match value &&& s with
            | result ->
                s <- result
                x <- result
                a <- result
                NZ result



    // ----- uOPS -----

    let FetchDiscard address operation = ReadMemory address <| (ignore >> operation)
    let FetchDummy operation = FetchDiscard pc <| operation

    let Fetch1RealInternal () =
        match pc with
            | currentPc ->
                opcode <- ReadMemoryInternal currentPc
                branchIrqHack <- false
                mi <- -1
                pc <- (currentPc + 1) &&& 0xFFFF

    let Fetch1Real () =
        IfReady <| Fetch1RealInternal

    let Fetch1Internal () =
        myIFlag <- i
        i <- iFlagPending
        match branchIrqHack, nmi, (irq && (not myIFlag)) with
            | false, true, _ ->
                interruptPending <- false
                ea <- nmiVector
                opcode <- vopNmi
                nmi <- false
                mi <- 0
                restart <- true
            | false, false, true ->
                interruptPending <- false
                ea <- irqVector
                opcode <- vopIrq
                mi <- 0
                restart <- true
            | _ -> Fetch1RealInternal()


    let Fetch1 () =
        IfReady <| Fetch1Internal

    let Fetch2 () = ReadMemoryPcIncrement <| SetOpcode2
    let Fetch3 () = ReadMemoryPcIncrement <| SetOpcode3
    let PushPch () = Push (GetPch()) <| ignore
    let PushPcl () = Push (GetPcl()) <| ignore

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
        PushPInterrupt true brkVector

    let PushPIrq () =
        PushPInterrupt false irqVector
            
    let PushPNmi () =
        PushPInterrupt false nmiVector

    let PushPReset () =
        match PushDummy() with
            | true ->
                b <- false
                i <- true
                ea <- resetVector
                true
            | _ -> false

    let FetchPclVectorInternal () =
        if nmi && ((ea = brkVector && b) || (ea = irqVector && (not b))) then
            nmi <- false
            ea <- nmiVector
        aluTemp <- ReadMemoryInternal ea
        
    let FetchPclVector () =
        IfReady <| FetchPclVectorInternal

    let FetchPchVectorInternal () =
        aluTemp <- aluTemp ||| (ReadMemoryInternal(ea + 1) <<< 8)
        pc <- aluTemp
            
    let FetchPchVector () =
        IfReady <| FetchPchVectorInternal

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
    let ImpSec () = Imp <| fun _ -> c <- true
    let ImpClc () = Imp <| fun _ -> c <- false
    let ImpClv () = Imp <| fun _ -> v <- false
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
            match (ea + y) with
                | result ->
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
            if aluTemp >= 0x100 then
                ea <- (ea + 0x100) &&& 0xFFFF

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
                opcode <- vopRelativeStuff
                mi <- -1)
            
    let RelBranchStage2Bvs () = RelBranchStage2 v
    let RelBranchStage2Bvc () = RelBranchStage2 (not v)
    let RelBranchStage2Bmi () = RelBranchStage2 n
    let RelBranchStage2Bpl () = RelBranchStage2 (not n)
    let RelBranchStage2Bcs () = RelBranchStage2 c
    let RelBranchStage2Bcc () = RelBranchStage2 (not c)
    let RelBranchStage2Beq () = RelBranchStage2 z
    let RelBranchStage2Bne () = RelBranchStage2 (not z)

    let RelBranchStage3 () =
        FetchDummy <| fun _ ->
            match (pc &&& 0xFF) + (if opcode2 < 0x80 then opcode2 else opcode2 - 256) with
                | address ->
                    aluTemp <- address
                    pc <- (pc &&& 0xFF00) ||| (aluTemp &&& 0xFF)
                    if (address &&& 0xFF00) >= 0x100 then
                        opcode <- vopRelativeStuff2
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

    let IdxIndRmwStage7 operation = WriteMemory ea aluTemp operation
    let IdxIndRmwStage7Slo () = IdxIndRmwStage7 <| Slo
    let IdxIndRmwStage7Sre () = IdxIndRmwStage7 <| Sre
    let IdxIndRmwStage7Rra () = IdxIndRmwStage7 <| Rra
    let IdxIndRmwStage7Isc () = IdxIndRmwStage7 <| Isc
    let IdxIndRmwStage7Dcp () = IdxIndRmwStage7 <| Dcp
    let IdxIndRmwStage7Rla () = IdxIndRmwStage7 <| Rla

    let IdxIndRmwStage8 () = WriteMemory ea aluTemp <| ignore

    let PushP () =
        b <- true
        PushDiscard <| GetP()

    let PushA () = PushDiscard a

    let PullPNoInc () =
        ReadMemoryS <| fun mem ->
            myIFlag <- i
            SetP mem
            iFlagPending <- i
            i <- myIFlag

    let PullANoInc () = ReadMemoryS <| fun mem -> a <- mem

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

    let ZpRmwInc () = ZpRmw Inc
    let ZpRmwDec () = ZpRmw Dec
    let ZpRmwAsl () = ZpRmw Asl
    let ZpRmwSre () = ZpRmw Sre
    let ZpRmwRra () = ZpRmw Rra
    let ZpRmwDcp () = ZpRmw Dcp
    let ZpRmwLsr () = ZpRmw Lsr
    let ZpRmwRor () = ZpRmw Ror
    let ZpRmwRol () = ZpRmw Rol
    let ZpRmwSlo () = ZpRmw Slo
    let ZpRmwIsc () = ZpRmw Isc
    let ZpRmwRla () = ZpRmw Rla

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
            if aluTemp >= 0x100 then
                ea <- ea + 0x100
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

    let AbsRmwStage5 operation = WriteMemory ea aluTemp operation
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

    let EndISpecial () =
        opcode <- vopFetch1
        mi <- -1
        restart <- true
        rdy

    let EndSuppressInterrupt () =
        opcode <- vopFetch1NoInterrupt
        mi <- -1
        restart <- true
        rdy

    let End () =
        opcode <- vopFetch1
        mi <- -1
        iFlagPending <- i
        restart <- true
        rdy

    let EndBranchSpecial = End

    let Jam () = false

    let rec ExecuteOneRetry () =


        // ----- Execution -----

        
        if () |>
            match microCode.[opcode].[mi] with
                | Uop.Fetch1 -> Fetch1
                | Uop.Fetch1Real -> Fetch1Real
                | Uop.Fetch2 -> Fetch2
                | Uop.Fetch3 -> Fetch3
                | Uop.FetchDummy -> fun _ -> FetchDummy <| ignore
                | Uop.PushPch -> PushPch
                | Uop.PushPcl -> PushPcl
                | Uop.PushPBrk -> PushPBrk
                | Uop.PushPIrq -> PushPIrq
                | Uop.PushPNmi -> PushPNmi
                | Uop.PushPReset -> PushPReset
                | Uop.PushDummy -> PushDummy
                | Uop.FetchPclVector -> FetchPclVector
                | Uop.FetchPchVector -> FetchPchVector
                | Uop.ImpIny -> ImpIny
                | Uop.ImpDey -> ImpDey
                | Uop.ImpInx -> ImpInx
                | Uop.ImpDex -> ImpDex
                | Uop.NzA -> fun _ -> FetchDummy <| NZA
                | Uop.NzX -> fun _ -> FetchDummy <| NZX
                | Uop.NzY -> fun _ -> FetchDummy <| NZY
                | Uop.ImpTsx -> ImpTsx
                | Uop.ImpTxs -> ImpTxs
                | Uop.ImpTax -> ImpTax
                | Uop.ImpTay -> ImpTay
                | Uop.ImpTya -> ImpTya
                | Uop.ImpTxa -> ImpTxa
                | Uop.ImpSei -> ImpSei
                | Uop.ImpCli -> ImpCli
                | Uop.ImpSec -> ImpSec
                | Uop.ImpClc -> ImpClc
                | Uop.ImpSed -> ImpSed
                | Uop.ImpCld -> ImpCld
                | Uop.ImpClv -> ImpClv
                | Uop.AbsWriteSta -> AbsWriteSta
                | Uop.AbsWriteStx -> AbsWriteStx
                | Uop.AbsWriteSty -> AbsWriteSty
                | Uop.AbsWriteSax -> AbsWriteSax
                | Uop.ZpWriteSta -> ZpWriteSta
                | Uop.ZpWriteSty -> ZpWriteSty
                | Uop.ZpWriteStx -> ZpWriteStx
                | Uop.ZpWriteSax -> ZpWriteSax
                | Uop.IndIdxStage3 -> IndIdxStage3
                | Uop.IndIdxStage4 -> IndIdxStage4
                | Uop.IndIdxWriteStage5 -> IndIdxWriteStage5
                | Uop.IndIdxReadStage5 -> IndIdxReadStage5
                | Uop.IndIdxRmwStage5 -> IndIdxRmwStage5
                | Uop.IndIdxWriteStage6Sta -> IndIdxWriteStage6Sta
                | Uop.IndIdxWriteStage6Sha -> IndIdxWriteStage6Sha
                | Uop.IndIdxReadStage6Lda -> IndIdxReadStage6Lda
                | Uop.IndIdxReadStage6Cmp -> IndIdxReadStage6Cmp
                | Uop.IndIdxReadStage6And -> IndIdxReadStage6And
                | Uop.IndIdxReadStage6Eor -> IndIdxReadStage6Eor
                | Uop.IndIdxReadStage6Lax -> IndIdxReadStage6Lax
                | Uop.IndIdxReadStage6Adc -> IndIdxReadStage6Adc
                | Uop.IndIdxReadStage6Sbc -> IndIdxReadStage6Sbc
                | Uop.IndIdxReadStage6Ora -> IndIdxReadStage6Ora
                | Uop.IndIdxRmwStage6 -> IndIdxRmwStage6
                | Uop.IndIdxRmwStage7Slo -> IndIdxRmwStage7Slo
                | Uop.IndIdxRmwStage7Sre -> IndIdxRmwStage7Sre
                | Uop.IndIdxRmwStage7Rra -> IndIdxRmwStage7Rra
                | Uop.IndIdxRmwStage7Isc -> IndIdxRmwStage7Isc
                | Uop.IndIdxRmwStage7Dcp -> IndIdxRmwStage7Dcp
                | Uop.IndIdxRmwStage7Rla -> IndIdxRmwStage7Rla
                | Uop.IndIdxRmwStage8 -> IndIdxRmwStage8
                | Uop.RelBranchStage2Bvs -> RelBranchStage2Bvs
                | Uop.RelBranchStage2Bvc -> RelBranchStage2Bvc
                | Uop.RelBranchStage2Bmi -> RelBranchStage2Bmi
                | Uop.RelBranchStage2Bpl -> RelBranchStage2Bpl
                | Uop.RelBranchStage2Bcs -> RelBranchStage2Bcs
                | Uop.RelBranchStage2Bcc -> RelBranchStage2Bcc
                | Uop.RelBranchStage2Beq -> RelBranchStage2Beq
                | Uop.RelBranchStage2Bne -> RelBranchStage2Bne
                | Uop.RelBranchStage3 -> RelBranchStage3
                | Uop.RelBranchStage4 -> RelBranchStage4
                | Uop.Nop -> fun _ -> ReadMemoryS <| ignore
                | Uop.IncS -> IncS
                | Uop.Jsr -> Jsr
                | Uop.PullP -> PullP
                | Uop.PullPcl -> PullPcl
                | Uop.PullPchNoInc -> PullPchNoInc
                | Uop.AbsReadLda -> AbsReadLda
                | Uop.AbsReadLdy -> AbsReadLdy
                | Uop.AbsReadLdx -> AbsReadLdx
                | Uop.AbsReadBit -> AbsReadBit
                | Uop.AbsReadLax -> AbsReadLax
                | Uop.AbsReadAnd -> AbsReadAnd
                | Uop.AbsReadEor -> AbsReadEor
                | Uop.AbsReadOra -> AbsReadOra
                | Uop.AbsReadAdc -> AbsReadAdc
                | Uop.AbsReadCmp -> AbsReadCmp
                | Uop.AbsReadCpy -> AbsReadCpy
                | Uop.AbsReadNop -> AbsReadNop
                | Uop.AbsReadCpx -> AbsReadCpx
                | Uop.AbsReadSbc -> AbsReadSbc
                | Uop.ZpIdxStage3X -> ZpIdxStage3X
                | Uop.ZpIdxStage3Y -> ZpIdxStage3Y
                | Uop.ZpIdxRmwStage4 -> ZpIdxRmwStage4
                | Uop.ZpIdxRmwStage6 -> ZpIdxRmwStage6
                | Uop.ZpReadEor -> ZpReadEor
                | Uop.ZpReadBit -> ZpReadBit
                | Uop.ZpReadLda -> ZpReadLda
                | Uop.ZpReadLdy -> ZpReadLdy
                | Uop.ZpReadLdx -> ZpReadLdx
                | Uop.ZpReadLax -> ZpReadLax
                | Uop.ZpReadCpy -> ZpReadCpy
                | Uop.ZpReadCmp -> ZpReadCmp
                | Uop.ZpReadCpx -> ZpReadCpx
                | Uop.ZpReadOra -> ZpReadOra
                | Uop.ZpReadNop -> ZpReadNop
                | Uop.ZpReadSbc -> ZpReadSbc
                | Uop.ZpReadAdc -> ZpReadAdc
                | Uop.ZpReadAnd -> ZpReadAnd
                | Uop.ImmEor -> ImmEor
                | Uop.ImmAnc -> ImmAnc
                | Uop.ImmAsr -> ImmAsr
                | Uop.ImmAxs -> ImmAxs
                | Uop.ImmArr -> ImmArr
                | Uop.ImmLxa -> ImmLxa
                | Uop.ImmOra -> ImmOra
                | Uop.ImmCpy -> ImmCpy
                | Uop.ImmCpx -> ImmCpx
                | Uop.ImmCmp -> ImmCmp
                | Uop.ImmSbc -> ImmSbc
                | Uop.ImmAnd -> ImmAnd
                | Uop.ImmAdc -> ImmAdc
                | Uop.ImmLda -> ImmLda
                | Uop.ImmLdx -> ImmLdx
                | Uop.ImmLdy -> ImmLdy
                | Uop.ImmUnsupported -> ImmUnsupported
                | Uop.IdxIndStage3 -> IdxIndStage3
                | Uop.IdxIndStage4 -> IdxIndStage4
                | Uop.IdxIndStage5 -> IdxIndStage5
                | Uop.IdxIndReadStage6Lda -> IdxIndReadStage6Lda
                | Uop.IdxIndReadStage6Ora -> IdxIndReadStage6Ora
                | Uop.IdxIndReadStage6Lax -> IdxIndReadStage6Lax
                | Uop.IdxIndReadStage6Cmp -> IdxIndReadStage6Cmp
                | Uop.IdxIndReadStage6Adc -> IdxIndReadStage6Adc
                | Uop.IdxIndReadStage6And -> IdxIndReadStage6And
                | Uop.IdxIndReadStage6Eor -> IdxIndReadStage6Eor
                | Uop.IdxIndReadStage6Sbc -> IdxIndReadStage6Sbc
                | Uop.IdxIndWriteStage6Sta -> IdxIndWriteStage6Sta
                | Uop.IdxIndWriteStage6Sax -> IdxIndWriteStage6Sax
                | Uop.IdxIndRmwStage6 -> IdxIndRmwStage6
                | Uop.IdxIndRmwStage7Slo -> IdxIndRmwStage7Slo
                | Uop.IdxIndRmwStage7Isc -> IdxIndRmwStage7Isc
                | Uop.IdxIndRmwStage7Dcp -> IdxIndRmwStage7Dcp
                | Uop.IdxIndRmwStage7Sre -> IdxIndRmwStage7Sre
                | Uop.IdxIndRmwStage7Rra -> IdxIndRmwStage7Rra
                | Uop.IdxIndRmwStage7Rla -> IdxIndRmwStage7Rla
                | Uop.IdxIndRmwStage8 -> IdxIndRmwStage8
                | Uop.PushP -> PushP
                | Uop.PushA -> PushA
                | Uop.PullANoInc -> PullANoInc
                | Uop.PullPNoInc -> PullPNoInc
                | Uop.ImpAslA -> ImpAslA
                | Uop.ImpRolA -> ImpRolA
                | Uop.ImpRorA -> ImpRorA
                | Uop.ImpLsrA -> ImpLsrA
                | Uop.JmpAbs -> JmpAbs
                | Uop.IncPc -> IncPc
                | Uop.ZpRmwStage3 -> ZpRmwStage3
                | Uop.ZpRmwStage5 -> ZpRmwStage5
                | Uop.ZpRmwInc -> ZpRmwInc
                | Uop.ZpRmwDec -> ZpRmwDec
                | Uop.ZpRmwAsl -> ZpRmwAsl
                | Uop.ZpRmwSre -> ZpRmwSre
                | Uop.ZpRmwRra -> ZpRmwRra
                | Uop.ZpRmwDcp -> ZpRmwDcp
                | Uop.ZpRmwLsr -> ZpRmwLsr
                | Uop.ZpRmwRor -> ZpRmwRor
                | Uop.ZpRmwRol -> ZpRmwRol
                | Uop.ZpRmwSlo -> ZpRmwSlo
                | Uop.ZpRmwIsc -> ZpRmwIsc
                | Uop.ZpRmwRla -> ZpRmwRla
                | Uop.AbsIdxStage3X -> AbsIdxStage3X
                | Uop.AbsIdxStage3Y -> AbsIdxStage3Y
                | Uop.AbsIdxReadStage4 -> AbsIdxReadStage4
                | Uop.AbsIdxStage4 -> AbsIdxStage4
                | Uop.AbsIdxWriteStage5Sta -> AbsIdxWriteStage5Sta
                | Uop.AbsIdxWriteStage5Shy -> AbsIdxWriteStage5Shy
                | Uop.AbsIdxWriteStage5Shx -> AbsIdxWriteStage5Shx
                | Uop.AbsIdxWriteStage5Tas -> AbsIdxWriteStage5Tas
                | Uop.AbsIdxRmwStage5 -> AbsIdxRmwStage5
                | Uop.AbsIdxRmwStage7 -> AbsIdxRmwStage7
                | Uop.AbsIdxRmwStage6Dec -> AbsIdxRmwStage6Dec
                | Uop.AbsIdxRmwStage6Dcp -> AbsIdxRmwStage6Dcp
                | Uop.AbsIdxRmwStage6Isc -> AbsIdxRmwStage6Isc
                | Uop.AbsIdxRmwStage6Inc -> AbsIdxRmwStage6Inc
                | Uop.AbsIdxRmwStage6Rol -> AbsIdxRmwStage6Rol
                | Uop.AbsIdxRmwStage6Lsr -> AbsIdxRmwStage6Lsr
                | Uop.AbsIdxRmwStage6Slo -> AbsIdxRmwStage6Slo
                | Uop.AbsIdxRmwStage6Sre -> AbsIdxRmwStage6Sre
                | Uop.AbsIdxRmwStage6Rra -> AbsIdxRmwStage6Rra
                | Uop.AbsIdxRmwStage6Rla -> AbsIdxRmwStage6Rla
                | Uop.AbsIdxRmwStage6Asl -> AbsIdxRmwStage6Asl
                | Uop.AbsIdxRmwStage6Ror -> AbsIdxRmwStage6Ror
                | Uop.AbsIdxReadStage5Lda -> AbsIdxReadStage5Lda
                | Uop.AbsIdxReadStage5Ldx -> AbsIdxReadStage5Ldx
                | Uop.AbsIdxReadStage5Lax -> AbsIdxReadStage5Lax
                | Uop.AbsIdxReadStage5Ldy -> AbsIdxReadStage5Ldy
                | Uop.AbsIdxReadStage5Ora -> AbsIdxReadStage5Ora
                | Uop.AbsIdxReadStage5Nop -> AbsIdxReadStage5Nop
                | Uop.AbsIdxReadStage5Cmp -> AbsIdxReadStage5Cmp
                | Uop.AbsIdxReadStage5Sbc -> AbsIdxReadStage5Sbc
                | Uop.AbsIdxReadStage5Adc -> AbsIdxReadStage5Adc
                | Uop.AbsIdxReadStage5Eor -> AbsIdxReadStage5Eor
                | Uop.AbsIdxReadStage5And -> AbsIdxReadStage5And
                | Uop.AbsIdxReadStage5Las -> AbsIdxReadStage5Las
                | Uop.AbsIndJmpStage4 -> AbsIndJmpStage4
                | Uop.AbsIndJmpStage5 -> AbsIndJmpStage5
                | Uop.AbsRmwStage4 -> AbsRmwStage4
                | Uop.AbsRmwStage5Inc -> AbsRmwStage5Inc
                | Uop.AbsRmwStage5Dec -> AbsRmwStage5Dec
                | Uop.AbsRmwStage5Dcp -> AbsRmwStage5Dcp
                | Uop.AbsRmwStage5Isc -> AbsRmwStage5Isc
                | Uop.AbsRmwStage5Asl -> AbsRmwStage5Asl
                | Uop.AbsRmwStage5Ror -> AbsRmwStage5Ror
                | Uop.AbsRmwStage5Slo -> AbsRmwStage5Slo
                | Uop.AbsRmwStage5Rla -> AbsRmwStage5Rla
                | Uop.AbsRmwStage5Sre -> AbsRmwStage5Sre
                | Uop.AbsRmwStage5Rra -> AbsRmwStage5Rra
                | Uop.AbsRmwStage5Rol -> AbsRmwStage5Rol
                | Uop.AbsRmwStage5Lsr -> AbsRmwStage5Lsr
                | Uop.AbsRmwStage6 -> AbsRmwStage6
                | Uop.EndISpecial -> EndISpecial
                | Uop.EndSuppressInterrupt -> EndSuppressInterrupt
                | Uop.End -> End
                | Uop.EndBranchSpecial -> EndBranchSpecial
                | Uop.JamFFFE -> fun _ -> FetchDiscard 0xFFFE |> ignore; true
                | Uop.JamFFFF -> fun _ -> FetchDiscard 0xFFFF |> ignore; true
                | _ -> fun _ -> FetchDiscard 0xFFFF |> ignore; false
        then
            mi <- mi + 1

        if restart then
            restart <- false
            ExecuteOneRetry()
        else
            totalCycles <- totalCycles + 1UL


    // ----- Interface -----


    member this.Clock () =
        rdy <- readRdy()
        ExecuteOneRetry()

    member this.ClockMultiple count =
        rdy <- readRdy()
        let mutable remaining = count
        while remaining > 0 do
            ExecuteOneRetry()
            remaining <- remaining - 1

    member this.ClockToAddress address =
        rdy <- readRdy()
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
    member this.N = n
    member this.V = v
    member this.B = b
    member this.I = i
    member this.Z = z
    member this.C = c
    member this.D = d

    member this.Sync =
        match microCode.[opcode].[mi] with
            | Uop.Fetch1 | Uop.Fetch1Real | Uop.End | Uop.EndBranchSpecial | Uop.EndISpecial | Uop.EndSuppressInterrupt -> true
            | _ -> false

    member this.SetA value = a <- value &&& 0xFF
    member this.SetX value = x <- value &&& 0xFF
    member this.SetY value = y <- value &&& 0xFF
    member this.SetS value = s <- value &&& 0xFF
    member this.SetPC value = pc <- value &&& 0xFFFF
    member this.SetN value = n <- value
    member this.SetV value = v <- value
    member this.SetB value = b <- value
    member this.SetI value = i <- value; iFlagPending <- value
    member this.SetZ value = z <- value
    member this.SetC value = c <- value
    member this.SetD value = d <- value; isDecimalMode <- d && hasDecimalMode

    member this.SetOpcode value =
        opcode <- value
        branchIrqHack <- false
        mi <- 0
        restart <- false

    member this.ForceOpcodeSync () =
        this.SetOpcode vopFetch1
        
    member this.TotalCycles = totalCycles

