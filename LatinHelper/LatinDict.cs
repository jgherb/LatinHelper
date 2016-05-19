using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace LatinHelper
{
    public class LatinDict
    {
        public static string DictServer = "http://navigium.de/latein-woerterbuch.php?form=";
        public static string DictServer_fc = "http://www.frag-caesar.de/lateinwoerterbuch/";
        public static string DictPath = @"C:\Latein\dictionary\";
        public static string LatinPath = @"C:\Latein\";
        public static string DictServerEnd = "";
        public static string DictServerEnd_fc = "-uebersetzung.html";
        public static string DictPathEnd = @".txt";
        public static string RefreshServer = "http://julius-herb.lima-city.de/latein/";
        static WebClient wc;
        public static string actualVok = "";
        public static void CreateEnvironment()
        {
            string location = "";
            int counter_restart = 0;
            try {
                Console.WriteLine("Anwendung wird gestartet...");
                location = StringOperations.GetApplicationsPath();
                location = StringOperations.StringBis(":", location);
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.Name.Contains(location))
                    {
                        if (d.DriveFormat.Contains("FAT"))
                        {
                            Console.Clear();
                            Console.WriteLine("Das Programm wurde von einem Laufwerk gestartet,\ndas nicht mit NTFS formatiert wurde.");
                            Console.WriteLine("Das wird in dieser Version leider noch nicht unterstützt.");
                            Console.WriteLine("Kopieren sie die Anwendung auf eine NTFS-Festplatte und starten sie erneut.");
                            Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden.");
                            Console.ReadKey(true);
                            Environment.Exit(0);
                        }
                    }
                }

                DictPath = location + @":\Latein\dictionary\";
                LatinPath = location + @":\Latein\";
                if (!Directory.Exists(LatinPath))
                {
                    Directory.CreateDirectory(LatinPath);
                }
                if (!Directory.Exists(location + @":\Latein\dictionary\"))
                {
                    Directory.CreateDirectory(location + @":\Latein\dictionary\");
                }
                if (!Directory.Exists(location + @":\Latein\output\"))
                {
                    Directory.CreateDirectory(location + @":\Latein\output\");
                }
                if (!Directory.Exists(LatinPath + @"dictversion\"))
                {
                    Directory.CreateDirectory(LatinPath + @"dictversion\");
                }
                if (!Directory.Exists(LatinPath + @"output\"))
                {
                    Directory.CreateDirectory(LatinPath + @"output\");
                }
                if (!Directory.Exists(LatinPath + @"output_old\"))
                {
                    Directory.CreateDirectory(LatinPath + @"output_old\");
                }
                if (!File.Exists(LatinPath + @"dictversion\dictversion.txt"))
                {
                    File.Create(LatinPath + @"dictversion\dictversion.txt");
                    File.WriteAllText(LatinPath + @"dictversion\dictversion.txt", "1");
                }
                if (!File.Exists(DictPath + "dict.txt"))
                {
                    File.Create(DictPath + "dict.txt");
                }
                Console.WriteLine("Loading offline dict. (Offline-Wörterbuch wird geladen.)");
                LoadDict();
                Console.Clear();
                Program.DownloadDict(false);
                Console.Clear();
            }
            catch
            {
                Console.Clear();
                counter_restart++;
                if (counter_restart < 5)
                {
                    Process.Start(StringOperations.GetApplicationsPath() + @"\LatinHelper.exe");
                    Environment.Exit(-1);
                    //goto newtry;
                }
                Console.WriteLine("Installationsfehler.");
                Console.WriteLine("Drücken Sie eine beliebige Taste zum Beenden.");
                Console.ReadKey(true);
                Environment.Exit(-1);
            }
        }
        public static Dictionary<string, Wort> dict = new Dictionary<string, Wort>();
        public static void LoadDict()
        {
            string allvoks = File.ReadAllText(DictPath+"dict.txt");
            string[] splittetvoks = allvoks.Split(new String[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rawvok in splittetvoks)
            {
                Wort w;
                if (getWortFromRAW(rawvok, out w))
                {
                    if (!dict.ContainsKey(w.Latein))
                    {
                        dict.Add(w.Latein, w);
                    }
                }
            }
        }
        public static void RefreshDict(bool manual)
        {
            int countfiles = dict.Count;
            string version = "1";
            int iversion2 = countfiles / 1000 + 1;
            if (File.Exists(LatinPath + @"dictversion\dictversion.txt"))
            {
                version = File.ReadAllText(LatinPath + @"dictversion\dictversion.txt");
            }
            bool nocontentinfile = false;
            int iversion = 0;
            try
            {
                iversion = int.Parse(version);
            }
            catch
            {
                nocontentinfile = true;
            }
            if(nocontentinfile)
            {
                File.WriteAllText(LatinPath + @"dictversion\dictversion.txt", "1");
            }
            if (iversion2 < iversion)
            {
                iversion = iversion2;
                File.WriteAllText(LatinPath + @"dictversion\dictversion.txt", (iversion2 + 1).ToString());
            }
            iversion = iversion2;
            WebClient webclient = new WebClient();
            int gesamt = 0;
            try
            {
                webclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0");
                gesamt = int.Parse(webclient.DownloadString(RefreshServer + "gesamt" + ".txt"));
            }
            catch (Exception e)
            {
                if (manual)
                {
                    Console.WriteLine("Das Wörterbuch konnte nicht aktualisiert werden.");
                }
                return;
            }
            Console.Clear();
            if (!manual)
            {
                if ((iversion + 1) < gesamt)
                {
                    Console.WriteLine("Ihr Wörterbuch ist nicht auf dem neuesten Stand.");
                    Console.WriteLine("Wollen sie ein Update durchführen? (y/n)");
                    if (Console.ReadKey(true).KeyChar != 'y')
                    {
                        Console.Clear();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            gesamt = gesamt - iversion;
            Console.Clear();
            int counter = 0;
            if (iversion < 1)
            {
                iversion = 1;
            }
        newupdate:
            if (counter >= gesamt)
            {
                Console.Clear();
                Console.WriteLine("Das Wörterbuch ist aktuell.");
                return;
            }
            int status = (counter * 10000) / gesamt;
            double dstatus = status / 100.0;
            Console.Clear();
            Console.WriteLine("Update wird durchgeführt: " + dstatus + "%");
            counter++;
            String newdict = "";
            version = "" + iversion;
            try
            {
                webclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0");
                newdict = webclient.DownloadString(RefreshServer + version + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Das Wörterbuch konnte nicht aktualisiert werden.");
                return;
            }
            if (newdict.Equals("NOT_READY"))
            {
                Console.WriteLine("Das Wörterbuch ist aktuell.");
                return;
            }
            try
            {
                newdict = StringOperations.StringAb("*", newdict);
            }
            catch
            {

            }
            String[] newvoks = newdict.Split(new String[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            String appendcontent = "";
            foreach (String newvok in newvoks)
            {
                string vok = StringOperations.StringBis("$", newvok);
                string content = StringOperations.StringAb("$", newvok);
                if (!dict.ContainsKey(vok))
                {                    
                    appendcontent += content + "&";
                    //Console.WriteLine(vok);
                }
            }
            File.AppendAllText(DictPath + "dict.txt", appendcontent);
            File.WriteAllText(LatinPath + @"dictversion\dictversion.txt", iversion + "");
            iversion++;
            goto newupdate;
        }
        public static void RefreshDictOld(bool manual)
        {
            int countfiles = Directory.GetFiles(DictPath).Length;
            String version = "1";
            int iversion2 = countfiles / 1000 + 1;
            if (File.Exists(LatinPath + @"dictversion\version.txt"))
            {
                version = File.ReadAllText(LatinPath + @"dictversion\version.txt");
            }
            int iversion = 0;
            try
            {
                iversion = Int32.Parse(version);
            }
            catch
            {

            }
            if (iversion2 < iversion)
            {
                iversion = iversion2;
                File.WriteAllText(LatinPath + @"dictversion\version.txt", (iversion2 + 1).ToString());
            }
            WebClient webclient = new WebClient();
            int gesamt = 0;
            try
            {
                webclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0");
                gesamt = Int32.Parse(webclient.DownloadString(RefreshServer + "gesamt" + ".txt"));
            }
            catch (Exception e)
            {
                if (manual)
                {
                    Console.WriteLine("Das Wörterbuch konnte nicht aktualisiert werden.");
                }
                return;
            }
            Console.Clear();
            if (!manual)
            {
                if ((iversion + 1) < gesamt)
                {
                    Console.WriteLine("Ihr Wörterbuch ist nicht auf dem neuesten Stand.");
                    Console.WriteLine("Wollen sie ein Update durchführen? (y/n)");
                    if (Console.ReadKey(true).KeyChar != 'y')
                    {
                        Console.Clear();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            gesamt = gesamt - iversion;
            Console.Clear();
            int counter = 0;
            if (iversion < 1)
            {
                iversion = 1;
            }
        newupdate:
            if (counter >= gesamt)
            {
                Console.Clear();
                Console.WriteLine("Das Wörterbuch ist aktuell.");
                return;
            }
            int status = (counter * 10000) / gesamt;
            double dstatus = status / 100.0;
            Console.Clear();
            Console.WriteLine("Update wird durchgeführt: " + dstatus + "%");
            counter++;
            String newdict = "";
            version = "" + iversion;
            try
            {
                webclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0");
                newdict = webclient.DownloadString(RefreshServer + version + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Das Wörterbuch konnte nicht aktualisiert werden.");
                return;
            }
            if (newdict.Equals("NOT_READY"))
            {
                Console.WriteLine("Das Wörterbuch ist aktuell.");
                return;
            }
            try
            {
                newdict = StringOperations.StringAb("*", newdict);
            }
            catch
            {

            }
            String[] newvoks = newdict.Split(new String[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String newvok in newvoks)
            {
                String vok = StringOperations.StringBis("$", newvok);
                String content = StringOperations.StringAb("$", newvok);
                File.WriteAllText(DictPath + vok + DictPathEnd, content);
            }
            File.WriteAllText(LatinPath + @"dictversion\version.txt", iversion + "");
            iversion++;
            goto newupdate;
        }
        public static void CreateDict(string backuppath, string newdictpath, string outputpath, string vversion)
        {
            String ret = "";
            Dictionary<String, String> newvoks = new Dictionary<String, String>();
            String[] files = Directory.GetFiles(newdictpath);
            int counter = 0;
            int counter2 = 0;
            foreach (String path in files)
            {
                try
                {
                    String vok = StringOperations.StringAb(newdictpath, path);
                    vok = StringOperations.StringBis(".", vok);
                    if (File.Exists(backuppath + vok + ".txt"))
                    {
                        if (File.ReadAllText(backuppath + vok + ".txt") == File.ReadAllText(newdictpath + vok + ".txt"))
                        {
                            continue;
                        }
                    }
                    String output = File.ReadAllText(newdictpath + vok + ".txt");
                    counter++;
                    newvoks.Add(vok, output);
                }
                catch
                {
                    //Console.WriteLine(path);
                    //Console.ReadKey(true);
                }
                

            }
            counter = 0;
            counter2++;
            foreach (String vok in newvoks.Keys)
            {
                String outp = "";
                newvoks.TryGetValue(vok, out outp);
                ret += outp + "&";
            }
            System.IO.File.WriteAllText(outputpath + counter2 + ".txt", ret);
        }
        public static void CreateDict2(string backuppath, string newdictpath, string outputpath, string vversion)
        {
            String ret = "";
            Dictionary<String, String> newvoks = new Dictionary<String, String>();
            String[] files = Directory.GetFiles(newdictpath);
            int counter = 0;
            int counter2 = 0;
            foreach (String path in files)
            {
                if (counter >= 1000)
                {
                    counter = 0;
                    counter2++;
                    int count2 = counter2 + 1;
                    ret = "";
                    foreach (String vok in newvoks.Keys)
                    {
                        String outp = "";
                        newvoks.TryGetValue(vok, out outp);
                        ret += vok + "$" + outp + "&";
                    }
                    newvoks.Clear();
                    System.IO.File.WriteAllText(outputpath + counter2 + ".txt", ret);
                    ret = "";
                    Console.WriteLine("File created: " + outputpath + counter2 + ".txt");
                }
                else
                {
                    try
                    {
                        String vok = StringOperations.StringAb(newdictpath, path);
                        vok = StringOperations.StringBis(".", vok);
                        if (File.Exists(backuppath + vok + ".txt"))
                        {
                            if (File.ReadAllText(backuppath + vok + ".txt") == File.ReadAllText(newdictpath + vok + ".txt"))
                            {
                                continue;
                            }
                        }
                        String output = File.ReadAllText(newdictpath + vok + ".txt");
                        counter++;
                        newvoks.Add(vok, output);
                    }
                    catch
                    {
                        //Console.WriteLine(path);
                        //Console.ReadKey(true);
                    }
                }
            }
            counter = 0;
            counter2++;
            foreach (String vok in newvoks.Keys)
            {
                String outp = "";
                newvoks.TryGetValue(vok, out outp);
                ret += vok + "$" + outp + "&";
            }
            File.WriteAllText(outputpath + counter2 + ".txt", ret);
        }
        public static bool getWortFromRAW(string raw, out Wort wort)
        {
            try
            {
                wort = new Wort();
                String[] mainsplit = raw.Split(new String[] { "#" }, StringSplitOptions.None);
                wort.Latein = mainsplit[0];
                String[] formenarr = mainsplit[1].Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String formstr in formenarr)
                {
                    Form form = new Form();
                    form._wortart = (Form.Wortart)Int32.Parse(formstr[0] + "");
                    switch (formstr.Length)
                    {
                        case 7:
                            form._typ = Form.Typ.Nomen;
                            form._kasus = (Form.Kasus)Int32.Parse(formstr[2] + "");
                            form._numerus = (Form.Numerus)Int32.Parse(formstr[4] + "");
                            form._genus = (Form.Genus)Int32.Parse(formstr[6] + "");
                            break;
                        case 11:
                            form._typ = Form.Typ.Verb;
                            form._person = (Form.Person)Int32.Parse(formstr[2] + "");
                            form._numerus = (Form.Numerus)Int32.Parse(formstr[4] + "");
                            form._tempus = (Form.Tempus)Int32.Parse(formstr[6] + "");
                            form._modus = (Form.Modus)Int32.Parse(formstr[8] + "");
                            form._genusVerbi = (Form.GenusVerbi)Int32.Parse(formstr[10] + "");
                            break;
                        case 3:
                            form._typ = Form.Typ.Gerundium;
                            form._kasus = (Form.Kasus)Int32.Parse(formstr[2] + "");
                            break;
                        default:
                            break;
                    }
                    
                    wort._formen.Add(form);
                }
                return true;
            }
            catch
            {
                wort = null;
                return false;
            }
            }
        public static Wort getVokabel(string vok, bool b)
        {
            actualVok = vok;
            b = true;
            //b = false;
            Wort wort = new Wort();
            String path = DictPath + vok + DictPathEnd;
            if (dict.ContainsKey(vok))// & b)
            {
                dict.TryGetValue(vok,out wort);
            }
            else
            {
                string html = getHTML(vok);
                actualVok = vok;
                string[] arr = getStringArray(html);
                string output = Output(arr);
                if (!output.Contains("NOT_AVAILABLE"))
                {
                    File.WriteAllText(@"C:\Latein\output_old\" + vok + ".txt", output);
                }
                wort._formen = getFormen(arr);
                //wort = getVok(vok);
                wort.Latein = vok;
            }
            return wort;
        }
        public static string getHTML(string vok)
        {
            actualVok = vok;
            String html = "";
            if (File.Exists(@"C:\Latein\output_old\" + vok + ".txt"))
            {
                using (StreamReader sr = new StreamReader(@"C:\Latein\output_old\" + vok + ".txt"))
                {
                    html = sr.ReadToEnd();
                }
            }
            else
            {
                wc = new WebClient();
                html = wc.DownloadString(DictServer + vok + DictServerEnd);
                html = StringOperations.StringAb("margin-top-10", html);
                html = StringOperations.StringAb("h3>", html);
                if (html.Contains("margin-top-50"))
                {
                    html = StringOperations.StringBis("margin-top-50", html);
                    html = StringOperations.StringBis("<h3", html);
                }
                else
                {
                    html = StringOperations.StringBis("<!-- BEGIN COMMENT -->", html);
                }

            }
            return html;
        }
        public string getGermanByForm(Form f)
        {
            String german = "";
            switch (f._wortart)
            {
                case Form.Wortart.Substantiv:

                    break;
                case Form.Wortart.Verb:
                    WebClient wclient = new WebClient();
                    //String html = wclient.DownloadString("http://konjugator.reverso.net/konjugation-deutsch-verb-" + f._deutsch);
                    break;
            }
            return german;
        }
        public static Wort getVok(string vok)
        {
            Wort wort = new Wort();
            if (dict.ContainsKey(vok))
            {
                dict.TryGetValue(vok, out wort);
            }
            else
            {
                try
                {
                    
                    List<Form> ret = new List<Form>();
                    String html = "";
                    if (File.Exists(LatinPath + @"output\" + vok + ".txt"))
                    {
                        using (StreamReader sr = new StreamReader(LatinPath + @"output\" + vok + ".txt"))
                        {
                            html = sr.ReadToEnd();
                        }
                    }
                    else
                    {
                        wc = new WebClient();
                        html = wc.DownloadString("http://www.latein.me/mixed/" + vok);
                        html = StringOperations.StringAb("<div class=\"contentBox\">", html);
                        html = StringOperations.StringBis("</dd></dl></div>", html);
                        html = StringOperations.StringAb("<dt class=\"translationEntryBox\">", html);
                    }
                    String[] arr = html.Split(new String[] { "<dt class=\"translationEntryBox\">" }, StringSplitOptions.None);
                    if (true)
                    {
                        System.IO.File.WriteAllText(LatinPath + @"output\" + vok + ".txt", html);
                    }
                    foreach (String ar in arr)
                    {
                        if (StringOperations.StringBis("(", ar).Contains("que (Konjunktion)"))
                        {
                            continue;
                        }
                        String wortart = StringOperations.StringAb("(", ar);
                        wortart = StringOperations.StringBis(")", wortart);
                        String formenstr = StringOperations.StringAb("<dd class=\"formAnalysisEntry\">", ar);
                        String[] formenarr = formenstr.Split(new String[] { "<dd class=\"formAnalysisEntry\">" }, StringSplitOptions.None);
                        foreach (String formenar in formenarr)
                        {
                            Form form3 = new Form();
                            switch (wortart)
                            {
                                case "Verb":
                                    form3._wortart = Form.Wortart.Verb;
                                    form3._typ = Form.Typ.Verb;
                                    break;
                                case "Substantiv":
                                    form3._typ = Form.Typ.Nomen;
                                    form3._wortart = Form.Wortart.Substantiv;
                                    break;
                                case "Adjektiv":
                                    form3._typ = Form.Typ.Nomen;
                                    form3._wortart = Form.Wortart.Adjektiv;
                                    break;
                                case "Zahlwort":
                                    form3._typ = Form.Typ.Nomen;
                                    form3._wortart = Form.Wortart.Adjektiv;
                                    break;
                                case "Pronomen":
                                    form3._typ = Form.Typ.Nomen;
                                    form3._wortart = Form.Wortart.Adjektiv;
                                    break;
                                default:
                                    break;
                            }
                            String formstr = formenar;
                            if (formenar.Contains("</dd>"))
                            {
                                formstr = StringOperations.StringBis("</dd>", formenar);
                            }
                            if (formstr.Contains("Singular"))
                            {
                                form3._numerus = Form.Numerus.Sg;
                            }
                            if (formstr.Contains("Plural"))
                            {
                                form3._numerus = Form.Numerus.Pl;
                            }
                            if (formstr.Contains("Nominativ") | formstr.Contains("Vokativ"))
                            {
                                form3._kasus = Form.Kasus.Nom;
                            }
                            if (formstr.Contains("Genitiv"))
                            {
                                form3._kasus = Form.Kasus.Gen;
                            }
                            if (formstr.Contains("Dativ"))
                            {
                                form3._kasus = Form.Kasus.Dat;
                            }
                            if (formstr.Contains("Akkusativ"))
                            {
                                form3._kasus = Form.Kasus.Akk;
                            }
                            if (formstr.Contains("Ablativ"))
                            {
                                form3._kasus = Form.Kasus.Abl;
                            }
                            if (formstr.Contains("Maskulinum"))
                            {
                                form3._genus = Form.Genus.m;
                            }
                            if (formstr.Contains("Femininum"))
                            {
                                form3._genus = Form.Genus.f;
                            }
                            if (formstr.Contains("Neutrum"))
                            {
                                form3._genus = Form.Genus.n;
                            }
                            if (formstr.Contains("PP"))
                            {
                                form3._wortart = Form.Wortart.Partizip;
                                form3._typ = Form.Typ.Nomen;
                            }
                            if (formstr.Contains("1."))
                            {
                                form3._person = Form.Person.P1;
                            }
                            if (formstr.Contains("2."))
                            {
                                form3._person = Form.Person.P2;
                            }
                            if (formstr.Contains("3."))
                            {
                                form3._person = Form.Person.P3;
                            }
                            if (formstr.Contains("PrÃ¤sens"))
                            {
                                form3._tempus = Form.Tempus.Praesens;
                            }
                            if (formstr.Contains("Perfekt"))
                            {
                                form3._tempus = Form.Tempus.Perfekt;
                            }
                            if (formstr.Contains("Imperfekt"))
                            {
                                form3._tempus = Form.Tempus.Imperfekt;
                            }
                            if (formstr.Contains("Futur I"))
                            {
                                form3._tempus = Form.Tempus.Futur1;
                            }
                            if (formstr.Contains("Plusquamperfekt"))
                            {
                                form3._tempus = Form.Tempus.PLQPF;
                            }
                            if (formstr.Contains("Futur II"))
                            {
                                form3._tempus = Form.Tempus.Futur2;
                            }
                            if (formstr.Contains("Indikativ"))
                            {
                                form3._modus = Form.Modus.Indikativ;
                            }
                            if (formstr.Contains("Konjunktiv"))
                            {
                                form3._modus = Form.Modus.Konjunktiv;
                            }
                            if (formstr.Contains("Aktiv"))
                            {
                                form3._genusVerbi = Form.GenusVerbi.Aktiv;
                            }
                            if (formstr.Contains("Passiv"))
                            {
                                form3._genusVerbi = Form.GenusVerbi.Passiv;
                            }
                            if ((form3._wortart == Form.Wortart.Verb) & (form3._kasus != Form.Kasus.Default))
                            {
                                form3._wortart = Form.Wortart.Partizip;
                                form3._typ = Form.Typ.Nomen;
                            }
                            if (formstr.Contains("MasFem"))
                            {
                                form3._genus = Form.Genus.m;
                                Form form2 = form3;
                                form2._genus = Form.Genus.f;
                                ret.Add(form2);
                            }
                            switch (form3._typ)
                            {
                                case Form.Typ.Nomen:
                                    if ((form3._kasus == Form.Kasus.Default) | (form3._numerus == Form.Numerus.Default) | (form3._genus == Form.Genus.Default))
                                    {
                                        continue;
                                    }
                                    break;
                                case Form.Typ.Verb:
                                    /*if ((form3._tempus == Form.Tempus.Default) | (form3._genusVerbi == Form.GenusVerbi.Default))
                                    {
                                        continue;
                                    }*/
                                    break;
                                case Form.Typ.Gerundium:
                                    if ((form3._kasus == Form.Kasus.Default))
                                    {
                                        continue;
                                    }
                                    break;
                                case Form.Typ.Default:
                                    continue;
                                default:
                                    break;
                            }
                            ret.Add(form3);
                        }
                        wort._formen = ret;
                    }
                    wort.Latein = vok;
                }
                catch
                {
                    wort = getVokabel(vok, true);
                }
            }
            return wort;
        }
        public static List<Form> getFormen(string[] input)
        {
            List<Form> ret = new List<Form>();
            foreach (String inp in input)
            {
                if (!inp.Contains("<span class=\"hauptwort "))
                {
                    continue;
                }
                String analyse = StringOperations.StringAb("<span class=\"hauptwort ", inp);
                String german = StringOperations.StringAb("<ol>", inp);
                german = StringOperations.StringBis("</ol>", german);
                Form f = new Form();
                switch (analyse[0])
                {
                    #region Adjektiv
                    case '0':
                        analyse = StringOperations.StringAb("<div class=\"formen\">", inp);
                        analyse = StringOperations.StringAb(": ", analyse);
                        analyse = StringOperations.StringBis("</div>", analyse);
                        String[] formarr = analyse.Split(',');
                        foreach (String formstr in formarr)
                        {
                            Form form3 = new Form();
                            form3._typ = Form.Typ.Nomen;
                            form3._wortart = Form.Wortart.Adjektiv;
                            if (formstr.Contains("Sg"))
                            {
                                form3._numerus = Form.Numerus.Sg;
                            }
                            else
                            {
                                form3._numerus = Form.Numerus.Pl;
                            }
                            if (formstr.Contains("Nom"))
                            {
                                form3._kasus = Form.Kasus.Nom;
                            }
                            if (formstr.Contains("Gen"))
                            {
                                form3._kasus = Form.Kasus.Gen;
                            }
                            if (formstr.Contains("Dat"))
                            {
                                form3._kasus = Form.Kasus.Dat;
                            }
                            if (formstr.Contains("Akk"))
                            {
                                form3._kasus = Form.Kasus.Akk;
                            }
                            if (formstr.Contains("Abl"))
                            {
                                form3._kasus = Form.Kasus.Abl;
                            }
                            if (formstr.Contains("mask."))
                            {
                                form3._genus = Form.Genus.m;
                            }
                            if (formstr.Contains("fem."))
                            {
                                form3._genus = Form.Genus.f;
                            }
                            if (formstr.Contains("neutr."))
                            {
                                form3._genus = Form.Genus.n;
                            }
                            ret.Add(form3);
                        }
                        break;
                    #endregion
                    #region Substantiv
                    case '1':
                        Form.Genus _gen = Form.Genus.Default;
                        String genus = StringOperations.StringAb(",", analyse);
                        genus = StringOperations.StringBis("span", genus);
                        if (genus.Contains("f<"))
                        {
                            _gen = Form.Genus.f;
                        }
                        if (genus.Contains("n<"))
                        {
                            _gen = Form.Genus.n;
                        } if (genus.Contains("m<"))
                        {
                            _gen = Form.Genus.m;
                        }
                        analyse = StringOperations.StringAb("<div class=\"formen\">", inp);
                        analyse = StringOperations.StringAb(": ", analyse);
                        analyse = StringOperations.StringBis("</div>", analyse);
                        formarr = analyse.Split(',');
                        foreach (String formstr in formarr)
                        {
                            Form form3 = new Form();
                            form3._typ = Form.Typ.Nomen;
                            form3._wortart = Form.Wortart.Substantiv;
                            form3._genus = _gen;
                            if (formstr.Contains("Sg"))
                            {
                                form3._numerus = Form.Numerus.Sg;
                            }
                            else
                            {
                                form3._numerus = Form.Numerus.Pl;
                            }
                            if (formstr.Contains("Nom"))
                            {
                                form3._kasus = Form.Kasus.Nom;
                            }
                            if (formstr.Contains("Gen"))
                            {
                                form3._kasus = Form.Kasus.Gen;
                            }
                            if (formstr.Contains("Dat"))
                            {
                                form3._kasus = Form.Kasus.Dat;
                            }
                            if (formstr.Contains("Akk"))
                            {
                                form3._kasus = Form.Kasus.Akk;
                            }
                            if (formstr.Contains("Abl"))
                            {
                                form3._kasus = Form.Kasus.Abl;
                            }
                            ret.Add(form3);
                        }
                        break;

                    #endregion
                    #region Verb/Partizip/Gerundium/Gerundivum
                    case '3':
                        String fform = StringOperations.StringAb("<div class=\"formen\">", analyse);
                        fform = StringOperations.StringAb(": ", fform);
                        #region Partizip
                        if (fform.StartsWith("PP"))
                        {
                            String ffform = StringOperations.StringBis("</div>", fform);
                            formarr = ffform.Split(',');
                            foreach (String formstring in formarr)
                            {
                                if (!formstring.Contains("PP"))
                                {
                                    continue;
                                }
                                String formstr = StringOperations.StringAb("(", formstring);
                                formstr = StringOperations.StringBis(")", formstr);
                                Form form3 = new Form();
                                form3._typ = Form.Typ.Nomen;
                                form3._wortart = Form.Wortart.Partizip;
                                if (formstr.Contains("Sg"))
                                {
                                    form3._numerus = Form.Numerus.Sg;
                                }
                                else
                                {
                                    form3._numerus = Form.Numerus.Pl;
                                }
                                if (formstr.Contains("Nom"))
                                {
                                    form3._kasus = Form.Kasus.Nom;
                                }
                                if (formstr.Contains("Gen"))
                                {
                                    form3._kasus = Form.Kasus.Gen;
                                }
                                if (formstr.Contains("Dat"))
                                {
                                    form3._kasus = Form.Kasus.Dat;
                                }
                                if (formstr.Contains("Akk"))
                                {
                                    form3._kasus = Form.Kasus.Akk;
                                }
                                if (formstr.Contains("Abl"))
                                {
                                    form3._kasus = Form.Kasus.Abl;
                                }
                                if (formstr.Contains("mask."))
                                {
                                    form3._genus = Form.Genus.m;
                                }
                                if (formstr.Contains("fem."))
                                {
                                    form3._genus = Form.Genus.f;
                                }
                                if (formstr.Contains("neutr."))
                                {
                                    form3._genus = Form.Genus.n;
                                }

                                //form3._deutsch = KonjugationDeutsch(german);
                                ret.Add(form3);
                            }
                        }
                        #endregion
                        #region Gerundium
                        else if (fform.StartsWith("Gerundium") | fform.StartsWith("Gerundivum"))
                        {
                            String ffform = StringOperations.StringBis("</div>", fform);
                            formarr = ffform.Split(',');
                            foreach (String formstring in formarr)
                            {
                                Form form3 = new Form();
                                if (formstring.Contains("Gerundium"))
                                {
                                    form3._typ = Form.Typ.Gerundium;
                                    form3._wortart = Form.Wortart.Gerundium;
                                }
                                if (formstring.Contains("Gerundivum"))
                                {
                                    form3._typ = Form.Typ.Nomen;
                                    form3._wortart = Form.Wortart.Gerundivum;
                                }
                                String formstr = formstring;
                                try
                                {
                                    formstr = StringOperations.StringAb("(", formstring);
                                    formstr = StringOperations.StringBis(")", formstr);
                                }
                                catch
                                {

                                }
                                form3._genus = Form.Genus.Default;
                                if (formstr.Contains("Sg"))
                                {
                                    form3._numerus = Form.Numerus.Sg;
                                }
                                if (formstr.Contains("Pl"))
                                {
                                    form3._numerus = Form.Numerus.Pl;
                                }
                                if (formstr.Contains("Nom"))
                                {
                                    form3._kasus = Form.Kasus.Nom;
                                }
                                if (formstr.Contains("Gen"))
                                {
                                    form3._kasus = Form.Kasus.Gen;
                                }
                                if (formstr.Contains("Dat"))
                                {
                                    form3._kasus = Form.Kasus.Dat;
                                }
                                if (formstr.Contains("Akk"))
                                {
                                    form3._kasus = Form.Kasus.Akk;
                                }
                                if (formstr.Contains("Abl"))
                                {
                                    form3._kasus = Form.Kasus.Abl;
                                }
                                if (formstr.Contains("mask."))
                                {
                                    form3._genus = Form.Genus.m;
                                }
                                if (formstr.Contains("fem."))
                                {
                                    form3._genus = Form.Genus.f;
                                }
                                if (formstr.Contains("neutr."))
                                {
                                    form3._genus = Form.Genus.n;
                                }
                                ret.Add(form3);
                            }
                        }
                        #endregion
                        else
                        {
                            Form form3 = new Form();
                            form3._typ = Form.Typ.Verb;
                            form3._wortart = Form.Wortart.Verb;
                            String ffform = StringOperations.StringBis("</div>", fform);
                            formarr = ffform.Split(',');
                            foreach (String formstring in formarr)
                            {
                                String formstr = formstring;
                                try
                                {
                                    formstr = StringOperations.StringAb(":", formstring);
                                }
                                catch
                                {

                                }
                                if (formstr.Contains("1. "))
                                {
                                    form3._person = Form.Person.P1;
                                }
                                if (formstr.Contains("2. "))
                                {
                                    form3._person = Form.Person.P2;
                                }
                                if (formstr.Contains("3. "))
                                {
                                    form3._person = Form.Person.P3;
                                }
                                if (formstr.Contains("Sg."))
                                {
                                    form3._numerus = Form.Numerus.Sg;
                                }
                                if (formstr.Contains("Pl."))
                                {
                                    form3._numerus = Form.Numerus.Pl;
                                }
                                if (formstr.Contains("PrÃ¤s. "))
                                {
                                    form3._tempus = Form.Tempus.Praesens;
                                }
                                if (formstr.Contains("Perf. "))
                                {
                                    form3._tempus = Form.Tempus.Perfekt;
                                }
                                if (formstr.Contains("Imperf. "))
                                {
                                    form3._tempus = Form.Tempus.Imperfekt;
                                }
                                if (formstr.Contains("Fut. I "))
                                {
                                    form3._tempus = Form.Tempus.Futur1;
                                }
                                if (formstr.Contains("Plusquamperf. "))
                                {
                                    form3._tempus = Form.Tempus.PLQPF;
                                }
                                if (formstr.Contains("Fut. II "))
                                {
                                    form3._tempus = Form.Tempus.Futur2;
                                }
                                if (formstr.Contains("Ind. "))
                                {
                                    form3._modus = Form.Modus.Indikativ;
                                }
                                if (formstr.Contains("Konj. "))
                                {
                                    form3._modus = Form.Modus.Konjunktiv;
                                }
                                if (formstr.Contains("Akt."))
                                {
                                    form3._genusVerbi = Form.GenusVerbi.Aktiv;
                                }
                                if (formstr.Contains("Pass."))
                                {
                                    form3._genusVerbi = Form.GenusVerbi.Passiv;
                                }
                                ret.Add(form3);
                            }
                        }

                        break;
                    #endregion
                    #region Default
                    default:
                        f._wortart = Form.Wortart.Default;
                        break;
                    #endregion
                }
            }
            return ret;
        }
        public static List<string> KonjugationDeutsch(string german)
        {
            throw new NotImplementedException();
        }
        public static string Output(string[] input)
        {
            String ret = "";
            foreach (String s in input)
            {
                ret += s + "-----\n";
            }
            return ret;
        }
        public static string[] getStringArray(string html)
        {
            String[] ret;
            if (html.Contains("-----"))
            {
                ret = html.Split(new String[] { "-----" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                if (html.Contains("<div class=\"hinweis\"></div>"))
                {
                    ret = html.Split(new String[] { "<div class=\"hinweis\"></div>" }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    ret = new String[] { "NOT_AVAILABLE" };
                    File.AppendAllText(@"C:\Latein\notfound.txt", actualVok + ";");
                }
            }
            return ret;
        }
        /*public static Wort encodeHTML(String html)
        {
            String htmlold = html;
            Wort wort = new Wort();
            html = StringOperations.StringAb("<span class=\"formen\">", html);
            String worta = StringOperations.StringBis("</span>", html);
            if (worta.Contains("Verb"))
            {
                wort._wortart = Wort.Wortart.Verb;
            }
            if (worta.Contains("Subst"))
            {
                wort._wortart = Wort.Wortart.Substantiv;
            }
            if (worta.Contains("Adj"))
            {
                wort._wortart = Wort.Wortart.Adjektiv;
            }
            String gen = StringOperations.StringAb("<span class=\"hauptwort 1\">", html);
            gen = StringOperations.StringBis("<span class", gen);
            String form = StringOperations.StringAb("class=\"formen\">", html);
            form = StringOperations.StringAb(":", html);
            form = StringOperations.StringBis("</div>", form);
            switch (wort._wortart)
            {
                case Wort.Wortart.Substantiv:
                    String genSubst = StringOperations.StringAb("<span class=\"hauptwort 1\">", htmlold);
                    genSubst = StringOperations.StringAb(", ", genSubst);
                    genSubst = genSubst.Substring(0, 1);
                    if(form.Contains(',')) {
                    String[] formes = form.Split(',');
                    foreach(String forme in formes) {
                        Form form3 = new Form();
                        if (forme.Contains("Sg"))
                        {
                            form3._numerus = Form.Numerus.Sg;
                        }
                        else
                        {
                            form3._numerus = Form.Numerus.Pl;
                        }
                        if (forme.Contains("Nom"))
                        {
                            form3._kasus = Form.Kasus.Nom;
                        }
                        if (forme.Contains("Gen"))
                        {
                            form3._kasus = Form.Kasus.Gen;
                        }
                        if (forme.Contains("Dat"))
                        {
                            form3._kasus = Form.Kasus.Dat;
                        }
                        if (forme.Contains("Akk"))
                        {
                            form3._kasus = Form.Kasus.Akk;
                        }
                        if (forme.Contains("Abl"))
                        {
                            form3._kasus = Form.Kasus.Abl;
                        }
                        if (genSubst.Contains("m"))
                        {
                            form3._genus = Form.Genus.m;
                        }
                        if (genSubst.Contains("f"))
                        {
                            form3._genus = Form.Genus.f;
                        }
                        if (genSubst.Contains("n"))
                        {
                            form3._genus = Form.Genus.n;
                        }
                        wort.addForm(form3);
                    }
                    }
                    else {
                        Form form3 = new Form();
                        if (form.Contains("Sg"))
                        {
                            form3._numerus = Form.Numerus.Sg;
                        }
                        else
                        {
                            form3._numerus = Form.Numerus.Pl;
                        }
                        if (form.Contains("Nom"))
                        {
                            form3._kasus = Form.Kasus.Nom;
                        }
                        if (form.Contains("Gen"))
                        {
                            form3._kasus = Form.Kasus.Gen;
                        }
                        if (form.Contains("Dat"))
                        {
                            form3._kasus = Form.Kasus.Dat;
                        }
                        if (form.Contains("Akk"))
                        {
                            form3._kasus = Form.Kasus.Akk;
                        }
                        if (form.Contains("Abl"))
                        {
                            form3._kasus = Form.Kasus.Abl;
                        }
                        if (genSubst.Contains("m"))
                        {
                            form3._genus = Form.Genus.m;
                        }
                        if (genSubst.Contains("f"))
                        {
                            form3._genus = Form.Genus.f;
                        }
                        if (genSubst.Contains("n"))
                        {
                            form3._genus = Form.Genus.n;
                        }
                        wort.addForm(form3);
                    }
                    break;
                case Wort.Wortart.Adjektiv:
                    if (form.Contains(','))
                    {
                        String[] formes = form.Split(',');
                        foreach (String forme in formes)
                        {
                            Form form3 = new Form();
                            if (forme.Contains("Sg"))
                            {
                                form3._numerus = Form.Numerus.Sg;
                            }
                            else
                            {
                                form3._numerus = Form.Numerus.Pl;
                            }
                            if (forme.Contains("Nom"))
                            {
                                form3._kasus = Form.Kasus.Nom;
                            }
                            if (forme.Contains("Gen"))
                            {
                                form3._kasus = Form.Kasus.Gen;
                            }
                            if (forme.Contains("Dat"))
                            {
                                form3._kasus = Form.Kasus.Dat;
                            }
                            if (forme.Contains("Akk"))
                            {
                                form3._kasus = Form.Kasus.Akk;
                            }
                            if (forme.Contains("Abl"))
                            {
                                form3._kasus = Form.Kasus.Abl;
                            }
                            if (forme.Contains("mask"))
                            {
                                form3._genus = Form.Genus.m;
                            }
                            if (forme.Contains("fem"))
                            {
                                form3._genus = Form.Genus.f;
                            }
                            if (forme.Contains("neutr"))
                            {
                                form3._genus = Form.Genus.n;
                            }
                            wort.addForm(form3);
                        }
                    }
                    else
                    {
                        Form form3 = new Form();
                        if (form.Contains("Sg"))
                        {
                            form3._numerus = Form.Numerus.Sg;
                        }
                        else
                        {
                            form3._numerus = Form.Numerus.Pl;
                        }
                        if (form.Contains("Nom"))
                        {
                            form3._kasus = Form.Kasus.Nom;
                        }
                        if (form.Contains("Gen"))
                        {
                            form3._kasus = Form.Kasus.Gen;
                        }
                        if (form.Contains("Dat"))
                        {
                            form3._kasus = Form.Kasus.Dat;
                        }
                        if (form.Contains("Akk"))
                        {
                            form3._kasus = Form.Kasus.Akk;
                        }
                        if (form.Contains("Abl"))
                        {
                            form3._kasus = Form.Kasus.Abl;
                        }
                        if (form.Contains("mask"))
                        {
                            form3._genus = Form.Genus.m;
                        }
                        if (form.Contains("fem"))
                        {
                            form3._genus = Form.Genus.f;
                        }
                        if (form.Contains("neutr"))
                        {
                            form3._genus = Form.Genus.n;
                        }
                        wort.addForm(form3);
                    }
                    break;
                default:
                    break;
            }
            /*if (form.Contains("Sg"))
            {
                _newform._numerus = Form.Numerus.Sg;
            }
            else
            {
                _newform._numerus = Form.Numerus.Pl;
            }
            if (wort._wortart == Wort.Wortart.Substantiv)
            {
                if (form.Contains("Nom"))
                {
                    _newform._kasus = Form.Kasus.Nom;
                }
                if (form.Contains("Gen"))
                {
                    _newform._kasus = Form.Kasus.Gen;
                }
                if (form.Contains("Dat"))
                {
                    _newform._kasus = Form.Kasus.Dat;
                }
                if (form.Contains("Akk"))
                {
                    _newform._kasus = Form.Kasus.Akk;
                }
                if (form.Contains("Abl"))
                {
                    _newform._kasus = Form.Kasus.Abl;
                }
                
                if (gen.Contains("m."))
                {
                    _newform._genus = Form.Genus.m;
                }
                if (gen.Contains("f."))
                {
                    _newform._genus = Form.Genus.f;
                }
                if (gen.Contains("n."))
                {
                    _newform._genus = Form.Genus.n;
                }
            }*/
        /*
     wort.setParameter();
     return wort;
 }*/
    }
}
