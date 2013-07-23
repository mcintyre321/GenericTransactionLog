using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericTransactionLog.Currying
{
    public static class Currier
    {
        public static Func<TA, TC> Then<TA, TB, TC>(this Func<TA, TB> left, Func<TB, TC> right)
        {
            return (a) => right(left(a));
        }

        public static Func<TA, IEnumerable<TC>> ThenForEach<TA, TB, TC>(this Func<TA, IEnumerable<TB>> left, Func<TB, TC> right)
        {
            return (a) => (left(a).Select(right));
        }

        public static Action<TA, TC> Then<TA, TB, TC>(this Func<TA, TB> left, Action<TB, TC> right)
        {
            return (a, c) => right(left(a), c);
        }
    }
}