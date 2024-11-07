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
        static int totalStar = 0;
        public static void AddStar()
        {
            stars++;
        }

        public static void PushStars()
        {
            totalStar = stars;
        }

        public static void PopStars()
        {
            stars = totalStar;
        }
    }
}
