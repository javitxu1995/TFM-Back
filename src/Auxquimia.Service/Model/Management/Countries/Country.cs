namespace Auxquimia.Model.Management.Countries
{
    using Capgemini.CommonObjectUtils;
    using FluentNHibernate.Mapping;
    using SolrNet.Attributes;
    /// <summary>
    /// Country entity for the application
    /// </summary>
    public class Country : BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the iso name of the country
        /// </summary>
        public virtual string IsoName { get; set; }

        /// <summary>
        /// Gets or sets the name of the country
        /// </summary>
        public virtual string Name { get; set; }

        public override bool Equals(object obj)
        {
            Country other = obj as Country;
            bool result = false;
            if (other != null)
            {
                result = new EqualsBuilder().Append(Id, other.Id).IsEquals;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(Id).GetHashCode();
        }
    }

    internal class CountryClassMap : ClassMap<Country>
    {
        public CountryClassMap()
        {
            Table("M_COUNTRY");

            Id(x => x.Id).Column("ID").GeneratedBy.GuidComb();
            Map(x => x.IsoName).Column("ISO_NAME");
            Map(x => x.Name).Column("NAME");
        }
    }
}
