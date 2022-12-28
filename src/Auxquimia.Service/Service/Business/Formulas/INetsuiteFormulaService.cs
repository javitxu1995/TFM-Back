namespace Auxquimia.Service.Business.Formulas
{
    using Auxquimia.Dto.Business.Formulas;
    using Auxquimia.Filters;
    using Izertis.Interfaces.Abstractions;
    using System;

    /// <summary>
    /// Defines the <see cref="INetsuiteFormulaService" />.
    /// </summary>
    public interface INetsuiteFormulaService : IService<NetsuiteFormulaDto, Guid>, ISupportsSave<NetsuiteFormulaDto, Guid>, ISearchableService<NetsuiteFormulaDto, BaseSearchFilter>
    {
    }
}
