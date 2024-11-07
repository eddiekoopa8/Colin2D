using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class GAMEManager
    {
        static int stars;
        public static int StarsCollected { get { return stars; } }
        public static void AddStar()
        {
            stars++;
        }
    }
}
