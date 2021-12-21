using Nest;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Linklives.Domain
{
    [ElasticsearchType(IdProperty = nameof(Key))]
    public class TranscribedPA : KeyedItem
    {
        public int Pa_id { get; set; }
        public int Source_id { get; set; }
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
            var type = Transcription.GetType();
            Pa_id = Convert.ToInt32(GetTranscriptionPropertyValue("pa_id"));
            Source_id = sourceId;
        }
        /// <summary>
        /// Returns a value for the given property of the Transcription property,
        /// or null if the property does not exists
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>A string containing the value of the property or null if it is not set</returns>
        public string GetTranscriptionPropertyValue(string propertyName)
        {
            try
            {
                if (Transcription.GetType() == typeof(Dictionary<string, object>))
                {
                    return (string)Transcription?[propertyName];
                }
                else
                {
                    return (string)Transcription.GetType().GetProperty(propertyName).GetValue(Transcription) ?? null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }
}
