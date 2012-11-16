using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

public class UnparsableColorException : Exception
{
    public UnparsableColorException (string message) : base(message)
    {
    }    
}

static class ColorNames
{
    public const string Red = "red";
    public const string Green = "green";
    public const string Blue = "blue";
    public const string Black = "black";
    public const string Cyan = "cyan";
    public const string Magenta = "magenta";
    public const string White = "white";
    public const string Yellow = "yellow";
    public const string Alpha = "alpha";
}



/// <summary>
/// Color Utilities - 
/// 
/// Since the colors in unity use float values 0.0-1.0 for the RGB(A) components
/// and the rest of the world uses hex values 0-FF (0-255) this class
/// provides a standard way for our code to convert between the two.
/// It contains a few extra ease of use routines that will allow 
/// use of a select few color names.
/// </summary>
static class ColorUtilities
{
 
#region Constants 
    // The hex value for the maximum value of a component of a color 
    // used for converting from the normal color values to the 
    // float values used by Unity.
    public const int RGBA_COMPONENT_MAX = 0xff;  // = 255 = 2^8 - 1 = b11111111 = b8_7777 
 
    // For symmetry and readability.
    public const int RGBA_COMPONENT_MIN = 0;  // Duh. 
 
    public const string HTML_HEX_ESCAPE = "#";
    public const string HTML_RGB_ESCAPE = "rgb";
    
    // Note that HTML hex codes have no alpha.
    // Matches "#xxxxxx" where the x's are hex values 00-ff
    public const string HTML_HEX_PATTERN = @"
                         [#]  # The escape
         (?<red>[0-9a-f]{2})  # 2 hex digits for red
       (?<green>[0-9a-f]{2})  # 2 hex digits for green
        (?<blue>[0-9a-f]{2})  # 2 hex digits for blue
    ";
    public static Regex HtmlHexRegex = new Regex (HTML_HEX_PATTERN,
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);
    
    // Matches "rgb(xxx,xxx,xxx)" where the x's are 0-255
    // also will match "rgb(xxx,xxx,xxx,xxx)" which includes
    // the alpha component.
    public const string HTML_RGB_PATTERN = @"
                 rgb[(]    # The escape + left paren
        (?<red>\d{1,3}),   # 3 digits for red
      (?<green>\d{1,3}),   # 3 digits for green
       (?<blue>\d{1,3})    # 3 digits for blue
    (,(?<alpha>\d{1,3}))?  # optional 3 digits for alpha
                     [)]   # right paren
    ";
    public static Regex HtmlRGBRegex = new Regex (HTML_RGB_PATTERN, 
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);
    
    // Open Question: Should we use consts for color names?
    public static readonly Dictionary<string,Color> nameToValueMap = 
        new Dictionary<string, Color>{
            {ColorNames.Red, Color.red},
            {ColorNames.Green, Color.green},
            {ColorNames.Blue, Color.blue},
            {ColorNames.Black, Color.black},
            {ColorNames.Cyan, Color.cyan},
            {ColorNames.Magenta, Color.magenta},
            {ColorNames.White, Color.white},
            {ColorNames.Yellow, Color.yellow},
    };
    public static readonly Dictionary<Color,string> valueToNameMap =
     new Dictionary<Color,string>{
         {Color.red, ColorNames.Red},
         {Color.green, ColorNames.Green},
         {Color.blue, ColorNames.Blue},
         {Color.black, ColorNames.Black},
         {Color.cyan, ColorNames.Cyan},
         {Color.magenta, ColorNames.Magenta},
         {Color.white, ColorNames.White},
         {Color.yellow, ColorNames.Yellow},    
    };
   
#endregion
    
    // Simply convert normal integer value for RGB [0,255] to the unity 
    //desired float [0.0,1.0] by diving it by the maximum integer value.
    public static float IntToUnityFloat (int hex)
    {
        if (hex > RGBA_COMPONENT_MAX)
            Debug.LogError (string.Format (
             "Color component greater than allowed value ({0} < {1})",
             RGBA_COMPONENT_MAX, hex));
             
        if (hex < RGBA_COMPONENT_MIN)
            Debug.LogError (string.Format (
             "Color component less than allowed value ({0} > {1})",
             RGBA_COMPONENT_MIN, hex));
             
        return (float)hex / RGBA_COMPONENT_MAX;
    }
   
    
    
    public static Color FromName (string name)
    {
        return nameToValueMap [name];
    }
 
    public static string GetName (Color color)
    {
        return valueToNameMap [color];
    }
    
#region Parse Functions
  
    // Parse from RGBA component decimal number
    // eventually all other parse functions call this function.
    public static Color Parse (int r, int g, int b, int a)
    {
        float rPrime = IntToUnityFloat (r);
        float gPrime = IntToUnityFloat (g);
        float bPrime = IntToUnityFloat (b);
        float aPrime = IntToUnityFloat (a);
     
        return new Color (rPrime, gPrime, bPrime, aPrime);
    }
  
    
    // Parse html style colors #xxxxxx and rgb(xxx,...)
    public static Color Parse (string colorCode)
    {     
        colorCode = colorCode.ToLower ();
        
        if (colorCode.StartsWith (HTML_HEX_ESCAPE)) {
            return ParseHtmlHex (colorCode);
        } else if (colorCode.StartsWith (HTML_RGB_ESCAPE)) {
            return ParseHtmlRGB (colorCode);
        }     
        throw new UnparsableColorException("Is this in HTML format?: " + colorCode);
        
    }
    
    // Parse from RGB component decimal numbers
    public static Color Parse (int r, int g, int b)
    {
        return Parse (r, g, b, RGBA_COMPONENT_MAX);
    }
    
    // 0-255 integer strings
    public static Color Parse(string r, string g, string b, string a){
        int rPrime = int.Parse(r);
        int gPrime = int.Parse(g);
        int bPrime = int.Parse(b);
        int aPrime = int.Parse (a);
        
        return Parse (rPrime, gPrime, bPrime, aPrime);
        
    }
    
    // Parse from RGB hex string "00"-"FF"
    private static Color ParseHex (string r, string g, string b)
    {
        int rPrime = int.Parse (r, NumberStyles.HexNumber);
        int gPrime = int.Parse (g, NumberStyles.HexNumber);
        int bPrime = int.Parse (g, NumberStyles.HexNumber);
        
        return Parse (rPrime, gPrime, bPrime);
    }
    
    // Get the unity color from the "#XXXXXX" string.
    private static Color ParseHtmlHex (string htmlCode)
    {
        
        Match hexMatch = HtmlHexRegex.Match (htmlCode);       
        string r, g, b = "ff";
        
        
        if (!hexMatch.Groups [ColorNames.Red].Success) {
            throw new UnparsableColorException (
                "Is this in the HTML hex format?: " + htmlCode.ToString ());
        }
        
        r = hexMatch.Groups [ColorNames.Red].Value;
        g = hexMatch.Groups [ColorNames.Green].Value;
        b = hexMatch.Groups [ColorNames.Blue].Value;
        
        return ParseHex (r, g, b);
    }
    
    private static Color ParseHtmlRGB (string htmlCode)
    {
        Match rgbMatch = HtmlRGBRegex.Match (htmlCode);
        string r,g,b,a = "255";
        
        if (!rgbMatch.Groups [ColorNames.Red].Success) {
            throw new UnparsableColorException (
                "Is this in the HTML hex format?: " + htmlCode.ToString ());
        }
       
        r = rgbMatch.Groups [ColorNames.Red].Value;
        g = rgbMatch.Groups [ColorNames.Green].Value;
        b = rgbMatch.Groups [ColorNames.Blue].Value;
        
        if (rgbMatch.Groups [ColorNames.Alpha].Success)
            a = rgbMatch.Groups [ColorNames.Alpha].Value;    
        
        return Parse (r,g,b,a);
    }
    
#endregion
   
}