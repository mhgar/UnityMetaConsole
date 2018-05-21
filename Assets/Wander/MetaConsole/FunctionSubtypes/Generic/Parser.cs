using System.Collections.Generic;
using System;
using UnityEngine.Assertions;

namespace Wander.Parsing
{
    public static class StringParser
    {
        public delegate T Parser<T>(string input);
        delegate object Parser(string input);

        static Dictionary<Type, Parser> parsers 
            = new Dictionary<Type, Parser>();

        static StringParser()
        {
            AddParser<String>((input) => {
                return input;
            });   
            AddParser<SByte>(SByte.Parse);
            AddParser<Byte>(Byte.Parse);
            AddParser<UInt16>(UInt16.Parse);
            AddParser<Int16>(Int16.Parse);
            AddParser<UInt32>(UInt32.Parse);
            AddParser<Int32>(Int32.Parse);
            AddParser<UInt64>(UInt64.Parse);
            AddParser<Int64>(Int64.Parse);
            AddParser<Boolean>(Boolean.Parse);
            AddParser<Single>(Single.Parse);
            AddParser<Double>(Double.Parse);
            AddParser<Decimal>(Decimal.Parse);
        }

        public static bool HasParser(Type t)
        {
            return parsers.ContainsKey(t);
        }

        public static void AddParser<T>(Parser<T> parser)
        {
            Assert.IsFalse(HasParser(typeof(T)));

            parsers.Add(typeof(T), (input) => {
                return (object) parser(input);
            });
        }

        public static  T Parse<T>(string input)
        {
            Type t = typeof(T);
            if (!HasParser(t)) {
                throw new InvalidOperationException("No parser for type " + t);
            }

            return (T)parsers[t](input);
        }

        public static bool TryParse<T>(string input, out T output)
        {
            try {
                output = (T)parsers[typeof(T)](input);
                return true;
            } catch {
                output = default(T);
                return false;
            }
        }
    }
}