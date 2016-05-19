using System;
using System.Diagnostics;

namespace LatinHelper
{
    class Program
    {
        public static void Main()
        {
            LatinDict.CreateEnvironment();
            Console.Clear();
            Console.ResetColor();
            Console.Title = "Latin Helper - Julius Herb";
            Console.WriteLine("LatinHelper (" + Version.branch + " branch; built on " + Version.buildDate() + ")");
            Console.WriteLine("by Julius Herb (julius-herb.tk)");
            while (true)
            {
                try
                {
                    
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("What do you want to do?");
                    Console.WriteLine();
                    Console.WriteLine("1.) Define a latin word (Ein lateinisches Wort bestimmen)");
                    Console.WriteLine("2.) Group latin words by KNG (Lateinische Wörter nach KNG gruppieren)");
                    Console.WriteLine("3.) Download recent latin dict (Das lateinisches Wörterbuch aktualisieren)");
                    Console.WriteLine("4.) Enter Admin Area");
                newtry:
                    char ch = Console.ReadKey(true).KeyChar;
                    Console.WriteLine();
                    switch (ch)
                    {
                        case '1':
                            OutputMain();
                            break;
                        case '2':
                            KNG_Group_Main();
                            break;
                        case '3':
                            DownloadDict(true);
                            break;
                        case '4':
                            Console.WriteLine("Enter Password:");
                            String pass = "";
                            while (true)
                            {
                                ConsoleKeyInfo cki = Console.ReadKey(true);
                                if (cki.Key == ConsoleKey.Enter)
                                {
                                    break;
                                }
                                pass += cki.KeyChar;
                            }
                            if (pass.Equals("latein"))
                            {
                                LatinDatabase.Main(new String[] { });
                            }
                            else
                            {
                                Console.WriteLine("Invalid Password - Access denied.");
                            }
                            break;
                        default:
                            goto newtry;
                    }
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurs. (Ein Fehler ist aufgetreten.)");
                    Console.WriteLine("Display detail information? (Details anzeigen?) - (y/n)");
                    if (Console.ReadKey(true).KeyChar == 'y')
                    {
                        Console.WriteLine(e.Data + ";" + e.Message + ";" + e.Source + ";" + e.ToString());
                    }
                }
            }
        }
        public static void DownloadDict(bool manual)
        {
            if (manual)
            {
                Console.WriteLine("Do you want to Update the Dictionary? (y/n)");
                if (Console.ReadKey(true).KeyChar == 'y')
                {
                    Console.Clear();
                    Console.WriteLine("Update is running...");
                    LatinDict.RefreshDict(manual);
                    Console.WriteLine("Update finished!");
                }
            }
            else
            {
                LatinDict.RefreshDict(manual);
            }
        }
        static void OutputMain()
        {
            Console.WriteLine("Enter Latin word:");
            String vok = Console.ReadLine();
            Wort w = LatinDict.getVok(vok);
            String output = w.Output();
            Console.WriteLine(output);
        }
        static void KNG_Group_Main()
        {
            Satz satz;
            string lateintext = "Senatores mala magnus alti arbor";
            Stopwatch zeit = new Stopwatch();
            satz = new Satz();
            Console.WriteLine("Enter latin words to group them by KNG");
            lateintext = Console.ReadLine();
            zeit.Start();
            satz._satz = lateintext;
            satz.Sortieren();
            string output = satz.OutputWortGruppen(true);
            zeit.Stop();
            Console.WriteLine();
            Console.WriteLine("Your result: (calculation time: " + zeit.Elapsed + ")");
            Console.WriteLine();
            Console.WriteLine(output);
        }
        static void testMultiWords()
        {
            LatinDict.getVokabel("arbor", true);
            Console.ReadKey(true);
        }
        static void testMultiWords2()
        {
            String html = LatinDict.getHTML("vocari");
            Console.WriteLine(html);

            Console.ReadKey(true);
        }
    }
}
