using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System;



public class XmlUtilities : MonoBehaviour {
	
	//Shared xml tags
	public const string DATA = "data";
	public const string RESOURCE = "resource";
	public const string WIDTH = "width";
	public const string HEIGHT = "height";
    


	private delegate string replace(string data);
	
	private static Dictionary<Regex, replace> replacers;


    void Start(){
     replacers = new Dictionary<Regex, replace>();
     replacers[new Regex("\\\\year")] = Year;
    } 
    
	public static string getData(XmlNode xn) {
		string val = xn.Attributes[DATA].Value;
        
		if (val.StartsWith(@"\")) {
			foreach (Regex replacement in replacers.Keys) {
				val = replacement.Replace(val, replacers[replacement](val));
			}

		}
		 
        return val;
	}


	
	public static IEnumerable<T> getDataFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNodeList xnl = xDoc.SelectNodes(xPath);
		return xnl.Cast<XmlNode>().Select<XmlNode, T>(f);
	}
	
	public static T getDatumFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNode xn = xDoc.SelectSingleNode(xPath);
		return f(xn);
	}

	private string Year(string data) {
		return UserProperty.getProp("year");
	}

}