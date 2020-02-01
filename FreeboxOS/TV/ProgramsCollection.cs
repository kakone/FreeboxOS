using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeboxOS.TV
{
    /// <summary>
    /// Programs collection
    /// </summary>
    public class ProgramsCollection : Dictionary<string, Program>
    {
        /// <summary>
        /// Gets the program corresponding to a date/time
        /// </summary>
        /// <param name="dateTimeOffset">a date</param>
        /// <returns>the program corresponding to the date/time</returns>
        public Program this[DateTimeOffset dateTimeOffset] =>
            Values.FirstOrDefault(p => dateTimeOffset >= p.StartDate && dateTimeOffset <= p.EndDate);
    }
}
