using UnityEngine;
using System;
using System.Collections;
using System.Xml;

public static class DateData {
	
	public static DateTime GetDate(this XmlNode xn) {
		return DateTime.Parse(xn.getString());
	}
	
	public static DateTime Lerp(this DateTime dt, DateTime other, float delt) {
		return new DateTime(dt.Ticks + ((long)(((float)(other.Ticks - dt.Ticks)) * delt)));
	}
}