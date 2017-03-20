namespace Breadbox

type private SpriteUop =
    | None
    | InvertYExpansionToggle
    | EnableDma
    | EnableDisplay
    | PerformCrunch

type private GraphicsUop =
    | None
    | LoadVcBase
    | IncrementRc

type private FetchUop =
    | None
    | Idle
    | SpritePointer
    | SpriteData
    | Graphics
    | Character
    | Refresh

type private BaUop =
    | None
    | Idle
    | Sprite0
    | Sprite01
    | Sprite012
    | Sprite12
    | Sprite123
    | Sprite23
    | Sprite234
    | Sprite34
    | Sprite345
    | Sprite45
    | Sprite456
    | Sprite56
    | Sprite567
    | Sprite67
    | Sprite7
    | Character

type Mos6567Configuration (cyclesPerRasterLine, rasterLinesPerFrame) =
    member val CyclesPerRasterLine = cyclesPerRasterLine
    member val RasterLinesPerFrame = rasterLinesPerFrame
    member val PixelsPerRasterLine = cyclesPerRasterLine * 8

[<Struct>]
type SpriteOutput =
    val Color:int
    val Output:bool
    val Priority:bool
    new (color, output, priority) = {
        Color = color;
        Output = output;
        Priority = priority
    }

[<Struct>]
type SpriteShiftOutput =
    val ShiftRegisterEnable:bool
    val MultiColorToggle:bool
    val XExpansionToggle:bool
    val ShiftRegisterData:int
    new (sre, mct, xet, sr) = {
        ShiftRegisterEnable = sre;
        MultiColorToggle = mct;
        XExpansionToggle = xet;
        ShiftRegisterData = sr
    }

[<Struct>]
type GraphicsOutput =
    val Color:int
    val Foreground:bool
    new (color, foreground) = {
        Color = color;
        Foreground = foreground
    }

[<Struct>]
type GraphicsShiftOutput =
    val MultiColorToggle:bool
    val ShiftRegisterData:int
    new (mct, sr) = {
        MultiColorToggle = mct;
        ShiftRegisterData = sr
    }

[<Struct>]
type ClockedRaster =
    val Counter:int
    val Y:int
    val X:int
    new (counter, y, x) = {
        Counter = counter;
        Y = y;
        X = x
    }

[<Struct>]
type MuxSpriteOutput =
    val Color:int
    val Output:bool
    val Priority:bool
    val SpriteCollisions:int
    val DataCollisions:int
    new (color, output, priority, spriteCollisions, dataCollisions) = {
        Color = color;
        Output = output;
        Priority = priority;
        SpriteCollisions = spriteCollisions;
        DataCollisions = dataCollisions;
    }
    new (sprite:SpriteOutput, spriteCollisions, dataCollisions) = {
        Color = sprite.Color;
        Output = sprite.Output;
        Priority = sprite.Priority;
        SpriteCollisions = spriteCollisions;
        DataCollisions = dataCollisions;
    }

[<Struct>]
type MuxOutput =
    val Color:int
    val SpriteCollisions:int
    val DataCollisions:int
    new (color, spriteCollisions, dataCollisions) = {
        Color = color;
        SpriteCollisions = spriteCollisions;
        DataCollisions = dataCollisions;
    }

[<Struct>]
type Sprites =
    val S0:SpriteOutput
    val S1:SpriteOutput
    val S2:SpriteOutput
    val S3:SpriteOutput
    val S4:SpriteOutput
    val S5:SpriteOutput
    val S6:SpriteOutput
    val S7:SpriteOutput
    new (s0, s1, s2, s3, s4, s5, s6, s7) = {
        S0 = s0;
        S1 = s1;
        S2 = s2;
        S3 = s3;
        S4 = s4;
        S5 = s5;
        S6 = s6;
        S7 = s7;
    }


type Mos6567Chip (config:Mos6567Configuration) =

    // Interface
    let ReadMemory address:int = 0
    
    // Timing Information
    let CyclesPerRasterLine = config.CyclesPerRasterLine
    let RasterLinesPerFrame = config.RasterLinesPerFrame
    let PixelsPerRasterLine = config.PixelsPerRasterLine

    // Determines phase transitions based on raster counter
    let IsFirstPhaseEdge x =
        (x &&& 0x7) = 4
    let IsSecondPhaseEdge x =
        (x &&& 0x7) = 0
    let IsPhaseEdge x =
        (x &&& 0x3) = 0

    // Determines raster counter for system-agnostic cycle (PAL 55 = old NTSC 56 = new NTSC 57)
    let RasterCounterForPalCycle cycle =
        match cycle with
            | c when c >= 1 && c <= 54 -> (((c - 1) <<< 3) + (PixelsPerRasterLine - 0x064)) % PixelsPerRasterLine
            | c when c >= 55 && c <= 63 -> (((c - 55) <<< 3) + (PixelsPerRasterLine - 0x0AC)) % PixelsPerRasterLine
            | _ -> raise (System.Exception("Cycle must be in the range 1-63."))

    // Determines raster counter for fetch index (0 = PAL 55, first BA)
    let RasterCounterForFetchIndex index =
        let indexPixel = (index % (CyclesPerRasterLine <<< 1)) <<< 2
        let start = RasterCounterForPalCycle 55
        (start + indexPixel) % PixelsPerRasterLine

    // Determines fetch index for raster counter
    let FetchIndexForRasterCounter counter =
        match counter with
            | c when not (IsPhaseEdge c) ->
                -1
            | c ->
                match (((PixelsPerRasterLine + c) - RasterCounterForPalCycle(55)) % PixelsPerRasterLine) >>> 2 with
                    | i when i >= 0 && i <= 126 -> i
                    | _ -> -1

    // Next value for raster counter
    let NextRasterCounter counter =
        match counter with
            | c when c = PixelsPerRasterLine - 1 -> 0x000
            | c -> c + 1

    // X coordinate for raster counter
    let RasterCounterX =
        let freeze = max 0 (PixelsPerRasterLine - 0x200)
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match x with
                    | x when x < 0x18C -> x
                    | x when x < 0x18C + freeze -> 0x18C
                    | _ -> x - freeze

    // Fetches for raster counter
    let RasterCounterFetch =
        let fetchOps = [|
            (if CyclesPerRasterLine < 64 then FetchUop.Graphics else FetchUop.Idle);
            FetchUop.Idle;
            FetchUop.Idle;
            FetchUop.Idle;
            FetchUop.Idle;
            FetchUop.Idle;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.SpritePointer; FetchUop.SpriteData; FetchUop.SpriteData; FetchUop.SpriteData;
            FetchUop.Refresh; FetchUop.Idle; FetchUop.Refresh; FetchUop.Idle;
            FetchUop.Refresh; FetchUop.Idle; FetchUop.Refresh; FetchUop.Idle; FetchUop.Refresh;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
            FetchUop.Character; FetchUop.Graphics; FetchUop.Character; FetchUop.Graphics;
        |]
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match (FetchIndexForRasterCounter x), (IsPhaseEdge x) with
                    | _, false -> FetchUop.None
                    | -1, true -> FetchUop.Idle
                    | c, _ -> fetchOps.[c]

    // Sprite index for fetches above
    let RasterCounterFetchSprite =
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match (FetchIndexForRasterCounter x), (IsPhaseEdge x) with
                    | c, true when c >= 6 && c < 38 -> (c - 6) >>> 2
                    | _ -> -1

    // BA checks for raster counter
    let RasterCounterBa =
        let baOps = [|
            BaUop.Sprite0; BaUop.Sprite0; BaUop.Sprite01; BaUop.Sprite01;
            BaUop.Sprite012; BaUop.Sprite12; BaUop.Sprite123; BaUop.Sprite23;
            BaUop.Sprite234; BaUop.Sprite34; BaUop.Sprite345; BaUop.Sprite45;
            BaUop.Sprite456; BaUop.Sprite56; BaUop.Sprite567; BaUop.Sprite67;
            BaUop.Sprite67; BaUop.Sprite7; BaUop.Sprite7; BaUop.Idle;
            BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character;
            BaUop.Character; BaUop.Character; BaUop.Character; BaUop.Character
        |]
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match (FetchIndexForRasterCounter x), (IsSecondPhaseEdge x) with
                    | _, false -> BaUop.None
                    | -1, true -> BaUop.Idle
                    | c, _ -> baOps.[c >>> 1]

    // Sprite ops for raster counter
    let SpriteUops =
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match x with
                    | x when x = RasterCounterForPalCycle 16 -> SpriteUop.PerformCrunch
                    | x when x = RasterCounterForPalCycle 55 -> SpriteUop.InvertYExpansionToggle
                    | x when x = RasterCounterForPalCycle 56 -> SpriteUop.EnableDma
                    | x when x = RasterCounterForPalCycle 58 -> SpriteUop.EnableDisplay
                    | _ -> SpriteUop.None

    // Graphics ops for raster counter
    let GraphicsUops =
        Array.init <|
            PixelsPerRasterLine <|
            fun x ->
                match x with
                    | x when x = RasterCounterForPalCycle 14 -> GraphicsUop.LoadVcBase
                    | x when x = RasterCounterForPalCycle 58 -> GraphicsUop.IncrementRc
                    | _ -> GraphicsUop.None

    // In cycle 14, Mc <- f(Mc)
    let SpriteMcAdd = [|
        0x03; 0x04; 0x05; 0x06;
        0x07; 0x08; 0x09; 0x0A;
        0x0B; 0x0C; 0x0D; 0x0E;
        0x0F; 0x10; 0x11; 0x12;
        0x13; 0x14; 0x15; 0x16;
        0x17; 0x18; 0x19; 0x1A;
        0x1B; 0x1C; 0x1D; 0x1E;
        0x1F; 0x20; 0x21; 0x22;
        0x23; 0x24; 0x25; 0x26;
        0x27; 0x28; 0x29; 0x2A;
        0x2B; 0x2C; 0x2D; 0x2E;
        0x2F; 0x30; 0x31; 0x32;
        0x33; 0x34; 0x35; 0x36;
        0x37; 0x38; 0x39; 0x3A;
        0x3B; 0x3C; 0x3D; 0x3E;
        0x3F; 0x00; 0x01; 0x3F;
    |]

    // If Y-flag and Y-expand are both off in cycle 16, McBase <- f(McBase)
    let SpriteMcBaseCrunch = [|
        0x01; 0x05; 0x05; 0x07;
        0x05; 0x05; 0x05; 0x07;
        0x09; 0x0D; 0x0D; 0x0F;
        0x0D; 0x15; 0x15; 0x17;
        0x11; 0x15; 0x15; 0x17;
        0x15; 0x15; 0x15; 0x17;
        0x19; 0x1D; 0x1D; 0x1F;
        0x1D; 0x15; 0x15; 0x17;
        0x21; 0x25; 0x25; 0x27;
        0x25; 0x25; 0x25; 0x27;
        0x29; 0x2D; 0x2D; 0x2F;
        0x2D; 0x35; 0x35; 0x37;
        0x31; 0x35; 0x35; 0x37;
        0x35; 0x35; 0x35; 0x37;
        0x39; 0x3D; 0x3D; 0x3F;
        0x3D; 0x15; 0x15; 0x3F;
    |]









    // Determine I fetch address.
    let FetchIAddress =
        0x3FFF

    // Determine P fetch address. VM must be pre-shifted.
    let FetchPAddress vm index =
        vm ||| 0x03F8 ||| index

    // Determine S fetch address. MP must be pre-shifted.
    let FetchSAddress mp mc =
        mc ||| mp

    // Determine R fetch address. REF must be pre-truncated.
    let FetchRAddress ref =
        0x3F00 ||| ref

    // Determine G fetch address. CB must be pre-shifted.
    let FetchGAddress idle ecm bmm vc cb c rc =
        (if idle then 0x3FFF elif bmm then ((cb &&& 0x2000) ||| (vc <<< 3) ||| rc) else (cb ||| ((c &&& 0xFF) <<< 3) ||| rc)) &&& (if ecm then 0x39FF else 0x3FFF)

    // Determine C fetch address. VM must be pre-shifted.
    let FetchCAddress vm vc =
        (vm ||| vc)
    
    // Determine fetch operation. (operation:int, index:int)
    let Fetch cycle =
        match RasterCounterFetch.[cycle] with
            | FetchUop.None           -> FetchUop.None, 0
            | FetchUop.Idle           -> FetchUop.Idle, 0
            | FetchUop.Graphics       -> FetchUop.Graphics, 0
            | FetchUop.Character      -> FetchUop.Character, 0
            | FetchUop.SpriteData     -> FetchUop.SpriteData, RasterCounterFetchSprite.[cycle]
            | FetchUop.SpritePointer  -> FetchUop.SpritePointer, RasterCounterFetchSprite.[cycle]
            | FetchUop.Refresh        -> FetchUop.Refresh, 0
    
    // Determine graphics output color and data [000] (color:int, foreground:bool)
    let RawGraphicsOutputStandardTextMode b0c color sr =
        match (sr &&& 0x80) with
            | 0x80 -> new GraphicsOutput(color >>> 8, true)
            | _    -> new GraphicsOutput(b0c, false)

    // Determine graphics output color and data [001] (color:int, foreground:bool)
    let RawGraphicsOutputMulticolorTextMode b0c b1c b2c color sr =
        match (sr &&& 0xC0), (color &&& 0x800) <> 0 with
            | 0x40         , true                     -> new GraphicsOutput(b1c, false)
            | 0x80         , true                     -> new GraphicsOutput(b2c, true)
            | 0x80         , _
            | 0xC0         , _                        -> new GraphicsOutput((color >>> 8) &&& 0x7, true)
            | _                                       -> new GraphicsOutput(b0c, false)

    // Determine graphics output color and data [010] (color:int, foreground:bool)
    let RawGraphicsOutputStandardBitmapMode color sr =
        match (sr &&& 0x80) with
            | 0x80            -> new GraphicsOutput((color >>> 4) &&& 0xF, true)
            | _               -> new GraphicsOutput(color &&& 0xF, false)

    // Determine graphics output color and data [011] (color:int, foreground:bool)
    let RawGraphicsOutputMulticolorBitmapMode b0c color sr =
        match (sr &&& 0xC0) with
            | 0x40            -> new GraphicsOutput((color >>> 4) &&& 0xF, false)
            | 0x80            -> new GraphicsOutput(color &&& 0xF, true)
            | 0xC0            -> new GraphicsOutput((color >>> 8), true)
            | _               -> new GraphicsOutput(b0c, false)

    // Determine graphics output color and data [100] (color:int, foreground:bool)
    let RawGraphicsOutputExtraColorMode b0c b1c b2c b3c color sr =
        match (sr &&& 0x80), (color &&& 0xC0) with
            | 0x80         , _                  -> new GraphicsOutput((color >>> 8), true)
            | _            , 0x40               -> new GraphicsOutput(b1c, false)
            | _            , 0x80               -> new GraphicsOutput(b2c, false)
            | _            , 0xC0               -> new GraphicsOutput(b3c, false)
            | _                                 -> new GraphicsOutput(b0c, false)

    // Determine graphics output color and data [101] (color:int, foreground:bool)
    let RawGraphicsOutputInvalidExtraColorMode sr =
        new GraphicsOutput(0, sr &&& 0x80 <> 0)

    // Determine graphics output color and data (color:int, foreground:bool)
    let RawGraphicsOutput ecm bmm mcm b0c b1c b2c b3c color sr =
        match ecm,   bmm,   mcm   with
            | false, false, false   -> RawGraphicsOutputStandardTextMode b0c color sr
            | false, false, true    -> RawGraphicsOutputMulticolorTextMode b0c b1c b2c color sr
            | false, true,  false   -> RawGraphicsOutputStandardBitmapMode color sr
            | false, true,  true    -> RawGraphicsOutputMulticolorBitmapMode b0c color sr
            | true,  false, false   -> RawGraphicsOutputExtraColorMode b0c b1c b2c b3c color sr
            | _                     -> RawGraphicsOutputInvalidExtraColorMode sr

    // Determine sprite output color and data (color:int, output:bool, priority:bool)
    let RawSpriteOutput mmc0 mmc1 color multicolor sr dp disp =
        match disp with
            | false -> 0, false, dp
            | _ ->
                match sr &&& 0x800000, multicolor, sr &&& 0xC00000 with
                    | 0x800000       , false     , _
                    | _              , true      , 0x800000          -> color, true, dp
                    | _              , true      , 0x400000          -> mmc0, true, dp
                    | _              , true      , 0xC00000          -> mmc1, true, dp
                    | _                                              -> 0, false, dp

    // Determine which sprites are outputting (register:int)
    let RawMuxSprites (sprites:Sprites) =
        (if sprites.S0.Output then 0x01 else 0x00) |||
        (if sprites.S1.Output then 0x02 else 0x00) |||
        (if sprites.S2.Output then 0x04 else 0x00) |||
        (if sprites.S3.Output then 0x08 else 0x00) |||
        (if sprites.S4.Output then 0x10 else 0x00) |||
        (if sprites.S5.Output then 0x20 else 0x00) |||
        (if sprites.S6.Output then 0x40 else 0x00) |||
        (if sprites.S7.Output then 0x80 else 0x00)

    // Determine shifted graphics state (mcToggle:bool, sr:int)
    let ClockedGraphics bmm mcm mct c sr =
        match bmm  , mcm  , mct  , (c &&& 0x800 <> 0) with
            | _    , false, _    , _                    
            | false, true , _    , false                -> new GraphicsShiftOutput(true, sr <<< 1)
            | _    , _    , true , _                    -> new GraphicsShiftOutput(false, sr)
            | _                                         -> new GraphicsShiftOutput(true, sr <<< 2)

    // Determine shifted sprite state
    let ClockedSprite (rasterx:int) x sre disp sr mc mct xe xet =
        if (not disp) || (not (sre || (rasterx = x))) then
            new SpriteShiftOutput(false, true, true, sr)
        else
            match mc, xe, mct, xet with
                | false, false, _    , _
                | false, true , _    , false -> new SpriteShiftOutput(true, true, true, sr <<< 1)
                | true , false, false, _
                | true , true , false, false -> new SpriteShiftOutput(true, true, true, sr <<< 2)
                | false, true , _    , true
                | _    , true , false, true  -> new SpriteShiftOutput(true, true, false, sr)
                | _    , false, _    , _
                | _    , _    , _    , true  -> new SpriteShiftOutput(true, false, true, sr)
                | _                          -> new SpriteShiftOutput(true, false, false, sr)

    // Determine clocked raster position. (counterX:int, rasterY:int, rasterX:int)
    let ClockedRaster rasterCounter rasterY =
        match (rasterCounter + 1) with
            | newCounter when newCounter >= PixelsPerRasterLine ->
                match (rasterY + 1) with
                    | newRasterY when newRasterY >= RasterLinesPerFrame -> new ClockedRaster(0, 0, 0)
                    | newRasterY -> new ClockedRaster(0, newRasterY, 0)
            | newCounter -> new ClockedRaster(newCounter, rasterY, RasterCounterX.[newCounter])

    // Determine frontmost sprite to render (color:int, output:bool, priority:bool)
    let MuxSpritesForeground (sprites:Sprites) =
        if (sprites.S0.Output) then (sprites.S0)
        elif (sprites.S1.Output) then (sprites.S1)
        elif (sprites.S2.Output) then (sprites.S2)
        elif (sprites.S3.Output) then (sprites.S3)
        elif (sprites.S4.Output) then (sprites.S4)
        elif (sprites.S5.Output) then (sprites.S5)
        elif (sprites.S6.Output) then (sprites.S6)
        elif (sprites.S7.Output) then (sprites.S7)
        else new SpriteOutput(0, false, true)

    // Determine sprite-sprite collision register result
    let MuxSpriteSpriteCollision rawmux =
        match rawmux with
            | 0x00 | 0x01 | 0x02 | 0x04 | 0x08 | 0x10 | 0x20 | 0x40 | 0x80 -> 0x00
            | _ -> rawmux

    // Determine sprite-data collision register result
    let MuxSpriteBackgroundCollision (g:GraphicsOutput) rawmux =
        if not g.Foreground then 0x00 else rawmux
    
    // Determine output sprite color, data, priority and collision
    let MuxSprites graphicsOutput sprites =
        match MuxSpritesForeground sprites, RawMuxSprites sprites with
            | spriteOutput, rawmux ->
                new MuxSpriteOutput(spriteOutput, MuxSpriteSpriteCollision rawmux, MuxSpriteBackgroundCollision graphicsOutput rawmux)
    
    // Determine output color from mux
    let MuxOutputColor graphicsOutput muxSpriteOutput =
        match graphicsOutput, muxSpriteOutput with
            | _, {MuxSpriteOutput.Output = false}
            | {GraphicsOutput.Foreground = true}, {MuxSpriteOutput.Priority = true} -> graphicsOutput.Color
            | _ -> muxSpriteOutput.Color

    // Determine graphics unit output (color:int, spriteCollisions:int, dataCollisions:int)
    let Mux sprites ec vborder ecm bmm mcm b0c b1c b2c b3c gc gsr =
        match vborder with
            | true -> new MuxOutput(ec, 0x00, 0x00)
            | _ ->
                match RawGraphicsOutput ecm bmm mcm b0c b1c b2c b3c gc gsr with
                    | graphicsOutput ->
                        match MuxSprites graphicsOutput sprites with
                            | muxSpriteOutput ->
                                new MuxOutput(MuxOutputColor graphicsOutput muxSpriteOutput, muxSpriteOutput.SpriteCollisions, muxSpriteOutput.DataCollisions)

    // Determine video output (color:int, spriteCollisions:int, dataCollisions:int)
    let Output sprites ec vborder mborder ecm bmm mcm b0c b1c b2c b3c gc gsr =
        match mborder with 
            | true -> new MuxOutput(ec, 0x00, 0x00)
            | _ -> Mux sprites ec vborder ecm bmm mcm b0c b1c b2c b3c gc gsr

    member this.TestFetchIAddress () = FetchIAddress
    member this.TestFetchRAddress ref = FetchRAddress ref
    member this.TestFetchGAddress idle ecm bmm vc cb c rc = FetchGAddress idle ecm bmm vc cb c rc
    member this.TestFetchCAddress vm vc = FetchCAddress vm vc
    member this.TestFetchPAddress vm index = FetchPAddress vm index
    member this.TestFetchSAddress mp mc = FetchSAddress mp mc

    member this.TestRawGraphicsOutputStandardTextMode b0c color sr = RawGraphicsOutputStandardTextMode b0c color sr
    member this.TestRawGraphicsOutputMulticolorTextMode b0c b1c b2c color sr = RawGraphicsOutputMulticolorTextMode b0c b1c b2c color sr
    member this.TestRawGraphicsOutputStandardBitmapMode color sr = RawGraphicsOutputStandardBitmapMode color sr
    member this.TestRawGraphicsOutputMulticolorBitmapMode b0c color sr = RawGraphicsOutputMulticolorBitmapMode b0c color sr
    member this.TestRawGraphicsOutputExtraColorMode b0c b1c b2c b3c color sr = RawGraphicsOutputExtraColorMode b0c b1c b2c b3c color sr
    member this.TestRawGraphicsOutputInvalidExtraColorMode sr = RawGraphicsOutputInvalidExtraColorMode sr

    member this.TestRawSpriteOutput mmc0 mmc1 color multicolor sr dp disp = RawSpriteOutput mmc0 mmc1 color multicolor sr dp disp
    member this.TestRawMuxSprites sprites = RawMuxSprites sprites
    member this.TestClockedGraphics bmm mcm mct c sr = ClockedGraphics bmm mcm mct c sr
    member this.TestClockedSprite rasterx x sre disp sr mc mct xe xet = ClockedSprite rasterx x sre disp sr mc mct xe xet
    member this.TestMuxSpritesForeground sprites = MuxSpritesForeground sprites
    member this.TestMuxSpriteSpriteCollision rawmux = MuxSpriteSpriteCollision rawmux
    member this.TestMuxSpriteBackgroundCollision g rawmux = MuxSpriteBackgroundCollision g rawmux
    member this.TestMuxSprites graphicsOutput sprites = MuxSprites graphicsOutput sprites
    member this.TestMux sprites ec vborder ecm bmm mcm b0c b1c b2c b3c gc gsr = Mux sprites ec vborder ecm bmm mcm b0c b1c b2c b3c gc gsr
    member this.TestOutput sprites ec vborder mborder ecm bmm mcm b0c b1c b2c b3c gc gsr = Output sprites ec vborder mborder ecm bmm mcm b0c b1c b2c b3c gc gsr
    member this.TestClockedRaster rasterCounter rasterY = ClockedRaster rasterCounter rasterY
    member this.TestMuxOutputColor graphicsOutput muxSpriteOutput = MuxOutputColor graphicsOutput muxSpriteOutput
