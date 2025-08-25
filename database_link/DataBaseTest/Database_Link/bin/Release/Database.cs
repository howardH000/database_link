using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Data.OleDb;
using Oracle.DataAccess.Client;
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
        Bool

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
    public class Database
    {
        public virtual void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {

        }
        protected string Insert_Str(string Table_Name, string[] Column_Name, string[] Value)
        {
            string insert_str = @"insert into [";
            insert_str += Table_Name;
            insert_str += "] (";
            for (int i = 0; i < Column_Name.Length; i++)
            {
                insert_str += "[" + Column_Name[i].Trim() + "]";
                if (i != Column_Name.Length - 1)
                    insert_str += ",";
            }
            insert_str += ") values (";
            for (int i = 0; i < Value.Length; i++)
            {
                insert_str += "@Value" + i.ToString();
                //         SQL_Insert_str += "@"+Value[i];
                if (i != Value.Length - 1)
                    insert_str += ",";
            }
            insert_str += ")";
            return insert_str;
        }

        public virtual void Updata(string Table_Name, string[] Search_Column_Name, string[] Search_value, DatabaseType[] Search_Type, string[] Modify_Column_Name, string[] Modify_Value, DatabaseType[] Modify_Type)
        {



        }
        protected string Updata_Str(string Table_Name, string[] Search_Column_Name, string[] Search_value, string[] Modify_Column_Name, string[] Modify_Value)
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
            return Updata_str;
        }
        public virtual DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {

            DataSet Select_DataSet = new DataSet();
            OleDbDataAdapter Adapter;
            DataTable Table = new DataTable();
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

            //OleDbCommand Select_Command = new OleDbCommand(Select_String, dataBaseConnection);
            if (Column_Name.Length > 0)
            {

                //   Select_Command = TransType(Select_Command, Value, arrayType);
            }
          //  Adapter = new OleDbDataAdapter(Select_Command);

            //Adapter.Fill(Select_DataSet, Table_Name);
            return Select_DataSet;
        }
        protected string all_dataset_str(string Table_Name, string[] Column_Name, string[] Value)
        {
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
            return Select_String;
        }
    }
    public class SQL : Database
    {
        public SqlConnection dataBaseConnection;
        static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public SQL(string Connecct_String)
        {
            SQL_Connecct_String = Connecct_String;
            dataBaseConnection = new SqlConnection(Connecct_String);
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public SQL()
        {
            dataBaseConnection = new SqlConnection(SQL_Connecct_String);
        }
        SqlCommand TransType(SqlCommand Cmd, string[] Value, DatabaseType[] Type)
        {

            for (int i = 0; i < Value.Length; i++)
            {
                object TransValue;
                SqlDbType databaseType;
                switch (Type[i])
                {
                    case DatabaseType.Int:

                        TransValue = Convert.ToInt32(Value[i]);
                        databaseType = SqlDbType.Int;
                        break;

                    case DatabaseType.Double:
                        TransValue = Convert.ToDouble(Value[i]);
                        databaseType = SqlDbType.Float;
                        break;
                    case DatabaseType.DateTime:
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        databaseType = SqlDbType.DateTime;
                        // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case DatabaseType.Date:
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                        databaseType = SqlDbType.Date;
                        break;
                    case DatabaseType.Bool:
                        if (Value[i] == "true")
                            TransValue = 1;
                        else
                            TransValue = 0;
                        databaseType = SqlDbType.Bit;

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
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            SqlCommand Insert_Cmd = new SqlCommand(base.Insert_Str(Table_Name, Column_Name, Value), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, Value, ValueType);
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

            SqlCommand Updata_Cmd = new SqlCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, Modify_Value), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[Modify_Value.Length + Search_value.Length];
            Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return;
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
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {                      
            SqlCommand Select_Command = new SqlCommand(base.all_dataset_str(Table_Name,  Column_Name, Value), dataBaseConnection);
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
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                
            }
            return Select_DataSet;
        }
    }
    public class Access : Database
    {
        public OleDbConnection dataBaseConnection;
        static string SQL_Connecct_String = @"Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
        ///<summary>
        ///
        /// 摘要:
        ///資料庫SQL    
        ///
        ///
        /// </summary>
        ///<param name="Connecct_String">資料庫連接字串</param>
        public Access(string Connecct_String)
        {
            SQL_Connecct_String = Connecct_String;
            dataBaseConnection = new OleDbConnection(Connecct_String);
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public Access()
        {
            dataBaseConnection = new OleDbConnection(SQL_Connecct_String);
        }
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
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        databaseType = OleDbType.DBTimeStamp;
                        // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case DatabaseType.Date:
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                        databaseType = OleDbType.DBDate;
                        break;
                    case DatabaseType.Bool:
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
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            OleDbCommand Insert_Cmd = new OleDbCommand(base.Insert_Str(Table_Name, Column_Name, Value), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, Value, ValueType);
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

            OleDbCommand Updata_Cmd = new OleDbCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, Modify_Value), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[Modify_Value.Length + Search_value.Length];
            Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return;
            }
            Run_Command(Updata_Cmd);

        }
        void Run_Command(OleDbCommand sqlcommand)
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
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            OleDbCommand Select_Command = new OleDbCommand(base.all_dataset_str(Table_Name, Column_Name, Value), dataBaseConnection);
            if (Column_Name.Length > 0)
            {

                Select_Command = TransType(Select_Command, Value, arrayType);
            }

            return Command_Catch_data(Table_Name, Select_Command);
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
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());

            }
            return Select_DataSet;
        }
    }
    public class Oracle : Database
    {
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
        public Oracle(string Connecct_String)
        {
            Oracle_Connecct_String = Connecct_String;
            dataBaseConnection = new OracleConnection(Connecct_String);
        }
        ///<summary>
        /// 摘要:
        ///資料庫SQL    
        /// </summary>
        public Oracle()
        {
            dataBaseConnection = new OracleConnection(Oracle_Connecct_String);
        }
        OracleCommand TransType(OracleCommand Cmd, string[] Value, DatabaseType[] Type)
        {

            for (int i = 0; i < Value.Length; i++)
            {
                object TransValue;
                OracleDbType databaseType;
                switch (Type[i])
                {
                    case DatabaseType.Int:

                        TransValue = Convert.ToInt32(Value[i]);
                        databaseType = OracleDbType.Int32;
                        break;

                    case DatabaseType.Double:
                        TransValue = Convert.ToDouble(Value[i]);
                        databaseType = OracleDbType.Double;
                        break;
                    case DatabaseType.DateTime:
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        databaseType = OracleDbType.Date;
                        // TransValue= DateTime.ParseExact(Value[i], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        //   TransValue = Convert.ToDateTime(Value[i]).Date.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case DatabaseType.Date:
                        TransValue = DateTime.ParseExact(Value[i], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("yyyy-MM-dd");
                        databaseType = OracleDbType.Date;
                        break;
                    case DatabaseType.Bool:
                        if (Value[i] == "true")
                            TransValue = 1;
                        else
                            TransValue = 0;
                        databaseType = OracleDbType.Boolean;

                        break;
                    default:
                        TransValue = Value[i];
                        databaseType = OracleDbType.NVarchar2;
                        break;

                }
                Cmd.Parameters.Add("@Value" + i.ToString(), databaseType).Value = TransValue;

            }


            return Cmd;
        }
        public override void Insert(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] ValueType)
        {
            OracleCommand Insert_Cmd = new OracleCommand(base.Insert_Str(Table_Name, Column_Name, Value), dataBaseConnection);
            try
            {
                Insert_Cmd = TransType(Insert_Cmd, Value, ValueType);
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

            OracleCommand Updata_Cmd = new OracleCommand(base.Updata_Str(Table_Name, Search_Column_Name, Search_value, Modify_Column_Name, Modify_Value), dataBaseConnection);
            DatabaseType[] arrayType = new DatabaseType[Modify_Value.Length + Search_value.Length];
            string[] Value = new string[Modify_Value.Length + Search_value.Length];
            Modify_Value.CopyTo(Value, 0);
            Search_value.CopyTo(Value, Modify_Value.Length);
            Modify_Type.CopyTo(arrayType, 0);
            Search_Type.CopyTo(arrayType, Modify_Type.Length);
            try
            {
                Updata_Cmd = TransType(Updata_Cmd, Value, arrayType);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return;
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
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public override DataSet All_DataSet(string Table_Name, string[] Column_Name, string[] Value, DatabaseType[] arrayType)
        {
            OracleCommand Select_Command = new OracleCommand(base.all_dataset_str(Table_Name, Column_Name, Value), dataBaseConnection);
            if (Column_Name.Length > 0)
            {

                Select_Command = TransType(Select_Command, Value, arrayType);
            }

            return Command_Catch_data(Table_Name, Select_Command);
        }
        DataSet Command_Catch_data(string Table_Name, OracleCommand Select_sqlcommand)
        {
            DataSet Select_DataSet = new DataSet();
            OracleDataAdapter Adapter;
            try
            {
                //  Inser_Command = TransType(Inser_Command, Value, ValueType);


                Adapter = new OracleDataAdapter(Select_sqlcommand);

                Adapter.Fill(Select_DataSet, Table_Name);
                return Select_DataSet;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());

            }
            return Select_DataSet;
        }
    }
}  
