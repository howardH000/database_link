using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer;
//using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Data.OleDb;
using System.Data.Odbc;
using MySql.Data.MySqlClient;
using Oracle.DataAccess.Client;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
namespace Database_Link
{
   public class DatabaseSQL
      {
       ///<summary>
       ///
       /// 摘要:
       ///資料庫SQL    
       ///
       ///
       /// </summary>
       ///<param name="Connecct_String">資料庫連接字串</param>
       public DatabaseSQL(string Connecct_String)
       {
           SQL_Connecct_String = Connecct_String;
           dataBaseConnection = new SqlConnection(Connecct_String);
        
       }
       ///<summary>
       /// 摘要:
       ///資料庫SQL    
       /// </summary>
       public DatabaseSQL()
       {
           dataBaseConnection = new SqlConnection(SQL_Connecct_String);
       }
       public static SqlConnection dataBaseConnection = new SqlConnection(SQL_Connecct_String);
        //private static string SQL_Connecct_String = @"Provider=SQLOLEDB;Data Source=howardH/howardH000;Initial Catalog=master;";
      //  private static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=howardH/howardH000;Pwd=441342087;";
       private static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        /// <param name="Connecct_String">資料庫連接字串</param>
       public static void Link (string Connecct_String)
        {
            SQL_Connecct_String = Connecct_String;
            dataBaseConnection = new SqlConnection(SQL_Connecct_String);
        }
       ///<summary>
       /// 摘要:
       ///資料庫SQL    
       /// </summary>
       public static void Link()
       {
           dataBaseConnection = new SqlConnection(SQL_Connecct_String);
       }

       ///<summary>
       ///
       /// 摘要:
       ///新增資料庫語法     
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Column_Name">要新增的欄位名稱</param>
       ///<param name="Value">要新增的值</param>
       ///
       public static void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType) 
        {
            //dataBaseConnection = new SqlConnection(SQL_Connecct_String);

            string Insert_str = @"insert into [";
            Insert_str += Table_Name;
            Insert_str += "] (";
            for (int i = 0; i < Column_Name.Length; i++)
            {
                Insert_str += "[" + Column_Name[i].Trim() + "]";
                if (i != Column_Name.Length - 1)
                    Insert_str += ",";
            }
            Insert_str += ") values (";
            for (int i = 0; i < Value.Length; i++)
            {
                Insert_str += "@Value" + i.ToString();
                //         SQL_Insert_str += "@"+Value[i];
                if (i != Value.Length - 1)
                    Insert_str += ",";
            }
            Insert_str += ")";
            SqlCommand Inser_Command = new SqlCommand(Insert_str, dataBaseConnection);
            try
            {
                Inser_Command= TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                Inser_Command.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        ///<summary>
        ///
        /// 摘要:
        ///資料庫比對欄位值，以第一個欄位為主的比對，回傳為比對欄位名稱比對後的布林值
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Column_Name">要比對的欄位名稱，如果第一個false，不會在往下比對皆為false</param>
        ///<param name="Value">要比對的值</param>
        ///
        public static bool[] Compare(string Table_Name, string[] Column_Name, string[] Value)  //In the first than to the main
        {
            bool[] Compare_Result = new bool[Column_Name.Length];
            if (Compare_Result.Length > 0)
            {
                DataSet Compare_DataSet = new DataSet();
                SqlDataAdapter Adapter;
                DataTable Table = new DataTable();
                string Select_String = @"select * from ";
                Select_String += Table_Name;
                Select_String += " where ";
                Select_String += "["+Column_Name[0] +"]"+ "=" ;
                Select_String +="'"+Value[0]+"'";
                try
                {
                    dataBaseConnection.Open();
                    Adapter = new SqlDataAdapter(Select_String, dataBaseConnection);
                    Adapter.Fill(Compare_DataSet, Table_Name);
                    dataBaseConnection.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
                if (Compare_DataSet.Tables[0].Rows.Count >= 1)
                {
                    Compare_Result[0] = true;
                    for (int i = 1; i < Column_Name.Length; i++)
                    {
                        string aa = (string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString();
                        string bb = aa;
                        if ((string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString() == Value[i])
                        {
                            Compare_Result[i] = true;
                        }
                    }
                }
            }
            return  Compare_Result;
        }
        ///<summary>
        ///
        /// 摘要:
        ///更新資料庫語法   
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
        ///<param name="Search_value">要搜尋的值</param>
        ///<param name="Modify_Column_Name">要修改的欄位名稱</param>
        ///<param name="Modify_Value">要修改的值</param>
        ///    
       public static void Updata(string Table_Name,string[] Search_Column_Name ,string[] Search_value,string[] Modify_Column_Name, string[] Modify_Value)
        {
            string Updata_str = "UPDATE ";
            Updata_str += Table_Name;
            Updata_str += " SET ";
            for (int i = 0; i < Modify_Column_Name.Length; i++)
            {
                Updata_str += Modify_Column_Name[i].Trim();
                Updata_str += "=";
                Updata_str += "'" + Modify_Value[i].Trim() + "'";
             
                if (i != Modify_Column_Name.Length - 1)
                    Updata_str += ",";

            }
            Updata_str += " WHERE ";
                for(int i=0;i<Search_Column_Name.Length;i++)
                {
                    Updata_str += Search_Column_Name[i].Trim();
                    Updata_str += "=";
                    Updata_str += "'" + Search_value[i].Trim() + "'";
                    if (i != Search_Column_Name.Length - 1)
                        Updata_str += ",";
                }
                
            SqlCommand Updata_Command = new SqlCommand(Updata_str, dataBaseConnection);
            try
            {
                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                Updata_Command.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
       ///<summary>
       ///
       /// 摘要:
       ///查詢資料庫語法 ，回傳為查詢欄位名稱之欄位值
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
       ///<param name="value">要搜尋的值</param>
       ///<param name="Column_Name">要查詢的欄位名稱</param>
       ///    
       
       public static string[] SQL_Select_Data(string Table_Name, string Search_Column_Name,string Value, string[] Column_Name )  //In the first than to the main
        {
            string[] Data_Result =new string[Column_Name.Length];
       
            
                DataSet Select_DataSet = new DataSet();
                SqlDataAdapter SQL_Adapter;
                DataTable SQL_Table = new DataTable();
                string SQL_Select_String = @"select * from ";
                SQL_Select_String += Table_Name;
                SQL_Select_String += " Where ";
                SQL_Select_String += Search_Column_Name.Trim() + "=";
                SQL_Select_String += "'" + Value.Trim() + "'";
               // dataBaseConnection.Open();
                SQL_Adapter = new SqlDataAdapter(SQL_Select_String, dataBaseConnection);
                SQL_Adapter.Fill(Select_DataSet, Table_Name);
               // dataBaseConnection.Close();
                if (Select_DataSet.Tables[0].Rows.Count == 1)
                {
                    for(int i=0;i<Column_Name.Length;i++)
                        Data_Result[i] = (string)Select_DataSet.Tables[0].Rows[0][Column_Name[i]];
                }

                return Data_Result;
        }
       static SqlCommand TransType(SqlCommand Cmd, string[] Value, DatabaseType[] Type)
       {
            
           for (int i = 0; i < Value.Length; i++)
           {
               object TransValue;
               SqlDbType databaseType;
               switch(Type[i])
               {
                   case DatabaseType.Int:
                      
                        TransValue = Convert.ToInt32(Value[i]);
                       databaseType=SqlDbType.Int;
                       break;
                 
                   case DatabaseType.Double:
                       TransValue = Convert.ToDouble(Value[i]);
                       databaseType=SqlDbType.Float;
                       break;
                   case DatabaseType.DateTime:
                       TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                       databaseType=SqlDbType.DateTime;
                      // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                    //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                       break;
                   case DatabaseType.Date:
                       TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                       databaseType=SqlDbType.Date;
                       break;
                   case DatabaseType.Bool:
                       if (Value[i] == "true")
                           TransValue = 1;
                       else
                           TransValue = 0;
                       databaseType=SqlDbType.Bit;

                       break;
                   default:
                       TransValue = Value[i];
                       databaseType = SqlDbType.NVarChar;
                       break;

               }
               Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;
              
           }


           return Cmd;
       }
       ///<summary>
        ///
        /// 摘要:
        ///新增資料庫語法   
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Column_Name">要新增的欄位名稱</param>
        ///<param name="Value">要新增的值</param>
        /// 
       public void Insert(string Table_Name, string[] Column_Name, string[] Value)
       {
           //dataBaseConnection = new SqlConnection(SQL_Connecct_String);

           string Insert_str = @"insert into [";
           Insert_str += Table_Name;
           Insert_str += "] (";
           for (int i = 0; i < Column_Name.Length; i++)
           {
               Insert_str += "[" + Column_Name[i].Trim() + "]";
               if (i != Column_Name.Length - 1)
                   Insert_str += ",";
           }
           Insert_str += ") values (";
           for (int i = 0; i < Value.Length; i++)
           {
               Insert_str += "'" + Value[i] + "'";
               //         SQL_Insert_str += "@"+Value[i];
               if (i != Value.Length - 1)
                   Insert_str += ",";
           }
           Insert_str += ")";
           SqlCommand Inser_Command = new SqlCommand(Insert_str, dataBaseConnection);
           try
           {
              

               if (dataBaseConnection.State == ConnectionState.Open)
                   dataBaseConnection.Close();
               dataBaseConnection.Open();
               Inser_Command.ExecuteNonQuery();
               dataBaseConnection.Close();
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }
       }
       public void insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            //dataBaseConnection = new SqlConnection(SQL_Connecct_String);

            string Insert_str = @"insert into [";
            Insert_str += Table_Name;
            Insert_str += "] (";
            for (int i = 0; i < Column_Name.Length; i++)
            {
                Insert_str += "[" + Column_Name[i].Trim() + "]";
                if (i != Column_Name.Length - 1)
                    Insert_str += ",";
            }
            Insert_str += ") values (";
            for (int i = 0; i < Value.Length; i++)
            {
                Insert_str += "@Value" + i.ToString();
                //         SQL_Insert_str += "@"+Value[i];
                if (i != Value.Length - 1)
                    Insert_str += ",";
            }
            Insert_str += ")";
            SqlCommand Inser_Command = new SqlCommand(Insert_str, dataBaseConnection);
            try
            {
                Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                Inser_Command.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
       ///<summary>
        ///
        /// 摘要:
       ///資料庫比對欄位值，以第一個欄位為主的比對，回傳為比對欄位名稱比對後的布林值      
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Column_Name">要比對的欄位名稱，如果第一個false，不會在往下比對皆為false</param>
        ///<param name="Value">要比對的值</param>
        ///
       public bool[] compare(string Table_Name, string[] Column_Name, string[] Value)
       {
           bool[] Compare_Result = new bool[Column_Name.Length];
           if (Compare_Result.Length > 0)
           {
               // Request bb = new Request();
               // bb.Urn = Value[0];
               // string aaa = bb.Urn.ToString();
               DataSet Compare_DataSet = new DataSet();
               SqlDataAdapter Adapter;
               DataTable Table = new DataTable();
               //string SQL_Select_String = @"select * from account where [account_number]=" + "@123";


               string Select_String = @"select * from ";
               Select_String += Table_Name;
               Select_String += " where ";
               Select_String += "[" + Column_Name[0] + "]" + "=";
               Select_String += "'"+Value[0]+"'";
           
               SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
               try
               {
                 //.  SQL_Select_Command = TransType(SQL_Select_Command, Value, arrayType);

                   //SqlParameter param = new SqlParameter("@Value", SqlDbType.NVarChar);
                   //param.Value = Value[0];
                   //  param.Size = 50;
                   //  SQL_Select_Command.Parameters.Add(param);

                   //SQL_Select_Command.Parameters.Add("@i'm", SqlDbType.NVarChar).Value="'";

                   // SQL_Select_Command.Parameters.AddWithValue("@i'm", "account_number");
                   //  dataBaseConnection.Open();
                   //   SQL_Select_Command.ExecuteNonQuery();
                   Adapter = new SqlDataAdapter(Select_Command);
                   Adapter.Fill(Compare_DataSet, Table_Name);
                   //  dataBaseConnection.Close();
                   //
                   if (Compare_DataSet.Tables[0].Rows.Count >= 1)
                   {
                       Compare_Result[0] = true;
                       for (int i = 1; i < Column_Name.Length; i++)
                       {

                           if ((string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim() == Value[i])
                           {
                               Compare_Result[i] = true;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   System.Windows.Forms.MessageBox.Show(ex.ToString());
               }
           }
           return Compare_Result;
       }
       public bool[] compare(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)  
        {
            bool[] Compare_Result = new bool[Column_Name.Length];
            if (Compare_Result.Length > 0)
            {
               // Request bb = new Request();
               // bb.Urn = Value[0];
               // string aaa = bb.Urn.ToString();
                DataSet Compare_DataSet = new DataSet();
                SqlDataAdapter Adapter;
                DataTable Table = new DataTable();
                //string SQL_Select_String = @"select * from account where [account_number]=" + "@123";
               
                
                string Select_String = @"select * from ";
                Select_String += Table_Name;
                Select_String += " where ";
                Select_String += "[" + Column_Name[0] + "]" + "=";
                Select_String += "@Value0";
         //       SqlDbType[] arrayType = { SqlDbType.NVarChar };
                SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
                try
                {
                    Select_Command = TransType(Select_Command, Value, arrayType);

                    //SqlParameter param = new SqlParameter("@Value", SqlDbType.NVarChar);
                    //param.Value = Value[0];
                  //  param.Size = 50;
                  //  SQL_Select_Command.Parameters.Add(param);
                    
                    //SQL_Select_Command.Parameters.Add("@i'm", SqlDbType.NVarChar).Value="'";
                  
                  // SQL_Select_Command.Parameters.AddWithValue("@i'm", "account_number");
                  //  dataBaseConnection.Open();
                //   SQL_Select_Command.ExecuteNonQuery();
                    Adapter = new SqlDataAdapter(Select_Command);
                    Adapter.Fill(Compare_DataSet, Table_Name);
                  //  dataBaseConnection.Close();
               //
                if (Compare_DataSet.Tables[0].Rows.Count >= 1)
                {
                    Compare_Result[0] = true;
                    for (int i = 1; i < Column_Name.Length; i++)
                    {
                       
                        if ((string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim() == Value[i])
                        {
                            Compare_Result[i] = true;
                        }
                    }
                }
                }
                catch (Exception ex)
                {
                 System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            }
            return Compare_Result;
        }
       ///<summary>
       ///
       /// 摘要:
       ///更新資料庫語法  
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
       ///<param name="Search_value">要搜尋的值</param>
       ///<param name="Modify_Column_Name">要修改的欄位名稱</param>
       ///<param name="Modify_Value">要修改的值</param>
       ///    
       public void updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            string Updata_str = "UPDATE ";
            Updata_str += Table_Name;
            Updata_str += " SET ";
            string[] Value = new string[Modify_Value.Length + Search_value.Length];
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
           int i=0;
           while(i<Modify_Column_Name.Length)
            {
                Updata_str += Modify_Column_Name[i].Trim();
                Updata_str += "=";
                Updata_str += "@Value" + i.ToString();

                if (i != Modify_Column_Name.Length - 1)
                   Updata_str += ",";
               i++;

            }
            Updata_str += " WHERE ";
            while (i < Modify_Column_Name.Length + Search_Column_Name.Length)
            {
                Updata_str += Search_Column_Name[i - Modify_Column_Name.Length].Trim();
                Updata_str += "=";
                Updata_str += "@Value" + i.ToString();
                if (i != Modify_Column_Name.Length + Search_Column_Name.Length - 1)
                    Updata_str += " AND ";
                i++;
            }
            SqlCommand Updata_Command = new SqlCommand(Updata_str, dataBaseConnection);
            Updata_Command = TransType(Updata_Command, Value, arrayType);
            try
            {
                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                Updata_Command.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
       ///<summary>
       ///
       /// 摘要:
       ///查詢資料庫語法 ，回傳為查詢欄位名稱之欄位值
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
       ///<param name="value">要搜尋的值</param>
       ///<param name="Column_Name">要查詢的欄位名稱</param>
       ///    
       public string[] select_data(string Table_Name, string Search_Column_Name, string Value, string[] Column_Name)  //In the first than to the main
       {
           string[] Data_Result = new string[Column_Name.Length];


           DataSet Select_DataSet = new DataSet();
           SqlDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           Select_String += " Where ";
           Select_String += Search_Column_Name.Trim() + "=";
           Select_String += "'"+Value+"'";
           string[] arrayValue = { Value };
         //  SqlDbType[] arrayType = { SqlDbType.NVarChar };
           SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
           try
           {

            //   SQL_Select_Command = TransType(SQL_Select_Command, arrayValue, arrayType);
               //      dataBaseConnection.Open();
               Adapter = new SqlDataAdapter(Select_Command);
               Adapter.Fill(Select_DataSet, Table_Name);
               //    dataBaseConnection.Close();
               if (Select_DataSet.Tables[0].Rows.Count == 1)
               {
                   for (int i = 0; i < Column_Name.Length; i++)
                       Data_Result[i] = (string)Select_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim();
               }
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }


           return Data_Result;
       }
       public string[] select_data(string Table_Name, string Search_Column_Name, string Value, string[] Column_Name, DatabaseType[] arrayType)  //In the first than to the main
       {
           string[] Data_Result = new string[Column_Name.Length];


           DataSet Select_DataSet = new DataSet();
           SqlDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           Select_String += " Where ";
           Select_String += Search_Column_Name.Trim() + "=";
           Select_String += "@Value0";
           string[] arrayValue= {Value};
        //   SqlDbType[] arrayType = { SqlDbType .NVarChar};
           SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
           try
           {

               Select_Command = TransType(Select_Command, arrayValue, arrayType);
               //      dataBaseConnection.Open();
               Adapter = new SqlDataAdapter(Select_Command);
               Adapter.Fill(Select_DataSet, Table_Name);
               //    dataBaseConnection.Close();
               if (Select_DataSet.Tables[0].Rows.Count == 1)
               {
                   for (int i = 0; i < Column_Name.Length; i++)
                       Data_Result[i] = (string)Select_DataSet.Tables[0].Rows[0][Column_Name[i]];
               }
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }


           return Data_Result;
       }

       public DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
       {

           DataSet Select_DataSet = new DataSet();
           SqlDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           if (Column_Name.Length > 0)
           {
               Select_String += " Where ";
               for (int i = 0; i < Column_Name.Length;i++ )
               {
                   Select_String += Column_Name[i].Trim() + "=";
               Select_String += "@Value"+i.ToString();
               if (i != Value.Length - 1)
                   Select_String += " AND ";
               }
             
           }
           Select_String += ";" + Select_String;
           SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
           if (Column_Name.Length > 0)
           {

               Select_Command = TransType(Select_Command, Value, arrayType);
           }
           Adapter = new SqlDataAdapter(Select_Command);

           Adapter.Fill(Select_DataSet, Table_Name);
           Adapter.Fill(Select_DataSet, Table_Name+"10");
           return Select_DataSet;
       }
      
       public DataSet DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType, string[] Display_Column_Name)
       {

           DataSet Select_DataSet = new DataSet();
           SqlDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select [";
           for (int i = 0; i < Display_Column_Name.Length; i++)
           {
               Select_String += Display_Column_Name[i].Trim() + "]";

               if (i != Display_Column_Name.Length - 1)
                   Select_String += ",[";
           }
           Select_String += " from ";
           Select_String += Table_Name;
           if (Column_Name.Length > 0)
           {
               Select_String += " Where ";
               for (int i = 0; i < Column_Name.Length; i++)
               {
                   Select_String += Column_Name[i].Trim() + "=";
                   Select_String += "@Value" + i.ToString();
                   if (i != Value.Length - 1)
                       Select_String += " AND ";
               }

           }

           SqlCommand Select_Command = new SqlCommand(Select_String, dataBaseConnection);
           if (Column_Name.Length > 0)
           {

               Select_Command = TransType(Select_Command, Value, arrayType);
           }
           Adapter = new SqlDataAdapter(Select_Command);

           Adapter.Fill(Select_DataSet, Table_Name);
           return Select_DataSet;
       }
       public DataSet Custom_DataSet(string Table_Name,string Clause)
       {
           DataSet Select_DataSet = new DataSet();
           SqlDataAdapter Adapter;
           DataTable Table = new DataTable();
           SqlCommand Select_Command = new SqlCommand(Clause, dataBaseConnection);
       
           Adapter = new SqlDataAdapter(Select_Command);

           Adapter.Fill(Select_DataSet,Table_Name);
           return Select_DataSet;
       }

    }
    public enum DatabaseType
    {
        Str,
        Int,
        Double,
        Date,
        DateTime,
        Bool,
        Char

    }
    public enum DatabaseProvider
    {
        SQL,
        Access,
        oracle,
        MSDAORA,
        MYSQL,
            mongodb,
    }
    public enum DatabaseConnection
    {
        sql,
        oledb,
        oracle,
              mysql,
        access,
        mongodb,
    }
    
    public class DatabaseAccess
    {
         ///<summary>
       ///
       /// 摘要:
       ///資料庫SQL    
       ///
       ///
       /// </summary>
       ///<param name="Connecct_String">資料庫連接字串</param>
       public DatabaseAccess(string Connecct_String)
       {
           Access_Connecct_String = Connecct_String;
           dataBaseConnection = new OleDbConnection(Connecct_String);
       }
       ///<summary>
       /// 摘要:
       ///資料庫SQL    
       /// </summary>
       public DatabaseAccess()
       {
           dataBaseConnection = new OleDbConnection(Access_Connecct_String);
       }
       public static OleDbConnection dataBaseConnection = new OleDbConnection(Access_Connecct_String);
        //private static string SQL_Connecct_String = @"Provider=SQLOLEDB;Data Source=howardH/howardH000;Initial Catalog=master;";
      //  private static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=howardH/howardH000;Pwd=441342087;";
       private static string Access_Connecct_String = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Hotel_Database";
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        /// <param name="Connecct_String">資料庫連接字串</param>
        OleDbCommand TransType(OleDbCommand Cmd, string[] Value, DatabaseType[] Type) 
       {

           for (int i = 0; i < Value.Length; i++)
           {
               object TransValue;
               OleDbType databaseType;
               switch (Type[i])
               {
                   case DatabaseType.Int:

                       TransValue = Convert.ToInt32(Value[i]);
                       databaseType = OleDbType.Integer;
                       break;

                   case DatabaseType.Double:
                       TransValue = Convert.ToDouble(Value[i]);
                       databaseType = OleDbType.Double;
                       break;
                   case DatabaseType.DateTime:
                       TransValue = DateTime.ParseExact(Value[i], "yyyymmddhhmmss", System.Globalization.CultureInfo.InvariantCulture);
                       databaseType = OleDbType.DBTimeStamp;
                       // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                       //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                       break;
                   case DatabaseType.Date:
                      
                       TransValue = DateTime.ParseExact(Value[i], "yyyymmdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyymmdd");
                       databaseType = OleDbType.Date;
                       break;
                   case  DatabaseType.Bool:
                       if (Value[i] == "true")
                           TransValue = 1;
                       else
                           TransValue = 0;
                       databaseType = OleDbType.Boolean;
                       break;
                   default:
                       TransValue = Value[i];
                       databaseType = OleDbType.VarChar;
                       break;

               }
               Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;

           }


           return Cmd;
       }
     
       public void insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
       {
           //dataBaseConnection = new SqlConnection(SQL_Connecct_String);

           string Insert_str = @"insert into [";
           Insert_str += Table_Name;
           Insert_str += "] (";
           for (int i = 0; i < Column_Name.Length; i++)
           {
               Insert_str += "[" + Column_Name[i].Trim() + "]";
               if (i != Column_Name.Length - 1)
                   Insert_str += ",";
           }
           Insert_str += ") values (";
           for (int i = 0; i < Value.Length; i++)
           {
              Insert_str += "@Value" + i.ToString();
               //         SQL_Insert_str += "@"+Value[i];
               if (i != Value.Length - 1)
                  Insert_str += ",";
           }
          Insert_str += ")";
           OleDbCommand Inser_Command = new OleDbCommand(Insert_str, dataBaseConnection);
           try
           {
               Inser_Command = TransType(Inser_Command, Value, ValueType);

               if (dataBaseConnection.State == ConnectionState.Open)
                   dataBaseConnection.Close();
               dataBaseConnection.Open();
               Inser_Command.ExecuteNonQuery();
               dataBaseConnection.Close();
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }
       }
       ///<summary>
       ///
       /// 摘要:
       ///資料庫比對欄位值，以第一個欄位為主的比對，回傳為比對欄位名稱比對後的布林值      
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Column_Name">要比對的欄位名稱，如果第一個false，不會在往下比對皆為false</param>
       ///<param name="Value">要比對的值</param>
       ///
       public bool[] compare(string Table_Name, string[] Column_Name, string[] Value)
       {
           bool[] Compare_Result = new bool[Column_Name.Length];
           if (Compare_Result.Length > 0)
           {
               // Request bb = new Request();
               // bb.Urn = Value[0];
               // string aaa = bb.Urn.ToString();
               DataSet Compare_DataSet = new DataSet();
               OleDbDataAdapter Adapter;
               DataTable Table = new DataTable();
               //string SQL_Select_String = @"select * from account where [account_number]=" + "@123";


               string Select_String = @"select * from ";
               Select_String += Table_Name;
               Select_String += " where ";
               Select_String += "[" + Column_Name[0] + "]" + "=";
               Select_String += "'" + Value[0] + "'";

               OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
               try
               {
                   //.  SQL_Select_Command = TransType(SQL_Select_Command, Value, arrayType);

                   //SqlParameter param = new SqlParameter("@Value", SqlDbType.NVarChar);
                   //param.Value = Value[0];
                   //  param.Size = 50;
                   //  SQL_Select_Command.Parameters.Add(param);

                   //SQL_Select_Command.Parameters.Add("@i'm", SqlDbType.NVarChar).Value="'";

                   // SQL_Select_Command.Parameters.AddWithValue("@i'm", "account_number");
                   //  dataBaseConnection.Open();
                   //   SQL_Select_Command.ExecuteNonQuery();
                   Adapter = new OleDbDataAdapter(Select_Command);
                   Adapter.Fill(Compare_DataSet, Table_Name);
                   //  dataBaseConnection.Close();
                   //
                   if (Compare_DataSet.Tables[0].Rows.Count >= 1)
                   {
                       Compare_Result[0] = true;
                       for (int i = 1; i < Column_Name.Length; i++)
                       {

                           if ((string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim() == Value[i])
                           {
                               Compare_Result[i] = true;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   System.Windows.Forms.MessageBox.Show(ex.ToString());
               }
           }
           return Compare_Result;
       }
       public bool[] compare(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
       {
           bool[] Compare_Result = new bool[Column_Name.Length];
           if (Compare_Result.Length > 0)
           {
               // Request bb = new Request();
               // bb.Urn = Value[0];
               // string aaa = bb.Urn.ToString();
               DataSet Compare_DataSet = new DataSet();
               OleDbDataAdapter Adapter;
               DataTable Table = new DataTable();
               //string SQL_Select_String = @"select * from account where [account_number]=" + "@123";


               string Select_String = @"select * from ";
               Select_String += Table_Name;
               Select_String += " where ";
               Select_String += "[" + Column_Name[0] + "]" + "=";
               Select_String += "@Value0";
               //       SqlDbType[] arrayType = { SqlDbType.NVarChar };
               OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
               try
               {
                   Select_Command = TransType(Select_Command, Value, arrayType);

                   //SqlParameter param = new SqlParameter("@Value", SqlDbType.NVarChar);
                   //param.Value = Value[0];
                   //  param.Size = 50;
                   //  SQL_Select_Command.Parameters.Add(param);

                   //SQL_Select_Command.Parameters.Add("@i'm", SqlDbType.NVarChar).Value="'";

                   // SQL_Select_Command.Parameters.AddWithValue("@i'm", "account_number");
                   //  dataBaseConnection.Open();
                   //   SQL_Select_Command.ExecuteNonQuery();
                   Adapter = new OleDbDataAdapter(Select_Command);
                   Adapter.Fill(Compare_DataSet, Table_Name);
                   //  dataBaseConnection.Close();
                   //
                   if (Compare_DataSet.Tables[0].Rows.Count >= 1)
                   {
                       Compare_Result[0] = true;
                       for (int i = 1; i < Column_Name.Length; i++)
                       {

                           if ((string)Compare_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim() == Value[i])
                           {
                               Compare_Result[i] = true;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   System.Windows.Forms.MessageBox.Show(ex.ToString());
               }
           }
           return Compare_Result;
       }
       ///<summary>
       ///
       /// 摘要:
       ///更新資料庫語法  
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
       ///<param name="Search_value">要搜尋的值</param>
       ///<param name="Modify_Column_Name">要修改的欄位名稱</param>
       ///<param name="Modify_Value">要修改的值</param>
       ///    
       public void updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
       {
           string Updata_str = "UPDATE ";
           Updata_str += Table_Name;
           Updata_str += " SET ";
           string[] Value = new string[Modify_Value.Length + Search_value.Length];
           DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
           Modify_Value.CopyTo(Value, 0);
           Search_value.CopyTo(Value, Modify_Value.Length);
           Modify_Type.CopyTo(arrayType, 0);
           Search_Type.CopyTo(arrayType, Modify_Type.Length);
           int i = 0;
           while (i < Modify_Column_Name.Length)
           {
               Updata_str += Modify_Column_Name[i].Trim();
               Updata_str += "=";
               Updata_str += "@Value" + i.ToString();

               if (i != Modify_Column_Name.Length - 1)
                   Updata_str += ",";
               i++;

           }
           Updata_str += " WHERE ";
           while (i < Modify_Column_Name.Length + Search_Column_Name.Length)
           {
               Updata_str += Search_Column_Name[i - Modify_Column_Name.Length].Trim();
               Updata_str += "=";
               Updata_str += "@Value" + i.ToString();
               if (i != Modify_Column_Name.Length + Search_Column_Name.Length - 1)
                   Updata_str += ",";
               i++;
           }
           OleDbCommand Updata_Command = new OleDbCommand(Updata_str, dataBaseConnection);
           Updata_Command = TransType(Updata_Command, Value, arrayType);
           try
           {
               if (dataBaseConnection.State == ConnectionState.Open)
                   dataBaseConnection.Close();
               dataBaseConnection.Open();
               Updata_Command.ExecuteNonQuery();
               dataBaseConnection.Close();
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }
       }
       ///<summary>
       ///
       /// 摘要:
       ///查詢資料庫語法 ，回傳為查詢欄位名稱之欄位值
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
       ///<param name="value">要搜尋的值</param>
       ///<param name="Column_Name">要查詢的欄位名稱</param>
       ///    
       public string[] select_data(string Table_Name, string Search_Column_Name, string Value, string[] Column_Name)  //In the first than to the main
       {
           string[] Data_Result = new string[Column_Name.Length];


           DataSet Select_DataSet = new DataSet();
           OleDbDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           Select_String += " Where ";
           Select_String += Search_Column_Name.Trim() + "=";
           Select_String += "'" + Value + "'";
           string[] arrayValue = { Value };
           //  SqlDbType[] arrayType = { SqlDbType.NVarChar };
           OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
           try
           {

               //   SQL_Select_Command = TransType(SQL_Select_Command, arrayValue, arrayType);
               //      dataBaseConnection.Open();
               Adapter = new OleDbDataAdapter(Select_Command);
               Adapter.Fill(Select_DataSet, Table_Name);
               //    dataBaseConnection.Close();
               if (Select_DataSet.Tables[0].Rows.Count == 1)
               {
                   for (int i = 0; i < Column_Name.Length; i++)
                       Data_Result[i] = (string)Select_DataSet.Tables[0].Rows[0][Column_Name[i]].ToString().Trim();
               }
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }


           return Data_Result;
       }
       public string[] select_data(string Table_Name, string Search_Column_Name, string Value, string[] Column_Name, DatabaseType[] arrayType)  //In the first than to the main
       {
           string[] Data_Result = new string[Column_Name.Length];


           DataSet Select_DataSet = new DataSet();
           OleDbDataAdapter Adapter;
           DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           Select_String += " Where ";
           Select_String += Search_Column_Name.Trim() + "=";
           Select_String += "@Value0";
           string[] arrayValue = { Value };
           //   SqlDbType[] arrayType = { SqlDbType .NVarChar};
           OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
           try
           {

              Select_Command = TransType(Select_Command, arrayValue, arrayType);
               //      dataBaseConnection.Open();
               Adapter = new OleDbDataAdapter(Select_Command);
               Adapter.Fill(Select_DataSet, Table_Name);
               //    dataBaseConnection.Close();
               if (Select_DataSet.Tables[0].Rows.Count == 1)
               {
                   for (int i = 0; i < Column_Name.Length; i++)
                       Data_Result[i] = (string)Select_DataSet.Tables[0].Rows[0][Column_Name[i]];
               }
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
           }


           return Data_Result;
       }

       public DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
       {

           DataSet Select_DataSet = new DataSet();
           OleDbDataAdapter Adapter;
           //DataTable Table = new DataTable();
           string Select_String = @"select * from ";
           Select_String += Table_Name;
           if (Column_Name.Length > 0)
           {
               Select_String += " Where ";
               for (int i = 0; i < Column_Name.Length; i++)
               {
                   Select_String += Column_Name[i].Trim() + "=";
                   Select_String += "@Value" + i.ToString();
                   if (i != Value.Length - 1)
                       Select_String += " AND ";
               }

           }

           OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
           if (Column_Name.Length > 0)
           {

               Select_Command = TransType(Select_Command, Value, arrayType);
           }
           Adapter = new OleDbDataAdapter(Select_Command);

           Adapter.Fill(Select_DataSet, Table_Name);
           return Select_DataSet;
       }

       public DataSet DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType, string[] Display_Column_Name)
       {

           DataSet Select_DataSet = new DataSet();
           OleDbDataAdapter Adapter;
          // DataTable Table = new DataTable();
           string Select_String = @"select [";
           for (int i = 0; i < Display_Column_Name.Length; i++)
           {
               Select_String += Display_Column_Name[i].Trim() + "],[";
              
               if (i != Value.Length - 1)
                   Select_String += "] ";
           }
           Select_String+=" from ";
           Select_String += Table_Name;
           if (Column_Name.Length > 0)
           {
               Select_String += " Where ";
               for (int i = 0; i < Column_Name.Length; i++)
               {
                   Select_String += Column_Name[i].Trim() + "=";
                   Select_String += "@Value" + i.ToString();
                   if (i != Value.Length - 1)
                       Select_String += " AND ";
               }

           }

           OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
           if (Column_Name.Length > 0)
           {

               Select_Command = TransType(Select_Command, Value, arrayType);
           }
           Adapter = new OleDbDataAdapter(Select_Command);

           Adapter.Fill(Select_DataSet, Table_Name);
           return Select_DataSet;
       }
       public DataSet Custom_DataSet(string Table_Name, string Clause)
       {
           DataSet Select_DataSet = new DataSet();
           OleDbDataAdapter Adapter;
       //    DataTable Table = new DataTable();
           OleDbCommand Select_Command = new OleDbCommand(Clause, dataBaseConnection);

           Adapter = new OleDbDataAdapter(Select_Command);
        

           Adapter.Fill(Select_DataSet, Table_Name);
           return Select_DataSet;
       }
      
    }
    public abstract class Database
    {
        ///<summary>
        ///
        /// 摘要:
        ///新增資料庫語法     
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Column_Name">要新增的欄位名稱</param>
        ///<param name="Value">要新增的值</param>
        ///<param name="ValueType">資料型態</param>
        ///
        public abstract void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType);
        
        protected string Insert_Str(string Table_Name, string[] Column_Name,ref string[] Value ,DatabaseProvider databaseprovider,DatabaseConnection Connection)
        {
            if (Column_Name.Length == Value.Length)
            {
                string insert_str = @"Insert into ";
                insert_str += Table_Name;
                insert_str += " (";
                for (int i = 0; i < Column_Name.Length; i++)
                {
                    if (databaseprovider == DatabaseProvider.oracle || databaseprovider == DatabaseProvider.MSDAORA)
                        insert_str += "\"" + Column_Name[i] + "\"";
                    else if(databaseprovider==DatabaseProvider.MYSQL)
                        insert_str +=  Column_Name[i];
                    else
                        insert_str += "[" + Column_Name[i] + "]";
                    if (i != Column_Name.Length - 1)
                        insert_str += ",";
                }
                insert_str += ") values (";
                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] == null)
                        Value[i] = string.Empty;
                    if (Connection == DatabaseConnection.oledb)
                        insert_str += "?";//":"+"Value" + i.ToString();
                    //         SQL_Insert_str += "@"+Value[i];
                    else if (Connection == DatabaseConnection.oracle)
                        insert_str += ":" + "Value" + i.ToString();
                    else
                        insert_str += "@" + "Value" + i.ToString();
                    if (i != Value.Length - 1)
                        insert_str += ",";
                }
                insert_str += ")";
                return insert_str;
            }
            else
            {
                throw new System.ArgumentException("欄位數量和值數量不正確 "); ;
            }
        }
        ///<summary>
        ///
        /// 摘要:
        ///更新資料庫語法   
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Search_Column_Name">要搜尋的欄位名稱</param>
        ///<param name="Search_value">要搜尋的值</param>
        ///<param name="Search_Type">要搜尋值的資料型態</param>
        ///<param name="Modify_Column_Name">要修改的欄位名稱</param>
        ///<param name="Modify_Value">要修改的值</param>
        ///<param name="Modify_Type">要修改值的資料型態</param>
        ///
        public abstract void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type);
      
        protected string Updata_Str(string Table_Name, string[] Search_Column_Name, string[] Search_value, string[] Modify_Column_Name,ref string[] Modify_Value,DatabaseConnection Connection)
        {
            if (Search_Column_Name.Length == Search_value.Length && Modify_Column_Name.Length == Modify_Value.Length)
            {
                string Updata_str = "UPDATE ";
                Updata_str += Table_Name;
                Updata_str += " SET ";
                string[] Value = new string[Modify_Value.Length + Search_value.Length];

                Modify_Value.CopyTo(Value, 0);
                Search_value.CopyTo(Value, Modify_Value.Length);

                int i = 0;
                while (i < Modify_Column_Name.Length)
                {
                    Updata_str += Modify_Column_Name[i].Trim();
                    Updata_str += "=";
                    if (Modify_Value[i] == null)
                        Modify_Value[i] = string.Empty;
                    //Updata_str += "@Value" + i.ToString();
                    if (Connection == DatabaseConnection.oledb)
                        Updata_str += "?";//":"+"Value" + i.ToString();
                    //         SQL_Insert_str += "@"+Value[i];
                    else if (Connection == DatabaseConnection.oracle)
                        Updata_str += ":" + "Value" + i.ToString();
                    else
                        Updata_str += "@" + "Value" + i.ToString();

                    if (i != Modify_Column_Name.Length - 1)
                        Updata_str += ",";
                    i++;

                }
                Updata_str += " WHERE ";
                while (i < Modify_Column_Name.Length + Search_Column_Name.Length)
                {
                    Updata_str += Search_Column_Name[i - Modify_Column_Name.Length].Trim();
                    if (Search_value[i - Modify_Column_Name.Length] == null)
                    {
                        Updata_str += " IS NULL";
                    }
                    else
                    {
                        Updata_str += "=";
                        //  Updata_str += "@Value" + i.ToString();
                        if (Connection == DatabaseConnection.oledb)
                            Updata_str += "?";//":"+"Value" + i.ToString();
                        //         SQL_Insert_str += "@"+Value[i];
                        else if (Connection == DatabaseConnection.oracle)
                            Updata_str += ":" + "Value" + i.ToString();
                        else
                            Updata_str += "@" + "Value" + i.ToString();
                    }
                    if (i != Modify_Column_Name.Length + Search_Column_Name.Length - 1)
                        Updata_str += " AND ";
                    i++;
                }
                return Updata_str;
            }
                  else
            {
                throw new System.ArgumentException("欄位數量和值數量不正確 "); ;
            }
            
        }
        public abstract DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType);



        public virtual DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
            throw new ArgumentNullException("DB不支援自訂SQL");
        }
       
        protected string all_dataset_str(string Table_Name, string[] Column_Name, string[] Value, DatabaseConnection Connection)
        {
            if (Column_Name.Length == Value.Length)
            {
                string Select_String = @"select * from ";
                Select_String += Table_Name;
                if (Column_Name.Length > 0)
                {
                    Select_String += " Where ";
                    for (int i = 0; i < Column_Name.Length; i++)
                    {
                        Select_String += Column_Name[i].Trim();
                        if (Value[i] == null)
                        {
                            Select_String += " IS NULL";
                        }
                        else
                        {
                            Select_String += "=";
                            //Select_String += "@Value" + i.ToString();
                            if (Connection == DatabaseConnection.oledb)
                                Select_String += "?";//":"+"Value" + i.ToString();
                            //         SQL_Insert_str += "@"+Value[i];
                            else if (Connection == DatabaseConnection.oracle)
                                Select_String += ":" + "Value" + i.ToString();
                            else
                                Select_String += "@" + "Value" + i.ToString();
                        }
                        if (i != Value.Length - 1)
                            Select_String += " AND ";
                    }

                }
                return Select_String;
            }
            else
            {
                throw new System.ArgumentException("欄位數量和值數量不正確 "); ;
            }
        }
        protected string delete_str(string Table_Name, string[] Column_Name, string[] Value, DatabaseConnection Connection)
        {
            if (Column_Name.Length == Value.Length)
            {
                string Select_String = @"delete from ";
                Select_String += Table_Name;
                if (Column_Name.Length > 0)
                {
                    Select_String += " Where ";
                    for (int i = 0; i < Column_Name.Length; i++)
                    {
                        Select_String += Column_Name[i].Trim();
                        if (Value[i] == null)
                        {
                            Select_String += " IS NULL";
                        }
                        else
                        {
                            //Select_String += "@Value" + i.ToString();
                            Select_String += "=";
                            if (Connection == DatabaseConnection.oledb)
                                Select_String += "?";//":"+"Value" + i.ToString();
                            //         SQL_Insert_str += "@"+Value[i];
                            else if (Connection == DatabaseConnection.oracle)
                                Select_String += ":" + "Value" + i.ToString();
                            else
                                Select_String += "@" + "Value" + i.ToString();


                        }
                        if (i != Value.Length - 1)
                            Select_String += " AND ";
                    }

                }
                return Select_String;
            }
            else
            {
                throw new System.ArgumentException("欄位數量和值數量不正確 "); ;
            }
        }
        public virtual void Run_Costom_Command(string Command_String)
        {
            throw new ArgumentNullException("DB不支援自訂SQL");
        }

        public abstract void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType);
    

         public virtual void test()
        {
         }
    }
    public class SQL : Database
    {
        public SqlConnection dataBaseConnection;
        private DatabaseProvider databaseprovider;
        static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public SQL(string connecct_string)
        {
           
            SQL_Connecct_String = connecct_string;
            dataBaseConnection = new SqlConnection(SQL_Connecct_String);
            databaseprovider = DatabaseProvider.SQL;
            Test_Connect();
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public SQL()
        {
            dataBaseConnection = new SqlConnection(SQL_Connecct_String);
            databaseprovider = DatabaseProvider.SQL;
            Test_Connect();
        }
        void Test_Connect()
        {
            try
            {
                dataBaseConnection.Open();
                dataBaseConnection.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        SqlCommand TransType(SqlCommand Cmd, string[] Value, DatabaseType[] Type)
        {
            try
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] != null)
                    {
                        object TransValue;
                        SqlDbType databaseType;
                        switch (Type[i])
                        {
                            case DatabaseType.Int:
                                if (Value[i] == string.Empty)
                                
                                       TransValue = DBNull.Value;
                                
                                else
                                    TransValue = Convert.ToInt32(Value[i]);
                                databaseType = SqlDbType.Int;
                                break;

                            case DatabaseType.Double:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Convert.ToDouble(Value[i]);
                                databaseType = SqlDbType.Float;
                                break;
                            case DatabaseType.DateTime:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                databaseType = SqlDbType.DateTime;
                                // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case DatabaseType.Date:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                                databaseType = SqlDbType.Date;
                                break;
                            case DatabaseType.Bool:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                {
                                    if (Value[i] == "true")
                                        TransValue = 1;
                                    else
                                        TransValue = 0;
                                }
                                    databaseType = SqlDbType.Bit;
                                
                                break;
                            default:
                                if (Value[i] == string.Empty)
                                    TransValue =  DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = SqlDbType.NVarChar;
                                break;

                        }
                        Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;
                    }
                }
            }
                 
            catch (Exception ex)
            {
                throw ex;
            }


            return Cmd;
        }
  
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            string[] _Value = new string[Value.Length];
           Array.Copy(Value, _Value, Value.Length);
           DatabaseConnection Connection = DatabaseConnection.sql;
           SqlCommand Insert_Cmd = new SqlCommand(base.Insert_Str(Table_Name, Column_Name, ref _Value, databaseprovider, Connection), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, _Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
               // System.Windows.Forms.MessageBox.Show(ex.ToString());
               // return;
            }
            Run_Command(Insert_Cmd);
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {

            string[] _Modify_Value = new string[Modify_Value.Length];
            Array.Copy(Modify_Value, _Modify_Value, Modify_Value.Length);
            DatabaseConnection Connection = DatabaseConnection.sql;
            SqlCommand Updata_Cmd = new SqlCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, ref _Modify_Value, Connection), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[_Modify_Value.Length + Search_value.Length];
            _Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, _Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(SqlCommand sqlcommand)
        {
             
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
               sqlcommand.ExecuteNonQuery();
              
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            DatabaseConnection Connection = DatabaseConnection.sql;
            SqlCommand Select_Command = new SqlCommand(base.all_dataset_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
            if (Column_Name.Length > 0)
            {

                Select_Command = TransType(Select_Command, Value, arrayType);
            }

            return Command_Catch_data(Table_Name, Select_Command);
        }
        DataSet Command_Catch_data(string Table_Name, SqlCommand Select_sqlcommand)
        {
            DataSet Select_DataSet = new DataSet();
            SqlDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);


                Adapter = new SqlDataAdapter(Select_sqlcommand);

                Adapter.Fill(Select_DataSet, Table_Name);
             //   return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Select_DataSet;
        }
        public override DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
            try
            {
                SqlCommand Select_Command = new SqlCommand(Custom_String, dataBaseConnection);
                DataSet Select_DataSet = Command_Catch_data(Table_Name, Select_Command);
                return Select_DataSet;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            
        }
        public override void Run_Costom_Command(string Command_String)
        {
            SqlCommand Costom_Command = new SqlCommand(Command_String, dataBaseConnection);
            Run_Command(Costom_Command);
        }
        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            DatabaseConnection Connection = DatabaseConnection.sql;
            SqlCommand Delete_Cmd = new SqlCommand(base.delete_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
            try
            {
                Delete_Cmd = TransType(Delete_Cmd, Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                // return;
            }
            Run_Command(Delete_Cmd);
        }
    }
    public class OLEDB : Database
    {
        private DatabaseProvider databaseprovider;
        public OleDbConnection dataBaseConnection;
        static string Connecct_String = @"Provider=oraoledb.oracle;Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
      //  "Provider=MSDAORA.1;User ID=myUID;password=myPWD;    Data Source=myOracleServer;Persist Security Info=False";
        //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Hotel_Database"
        //Provider=MySQL Provider; Data Source=MySQLServerIP; User ID =MyID; Password=MyPassword; Initial Catalog=DatabaseName; Activation=TheActivationCode;
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public OLEDB(string connecct_string)
        {
            Connecct_String = connecct_string;
            try
            {
                dataBaseConnection = new OleDbConnection(Connecct_String);

                int a = Connecct_String.IndexOf("Provider=");
                string Str = Connecct_String.Remove(0, a);
                int b = Str.IndexOf(";");
                string Provider_Str = Str.Remove(b);
                int c = Provider_Str.IndexOf("=");
                string Provider = Provider_Str.Remove(0, c);
                string Provider_lower = "";
                for (int i = 0; i < Provider.Length; i++)
                    Provider_lower += char.ToLower(Provider[i]);
                if (Provider_lower.IndexOf("msdaora") > 0)
                    databaseprovider = DatabaseProvider.MSDAORA;
                else if (Provider_lower.IndexOf("oracle") > 0)
                    databaseprovider = DatabaseProvider.oracle;
                else if (Provider_lower.IndexOf("microsoft") > 0)
                    databaseprovider = DatabaseProvider.Access;
                else
                    databaseprovider = DatabaseProvider.SQL;

            }
            catch(Exception ex)
            {

                throw ex;
            }

       // string bb = Provider_lower;
            Test_Connect();
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public  OLEDB()
        {
            dataBaseConnection = new OleDbConnection(Connecct_String);
            databaseprovider = DatabaseProvider.oracle;
            Test_Connect();
        }
        void Test_Connect()
        {
            try
            {
                dataBaseConnection.Open();
                dataBaseConnection.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
      
       ///<summary>
       ///
       /// 摘要:
       ///新增資料庫語法   
       /// </summary>
       ///<param name="Table_Name">資料庫Table名稱</param>
       ///<param name="Column_Name">要新增的欄位名稱</param>
       ///<param name="Value">要新增的值</param>
       /// 
       public void Insert(string Table_Name, string[] Column_Name, string[] Value)
       {
           //dataBaseConnection = new SqlConnection(SQL_Connecct_String);

           string Insert_str = @"insert into [";
           Insert_str += Table_Name;
           Insert_str += "] (";
           for (int i = 0; i < Column_Name.Length; i++)
           {
               Insert_str += "[" + Column_Name[i] + "]";
               if (i != Column_Name.Length - 1)
                   Insert_str += ",";
           }
           Insert_str += ") values (";
           for (int i = 0; i < Value.Length; i++)
           {
               Insert_str += "'" + Value[i] + "'";
               //         SQL_Insert_str += "@"+Value[i];
               if (i != Value.Length - 1)
                   Insert_str += ",";
           }
           Insert_str += ")";
           OleDbCommand Inser_Command = new OleDbCommand(Insert_str, dataBaseConnection);
           try
           {


               if (dataBaseConnection.State == ConnectionState.Open)
                   dataBaseConnection.Close();
               dataBaseConnection.Open();
               Inser_Command.ExecuteNonQuery();
               dataBaseConnection.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        OleDbCommand TransType(OleDbCommand Cmd, string[] Value, DatabaseType[] Type)
        {
            try
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] != null)
                    {
                        object TransValue;
                        OleDbType databaseType;
                        switch (Type[i])
                        {
                            case DatabaseType.Int:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Convert.ToInt32(Value[i]);
                                databaseType = OleDbType.Integer;
                                break;

                            case DatabaseType.Double:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Convert.ToDouble(Value[i]);
                                databaseType = OleDbType.Double;
                                break;
                            case DatabaseType.DateTime:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                databaseType = OleDbType.DBTimeStamp;
                                // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case DatabaseType.Date:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                                databaseType = OleDbType.DBDate;
                                break;
                            case DatabaseType.Bool:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                {
                                    if (Value[i] == "true")
                                        TransValue = 1;
                                    else
                                        TransValue = 0;
                                }
                                databaseType = OleDbType.Boolean;

                                break;
                            case DatabaseType.Char:
                                if (Value[i] == string.Empty)
                                    TransValue = (object)DBNull.Value;// DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = OleDbType.Char;
                                break;
                            default:
                                if (Value[i] == string.Empty)
                                    TransValue = (object)DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = OleDbType.VarChar;
                                break;

                        }

                        switch (databaseprovider)
                        {
                            case DatabaseProvider.MSDAORA:
                                OleDbParameter Para = new OleDbParameter();
                                Para.Value = TransValue;
                                Cmd.Parameters.Add(Para);
                                break;
                            case DatabaseProvider.oracle:
                                Cmd.Parameters.Add(":Value" + i.ToString(), databaseType).Value = TransValue;
                                break;
                            default:
                                Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;
                                break;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Cmd;
        }
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            string[] _Value = new string[Value.Length];
            Array.Copy(Value, _Value, Value.Length);
            DatabaseConnection Connection = DatabaseConnection.oledb;
            OleDbCommand Insert_Cmd = new OleDbCommand(base.Insert_Str(Table_Name, Column_Name, ref _Value, databaseprovider,Connection), dataBaseConnection);
            
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, _Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Insert_Cmd);
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            string[] _Modify_Value = new string[Modify_Value.Length];
            Array.Copy(Modify_Value, _Modify_Value, Modify_Value.Length);
            DatabaseConnection Connection = DatabaseConnection.oledb;
            OleDbCommand Updata_Cmd = new OleDbCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, ref _Modify_Value, Connection), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[_Modify_Value.Length + Search_value.Length];
            _Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, _Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(OleDbCommand sqlcommand)
        {
            sqlcommand.CommandType = System.Data.CommandType.Text;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
            sqlcommand.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            try
            {
                DatabaseConnection Connection = DatabaseConnection.oledb;
                OleDbCommand Select_Command = new OleDbCommand(base.all_dataset_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
                if (Column_Name.Length > 0)
                {

                    Select_Command = TransType(Select_Command, Value, arrayType);
                }

                return Command_Catch_data(Table_Name, Select_Command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        DataSet Command_Catch_data(string Table_Name, OleDbCommand Select_sqlcommand)
        {
            DataSet Select_DataSet = new DataSet();
            OleDbDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);


                Adapter = new OleDbDataAdapter(Select_sqlcommand);

                Adapter.Fill(Select_DataSet, Table_Name);
              //  return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Select_DataSet;
        }
        public override DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
            try
            {
                OleDbCommand Select_Command = new OleDbCommand(Custom_String, dataBaseConnection);
                DataSet Select_DataSet = Command_Catch_data(Table_Name, Select_Command);

                return Select_DataSet;
            }
            catch(Exception ex)
            {
                throw ex;

            }

        }
        public override void Run_Costom_Command(string Command_String)
        {
            try
            {
                OleDbCommand Costom_Command = new OleDbCommand(Command_String, dataBaseConnection);
                Costom_Command.CommandType = System.Data.CommandType.Text;
                Run_Command(Costom_Command);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            DatabaseConnection Connection = DatabaseConnection.oledb;
            OleDbCommand Delete_Cmd = new OleDbCommand(base.delete_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
            try
            {
                Delete_Cmd = TransType(Delete_Cmd, Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                // return;
            }
            Run_Command(Delete_Cmd);
        }
    }
#if DEBUG
    public class Oracle : Database
    {
        private DatabaseProvider databaseprovider;
        public OracleConnection dataBaseConnection;
        static string Oracle_Connecct_String = @"Data Source=(DESCRIPTION="
            + "(ADDRESS=(PROTOCOL=TCP)(HOST=資料庫位址)(PORT=1521))"
            + "(CONNECT_DATA=(SERVICE_NAME=資料庫名稱)));"
            + "User Id=資料庫帳號;Password=資料庫密碼;";
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public Oracle(string connecct_string)
        {
            Oracle_Connecct_String = connecct_string;
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            dataBaseConnection = new OracleConnection(Oracle_Connecct_String);
            databaseprovider = DatabaseProvider.oracle;

           
            Test_Connect();
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public Oracle()
        {
            dataBaseConnection = new OracleConnection(Oracle_Connecct_String);

            databaseprovider = DatabaseProvider.oracle;
            Test_Connect();
        }
 
        void Test_Connect()
        {
            try
            {
                dataBaseConnection.Open();
                
               dataBaseConnection.Close();
            }
           catch(Exception ex)
            {
                throw ex;
           }
        }
        OracleCommand TransType(OracleCommand Cmd, string[] Value, DatabaseType[] Type)
        {
            try
            {

                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] != null)
                    {
                        object TransValue;
                        OracleDbType databaseType;
                        switch (Type[i])
                        {
                            case DatabaseType.Int:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Convert.ToInt32(Value[i]);
                                databaseType = OracleDbType.Int32;
                                break;

                            case DatabaseType.Double:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Convert.ToDouble(Value[i]);
                                databaseType = OracleDbType.Double;
                                break;
                            case DatabaseType.DateTime:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                databaseType = OracleDbType.Date;
                                // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case DatabaseType.Date:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                                databaseType = OracleDbType.Date;
                                break;
                            case DatabaseType.Bool:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                {
                                    if (Value[i] == "true")
                                        TransValue = 1;
                                    else
                                        TransValue = 0;
                                }
                                    databaseType = OracleDbType.Boolean;
                                
                                break;
                            case DatabaseType.Char:
                            //    byte[] iso = System.Text.Encoding.GetEncoding("GBK").GetBytes(Value[i]);
                               // string bb = System.Text.Encoding.GetEncoding("ISO8859-1").GetString(iso);
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Value[i];
                                databaseType = OracleDbType.Char;
                                break;
                            default:

                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                TransValue = Value[i];
                                databaseType = OracleDbType.Varchar2;
                                break;

                        }
                        Cmd.Parameters.Add(":Value" + i.ToString(), databaseType).Value = TransValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return Cmd;
        }
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            string[] _Value = new string[Value.Length];
            Array.Copy(Value, _Value, Value.Length);
            DatabaseConnection Connection = DatabaseConnection.oracle;
            OracleCommand Insert_Cmd = new OracleCommand(base.Insert_Str(Table_Name, Column_Name,ref _Value, databaseprovider,Connection), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, _Value, ValueType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return;
            }
            Run_Command(Insert_Cmd);
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            string[] _Modify_Value = new string[Modify_Value.Length];
            Array.Copy(Modify_Value, _Modify_Value, Modify_Value.Length);
            DatabaseConnection Connection = DatabaseConnection.oracle;
            OracleCommand Updata_Cmd = new OracleCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, ref _Modify_Value, Connection), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[_Modify_Value.Length + Search_value.Length];
           _Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, _Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(OracleCommand sqlcommand)
        {
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                sqlcommand.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            try
            {
                DatabaseConnection Connection = DatabaseConnection.oracle;
                OracleCommand Select_Command = new OracleCommand(base.all_dataset_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
                if (Column_Name.Length > 0)
                {

                    Select_Command = TransType(Select_Command, Value, arrayType);
                }

                return Command_Catch_data(Table_Name, Select_Command);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        DataSet Command_Catch_data(string Table_Name, OracleCommand Select_command)
        {
            DataSet Select_DataSet = new DataSet();
            OracleDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

              //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter = new OracleDataAdapter(Select_command);
             //   System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter.Fill(Select_DataSet, Table_Name);
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;

            }
       //     return Select_DataSet;
        }
        public override DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
          //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            
          //  dataBaseConnection.SetSessionInfo(info);
            try
            {
                OracleCommand Select_Command = new OracleCommand(Custom_String, dataBaseConnection);

                DataSet Select_DataSet = Command_Catch_data(Table_Name, Select_Command);


                return Select_DataSet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public override void Run_Costom_Command(string Command_String)
        {
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            OracleCommand Costom_Command = new OracleCommand(Command_String, dataBaseConnection);
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            Run_Command(Costom_Command);
        }
        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            DatabaseConnection Connection = DatabaseConnection.oracle;
            OracleCommand Delete_Cmd = new OracleCommand(base.delete_str(Table_Name, Column_Name, Value,Connection), dataBaseConnection);
            try
            {
                Delete_Cmd = TransType(Delete_Cmd, Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                // return;
            }
            Run_Command(Delete_Cmd);
        }
    
    }
#endif
    public class ODBC : Database
    {
        private DatabaseProvider databaseprovider;
        public OdbcConnection dataBaseConnection;
        static string Connecct_String = @"Provider=oraoledb.oracle;Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        //  "Provider=MSDAORA.1;User ID=myUID;password=myPWD;    Data Source=myOracleServer;Persist Security Info=False";
        //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Hotel_Database"
        //Provider=MySQL Provider; Data Source=MySQLServerIP; User ID =MyID; Password=MyPassword; Initial Catalog=DatabaseName; Activation=TheActivationCode;
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public ODBC(string connecct_string)
        {
            Connecct_String = connecct_string;
            try
            {
                dataBaseConnection = new OdbcConnection(Connecct_String);

              /*  int a = Connecct_String.IndexOf("Provider=");
                string Str = Connecct_String.Remove(0, a);
                int b = Str.IndexOf(";");
                string Provider_Str = Str.Remove(b);
                int c = Provider_Str.IndexOf("=");
                string Provider = Provider_Str.Remove(0, c);
                string Provider_lower = "";
                for (int i = 0; i < Provider.Length; i++)
                    Provider_lower += char.ToLower(Provider[i]);
                if (Provider_lower.IndexOf("msdaora") > 0)
                    databaseprovider = DatabaseProvider.MSDAORA;
                else if (Provider_lower.IndexOf("oracle") > 0)
                    databaseprovider = DatabaseProvider.oracle;
                else if (Provider_lower.IndexOf("microsoft") > 0)
                    databaseprovider = DatabaseProvider.Access;
                else
                    databaseprovider = DatabaseProvider.SQL;
                */
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // string bb = Provider_lower;
            Test_Connect();
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public ODBC()
        {
            dataBaseConnection = new OdbcConnection(Connecct_String);
            databaseprovider = DatabaseProvider.oracle;
            Test_Connect();
        }
        void Test_Connect()
        {
            try
            {
                dataBaseConnection.Open();
                dataBaseConnection.Close();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        OdbcCommand TransType(OdbcCommand Cmd, string[] Value, DatabaseType[] Type)
        {
            try
            {

                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] != null)
                    {
                        object TransValue;
                        OdbcType databaseType;
                        switch (Type[i])
                        {
                            case DatabaseType.Int:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Convert.ToInt32(Value[i]);
                                databaseType = OdbcType.Int;
                                break;

                            case DatabaseType.Double:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Convert.ToDouble(Value[i]);
                                databaseType = OdbcType.Double;
                                break;
                            case DatabaseType.DateTime:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                databaseType = OdbcType.Date;
                                // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case DatabaseType.Date:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                                databaseType = OdbcType.Date;
                                break;
                            case DatabaseType.Bool:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                {
                                    if (Value[i] == "true")
                                        TransValue = 1;
                                    else
                                        TransValue = 0;
                                }
                                databaseType = OdbcType.Bit;

                                break;
                            case DatabaseType.Char:
                                //    byte[] iso = System.Text.Encoding.GetEncoding("GBK").GetBytes(Value[i]);
                                // string bb = System.Text.Encoding.GetEncoding("ISO8859-1").GetString(iso);
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = OdbcType.Char;
                                break;
                            default:

                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = OdbcType.NVarChar;
                                break;

                        }
                        Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return Cmd;
        }
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            string[] _Value = new string[Value.Length];
            Array.Copy(Value, _Value, Value.Length);
            DatabaseConnection Connection = DatabaseConnection.sql;
            OdbcCommand Insert_Cmd = new OdbcCommand(base.Insert_Str(Table_Name, Column_Name, ref _Value, databaseprovider, Connection), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, _Value, ValueType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return;
            }
            Run_Command(Insert_Cmd);
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            string[] _Modify_Value = new string[Modify_Value.Length];
            Array.Copy(Modify_Value, _Modify_Value, Modify_Value.Length);
            DatabaseConnection Connection = DatabaseConnection.sql;
            OdbcCommand Updata_Cmd = new OdbcCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, ref _Modify_Value, Connection), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[_Modify_Value.Length + Search_value.Length];
            _Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, _Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(OdbcCommand sqlcommand)
        {
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                sqlcommand.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            try
            {
                DatabaseConnection Connection = DatabaseConnection.oracle;
                OdbcCommand Select_Command = new OdbcCommand(base.all_dataset_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
                if (Column_Name.Length > 0)
                {

                    Select_Command = TransType(Select_Command, Value, arrayType);
                }

                return Command_Catch_data(Table_Name, Select_Command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        DataSet Command_Catch_data(string Table_Name, OdbcCommand Select_command)
        {
            DataSet Select_DataSet = new DataSet();
            OdbcDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter = new OdbcDataAdapter(Select_command);
                //   System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter.Fill(Select_DataSet, Table_Name);
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            //     return Select_DataSet;
        }
        public override DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
            //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");

            //  dataBaseConnection.SetSessionInfo(info);
            try
            {
                OdbcCommand Select_Command = new OdbcCommand(Custom_String, dataBaseConnection);

                DataSet Select_DataSet = Command_Catch_data(Table_Name, Select_Command);


                return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void Run_Costom_Command(string Command_String)
        {
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            OdbcCommand Costom_Command = new OdbcCommand(Command_String, dataBaseConnection);
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            Run_Command(Costom_Command);
        }
        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            DatabaseConnection Connection = DatabaseConnection.oracle;
            OdbcCommand Delete_Cmd = new OdbcCommand(base.delete_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
            try
            {
                Delete_Cmd = TransType(Delete_Cmd, Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                // return;
            }
            Run_Command(Delete_Cmd);
        }
    }
    public class MYSQL : Database
    {
        private DatabaseProvider databaseprovider;
    
        public MySqlConnection dataBaseConnection;
        static string Connecct_String = @"Provider=oraoledb.oracle;Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        //  "Provider=MSDAORA.1;User ID=myUID;password=myPWD;    Data Source=myOracleServer;Persist Security Info=False";
        //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Hotel_Database"
        //Provider=MySQL Provider; Data Source=MySQLServerIP; User ID =MyID; Password=MyPassword; Initial Catalog=DatabaseName; Activation=TheActivationCode;
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public MYSQL(string connecct_string)
        {
            Connecct_String = connecct_string;
            try
            {
                dataBaseConnection = new MySqlConnection(Connecct_String);
                databaseprovider = DatabaseProvider.MYSQL;

                /*  int a = Connecct_String.IndexOf("Provider=");
                  string Str = Connecct_String.Remove(0, a);
                  int b = Str.IndexOf(";");
                  string Provider_Str = Str.Remove(b);
                  int c = Provider_Str.IndexOf("=");
                  string Provider = Provider_Str.Remove(0, c);
                  string Provider_lower = "";
                  for (int i = 0; i < Provider.Length; i++)
                      Provider_lower += char.ToLower(Provider[i]);
                  if (Provider_lower.IndexOf("msdaora") > 0)
                      databaseprovider = DatabaseProvider.MSDAORA;
                  else if (Provider_lower.IndexOf("oracle") > 0)
                      databaseprovider = DatabaseProvider.oracle;
                  else if (Provider_lower.IndexOf("microsoft") > 0)
                      databaseprovider = DatabaseProvider.Access;
                  else
                      databaseprovider = DatabaseProvider.SQL;
                  */
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // string bb = Provider_lower;
            Test_Connect();
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public MYSQL()
        {
            dataBaseConnection = new MySqlConnection(Connecct_String);
            databaseprovider = DatabaseProvider.MYSQL;
            Test_Connect();
        }
        void Test_Connect()
        {
            try
            {
                dataBaseConnection.Open();
                dataBaseConnection.Close();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        ///
        /// 摘要:
        ///新增資料庫語法   
        /// </summary>
        ///<param name="Table_Name">資料庫Table名稱</param>
        ///<param name="Column_Name">要新增的欄位名稱</param>
        ///<param name="Value">要新增的值</param>
        /// 
        MySqlCommand TransType(MySqlCommand Cmd, string[] Value, DatabaseType[] Type)
        {
            try
            {

                for (int i = 0; i < Value.Length; i++)
                {
                    if (Value[i] != null)
                    {
                        object TransValue;
                        MySqlDbType databaseType;
                        switch (Type[i])
                        {
                            case DatabaseType.Int:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Convert.ToInt32(Value[i]);
                                databaseType = MySqlDbType.Int32;
                                break;

                            case DatabaseType.Double:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Convert.ToDouble(Value[i]);
                                databaseType = MySqlDbType.Double;
                                break;
                            case DatabaseType.DateTime:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = DateTime.ParseExact(Value[i], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                databaseType = MySqlDbType.DateTime;
                                // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            case DatabaseType.Date:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = DateTime.ParseExact(Value[i], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                                databaseType = MySqlDbType.Date;
                                break;
                            case DatabaseType.Bool:
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                {
                                    if (Value[i] == "True")
                                        TransValue = 1;
                                    else
                                        TransValue = 0;
                                }
                                databaseType = MySqlDbType.Bit;

                                break;
                            case DatabaseType.Char:
                                //    byte[] iso = System.Text.Encoding.GetEncoding("GBK").GetBytes(Value[i]);
                                // string bb = System.Text.Encoding.GetEncoding("ISO8859-1").GetString(iso);
                                if (Value[i] == string.Empty)
                                    TransValue = DBNull.Value;
                                else
                                    TransValue = Value[i];
                                databaseType = MySqlDbType.VarChar;
                                break;
                            default:

                          //      if (Value[i] == string.Empty)
                            //        TransValue = DBNull.Value;
                              //  else
                                    TransValue = Value[i].ToString();
                                databaseType = MySqlDbType.VarString;
                                break;

                        }
                        Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return Cmd;
        }
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            string[] _Value = new string[Value.Length];
            Array.Copy(Value, _Value, Value.Length);
            DatabaseConnection Connection = DatabaseConnection.sql;
            
            MySqlCommand Insert_Cmd = new MySqlCommand(base.Insert_Str(Table_Name, Column_Name, ref _Value, databaseprovider, Connection), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, _Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
              //  System.Windows.Forms.MessageBox.Show(ex.ToString());
               // return;
            }
            Run_Command(Insert_Cmd);
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            string[] _Modify_Value = new string[Modify_Value.Length];
            Array.Copy(Modify_Value, _Modify_Value, Modify_Value.Length);
            DatabaseConnection Connection = DatabaseConnection.sql;
            MySqlCommand Updata_Cmd = new MySqlCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, ref _Modify_Value, Connection), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[_Modify_Value.Length + Search_value.Length];
            _Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, _Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(MySqlCommand sqlcommand)
        {
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                if (dataBaseConnection.State == ConnectionState.Open)
                    dataBaseConnection.Close();
                dataBaseConnection.Open();
                sqlcommand.ExecuteNonQuery();
                dataBaseConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            try
            {
                DatabaseConnection Connection = DatabaseConnection.oracle;
                MySqlCommand Select_Command = new MySqlCommand(base.all_dataset_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
                if (Column_Name.Length > 0)
                {

                    Select_Command = TransType(Select_Command, Value, arrayType);
                }

                return Command_Catch_data(Table_Name, Select_Command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        DataSet Command_Catch_data(string Table_Name, MySqlCommand Select_command)
        {
            DataSet Select_DataSet = new DataSet();
            MySqlDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);

                //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter = new MySqlDataAdapter(Select_command);
                //   System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                Adapter.Fill(Select_DataSet, Table_Name);
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            //     return Select_DataSet;
        }
        public override DataSet Custom_DataSet(string Table_Name, string Custom_String)
        {
            //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");

            //  dataBaseConnection.SetSessionInfo(info);
            try
            {
                MySqlCommand Select_Command = new MySqlCommand(Custom_String, dataBaseConnection);

                DataSet Select_DataSet = Command_Catch_data(Table_Name, Select_Command);


                return Select_DataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void Run_Costom_Command(string Command_String)
        {
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            MySqlCommand Costom_Command = new MySqlCommand(Command_String, dataBaseConnection);
            //System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            Run_Command(Costom_Command);
        }
        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            DatabaseConnection Connection = DatabaseConnection.oracle;
            MySqlCommand Delete_Cmd = new MySqlCommand(base.delete_str(Table_Name, Column_Name, Value, Connection), dataBaseConnection);
            try
            {
                Delete_Cmd = TransType(Delete_Cmd, Value, ValueType);
            }
            catch (Exception ex)
            {
                throw ex;
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                // return;
            }
            Run_Command(Delete_Cmd);
        }
    }
    public class Mongodb : Database
    {
        private DatabaseProvider databaseprovider;
        public MongoClient dataBaseConnection;
        static string Connecct_String = @"Provider=oraoledb.oracle;Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        IMongoDatabase DB;
        public Mongodb(string connecct_string, string DatabaseName)
        {
            Connecct_String = connecct_string;
            try
            {
                dataBaseConnection = new MongoClient(Connecct_String);
                databaseprovider = DatabaseProvider.mongodb;
                DB = dataBaseConnection.GetDatabase(DatabaseName);


                /*  int a = Connecct_String.IndexOf("Provider=");
                  string Str = Connecct_String.Remove(0, a);
                  int b = Str.IndexOf(";");
                  string Provider_Str = Str.Remove(b);
                  int c = Provider_Str.IndexOf("=");
                  string Provider = Provider_Str.Remove(0, c);
                  string Provider_lower = "";
                  for (int i = 0; i < Provider.Length; i++)
                      Provider_lower += char.ToLower(Provider[i]);
                  if (Provider_lower.IndexOf("msdaora") > 0)
                      databaseprovider = DatabaseProvider.MSDAORA;
                  else if (Provider_lower.IndexOf("oracle") > 0)
                      databaseprovider = DatabaseProvider.oracle;
                  else if (Provider_lower.IndexOf("microsoft") > 0)
                      databaseprovider = DatabaseProvider.Access;
                  else
                      databaseprovider = DatabaseProvider.SQL;
                  */
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // string bb = Provider_lower;

        }

        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public Mongodb()
        {

            dataBaseConnection = new MongoClient(Connecct_String);
            databaseprovider = DatabaseProvider.mongodb;
            DB = dataBaseConnection.GetDatabase("Testdb");

        }


        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            var collection = DB.GetCollection<BsonDocument>(Table_Name);

            if (Column_Name.Length == Value.Length)
            {
                Dictionary<string, object> InsertDictionary = new Dictionary<string, object>();
                for (int i = 0; i < Column_Name.Length; i++)
                {
                    InsertDictionary.Add(Column_Name[i], Value[i]);
                }
                BsonDocument documnt = new BsonDocument(InsertDictionary);

                collection.InsertOneAsync(documnt);
            }
            else
            {
                throw new ArgumentNullException("欄位名稱和值的數量不一致");
            }
        }
        public override void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {
            var collection = DB.GetCollection<BsonDocument>(Table_Name); //指定對collection"categories"操作  
            Dictionary<string, object> searchDictionary;
            if (Search_Column_Name != null && Search_value != null && Search_Column_Name.Length == Search_value.Length)
            {
                searchDictionary = new Dictionary<string, object>();
                for (int j = 0; j < Search_Column_Name.Length; j++)
                {
                    searchDictionary.Add(Search_Column_Name[j], Search_value[j]);
                }
            }
            else
            {
                throw new ArgumentNullException("欄位名稱和值的數量不一致或欄位NULL");
            }
            BsonDocument Searchdocumnt = new BsonDocument(searchDictionary); Dictionary<string, object> ModifyDictionary;
            if (Modify_Column_Name != null && Modify_Value != null && Modify_Column_Name.Length == Modify_Value.Length)
            {
                ModifyDictionary = new Dictionary<string, object>();
                for (int j = 0; j < Modify_Column_Name.Length; j++)
                {
                    ModifyDictionary.Add(Modify_Column_Name[j], Modify_Value[j]);
                }
            }
            else
            {
                throw new ArgumentNullException("欄位名稱和值的數量不一致或欄位NULL");
            }
            BsonDocument Modifydocumnt = new BsonDocument(ModifyDictionary);
            var result = collection.FindOneAndReplace<BsonDocument>(Searchdocumnt, Modifydocumnt);


        }

        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            var collection = DB.GetCollection<BsonDocument>(Table_Name); //指定對collection"categories"操作  
            Dictionary<string, object> selsetDictionary;
            if (Column_Name != null && Value != null && Column_Name.Length == Value.Length)
            {
                selsetDictionary = new Dictionary<string, object>();
                for (int j = 0; j < Column_Name.Length; j++)
                {
                    selsetDictionary.Add(Column_Name[j], Value[j]);
                }
            }
            else
            {
                throw new ArgumentNullException("欄位名稱和值的數量不一致或欄位NULL");
            }
            BsonDocument documnt = new BsonDocument(selsetDictionary);
            List<BsonDocument> documents = collection.Find(documnt).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();

            int i = 0;
            string json = "{\"Table1\":[";
            foreach (var doc in documents)
            {


                json += doc.ToJson();

                i++;
                if (i != documents.Count)
                {
                    json += ",";
                }

            }
            json += "]}";
            DataSet data = JsonConvert.DeserializeObject<DataSet>(json);
            return data;
        }


        public override void Delete(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            var collection = DB.GetCollection<BsonDocument>(Table_Name); //指定對collection"categories"操作  
            Dictionary<string, object> searchDictionary;
            if (Column_Name != null && Value != null && Column_Name.Length == Value.Length)
            {
                searchDictionary = new Dictionary<string, object>();
                for (int j = 0; j < Column_Name.Length; j++)
                {
                    searchDictionary.Add(Column_Name[j], Value[j]);
                }
            }
            else
            {
                throw new ArgumentNullException("欄位名稱和值的數量不一致或欄位NULL");
            }
            BsonDocument Searchdocumnt = new BsonDocument(searchDictionary); Dictionary<string, object> ModifyDictionary;

            var result = collection.DeleteManyAsync(Searchdocumnt);
        }
    }
}