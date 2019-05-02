using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resources_demo
{
    class Program
    {
        static void Main(string[] args)
        {

            // System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            WriteText("Action_greeting");
            WriteText("Action_cancel");

            // System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            WriteText("Action_greeting");
            WriteText("Action_cancel");

            Console.Read();
        }


        private static void WriteText(string key) {
            Console.WriteLine($"[Culture: {System.Threading.Thread.CurrentThread.CurrentCulture}] Key:{key} -> value: {CustomResources.GetText(key)}");
        }
    }
}
