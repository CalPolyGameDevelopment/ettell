using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class MathData {
 
    public static float GetFloat(XmlNode node){
        string data = XmlUtilities.getData(node);
        return float.Parse(data);
    }
    
    public static int GetInt(XmlNode node){
        string data =XmlUtilities.getData(node);
        return int.Parse(data);
    }
    
    public static Vector3 GetVector(XmlNode node){
        string data = XmlUtilities.getData(node);
        return VectorUtilities.Parse(data);
    }
    
    
    
}

public class UnparsableVectorException: Exception{
    public UnparsableVectorException(string message) : base(message){}
}

static class VectorUtilities{
    public static string VECTOR3_PATTERN = @"
        vector[(]
        (?<x> [-]? \d+([.]\d+)? )[,]
        (?<y> [-]? \d+([.]\d+)? )[,]
        (?<z> [-]? \d+([.]\d+)? )
        [)]
        ";
    public static string WHITESPACE_PATTERN = @"\s";
    
    public const string X = "x";
    public const string Y = "y";
    public const string Z = "z";
    
    static Regex Vector3Regex = new Regex(VECTOR3_PATTERN,
        RegexOptions.IgnoreCase | 
        RegexOptions.IgnorePatternWhitespace);
    static Regex WhitespaceRegex = new Regex(WHITESPACE_PATTERN);
    
    public static Vector3 Parse(string data){
        data = WhitespaceRegex.Replace (data, string.Empty);
        Match match = Vector3Regex.Match(data);
        if (!match.Success){
            throw new UnparsableVectorException(
                string.Format("Cannot parse the string {0} into a vector.", data));
        }
        
        float x = float.Parse(match.Groups[X].Value);
        float y = float.Parse(match.Groups[Y].Value);
        float z = float.Parse(match.Groups[Z].Value);
     
        Vector3 vector = new Vector3(x, y, z);
        
        return vector;
    }
    
}