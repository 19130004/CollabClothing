﻿using CollabClothing.ManageAdminApp.Service;
using CollabClothing.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ManageAdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;

        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            //var session = HttpContext.Session.GetString("Token"); //tao base controller
            var request = new GetUserRequestPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetListUser(request);
            return View(data.ResultObject);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Register(request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            if (user.IsSuccessed)
            {
                var userResult = user.ResultObject;
                var editUser = new UserEditRequest()
                {
                    Id = id,
                    Dob = userResult.Dob,
                    Email = userResult.Email,
                    FirstName = userResult.FirstName,
                    LastName = userResult.LastName,
                    PhoneNumber = userResult.PhoneNumber
                };
                return View(editUser);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.Edit(request.Id, request);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var user = await _userApiClient.GetById(Id);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(user.ResultObject);

        }
        //[HttpDelete]
        //public async Task<IActionResult> Delete(Guid Id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(ModelState);
        //    }
        //    var result = await _userApiClient.Delete(Id);
        //    if (result.IsSuccessed)
        //    {

        //    }
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }


    }
}