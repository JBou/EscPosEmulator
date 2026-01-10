using ReceiptPrinterEmulator.Emulator;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.EscPos.Commands.FS;

public class CancelKanjiCharacterModeCommand : BaseCommandNoArgs
{
    public override string Prefix => EscPosInterpreter.FS + ".";

    public override void Execute(ReceiptPrinter printer, string? args)
    {
        // TODO: Implement Kanji character mode cancellation
        Logger.Info("Cancel Kanji character mode - not yet implemented");
    }
}