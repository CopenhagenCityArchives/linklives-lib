using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    public class TranscribedPA : KeyedItem
    {
        public int Pa_id { get; set; }
        public int Source_id { get; set; }
        public dynamic Transcription { get; set; }
        public TranscribedPA()
        {

        }
        public TranscribedPA(dynamic transcription, int sourceId)
        {
            Pa_id = Convert.ToInt32(transcription.pa_id);
            Source_id = sourceId;
            Transcription = transcription;
        }
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }
}
