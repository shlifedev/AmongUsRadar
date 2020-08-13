using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGsHarp
{
    /// <summary>
    /// Team enum
    /// </summary>
    public enum Team : int
    {
        NOD = 0,
        GDI = 1
    }

    /// <summary>
    /// RadarMode enum
    /// </summary>
    public enum RadarMode : int
    {
        /// <summary>
        /// Don't show anything on the radar
        /// </summary>
        None = 0,
        /// <summary>
        /// Only show players of the same team
        /// </summary>
        Own = 1,
        /// <summary>
        /// Show all players (Radarhack)
        /// </summary>
        All = 2,

        Default = Own
    }

}
