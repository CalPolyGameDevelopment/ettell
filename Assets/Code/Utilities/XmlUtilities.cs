using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System;
using System.Reflection;

/// <summary>
/// Transformations -
/// 
/// Transform/Replacement Methods for XmlUtilities. 
/// </summary>
public static class XmlTransforms{
    
    // "\year" -> UserProp "year"
    public static Regex YearRegex = new Regex("\\\\year");

    public static string Year() {
        return UserProperty.getProp("year");
    }
    
    
    // "\rgb(xxx,xxx,xxx)" -> Color
    public static Regex ColorRegex = new Regex(
        @"\\" + ColorUtilities.HTML_RGB_PATTERN, 
        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

    public static Color ColorFromString(string val){
        string colorCode = val.Replace(@"\", "");
        return ColorUtilities.Parse(colorCode);       
    }

}

public class XmlUtilities : MonoBehaviour {
	
	//Shared xml tags
	public const string DATA = "data";
	public const string RESOURCE = "resource";
	public const string WIDTH = "width";
	public const string HEIGHT = "height";
    
    public const int ONE_PARAMETER = 1;
    public const int NO_PARAMTERS = 0;
    
	private static Dictionary<Regex, string> replacers = new Dictionary<Regex, string>{
        {XmlTransforms.YearRegex, "Year"},
        {XmlTransforms.ColorRegex, "ColorFromString"},
   
    };
    
	void Start() {
	     
    }
	
    
	public static TResult getData<TResult>(XmlNode xn) {
		string val = xn.Attributes[DATA].Value;
        
        // Start off with the generic object. 
        // If you have "TResult data" it encodes a ton of 
        // extra assumptions about the type. So use the base object
        // type for now to get around those.
        object data = null; 
        
        if (typeof(TResult) == typeof(String)){
            data = val;
        }
        
        object[] methodParams;
		if (val.StartsWith(@"\")) {
			foreach (Regex replacement in replacers.Keys) {
                // Does the pattern match?
                if (!replacement.Match(val).Success){
                    // No, so skip it.
                    continue;
                }
                
                // Call by reflection code so we don't have to be tied down to a delegate 
                // call signature.
                
                // Get the name of the method that we want to call for
                // the successfully matched Regex.
				string methodName = replacers[replacement];
                
                // Obtain the method object from the transformations class.
                MethodInfo method = typeof(XmlTransforms).GetMethod(methodName);
                
                methodParams = new object[method.GetParameters().Length];
                
                // For now assume that the replacer will either be a
                // get style (no arguments, returns some userprop value) 
                // or will take 1 argument (the data string from the XML) 
                // and parse it in some way.
                if(methodParams.Length == ONE_PARAMETER){
                    
                   // add the original data string to the method parameters
                   methodParams[0] = val;
                
                } else if (methodParams.Length != NO_PARAMTERS){

                    Debug.LogError(
                        "Assertion that an XML replacement method should only "+
                        "have 1 or 0 arguments was violated.");
                }
                
                // Call the method. The first agrument is typically the object on 
                // which you wish to invoke the method. Since we are using static 
                // class/methods there is no object instance, ergo we use null.
                data = method.Invoke(null, methodParams);
                
                // Assume only one replacement type. Since we found a replacement 
                // stop. 
                break;
            }
		}
		 
        // Make sure to cast to the desired return type.
        return (TResult)data;

	}
 
     
    
	public static float[] getPosition(XmlNode position) {
		return getData<string>(position).Split(',').Select(x => float.Parse(x)).ToArray();
	}
	
	public static IEnumerable<T> getDataFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNodeList xnl = xDoc.SelectNodes(xPath);
		return xnl.Cast<XmlNode>().Select<XmlNode, T>(f);
	}
	
	public static T getDatumFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNode xn = xDoc.SelectSingleNode(xPath);
		return f(xn);
	}
	

}