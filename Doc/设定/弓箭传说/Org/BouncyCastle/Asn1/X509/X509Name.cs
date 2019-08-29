namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class X509Name : Asn1Encodable
    {
        public static readonly DerObjectIdentifier C = new DerObjectIdentifier("2.5.4.6");
        public static readonly DerObjectIdentifier O = new DerObjectIdentifier("2.5.4.10");
        public static readonly DerObjectIdentifier OU = new DerObjectIdentifier("2.5.4.11");
        public static readonly DerObjectIdentifier T = new DerObjectIdentifier("2.5.4.12");
        public static readonly DerObjectIdentifier CN = new DerObjectIdentifier("2.5.4.3");
        public static readonly DerObjectIdentifier Street = new DerObjectIdentifier("2.5.4.9");
        public static readonly DerObjectIdentifier SerialNumber = new DerObjectIdentifier("2.5.4.5");
        public static readonly DerObjectIdentifier L = new DerObjectIdentifier("2.5.4.7");
        public static readonly DerObjectIdentifier ST = new DerObjectIdentifier("2.5.4.8");
        public static readonly DerObjectIdentifier Surname = new DerObjectIdentifier("2.5.4.4");
        public static readonly DerObjectIdentifier GivenName = new DerObjectIdentifier("2.5.4.42");
        public static readonly DerObjectIdentifier Initials = new DerObjectIdentifier("2.5.4.43");
        public static readonly DerObjectIdentifier Generation = new DerObjectIdentifier("2.5.4.44");
        public static readonly DerObjectIdentifier UniqueIdentifier = new DerObjectIdentifier("2.5.4.45");
        public static readonly DerObjectIdentifier BusinessCategory = new DerObjectIdentifier("2.5.4.15");
        public static readonly DerObjectIdentifier PostalCode = new DerObjectIdentifier("2.5.4.17");
        public static readonly DerObjectIdentifier DnQualifier = new DerObjectIdentifier("2.5.4.46");
        public static readonly DerObjectIdentifier Pseudonym = new DerObjectIdentifier("2.5.4.65");
        public static readonly DerObjectIdentifier DateOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.1");
        public static readonly DerObjectIdentifier PlaceOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.2");
        public static readonly DerObjectIdentifier Gender = new DerObjectIdentifier("1.3.6.1.5.5.7.9.3");
        public static readonly DerObjectIdentifier CountryOfCitizenship = new DerObjectIdentifier("1.3.6.1.5.5.7.9.4");
        public static readonly DerObjectIdentifier CountryOfResidence = new DerObjectIdentifier("1.3.6.1.5.5.7.9.5");
        public static readonly DerObjectIdentifier NameAtBirth = new DerObjectIdentifier("1.3.36.8.3.14");
        public static readonly DerObjectIdentifier PostalAddress = new DerObjectIdentifier("2.5.4.16");
        public static readonly DerObjectIdentifier DmdName = new DerObjectIdentifier("2.5.4.54");
        public static readonly DerObjectIdentifier TelephoneNumber = X509ObjectIdentifiers.id_at_telephoneNumber;
        public static readonly DerObjectIdentifier Name = X509ObjectIdentifiers.id_at_name;
        public static readonly DerObjectIdentifier EmailAddress = PkcsObjectIdentifiers.Pkcs9AtEmailAddress;
        public static readonly DerObjectIdentifier UnstructuredName = PkcsObjectIdentifiers.Pkcs9AtUnstructuredName;
        public static readonly DerObjectIdentifier UnstructuredAddress = PkcsObjectIdentifiers.Pkcs9AtUnstructuredAddress;
        public static readonly DerObjectIdentifier E = EmailAddress;
        public static readonly DerObjectIdentifier DC = new DerObjectIdentifier("0.9.2342.19200300.100.1.25");
        public static readonly DerObjectIdentifier UID = new DerObjectIdentifier("0.9.2342.19200300.100.1.1");
        private static readonly bool[] defaultReverse = new bool[1];
        public static readonly Hashtable DefaultSymbols = new Hashtable();
        public static readonly Hashtable RFC2253Symbols = new Hashtable();
        public static readonly Hashtable RFC1779Symbols = new Hashtable();
        public static readonly Hashtable DefaultLookup = new Hashtable();
        private readonly IList ordering;
        private readonly X509NameEntryConverter converter;
        private IList values;
        private IList added;
        private Asn1Sequence seq;

        static X509Name()
        {
            DefaultSymbols.Add(C, "C");
            DefaultSymbols.Add(O, "O");
            DefaultSymbols.Add(T, "T");
            DefaultSymbols.Add(OU, "OU");
            DefaultSymbols.Add(CN, "CN");
            DefaultSymbols.Add(L, "L");
            DefaultSymbols.Add(ST, "ST");
            DefaultSymbols.Add(SerialNumber, "SERIALNUMBER");
            DefaultSymbols.Add(EmailAddress, "E");
            DefaultSymbols.Add(DC, "DC");
            DefaultSymbols.Add(UID, "UID");
            DefaultSymbols.Add(Street, "STREET");
            DefaultSymbols.Add(Surname, "SURNAME");
            DefaultSymbols.Add(GivenName, "GIVENNAME");
            DefaultSymbols.Add(Initials, "INITIALS");
            DefaultSymbols.Add(Generation, "GENERATION");
            DefaultSymbols.Add(UnstructuredAddress, "unstructuredAddress");
            DefaultSymbols.Add(UnstructuredName, "unstructuredName");
            DefaultSymbols.Add(UniqueIdentifier, "UniqueIdentifier");
            DefaultSymbols.Add(DnQualifier, "DN");
            DefaultSymbols.Add(Pseudonym, "Pseudonym");
            DefaultSymbols.Add(PostalAddress, "PostalAddress");
            DefaultSymbols.Add(NameAtBirth, "NameAtBirth");
            DefaultSymbols.Add(CountryOfCitizenship, "CountryOfCitizenship");
            DefaultSymbols.Add(CountryOfResidence, "CountryOfResidence");
            DefaultSymbols.Add(Gender, "Gender");
            DefaultSymbols.Add(PlaceOfBirth, "PlaceOfBirth");
            DefaultSymbols.Add(DateOfBirth, "DateOfBirth");
            DefaultSymbols.Add(PostalCode, "PostalCode");
            DefaultSymbols.Add(BusinessCategory, "BusinessCategory");
            DefaultSymbols.Add(TelephoneNumber, "TelephoneNumber");
            RFC2253Symbols.Add(C, "C");
            RFC2253Symbols.Add(O, "O");
            RFC2253Symbols.Add(OU, "OU");
            RFC2253Symbols.Add(CN, "CN");
            RFC2253Symbols.Add(L, "L");
            RFC2253Symbols.Add(ST, "ST");
            RFC2253Symbols.Add(Street, "STREET");
            RFC2253Symbols.Add(DC, "DC");
            RFC2253Symbols.Add(UID, "UID");
            RFC1779Symbols.Add(C, "C");
            RFC1779Symbols.Add(O, "O");
            RFC1779Symbols.Add(OU, "OU");
            RFC1779Symbols.Add(CN, "CN");
            RFC1779Symbols.Add(L, "L");
            RFC1779Symbols.Add(ST, "ST");
            RFC1779Symbols.Add(Street, "STREET");
            DefaultLookup.Add("c", C);
            DefaultLookup.Add("o", O);
            DefaultLookup.Add("t", T);
            DefaultLookup.Add("ou", OU);
            DefaultLookup.Add("cn", CN);
            DefaultLookup.Add("l", L);
            DefaultLookup.Add("st", ST);
            DefaultLookup.Add("serialnumber", SerialNumber);
            DefaultLookup.Add("street", Street);
            DefaultLookup.Add("emailaddress", E);
            DefaultLookup.Add("dc", DC);
            DefaultLookup.Add("e", E);
            DefaultLookup.Add("uid", UID);
            DefaultLookup.Add("surname", Surname);
            DefaultLookup.Add("givenname", GivenName);
            DefaultLookup.Add("initials", Initials);
            DefaultLookup.Add("generation", Generation);
            DefaultLookup.Add("unstructuredaddress", UnstructuredAddress);
            DefaultLookup.Add("unstructuredname", UnstructuredName);
            DefaultLookup.Add("uniqueidentifier", UniqueIdentifier);
            DefaultLookup.Add("dn", DnQualifier);
            DefaultLookup.Add("pseudonym", Pseudonym);
            DefaultLookup.Add("postaladdress", PostalAddress);
            DefaultLookup.Add("nameofbirth", NameAtBirth);
            DefaultLookup.Add("countryofcitizenship", CountryOfCitizenship);
            DefaultLookup.Add("countryofresidence", CountryOfResidence);
            DefaultLookup.Add("gender", Gender);
            DefaultLookup.Add("placeofbirth", PlaceOfBirth);
            DefaultLookup.Add("dateofbirth", DateOfBirth);
            DefaultLookup.Add("postalcode", PostalCode);
            DefaultLookup.Add("businesscategory", BusinessCategory);
            DefaultLookup.Add("telephonenumber", TelephoneNumber);
        }

        protected X509Name()
        {
            this.ordering = Platform.CreateArrayList();
            this.values = Platform.CreateArrayList();
            this.added = Platform.CreateArrayList();
        }

        protected X509Name(Asn1Sequence seq)
        {
            this.ordering = Platform.CreateArrayList();
            this.values = Platform.CreateArrayList();
            this.added = Platform.CreateArrayList();
            this.seq = seq;
            IEnumerator enumerator = seq.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Asn1Set instance = Asn1Set.GetInstance(((Asn1Encodable) enumerator.Current).ToAsn1Object());
                    for (int i = 0; i < instance.Count; i++)
                    {
                        Asn1Sequence sequence = Asn1Sequence.GetInstance(instance[i].ToAsn1Object());
                        if (sequence.Count != 2)
                        {
                            throw new ArgumentException("badly sized pair");
                        }
                        this.ordering.Add(DerObjectIdentifier.GetInstance(sequence[0].ToAsn1Object()));
                        Asn1Object obj2 = sequence[1].ToAsn1Object();
                        if ((obj2 is IAsn1String) && !(obj2 is DerUniversalString))
                        {
                            string source = ((IAsn1String) obj2).GetString();
                            if (Platform.StartsWith(source, "#"))
                            {
                                source = @"\" + source;
                            }
                            this.values.Add(source);
                        }
                        else
                        {
                            this.values.Add("#" + Hex.ToHexString(obj2.GetEncoded()));
                        }
                        this.added.Add(i != 0);
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public X509Name(string dirName) : this(DefaultReverse, DefaultLookup, dirName)
        {
        }

        public X509Name(bool reverse, string dirName) : this(reverse, DefaultLookup, dirName)
        {
        }

        public X509Name(IList ordering, IDictionary attributes) : this(ordering, attributes, new X509DefaultEntryConverter())
        {
        }

        public X509Name(IList oids, IList values) : this(oids, values, new X509DefaultEntryConverter())
        {
        }

        public X509Name(string dirName, X509NameEntryConverter converter) : this(DefaultReverse, DefaultLookup, dirName, converter)
        {
        }

        public X509Name(bool reverse, IDictionary lookUp, string dirName) : this(reverse, lookUp, dirName, new X509DefaultEntryConverter())
        {
        }

        public X509Name(bool reverse, string dirName, X509NameEntryConverter converter) : this(reverse, DefaultLookup, dirName, converter)
        {
        }

        public X509Name(IList ordering, IDictionary attributes, X509NameEntryConverter converter)
        {
            this.ordering = Platform.CreateArrayList();
            this.values = Platform.CreateArrayList();
            this.added = Platform.CreateArrayList();
            this.converter = converter;
            IEnumerator enumerator = ordering.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DerObjectIdentifier current = (DerObjectIdentifier) enumerator.Current;
                    object obj2 = attributes[current];
                    if (obj2 == null)
                    {
                        throw new ArgumentException("No attribute for object id - " + current + " - passed to distinguished name");
                    }
                    this.ordering.Add(current);
                    this.added.Add(false);
                    this.values.Add(obj2);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
        }

        public X509Name(IList oids, IList values, X509NameEntryConverter converter)
        {
            this.ordering = Platform.CreateArrayList();
            this.values = Platform.CreateArrayList();
            this.added = Platform.CreateArrayList();
            this.converter = converter;
            if (oids.Count != values.Count)
            {
                throw new ArgumentException("'oids' must be same length as 'values'.");
            }
            for (int i = 0; i < oids.Count; i++)
            {
                this.ordering.Add(oids[i]);
                this.values.Add(values[i]);
                this.added.Add(false);
            }
        }

        public X509Name(bool reverse, IDictionary lookUp, string dirName, X509NameEntryConverter converter)
        {
            this.ordering = Platform.CreateArrayList();
            this.values = Platform.CreateArrayList();
            this.added = Platform.CreateArrayList();
            this.converter = converter;
            X509NameTokenizer tokenizer = new X509NameTokenizer(dirName);
            while (tokenizer.HasMoreTokens())
            {
                string str = tokenizer.NextToken();
                int index = str.IndexOf('=');
                if (index == -1)
                {
                    throw new ArgumentException("badly formated directory string");
                }
                string name = str.Substring(0, index);
                string oid = str.Substring(index + 1);
                DerObjectIdentifier identifier = this.DecodeOid(name, lookUp);
                if (oid.IndexOf('+') > 0)
                {
                    X509NameTokenizer tokenizer2 = new X509NameTokenizer(oid, '+');
                    string str4 = tokenizer2.NextToken();
                    this.ordering.Add(identifier);
                    this.values.Add(str4);
                    this.added.Add(false);
                    while (tokenizer2.HasMoreTokens())
                    {
                        string str5 = tokenizer2.NextToken();
                        int length = str5.IndexOf('=');
                        string str6 = str5.Substring(0, length);
                        string str7 = str5.Substring(length + 1);
                        this.ordering.Add(this.DecodeOid(str6, lookUp));
                        this.values.Add(str7);
                        this.added.Add(true);
                    }
                }
                else
                {
                    this.ordering.Add(identifier);
                    this.values.Add(oid);
                    this.added.Add(false);
                }
            }
            if (reverse)
            {
                IList list = Platform.CreateArrayList();
                IList list2 = Platform.CreateArrayList();
                IList list3 = Platform.CreateArrayList();
                int num3 = 1;
                for (int i = 0; i < this.ordering.Count; i++)
                {
                    if (!((bool) this.added[i]))
                    {
                        num3 = 0;
                    }
                    int index = num3++;
                    list.Insert(index, this.ordering[i]);
                    list2.Insert(index, this.values[i]);
                    list3.Insert(index, this.added[i]);
                }
                this.ordering = list;
                this.values = list2;
                this.added = list3;
            }
        }

        private void AppendValue(StringBuilder buf, IDictionary oidSymbols, DerObjectIdentifier oid, string val)
        {
            string str = (string) oidSymbols[oid];
            if (str != null)
            {
                buf.Append(str);
            }
            else
            {
                buf.Append(oid.Id);
            }
            buf.Append('=');
            int length = buf.Length;
            buf.Append(val);
            int num2 = buf.Length;
            if (Platform.StartsWith(val, @"\#"))
            {
                length += 2;
            }
            while (length != num2)
            {
                if ((((buf[length] == ',') || (buf[length] == '"')) || ((buf[length] == '\\') || (buf[length] == '+'))) || (((buf[length] == '=') || (buf[length] == '<')) || ((buf[length] == '>') || (buf[length] == ';'))))
                {
                    buf.Insert(length++, @"\");
                    num2++;
                }
                length++;
            }
        }

        private static string canonicalize(string s)
        {
            string source = Platform.ToLowerInvariant(s).Trim();
            if (Platform.StartsWith(source, "#"))
            {
                Asn1Object obj2 = decodeObject(source);
                if (obj2 is IAsn1String)
                {
                    source = Platform.ToLowerInvariant(((IAsn1String) obj2).GetString()).Trim();
                }
            }
            return source;
        }

        private static Asn1Object decodeObject(string v)
        {
            Asn1Object obj2;
            try
            {
                obj2 = Asn1Object.FromByteArray(Hex.Decode(v.Substring(1)));
            }
            catch (IOException exception)
            {
                throw new InvalidOperationException("unknown encoding in name: " + exception.Message, exception);
            }
            return obj2;
        }

        private DerObjectIdentifier DecodeOid(string name, IDictionary lookUp)
        {
            if (Platform.StartsWith(Platform.ToUpperInvariant(name), "OID."))
            {
                return new DerObjectIdentifier(name.Substring(4));
            }
            if ((name[0] >= '0') && (name[0] <= '9'))
            {
                return new DerObjectIdentifier(name);
            }
            DerObjectIdentifier identifier = (DerObjectIdentifier) lookUp[Platform.ToLowerInvariant(name)];
            if (identifier == null)
            {
                throw new ArgumentException("Unknown object id - " + name + " - passed to distinguished name");
            }
            return identifier;
        }

        public bool Equivalent(X509Name other)
        {
            if (other == null)
            {
                return false;
            }
            if (other != this)
            {
                int num2;
                int num3;
                int num4;
                int count = this.ordering.Count;
                if (count != other.ordering.Count)
                {
                    return false;
                }
                bool[] flagArray = new bool[count];
                if (this.ordering[0].Equals(other.ordering[0]))
                {
                    num2 = 0;
                    num3 = count;
                    num4 = 1;
                }
                else
                {
                    num2 = count - 1;
                    num3 = -1;
                    num4 = -1;
                }
                for (int i = num2; i != num3; i += num4)
                {
                    bool flag = false;
                    DerObjectIdentifier identifier = (DerObjectIdentifier) this.ordering[i];
                    string str = (string) this.values[i];
                    for (int j = 0; j < count; j++)
                    {
                        if (!flagArray[j])
                        {
                            DerObjectIdentifier identifier2 = (DerObjectIdentifier) other.ordering[j];
                            if (identifier.Equals(identifier2))
                            {
                                string str2 = (string) other.values[j];
                                if (equivalentStrings(str, str2))
                                {
                                    flagArray[j] = true;
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Equivalent(X509Name other, bool inOrder)
        {
            if (!inOrder)
            {
                return this.Equivalent(other);
            }
            if (other == null)
            {
                return false;
            }
            if (other != this)
            {
                int count = this.ordering.Count;
                if (count != other.ordering.Count)
                {
                    return false;
                }
                for (int i = 0; i < count; i++)
                {
                    DerObjectIdentifier identifier = (DerObjectIdentifier) this.ordering[i];
                    DerObjectIdentifier identifier2 = (DerObjectIdentifier) other.ordering[i];
                    if (!identifier.Equals(identifier2))
                    {
                        return false;
                    }
                    string str = (string) this.values[i];
                    string str2 = (string) other.values[i];
                    if (!equivalentStrings(str, str2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool equivalentStrings(string s1, string s2)
        {
            string str = canonicalize(s1);
            string str2 = canonicalize(s2);
            if (!str.Equals(str2))
            {
                str = stripInternalSpaces(str);
                str2 = stripInternalSpaces(str2);
                if (!str.Equals(str2))
                {
                    return false;
                }
            }
            return true;
        }

        public static X509Name GetInstance(object obj)
        {
            if ((obj == null) || (obj is X509Name))
            {
                return (X509Name) obj;
            }
            if (obj == null)
            {
                throw new ArgumentException("null object in factory", "obj");
            }
            return new X509Name(Asn1Sequence.GetInstance(obj));
        }

        public static X509Name GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public IList GetOidList() => 
            Platform.CreateArrayList((ICollection) this.ordering);

        public IList GetValueList() => 
            this.GetValueList(null);

        public IList GetValueList(DerObjectIdentifier oid)
        {
            IList list = Platform.CreateArrayList();
            for (int i = 0; i != this.values.Count; i++)
            {
                if ((oid == null) || oid.Equals(this.ordering[i]))
                {
                    string source = (string) this.values[i];
                    if (Platform.StartsWith(source, @"\#"))
                    {
                        source = source.Substring(1);
                    }
                    list.Add(source);
                }
            }
            return list;
        }

        private static string stripInternalSpaces(string str)
        {
            StringBuilder builder = new StringBuilder();
            if (str.Length != 0)
            {
                char ch = str[0];
                builder.Append(ch);
                for (int i = 1; i < str.Length; i++)
                {
                    char ch2 = str[i];
                    if ((ch != ' ') || (ch2 != ' '))
                    {
                        builder.Append(ch2);
                    }
                    ch = ch2;
                }
            }
            return builder.ToString();
        }

        public override Asn1Object ToAsn1Object()
        {
            if (this.seq == null)
            {
                Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
                Asn1EncodableVector vector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
                DerObjectIdentifier identifier = null;
                for (int i = 0; i != this.ordering.Count; i++)
                {
                    DerObjectIdentifier oid = (DerObjectIdentifier) this.ordering[i];
                    string str = (string) this.values[i];
                    if ((identifier != null) && !((bool) this.added[i]))
                    {
                        Asn1Encodable[] encodableArray1 = new Asn1Encodable[] { new DerSet(vector2) };
                        v.Add(encodableArray1);
                        vector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
                    }
                    Asn1Encodable[] encodableArray2 = new Asn1Encodable[1];
                    Asn1Encodable[] encodableArray3 = new Asn1Encodable[] { oid, this.converter.GetConvertedValue(oid, str) };
                    encodableArray2[0] = new DerSequence(encodableArray3);
                    vector2.Add(encodableArray2);
                    identifier = oid;
                }
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerSet(vector2) };
                v.Add(objs);
                this.seq = new DerSequence(v);
            }
            return this.seq;
        }

        public override string ToString() => 
            this.ToString(DefaultReverse, DefaultSymbols);

        public string ToString(bool reverse, IDictionary oidSymbols)
        {
            ArrayList list = new ArrayList();
            StringBuilder buf = null;
            for (int i = 0; i < this.ordering.Count; i++)
            {
                if ((bool) this.added[i])
                {
                    buf.Append('+');
                    this.AppendValue(buf, oidSymbols, (DerObjectIdentifier) this.ordering[i], (string) this.values[i]);
                }
                else
                {
                    buf = new StringBuilder();
                    this.AppendValue(buf, oidSymbols, (DerObjectIdentifier) this.ordering[i], (string) this.values[i]);
                    list.Add(buf);
                }
            }
            if (reverse)
            {
                list.Reverse();
            }
            StringBuilder builder2 = new StringBuilder();
            if (list.Count > 0)
            {
                builder2.Append(list[0].ToString());
                for (int j = 1; j < list.Count; j++)
                {
                    builder2.Append(',');
                    builder2.Append(list[j].ToString());
                }
            }
            return builder2.ToString();
        }

        public static bool DefaultReverse
        {
            get => 
                defaultReverse[0];
            set => 
                (defaultReverse[0] = value);
        }
    }
}

