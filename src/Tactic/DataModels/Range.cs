using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.DataModels
{
    [DataContract(Namespace = Namespaces.DEFAULT)]
    public class Range
    {
        [DataMember]
        public int Max
        { get; set; }
        [DataMember]
        public int Min
        { get; set; }

        public Range()
        { }

        public Range(int max)
        {
            Contract.Requires(max >= 0);

            this.Max = max;
        }

        public Range(int min, int max)
        {
            Contract.Requires(max >= min);

            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Min, Max);
        }

        /// <summary>
        /// determines whether the specified number is in the range
        /// </summary>
        public bool IsInRange(double number)
        {
            return number >= Min && number <= Max;
        }
    }
}
