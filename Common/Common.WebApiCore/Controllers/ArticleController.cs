﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;
using Common.Services.Infrastructure.Services;
using Common.WebApiCore.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.WebApiCore.Controllers
{
    /// <summary>
    /// Articles management
    /// </summary>
    [Route("Article")]
    public class ArticleController : BaseApiController
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// This endpoint is using for create or update a article.
        /// To create an article you must provide an empty Id.
        /// To update existing, you must provide the Id of the article.
        /// NOTE: You must always provide categoryId, the article cannot be created without category.
        /// </summary>
        /// <param name="dto">DTO that should pass for create or update article</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Article
        ///     {
        ///         "id": "",
        ///         "title": "The Battle of C# to JSON Serializers in .NET Core 3",
        ///         "description": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
        ///         "categoryId": "cba4a6f7-200d-49dc-a093-67869f3283a6"
        ///     }
        /// </remarks>
        /// <returns>Article DTO</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(ArticleAddDTO dto)
        {
            var result = await this._articleService.Edit(dto);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is using for getting all user's categories
        /// </summary>
        /// <returns>Collection of Article DTO</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var result = await this._articleService.Get();
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is using for getting specific article by Id
        /// </summary>
        /// <param name="id">Article id</param>
        /// <returns>Article DTO</returns>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(Guid id)
        {
            var isExists = await this._articleService.Exists(id);
            if (!isExists) throw new BadRequestException("Article does not exist");
            var result = await this._articleService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is using for delete specific article by Id
        /// </summary>
        /// <param name="id">Article id</param>
        /// <returns>Success: 200</returns>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isExists = await this._articleService.Exists(id);
            if (!isExists) throw new BadRequestException("Article does not exist");
            await this._articleService.Delete(id);
            return Ok();
        }
    }
}