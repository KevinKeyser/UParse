using System;
using System.Collections.Generic;
using System.Text;

namespace UParse
{
    public class ConversionObjectNode
    {
        public ConversionObjectNode OwnerNode { get; }

        public ConversionObjectInfo ConversionInfo { get; }

        public ConversionObjectNode(ConversionObjectInfo conversionInfo, ConversionObjectNode ownerNode = null)
        {
            ConversionInfo = conversionInfo;
            OwnerNode = ownerNode;
        }
    }
}
