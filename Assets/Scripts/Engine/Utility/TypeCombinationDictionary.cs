// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Engine.Utility
{
    public class TypeCombinationDictionary<T>
    {
        private LinkedList<TypeCombinationEntry> _registrations = new LinkedList<TypeCombinationEntry>();

        public IEnumerable<TypeCombinationEntry> GetSupportedRegistrations(Type sourceType, Type destinationType)
        {
            return
                _registrations.Where(
                    r =>
                        r.SourceType.IsAssignableFrom(sourceType) &&
                        r.DestinationType.IsAssignableFrom(destinationType));
        }

        public TypeCombinationEntry GetSupportedRegistration(Type sourceType, Type destinationType)
        {
            return GetSupportedRegistrations(sourceType, destinationType).FirstOrDefault();
        }

        public bool TryGetSupported(Type sourceType, Type destinationType, out T value)
        {
            var r = GetSupportedRegistration(sourceType, destinationType);
            if (r == null)
            {
                value = default(T);
                return false;
            }
            value = r.Value;
            return true;
        }

        public void AddFirst(Type sourceType, Type destinationType, T value)
        {
            _registrations.AddFirst(new TypeCombinationEntry(sourceType, destinationType, value));
        }

        public void AddLast(Type sourceType, Type destinationType, T value)
        {
            _registrations.AddLast(new TypeCombinationEntry(sourceType, destinationType, value));
        }

        public class TypeCombinationEntry
        {
            private readonly Type _destinationType;
            private readonly Type _sourceType;
            private readonly T _value;

            public TypeCombinationEntry(Type sourceType, Type destinationType, T value)
            {
                _sourceType = sourceType;
                _destinationType = destinationType;
                _value = value;
            }

            public Type SourceType
            {
                get { return _sourceType; }
            }

            public Type DestinationType
            {
                get { return _destinationType; }
            }

            public T Value
            {
                get { return _value; }
            }
        }
    }
}