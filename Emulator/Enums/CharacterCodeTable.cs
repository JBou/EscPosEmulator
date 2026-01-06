using System.Text;

namespace ReceiptPrinterEmulator.Emulator.Enums;

/// <summary>
/// ESC t command - Select character code table
/// </summary>
public enum CharacterCodeTable
{
    PC437 = 0,
    USA = 0,
    STANDARD_EUROPE = 0,
    KATAKANA = 1,
    PC850 = 2,
    MULTILINGUAL = 2,
    PC860 = 3,
    PORTUGUESE = 3,
    PC863 = 4,
    CANADIAN_FRENCH = 4,
    PC865 = 5,
    NORDIC = 5,
    HIRAGANA = 6,
    KANJI_1 = 7,
    KANJI_2 = 8,
    PC851 = 11,
    GREEK_1 = 11,
    PC853 = 12,
    TURKISH_1 = 12,
    PC857 = 13,
    TURKISH_2 = 13,
    PC737 = 14,
    GREEK_2 = 14,
    ISO8859_7 = 15,
    GREEK_3 = 15,
    WPC1252 = 16,
    PC866 = 17,
    CYRILLIC_2 = 17,
    PC852 = 18,
    LATIN_2 = 18,
    PC858 = 19,
    EURO = 19,
    THAI_42 = 20,
    THAI_11 = 21,
    THAI_13 = 22,
    THAI_14 = 23,
    THAI_16 = 24,
    THAI_17 = 25,
    THAI_18 = 26,
    TCVN_3_1 = 30,
    VIETNAMESE_1 = 30,
    TCVN_3_2 = 31,
    VIETNAMESE_2 = 31,
    PC720 = 32,
    ARABIC = 32,
    WPC775 = 33,
    BALTIC_RIM = 33,
    PC855 = 34,
    CYRILLIC = 34,
    PC861 = 35,
    ICELANDIC = 35,
    PC862 = 36,
    HEBREW = 36,
    PC864 = 37,
    ARABIC_2 = 37,
    PC869 = 38,
    GREEK_4 = 38,
    ISO8859_2 = 39,
    ISO8859_15 = 40,
    PC1098 = 41,
    FARSI = 41,
    PC1118 = 42,
    PC1119 = 43,
    PC1125 = 44,
    WPC1250 = 45,
    WPC1251 = 46,
    WPC1253 = 47,
    WPC1254 = 48,
    WPC1255 = 49,
    WPC1256 = 50,
    WPC1257 = 51,
    WPC1258 = 52,
    KZ_1048 = 53,
    DEVANAGARI = 66,
    BENGALI = 67,
    TAMIL = 68,
    TELUGU = 69,
    ASSAMESE = 70,
    ORIYA = 71,
    KANNADA = 72,
    MALAYALAM = 73,
    GUJARATI = 74,
    PUNJABI = 75,
    MARATHI = 82,
    PAGE_254 = 254,
    PAGE_255 = 255
}

/// <summary>
/// Extension methods for CharacterCodeTable enum
/// </summary>
public static class CharacterCodeTableExtensions
{
    /// <summary>
    /// Gets the ESC/POS command code for the character code table
    /// </summary>
    public static int GetCode(this CharacterCodeTable codeTable)
    {
        return (int)codeTable;
    }

    /// <summary>
    /// Gets the .NET code page number for the character code table
    /// </summary>
    public static int GetCodePageNumber(this CharacterCodeTable codeTable) => codeTable switch
    {
        CharacterCodeTable.PC437 or CharacterCodeTable.USA or CharacterCodeTable.STANDARD_EUROPE => 437,
        CharacterCodeTable.KATAKANA => 932,  // Shift-JIS
        CharacterCodeTable.PC850 or CharacterCodeTable.MULTILINGUAL => 850,
        CharacterCodeTable.PC860 or CharacterCodeTable.PORTUGUESE => 860,
        CharacterCodeTable.PC863 or CharacterCodeTable.CANADIAN_FRENCH => 863,
        CharacterCodeTable.PC865 or CharacterCodeTable.NORDIC => 865,
        CharacterCodeTable.PC851 or CharacterCodeTable.GREEK_1 => 851,
        CharacterCodeTable.PC853 or CharacterCodeTable.TURKISH_1 => 853,
        CharacterCodeTable.PC857 or CharacterCodeTable.TURKISH_2 => 857,
        CharacterCodeTable.PC737 or CharacterCodeTable.GREEK_2 => 737,
        CharacterCodeTable.WPC1252 => 1252,
        CharacterCodeTable.PC866 or CharacterCodeTable.CYRILLIC_2 => 866,
        CharacterCodeTable.PC852 or CharacterCodeTable.LATIN_2 => 852,
        CharacterCodeTable.PC858 or CharacterCodeTable.EURO => 858,
        CharacterCodeTable.TCVN_3_1 or CharacterCodeTable.VIETNAMESE_1 => 1258,
        CharacterCodeTable.TCVN_3_2 or CharacterCodeTable.VIETNAMESE_2 => 1258,
        CharacterCodeTable.ISO8859_2 => 28591,
        CharacterCodeTable.ISO8859_15 => 28605,
        CharacterCodeTable.WPC1250 => 1250,
        CharacterCodeTable.WPC1251 => 1251,
        CharacterCodeTable.WPC1253 => 1253,
        CharacterCodeTable.WPC1254 => 1254,
        CharacterCodeTable.WPC1255 => 1255,
        CharacterCodeTable.WPC1256 => 1256,
        CharacterCodeTable.WPC1257 => 1257,
        CharacterCodeTable.WPC1258 => 1258,
        _ => 437  // Default to PC437 for unsupported code pages
    };
}