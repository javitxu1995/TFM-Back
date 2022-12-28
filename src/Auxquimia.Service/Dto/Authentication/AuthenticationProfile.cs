namespace Auxquimia.Dto.Authentication
{
    using AutoMapper;
    using Auxquimia.Dto.Management.Countries;
    using Auxquimia.Dto.Management.Factories;
    using Auxquimia.Model.Authentication;
    using Auxquimia.Repository.Authentication;
    using Auxquimia.Service.Filters.Authentication;
    using Auxquimia.Utils;
    using Izertis.Misc.Utils;
    using Izertis.Paging.Abstractions;
    using System;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="AuthenticationProfile" />.
    /// </summary>
    internal class AuthenticationProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProfile"/> class.
        /// </summary>
        public AuthenticationProfile()
        {
            CreateMap<FindRequestDto<UserSearchFilter>, FindRequestImpl<UserSearchFilter>>();
            CreateMap<User, UserDto>()
                .ForMember(x => x.Password, opt => opt.MapFrom<PasswordObfuscatorResolver>())
                .ForMember(x => x.Roles, opt => opt.MapFrom(y => y.Roles.Select(x => x.Role)));
            CreateMap<UserDto, User>()
                .ForMember(x => x.Password, opt => opt.MapFrom<PasswordResolver>())
                //.ForMember(x => x.Country, opt => opt.MapFrom<CountryResolver, string>(y => y.Country != null ? y.Country.Id : null))
                .ForMember(x => x.Factory, opt => opt.MapFrom<FactoryResolver, string>(y => y.Factory != null ? y.Factory.Id : null))
                .ForMember(x => x.Roles, opt => opt.Ignore());
            CreateMap<Page<User>, Page<UserDto>>();
        }
    }

    /// <summary>
    /// Defines the <see cref="PasswordResolver" />.
    /// </summary>
    internal class PasswordResolver : IValueResolver<UserDto, User, string>
    {
        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="UserDto"/>.</param>
        /// <param name="destination">The destination<see cref="User"/>.</param>
        /// <param name="destMember">The destMember<see cref="string"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string Resolve(UserDto source, User destination, string destMember, ResolutionContext context)
        {
            string password = destination.Password;
            if (string.IsNullOrEmpty(password) || source.PasswordChanged)
            {
                password = HelperMethods.GetHash(source.Password);
            }
            return password;
        }
    }

    /// <summary>
    /// Defines the <see cref="PasswordObfuscatorResolver" />.
    /// </summary>
    internal class PasswordObfuscatorResolver : IValueResolver<User, UserDto, string>
    {
        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="User"/>.</param>
        /// <param name="destination">The destination<see cref="UserDto"/>.</param>
        /// <param name="destMember">The destMember<see cref="string"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            string password = source.Password;
            if (!string.IsNullOrEmpty(password))
            {
                password = "**********";
            }
            return password;
        }
    }

    /// <summary>
    /// Defines the <see cref="UserResolver" />.
    /// </summary>
    internal class UserResolver : IMemberValueResolver<object, object, string, User>
    {
        /// <summary>
        /// Defines the userRepository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserResolver"/> class.
        /// </summary>
        /// <param name="userRepository">The userRepository<see cref="IUserRepository"/>.</param>
        public UserResolver(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="source">The source<see cref="object"/>.</param>
        /// <param name="destination">The destination<see cref="object"/>.</param>
        /// <param name="sourceMember">The sourceMember<see cref="string"/>.</param>
        /// <param name="destMember">The destMember<see cref="User"/>.</param>
        /// <param name="context">The context<see cref="ResolutionContext"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        public User Resolve(object source, object destination, string sourceMember, User destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.userRepository.GetAsync(sourceMember.PerformMapping<string, Guid>()));
        }
    }

    internal class UserNameResolver : IMemberValueResolver<object, object, string, User>
    {
        /// <summary>
        /// Defines the userRepository.
        /// </summary>
        private readonly IUserRepository userRepository;

        public UserNameResolver(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Resolve(object source, object destination, string sourceMember, User destMember, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(sourceMember) ? null : TaskUtils.NonBlockingAwaiter(() => this.userRepository.FindByUsernameAsync(sourceMember));
        }
    }


}
