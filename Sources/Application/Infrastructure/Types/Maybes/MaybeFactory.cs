using VsBuddy.Infrastructure.Types.Maybes.Implementation;

namespace VsBuddy.Infrastructure.Types.Maybes
{
    public static class MaybeFactory
    {
        public static Maybe<T> CreateFromNullable<T>(T possiblyNull)
        {
            if (possiblyNull == null)
            {
                return None.Value;
            }

            return possiblyNull;
        }
    }
}