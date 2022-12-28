namespace Auxquimia.Modules
{
    using Autofac;
    using Auxquimia.Module;

    /// <summary>
    /// Defines the <see cref="ApiModule" />
    /// </summary>
    public class ApiModule : Autofac.Module
    {
        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="builder">The builder<see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<KafkaModule>();
        }
    }
}
