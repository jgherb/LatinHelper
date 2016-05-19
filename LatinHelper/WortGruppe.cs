using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinHelper
{
    class WortGruppe
    {
        public void sort()
        {
            var list = _wortliste.Keys.ToList();
            list.Sort();
            Dictionary<string, Wort> newdict = new Dictionary<string, Wort>();
            foreach (string s in list)
            {
                newdict.Add(s, _wortliste[s]);
            }
            _wortliste = newdict;
        }
        public override string ToString()
        {
            return getLatein();
        }
        public String getIndex()
        {
            string ret = "";
            List<int> indexlist = new List<int>();
            foreach (Wort w in _wortliste.Values)
            {
                indexlist.Add(w.index);
            }
            indexlist.Sort();
            foreach (int i in indexlist)
            {
                ret += i + '-';
            }
            ret += gemeinsameForm;
            return ret;
        }
        public List<Form> gemeinsameFormen = new List<Form>();
        public Form gemeinsameForm = new Form();
        public Dictionary<string,Wort> _wortliste;
        string _wortgruppe = "";
        public WortGruppe()
        {
            _wortliste = new Dictionary<string, Wort>();
        }
        public String getLatein()
        {
            String ret = "";
            foreach(String s in _wortliste.Keys)
            {
                ret += s + " ";
            }
            ret = ret.Substring(0,ret.Length - 1);
            ret += " (" + gemeinsameForm + ")";
            return ret;
        }
        public bool KNG_kongruent(Wort w)
        {
            gemeinsameFormen.Clear();
            foreach(Wort v in _wortliste.Values)
            {
                if (w._formen.Count == 0)
                {
                    return true;
                }
                if (v._formen.Count == 0)
                {
                    return true;
                }
                foreach (Form f1 in w._formen)
                {
                    if (v.isForm(f1._kasus, f1._numerus, f1._genus))
                    {
                        gemeinsameFormen.Add(f1);
                    }
                    else
                    {
                        goto Verlassen;
                    }
                }
                continue;
                Verlassen:
                return false;
            }
            return true;
        }
    }
}
