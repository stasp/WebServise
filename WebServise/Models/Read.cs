using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using System.Linq;
using System.Web;
using System.Xml;

namespace WebServise.Models
{
    public class Read
    {
        public List<DataValue> ListValue = new List<DataValue>();

        public bool readFile(string mode, string[] fileContents)
        {
            try
            {
                string code = string.Empty;
                DateTime date = DateTime.MinValue;
                double rate = 0;
                string TextFile = string.Empty;
                if (mode == "1")
                {
                    DataValue valueXML = new DataValue();
                    foreach (string items in fileContents)
                    {
                         TextFile = System.IO.File.ReadAllText(items);
                        if (TextFile.Contains("xml"))
                        {
                            using (XmlReader reader = XmlReader.Create(new StringReader(TextFile)))
                            {
                                while (reader.Read())
                                {
                                    if (reader.Name != "xml")
                                    {
                                        int countAtribute = reader.AttributeCount;
                                        for (int i = 0; i < countAtribute; i++)
                                        {
                                            string value = reader.GetAttribute(i);
                                            if ( DateTime.TryParseExact(value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture,  System.Globalization.DateTimeStyles.None, out date))  //Int32.TryParse(value, out date))
                                            {
                                                valueXML.date = date;
                                            }
                                            else if (Double.TryParse(value.Replace('.', ','), out rate))
                                            {
                                                valueXML.rate = rate;
                                            }
                                            else
                                            {
                                                code = value;
                                                valueXML.code = value;
                                            }
                                            if (valueXML.code != null && valueXML.rate != 0 && valueXML.date != DateTime.MinValue)
                                            {
                                                ListValue.Add(valueXML);
                                                valueXML = new DataValue() { code = code };
                                            }
                                        }
                                    }
                                }
                            }
                            return true;
                        }
                    }
                    return true;
                }
                else if(mode == "2")
                {
                    foreach (var items in fileContents)
                    {
                        TextFile = System.IO.File.ReadAllText(items);
                        if (!TextFile.Contains("xml"))
                        {
                            using (TextFieldParser parser = new TextFieldParser(items))
                            {
                                parser.TextFieldType = FieldType.Delimited;
                                parser.SetDelimiters(",");
                                while (!parser.EndOfData)
                                {
                                    string[] fields = parser.ReadFields();
                                    DataValue valueCSV = new DataValue();
                                    foreach (string field in fields)
                                    {
                                        if (DateTime.TryParseExact(field, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                                        {
                                            valueCSV.date = date;
                                        }
                                        else if (Double.TryParse(field.Replace('.', ','), out rate))
                                        {
                                            valueCSV.rate = rate;
                                        }
                                        else
                                        {
                                            valueCSV.code = field;
                                        }
                                    }
                                    ListValue.Add(valueCSV);
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}