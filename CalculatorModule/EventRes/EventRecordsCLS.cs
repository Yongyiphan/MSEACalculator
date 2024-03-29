﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.EventRes
{
    public class EventRecordsCLS
    {
        public string eventTitle { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public int eventDuration { get; set; }
        public Dictionary<string, bool> availableDays { get; set; }
        public List<Dictionary<string, int>> chartrackCoins { get; set; }

        public EventRecordsCLS() { }
    }
}
