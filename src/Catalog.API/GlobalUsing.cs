global using System.Net;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Ardalis.Specification;
global using Ardalis.Specification.EntityFrameworkCore;
global using AutoMapper;
global using MassTransit;
global using StackExchange.Redis;

global using Catalog.API.Application.Decorators;
global using Catalog.API.Application.DTOs.Brands;
global using Catalog.API.Application.DTOs.Categories;
global using Catalog.API.Application.DTOs.Products;
global using Catalog.API.Application.Interfaces;
global using Catalog.API.Application.Mappings;
global using Catalog.API.Application.Services;
global using Catalog.API.Domain.Entities;
global using Catalog.API.Domain.Filters;
global using Catalog.API.Domain.Interfaces;
global using Catalog.API.Domain.Specifications.BrandSpecification;
global using Catalog.API.Domain.Specifications.CategorySpecification;
global using Catalog.API.Domain.Specifications.ProductSpecification;
global using Catalog.API.Infrastructure.Data;
global using Catalog.API.Infrastructure.Repositories;
global using Catalog.API.Infrastructure.Services;
global using Catalog.API.Presentation.Middlewares;

global using FileStorage.Protos;
global using Shared.DTOs;