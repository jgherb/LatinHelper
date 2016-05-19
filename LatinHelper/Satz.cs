using System;
using System.Collections.Generic;

namespace LatinHelper
{
    class Satz
    {
        public Satz()
        {
            wortgrArray = new Dictionary<string, WortGruppe>();
            wortliste = new Dictionary<string, Wort>();
        }
        public Satz(String s)
        {
            _satz = s;
            wortgrArray = new Dictionary<string, WortGruppe>();
            wortliste = new Dictionary<string, Wort>();
        }
        public Dictionary<string, WortGruppe> wortgrArray;
        public Dictionary<string, Wort> wortliste;
        Dictionary<string, Wort> praedikatListe;
        Dictionary<string, Wort> subjektListe;
        public string _satz = "";
        public string OutputWortGruppen(bool b)
        {
            String ret = "";
            foreach (WortGruppe wg in wortgrArray.Values)
            {
                ret += wg.getLatein();
                ret += "\n";
                ret += "----------------------------------------------------";
                ret += "\n";
            }
            return ret;
        }
        public void Sortieren()
        {
            CreateWortList();
            //SubjektPraedikatSortieren();
            KNG_sortieren_Neu();
            //ObjekteSortieren();
            DuplikateDelete();
        }
        void DuplikateDelete()
        {
            List<string> keys2delete = new List<string>();
            foreach (WortGruppe w in wortgrArray.Values)
            {
                if (keys2delete.Contains(w.getLatein()))
                {
                    continue;
                }
                foreach (WortGruppe v in wortgrArray.Values)
                {
                    if (keys2delete.Contains(v.getLatein()))
                    {
                        continue;
                    }
                    if (w.getLatein().Equals(v.getLatein()))
                    {
                        continue;
                    }
                    if (v.getIndex().Equals(w.getIndex()))
                    {
                        keys2delete.Add(w.getLatein());
                    }
                }
            }
            foreach (String s in keys2delete)
            {
                wortgrArray.Remove(s);
            }
        }
        void ObjekteSortieren()
        {
            Dictionary<string, Wort> objektl = getObjekte();
            WortGruppe wg = new WortGruppe();
            foreach (Wort w in objektl.Values)
            {
                wg = new WortGruppe();
                wg._wortliste.Add(w.Latein, w);
                foreach (Wort v in objektl.Values)
                {
                    if (v.Equals(w))
                    {
                        continue;
                    }
                    if (w.KNG_kongruent(v))
                    {
                        wg._wortliste.Add(v.Latein, v);
                    }
                }
                wortgrArray.Add(wg.getLatein(), wg);
            }
        }
        void KNG_sortieren() 
        {
            Dictionary<string, Wort> objektl = wortliste;
            WortGruppe wg = new WortGruppe();
            foreach (Wort w in objektl.Values)
            {
                wg = new WortGruppe();
                wg._wortliste.Add(w.Latein, w);
                foreach (Wort v in objektl.Values)
                {
                    if (v.Equals(w))
                    {
                        continue;
                    }
                    if (wg.KNG_kongruent(v))
                    {
                        wg._wortliste.Add(v.Latein, v);
                    }
                }
                wortgrArray.Add(wg.getLatein(), wg);
            }
        }
        void KNG_sortieren_Neu() 
        {
            Dictionary<string, Wort> objektl = wortliste;
            WortGruppe wg = new WortGruppe();
            foreach (Wort w in objektl.Values)
            {
                foreach (Form f in w._formen)
                {
                    if(f._typ!=Form.Typ.Nomen)
                    {
                        continue;
                    }
                    wg = new WortGruppe();
                    wg._wortliste.Add(w.Latein, w);
                    wg.gemeinsameForm = f;
                    foreach (Wort v in objektl.Values)
                    {
                        if (v.Equals(w))
                        {
                            continue;
                        }
                        if (v.isForm(f))
                        {
                            if (!wg._wortliste.ContainsKey(v.Latein))
                            {
                                wg._wortliste.Add(v.Latein, v);
                            }
                        }
                    }
                    if (!wortgrArray.ContainsKey(wg.getLatein()))
                    {
                        wortgrArray.Add(wg.getLatein(), wg);
                    }
                }
            }
        }
        void SubjektPraedikatSortieren()
        {
            Dictionary<string, Wort> praedikatl = getPraedikat();
            Dictionary<string, Wort> subjektl = getSubjekt();
            subjektListe = subjektl;
            praedikatListe = praedikatl;
            WortGruppe wg = new WortGruppe();
            foreach (Wort w in praedikatl.Values)
            {
                wg = new WortGruppe();
                wg._wortliste.Add(w.Latein, w);
                foreach (Wort v in subjektl.Values)
                {
                    if (w.KNG_kongruent(v))
                    {
                        wg._wortliste.Add(v.Latein, v);
                    }
                }
                wortgrArray.Add(wg.getLatein(), wg);
            }
        }
        void CreateWortList()
        {
            _satz = _satz.ToLower();
            _satz = _satz.Replace(".", "");
            String[] wortl = _satz.Split(' ');
            int zaehler = 0;
            foreach (String wl in wortl)
            {
                Wort w = LatinDict.getVokabel(wl,true);
                w.index = zaehler;
                wortliste.Add(wl, w);
                zaehler++;
            }
        }
        public Dictionary<string, Wort> getObjekte()
        {
            Dictionary<string, Wort> ret = new Dictionary<string, Wort>();
            foreach (Wort w in wortliste.Values)
            {
                if (!(praedikatListe.ContainsValue(w) | subjektListe.ContainsValue(w)))
                {
                    ret.Add(w.Latein, w);
                }
            }
            return ret;
        }
        public Dictionary<string, Wort> getSubjekt()
        {
            Dictionary<string, Wort> ret = new Dictionary<string, Wort>();
            foreach (Wort w in wortliste.Values)
            {
                if (w.canSubjekt)
                {
                    ret.Add(w.Latein, w);
                }
            }
            return ret;
        }
        public Dictionary<string, Wort> getPraedikat()
        {
            Dictionary<string, Wort> ret = new Dictionary<string, Wort>();
            foreach (Wort w in wortliste.Values)
            {
                if (w.canPraedikat)
                {
                    ret.Add(w.Latein, w);
                }
            }
            return ret;
        }
    }
}
