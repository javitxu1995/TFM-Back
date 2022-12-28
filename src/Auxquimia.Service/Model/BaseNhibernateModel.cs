namespace Auxquimia.Model
{
    using SolrNet.Attributes;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base NHibernate Model: Specifies the identifier of the object and current class for indexation
    /// </summary>
    public class BaseNhibernateModel
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets the ClassName
        /// </summary>
        public virtual IEnumerable<string> ClassName
        {
            get
            {
                Type currentType = GetType();
                bool ended = false;

                while (!ended)
                {
                    yield return currentType.FullName;
                    currentType = currentType.BaseType;
                    ended = currentType == typeof(BaseNhibernateModel);
                }

            }
        }
        public virtual DateTime LastIndexed => DateTime.Now;
    }
}
