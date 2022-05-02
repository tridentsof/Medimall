using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medimall.Helper
{
    public class Constants
    {
        public struct PaymentMethod
        {
            public const int PayLater = 1;

            public const int Online = 2;
        }

        public struct IsUsePoint
        {
            public const int NotUse = 0;

            public const int Use = 1;
        }
    }
}