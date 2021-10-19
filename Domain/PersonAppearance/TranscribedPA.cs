using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain.PersonAppearance
{
    public class TranscribedPA : KeyedItem
    {
        public int Pa_id { get; set; }
        public int Source_id { get; set; }
        public dynamic Transcription { get; set; }
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }
}
