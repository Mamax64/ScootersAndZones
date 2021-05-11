using ScootAPI.Configs;

namespace ScootAPI.Config
{
    public class Config : IConfig
    {
		public string AMQP_HOST { get; set; }
		public int AMQP_PORT { get; set; }
		public string AMQP_USERNAME { get; set; }
		public string AMQP_PASSWORD { get; set; }
		public int AMQP_PREFETCH { get; set; }
		public int AMQP_TIME_RETRY { get; set; }
		public string AMQP_NAME { get; set; }

		public string AMQP_QUEUE_WORKER { get; set; }

		public string AMQP_EXCHANGE_DISPATCH_INFOS { get; set; }
	}
}
