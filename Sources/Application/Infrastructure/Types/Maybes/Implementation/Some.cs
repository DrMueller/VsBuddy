namespace VsBuddy.Infrastructure.Types.Maybes.Implementation
{
    public class Some<T> : Maybe<T>
    {
        private readonly T _content;

        public Some(T content)
        {
            _content = content;
        }

        public static implicit operator T(Some<T> value)
        {
            return value._content;
        }

        public override bool Equals(Maybe<T> other)
        {
            return Equals(other as Some<T>);
        }

        public override bool Equals(T other)
        {
            return ContentEquals(other);
        }

        public override int GetHashCode()
        {
            return _content.GetHashCode();
        }

        // ReSharper disable once UnusedMember.Global
        public T ToT(Some<T> value)
        {
            return value._content;
        }

        private bool ContentEquals(T other)
        {
            if (_content == null && other == null)
            {
                return true;
            }

            if (_content != null && _content.Equals(other))
            {
                return true;
            }

            return false;
        }

        private bool Equals(Some<T> other)
        {
            return other != null &&
                   ContentEquals(other._content);
        }
    }
}