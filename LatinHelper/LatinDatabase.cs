using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace LatinHelper
{
    public class LatinDatabase
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("LatinDataBase - Julius Herb (julius-herb.tk)");
            Console.WriteLine("You are in the admin area - be careful!");
            while (true)
            {
                try
                {
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("What do you want to do?");
                    Console.WriteLine();
                    Console.WriteLine("0.) JIT-Compiler");
                    Console.WriteLine("1.) Create Output from Text");
                    Console.WriteLine("2.) Create Output from List");
                    Console.WriteLine("3.) Create Output from NotFound");
                    Console.WriteLine("4.) Create Dict for Upload");
                    Console.WriteLine("5.) Create Dict from All");
                    Console.WriteLine("6.) Create Dict from All Lists");
                    Console.WriteLine("7.) Create List from Dict");
                    Console.WriteLine("8.) Leave Admin Area");
                    newtry:
                    char ch = Console.ReadKey(true).KeyChar;
                    Console.WriteLine();
                    switch (ch)
                    {
                        case '0':
                            RunCostumCode();
                            break;
                        case '1':
                            CreateOutputFromText();
                            break;
                        case '2':
                            CreateOutputFromList();
                            break;
                        case '3':
                            CreateOutputFromNotFound();
                            break;
                        case '4':
                            CreateUploadDict();
                            break;
                        case '5':
                            CreateDictFromAll();
                            break;
                        case '6':
                            CreateDictFromAllList();
                            break;
                        case '7':
                            CreateListFromDict();
                            break;
                        case '8':
                            Program.Main();
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
        static void CreateListFromDict()
        {
            Console.WriteLine("Enter Dict Path:");
            string dictpath = Console.ReadLine();
            Console.WriteLine("Enter iunction pattern:");
            string pattern = Console.ReadLine();
            Console.WriteLine("Enter Output Path:");
            string outputpath = Console.ReadLine();
            string[] voklist = Directory.GetFiles(dictpath);
            string ret = "";
            foreach (string vok in voklist)
            {
                string vokabel = StringOperations.StringAb(dictpath, vok);
                vokabel = StringOperations.StringBis(".", vokabel);
                ret += vokabel + pattern;
            }
            File.WriteAllText(outputpath, ret);
        }
        static void CreateDictList()
        {
            Console.WriteLine("Enter Dict Path:");
            string dictpath = Console.ReadLine();
            Console.WriteLine("Enter iunction pattern:");
            string pattern = Console.ReadLine();
            Console.WriteLine("Enter Output Path:");
            string outputpath = Console.ReadLine();
            string[] voklist = Directory.GetFiles(dictpath);
            string ret = "";
            foreach (string vok in voklist)
            {
                string vokabel = StringOperations.StringAb(dictpath, vok);
                vokabel = StringOperations.StringBis(".", vokabel);
                ret += vokabel + pattern;
            }
            File.WriteAllText(outputpath, ret);
        }
        public static string[] GetVokArray()
        {
            string[] voklist = Directory.GetFiles(LatinDict.DictPath);
            return voklist;
        }
        public static string GetRandomVok()
        {
            string[] voklist = Directory.GetFiles(LatinDict.DictPath);
            Random r = new Random();
            string ret = voklist[r.Next(voklist.Length)];
            ret = StringOperations.StringAb(LatinDict.DictPath, ret);
            ret = StringOperations.StringBis(".", ret);
            return ret;
        }
        static string aktuelleVok = "";
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!aktuelleVok.Equals(""))
            {
                Console.WriteLine(aktuelleVok);
            }
        }
        static void CreateDictFromAllList()
        {
            string[] files = Directory.GetFiles(@"C:\Latein\Listen\");
            foreach (string path in files)
            {
                string line = "";
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            if (File.Exists(@"C:\Latein\output\" + line + ".txt"))
                            {
                                Console.WriteLine("File exists!");
                            }
                            else
                            {
                                aktuelleVok = line;
                                Wort wwort = new Wort();
                                wwort = LatinDict.getVok(line);
                                wwort.Latein = line;
                                string output = wwort.OutputNew();
                                System.IO.File.WriteAllText(@"C:\Latein\dict\" + line + ".txt", output);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("The file could not be read:");
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
        static void CreateDictFromAll()
        {
            string[] files = Directory.GetFiles(@"C:\Latein\Texte\");
            foreach (string path in files)
            {
                string line = "";
                Console.WriteLine("Enter Output Path:");
                string errorpath = @"C:\Latein\error.txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    line = sr.ReadToEnd();
                }
                line = line.Replace(".", "");
                line = line.Replace(":", "");
                line = line.Replace(",", "");
                line = line.Replace("?", "");
                line = line.Replace("!", "");
                line = line.Replace("-", "");
                line = line.Replace(";", "");
                line = line.Replace("[", "");
                line = line.Replace("]", "");
                line = line.Replace("1", "");
                line = line.Replace("2", "");
                line = line.Replace("3", "");
                line = line.Replace("4", "");
                line = line.Replace("5", "");
                line = line.Replace("6", "");
                line = line.Replace("7", "");
                line = line.Replace("8", "");
                line = line.Replace("9", "");
                line = line.Replace("0", "");
                line = line.Replace("(", "");
                line = line.Replace(")", "");
                line = line.Replace("'", "");
                line = line.Replace("\n", "");
                string[] strarray = line.Split(' ');
                string errorvok = "";
                foreach (string lline in strarray)
                {
                    try
                    {
                        string llline = lline.Trim();
                        errorvok = llline;
                        if (false)//File.Exists(@"C:\Latein\output\" + llline + ".txt"))
                        {
                            //Console.WriteLine(llline + ": File exists!");
                        }
                        else
                        {

                            System.Console.WriteLine(llline);
                            Wort wwort = new Wort();
                            wwort = LatinDict.getVok(llline);
                            wwort.Latein = llline;
                            string output = wwort.OutputNew();
                            System.IO.File.WriteAllText(@"C:\Latein\dict\" + llline + ".txt", output);
                            //String html = LatinDict.getHTML(llline);
                            //String[] arr = LatinDict.getStringArray(html);
                            //String output = LatinDict.Output(arr);
                            /*if (!output.Contains("NOT_AVAILABLE"))
                            {
                                System.IO.File.WriteAllText(@"C:\Latein\output\" + llline + ".txt", output);
                            }
                            else
                            {
                                File.AppendAllText(@"C:\Latein\notfound.txt", llline+";");
                            }*/
                        }
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText(errorpath, errorvok + " ");
                        //Console.WriteLine("Fehler!");
                        //Console.WriteLine(e.Message);
                    }
                }
            }
        }
        static void CreateOutputFromText()
        {
            string line = "";
            Console.WriteLine("Enter Text Path:");
            string path = Console.ReadLine();
            Console.WriteLine("Enter Error Path:");
            string errorpath = Console.ReadLine();
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

            using (StreamReader sr = new StreamReader(path))
            {
                line = sr.ReadToEnd();
            }
            line = line.Replace(".", "");
            line = line.Replace(":", "");
            line = line.Replace(",", "");
            line = line.Replace("?", "");
            line = line.Replace("!", "");
            line = line.Replace("-", "");
            line = line.Replace(";", "");
            line = line.Replace("[", "");
            line = line.Replace("]", "");
            line = line.Replace("1", "");
            line = line.Replace("2", "");
            line = line.Replace("3", "");
            line = line.Replace("4", "");
            line = line.Replace("5", "");
            line = line.Replace("6", "");
            line = line.Replace("7", "");
            line = line.Replace("8", "");
            line = line.Replace("9", "");
            line = line.Replace("0", "");
            line = line.Replace("(", "");
            line = line.Replace(")", "");
            line = line.Replace("'", "");
            line = line.Replace("\n", "");
            string[] strarray = line.Split(' ');
            string errorvok = "";
            foreach (string lline in strarray)
            {
                try
                {
                    string llline = lline.Trim();
                    errorvok = llline;
                    if (File.Exists(@"C:\Latein\dict\" + llline + ".txt"))
                    {
                        //Console.WriteLine(llline + ": File exists!");
                    }
                    else
                    {

                        aktuelleVok = llline;
                        Wort wwort = new Wort();
                        wwort = LatinDict.getVok(llline);
                        wwort.Latein = llline;
                        string output = wwort.OutputNew();
                        System.IO.File.WriteAllText(@"C:\Latein\dict\" + llline + ".txt", output);
                        //String html = LatinDict.getHTML(llline);
                        //String[] arr = LatinDict.getStringArray(html);
                        //String output = LatinDict.Output(arr);
                        /*if (!output.Contains("NOT_AVAILABLE"))
                        {
                            System.IO.File.WriteAllText(@"C:\Latein\output\" + llline + ".txt", output);
                        }
                        else
                        {
                            File.AppendAllText(@"C:\Latein\notfound.txt", llline+";");
                        }*/
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(errorpath, errorvok + " ");
                    //Console.WriteLine("Fehler!");
                    //Console.WriteLine(e.Message);
                }
            }

        }
        static void CreateOutputFromNotFound()
        {
            string line = "";
            Console.WriteLine("Enter Text Path:");
            string path = Console.ReadLine();
            Console.WriteLine("Enter Output Path:");
            string outputpath = Console.ReadLine();
            Console.WriteLine("Enter Error Path:");
            string errorpath = Console.ReadLine();
            using (StreamReader sr = new StreamReader(path))
            {
                line = sr.ReadToEnd();
            }
            line = line.Replace(".", "");
            line = line.Replace(":", "");
            line = line.Replace(",", "");
            line = line.Replace("?", "");
            line = line.Replace("!", "");
            line = line.Replace("-", "");
            line = line.Replace("[", "");
            line = line.Replace("]", "");
            line = line.Replace("1", "");
            line = line.Replace("2", "");
            line = line.Replace("3", "");
            line = line.Replace("4", "");
            line = line.Replace("5", "");
            line = line.Replace("6", "");
            line = line.Replace("7", "");
            line = line.Replace("8", "");
            line = line.Replace("9", "");
            line = line.Replace("0", "");
            line = line.Replace("(", "");
            line = line.Replace(")", "");
            line = line.Replace("'", "");
            line = line.Replace("\n", "");
            string[] strarray = line.Split(';');
            string errorvok = "";
            foreach (string lline in strarray)
            {
                try
                {
                    string llline = lline.Trim();
                    errorvok = llline;
                    if (File.Exists(@"C:\Latein\output\" + llline + ".txt"))
                    {
                        //Console.WriteLine(llline + ": File exists!");
                    }
                    else
                    {

                        System.Console.WriteLine(llline);
                        Wort wwort = new Wort();
                        wwort = LatinDict.getVok(llline);
                        wwort.Latein = llline;
                        string output = wwort.OutputNew();
                        System.IO.File.WriteAllText(@"C:\Latein\dict\" + llline + ".txt", output);
                        //String html = LatinDict.getHTML(llline);
                        //String[] arr = LatinDict.getStringArray(html);
                        //String output = LatinDict.Output(arr);
                        /*if (!output.Contains("NOT_AVAILABLE"))
                        {
                            System.IO.File.WriteAllText(@"C:\Latein\output\" + llline + ".txt", output);
                        }
                        else
                        {
                            File.AppendAllText(@"C:\Latein\notfound.txt", llline+";");
                        }*/
                    }
                }
                catch (Exception e)
                {
                    File.AppendAllText(errorpath, errorvok + " ");
                    Console.WriteLine("Fehler!");
                    //Console.WriteLine(e.Message);
                }
            }

        }
        static void CreateOutputFromList()
        {
            string line = "";
            Console.WriteLine("Enter Path:");
            string path = Console.ReadLine();
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        if (File.Exists(@"C:\Latein\output\" + line + ".txt"))
                        {
                            Console.WriteLine("File exists!");
                        }
                        else
                        {
                            System.Console.WriteLine(line);
                            Wort wwort = new Wort();
                            wwort = LatinDict.getVok(line);
                            wwort.Latein = line;
                            string output = wwort.OutputNew();
                            System.IO.File.WriteAllText(@"C:\Latein\dict\" + line + ".txt", output);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        static void CreateUploadDict()
        {
            string line = "";
            Console.WriteLine("Old Dict Path:");
            string backuppath = Console.ReadLine();
            Console.WriteLine("New Dict Path:");
            string newdictpath = Console.ReadLine();
            Console.WriteLine("Output Path:");
            string outputpath = Console.ReadLine();
            Console.WriteLine("New Version:");
            string vversion = Console.ReadLine();
            LatinDict.CreateDict(backuppath, newdictpath, outputpath, vversion);
        }
        static void CreateDictFromOutput()
        {
            Console.Write("Wortliste wird geladen... ");
            string[] files = Directory.GetFiles(@"C:\Latein\output\");
            Console.WriteLine("fertig!");
            foreach (string path in files)
            {
                string vok = StringOperations.StringAb(@"output\", path);
                vok = StringOperations.StringBis(".", vok);
                Wort w = LatinDict.getVokabel(vok, false);
                string output = w.OutputNew();
                Console.WriteLine(vok);
                System.IO.File.WriteAllText(@"C:\Latein\dict\" + vok + ".txt", output);
            }
        }
        static void Main5(string[] args)
        {
            string vok = "exercitus";
            Wort w = LatinDict.getVokabel(vok, false);
            string output = w.OutputNew();
            Console.WriteLine(vok);
            System.IO.File.WriteAllText(@"C:\Latein\dict\" + vok + ".txt", output);
        }
        static void Main7(string[] args)
        {
            Console.Write("Wortliste wird geladen... ");
            string[] files = Directory.GetFiles(@"C:\Latein\output\");
            Console.WriteLine("fertig!");
            string ret = "";
            foreach (string path in files)
            {
                string vok = StringOperations.StringAb(@"output\", path);
                vok = StringOperations.StringBis(".", vok);
                Wort w = LatinDict.getVokabel(vok, false);
                string output = w.OutputNew();
                Console.WriteLine(vok);
                ret += output + "&&";
            }
            System.IO.File.WriteAllText(@"C:\Latein\dict\dictall.txt", ret);
        }
        static void Main6(string[] args)
        {
            Console.Write("Wortliste wird geladen... ");
            string[] files = Directory.GetFiles(@"C:\Latein\output\");
            Console.WriteLine("fertig!");
            string ret = "";
            foreach (string path in files)
            {
                string vok = StringOperations.StringAb(@"output\", path);
                vok = StringOperations.StringBis(".", vok);
                ret += vok + "#";
            }
            System.IO.File.WriteAllText(@"C:\Latein\dictall.txt", ret);
        }
        static void RunCostumCode()
        {
            bool advanced = false;
            Console.WriteLine("Select Mode:");
            Console.WriteLine("1) Easy");
            Console.WriteLine("2) Advanced");
            if(Console.ReadKey(true).KeyChar=='2')
            {
                advanced = true;
            }
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            string Output = "JIT_" + StringOperations.GetMacAddress() + "_" + StringOperations.GetTimestamp(DateTime.Now) + ".exe";
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add(StringOperations.GetApplicationsFilePath());
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            string code = "using System; namespace HelloWorld { class HelloWorldClass { static void Main(string[] args) { Console.WriteLine(\"Hello World!\"); Console.ReadLine(); } } }";
            code = "using System; using LatinHelper; using System.IO; namespace HelloWorld { class HelloWorldClass { static void Main(string[] args) { ";
            if(advanced)
            {
                code = "";
            }
            while(true)
            {
                string line = Console.ReadLine();
                if(line == "@@exit@@")
                {
                    if (!advanced)
                    {
                        code += " } } }";
                    }
                    break;
                }
                else
                {
                    code += line + " " + Environment.NewLine;
                }
            }
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    Console.WriteLine(
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";");
                }
            }
            else
            {
                //Successful Compile
                //If we clicked run then launch our EXE
                Process.Start(Output);
            }


        }
    }
}
