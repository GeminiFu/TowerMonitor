using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TowerMonitor.util
{
    public class StringUtil
    {
        public static bool isEmpty(String data)
        {
            return String.IsNullOrEmpty(data);
        }

        public static bool isEmail(String data)
        {
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regex.IsMatch(data);
        }

        public static bool isGuid(string data)
        {
            Guid x;

            return Guid.TryParse(data, out x);
        }

        public static bool isInteger(string data) {
            int x;
            return int.TryParse(data, out x);
        }

        /*
        public static int getAsciiNumber(Char character)
        {
            return (int)character;
        }
        public static String getAsciiString(int number) {
            return ((char)number).ToString();
        }
         */

        public static char getCharacter(int number)
        {
            return Convert.ToChar(number);
        }

        public static int getASCII(string S)
        {
            return Convert.ToInt32(S[0]);
        }

        public static int getASCII(char C)
        {
            return Convert.ToInt32(C);
        }
      
        public static String getRandomString(int length)
        {
            String randomString = "";
            char[] randomArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                randomArray[i] = getRandomChar();

                for (int j = 0; j < i; j++)
                {
                    while (randomArray[j] == randomArray[i])    //檢查是否與前面產生的數值發生重複，如果有就重新產生
                    {
                        j = 0;  //如有重複，將變數j設為0，再次檢查 (因為還是有重複的可能)
                        randomArray[i] = getRandomChar();
                    }
                }
            }

            for (int j = 0; j < length; j++)
            {
                randomString += randomArray[j].ToString();
            }

            return randomString;
        }

        // Only get the english upper/lower case and number
        public static char getRandomChar()
        {
            Random letter = new Random();       // English letter
            Random letterCase = new Random();   // Upper Case / Lower Case
            char c = new char();

            switch (letterCase.Next(0, 3))
            {
                case 1: // Upper Case
                    c = (char)letter.Next(65, 91);
                    break;

                case 2: // Lower Caser
                    c = (char)letter.Next(97, 113);
                    break;

                default:    // number;
                    c = (char)letter.Next(48, 58);
                    break;

            }

            return c;
        }

        public static String getFilNameExtension(String data)
        {
            String[] stringArray = data.Split('.');
            if (stringArray.Length > 0)
            {
                return stringArray.Last();
            }

            return "";

        }
    }
}
