using System.Text;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

namespace MiscFunctionsLibrary
{
    ///<summary>Holds information about a single setting.</summary>
    public class XMLSettingItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public XMLSettingItem()
        {
            Name = null;
            Value = null;
        }
    }

    ///<summary>Class managing a list of setting items.</summary>
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
        public void AddNewItem(string itemName, string value)
        {
            XMLSettingItem item = new XMLSettingItem();
            item.Name = itemName;
            item.Value = value;
            this.Items.Add(item);
        }

        ///<summary>Returns the value of a setting based on the first occurrence of the item name. If no item is found with that name it returns null.</summary>
        public string GetItemValue(string itemName)
        {
            foreach (XMLSettingItem item in this.Items)
            {
                if(item.Name == itemName)
                {
                    return item.Value;
                }
            }

            return null;
        }

        ///<summary>Returns the index of a setting based on the first occurrence of the item name. If no item is found with that name it returns -1.</summary>
        public int GetItemIndex(string itemName)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Name == itemName)
                {
                    return i;
                }
            }

            return -1;
        }

        ///<summary>Removes the an item based on the first occurrence of the item name. Returns true if the is successfully removed.</summary>
        public bool RemoveItem(string itemName)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Name == itemName)
                {
                    this.Items.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        ///<summary>Updates an item based on the first occurrence of the item name. Returns true if the is successfully updated or created.
        ///If createIfNotExist is true and no item with the name passed is found, it will add a new item to the setting list.</summary>
        public bool UpdateItem(string itemName, string value, bool createIfNotExist)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Name == itemName)
                {
                    this.Items[i].Value = value;
                    return true;
                }
            }

            if (createIfNotExist)
            {
                AddNewItem(itemName, value);
                return true;
            }

            return false;
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

        ///<summary>Reads a XML settings file and loads its contents into a XMLSettings object.</summary>
        public static XMLSettings LoadConfigXML(string fileName)
        {
            XMLSettings settings = new XMLSettings();

            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(fileName);
                XmlNode node;
                node = myXmlDocument.DocumentElement;

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

            return settings;
        }
    }
}
