using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;


public class MathData {
	private const string MATH = "math";
	private const string VALUE = "value";
	private const string ADD = "add";
	private const string SUBTRACT = "subtract";
	private const string MULTIPLY = "multiply";
	private const string DIVIDE = "divide";
	
	//There is no numeric superclass in C# for some reason
	//The fact that all numeric types have identical Parse functions is not enforced by type hierarchy
	private interface IMathOps<T> {
		T Parse(XmlNode xn);
		T Add(T a, T b);
		T Subtract(T a, T b);
		T Multiply(T a, T b);
		T Divide(T a, T b);
	}
	
	private sealed class FlOps : IMathOps<float> {
		public float Parse(XmlNode xn) {
			return float.Parse(XmlUtilities.getData(xn));
		}
		public float Add(float a, float b) {
			return a + b;
		}
		public float Subtract(float a, float b) {
			return a - b;
		}
		public float Multiply(float a, float b) {
			return a * b;
		}
		public float Divide(float a, float b) {
			return a / b;
		}
	}
	
	private sealed class IntOps : IMathOps<int> {
		public int Parse(XmlNode xn) {
			return int.Parse(XmlUtilities.getData(xn));
		}
		public int Add(int a, int b) {
			return a + b;
		}
		public int Subtract(int a, int b) {
			return a - b;
		}
		public int Multiply(int a, int b) {
			return a * b;
		}
		public int Divide(int a, int b) {
			return a / b;
		}
	}
	
	private static FlOps flOps = new FlOps();
	private static IntOps intOps = new IntOps();
	
	private static XmlNode getMathNode(XmlNode xn) {
		if (xn.Attributes[MATH] != null) {
			return StoryController.findById(xn.Attributes[MATH].Value);
		}
		return null;
	}
	
	private static T aggreggateChildren<T>(XmlNode xn, System.Func<T, T, T> f, IMathOps<T> conversions) {
		return xn.ChildNodes.Cast<XmlNode>().Select<XmlNode, T>(child => processMathNode<T>(child, conversions)).Aggregate(f);
	}
    
	
	private static T processMathNode<T>(XmlNode xn, IMathOps<T> conversions) {
		if (xn.Name == VALUE) {
			return conversions.Parse(xn);
		}
		if (xn.Name == ADD) {
			return aggreggateChildren<T>(xn, conversions.Add, conversions);
		}
		if (xn.Name == SUBTRACT) {
			return aggreggateChildren<T>(xn, conversions.Subtract, conversions);
		}
		if (xn.Name == MULTIPLY) {
			return aggreggateChildren<T>(xn, conversions.Multiply, conversions);
		}
		if (xn.Name == DIVIDE) {
			return aggreggateChildren<T>(xn, conversions.Divide, conversions);
		}
		return default(T);
	}
	
	private static T getVal<T>(XmlNode xn, IMathOps<T> conversions) {
		XmlNode mathNode = getMathNode(xn);
		if (mathNode == null) {
			return conversions.Parse(xn);
		}
		return processMathNode<T>(mathNode, conversions);
	}
	
	public static int GetInt(XmlNode node) {
		return getVal<int>(node, intOps);
	}
	
	public static float GetFloat(XmlNode node) {
		return getVal<float>(node, flOps);
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