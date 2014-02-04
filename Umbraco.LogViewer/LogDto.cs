using System;
using log4net.Core;
using Newtonsoft.Json;

namespace Umbraco.LogViewer
{
    public class LogDto
    {
        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }
        [JsonProperty("thread")]
        public string Thread { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("logger")]
        public string Logger { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4}", TimeStamp, Level, Thread, Logger, Message);
        }

        #region Equality

        protected bool Equals(LogDto other)
        {
            return
                string.Equals(TimeStamp, other.TimeStamp) && 
                string.Equals(Thread, other.Thread) && 
                Equals(Level, other.Level) && 
                string.Equals(Logger, other.Logger) && 
                string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LogDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TimeStamp != null ? TimeStamp.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Thread != null ? Thread.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Level != null ? Level.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Logger != null ? Logger.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Message != null ? Message.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(LogDto left, LogDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogDto left, LogDto right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
