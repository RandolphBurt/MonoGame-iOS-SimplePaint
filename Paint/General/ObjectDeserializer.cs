/// <summary>
/// ObjectDeserializer.cs
/// Randolph Burt - December 2012
/// </summary>
namespace Paint
{
	using System.IO;
	using System.Xml;
	using System.Xml.Serialization;

	/// <summary>
	/// Object deserializer.
	/// Create an object from xml
	/// </summary>
	public static class ObjectDeserializer
	{
		/// <summary>
		/// Deserialises an xml file into an instance of class 'T'.
		/// </summary>
		/// <returns>A newly created instance of class 'T' based on the xml in the given file</returns>
		/// <param name='filename'>The file containing the serialized xml representing the class.</param>
		/// <typeparam name='T'>The 'type' which we need to instantiate from the xml file</typeparam>
		public static T DeserialiseFromXmlFile<T>(string filename) where T : class
		{
			return DeserialiseFromXmlText<T>(File.ReadAllText(filename));
		}
	
		/// <summary>
		/// Deserialises xml text into an instance of class 'T'.
		/// </summary>
		/// <returns>A newly created instance of class 'T' based on the xml in the given string</returns>
		/// <param name='filename'>The file containing the serialized xml representing the class.</param>
		/// <typeparam name='T'>The 'type' which we need to instantiate from the xml string</typeparam>
		public static T DeserialiseFromXmlText<T>(string text) where T : class
		{
			using (StringReader stReader = new StringReader(text))
			{
				XmlTextReader reader = new XmlTextReader(stReader);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				return xmlSerializer.Deserialize(reader) as T;
			}
		}
	}
}

