﻿using System;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from census records
    /// </summary>
    public class CensusPA : BasePA
    {
        public override string Source_type_wp4
        {
            get
            {
                return "census";
            }
        }
        public override string Role_display
        {
            get
            {
                return Standard.Household_position;
            }
        }
        public override string Source_type_display
        {
            get
            {
                return "Folketælling";
            }
        }
        public override string Source_archive_display
        {
            get
            {
                return "Rigsarkivet";
            }
        }
        public override string Role_searchable
        {
            get
            {
                return Standard.Household_position;
            }
        }
        public CensusPA()
        {

        }
        public CensusPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.census;
        }
    }
}
