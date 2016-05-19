using System;
using System.Collections.Generic;

namespace LatinHelper
{
    public class Wort
    {
        public int index = 0;
        public List<Form> _formen = new List<Form>();
        public override string ToString()
        {
            return Latein;
        }
        public bool isForm(Form.Kasus k, Form.Numerus n, Form.Genus g)
        {
            foreach (Form f in _formen)
            {
                if (((f._kasus == k) | (f._kasus == Form.Kasus.Default) | (k == Form.Kasus.Default)) & ((f._numerus == n) | (f._numerus == Form.Numerus.Default) | (n == Form.Numerus.Default)) & ((f._genus == g) | (f._genus == Form.Genus.Default) | (g == Form.Genus.Default)))
                {
                    return true;
                }
            }
            return false;
        }
        public bool isForm(Form form)
        {
            Form.Kasus k = form._kasus;
            Form.Numerus n = form._numerus;
            Form.Genus g = form._genus;
            foreach (Form f in _formen)
            {
                if (((f._kasus == k) | (f._kasus == Form.Kasus.Default) | (k == Form.Kasus.Default)) & ((f._numerus == n) | (f._numerus == Form.Numerus.Default) | (n == Form.Numerus.Default)) & ((f._genus == g) | (f._genus == Form.Genus.Default) | (g == Form.Genus.Default)))
                {
                    return true;
                }
            }
            return false;
        }
        public bool addForm(Form.Kasus k, Form.Numerus n, Form.Genus g)
        {
            Form _form = new Form();
            _form._kasus = k;
            _form._genus = g;
            _form._numerus = n;
            _formen.Add(_form);
            return false;
        }
        public bool addForm(Form f1)
        {
            Form.Kasus k = f1._kasus;
            Form.Numerus n = f1._numerus;
            Form.Genus g = f1._genus;
            Form _form = new Form();
            _form._kasus = k;
            _form._genus = g;
            _form._numerus = n;
            _formen.Add(_form);
            return false;
        }
        public bool isKasus(Form.Kasus k)
        {
            foreach (Form f in _formen)
            {
                if (f._kasus == k)
                {
                    return true;
                }
            }
            return false;
        }
        public bool isNumerus(Form.Numerus n)
        {
            foreach (Form f in _formen)
            {
                if (f._numerus == n)
                {
                    return true;
                }
            }
            return false;
        }
        public bool isGenus(Form.Genus g)
        {
            foreach (Form f in _formen)
            {
                if (f._genus == g)
                {
                    return true;
                }
            }
            return false;
        }
        public string Latein = "";
        public bool infinitiv = false;
        public bool canPraedikat = false;
        public bool canSubjekt = false;
        public Form formen = new Form();
        public void setParameter()
        {
            foreach (Form f in _formen)
            {
                if ((f._wortart == Form.Wortart.Verb) & (!infinitiv))
                {
                    canPraedikat = true;
                }
                if ((f._wortart == Form.Wortart.Substantiv) & (isKasus(Form.Kasus.Nom)))
                {
                    canSubjekt = true;
                }
            }
        }
        public bool KNG_kongruent(Wort w)
        {
            if (w._formen.Count == 0)
            {
                return true;
            }
            if (_formen.Count == 0)
            {
                return true;
            }
            foreach (Form f1 in w._formen)
            {
                if (isForm(f1._kasus, f1._numerus, f1._genus))
                {
                    return true;
                }
            }
            return false;
        }
        public string KNG_kongruenz(Wort w)
        {
            String ret = "";
            foreach (Form f1 in w._formen)
            {
                if (isForm(f1._kasus, f1._numerus, f1._genus))
                {
                    ret += Form2string(f1._kasus, f1._numerus, f1._genus) + ", ";
                }
            }
            return ret;
        }
        string z = "-";
        public string Output()
        {
            z = " ";
            String ret = "";
            ret += Latein + z;
            ret += ": " + z;
            if (_formen.Count == 0)
            {
                ret += "Def." + z;
                ret += "Def." + z;
                ret += "Def." + z;
            }
            else
            {
                ret += Form2string(_formen[0]);
                for (int i = 1; i < _formen.Count; i++)
                {
                    ret += ", " + z;
                    ret += Form2string(_formen[i]);
                }
            }return ret;
        }     
        public string Form2string(Form.Kasus kasus, Form.Numerus numerus, Form.Genus genus)
        {
            string ret = "";
            switch (kasus)
            {
                case Form.Kasus.Nom:
                    ret += "Nom." + z;
                    break;
                case Form.Kasus.Gen:
                    ret += "Gen." + z;
                    break;
                case Form.Kasus.Dat:
                    ret += "Dat." + z;
                    break;
                case Form.Kasus.Akk:
                    ret += "Akk." + z;
                    break;
                case Form.Kasus.Abl:
                    ret += "Abl." + z;
                    break;
                default:
                    break;
            }
            switch (numerus)
            {
                case Form.Numerus.Sg:
                    ret += "Sg." + z;
                    break;
                case Form.Numerus.Pl:
                    ret += "Pl." + z;
                    break;
                default:
                    break;
            }
            switch (genus)
            {
                case Form.Genus.m:
                    ret += "m." + z;
                    break;
                case Form.Genus.f:
                    ret += "f." + z;
                    break;
                case Form.Genus.n:
                    ret += "n." + z;
                    break;
                default:
                    break;
            }
            return ret;
        }
        public string Form2string(Form.Person person, Form.Numerus numerus, Form.Tempus tempus, Form.Modus modus, Form.GenusVerbi genusVerbi)
        {
            string ret = "";
            switch (person)
            {
                case Form.Person.P1:
                    ret += "1. Pers" + z;
                    break;
                case Form.Person.P2:
                    ret += "2. Pers" + z;
                    break;
                case Form.Person.P3:
                    ret += "3. Pers" + z;
                    break;
                default:
                    break;
            }
            switch (numerus)
            {
                case Form.Numerus.Sg:
                    ret += "Sg." + z;
                    break;
                case Form.Numerus.Pl:
                    ret += "Pl." + z;
                    break;
                default:
                    break;
            }
            switch (tempus)
            {
                case Form.Tempus.Praesens:
                    ret += "Präsens" + z;
                    break;
                case Form.Tempus.Perfekt:
                    ret += "Perfekt" + z;
                    break;
                case Form.Tempus.Imperfekt:
                    ret += "Imperfekt" + z;
                    break;
                case Form.Tempus.PLQPF:
                    ret += "Plusquamperfekt" + z;
                    break;
                case Form.Tempus.Futur1:
                    ret += "Futur 1" + z;
                    break;
                case Form.Tempus.Futur2:
                    ret += "Futur 2" + z;
                    break;
                default:
                    break;
            }
            switch (modus)
            {
                case Form.Modus.Indikativ:
                    ret += "Ind." + z;
                    break;
                case Form.Modus.Konjunktiv:
                    ret += "Konj." + z;
                    break;
                default:
                    break;
            }
            switch (genusVerbi)
            {
                case Form.GenusVerbi.Aktiv:
                    ret += "Aktiv" + z;
                    break;
                case Form.GenusVerbi.Passiv:
                    ret += "Passiv" + z;
                    break;
                default:
                    break;
            }
            return ret;
        }
        public string Form2string(Form f)
        {
            string ret = "";
            switch (f._typ)
            {
                case Form.Typ.Verb:
                    ret = Form2string(f._person,f._numerus,f._tempus,f._modus,f._genusVerbi);
                    break;
                case Form.Typ.Nomen:
                    ret = Form2string(f._kasus, f._numerus, f._genus);
                    break;
                default:
                    break;
            }
            return ret;
        }
        public string OutputNew()
        {
            String ret = "";
            ret += Latein;
            ret += "#";
            if (_formen.Count == 0)
            {
                ret += Form2stringNew(new Form());
            }
            else
            {
                ret += Form2stringNew(_formen[0]);
                for (int i = 1; i < _formen.Count; i++)
                {
                    ret += ";";
                    ret += Form2stringNew(_formen[i]);
                }
            }
            ret += "#";
            return ret;
        }
        public string Form2stringNew(Form f)
        {
            String ret = (int)f._wortart + z;
            switch (f._typ)
            {
                case Form.Typ.Nomen:
                    ret += (int)f._kasus + z + (int)f._numerus + z + (int)f._genus;
                    break;
                case Form.Typ.Verb:
                    ret += (int)f._person + z + (int)f._numerus + z + (int)f._tempus + z + (int)f._modus + z + (int)f._genusVerbi;
                    break;
                case Form.Typ.Gerundium:
                    ret += (int)f._kasus;
                    break;
                case Form.Typ.Default:
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
}
