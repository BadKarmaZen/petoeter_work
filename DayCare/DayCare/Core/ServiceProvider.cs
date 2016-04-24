using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
	public class ServiceProvider
	{
		#region Instance
		private static ServiceProvider _instance;
		private static readonly object _lock = new object();

		/// <summary>
		/// The instance for the service provider
		/// </summary>
		public static ServiceProvider Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new ServiceProvider();
					}

					return _instance;
				}
			}
		}
		#endregion

		#region Members
		private readonly Dictionary<string, object> _services = new Dictionary<string, object>();
		#endregion

		#region Methods

		public void RegisterService<T>(T service)
		{
			string id = service.GetType().ToString();
			_services.Add(id, service);
		}

		public T GetService<T>()
		{
			string id = typeof(T).ToString();

			if (_services.ContainsKey(id))
			{
				return (T)_services[id];
			}

			throw new NotSupportedException(string.Format("Service '{0}' not found", id));
		}
		#endregion
	}
}
