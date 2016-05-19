using System;
using System.Collections.Generic;

namespace LatinHelper
{
    public class Form
    {
        public enum Wortart { Default, Substantiv, Verb, Partizip, Adjektiv, Unv, Gerundium, Gerundivum };
        public enum Kasus { Default, Nom, Gen, Dat, Akk, Abl };
        public enum Numerus { Default, Sg, Pl };
        public enum Genus { Default, m, n, f };
        public enum Typ { Default, Nomen, Verb, Gerundium, Unv };
        public enum Person { Default, P1, P2, P3 };
        public enum Tempus { Default, Praesens, Perfekt, Imperfekt, PLQPF, Futur1, Futur2 };
        public enum Modus { Default, Indikativ, Konjunktiv };
        public enum GenusVerbi { Default, Aktiv, Passiv, Medium };
        public Wortart _wortart = Wortart.Default;
        public Kasus _kasus = Kasus.Default;
        public Numerus _numerus = Numerus.Default;
        public Genus _genus = Genus.Default;
        public Typ _typ = Typ.Default;
        public Person _person = Person.Default;
        public Tempus _tempus = Tempus.Default;
        public Modus _modus = Modus.Default;
        public GenusVerbi _genusVerbi = GenusVerbi.Default;
        public List<String> _deutsch = new List<String>();
        public string Grundform = "";
        public override string ToString()
        {
            string ret = "";
            switch (_typ)
            {
                case Form.Typ.Verb:
                    ret = Form2string(_person, _numerus, _tempus, _modus, _genusVerbi);
                    break;
                case Form.Typ.Nomen:
                    ret = Form2string(_kasus, _numerus, _genus);
                    break;
                default:
                    break;
            }
            return ret;
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
        string z = " ";
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
    }
}
