using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain.Utilities
{
    public class IntToRangeAsStringHelper
    {
        public static string GetRangePlusMinus3(int? input)
        {
            return input == null ? null : string.Join(' ', new int[]
                {
                    input.Value -3,
                    input.Value -2,
                    input.Value -1,
                    input.Value,
                    input.Value +1,
                    input.Value +2,
                    input.Value +3
                });
        }

        public static string GetRangePlusMinus3(string input)
        {
            var inputInt = Int32.TryParse(input, out var tempInt) ? tempInt : (int?)null;
            return (input == null || input == "") ? null : string.Join(' ', new int[]
                {
                    inputInt.Value -3,
                    inputInt.Value -2,
                    inputInt.Value -1,
                    inputInt.Value,
                    inputInt.Value +1,
                    inputInt.Value +2,
                    inputInt.Value +3
                });
        }
    }
}
