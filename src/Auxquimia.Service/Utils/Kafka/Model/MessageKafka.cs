namespace Auxquimia.Utils.Kafka.Model
{
    using Auxquimia.Utils.Kafka.Enum;
    using System;

    /// <summary>
    /// Defines the <see cref="MessageKafka" />.
    /// </summary>
    [Serializable]
    public class MessageKafka
    {
        public MessageKafka(MessageType type, string content, string source)
        {
            Type = type;
            Content = content;
            Source = source;
        }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the Source.
        /// </summary>
        public string Source { get; set; }
    }
}
