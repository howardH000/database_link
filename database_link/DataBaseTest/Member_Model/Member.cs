using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Link;
using System.Data;

namespace Member_Model
{
    public class Member
    {
        Database DB;
        string Table_Name;
        string Account_Column_Name="Account";
        string Password_Column_Name="Password";
        public Member(DatabaseProvider Provider,string DataBaseLink,string table_name)
        {
            ProviderLink(Provider, DataBaseLink);
             Table_Name = table_name;
        
                }

        void ProviderLink(DatabaseProvider provider,string databaselink)
        {
            try
            {
                switch (provider)
                {
                    case DatabaseProvider.Access:
                        DB = new OLEDB(databaselink);
                        break;
                    case DatabaseProvider.SQL:
                        DB = new SQL(databaselink);
                        break;
                    case DatabaseProvider.oracle:
                        DB = new Oracle(databaselink);
                        break;
                    case DatabaseProvider.MSDAORA:
                        DB = new OLEDB(databaselink);

                        break;
                    case DatabaseProvider.MYSQL:
                        DB = new MYSQL(databaselink);

                        break;
                   
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public Member(DatabaseProvider Provider, string DataBaseLink, string table_name , string account_column_name, string password_column_name)
        {
            ProviderLink(Provider, DataBaseLink);
            Table_Name = table_name;
      
            Account_Column_Name = account_column_name;
            Password_Column_Name = password_column_name;
        }
        public bool Register(string Account, string[] Insert_Column_Name,string[] Insert_Value,DatabaseType[] Insert_Type,  out string Message)
        {
            string[] Column_Name = { Account_Column_Name };
            string[] Value = { Account };
            DatabaseType[] type = { DatabaseType.Str };
            try
            {
                if (DB.All_DataSet(Table_Name, Column_Name, Value, type).Tables[0].Rows.Count > 0)
                {
                    Message = "有此帳號";
                    return false;
                }
                else
                {
                    DB.Insert(Table_Name, Insert_Column_Name, Insert_Value, Insert_Type);
                    Message = "註冊成功";
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public object[] User_Data(string Account, string[] User_Column_Name, out string Message)
        {

            if (string.IsNullOrEmpty(Account))
            {
                Message = "帳號為空值";
                return null;
            }
            object[] UserValue = new object[User_Column_Name.Length];
          
            List<string> Column_List = new List<string>();
            List<string> Value_List = new List<string>();
            List<DatabaseType> Type_List = new List<DatabaseType>();
            Column_List.Add(Account_Column_Name);
            Value_List.Add(Account);
            Type_List.Add(DatabaseType.Str);
            string[] Column_Name = Column_List.ToArray();
            string[] Value = Value_List.ToArray();
            DatabaseType[] type = Type_List.ToArray();
            try
            {
                DataSet ds = DB.All_DataSet(Table_Name, Column_Name, Value, type);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    
                    
                        for (int i = 0; i < User_Column_Name.Length; i++)
                        {
                            UserValue[i] = ds.Tables[0].Rows[0][User_Column_Name[i]];

                        }
                        Message = "成功";
                        return UserValue;
                    
                   
                }
                else
                {
                    Message = "查無此帳號";
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Login(string Account, string Password,  out string Message)
        {
            
            List<string> Column_List = new List<string>();
            List<string> Value_List = new List<string>();
            List<DatabaseType> Type_List = new List<DatabaseType>();
            Column_List.Add(Account_Column_Name);
            Value_List.Add(Account);
            Type_List.Add(DatabaseType.Str);
            string[] Column_Name = Column_List.ToArray();
            string[] Value = Value_List.ToArray();
            DatabaseType[] type = Type_List.ToArray();
            try
            {
                if (DB.All_DataSet(Table_Name, Column_Name, Value, type).Tables[0].Rows.Count > 0)
                {

                    Column_List.Add(Password_Column_Name);
                    Value_List.Add(Password);
                    Type_List.Add(DatabaseType.Str);

                    Column_Name = Column_List.ToArray();
                    Value = Value_List.ToArray();
                    type = Type_List.ToArray();
                    DataSet ds = DB.All_DataSet(Table_Name, Column_Name, Value, type);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       
                        Message = "登入成功";
                        return "登入成功";
                    }
                    else
                    {
                        Message = "密碼錯誤";
                        return "密碼錯誤";
                    }
                }
                else
                {
                    Message = "查無此帳號";
                    return "查無此帳號";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Chang_password(string Account, string OldPassword, string NewPassword, out   string Message)
        {
            List<string> Column_List = new List<string>();
            List<string> Value_List = new List<string>();
            List<DatabaseType> Type_List = new List<DatabaseType>();
            Column_List.Add(Account_Column_Name);
            Value_List.Add(Account);
            Type_List.Add(DatabaseType.Str);
            Column_List.Add(Password_Column_Name);
            Value_List.Add(OldPassword);
            Type_List.Add(DatabaseType.Str);

            string[] Column_Name = Column_List.ToArray();
            string[] OldValue = Value_List.ToArray();
            DatabaseType[] type = Type_List.ToArray();
            try
            {
                if (DB.All_DataSet(Table_Name, Column_Name, OldValue, type).Tables[0].Rows.Count > 0)
                {

                    string[] NewColumn_Name = { Password_Column_Name };
                    string[] NewValue = { NewPassword };
                    DatabaseType[] Newtype = { DatabaseType.Str };
                    DB.Updata(Table_Name, Column_Name, OldValue, type, NewColumn_Name, NewValue, Newtype);
                    Message = "修改密碼成功";
                    return true;
                }
                else
                {
                    Message = "舊密碼錯誤";
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
