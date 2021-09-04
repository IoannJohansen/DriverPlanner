using System;
using System.Security.Cryptography;
using System.Text;

namespace DriverPlanner.Infrastructure.Hash
{
	public static class HashPassword
	{
		public static string GetHash(string pass)
		{
			if (pass == null) return null;
			var passHasher = MD5.Create();
			return Convert.ToBase64String(passHasher.ComputeHash(Encoding.UTF8.GetBytes(pass)));
		}

	}
}
