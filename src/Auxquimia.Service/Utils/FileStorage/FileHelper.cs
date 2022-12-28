namespace Auxquimia.Utils
{
    using Auxquimia.Exceptions;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Izertis.Misc.Utils;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="FileHelper" />.
    /// </summary>
    public class FileHelper : IDisposable
    {
        /// <summary>
        /// Gets or sets the CsvReader.
        /// </summary>
        private CsvReader CsvReader { get; set; }

        /// <summary>
        /// Gets or sets the CsvWriter.
        /// </summary>
        private CsvWriter CsvWriter { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="FileHelper"/> class from being created.
        /// </summary>
        private FileHelper()
        {
            CsvReader = null;
            CsvWriter = null;
        }

        /// <summary>
        /// The ReadHeader.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool ReadHeader()
        {
            if (CsvReader == null)
            {
                throw new CustomException(Constants.Ftp.Errors.READER_INITIALIZE_ERROR);
            }
            return CsvReader.ReadHeader();
        }

        /// <summary>
        /// The SkipLine.
        /// </summary>
        public void SkipLine()
        {
            Read();
        }

        /// <summary>
        /// The Read.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Read()
        {
            if (CsvReader == null)
            {
                throw new CustomException(Constants.Ftp.Errors.READER_INITIALIZE_ERROR);
            }
            try
            {
                return CsvReader.Read();
            }catch(Exception e)
            {
                Console.WriteLine($"[Stream reading exception] Exception - {e.Message}");
                return false;
            }
            
        }

        /// <summary>
        /// The ReadStepFromCSV.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, object}"/>.</returns>
        public Dictionary<string, object> ReadStepFromCSV()
        {
            if (CsvReader == null)
            {
                throw new CustomException(Constants.Ftp.Errors.READER_INITIALIZE_ERROR);
            }
            Dictionary<string, object> entry = new Dictionary<string, object>();

            entry.Add(Constants.Ftp.ASSEMBLY_NUMBER, CsvReader.GetField(0));
            entry.Add(Constants.Ftp.ASSEMBLY_LOAD_DATE, getLongFromStringDate(CsvReader.GetField(1)));
            entry.Add(Constants.Ftp.STEP_ITEM_CODE, StringUtils.HasText(CsvReader.GetField(2)) ? CsvReader.GetField(2) : default(string));
            entry.Add(Constants.Ftp.STEP_ITEM_DISPLAY_NAME, StringUtils.HasText(CsvReader.GetField(3)) ? CsvReader.GetField(3) : default(string));
            entry.Add(Constants.Ftp.STEP_ITEM_QTY_REQUIRED, StringUtils.HasText(CsvReader.GetField(4)) ? Convert.ToDecimal(CsvReader.GetField(4)) : default(decimal));
            entry.Add(Constants.Ftp.STEP_ITEM_UNITS, StringUtils.HasText(CsvReader.GetField(5)) ? CsvReader.GetField(5) : default(string));
            entry.Add(Constants.Ftp.STEP_NUMBER, StringUtils.HasText(CsvReader.GetField(6)) ? Convert.ToInt16(CsvReader.GetField(6)) : default(int));
            entry.Add(Constants.Ftp.STEP_INVENTORY_DETAIL, StringUtils.HasText(CsvReader.GetField(7)) ? Convert.ToInt64(CsvReader.GetField(7)) : default(long));
            entry.Add(Constants.Ftp.STEP_INVENTORY_LOT, StringUtils.HasText(CsvReader.GetField(8)) ? CsvReader.GetField(8) : default(string));
            entry.Add(Constants.Ftp.ASSEMBLY_BLENDER, StringUtils.HasText(CsvReader.GetField(9)) ? CsvReader.GetField(9) : default(string));
            entry.Add(Constants.Ftp.STEP_OPERATOR, StringUtils.HasText(CsvReader.GetField(10)) ? Convert.ToInt16(CsvReader.GetField(10)) : default(int));
            entry.Add(Constants.Ftp.STEP_BLENDER_SPEED1, StringUtils.HasText(CsvReader.GetField(11)) ? CsvReader.GetField(11) : default(string));
            entry.Add(Constants.Ftp.STEP_BLENDER_SPEED2, StringUtils.HasText(CsvReader.GetField(12)) ? CsvReader.GetField(12) : default(string));
            entry.Add(Constants.Ftp.STEP_BLENDING_TIME, StringUtils.HasText(CsvReader.GetField(13)) ? Convert.ToInt16(CsvReader.GetField(13)) : default(Int16));
            entry.Add(Constants.Ftp.STEP_BATCH_NUMBER, StringUtils.HasText(CsvReader.GetField(14)) ? Convert.ToInt64(CsvReader.GetField(14)) : default(long));
            entry.Add(Constants.Ftp.STEP_TEMPERATURE, StringUtils.HasText(CsvReader.GetField(15)) ? CsvReader.GetField(15) : default(string));

            return entry;
        }

        /// <summary>
        /// The Reader.
        /// </summary>
        /// <param name="fileStream">The fileStream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="FileHelper"/>.</returns>
        public static FileHelper Reader(Stream fileStream)
        {
            FileHelper helper = new FileHelper();
            CsvConfiguration configuration = helper.CreateConfiguration();
            TextReader tr = new StreamReader(fileStream);
            helper.CsvReader = new CsvReader(tr, configuration);
            return helper;
        }

        /// <summary>
        /// The Writer.
        /// </summary>
        /// <param name="fileStream">The fileStream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="FileHelper"/>.</returns>
        public static FileHelper Writer(Stream fileStream)
        {
            FileHelper helper = new FileHelper();
            CsvConfiguration configuration = helper.CreateConfiguration();
            TextWriter tw = new StreamWriter(fileStream);
            helper.CsvWriter = new CsvWriter(tw, configuration);
            return helper;
        }

        /// <summary>
        /// The CreateConfiguration.
        /// </summary>
        /// <returns>The <see cref="CsvConfiguration"/>.</returns>
        private CsvConfiguration CreateConfiguration()
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture);
            config.Delimiter = ",";
            //config.SanitizeForInjection = false;
            return config;
        }

        /// <summary>
        /// The getLongFromStringDate.
        /// </summary>
        /// <param name="dateString">The dateString<see cref="string"/>.</param>
        /// <returns>The <see cref="long"/>.</returns>
        private static long getLongFromStringDate(string dateString)
        {
            if (StringUtils.HasText(dateString))
            {
                return DateHelper.ToUnixTimeMilliseconds(Convert.ToDateTime(dateString));
            }
            else
            {
                return default(long);
            }
        }

        public void Write<T>(IList<T> records) 
        {
            this.CsvWriter.WriteRecords<T>(records);
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            if (CsvReader != null)
            {
                CsvReader.Dispose();
                CsvReader = null;
            }

            if (CsvWriter != null)
            {
                CsvWriter.Dispose();
                CsvWriter = null;
            }
        }
    }
}
