using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

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
	
	public static int getInt(XmlNode xn) {
		return getVal<int>(xn, intOps);
	}
	
	public static float getFloat(XmlNode xn) {
		return getVal<float>(xn, flOps);
	}
}