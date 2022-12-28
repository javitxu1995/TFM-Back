using System.Linq;
using System.Collections.Generic;
using Auxquimia.Batch.Infraestructure;
using Microsoft.Extensions.Logging;

namespace Auxquimia.Batch.TestMultistep
{
    public class MultistepReader : IReader<long>
    {
        public IEnumerable<long> Read(long page)
        {
            if (page < 3)
            {
                long cur = page * 4;
                return new[] { cur + 1, cur + 2, cur + 3, cur + 4 };
            }
            else
            {
                return new long[] { };
            }
        }
    }

    public class MultistepProcessor : IProcessor<long, string>
    {
        public IEnumerable<string> Process(IEnumerable<long> elements)
        {
            IList<string> result = new List<string>();

            foreach (int element in elements)
            {
                result.Add((element + 1).ToString());
            }

            return result;
        }
    }

    public class MultistepWriter : IWriter<string>
    {
        private readonly ILogger<MultistepWriter> logger;

        public MultistepWriter(ILogger<MultistepWriter> logger)
        {
            this.logger = logger;
        }

        public long Write(IEnumerable<string> elements)
        {
            foreach(string element in elements)
            {
                logger.LogInformation($"element: {element}");
            }

            return elements.Count();
        }
    }

    public class MultistepRunner : AbstractRunner<long, string>
    {
        private readonly MultistepReader reader;
        private readonly MultistepProcessor processor;
        private readonly MultistepWriter writer;
        private readonly ILogger<MultistepRunner> logger;

        public MultistepRunner(MultistepReader reader, MultistepProcessor processor, MultistepWriter writer, ILogger<MultistepRunner> logger)
        {
            this.reader = reader;
            this.processor = processor;
            this.writer = writer;
            this.logger = logger;
        }

        internal override JobResult GetJobResult(long read, long processed, long written, long errors) => new MultistepJobResult();

        internal override IProcessor<long, string> GetProcessor() => processor;

        internal override IReader<long> GetReader() => reader;

        internal override IWriter<string> GetWriter() => writer;

        internal override void PostProcess() => logger.LogInformation("Postprocessing job");

        internal override void PreProcess() => logger.LogInformation("Preprocessing job");
    }
}
