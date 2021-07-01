using System;

namespace IOExtensions
{
    public class TransferProgress
    {
        public TransferProgress(DateTime startedTimestamp, long bytesTransfered)
        {
            BytesTransferred = bytesTransfered;
            BytesPerSecond = BytesTransferred / DateTime.Now.Subtract(startedTimestamp).TotalSeconds;
        }
        public string ID { get; set; }
        public long Total { get; set; }

        public long Transferred { get; set; }

        public long BytesTransferred { get; set; }

        public long StreamSize { get; set; }

        public string ProcessedFile { get; set; }

        public double BytesPerSecond { get; }

        public double Fraction
        {
            get
            {
                return BytesTransferred / (double)Total;
            }
        }

        public double Percentage
        {
            get
            {
                return 100.0 * Fraction;
            }
        }

        public string GetBytesTransferedFormatted(SuffixStyle suffixStyle, int decimalPlaces)
        {
            return Helpers.ToSizeWithSuffix(BytesTransferred, suffixStyle, decimalPlaces);
        }

        public string GetDataPerSecondFormatted(SuffixStyle suffixStyle, int decimalPlaces)
        {
            return string.Format("{0}/sec", Helpers.ToSizeWithSuffix((long)BytesPerSecond, suffixStyle, decimalPlaces));
        }

        public override string ToString()
        {
            return $"ID={ID}, Total={Total.ToString()}, Transferred={Transferred.ToString()}, BytesTransferred={BytesTransferred.ToString()}, StreamSize={StreamSize.ToString()}, ProcessedFile={ProcessedFile}, BytesPerSecond={BytesPerSecond.ToString()}, Fraction={Fraction.ToString()}, Percentage={Percentage.ToString()}";
        }
    }
}
