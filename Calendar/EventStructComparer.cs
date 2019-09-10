using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calendar
{
    class EventStructComparer: IComparer<EventStruct>
    {
        public int Compare(EventStruct x, EventStruct y)
        {
            // даты событий
            if (x.Date > y.Date)
                return (1);
            else if (x.Date < y.Date)
                return (-1);
            else        // равны
            {
                // надо бы посравнивать другие члены структуры
                // но пока и так сойдет
                return (0);
            }
        }
    }
}
