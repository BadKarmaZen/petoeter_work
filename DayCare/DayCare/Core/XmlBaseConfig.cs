using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayCare.Core
{ 
	[Serializable]
	public class XmlBaseConfig<ClassType>
		where ClassType : class, new()
	{
		public static ClassType LoadFromFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				StreamReader fileReader = new StreamReader(fileName);
				try
				{
					XmlSerializer xmlReader = new XmlSerializer(typeof(ClassType));
					return (ClassType)xmlReader.Deserialize(fileReader);
				}
				catch (Exception error)
				{
					throw new InvalidCastException("Unable to create xml object from file", error);
				}
				finally
				{
					fileReader.Close();
				}
			}

			//  Maak een leeg address book
			return new ClassType();
		}

		public static void SaveToFile(string fileName, XmlBaseConfig<ClassType> data)
		{
			StreamWriter fileWriter = new StreamWriter(fileName);
			try
			{
				XmlSerializer xmlWriter = new XmlSerializer(typeof(ClassType));
				xmlWriter.Serialize(fileWriter, data);
			}
			catch (Exception error)
			{
				throw new InvalidCastException("Unable to create xml file from object", error);
			}
			finally
			{
				fileWriter.Close();
			}
		}
	}
}
