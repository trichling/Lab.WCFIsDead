﻿using System;

namespace Lab.WCFIsDead.AzureServiceBus.Shared
{
    public static class Constants
    {
        public const string ServiceBusConnectionString = "Endpoint=sb://tobidevtest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jiHSeEVW/ka/OEfiE3A1S4pcnR/9zKrmDHrrEl36qA8=";
        public const string RequestQueueName = "calculations";
        public const string ResponseQueueName = "calculationresponses";
    }

    public class Calculation
    {
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Operator { get; set; }
    }

    public class CalculationResult
    {
        public Calculation Calculation { get; set; }
        public decimal Result { get; set; }

    }
}
