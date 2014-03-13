using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
  public  enum BookingInfoStatusEnums
    {

      PhongTrong=0,
        /// <summary>
        /// dat phong dam bao
        /// </summary>
        DamBao=1,
        /// <summary>
        /// Dat phong khong dam bao
        /// </summary>
        KoDamBao=2,
        /// <summary>
        /// phong dang o
        /// </summary>
        PhongDangO=3,
        /// <summary>
        /// phong da checkout
        /// </summary>
        PhongDaCheckOut=4

    }
}
