using System.Text;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

namespace MiscFunctionsLibrary
{
    ///<summary>Holds information about a single setting.</summary>
    public struct XMLSettingItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class XMLSettings
    {
        ///<summary>List of setting items.</summary>
        public List<XMLSettingItem> Items { get; set; }

        ///<summary>Creates a new XMLSettings object.</summary>
        public XMLSettings()
        {
            Items = new List<XMLSettingItem>();
        }

        ///<summary>Adds a new item to the setting items list.</summary>
        public void AddNewItem(string name, string value)
        {
            XMLSettingItem item = new XMLSettingItem();
            item.Name = name;
            item.Value = value;
            this.Items.Add(item);
        }

        ///<summary>Returns the value of a setting based on the first occurrence of the item name.</summary>
        public string GetItemValue(string itemName)
        {
            string value = null;

            foreach (XMLSettingItem item in this.Items)
            {
                if(item.Name == itemName)
                {
                    return item.Value;
                }
            }

            return value;
        }
    }

    public class XMLHandler
    {
        ///<summary>Writes a XMLSettings object to a file overwriting any existing file.</summary>
        public static void WriteConfigXML(string fileName, XMLSettings settings)
        {
            
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("Config");

            foreach(XMLSettingItem si in settings.Items)
            {
                writer.WriteStartElement(si.Name);
                writer.WriteString(si.Value);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        ///<summary>Reads a XML file and loads its contents into a XMLSettings object.</summary>
        public static XMLSettings LoadConfigXML(string fileName)
        {
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(fileName);
                XmlNode node;
                node = myXmlDocument.DocumentElement;
                XMLSettings settings = new XMLSettings();

                foreach (XmlNode node2 in node.ChildNodes)//config node
                {
                    XMLSettingItem item = new XMLSettingItem();
                    item.Name = node2.Name;
                    item.Value = node2.InnerText;
                    settings.Items.Add(item);
                }

                myXmlDocument.Save(fileName);
                return settings;
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }
    }
}
