﻿using Linklives.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance fromed from parish register records
    /// </summary>
    public class ParishPA : BasePA
    {
        public override int? Deathyear_searchable
        {
            get
            {
                int deathyearSearchable;
                if (Standard.Event_type.Equals("burial") && Int32.TryParse(Standard.Event_year, out deathyearSearchable))
                {
                    return deathyearSearchable;
                }

                return null;
            }
        }
        public override string Deathyear_searchable_fz
        {
            get
            {
                if (!Standard.Event_type.Equals("burial")) { return null; }
                return IntToRangeAsStringHelper.GetRangePlusMinus3(Standard.Event_year);
            }
        }
        public override string Deathplace_searchable
        {
            get
            {
                return Standard.Event_type.Equals("burial") ? Sourceplace_searchable : null;
            }
        }
        public override string Source_type_wp4
        {
            get
            {
                return "parish_register";
            }
        }
        public override string Sourceplace_display
        {
            get
            {
                var strList = new List<string>() { Transcribed.Transcription.browselevel, Transcribed.Transcription.browselevel1 };

                return string.Join(", ", strList.Where(sl => !string.IsNullOrEmpty(sl)));
            }
        }
        public override string Pa_grouping_id_wp4
        {
            get
            {
                return Transcribed.Transcription.event_id;
            }
        }
        public override int? Deathyear_display
        {
            get
            {
                return Deathyear_searchable;
            }
        }
        public override string Source_type_display
        {
            get
            {
                return "Kirkebog";
            }
        }
        public override string Source_archive_display
        {
            get
            {
                return "Rigsarkivet";
            }
        }
        public override string Sourceyear_display
        {
            get
            {
                return Transcribed.Transcription.browselevel2;
            }
        } 
        public ParishPA()
        {

        }
        public ParishPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.parish_register;
        }
    }
}
