using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.ESC;

/// <summary>
/// Turn double-strike mode on/off
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=27
/// </summary>
public class ToggleDoubleStrikeCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.ESC + "G";
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
        if (_n is 0)
            printer.SelectDoubleStrikeMode(false);
        else if (_n is 1)
            printer.SelectDoubleStrikeMode(true);
    }
}