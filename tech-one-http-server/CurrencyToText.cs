using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tech_one_http_server
{

    public class CurrencyToText
    {
        public string Value;

        public CurrencyToText(string value)
        {
            this.Value = convertCurrencyText(value);
        }

        private string convertCurrencyText(string prString)
        {
            Console.WriteLine("Incoming Str:");
            Console.WriteLine(prString);

            var result = "";

            //Add trailing .00 if there is no ending number
            if (!prString.Contains("."))
            {
                prString = prString + ".00";
            }

            Console.WriteLine("Formatted Str:");
            Console.WriteLine(prString);

            var processedCount = 0;
            var doubleRequired = false;
            var splitProcessed = false;

            foreach (var Char in prString.Select((value, i) => new { i, value }))
            {
                var value = Char.value;
                int index = Char.i;
                int remainingCount = prString.Length - (index + 1);
                splitProcessed = false;

                if (int.Parse(remainingCount.ToString()) % 2 != 0)
                {
                    //Odd number, start on 2. Unless we have processed a split
                    if(splitProcessed == false)
                    {
                        doubleRequired = true;
                    }
                    Console.WriteLine("Double Required");
                }
                if (int.Parse(remainingCount.ToString()) % 2 == 0)
                {
                    //Even number, start on 2. Unless we have processed a split
                    doubleRequired = false;
                    Console.WriteLine("Single Required");
                }

                Console.WriteLine(index.ToString() + " Str: " + value.ToString());
                if (processedCount <= index)
                {
                    //is the number even or odd? 
                    if (value.ToString() == ".")
                    {
                        Console.WriteLine("decimal");
                        result += "DOLLARS";
                        doubleRequired = true;
                        processedCount++;
                    }
                    else if (value.ToString() == "0")
                    {
                        processedCount++;
                    }
                    else if (doubleRequired == false)
                    {
                        Console.WriteLine("single no");
                        //Even Number, process 1
                        result += singleDigit(value.ToString());
                        processedCount++;
                        doubleRequired = true;
                    }
                    else if (doubleRequired == true)
                    {
                        Console.WriteLine("double no");
                        //Odd Number, process 2
                        result += doubleDigit(value.ToString() + prString[index + 1]);
                        processedCount += 2;
                        doubleRequired = false;
                    }
                }

                //Calculate thousands
                if (remainingCount == 6 && splitProcessed == false)
                {
                    result += " THOUSAND ";
                    splitProcessed = true;
                }
                else if (remainingCount == 5 && splitProcessed == false)
                {
                    result += " HUNDRED ";
                    splitProcessed = true;
                }
                else if (remainingCount == 0 && !prString.EndsWith(".00"))
                {
                    result += " CENTS";
                }
                Console.WriteLine("Loop Result " + result);
            }

            Console.WriteLine("Result: " + result);
            return result;
        }

        private string singleDigit(string prString)
        {
            var result = "";

            if(prString == "1")
            {
                result = "ONE ";
            }
            else if(prString == "2")
            {
                result = "TWO ";
            }
            else if (prString == "3")
            {
                result = "THREE ";
            }
            else if (prString == "4")
            {
                result = "FOUR ";
            }
            else if (prString == "5")
            {
                result = "FIVE ";
            }
            else if (prString == "6")
            {
                result = "SIX ";
            }
            else if (prString == "7")
            {
                result = "SEVEN ";
            }
            else if (prString == "8")
            {
                result = "EIGHT ";
            }
            else if (prString == "9")
            {
                result = "NINE ";
            }

            return result;
        }
        private string doubleDigit(string prString)
        {
            var result = "";
            //Teens
            if (prString == "11")
            {
                return "ELEVEN ";
            }
            else if (prString == "12")
            {
                return "TWELVE ";
            }
            else if(prString == "13")
            {
                return "THIRTEEN ";
            }
            else if(prString == "14")
            {
                return "FOURTEEN ";
            }
            else if(prString == "15")
            {
                return "FIFTEEN ";
            }
            else if(prString == "16")
            {
                return "SIXTEEN ";
            }
            else if(prString == "17")
            {
                return "SEVENTEEN ";
            }
            else if(prString == "18")
            {
                return "EIGHTEEN ";
            }
            else if(prString == "19")
            {
                return "NINTEEN ";
            }
            //Tens
            else if(prString.StartsWith("2"))
            {
                result = "TWENTY ";
            }
            else if (prString.StartsWith("3"))
            {
                result = "THIRTY ";
            }
            else if (prString.StartsWith("4"))
            {
                result = "FOURTY ";
            }
            else if (prString.StartsWith("5"))
            {
                result = "FIFTY ";
            }
            else if (prString.StartsWith("6"))
            {
                result = "SIXTY ";
            }
            else if (prString.StartsWith("7"))
            {
                result = "SEVENTY ";
            }
            else if (prString.StartsWith("8"))
            {
                result = "EIGHTTY ";
            }
            else if (prString.StartsWith("9"))
            {
                result = "NINETY ";
            }

            result += singleDigit(prString[1].ToString());

            return result;
        }
    }
}
