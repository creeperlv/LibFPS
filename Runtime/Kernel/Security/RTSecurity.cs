using System.Security.Cryptography;

namespace LibFPS.Kernel.Security{
	public class RTSecurity{
		public static RTSecurity Instance;
		public RSA SharedRSA;
	}
}
