using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using CourseStoreMinimalAPI.AplicationService;
using CourseStoreMinimalAPI.Endpoint.InfraStructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.CategoryRAR;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses.UserRequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class UserEndpoints
{
    static string Cachekey = "user";
    private static string _prefix;
    public static WebApplication MapUsers(this WebApplication app, string prefix)
    {
        _prefix = prefix;
        var MGCategories = app.MapGroup(prefix);
        MGCategories.MapPost("/", Insert);
        MGCategories.MapPost("/login", Login);
        return app;
    }

    static async Task<Results<Created<UserRegistrationResponse>, BadRequest<IEnumerable<IdentityError>>>> Insert(
        [FromServices] UserManager<IdentityUser> userManager,
        UserRegistrationRequest userRegistrationRequest)
    {
        var identityUser = new IdentityUser(userRegistrationRequest.Email);
        var result = await userManager.CreateAsync(identityUser, userRegistrationRequest.Password);
        if (result.Succeeded)
        {
            var response = new UserRegistrationResponse { IsOk = true };
            return TypedResults.Created($"/{_prefix}/{identityUser.Id}", response);
        }
        return TypedResults.BadRequest<IEnumerable<IdentityError>>(result.Errors);
    }

    static async Task<Results<Ok<UserLoginResponse>, BadRequest>> Login(
        [FromServices] UserManager<IdentityUser> userManager,
        UserLoginRequest userLogingRequest, IConfiguration configuration)
    {
        var user = await userManager.FindByNameAsync(userLogingRequest.Email);
        if (user is null)
        {
            return TypedResults.BadRequest();
        }
        bool isValidPassword = await userManager.CheckPasswordAsync(user, userLogingRequest.Password);
        if (!isValidPassword)
        {
            return TypedResults.BadRequest();
        }
        //u can get this claim from anywhere like database or somewhere else
        var authCalim = new List<Claim> { new Claim(ClaimTypes.Country, "Iran") };
        var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(12),
            claims: authCalim,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        //for convert it to string =>>>
        return TypedResults.Ok(new UserLoginResponse
        {
            JWT = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireTime = token.ValidTo
        });

    }
}

