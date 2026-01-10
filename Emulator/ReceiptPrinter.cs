using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using ReceiptPrinterEmulator.Emulator.Enums;
using ReceiptPrinterEmulator.EscPos;
using ReceiptPrinterEmulator.Logging;

namespace ReceiptPrinterEmulator.Emulator;

public class ReceiptPrinter
{
    private readonly PaperConfiguration _paperConfiguration;
    private readonly EscPosInterpreter _escPosInterpreter;
    
    private PrintMode _printMode;
    private int _lineSpacing;
    private int _tabSpacing;
    
    public Receipt CurrentReceipt { get; private set; }
    public List<Receipt> ReceiptStack { get; private set; }

    public event EventHandler<EventArgs> OnActivityEvent;

    public ReceiptPrinter(PaperConfiguration paperConfiguration)
    {
        _paperConfiguration = paperConfiguration;
        _escPosInterpreter = new(this);

        _printMode = new PrintMode();

        ReceiptStack = new();

        StartNewReceipt();
        
        PowerCycle();
    }

    #region ESC/POS

    public void FeedEscPos(string data)
    {
        if (data.Length > 10000)
        {
            File.WriteAllText("last_ticket.txt", data, Encoding.Latin1);
        }
        File.WriteAllText("last_escpos_receive.txt", data, Encoding.Latin1);

        try
        {
            Logger.Info($"Received: {data} (CodePage: {_printMode.CharacterCodeTable.GetCodePageNumber()})");
            _escPosInterpreter.Interpret(data);
        }
        catch (Exception ex)
        {
            Logger.Exception(ex, "ESC/POS Interpreter Error");
        }

        OnActivityEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Converts a Latin1 string (which preserves original byte values 0-255) to a decoded string using the current codepage
    /// </summary>
    public string GetDecodedFromLatin1(string latin1Text)
    {
        var codePage = _printMode.CharacterCodeTable.GetCodePageNumber();
        
        // Convert Latin1 string back to bytes (preserves byte values 0-255)
        var bytes = Encoding.Latin1.GetBytes(latin1Text);
        
        // Decode using the target codepage (e.g., PC858 for German)
        var targetEncoding = Encoding.GetEncoding(codePage);
        return targetEncoding.GetString(bytes);
    }

    #endregion

    #region Receipt meta

    public void StartNewReceipt()
    {
        CurrentReceipt = new(_paperConfiguration, _printMode, _lineSpacing);
        ReceiptStack.Add(CurrentReceipt);
        
        Logger.Info($"Starting new receipt (#{ReceiptStack.Count})");
    }

    #endregion

    #region Emulated

    public void PowerCycle()
    {
        Initialize();
    }

    #endregion

    #region Direct API

    public void Initialize()
    {
        _escPosInterpreter.ClearBuffers();
    
        SelectFont(PrinterFont.FontA);
        SelectJustification(TextJustification.Left);
        SelectCharacterSize(1, 1);
        SelectEmphasizeMode(false);
        SelectDoubleStrikeMode(false);
        SelectItalicMode(false);
        SelectUnderlineMode(UnderlineMode.Off);
        SelectCharacterSet(CharacterSet.USA);
        SelectCharacterCodeTable(CharacterCodeTable.PC437);
        SetDefaultLineSpacing();
        SetDefaultTabSpacing();
        SelectWhiteBlackReverseMode(false);
    }

    public void PrintText(string text)
    {
        Logger.Info($"Print: {text}");
        
        CurrentReceipt.PrintText(text,_printMode);
    }

    public void Cut(CutFunction cutFunction = CutFunction.Cut, CutShape cutShape = CutShape.Full, int n = 0)
    {
        Logger.Info($"Execute cut: {cutFunction}, {cutShape}, {n}");
        
        LineFeed();
        
        // TODO Support alternate cut modes
        
        StartNewReceipt();
    }

    /// <summary>
    /// Feeds one line, based on the current line spacing.
    /// </summary>
    /// <remarks>
    /// - The amount of paper fed per line is based on the value set using the line spacing command (ESC 2 or ESC 3).
    /// </remarks>
    public void LineFeed()
    {
        Logger.Info($"Line feed");
        CurrentReceipt.AdvanceToNewLine();
    }

    public void SelectFont(PrinterFont printerFont)
    {
        Logger.Info($"Select font: {printerFont}");
        
        _printMode.Font = printerFont;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectJustification(TextJustification justification)
    {
        Logger.Info($"Select justification: {justification}");

        _printMode.Justification = justification;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectCharacterSize(int width, int height)
    {
        Logger.Info($"Set character size scale: x{width} width, x{height} height");

        _printMode.CharWidthScale = width;
        _printMode.CharHeightScale = height;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectEmphasizeMode(bool enable)
    {
        Logger.Info($"Set emphasize mode: {enable}");

        _printMode.Emphasize = enable;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectDoubleStrikeMode(bool enable)
    {
        Logger.Info($"Set double strike mode: {enable}");

        _printMode.DoubleStrike = enable;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectWhiteBlackReverseMode(bool enable)
    {
        Logger.Info($"Set white/black reverse mode: {enable}");

        _printMode.WhiteBlackReverse = enable;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectItalicMode(bool enable)
    {
        Logger.Info($"Set italic mode: {enable}");

        _printMode.Italic = enable;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SelectUnderlineMode(UnderlineMode mode)
    {
        Logger.Info($"Set underline mode: {mode}");

        _printMode.Underline = mode;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }

    public void SetLineSpacing(int value)
    {
        Logger.Info($"Set line spacing: {value}");

        _lineSpacing = value;
        CurrentReceipt.SetLineSpacing(_lineSpacing);
    }

    public void SetTabSpacing(int value)
    {
        Logger.Info($"Set tab spacing: {value}");

        _tabSpacing = value;
        CurrentReceipt.SetTabSpacing(_tabSpacing);
    }

    public void SetDefaultLineSpacing() => SetLineSpacing(_paperConfiguration.DefaultLineSpacing);
    public void SetDefaultTabSpacing() => SetTabSpacing(_paperConfiguration.DefaultTabSpacing);

    public void PrintBitmap(Bitmap bitmap)
    {
        Logger.Info($"Print bitmap: {bitmap.Width}x{bitmap.Height}");
        
        CurrentReceipt.PrintBitmap(bitmap);
    }

    #endregion

    #region Command API

    /// <summary>
    /// Prints the data in the print buffer and feeds one line, based on the current line spacing.
    /// </summary>
    public void PrintAndLineFeed(string printBuffer)
    {
        PrintText(printBuffer);
        LineFeed();
    }

    public void PrintTab()
    {
    		string tabs = "";
    		
    		for (var i = 0; i < _tabSpacing; i++) tabs += " ";
        PrintText(tabs);
    }

    /// <summary>
    /// Selects a character set using ESC R command
    /// </summary>
    public void SelectCharacterSet(CharacterSet charSet)
    {
        Logger.Info($"Select character set: {charSet}");
        
        _printMode.CharacterSet = charSet;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }
    
    /// <summary>
    /// Selects a character code table using ESC t command
    /// </summary>
    public void SelectCharacterCodeTable(CharacterCodeTable codeTable)
    {
        Logger.Info($"Select character code table: {codeTable}");
        
        _printMode.CharacterCodeTable = codeTable;
        CurrentReceipt.ChangeFontConfiguration(_printMode);
    }
    
    #endregion
}