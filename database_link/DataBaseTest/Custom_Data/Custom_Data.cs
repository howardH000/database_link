using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data;
namespace Custom_Data
{
    public class Custom_Data
    {
        XmlDocument doc;
        bool Encrpt = false;
        string DataBaseConntection = "";
        public Custom_Data(string dataBaseconntection, string Table_Name)
        {
            DataBaseConntection = dataBaseconntection;
          
            if (!File.Exists(DataBaseConntection))
            {

                doc = new XmlDocument();
                //  XmlNode xn = doc.SelectSingleNode(Table_Name);
                XmlElement Table = doc.CreateElement(Table_Name);            
                doc.AppendChild(Table);            
                if (Encrpt == false)
                    doc.Save(DataBaseConntection);
                else
                    String_Password.Encrypt.EncryptedText(DataBaseConntection, doc.InnerXml);
            }
            else
            {
                doc = new XmlDocument();
                if (Encrpt == false)
                {
                    doc.Load(DataBaseConntection);
                }
                else
                {
                    doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
                }         
            }
        }
        public Custom_Data(string dataBaseconntection, string Table_Name, bool encrpt)
        {
            DataBaseConntection = dataBaseconntection;
            Encrpt = encrpt;
            if (!File.Exists(DataBaseConntection))
            {

                doc = new XmlDocument();
                //  XmlNode xn = doc.SelectSingleNode(Table_Name);
                XmlElement Table = doc.CreateElement(Table_Name);
                /* Table.SetAttribute("Number","H999999999");
                 XmlElement IDCard = doc.CreateElement("IDCard");
                 IDCard.InnerText = "H999999999";
                 XmlElement Name = doc.CreateElement("Name");
                 Name.InnerText = "哈哈";
                 XmlElement ID_Enable = doc.CreateElement("Enable");
                 ID_Enable.InnerText = "true";
                 Table.AppendChild(IDCard);
                 Table.AppendChild(Name);
                 Table.AppendChild(ID_Enable);*/
                doc.AppendChild(Table);
                //  xn.AppendChild(Table);
                if (Encrpt == false)
                    doc.Save(DataBaseConntection);
                else
                    String_Password.Encrypt.EncryptedText(DataBaseConntection,doc.InnerXml);
            }
            else
            {
                doc = new XmlDocument();
                if (Encrpt == false)
                {
                    doc.Load(DataBaseConntection);
                   
                }
                else
                {

                    doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
                  
                }
            
             //   Stream stream = new MemoryStream();

             //   string bbb = "";
             //   using (StreamReader a = new StreamReader(DataBaseConntection))
             //   {
             //       bbb = a.ReadToEnd();

            //    }
            
            //    XmlDocument New_doc = new XmlDocument();
          //      New_doc.InnerXml = bbb;
            }
        }
        public void DecryptXML()
        {
            doc.Save("DecrptDataBase");
        }
        bool Check_File = false;
        public void Insert_Data(string Table_Name,string Node_Name ,string[] Column_Name,string[] Value)
        {
            doc = new XmlDocument();
            if (Encrpt == false)
            {
                doc.Load(DataBaseConntection);
                
            }
            else
            {

                doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
                
            }
            try
            {
                XmlNode xn = doc.SelectSingleNode(Table_Name);
                XmlElement Table = doc.CreateElement(Node_Name);
                // Table.SetAttribute("Number", Value[0]);
                XmlElement[] xmlelement = new XmlElement[Column_Name.Length];

                for (int i = 0; i < xmlelement.Length; i++)
                {
                    xmlelement[i] = doc.CreateElement(Column_Name[i]);
                    xmlelement[i].InnerText = Value[i];

                }

                for (int i = 0; i < xmlelement.Length; i++)
                {

                    Table.AppendChild(xmlelement[i]);
                }



                // doc.AppendChild(Table);
                xn.AppendChild(Table);

            }
            catch (Exception ex)
            {
                throw ex;
            }
                if (Encrpt == false)
                    doc.Save(DataBaseConntection);
                else
                    String_Password.Encrypt.EncryptedText(DataBaseConntection, doc.InnerXml);
    
            
        }
        public void Remove_Data(string Table_Name, string Node_Name, string SearchColumnName, string Value)
        {

            doc = new XmlDocument();
            if (Encrpt == false)
                doc.Load(DataBaseConntection);
            else
                doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
            XmlNode node = doc.SelectSingleNode(Table_Name);
            XmlNodeList Node = doc.SelectNodes(Table_Name + @"/" + Node_Name);
            foreach (XmlNode xn in Node)
                  // foreach (XmlNode xn in node.ChildNodes)
                   {
                       try
                       {
                           if (xn[SearchColumnName].InnerText == Value)
                           {

                               node.RemoveChild(xn);
                           }
                       }
                       catch (Exception ex)
                       {
                           throw ex;
                       }
                   }
               
            if (Encrpt == false)
                doc.Save(DataBaseConntection);
            else
                String_Password.Encrypt.EncryptedText(DataBaseConntection, doc.InnerXml);
            //      doc.Load("DataBase.xml");
        }
        public string[] Read_Data(string Table_Name, string Node_Name, string SearchColumnName, string SearchValue, string[] DisplayColumnName)
        {
            doc = new XmlDocument();
            if (Encrpt == false)
                doc.Load(DataBaseConntection);
            else
                doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
         //   XmlNode node = doc.SelectSingleNode(Table_Name);
            XmlNodeList Node = doc.SelectNodes(Table_Name + @"/" + Node_Name);
            string[] read_data = new string[0];

          
               // foreach (XmlNode xn in node.ChildNodes)
            foreach (XmlNode xn in Node)
                {
                    try
                    {
                        if (xn[SearchColumnName].InnerText == SearchValue)
                        {
                            read_data = new string[xn.ChildNodes.Count];

                            for (int i = 0; i < read_data.Length; i++)
                            {
                                read_data[i] = xn[DisplayColumnName[i]].InnerText;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                
            }
            return read_data;
        }
        public DataSet Read_Data(string Table_Name, string Node_Name, string[] DisplayColumnName)
        {
            doc = new XmlDocument();
            if (Encrpt == false)
                doc.Load(DataBaseConntection);
            else
                doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
           
            DataSet read_data = new DataSet();
            DataTable datatable = new DataTable();
            datatable.TableName = Table_Name;
            for (int i = 0; i<DisplayColumnName.Length;i++ )
                datatable.Columns.Add(DisplayColumnName[i]);
            XmlNodeList Node = doc.SelectNodes(Table_Name + @"/" + Node_Name);
             //   foreach (XmlNode Node in  node.SelectSingleNode(Node_Name))
                {
          //  XmlNode Node = node.SelectSingleNode(Node_Name);
          //  XmlNodeList NodeList = Node.ChildNodes;
         //   foreach (XmlNode xn in NodeList)
                  foreach (XmlNode xn in Node)
                  {
                      try
                      {

                          //  read_data = new string[xn.ChildNodes.Count];


                          DataRow row = datatable.NewRow();
                          for (int j = 0; j < DisplayColumnName.Length; j++)
                          {
                    
                                // XmlNode stu1 = xn.LastChild;
                           //   XmlElement xe1 = (XmlElement)stu1;
                             // if (xe1 != null)
                              {
                                //  row[DisplayColumnName[j]] = stu1.InnerText;
                              }
                                      row[DisplayColumnName[j]] = xn[DisplayColumnName[j]].InnerText;

                          }
                          datatable.Rows.Add(row);
                      }




                      catch (Exception ex)
                      {
                          throw ex;
                      }
                  }
                }
            read_data.Tables.Add(datatable);
            return read_data;
        }
        public void Updata_Data(string Table_Name, string Node_Name, string SearchColumnName, string SearchValue, string[] ModifyColumnName, string[] ModifyValue)
        {
            doc = new XmlDocument();
            if (Encrpt == false)
                doc.Load(DataBaseConntection);
            else
                doc.InnerXml = String_Password.Encrypt.DecryptText(DataBaseConntection);
           // XmlNode node = doc.SelectSingleNode(Table_Name);

            XmlNodeList Node = doc.SelectNodes(Table_Name + @"/" + Node_Name);
            foreach (XmlNode xn in Node)
              //  foreach (XmlNode xn in node.ChildNodes)
                {
                    try
                    {
                        if (xn[SearchColumnName].InnerText == SearchValue)
                        {

                            for (int i = 0; i < ModifyValue.Length; i++)
                            {
                                xn[ModifyColumnName[i]].InnerText = ModifyValue[i];
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            

            if (Encrpt == false)
                doc.Save(DataBaseConntection);
            else
                String_Password.Encrypt.EncryptedText(DataBaseConntection, doc.InnerXml);
            //  doc.Load("DataBase.xml");

        }
    }
}
