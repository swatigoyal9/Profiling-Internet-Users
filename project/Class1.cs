using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project

{
	class Class1
	{
		public double epoch1;
		public double dockets;
		public double duration;
		public double epoch2;
		
		public double internet;

		public Class1(double _epoch1, double _epoch2, double _dockets, double _duration)
		{
			//getting data
			this.epoch1 = _epoch1;
			this.epoch2 = _epoch2;
			this.dockets = _dockets;
			this.duration = _duration;
			if (duration != 0)
				this.internet = _dockets / _duration;
			else
				this.internet = 0;

		}

		public override String ToString()
		{
			return "RealFirstPacket: " + this.epoch1
				+ " RealEndPacket: " + this.epoch2
				+ " doctets: " + this.dockets
				+ " duration: " + this.duration
				+ "InternetUsage: " + this.internet;
		}


	}
}
