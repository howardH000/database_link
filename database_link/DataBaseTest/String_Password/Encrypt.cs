using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace String_Password
{
   public class Encrypt
    {
       const int Basic_code = 29;
      public static void EncryptedText(string file,string OrginText)
       {
           byte[] GetBytes = ASCIIEncoding.UTF8.GetBytes(OrginText);
           byte b_code = (byte)(GetBytes.Length % Basic_code);
           for (int i = 0; i < GetBytes.Length; i++)
           {
              
               GetBytes[i] += b_code;
               GetBytes[i] = (byte)(~GetBytes[i]);
               GetBytes[i] += b_code;
           }
           File.WriteAllBytes(file, GetBytes);
       }
      public static string DecryptText(string file)
      {
          byte[] GetBytes = File.ReadAllBytes(file);
          byte b_code = (byte)(GetBytes.Length % Basic_code);
          for (int i = 0; i < GetBytes.Length; i++)
          {

              GetBytes[i] -= b_code;
              GetBytes[i] = (byte)(~GetBytes[i]);
              GetBytes[i] -= b_code;
          }
          return ASCIIEncoding.UTF8.GetString(GetBytes);
      }
    }
}
