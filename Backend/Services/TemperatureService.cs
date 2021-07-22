using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using IOT_Temperature.Interface;
using Temperatures;

namespace IOT_Temperature.Services
{
    public class TemperatureService:TemperatureSensing.TemperatureSensingBase
    {
        private ITemperatureRepository _iTemp;
        public TemperatureService(ITemperatureRepository iTemp)
        {
            _iTemp = iTemp;
        }

        public override Task<Empty> AddTemperatureData(TemperatureData request, ServerCallContext context)
        {
            _iTemp.AddData(request);
            return Task.FromResult(new Empty());
        }

    }
}
