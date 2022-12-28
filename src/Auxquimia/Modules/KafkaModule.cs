namespace Auxquimia.Module
{
    using Autofac;
    using Auxquimia.Config;

    /// <summary>
    /// Defines the <see cref="KafkaModule" />.
    /// </summary>
    public class KafkaModule : Module
    {
        /// <summary>
        /// The Load.
        /// </summary>
        /// <param name="builder">The builder<see cref="ContainerBuilder"/>.</param>
        protected override void Load(ContainerBuilder builder)
        {
            //Kafka
            builder.RegisterType<AuxquimiaKafkaLauncher>()
                .AsImplementedInterfaces().AsSelf().SingleInstance().AutoActivate().PropertiesAutowired();
        }
    }
}
