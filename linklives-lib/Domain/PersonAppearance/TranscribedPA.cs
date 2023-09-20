using Nest;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Linklives.Serialization;

namespace Linklives.Domain
{
    enum TranscriptionType { DICTIONARY, EXPANDO, DYNAMIC }

    [ElasticsearchType(IdProperty = nameof(Key))]
    public class TranscribedPA : KeyedItem
    {
        private TranscriptionType transcriptionType;
        public int Pa_id { get; set; }
        public int Source_id { get; set; }

        [NestedExportable(forcedStrategy: NestingStrategy.Inline)]
        public dynamic Transcription { get; set; }

        public TranscribedPA()
        {

        }
        //TODO: Simplify TranscribedPA support of dynamic
        //TranscribedPa has to suport 3 scenarios:
        // * dynamic object from CSVHelper when reading transcriptions for indexation (and in tests)
        // * dictionary when reading single PAs from elastic search in the API
        public TranscribedPA(dynamic transcription, int sourceId)
        {
            Transcription = transcription;
            transcriptionType = GetTranscriptionType();
            Pa_id = Convert.ToInt32(GetTranscriptionPropertyValue("pa_id"));
            Source_id = sourceId;
        }

        private TranscriptionType GetTranscriptionType()
        {
            var type = Transcription.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                return TranscriptionType.DICTIONARY;
            }
            else if (type == typeof(ExpandoObject))
            {
                return TranscriptionType.EXPANDO;
            }
            return TranscriptionType.DYNAMIC;
        }

        /// <summary>
        /// Returns a value for the given property of the Transcription property,
        /// or null if the property does not exists
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>A string containing the value of the property or null if it is not set</returns>
        public string GetTranscriptionPropertyValue(string propertyName)
        {
            switch (transcriptionType)
            {
                case TranscriptionType.DICTIONARY:
                    return (string)Transcription?[propertyName];

                case TranscriptionType.EXPANDO:
                    var val = (IDictionary<string, object>)Transcription;
                    return (string)val[propertyName];

                case TranscriptionType.DYNAMIC:
                    return (string)Transcription.GetType()?.GetProperty(propertyName)?.GetValue(Transcription) ?? null;

                default:
                    throw new Exception("Unknown TranscriptionType");
            }
        }
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }
}
