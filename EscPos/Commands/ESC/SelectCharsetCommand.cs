using System;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select an international character set (ESC R n)
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=22
/// </summary>
public class SelectCharsetCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "R";
    public override bool HasArgs => true;
    
    private int _n;

    public override void Reset()
    {
        _n = 0;
    }
    
    public override bool InterpretNextChar(char c)
    {
        _n = c;
        return false;
    }

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        // Find the character set enum value that matches the code
        // Default to USA (0) if not found
        CharacterSet charSet;
        
        try
        {
            charSet = (CharacterSet)_n;
        }
        catch
        {
            Logger.Warn($"Unsupported character set code: {_n}, defaulting to USA");
            charSet = CharacterSet.USA;
        }
        
        printer.SelectCharacterSet(charSet);
    }
}