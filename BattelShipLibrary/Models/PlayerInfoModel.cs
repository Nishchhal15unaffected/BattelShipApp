using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattelShipLibrary.Models
{
    public class PlayerInfoModel
    {
        public string UserName { get; set; }
        public List<GridSpotModel> ShipLocation { get; set; }
        public List<GridSpotModel> ShotGrid { get; set; }
    }
}
