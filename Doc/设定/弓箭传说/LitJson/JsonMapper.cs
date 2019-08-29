namespace LitJson
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class JsonMapper
    {
        private static int max_nesting_depth = 100;
        private static IFormatProvider datetime_format = DateTimeFormatInfo.InvariantInfo;
        private static IDictionary<Type, ExporterFunc> base_exporters_table = new Dictionary<Type, ExporterFunc>();
        private static IDictionary<Type, ExporterFunc> custom_exporters_table = new Dictionary<Type, ExporterFunc>();
        private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
        private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
        private static IDictionary<Type, ArrayMetadata> array_metadata = new Dictionary<Type, ArrayMetadata>();
        private static readonly object array_metadata_lock = new object();
        private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
        private static readonly object conv_ops_lock = new object();
        private static IDictionary<Type, ObjectMetadata> object_metadata = new Dictionary<Type, ObjectMetadata>();
        private static readonly object object_metadata_lock = new object();
        private static IDictionary<Type, IList<PropertyMetadata>> type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
        private static readonly object type_properties_lock = new object();
        private static JsonWriter static_writer = new JsonWriter();
        private static readonly object static_writer_lock = new object();
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache0;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache1;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache2;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache3;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache4;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache5;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache6;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache7;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache8;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache9;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheA;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheB;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheC;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheD;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheE;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cacheF;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache10;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache11;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache12;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache13;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache14;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache15;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache16;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache17;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache18;

        static JsonMapper()
        {
            RegisterBaseExporters();
            RegisterBaseImporters();
        }

        private static void AddArrayMetadata(Type type)
        {
            if (!array_metadata.ContainsKey(type))
            {
                ArrayMetadata metadata = new ArrayMetadata {
                    IsArray = type.IsArray
                };
                if (HasInterface(type, "System.Collections.IList"))
                {
                    metadata.IsList = true;
                }
                foreach (PropertyInfo info in GetPublicInstanceProperties(type))
                {
                    if (info.Name == "Item")
                    {
                        ParameterInfo[] indexParameters = info.GetIndexParameters();
                        if ((indexParameters.Length == 1) && (indexParameters[0].ParameterType == typeof(int)))
                        {
                            metadata.ElementType = info.PropertyType;
                        }
                    }
                }
                object obj2 = array_metadata_lock;
                lock (obj2)
                {
                    try
                    {
                        array_metadata.Add(type, metadata);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private static void AddObjectMetadata(Type type)
        {
            if (!object_metadata.ContainsKey(type))
            {
                ObjectMetadata metadata = new ObjectMetadata();
                if (HasInterface(type, "System.Collections.IDictionary"))
                {
                    metadata.IsDictionary = true;
                }
                metadata.Properties = new Dictionary<string, PropertyMetadata>();
                foreach (PropertyInfo info in GetPublicInstanceProperties(type))
                {
                    if (info.Name == "Item")
                    {
                        ParameterInfo[] indexParameters = info.GetIndexParameters();
                        if ((indexParameters.Length == 1) && (indexParameters[0].ParameterType == typeof(string)))
                        {
                            metadata.ElementType = info.PropertyType;
                        }
                    }
                    else
                    {
                        PropertyMetadata metadata2 = new PropertyMetadata {
                            Info = info,
                            Type = info.PropertyType
                        };
                        metadata.Properties.Add(info.Name, metadata2);
                    }
                }
                foreach (FieldInfo info2 in type.GetFields())
                {
                    PropertyMetadata metadata3 = new PropertyMetadata {
                        Info = info2,
                        IsField = true,
                        Type = info2.FieldType
                    };
                    metadata.Properties.Add(info2.Name, metadata3);
                }
                object obj2 = object_metadata_lock;
                lock (obj2)
                {
                    try
                    {
                        object_metadata.Add(type, metadata);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private static void AddTypeProperties(Type type)
        {
            if (!type_properties.ContainsKey(type))
            {
                IList<PropertyMetadata> list = new List<PropertyMetadata>();
                foreach (PropertyInfo info in GetPublicInstanceProperties(type))
                {
                    if (info.Name != "Item")
                    {
                        PropertyMetadata item = new PropertyMetadata {
                            Info = info,
                            IsField = false
                        };
                        list.Add(item);
                    }
                }
                foreach (FieldInfo info2 in type.GetFields())
                {
                    PropertyMetadata item = new PropertyMetadata {
                        Info = info2,
                        IsField = true
                    };
                    list.Add(item);
                }
                object obj2 = type_properties_lock;
                lock (obj2)
                {
                    try
                    {
                        type_properties.Add(type, list);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
        }

        private static MethodInfo GetConvOp(Type t1, Type t2)
        {
            object obj2 = conv_ops_lock;
            lock (obj2)
            {
                if (!conv_ops.ContainsKey(t1))
                {
                    conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
                }
            }
            if (conv_ops[t1].ContainsKey(t2))
            {
                return conv_ops[t1][t2];
            }
            Type[] types = new Type[] { t2 };
            MethodInfo method = t1.GetMethod("op_Implicit", types);
            object obj3 = conv_ops_lock;
            lock (obj3)
            {
                try
                {
                    conv_ops[t1].Add(t2, method);
                }
                catch (ArgumentException)
                {
                    return conv_ops[t1][t2];
                }
            }
            return method;
        }

        public static PropertyInfo[] GetPublicInstanceProperties(Type type) => 
            type.GetProperties();

        private static bool HasInterface(Type type, string name) => 
            (type.GetInterface(name, true) != null);

        private static void ReadSkip(JsonReader reader)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (WrapperFactory) (() => new JsonMockWrapper());
            }
            ToWrapper(<>f__am$cache0, reader);
        }

        private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
        {
            reader.Read();
            if ((reader.Token == JsonToken.ArrayEnd) || (reader.Token == JsonToken.Null))
            {
                return null;
            }
            IJsonWrapper wrapper = factory();
            if (reader.Token == JsonToken.String)
            {
                wrapper.SetString((string) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Double)
            {
                wrapper.SetDouble((double) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Int)
            {
                wrapper.SetInt((int) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Long)
            {
                wrapper.SetLong((long) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Boolean)
            {
                wrapper.SetBoolean((bool) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.ArrayStart)
            {
                wrapper.SetJsonType(JsonType.Array);
                while (true)
                {
                    IJsonWrapper wrapper2 = ReadValue(factory, reader);
                    if ((wrapper2 == null) && (reader.Token == JsonToken.ArrayEnd))
                    {
                        return wrapper;
                    }
                    wrapper.Add(wrapper2);
                }
            }
            if (reader.Token != JsonToken.ObjectStart)
            {
                return wrapper;
            }
            wrapper.SetJsonType(JsonType.Object);
            while (true)
            {
                reader.Read();
                if (reader.Token == JsonToken.ObjectEnd)
                {
                    return wrapper;
                }
                string str = (string) reader.Value;
                wrapper[str] = ReadValue(factory, reader);
            }
        }

        private static object ReadValue(Type inst_type, JsonReader reader)
        {
            IList list;
            Type elementType;
            object obj3;
            reader.Read();
            if (reader.Token == JsonToken.ArrayEnd)
            {
                return null;
            }
            if (reader.Token == JsonToken.Null)
            {
                if (!inst_type.IsClass)
                {
                    throw new JsonException($"Can't assign null to an instance of type {inst_type}");
                }
                return null;
            }
            if (((reader.Token == JsonToken.Double) || (reader.Token == JsonToken.Int)) || (((reader.Token == JsonToken.Long) || (reader.Token == JsonToken.String)) || (reader.Token == JsonToken.Boolean)))
            {
                Type c = reader.Value.GetType();
                if (inst_type.IsAssignableFrom(c))
                {
                    return reader.Value;
                }
                if (custom_importers_table.ContainsKey(c) && custom_importers_table[c].ContainsKey(inst_type))
                {
                    ImporterFunc func = custom_importers_table[c][inst_type];
                    return func(reader.Value);
                }
                if (base_importers_table.ContainsKey(c) && base_importers_table[c].ContainsKey(inst_type))
                {
                    ImporterFunc func2 = base_importers_table[c][inst_type];
                    return func2(reader.Value);
                }
                if (inst_type.IsEnum)
                {
                    return Enum.ToObject(inst_type, reader.Value);
                }
                MethodInfo convOp = GetConvOp(inst_type, c);
                if (convOp == null)
                {
                    throw new JsonException($"Can't assign value '{reader.Value}' (type {c}) to type {inst_type}");
                }
                object[] parameters = new object[] { reader.Value };
                return convOp.Invoke(null, parameters);
            }
            object obj2 = null;
            if (reader.Token != JsonToken.ArrayStart)
            {
                goto Label_02B1;
            }
            if (inst_type.FullName == "System.Object")
            {
                inst_type = typeof(object[]);
            }
            AddArrayMetadata(inst_type);
            ArrayMetadata metadata = array_metadata[inst_type];
            if (!metadata.IsArray && !metadata.IsList)
            {
                throw new JsonException($"Type {inst_type} can't act as an array");
            }
            if (!metadata.IsArray)
            {
                list = (IList) Activator.CreateInstance(inst_type);
                elementType = metadata.ElementType;
            }
            else
            {
                list = new ArrayList();
                elementType = inst_type.GetElementType();
            }
        Label_0224:
            obj3 = ReadValue(elementType, reader);
            if ((obj3 != null) || (reader.Token != JsonToken.ArrayEnd))
            {
                list.Add(obj3);
                goto Label_0224;
            }
            if (!metadata.IsArray)
            {
                return list;
            }
            int count = list.Count;
            obj2 = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                ((Array) obj2).SetValue(list[i], i);
            }
            return obj2;
        Label_02B1:
            if (reader.Token != JsonToken.ObjectStart)
            {
                return obj2;
            }
            if (inst_type == typeof(object))
            {
                inst_type = typeof(Dictionary<string, object>);
            }
            AddObjectMetadata(inst_type);
            ObjectMetadata metadata2 = object_metadata[inst_type];
            obj2 = Activator.CreateInstance(inst_type);
            while (true)
            {
                reader.Read();
                if (reader.Token == JsonToken.ObjectEnd)
                {
                    return obj2;
                }
                string key = (string) reader.Value;
                if (metadata2.Properties.ContainsKey(key))
                {
                    PropertyMetadata metadata3 = metadata2.Properties[key];
                    if (metadata3.IsField)
                    {
                        ((FieldInfo) metadata3.Info).SetValue(obj2, ReadValue(metadata3.Type, reader));
                    }
                    else
                    {
                        PropertyInfo info = (PropertyInfo) metadata3.Info;
                        if (info.CanWrite)
                        {
                            info.SetValue(obj2, ReadValue(metadata3.Type, reader), null);
                        }
                        else
                        {
                            ReadValue(metadata3.Type, reader);
                        }
                    }
                }
                else if (!metadata2.IsDictionary)
                {
                    if (!reader.SkipNonMembers)
                    {
                        throw new JsonException($"The type {inst_type} doesn't have the property '{key}'");
                    }
                    ReadSkip(reader);
                }
                else
                {
                    ((IDictionary) obj2).Add(key, ReadValue(metadata2.ElementType, reader));
                }
            }
        }

        private static void RegisterBaseExporters()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (obj, writer) => writer.Write(Convert.ToInt32((byte) obj));
            }
            base_exporters_table[typeof(byte)] = <>f__am$cache1;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = (obj, writer) => writer.Write(Convert.ToString((char) obj));
            }
            base_exporters_table[typeof(char)] = <>f__am$cache2;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = (obj, writer) => writer.Write(Convert.ToString((DateTime) obj, datetime_format));
            }
            base_exporters_table[typeof(DateTime)] = <>f__am$cache3;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = (obj, writer) => writer.Write((decimal) obj);
            }
            base_exporters_table[typeof(decimal)] = <>f__am$cache4;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = (obj, writer) => writer.Write(Convert.ToInt32((sbyte) obj));
            }
            base_exporters_table[typeof(sbyte)] = <>f__am$cache5;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = (obj, writer) => writer.Write(Convert.ToInt32((short) obj));
            }
            base_exporters_table[typeof(short)] = <>f__am$cache6;
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = (obj, writer) => writer.Write(Convert.ToInt32((ushort) obj));
            }
            base_exporters_table[typeof(ushort)] = <>f__am$cache7;
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = (obj, writer) => writer.Write(Convert.ToUInt64((uint) obj));
            }
            base_exporters_table[typeof(uint)] = <>f__am$cache8;
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = (obj, writer) => writer.Write((ulong) obj);
            }
            base_exporters_table[typeof(ulong)] = <>f__am$cache9;
        }

        private static void RegisterBaseImporters()
        {
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = (ImporterFunc) (input => Convert.ToByte((int) input));
            }
            ImporterFunc importer = <>f__am$cacheA;
            RegisterImporter(base_importers_table, typeof(int), typeof(byte), importer);
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = (ImporterFunc) (input => Convert.ToUInt64((int) input));
            }
            importer = <>f__am$cacheB;
            RegisterImporter(base_importers_table, typeof(int), typeof(ulong), importer);
            if (<>f__am$cacheC == null)
            {
                <>f__am$cacheC = (ImporterFunc) (input => Convert.ToSByte((int) input));
            }
            importer = <>f__am$cacheC;
            RegisterImporter(base_importers_table, typeof(int), typeof(sbyte), importer);
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = (ImporterFunc) (input => Convert.ToInt16((int) input));
            }
            importer = <>f__am$cacheD;
            RegisterImporter(base_importers_table, typeof(int), typeof(short), importer);
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = (ImporterFunc) (input => Convert.ToUInt16((int) input));
            }
            importer = <>f__am$cacheE;
            RegisterImporter(base_importers_table, typeof(int), typeof(ushort), importer);
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = (ImporterFunc) (input => Convert.ToUInt32((int) input));
            }
            importer = <>f__am$cacheF;
            RegisterImporter(base_importers_table, typeof(int), typeof(uint), importer);
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = (ImporterFunc) (input => Convert.ToSingle((int) input));
            }
            importer = <>f__am$cache10;
            RegisterImporter(base_importers_table, typeof(int), typeof(float), importer);
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = (ImporterFunc) (input => Convert.ToDouble((int) input));
            }
            importer = <>f__am$cache11;
            RegisterImporter(base_importers_table, typeof(int), typeof(double), importer);
            if (<>f__am$cache12 == null)
            {
                <>f__am$cache12 = (ImporterFunc) (input => Convert.ToDecimal((double) input));
            }
            importer = <>f__am$cache12;
            RegisterImporter(base_importers_table, typeof(double), typeof(decimal), importer);
            if (<>f__am$cache13 == null)
            {
                <>f__am$cache13 = (ImporterFunc) (input => Convert.ToUInt32((long) input));
            }
            importer = <>f__am$cache13;
            RegisterImporter(base_importers_table, typeof(long), typeof(uint), importer);
            if (<>f__am$cache14 == null)
            {
                <>f__am$cache14 = (ImporterFunc) (input => Convert.ToChar((string) input));
            }
            importer = <>f__am$cache14;
            RegisterImporter(base_importers_table, typeof(string), typeof(char), importer);
            if (<>f__am$cache15 == null)
            {
                <>f__am$cache15 = (ImporterFunc) (input => Convert.ToDateTime((string) input, datetime_format));
            }
            importer = <>f__am$cache15;
            RegisterImporter(base_importers_table, typeof(string), typeof(DateTime), importer);
        }

        public static void RegisterExporter<T>(ExporterFunc<T> exporter)
        {
            <RegisterExporter>c__AnonStorey0<T> storey = new <RegisterExporter>c__AnonStorey0<T> {
                exporter = exporter
            };
            ExporterFunc func = new ExporterFunc(storey.<>m__0);
            custom_exporters_table[typeof(T)] = func;
        }

        public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
        {
            <RegisterImporter>c__AnonStorey1<TJson, TValue> storey = new <RegisterImporter>c__AnonStorey1<TJson, TValue> {
                importer = importer
            };
            ImporterFunc func = new ImporterFunc(storey.<>m__0);
            RegisterImporter(custom_importers_table, typeof(TJson), typeof(TValue), func);
        }

        private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
        {
            if (!table.ContainsKey(json_type))
            {
                table.Add(json_type, new Dictionary<Type, ImporterFunc>());
            }
            table[json_type][value_type] = importer;
        }

        public static string ToJson(object obj)
        {
            object obj2 = static_writer_lock;
            lock (obj2)
            {
                static_writer.Reset();
                WriteValue(obj, static_writer, true, 0);
                return static_writer.ToString();
            }
        }

        public static void ToJson(object obj, JsonWriter writer)
        {
            WriteValue(obj, writer, false, 0);
        }

        public static JsonData ToObject(JsonReader reader)
        {
            if (<>f__am$cache16 == null)
            {
                <>f__am$cache16 = (WrapperFactory) (() => new JsonData());
            }
            return (JsonData) ToWrapper(<>f__am$cache16, reader);
        }

        public static T ToObject<T>(JsonReader reader) => 
            ((T) ReadValue(typeof(T), reader));

        public static JsonData ToObject(TextReader reader)
        {
            JsonReader reader2 = new JsonReader(reader);
            if (<>f__am$cache17 == null)
            {
                <>f__am$cache17 = (WrapperFactory) (() => new JsonData());
            }
            return (JsonData) ToWrapper(<>f__am$cache17, reader2);
        }

        public static T ToObject<T>(TextReader reader)
        {
            JsonReader reader2 = new JsonReader(reader);
            return (T) ReadValue(typeof(T), reader2);
        }

        public static JsonData ToObject(string json)
        {
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = (WrapperFactory) (() => new JsonData());
            }
            return (JsonData) ToWrapper(<>f__am$cache18, json);
        }

        public static T ToObject<T>(string json)
        {
            JsonReader reader = new JsonReader(json);
            return (T) ReadValue(typeof(T), reader);
        }

        public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader) => 
            ReadValue(factory, reader);

        public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
        {
            JsonReader reader = new JsonReader(json);
            return ReadValue(factory, reader);
        }

        public static void UnregisterExporters()
        {
            custom_exporters_table.Clear();
        }

        public static void UnregisterImporters()
        {
            custom_importers_table.Clear();
        }

        private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
        {
            if (depth > max_nesting_depth)
            {
                throw new JsonException($"Max allowed object depth reached while trying to export from type {obj.GetType()}");
            }
            if (obj == null)
            {
                writer.Write((string) null);
            }
            else if (obj is IJsonWrapper)
            {
                if (writer_is_private)
                {
                    writer.TextWriter.Write(((IJsonWrapper) obj).ToJson());
                }
                else
                {
                    ((IJsonWrapper) obj).ToJson(writer);
                }
            }
            else
            {
                switch (obj)
                {
                    case (string _):
                        writer.Write((string) obj);
                        break;

                    case (double _):
                        writer.Write((double) obj);
                        break;

                    case (int _):
                        writer.Write((int) obj);
                        break;

                    case (bool _):
                        writer.Write((bool) obj);
                        break;

                    case (long _):
                        writer.Write((long) obj);
                        break;

                    case (Array _):
                    {
                        writer.WriteArrayStart();
                        IEnumerator enumerator = ((Array) obj).GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                object current = enumerator.Current;
                                WriteValue(current, writer, writer_is_private, depth + 1);
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
                        writer.WriteArrayEnd();
                        break;
                    }
                    default:
                        if (obj is IList)
                        {
                            writer.WriteArrayStart();
                            IEnumerator enumerator2 = ((IList) obj).GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    object current = enumerator2.Current;
                                    WriteValue(current, writer, writer_is_private, depth + 1);
                                }
                            }
                            finally
                            {
                                if (enumerator2 is IDisposable disposable2)
                                {
                                    IDisposable disposable2;
                                    disposable2.Dispose();
                                }
                            }
                            writer.WriteArrayEnd();
                        }
                        else if (obj is IDictionary)
                        {
                            writer.WriteObjectStart();
                            IDictionaryEnumerator enumerator3 = ((IDictionary) obj).GetEnumerator();
                            try
                            {
                                while (enumerator3.MoveNext())
                                {
                                    DictionaryEntry current = (DictionaryEntry) enumerator3.Current;
                                    writer.WritePropertyName((string) current.Key);
                                    WriteValue(current.Value, writer, writer_is_private, depth + 1);
                                }
                            }
                            finally
                            {
                                if (enumerator3 is IDisposable disposable3)
                                {
                                    IDisposable disposable3;
                                    disposable3.Dispose();
                                }
                            }
                            writer.WriteObjectEnd();
                        }
                        else
                        {
                            Type key = obj.GetType();
                            if (custom_exporters_table.ContainsKey(key))
                            {
                                ExporterFunc func = custom_exporters_table[key];
                                func(obj, writer);
                            }
                            else if (base_exporters_table.ContainsKey(key))
                            {
                                ExporterFunc func2 = base_exporters_table[key];
                                func2(obj, writer);
                            }
                            else if (obj is Enum)
                            {
                                Type underlyingType = Enum.GetUnderlyingType(key);
                                if (((underlyingType == typeof(long)) || (underlyingType == typeof(uint))) || (underlyingType == typeof(ulong)))
                                {
                                    writer.Write((ulong) obj);
                                }
                                else
                                {
                                    writer.Write((int) obj);
                                }
                            }
                            else
                            {
                                AddTypeProperties(key);
                                IList<PropertyMetadata> list = type_properties[key];
                                writer.WriteObjectStart();
                                foreach (PropertyMetadata metadata in list)
                                {
                                    if (metadata.IsField)
                                    {
                                        writer.WritePropertyName(metadata.Info.Name);
                                        WriteValue(((FieldInfo) metadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
                                    }
                                    else
                                    {
                                        PropertyInfo info = (PropertyInfo) metadata.Info;
                                        if (info.CanRead)
                                        {
                                            writer.WritePropertyName(metadata.Info.Name);
                                            WriteValue(info.GetValue(obj, null), writer, writer_is_private, depth + 1);
                                        }
                                    }
                                }
                                writer.WriteObjectEnd();
                            }
                        }
                        break;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterExporter>c__AnonStorey0<T>
        {
            internal ExporterFunc<T> exporter;

            internal void <>m__0(object obj, JsonWriter writer)
            {
                this.exporter((T) obj, writer);
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterImporter>c__AnonStorey1<TJson, TValue>
        {
            internal ImporterFunc<TJson, TValue> importer;

            internal object <>m__0(object input) => 
                this.importer((TJson) input);
        }
    }
}

