using System;
using VsBuddy.Infrastructure.Types.Maybes.Implementation;

namespace VsBuddy.Infrastructure.Types.Maybes
{
    public abstract class Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        public static bool operator ==(Maybe<T> a, Maybe<T> b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a != null && b != null && a.Equals(b))
            {
                return true;
            }

            return false;
        }

        public static bool operator ==(Maybe<T> a, T b)
        {
            return a != null && a.Equals(b);
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Some<T>(value);
        }

        // ReSharper disable once UnusedParameter.Global
        public static implicit operator Maybe<T>(None none)
        {
            return new None<T>();
        }

        public static bool operator !=(Maybe<T> a, Maybe<T> b)
        {
            return !(a == b);
        }

        public static bool operator !=(Maybe<T> a, T b)
        {
            return !(a == b);
        }

        public abstract bool Equals(Maybe<T> other);

        public abstract bool Equals(T other);

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Maybe<T>)obj);
        }

        public abstract override int GetHashCode();

        // ReSharper disable once UnusedMember.Global
        public Maybe<T> ToMaybe()
        {
            return new None<T>();
        }
    }
}