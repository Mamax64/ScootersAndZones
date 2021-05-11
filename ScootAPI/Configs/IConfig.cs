namespace ScootAPI.Configs
{
    public interface IConfig
    {
		string AMQP_HOST { get; set; }
		int AMQP_PORT { get; set; }
		string AMQP_USERNAME { get; set; }
		string AMQP_PASSWORD { get; set; }
		int AMQP_PREFETCH { get; set; }
		int AMQP_TIME_RETRY { get; set; }
		string AMQP_NAME { get; set; }
		string AMQP_QUEUE_WORKER { get; set; }

		string AMQP_EXCHANGE_DISPATCH_INFOS { get; set; }
	}
}
