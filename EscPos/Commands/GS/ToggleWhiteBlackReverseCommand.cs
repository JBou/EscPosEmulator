using ReceiptPrinterEmulator.Emulator;

namespace ReceiptPrinterEmulator.EscPos.Commands.GS;

/// <summary>
/// Turn white/black reverse printing mode on/off
/// https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=53
/// </summary>
public class ToggleWhiteBlackReverseCommand : BaseCommand
{
    public override string Prefix => EscPosInterpreter.GS + "B";
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
            printer.SelectWhiteBlackReverseMode(false);
        else if (_n is 1)
            printer.SelectWhiteBlackReverseMode(true);
    }
}