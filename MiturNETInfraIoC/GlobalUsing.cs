global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using MiturNetInfrastructure;
global using MiturNetApplication.Interfaces;
global using MiturNetApplication.Mappings;
global using MiturNetApplication.Services;
global using System.Reflection;
global using System;
global using System.Text;

global using SegasaMRP.Infrastructure.DBContext;
global using MiturNetInfrastructure.DBContext;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.IdentityModel.Tokens;
global using MiturNetDomain.Entities.UserManagement;
global using Microsoft.AspNetCore.Authorization;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.ResponseCompression;
global using MiturNetApplication.SignalR;
global using Microsoft.OpenApi.Models;
global using MiturNetInfrastructure.DBContext;
global using System.Text.Json;
global using System.Text.Json.Serialization;