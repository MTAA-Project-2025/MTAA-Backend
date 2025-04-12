using MTAA_Backend.Domain.Resources.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.Locations
{
    public interface INormalizeLocationService
    {
        public void NormalizeLocation(ref double latitude, ref double longitude);
        public void NormalizeLocation(ref double latitude, ref double longitude, ref double radius, int zoomLevel);
    }
}
