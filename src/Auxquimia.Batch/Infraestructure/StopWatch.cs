using System;
using System.Collections.Generic;
using System.Text;

namespace Auxquimia.Batch.Infraestructure
{
    /// <summary>
    ///
    /// </summary>
    internal class Stopwatch
    {
        /** The Constant REPORT_HEADER_FOOTER. */

        /// <summary>
        /// The report header footer
        /// </summary>
        private static readonly String REPORT_HEADER_FOOTER = "==================================================";

        /** The Constant REPORT_FORMAT. */

        /// <summary>
        /// The report format
        /// </summary>
        private static readonly String REPORT_FORMAT = "{0,-70}{1,10}";

        /** The first start. */

        /// <summary>
        /// The first start
        /// </summary>
        private readonly long firstStart;

        /** The events. */

        /// <summary>
        /// The events
        /// </summary>
        private IList<Event> events;

        /** The instance. */

        /// <summary>
        /// The instance
        /// </summary>
        private static Stopwatch instance;

        /**
         * Instantiates a new stopwatch.
         */

        /// <summary>
        /// Prevents a default instance of the <see cref="Stopwatch"/> class from being created.
        /// </summary>
        private Stopwatch()
        {
            this.firstStart = Environment.TickCount;
            this.events = new List<Event>();
        }

        /**
         * Gets the single instance of Stopwatch.
         *
         * @return single instance of Stopwatch
         */

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static Stopwatch GetInstance()
        {
            if (instance == null)
            {
                instance = new Stopwatch();
            }
            return instance;
        }

        /**
         * Register event.
         *
         * @param description
         *            the description
         */

        /// <summary>
        /// Registers the event.
        /// </summary>
        /// <param name="description">The description.</param>
        public void RegisterEvent(string description)
        {
            this.RegisterEvent(description, new object[] { });
        }

        /**
         * Register event.
         *
         * @param description
         *            the description
         * @param parameters
         *            the parameters
         */

        /// <summary>
        /// Registers the event.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="parameters">The parameters.</param>
        public void RegisterEvent(string description, params object[] parameters)
        {
            events.Add(new Event(string.Format(description, parameters)));
        }

        /**
         * Generate report.
         *
         * @return the string
         */

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <returns></returns>
        public string GenerateReport()
        {
            long currentTime = this.firstStart;
            StringBuilder builder = new StringBuilder();

            builder.Append(Environment.NewLine);
            builder.Append(REPORT_HEADER_FOOTER);
            builder.Append(Environment.NewLine);
            builder.Append(string.Format(REPORT_FORMAT, "Evento", "Tiempo (ms)"));
            builder.Append(Environment.NewLine);

            foreach (Event evt in this.events)
            {
                builder.Append(string.Format(REPORT_FORMAT, evt.Description, evt.Time - currentTime));
                builder.Append(Environment.NewLine);
                currentTime = evt.Time;
            }

            builder.Append(REPORT_HEADER_FOOTER);
            builder.Append(Environment.NewLine);

            return builder.ToString();
        }
    }

    /**
     * The Class Event.
     */

    /// <summary>
    ///
    /// </summary>
    public class Event
    {
        /** The description. */

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; private set; }

        /** The time. */

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public long Time { get; private set; }

        /**
         * Instantiates a new event.
         *
         * @param description
         *            the description
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public Event(string description)
        {
            Description = description;
            Time = Environment.TickCount;
        }
    }
}
