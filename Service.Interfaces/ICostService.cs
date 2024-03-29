﻿using Entities.LinkModels;
using Entities.Models;
using Shared.Dtos.Costs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICostService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetCostsAsync(Guid id,LinkParameters linkParameters, bool trackChanges);
        Task<CostDto> GetCostAsync(Guid movieId, Guid id, bool trachChanges);
        Task<CostDto> CreateCostAsync(Guid movieId, CreateCostDto createCostDto, bool trackChanges);
        Task DeleteCostAsync(Guid movieId, Guid id, bool trackChanges);
        Task UpdateCostAsync(Guid movieId, Guid id, UpdateCostDto updateCostDto, bool movieTrackChanges, bool costTrackChanges);

    }
}
