using System;
using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Emulator.Enums;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Select character code table (ESC t n)
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=40
/// </summary>
public class SelectCharTableCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "t";
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
        // Find the character code table enum value that matches the code
        // Default to PC437 (0) if not found
        CharacterCodeTable codeTable;
        
        try
        {
            // Check if the code exists in the enum
            if (Enum.IsDefined(typeof(CharacterCodeTable), _n))
            {
                codeTable = (CharacterCodeTable)_n;
            }
            else
            {
                Logger.Warn($"Unsupported character code table: {_n}, defaulting to PC437");
                codeTable = CharacterCodeTable.PC437;
            }
        }
        catch
        {
            Logger.Warn($"Invalid character code table: {_n}, defaulting to PC437");
            codeTable = CharacterCodeTable.PC437;
        }
        
        printer.SelectCharacterCodeTable(codeTable);
    }
}