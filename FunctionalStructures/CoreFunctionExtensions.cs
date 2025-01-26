#region Header
// // © 2025 Koninklijke Philips N.V.  All rights reserved.
// // Reproduction or transmission in whole or in part, in any form or by any means,
// // electronic, mechanical or otherwise, is prohibited without the prior  written consent of
// // the owner.
#endregion

using LaYumba.Functional;

namespace FunctionalStructures;

public static class CoreFunctionExtensions
{
    public static Option<Unit> Map<T>(this Option<T> @this, Action<T> action)
    {
        return @this.Map(action.ToFunc());
    }
}