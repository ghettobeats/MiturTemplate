namespace MiturNetAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly MiturNetContext _MiturDBContext;
    private IWebHostEnvironment _env;
    private List<CustomMenu> tMenu { get; set; }
    private List<CustomMenu> MenuListBySerial { get; set; }
    private readonly UrlEncoder _urlEncoder;
    public IConfigurationRoot Configuration { get; set; }

    public AccountController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger,
        IConfiguration config,
        MiturNetContext MiturDBContext,
        IWebHostEnvironment env,
        UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _config = config;
        _MiturDBContext = MiturDBContext;
        _env = env;
        _urlEncoder = urlEncoder;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (!_MiturDBContext.Setting.Any())
                {
                    var settingData = System.IO.File.ReadAllText("Data/SettingSeedData.json");
                    var settings = JsonConvert.DeserializeObject<List<Setting>>(settingData);
                    foreach (var setting in settings)
                    {
                        _MiturDBContext.Setting.Add(setting);
                    }
                    _MiturDBContext.SaveChanges();
                }

                if (!_MiturDBContext.AspNetUsersMenus.Any())
                {
                    var menuData = System.IO.File.ReadAllText("Data/MenuSeedData.json");
                    var menus = JsonConvert.DeserializeObject<List<AspNetUsersMenu>>(menuData);
                    foreach (var AspNetUsersMenu in menus)
                    {
                        _MiturDBContext.AspNetUsersMenus.Add(AspNetUsersMenu);
                    }
                    _MiturDBContext.SaveChanges();
                }

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var roleId = "";
                        bool isEmailVerificationDisabled = false;
                        if (_MiturDBContext.AspNetUsers.Any())
                        {
                            if (_MiturDBContext.AspNetUsers.Where(x => x.Email == model.Email).Any())
                            {
                                return BadRequest("User with the same email already taken.");
                            }
                            if (_MiturDBContext.AspNetUsers.Where(x => x.UserName == model.UserName).Any())
                            {
                                return BadRequest("User with the same username already taken.");
                            }
                            else
                            {
                                roleId = _MiturDBContext.Setting.Where(x => x.VSettingId == "9B55EDC6-2B9C-4869-B5D7-8B2A788DAA12").FirstOrDefault().VSettingOption;

                                if (!_MiturDBContext.AspNetRoles.Where(x => x.Id == roleId).Any())
                                    return BadRequest("User registration role not set at settings. Please go to settings page and set the user registration role.");
                            }
                            isEmailVerificationDisabled = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "8196514E-70AA-47F0-8844-3AC3B707F8C5").FirstOrDefault().VSettingOption);
                        }
                        else
                        {
                            roleId = "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03";

                            List<AspNetUsersMenu> anuml = _MiturDBContext.AspNetUsersMenus.OrderBy(x => x.ISerialNo).ToList();

                            AspNetRoles role = new AspNetRoles
                            {
                                Id = roleId,
                                //Name = "Super Admin",
                                Name = "Administrador",
                                IndexPage = _MiturDBContext.AspNetUsersMenus.Where(x => x.VMenuId == "03D5B7E1-117D-429F-89AC-DE59E30B9386").FirstOrDefault().NvPageUrl
                            };
                            _MiturDBContext.AspNetRoles.Add(role);
                            _MiturDBContext.SaveChanges();

                            foreach (var mn in anuml)
                            {
                                AspNetUsersMenuPermission anump = new AspNetUsersMenuPermission
                                {
                                    VMenuPermissionId = Guid.NewGuid().ToString(),
                                    Id = roleId,
                                    VMenuId = mn.VMenuId
                                };
                                _MiturDBContext.AspNetUsersMenuPermission.Add(anump);
                                _MiturDBContext.SaveChanges();
                            }
                        }

                        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = isEmailVerificationDisabled, Date = DateTime.UtcNow };

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            user = await _userManager.FindByEmailAsync(model.Email);

                            var aspuser = _MiturDBContext.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault();

                            AspNetUserRoles anur = new AspNetUserRoles
                            {
                                UserId = aspuser.Id,
                                RoleId = roleId
                            };
                            _MiturDBContext.AspNetUserRoles.Add(anur);
                            _MiturDBContext.SaveChanges();

                            if (!isEmailVerificationDisabled)
                            {
                                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Host.Value);  //Only use when Core & Angular are in same domain
                                var host = _config.GetSection("AppSettings")["AngularPath"];
                                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, host);

                                var webRoot = System.IO.Directory.GetCurrentDirectory();
                                var sPath = System.IO.Path.Combine(webRoot, "EmailTemplate/email-confirmation.html");

                                var builder = new StringBuilder();
                                using (var reader = System.IO.File.OpenText(sPath))
                                {
                                    builder.Append(reader.ReadToEnd());
                                }

                                builder.Replace("{logo}", _config.GetSection("AppSettings")["Logo"]);
                                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                                builder.Replace("{username}", myTI.ToTitleCase(model.UserName));
                                builder.Replace("{appname}", _config.GetSection("AppSettings")["AppName"]);
                                builder.Replace("{verifylink}", callbackUrl);
                                builder.Replace("{twitterlink}", _config.GetSection("Company")["SocialTwitter"]);
                                builder.Replace("{facebooklink}", _config.GetSection("Company")["SocialFacebook"]);
                                builder.Replace("{companyname}", _config.GetSection("Company")["Company"]);
                                builder.Replace("{companyemail}", _config.GetSection("Company")["MailAddress"]);

                                //await _emailSender.SendEmailConfirmationAsync(model.Email, builder.ToString());

                                await _signInManager.SignInAsync(user, isPersistent: false);
                            }

                            scope.Complete();

                            return Ok(isEmailVerificationDisabled);
                        }
                        else
                        {
                            scope.Dispose();
                            return BadRequest(result.Errors);
                        }
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        if (ex.InnerException != null)
                        {
                            return BadRequest(ex.InnerException.Message);
                        }
                        else
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }
            return BadRequest("Registration Failed.");
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("confirmemail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        try
        {
            if (userId == "null" || code == "null")
            {
                return BadRequest("Invalid email verify link found.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest("User not found in system to verify email.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code.Replace(" ", "+"));
            if (result.Succeeded)
            {
                return StatusCode(201);
            }
            else
            {
                var codeparam = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, codeparam, Request.Host.Value);

                var webRoot = System.IO.Directory.GetCurrentDirectory();
                var sPath = System.IO.Path.Combine(webRoot, "EmailTemplate/email-confirmation.html");

                var builder = new StringBuilder();
                using (var reader = System.IO.File.OpenText(sPath))
                {
                    builder.Append(reader.ReadToEnd());
                }

                builder.Replace("{logo}", _config.GetSection("AppSettings")["Logo"]);
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                builder.Replace("{username}", myTI.ToTitleCase(user.UserName));
                builder.Replace("{appname}", _config.GetSection("AppSettings")["AppName"]);
                builder.Replace("{verifylink}", HtmlEncoder.Default.Encode(callbackUrl));
                builder.Replace("{twitterlink}", _config.GetSection("Company")["SocialTwitter"]);
                builder.Replace("{facebooklink}", _config.GetSection("Company")["SocialFacebook"]);
                builder.Replace("{companyname}", _config.GetSection("Company")["Company"]);
                builder.Replace("{companyemail}", _config.GetSection("Company")["MailAddress"]);

                await _emailSender.SendEmailConfirmationAsync(user.Email, builder.ToString());

                await _signInManager.SignInAsync(user, isPersistent: false);
                return BadRequest("Your mail verification token has expired. We send you another confirmation mail link to your email address.");
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }
    [AllowAnonymous]
    [HttpPost("login")]        
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByEmailAsync(model.Email);
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    //if (!user.EmailConfirmed)
                    //{
                    //    return BadRequest("Email not verified");
                    //}
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var loginResult = await LoginSuccess(user);
                       var dataLogIn = new { Data = loginResult, Message = "Login", Succes = true };

                        return Ok(dataLogIn);
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        return Ok(new { is2faenabled = true, user.Id });
                    }
                    else
                    {
                        //return BadRequest("El nombre de usuario y/o la contraseña no concuerdan con nuestros registros. Por favor verifique sus credenciales e inténtelo de nuevo.");
                        return Ok(new { Data = "", Message = "El nombre de usuario y / o la contraseña no concuerdan con nuestros registros.Por favor verifique sus credenciales e inténtelo de nuevo.", Succes = false });

                    }
                }
                else
                {
                    return Ok(new { Data = "", Message = "El nombre de usuario y / o la contraseña no concuerdan con nuestros registros.Por favor verifique sus credenciales e inténtelo de nuevo.", Succes = false });
                    //return BadRequest("El nombre de usuario y/o la contraseña no concuerdan con nuestros registros. Por favor verifique sus credenciales e inténtelo de nuevo.");
                }
            }
            return StatusCode(201);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }

    }

    [HttpPost("loginByTFA")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginByTFA(TwoFactorAuthModel TFM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(TFM.Id);
                if (user == null)
                {
                    return BadRequest("No User Found");
                }

                var authenticatorCode = TFM.TFACode.Replace(" ", string.Empty).Replace("-", string.Empty);
                var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);

                if (is2FaTokenValid)
                {
                    var loginResult = await LoginSuccess(user);
                    return Ok(loginResult);
                }
                else
                {
                    return BadRequest("This code is not valid. Please try again.");
                }
            }
            return StatusCode(201);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet]
    private async Task<object> LoginSuccess(ApplicationUser user)
    {
        try
        {
            
            var ULHID ="";
            var roleId = _MiturDBContext.AspNetUserRoles.Where(x => x.UserId == user.Id).FirstOrDefault().RoleId;
            var index = _MiturDBContext.AspNetRoles.Where(x => x.Id == roleId).FirstOrDefault().IndexPage;
            var is2faenabled = user.TwoFactorEnabled;
            
           

            using (var _MiturDBContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    ULHID = Guid.NewGuid().ToString();
                    var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                    AspNetUsersLoginHistory anulh = new AspNetUsersLoginHistory();
                    anulh.VUlhid = ULHID;
                    anulh.Id = user.Id;
                    anulh.DLogIn = DateTime.UtcNow;
                    anulh.NvIpaddress = ip;
                    _MiturDBContext.AspNetUsersLoginHistory.Add(anulh);
                    _MiturDBContext.SaveChanges();

                    _MiturDBContextTransaction.Commit();
                }
                catch
                {
                    _MiturDBContextTransaction.Rollback();
                }
            }

            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings")["Token"]));
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var rolesString = JsonConvert.SerializeObject(roles);

            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim("userName",user.UserName),
                                //new Claim("email",user.Email),
                               // new Claim("role", rolesString),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            roles.ForEach((r =>
            {
                claim.Add(new Claim(ClaimTypes.Role, r));
            }));
            var tokeOptions = new JwtSecurityToken(
                         issuer: Request.Host.Value,
                         audience: Request.Host.Value,

                       // issuer: _config["AppSettings:Issuer"],
                       // audience: _config["AppSettings:Audience"],
                        claims: claim/*new List<Claim>(
                            new List<Claim> {
                                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                new Claim("userName",user.UserName),
                                //new Claim("email",user.Email),
                                new Claim("role", rolesString),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            }
                        )*/,
                        expires: DateTime.Now.AddDays(2),
                        signingCredentials: signinCredentials
            );
            
            var tokenHandler = new JwtSecurityTokenHandler();
            getDBConnection.Token = tokenHandler.WriteToken(tokeOptions);
            return new
            {
                token = tokenHandler.WriteToken(tokeOptions),
                user.Id,
                index,
                ulhid = ULHID,
                is2faenabled                    
            };
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("generateQRCodeUri")]
    public async Task<IActionResult> GenerateQRCodeUri()
    {
        try
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(id);
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }
            var AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            var uri = string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("CoreADMIN"),
                _urlEncoder.Encode(user.Email),
                unformattedKey);
            return Ok(new
            {
                uri
            });
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("verifyAuthApp")]
    public async Task<IActionResult> VerifyAuthApp(TwoFactorAuthModel TFM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(Id);
                var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, TFM.TFACode);

                if (is2FaTokenValid)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                    return Ok();
                }
                else
                {
                    return BadRequest("This code is not valid. Please try again.");
                }
            }
            return StatusCode(201);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("isLoggedIn")]
    public IActionResult IsLoggedIn()
    {
        return Ok(true);
    }

    [HttpGet("isAuthorized")]
    public IActionResult IsAuthorized(string url)
    {
        var isAuthorized = true;
        if (url != "/home/profile")
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleId = _MiturDBContext.AspNetUserRoles.Where(x => x.UserId == Id).FirstOrDefault().RoleId;
            var menus = _MiturDBContext.AspNetUsersMenus.Where(x => x.NvPageUrl == url).ToList();
            foreach (var menu in menus)
            {
                isAuthorized = _MiturDBContext.AspNetUsersMenuPermission.Where(x => x.VMenuId == menu.VMenuId && x.Id == roleId).Any();
                if (isAuthorized)
                {
                    break;
                }
            }
        }
        return Ok(isAuthorized);
    }

    [HttpGet("isAllowed")]
    [AllowAnonymous]
    public IActionResult IsAllowed(string id)
    {
        var isAllowed = true;
        if (_MiturDBContext.AspNetUsers.Any())
        {
            isAllowed = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == id).FirstOrDefault().VSettingOption);
        }
        return Ok(isAllowed);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout(string ulhid)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                    AspNetUsersLoginHistory anulh = _MiturDBContext.AspNetUsersLoginHistory.Where(x => x.VUlhid == ulhid && x.NvIpaddress == ip).FirstOrDefault();
                    anulh.DLogOut = DateTime.UtcNow;
                    _MiturDBContext.Entry(anulh).State = EntityState.Modified;
                    _MiturDBContext.SaveChanges();
                    await _signInManager.SignOutAsync();
                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("forgotpassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest("No user found against this email id.");
                }
                else
                {
                    bool isUserSuperAdmin = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03" && x.UserId == user.Id).Any();
                    bool isAllowed = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "95A1ED0B-9645-4E18-9BD1-CAAB4F9F21F5").FirstOrDefault().VSettingOption);
                    if (isUserSuperAdmin || isAllowed)
                    {

                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Host.Value);

                        var webRoot = System.IO.Directory.GetCurrentDirectory();
                        var sPath = System.IO.Path.Combine(webRoot, "EmailTemplate/reset-password.html");

                        var builder = new StringBuilder();
                        using (var reader = System.IO.File.OpenText(sPath))
                        {
                            builder.Append(reader.ReadToEnd());
                        }

                        builder.Replace("{logo}", _config.GetSection("AppSettings")["Logo"]);
                        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                        builder.Replace("{username}", myTI.ToTitleCase(user.UserName));
                        builder.Replace("{verifylink}", callbackUrl);
                        builder.Replace("{twitterlink}", _config.GetSection("Company")["SocialTwitter"]);
                        builder.Replace("{facebooklink}", _config.GetSection("Company")["SocialFacebook"]);
                        builder.Replace("{companyname}", _config.GetSection("Company")["Company"]);
                        builder.Replace("{companyemail}", _config.GetSection("Company")["MailAddress"]);

                        await _emailSender.SendEmailAsync(model.Email, "Reset Password", builder.ToString());

                        return Ok();
                    }
                    return BadRequest("Recovery password not allowed");
                }
            }
            return StatusCode(400);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("resetpassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            var user = await _userManager.FindByIdAsync(model.id);
            
            if (user == null)
            {
                return BadRequest("User not found in system to change password.");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var confrimationCode =  await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, confrimationCode, model.Password);
                if (result.Succeeded)
                {
                    AspNetUsers anu = _MiturDBContext.AspNetUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                    anu.TwoFactorEnabled = false;
                    _MiturDBContext.Entry(anu).State = EntityState.Modified;
                    _MiturDBContext.SaveChanges();
                    scope.Complete();                        
                    return Ok(new { Data = anu, Message = "Changed", Succes = true });
                }
                else
                {
                    scope.Dispose();
                    return BadRequest(result.Errors.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getCurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await (from asp in _MiturDBContext.AspNetUsers
                              where asp.Id == Id
                              select new
                              {
                                  Name = asp.AspNetUsersProfile.VFirstName + " " + asp.AspNetUsersProfile.VLastName,
                                  Photo = asp.AspNetUsersProfile.VPhoto
                              }).ToListAsync();

            var data = new { CURRENTUSER = user };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getCurrentUserByID")]
    public async Task<IActionResult> GetCurrentUserByID(string id)
    {
        try
        {
            //var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await (from asp in _MiturDBContext.AspNetUsers
                              where asp.Id == id
                              select new
                              {
                                  asp.Id,
                                  asp.UserName,
                                  asp.AspNetUserRoles.FirstOrDefault().RoleId,
                                  asp.AspNetUsersProfile.VFirstName,
                                  asp.AspNetUsersProfile.VLastName
                                  //asp.AspNetUsersProfile.VGender
                                  //id = asp.Id,
                                  //username = asp.UserName,
                                  //Name = asp.AspNetUsersProfile.VFirstName + " " + asp.AspNetUsersProfile.VLastName,
                                  //Photo = asp.AspNetUsersProfile.VPhoto
                              }).FirstOrDefaultAsync();

            var data = new { Data = user, Message = "Ok", Succes = true };

            // var data = new { CURRENTUSER = user };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }


    [HttpGet("pagevisit")]
    public async Task<IActionResult> PageVisit(string path)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var ip = HttpContext.Connection.RemoteIpAddress.ToString();

                    AspNetUsersPageVisited anupv = new AspNetUsersPageVisited();
                    anupv.VPageVisitedId = Guid.NewGuid().ToString();
                    anupv.Id = Id;
                    anupv.NvPageName = path;
                    anupv.DDateVisited = DateTime.UtcNow;
                    anupv.NvIpaddress = ip;
                    _MiturDBContext.AspNetUsersPageVisited.Add(anupv);
                    _MiturDBContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getDashboardData")]
    public async Task<IActionResult> GetDashboardData(int offset)
    {
        try
        {
            var userlist = await _MiturDBContext.AspNetUsers.ToListAsync();
            var totalUser = userlist.Count();
            var activeUser = userlist.Where(x => x.EmailConfirmed == true).Count();
            var totalLogin = await _MiturDBContext.AspNetUsersLoginHistory.CountAsync();
            var totalPageVisit = await _MiturDBContext.AspNetUsersPageVisited.CountAsync();
            var roles = await _MiturDBContext.AspNetRoles.ToListAsync();
            var pages = await _MiturDBContext.AspNetUsersPageVisited.ToListAsync();
            var pagess = pages.GroupBy(x => x.NvPageName).OrderByDescending(x => x.Count()).ToList();

            var RWUDataList = new List<dynamic>();
            var rwuLabels = new List<string>();
            var rwuData = new List<int>();
            var rwuBackgroundColor = new List<string>();
            var random = new Random();
            foreach (var role in roles)
            {
                rwuLabels.Add(role.Name);
                var userCount = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == role.Id).Count();
                rwuData.Add(userCount);
                var color = string.Format("#{0:X6}", random.Next(0x100000));
                rwuBackgroundColor.Add(color);
            }
            RWUDataList.Add(rwuLabels);
            RWUDataList.Add(rwuData);
            RWUDataList.Add(rwuBackgroundColor);

            var TRUDataList = new List<dynamic>();
            var truLabels = new List<string>();
            var truData = new List<int>();
            var lhData = new List<List<int>>();
            DateTime today = DateTime.UtcNow;
            today = today.AddMinutes(offset);
            for (var i = 5; i >= 0; i--)
            {
                var predate = today.AddMonths(-i);
                var month = predate.ToString("MMM");
                var userCount = (from ur in userlist
                                 where ur.Date.AddMinutes(offset).Month == predate.Month && ur.Date.AddMinutes(offset).Year == predate.Year
                                 select ur).Count();

                var countList = new List<int>();
                var days = DateTime.DaysInMonth(predate.Year, predate.Month);
                for (var j = 1; j <= days; j++)
                {
                    var loginCount = (from lh in _MiturDBContext.AspNetUsersLoginHistory
                                      where lh.DLogIn.AddMinutes(offset).Month == predate.Month && lh.DLogIn.AddMinutes(offset).Year == predate.Year && lh.DLogIn.AddMinutes(offset).Day == j
                                      select lh).Count();
                    countList.Add(loginCount);
                    if (i == 0 && today.Day == j)
                        break;
                }

                lhData.Add(countList);
                truLabels.Add(month);
                truData.Add(userCount);
            }
            TRUDataList.Add(truLabels);
            TRUDataList.Add(truData);

            var TPVDataList = new List<dynamic>();
            var tpvLabels = new List<string>();
            var tpvData = new List<int>();
            foreach (var p in pagess)
            {
                tpvLabels.Add(p.Key.Substring(1));
                tpvData.Add(p.Count());
            }
            TPVDataList.Add(tpvLabels);
            TPVDataList.Add(tpvData);

            var data = new { TOTALUSER = totalUser, ACTIVEUSER = activeUser, TOTALLOGIN = totalLogin, TOTALPAGEVISIT = totalPageVisit, RWUDATA = RWUDataList, TRUDATA = TRUDataList, LHDATA = lhData, TPVDATA = TPVDataList };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getSideMenu")]
    public async Task<IActionResult> GetSideMenu()
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var roleId = _MiturDBContext.AspNetUserRoles.Where(x => x.UserId == userId).FirstOrDefault().RoleId;
                    var menuData = await (from mn in _MiturDBContext.AspNetUsersMenus
                                          join anump in _MiturDBContext.AspNetUsersMenuPermission on mn.VMenuId equals anump.VMenuId
                                          where anump.Id == roleId
                                          select new
                                          {
                                              mn.VMenuId,
                                              mn.VParentMenuId,
                                              mn.ISerialNo,
                                              mn.NvMenuName,
                                              mn.NvPageUrl,
                                              mn.NvFabIcon,
                                          }).OrderBy(x => x.ISerialNo).ToListAsync();

                    List<CustomMenu> menuList = new List<CustomMenu>();
                    foreach (var mn in menuData)
                    {
                        CustomMenu menu = new CustomMenu();
                        menu.vMenuID = mn.VMenuId;
                        menu.vParentMenuID = mn.VParentMenuId;
                        menu.iSerialNo = mn.ISerialNo;
                        menu.nvMenuName = mn.NvMenuName;
                        menu.nvPageUrl = mn.NvPageUrl;
                        menu.nvFabIcon = mn.NvFabIcon;
                        menu.Child = new List<CustomMenu>();
                        menuList.Add(menu);
                    }

                    var pMenu = menuList.Where(x => x.vParentMenuID == null).OrderBy(x => x.iSerialNo).ToList();
                    tMenu = menuList.Where(x => x.vParentMenuID != null).OrderBy(x => x.iSerialNo).ToList();

                    List<CustomMenu> Menu = new List<CustomMenu>();
                    foreach (var pm in pMenu)
                    {
                        CustomMenu mn = new CustomMenu();
                        mn = GetMenuHierarchy(pm, tMenu.ToList());
                        Menu.Add(mn);
                    }

                    var data = new { MENU = Menu };
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    private CustomMenu GetMenuHierarchy(CustomMenu pMenu, List<CustomMenu> menuList)
    {
        List<CustomMenu> childList = new List<CustomMenu>();
        List<CustomMenu> tempList = new List<CustomMenu>();
        foreach (var ml in menuList)
        {
            if (pMenu.vMenuID == ml.vParentMenuID)
            {
                childList.Add(ml);
                tMenu.Remove(ml);
            }
            else
                tempList.Add(ml);
        }
        foreach (var cl in childList)
        {
            CustomMenu child = new CustomMenu();
            child = GetMenuHierarchy(cl, tempList);
            pMenu.Child.Add(child);
        }

        return pMenu;
    }

    [HttpGet("loginhistorydata")]
    public async Task<IActionResult> LoginHistoryData()
    {
        try
        {
            var loginList = await (from anu in _MiturDBContext.AspNetUsers
                                   join anulh in _MiturDBContext.AspNetUsersLoginHistory on anu.Id equals anulh.Id
                                   join anup in _MiturDBContext.AspNetUsersProfile on anu.Id equals anup.Id into joined
                                   from anup in joined.DefaultIfEmpty()
                                   select new
                                   {
                                       anu.Id,
                                       Name = anup.VFirstName + " " + anup.VLastName,
                                       anu.Email,
                                       Login = anulh.DLogIn,
                                       Logout = anulh.DLogOut,
                                       IP = anulh.NvIpaddress,
                                   }).OrderByDescending(x => x.Login).ToListAsync();
            var totalLogin = loginList.Count();
            var highestLogin = (from ll in loginList
                                group ll.Id by ll.Id into l
                                select new
                                {
                                    Id = l.Key,
                                    Count = l.ToList().Count(),
                                    loginList.Where(x => x.Id == l.Key).FirstOrDefault().Name
                                }).OrderByDescending(x => x.Count).FirstOrDefault();

            var data = new { LOGINLIST = loginList, TOTALLOGIN = totalLogin, HIGHESTLOGINBY = highestLogin.Name, HIGHESTLOGIN = highestLogin.Count };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("pagevisitdata")]
    public async Task<IActionResult> PageVisitHistoryData()
    {
        try
        {
            var visitedList = await (from anu in _MiturDBContext.AspNetUsers
                                     join anupv in _MiturDBContext.AspNetUsersPageVisited on anu.Id equals anupv.Id
                                     join anup in _MiturDBContext.AspNetUsersProfile on anu.Id equals anup.Id into joined
                                     from anup in joined.DefaultIfEmpty()
                                     select new
                                     {
                                         anu.Id,
                                         Name = anup.VFirstName + " " + anup.VLastName,
                                         anu.Email,
                                         PageName = anupv.NvPageName,
                                         VisitedDate = anupv.DDateVisited,
                                         IP = anupv.NvIpaddress,
                                     }).OrderByDescending(x => x.VisitedDate).ToListAsync();
            var totalVisit = visitedList.Count();
            var highestVisit = (from vl in visitedList
                                group vl.PageName by vl.PageName into l
                                select new
                                {
                                    PageName = l.Key,
                                    Count = l.ToList().Count()
                                }).OrderByDescending(x => x.Count).FirstOrDefault();
            var highestVisitedBy = (from vl in visitedList
                                    group vl.Id by vl.Id into l
                                    select new
                                    {
                                        Id = l.Key,
                                        Count = l.ToList().Count(),
                                        visitedList.Where(x => x.Id == l.Key).FirstOrDefault().Name
                                    }).OrderByDescending(x => x.Count).FirstOrDefault();

            var data = new { VISITEDLIST = visitedList, TOTALVISIT = totalVisit, HIGHESTVISITEDPAGE = highestVisit.PageName, HIGHESTVISITEDBY = highestVisitedBy.Name };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("pendingemailverification")]
    public async Task<IActionResult> PendingEmailVerification()
    {
        try
        {
            var user = await (from asp in _MiturDBContext.AspNetUsers
                              where asp.EmailConfirmed == false
                              select new
                              {
                                  asp.Id,
                                  asp.Email,
                                  uType = asp.AspNetUserRoles.Where(x => x.UserId == asp.Id).FirstOrDefault().Role.Name
                              }).ToListAsync();

            var data = new { PEV = user };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getSettingData")]
    public async Task<IActionResult> GetSettingData()
    {
        try
        {
            var roles = await (from rl in _MiturDBContext.AspNetRoles
                               where rl.Id != "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03"
                               select new
                               {
                                   rl.Id,
                                   rl.Name
                               }).OrderBy(x => x.Name).ToListAsync();

            var settings = _MiturDBContext.Setting.ToList();
            Settings customSettings = new Settings
            {
                UserRegister = Convert.ToBoolean(settings.Where(x => x.VSettingId == "82A52FA2-E91F-4195-84FD-8EA32DA2637A").FirstOrDefault().VSettingOption),
                EmailVerificationDisable = Convert.ToBoolean(settings.Where(x => x.VSettingId == "8196514E-70AA-47F0-8844-3AC3B707F8C5").FirstOrDefault().VSettingOption),
                UserRole = settings.Where(x => x.VSettingId == "9B55EDC6-2B9C-4869-B5D7-8B2A788DAA12").FirstOrDefault().VSettingOption,
                RecoverPassword = Convert.ToBoolean(settings.Where(x => x.VSettingId == "95A1ED0B-9645-4E18-9BD1-CAAB4F9F21F5").FirstOrDefault().VSettingOption),
                ChangePassword = Convert.ToBoolean(settings.Where(x => x.VSettingId == "A55D224B-8C28-4A27-A767-C15C089F26A8").FirstOrDefault().VSettingOption),
                ChangeProfile = Convert.ToBoolean(settings.Where(x => x.VSettingId == "03665F19-463B-4168-94AE-A27D9857605A").FirstOrDefault().VSettingOption),
                TwoFactorEnabled = Convert.ToBoolean(settings.Where(x => x.VSettingId == "AC80C53B-9D5C-4D03-8FB8-1793ABB3DB60").FirstOrDefault().VSettingOption),
                ExternalLogin = Convert.ToBoolean(settings.Where(x => x.VSettingId == "FAC27C8C-ED92-453E-84A9-659C5D8FD651").FirstOrDefault().VSettingOption)
            };
            List<Settings> CustomSettingsList = new List<Settings>();
            CustomSettingsList.Add(customSettings);

            var data = new { ROLES = roles, SETTINGS = CustomSettingsList };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("updateSetting")]
    public async Task<IActionResult> UpdateSetting(Settings setting)
    {
        using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
        {
            try
            {
                var settings = _MiturDBContext.Setting.ToList();
                bool preTFE = false;
                bool postTFE = false;
                foreach (var st in settings)
                {
                    if (st.VSettingId == "82A52FA2-E91F-4195-84FD-8EA32DA2637A")
                        st.VSettingOption = setting.UserRegister.ToString();
                    else if (st.VSettingId == "8196514E-70AA-47F0-8844-3AC3B707F8C5")
                        st.VSettingOption = setting.EmailVerificationDisable.ToString();
                    else if (st.VSettingId == "9B55EDC6-2B9C-4869-B5D7-8B2A788DAA12")
                        st.VSettingOption = setting.UserRole;
                    else if (st.VSettingId == "95A1ED0B-9645-4E18-9BD1-CAAB4F9F21F5")
                        st.VSettingOption = setting.RecoverPassword.ToString();
                    else if (st.VSettingId == "A55D224B-8C28-4A27-A767-C15C089F26A8")
                        st.VSettingOption = setting.ChangePassword.ToString();
                    else if (st.VSettingId == "03665F19-463B-4168-94AE-A27D9857605A")
                        st.VSettingOption = setting.ChangeProfile.ToString();
                    else if (st.VSettingId == "AC80C53B-9D5C-4D03-8FB8-1793ABB3DB60")
                    {
                        preTFE = Convert.ToBoolean(st.VSettingOption);
                        postTFE = setting.TwoFactorEnabled;
                        st.VSettingOption = setting.TwoFactorEnabled.ToString();
                    }
                    else if (st.VSettingId == "FAC27C8C-ED92-453E-84A9-659C5D8FD651")
                        st.VSettingOption = setting.ExternalLogin.ToString();

                    _MiturDBContext.Entry(st).State = EntityState.Modified;
                    _MiturDBContext.SaveChanges();
                }

                if (preTFE)
                {
                    if (!postTFE)
                    {
                        _MiturDBContext.AspNetUsers
                        .Where(x => x.TwoFactorEnabled == true)
                        .ToList()
                        .ForEach(y => y.TwoFactorEnabled = false);

                        _MiturDBContext.SaveChanges();
                    }
                }

                dbContextTransaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    [HttpGet("getMenuList")]
    public async Task<IActionResult> GetMenuList()
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    var menuData = await (from mn in _MiturDBContext.AspNetUsersMenus
                                          select new
                                          {
                                              mn.VMenuId,
                                              mn.VParentMenuId,
                                              mn.ISerialNo,
                                              mn.NvMenuName,
                                              mn.NvPageUrl,
                                              mn.NvFabIcon,
                                          }).OrderBy(x => x.ISerialNo).ToListAsync();

                    List<CustomMenu> menuList = new List<CustomMenu>();
                    foreach (var mn in menuData)
                    {
                        CustomMenu menu = new CustomMenu();
                        menu.vMenuID = mn.VMenuId;
                        menu.vParentMenuID = mn.VParentMenuId;
                        menu.iSerialNo = mn.ISerialNo;
                        menu.nvMenuName = mn.NvMenuName;
                        menu.nvPageUrl = mn.NvPageUrl;
                        menu.nvFabIcon = mn.NvFabIcon;
                        menu.Child = new List<CustomMenu>();
                        menuList.Add(menu);
                    }

                    var pMenu = menuList.Where(x => x.vParentMenuID == null).OrderBy(x => x.iSerialNo).ToList();
                    tMenu = menuList.Where(x => x.vParentMenuID != null).OrderBy(x => x.iSerialNo).ToList();

                    List<CustomMenu> Menu = new List<CustomMenu>();
                    MenuListBySerial = new List<CustomMenu>();
                    foreach (var pm in pMenu)
                    {
                        pm.NameWithParent = pm.nvMenuName;
                        MenuListBySerial.Add(pm);
                        CustomMenu mn = new CustomMenu();
                        GetAllMenu(pm, tMenu.ToList());
                        Menu.Add(mn);
                    }
                    var parent = MenuListBySerial.Where(x => x.nvPageUrl == "#").ToList();
                    var Index = MenuListBySerial.Where(x => x.nvPageUrl != "#").ToList();

                    var data = new { MENULIST = MenuListBySerial, PARENT = parent, INDEX = Index };
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    private void GetAllMenu(CustomMenu pMenu, List<CustomMenu> menuList)
    {
        List<CustomMenu> childList = new List<CustomMenu>();
        List<CustomMenu> tempList = new List<CustomMenu>();
        foreach (var ml in menuList)
        {
            if (pMenu.vMenuID == ml.vParentMenuID)
            {
                ml.NameWithParent = (pMenu.NameWithParent == null ? pMenu.nvMenuName : pMenu.NameWithParent) + " >> " + ml.nvMenuName;
                childList.Add(ml);
                tMenu.Remove(ml);
            }
            else
                tempList.Add(ml);
        }
        foreach (var cl in childList)
        {
            MenuListBySerial.Add(cl);
            GetAllMenu(cl, tempList);
        }
    }

    [HttpPost("saveMenu")]
    public async Task<IActionResult> SaveMenu(AspNetUsersMenu data)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    bool isSerialExists = _MiturDBContext.AspNetUsersMenus.Where(x => x.ISerialNo == data.ISerialNo && x.VParentMenuId == data.VParentMenuId && x.VMenuId != data.VMenuId).Any();
                    if (isSerialExists)
                        return BadRequest("Serial No. Exists");

                    if (data.VMenuId == null)
                    {
                        data.VMenuId = Guid.NewGuid().ToString();
                        _MiturDBContext.AspNetUsersMenus.Add(data);
                        _MiturDBContext.SaveChanges();
                    }
                    else
                    {
                        _MiturDBContext.Entry(data).State = EntityState.Modified;
                        _MiturDBContext.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("deleteMenu")]
    public async Task<IActionResult> DeleteMenu(string Id)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    _MiturDBContext.AspNetUsersMenus.RemoveRange(_MiturDBContext.AspNetUsersMenus.Where(x => x.VMenuId == Id));
                    _MiturDBContext.SaveChanges();

                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getRoleList")]
    public async Task<IActionResult> GetRoleList()
    {
        try
        {
            //var cId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var RoleId = _MiturDBContext.AspNetUserRoles.Where(x => x.UserId == cId).FirstOrDefault().RoleId;
            

            //var Role = RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03" ? null : "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03";
            var roles = await (from role in _MiturDBContext.AspNetRoles
                               //where role.Id != Role
                               select new
                               {
                                   role.Id,
                                   role.Name
                                   //role.IndexPage,
                                   //totalUser = role.AspNetUserRoles.Where(x => x.RoleId == role.Id).Count()
                               }).ToListAsync();

            //var data = new { ROLES = roles };
            // var data = new { Data = users, Message = "Ok", Succes = true };
            var data = new { Data = roles, Message = "Ok", Succes = true };
           // return Ok(new { Data = roles, Message = "Ok", Succes = true });
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                var data = new { Data = "", Message = ex.InnerException.Message, Succes = false };
                return Ok(data);
                //return BadRequest(ex.InnerException.Message);
            }
            else
            {
                var data = new { Data = "", Message = ex.InnerException.Message, Succes = false };
                return Ok(data);
                
                //return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getRoleById")]
    public async Task<IActionResult> GetRoleById(string Id)
    {
        try
        {
            var getData =  (from role in _MiturDBContext.AspNetRoles
                                 where role.Id == Id
                                 select new
                                 {
                                     role.Id,
                                     role.Name,
                                     role.IndexPage
                                 }).FirstOrDefault();
                                 //.ToListAsync(); //ESTA MAL, SE SUPONE QUE ES UN SOLO ROL, NO UNA LISTA DE ROLES


            var menuPer = await (from anump in _MiturDBContext.AspNetUsersMenuPermission
                                 where anump.Id == Id
                                 select new
                                 {
                                     anump.VMenuId
                                 }).ToListAsync();
            var data = new { ROLE = getData, MENUPERMISSION = menuPer };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("saveRole")]
    public async Task<IActionResult> SaveRole(RoleWithMenuPermission model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    model.Role.IndexPage = "/home/index";
                    if (model.Role.Id == null)
                    {
                        model.Role.Id = Guid.NewGuid().ToString();
                        _MiturDBContext.AspNetRoles.Add(model.Role);
                        _MiturDBContext.SaveChanges();
                    }
                    else
                    {
                        _MiturDBContext.Entry(model.Role).State = EntityState.Modified;
                        _MiturDBContext.SaveChanges();
                    }
                    
                    //_MiturDBContext.AspNetUsersMenuPermission.RemoveRange(_MiturDBContext.AspNetUsersMenuPermission.Where(x => x.Id == model.Role.Id));
                    //_MiturDBContext.SaveChanges();
                    //if (!model.MenuPermission.Any())
                    //{
                    //    model.MenuPermission.Add(new AspNetUsersMenuPermission
                    //    {
                    //        VMenuPermissionId = Guid.NewGuid().ToString(),
                    //        Id = model.Role.Id,
                    //        VMenuId = "03D5B7E1-117D-429F-89AC-DE59E30B9386"
                    //    });
                    //    foreach (var mn in model.MenuPermission)
                    //    {
                    //        //mn.VMenuPermissionId = Guid.NewGuid().ToString();
                    //        //mn.Id = model.Role.Id;
                    //        _MiturDBContext.AspNetUsersMenuPermission.Add(mn);
                    //        _MiturDBContext.SaveChanges();
                    //    }
                    //}
                    

                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("deleteRole")]
    public async Task<IActionResult> DeleteRole(string Id)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    _MiturDBContext.AspNetUsersMenuPermission.RemoveRange(_MiturDBContext.AspNetUsersMenuPermission.Where(x => x.Id == Id));
                    _MiturDBContext.SaveChanges();

                    _MiturDBContext.AspNetRoles.Remove(_MiturDBContext.AspNetRoles.Where(x => x.Id == Id).FirstOrDefault());
                    _MiturDBContext.SaveChanges();

                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getUserList")]
    public async Task<IActionResult> GetUserList()
    {
        try
        {
            var Id = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03").FirstOrDefault().UserId;
            Id = User.FindFirstValue(ClaimTypes.NameIdentifier) == Id ? null : Id;
            var roles = await (from role in _MiturDBContext.AspNetRoles
                               where role.Id != (Id == null ? null : "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03")
                               select new
                               {
                                   role.Id,
                                   role.Name
                               }).OrderBy(x => x.Name).ToListAsync();

            var users = await (from usr in _MiturDBContext.AspNetUsers
                               where usr.Id != Id
                               select new
                               {
                                   usr.Id,
                                   usr.Email,
                                   usr.UserName,
                                   RoleName = usr.AspNetUserRoles.FirstOrDefault().Role.Name,
                                   RoleId = usr.AspNetUserRoles.FirstOrDefault().Role.Id,
                                   usr.AspNetUsersProfile.VFirstName,
                                   usr.AspNetUsersProfile.VLastName,
                                   usr.PhoneNumber,
                                   FullName = usr.AspNetUsersProfile.VFirstName + " " + usr.AspNetUsersProfile.VLastName,
                                   usr.AspNetUsersProfile.VCountry,
                                   //usr.AspNetUsersProfile.VGender,
                                   usr.AspNetUsersProfile.VPhoto                                       
                               }).ToListAsync();

            // var data = new { ROLES = roles, USERLIST = users };

            var data = new { Data = users, Message = "Ok", Succes = true };            

            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("getUserByID")]
    public async Task<IActionResult> GetUserListByID(string Id)
    {
        try
        {
            //var Id = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03").FirstOrDefault().UserId;
            //Id = User.FindFirstValue(ClaimTypes.NameIdentifier) == Id ? null : Id;
            //var roles = await (from role in _MiturDBContext.AspNetRoles
            //                   //where role.Id != (Id == null ? null : "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03")
            //                   select new
            //                   {
            //                       role.Id,
            //                       role.Name
            //                   }).OrderBy(x => x.Name).ToListAsync();

            var users = await (from usr in _MiturDBContext.AspNetUsers
                               where usr.Id == Id
                               select new
                               {
                                   usr.Id,
                                   //usr.Email,
                                   usr.UserName,                                       
                                   //RoleName = usr.AspNetUserRoles.FirstOrDefault().Role.Name,
                                   RoleId = usr.AspNetUserRoles.FirstOrDefault().Role.Id,
                                   usr.AspNetUsersProfile.VFirstName,
                                   usr.AspNetUsersProfile.VLastName,
                                   //usr.PhoneNumber,
                                   //FullName = usr.AspNetUsersProfile.VFirstName + " " + usr.AspNetUsersProfile.VLastName,
                                   //usr.AspNetUsersProfile.VCountry,
                                   usr.AspNetUsersProfile.VGender,
                                   //usr.AspNetUsersProfile.VPhoto
                               }).FirstOrDefaultAsync();

            //var data = new { ROLES = roles, USERLIST = users };
            //var data = new {USERLIST = users };
            var data = new { Data = users, Message = "Ok", Succes = true };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }


    [HttpPost("saveUser")]
    public async Task<IActionResult> SaveUser(UserRegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                //Email = model.Email,
                //PhoneNumber = model.PhoneNumber,
                //EmailConfirmed = model.EmailVerificationDisabled,
                Date = DateTime.UtcNow
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //bool isEmailDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.Email == model.Email).Any();
                    bool isUserNameDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.UserName == model.UserName).Any();
                    //if (isEmailDuplicate)
                    //    return BadRequest("duplicate_email");
                    if (isUserNameDuplicate)
                        return BadRequest("duplicate_userName");
                    else
                    {
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            user = await _userManager.FindByNameAsync(model.UserName);
                            //user = await _userManager.FindByEmailAsync(model.Email);
                            var aspuser = _MiturDBContext.AspNetUsers.Where(x => x.UserName == model.UserName).FirstOrDefault();
                            //var aspuser = _MiturDBContext.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault();
                            AspNetUserRoles anur = new AspNetUserRoles();
                            anur.UserId = aspuser.Id;
                            anur.RoleId = model.RoleId;
                            _MiturDBContext.AspNetUserRoles.Add(anur);
                            _MiturDBContext.SaveChanges();

                            AspNetUsersProfile anup = new AspNetUsersProfile();
                            anup.Id = user.Id;
                            anup.VFirstName = model.VFirstName;
                            anup.VLastName = model.VLastName;
                            //anup.VCountry = model.Country;
                            anup.VGender = model.VGender;
                            //anup.VPhoto = "../../assets/images/profile_Image/" + user.Id + ".png";
                            _MiturDBContext.AspNetUsersProfile.Add(anup);
                            _MiturDBContext.SaveChanges();

                            //bool isImageSaved = UploadImage(model.Photo, user.Id);
                            //if (isImageSaved)
                            //{
                            //    AspNetUsersProfile anup = new AspNetUsersProfile();
                            //    anup.Id = user.Id;
                            //    anup.VFirstName = model.FirstName;
                            //    anup.VLastName = model.LastName;
                            //    //anup.VCountry = model.Country;
                            //    anup.VGender = model.Gender;
                            //    anup.VPhoto = "../../assets/images/profile_Image/" + user.Id + ".png";
                            //    _MiturDBContext.AspNetUsersProfile.Add(anup);
                            //    _MiturDBContext.SaveChanges();
                            //}
                            //else
                            //{
                            //    scope.Dispose();
                            //    return BadRequest("Something Wrong!");
                            //}

                            //if (!model.EmailVerificationDisabled)
                            //{
                            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            //    // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Host.Value);  //Only use when Core & Angular are in same domain
                            //    var host = _config.GetSection("AppSettings")["AngularPath"];
                            //    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, host);

                            //    var webRoot = System.IO.Directory.GetCurrentDirectory();
                            //    var sPath = System.IO.Path.Combine(webRoot, "EmailTemplate/email-confirmation.html");

                            //    var builder = new StringBuilder();
                            //    using (var reader = System.IO.File.OpenText(sPath))
                            //    {
                            //        builder.Append(reader.ReadToEnd());
                            //    }

                            //    builder.Replace("{logo}", _config.GetSection("AppSettings")["Logo"]);
                            //    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                            //    builder.Replace("{username}", myTI.ToTitleCase(model.UserName));
                            //    builder.Replace("{appname}", _config.GetSection("AppSettings")["AppName"]);
                            //    builder.Replace("{verifylink}", callbackUrl);
                            //    builder.Replace("{twitterlink}", _config.GetSection("Company")["SocialTwitter"]);
                            //    builder.Replace("{facebooklink}", _config.GetSection("Company")["SocialFacebook"]);
                            //    builder.Replace("{companyname}", _config.GetSection("Company")["Company"]);
                            //    builder.Replace("{companyemail}", _config.GetSection("Company")["MailAddress"]);

                            //    await _emailSender.SendEmailConfirmationAsync(model.Email, builder.ToString());

                            //    await _signInManager.SignInAsync(user, isPersistent: false);
                            //}

                            scope.Complete();

                            return StatusCode(201);
                        }
                        else
                        {
                            scope.Dispose();
                            return BadRequest("Something Wrong!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("updateUser")]
    public async Task<IActionResult> UpdateUser(ProfileViewModel data)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    //bool isEmailDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.Email == data.Email && x.Id != data.Id).Any();
                    bool isUserNameDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.UserName == data.UserName && x.Id != data.Id).Any();
                    //if (isEmailDuplicate)
                    //    return BadRequest("duplicate_email");
                    if (isUserNameDuplicate)
                        return BadRequest("duplicate_userName");
                    else
                    {
                        AspNetUsers anu = _MiturDBContext.AspNetUsers.Where(x => x.Id == data.Id).FirstOrDefault();
                        //anu.Email = data.Email;
                        //anu.NormalizedEmail = data.Email.ToUpper();
                        anu.UserName = data.UserName;
                        anu.NormalizedUserName = data.UserName.ToUpper();
                        //anu.PhoneNumber = data.PhoneNumber;
                        _MiturDBContext.Entry(anu).State = EntityState.Modified;
                        _MiturDBContext.SaveChanges();

                        _MiturDBContext.AspNetUserRoles.RemoveRange(_MiturDBContext.AspNetUserRoles.Where(x => x.UserId == data.Id));
                        _MiturDBContext.SaveChanges();
                        AspNetUserRoles anur = new AspNetUserRoles();
                        anur.UserId = data.Id;
                        anur.RoleId = data.RoleId;
                        _MiturDBContext.Add(anur);
                        _MiturDBContext.SaveChanges();
                        AspNetUsersProfile anupro = _MiturDBContext.AspNetUsersProfile.Where(x => x.Id == data.Id).FirstOrDefault();
                        if (anupro != null)
                        {
                            anupro.Id = data.Id;
                            anupro.VFirstName = data.VFirstName;
                            anupro.VLastName = data.VLastName;
                            //anupro.VCountry = data.Country;
                            //anupro.VGender = data.VGender;
                            //anupro.VPhoto = "../../assets/images/profile_Image/" + data.Id + ".png";
                            _MiturDBContext.Entry(anupro).State = EntityState.Modified;
                        }
                        else
                        {
                            AspNetUsersProfile anup = new AspNetUsersProfile();
                            anup.Id = data.Id;
                            anup.VFirstName = data.VFirstName;
                            anup.VLastName = data.VLastName;
                            //anup.VCountry = data.Country;
                           // anup.VGender = data.VGender;
                            //anup.VPhoto = "../../assets/images/profile_Image/" + data.Id + ".png";
                            _MiturDBContext.AspNetUsersProfile.Add(anup);
                        }

                        _MiturDBContext.SaveChanges();
                        //bool isImageSaved = UploadImage(data.Photo, data.Id);
                        //if (isImageSaved)
                        //{
                        //    AspNetUsersProfile anupro = _MiturDBContext.AspNetUsersProfile.Where(x => x.Id == data.Id).FirstOrDefault();
                        //    if (anupro != null)
                        //    {
                        //        anupro.Id = data.Id;
                        //        anupro.VFirstName = data.FirstName;
                        //        anupro.VLastName = data.LastName;
                        //        anupro.VCountry = data.Country;
                        //        anupro.VGender = data.Gender;
                        //        anupro.VPhoto = "../../assets/images/profile_Image/" + data.Id + ".png";
                        //        _MiturDBContext.Entry(anupro).State = EntityState.Modified;
                        //    }
                        //    else
                        //    {
                        //        AspNetUsersProfile anup = new AspNetUsersProfile();
                        //        anup.Id = data.Id;
                        //        anup.VFirstName = data.FirstName;
                        //        anup.VLastName = data.LastName;
                        //        anup.VCountry = data.Country;
                        //        anup.VGender = data.Gender;
                        //        anup.VPhoto = "../../assets/images/profile_Image/" + data.Id + ".png";
                        //        _MiturDBContext.AspNetUsersProfile.Add(anup);
                        //    }

                        //    _MiturDBContext.SaveChanges();
                        //}
                        //else
                        //{
                        //    var isDelete = DeleteImage(data.Id);
                        //    dbContextTransaction.Rollback();
                        //    return BadRequest("Something Wrong!");
                        //}
                    }

                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    //var isDelete = DeleteImage(data.Id);
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        //var data = new { Data = users, Message = "Ok", Succes = true };
                        return Ok(new {Data="", Message=ex.InnerException.Message, Succes = false});
                        //return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        //return BadRequest(ex.Message);
                        return Ok(new { Data = "", Message = ex.Message, Succes = false });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                //return BadRequest(ex.InnerException.Message);
                return Ok(new { Data = "", Message = ex.InnerException.Message, Succes = false });
            }
            else
            {
                //return BadRequest(ex.Message);
                return Ok(new { Data = "", Message = ex.Message, Succes = false });
            }
        }
    }

    [HttpGet("deleteUser")]
    public async Task<IActionResult> DeleteUser(string Id)
    {
        try
        {
            using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
            {
                try
                {
                    var roleId = _MiturDBContext.AspNetUserRoles.Where(x => x.UserId == Id).FirstOrDefault().RoleId;
                    if (roleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03")
                        return BadRequest("SuperAdmin");
                    else
                    {
                        _MiturDBContext.AspNetUsersLoginHistory.RemoveRange(_MiturDBContext.AspNetUsersLoginHistory.Where(x => x.Id == Id));
                        _MiturDBContext.SaveChanges();

                        _MiturDBContext.AspNetUsersPageVisited.RemoveRange(_MiturDBContext.AspNetUsersPageVisited.Where(x => x.Id == Id));
                        _MiturDBContext.SaveChanges();

                        _MiturDBContext.AspNetUserRoles.RemoveRange(_MiturDBContext.AspNetUserRoles.Where(x => x.UserId == Id));
                        _MiturDBContext.SaveChanges();

                        _MiturDBContext.AspNetUsersProfile.RemoveRange(_MiturDBContext.AspNetUsersProfile.Where(x => x.Id == Id));
                        _MiturDBContext.SaveChanges();

                        _MiturDBContext.AspNetUsers.RemoveRange(_MiturDBContext.AspNetUsers.Where(x => x.Id == Id));
                        _MiturDBContext.SaveChanges();

                        //bool isImageDeleted = DeleteImage(Id);
                        //if (!isImageDeleted)
                        //{
                        //    dbContextTransaction.Rollback();
                        //    return BadRequest("Image");
                        //}

                        dbContextTransaction.Commit();
                        return Ok();
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        return BadRequest(ex.InnerException.Message);
                    }
                    else
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    private bool UploadImage(string photo, string name)
    {
        try
        {
            var webRoot = System.IO.Directory.GetCurrentDirectory();
            var iPath = _config.GetSection("AppSettings")["ImagePath"] + "/assets/images/profile_Image/";
            var sPath = System.IO.Path.Combine(webRoot, iPath);
            string imageName = name + ".png";
            string imgPath = System.IO.Path.Combine(sPath, imageName);
            if (!System.IO.Directory.Exists(sPath))
            {
                System.IO.Directory.CreateDirectory(sPath);
            }
            var imgStr = photo.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imgStr);
            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool DeleteImage(string name)
    {
        try
        {
            var webRoot = System.IO.Directory.GetCurrentDirectory();
            var iPath = _config.GetSection("AppSettings")["ImagePath"] + "/assets/images/profile_Image/";
            var sPath = System.IO.Path.Combine(webRoot, iPath);
            System.IO.File.Delete(sPath + name + ".png");
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpGet("getProfileData")]
    public async Task<IActionResult> GetProfileData()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loginList = await (from anu in _MiturDBContext.AspNetUsers
                                   join anulh in _MiturDBContext.AspNetUsersLoginHistory on anu.Id equals anulh.Id
                                   join anup in _MiturDBContext.AspNetUsersProfile on anu.Id equals anup.Id into joined
                                   from anup in joined.DefaultIfEmpty()
                                   where anu.Id == userId
                                   select new
                                   {
                                       Login = anulh.DLogIn,
                                       Logout = anulh.DLogOut,
                                       IP = anulh.NvIpaddress
                                   }).OrderByDescending(x => x.Login).ToListAsync();

            var visitedList = await (from anu in _MiturDBContext.AspNetUsers
                                     join anupv in _MiturDBContext.AspNetUsersPageVisited on anu.Id equals anupv.Id
                                     join anup in _MiturDBContext.AspNetUsersProfile on anu.Id equals anup.Id into joined
                                     from anup in joined.DefaultIfEmpty()
                                     where anu.Id == userId
                                     select new
                                     {
                                         PageName = anupv.NvPageName,
                                         VisitedDate = anupv.DDateVisited,
                                         IP = anupv.NvIpaddress,
                                     }).OrderByDescending(x => x.VisitedDate).ToListAsync();

            var lastLogin = loginList[0].Login;
            var user = await (from usr in _MiturDBContext.AspNetUsers
                              where usr.Id == userId
                              select new
                              {
                                  usr.Id,
                                  usr.Email,
                                  usr.UserName,
                                  usr.TwoFactorEnabled,
                                  RoleId = usr.AspNetUserRoles.FirstOrDefault().Role.Id,
                                  RoleName = usr.AspNetUserRoles.FirstOrDefault().Role.Name,
                                  FirstName = usr.AspNetUsersProfile.VFirstName,
                                  LastName = usr.AspNetUsersProfile.VLastName,
                                  FullName = usr.AspNetUsersProfile.VFirstName + " " + usr.AspNetUsersProfile.VLastName,
                                  Country = usr.AspNetUsersProfile.VCountry,
                                 // Gender = usr.AspNetUsersProfile.VGender,
                                  usr.PhoneNumber,
                                  Photo = usr.AspNetUsersProfile.VPhoto,
                                  LastLogin = lastLogin,
                                  TotalLogin = loginList.Count,
                                  TotalPageVisited = visitedList.Count
                              }).ToListAsync();

            var Id = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03").FirstOrDefault().UserId;
            Id = userId == Id ? null : Id;
            var roles = await (from role in _MiturDBContext.AspNetRoles
                               where role.Id != (Id == null ? null : "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03")
                               select new
                               {
                                   role.Id,
                                   role.Name
                               }).OrderBy(x => x.Name).ToListAsync();

            var isTwoFactorEnabled = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "AC80C53B-9D5C-4D03-8FB8-1793ABB3DB60").FirstOrDefault().VSettingOption);
            var DisplaySetPassword = !await _userManager.HasPasswordAsync(await _userManager.FindByIdAsync(userId));
            var data = new { USER = user, ROLES = roles, LOGINLIST = loginList, VISITEDLIST = visitedList, ISTWOFACTORENABLED = isTwoFactorEnabled, SETPASS = DisplaySetPassword };
            return Ok(data);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("updateprofile")]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel data)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isUserSuperAdmin = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03" && x.UserId == Id).Any();
            bool isAllowed = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "03665F19-463B-4168-94AE-A27D9857605A").FirstOrDefault().VSettingOption);
            if (isUserSuperAdmin || isAllowed)
            {
                using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
                {
                    try
                    {
                        //bool isEmailDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.Email == data.Email && x.Id != Id).Any();
                        bool isUserNameDuplicate = _MiturDBContext.AspNetUsers.Where(x => x.UserName == data.UserName && x.Id != data.Id).Any();
                        //if (isEmailDuplicate)
                        //    return BadRequest("duplicate_email");
                        if (isUserNameDuplicate)
                            return BadRequest("duplicate_userName");
                        else
                        {
                            AspNetUsers anu = _MiturDBContext.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
                            //anu.Email = data.Email;
                            //anu.NormalizedEmail = data.Email.ToUpper();
                            anu.UserName = data.UserName;
                            anu.NormalizedUserName = data.UserName.ToUpper();
                            //anu.PhoneNumber = data.PhoneNumber;
                            _MiturDBContext.Entry(anu).State = EntityState.Modified;
                            _MiturDBContext.SaveChanges();

                            _MiturDBContext.AspNetUserRoles.RemoveRange(_MiturDBContext.AspNetUserRoles.Where(x => x.UserId == Id));
                            _MiturDBContext.SaveChanges();
                            AspNetUserRoles anur = new AspNetUserRoles();
                            anur.UserId = Id;
                            anur.RoleId = data.RoleId;
                            _MiturDBContext.Add(anur);
                            _MiturDBContext.SaveChanges();

                            AspNetUsersProfile anupro = _MiturDBContext.AspNetUsersProfile.Where(x => x.Id == Id).FirstOrDefault();
                            if (anupro == null)
                            {
                                AspNetUsersProfile anup = new AspNetUsersProfile();
                                anup.Id = Id;
                                anup.VFirstName = data.VFirstName;
                                anup.VLastName = data.VLastName;
                                //anup.VCountry = data.Country;
                                //anup.VGender = data.VGender;
                                //anup.VPhoto = "../../assets/images/profile_Image/" + Id + ".png";
                                _MiturDBContext.AspNetUsersProfile.Add(anup);
                            }
                            else
                            {
                                anupro.Id = Id;
                                anupro.VFirstName = data.VFirstName;
                                anupro.VLastName = data.VLastName;
                                //anupro.VCountry = data.Country;
                                //anupro.VGender = data.VGender;
                                //anupro.VPhoto = "../../assets/images/profile_Image/" + Id + ".png";
                                _MiturDBContext.Entry(anupro).State = EntityState.Modified;
                            }
                            _MiturDBContext.SaveChanges();
                            //bool isImageSaved = UploadImage(data.Photo, Id);
                            //if (isImageSaved)
                            //{
                            //    AspNetUsersProfile anupro = _MiturDBContext.AspNetUsersProfile.Where(x => x.Id == Id).FirstOrDefault();
                            //    if (anupro == null)
                            //    {
                            //        AspNetUsersProfile anup = new AspNetUsersProfile();
                            //        anup.Id = Id;
                            //        anup.VFirstName = data.FirstName;
                            //        anup.VLastName = data.LastName;
                            //        anup.VCountry = data.Country;
                            //        anup.VGender = data.Gender;
                            //        anup.VPhoto = "../../assets/images/profile_Image/" + Id + ".png";
                            //        _MiturDBContext.AspNetUsersProfile.Add(anup);
                            //    }
                            //    else
                            //    {
                            //        anupro.Id = Id;
                            //        anupro.VFirstName = data.FirstName;
                            //        anupro.VLastName = data.LastName;
                            //        anupro.VCountry = data.Country;
                            //        anupro.VGender = data.Gender;
                            //        anupro.VPhoto = "../../assets/images/profile_Image/" + Id + ".png";
                            //        _MiturDBContext.Entry(anupro).State = EntityState.Modified;
                            //    }
                            //    _MiturDBContext.SaveChanges();
                            //}
                            //else
                            //{
                            //    var isDelete = DeleteImage(Id);
                            //    dbContextTransaction.Rollback();
                            //    return BadRequest("Something Wrong!");
                            //}
                        }

                        dbContextTransaction.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        var isDelete = DeleteImage(Id);
                        dbContextTransaction.Rollback();
                        if (ex.InnerException != null)
                        {
                            return BadRequest(ex.InnerException.Message);
                        }
                        else
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }
            else
                return BadRequest("Change Profile Not Allowed");
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(Id);
            bool isUserSuperAdmin = _MiturDBContext.AspNetUserRoles.Where(x => x.RoleId == "4594BBC7-831E-4BFE-B6C4-91DFA42DBB03" && x.UserId == Id).Any();
            bool isAllowed = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "A55D224B-8C28-4A27-A767-C15C089F26A8").FirstOrDefault().VSettingOption);
            if (isUserSuperAdmin || isAllowed)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors.ToString());
                }
            }
            else
            {
                return BadRequest("Password Change Is Not Allowed.");
            }

        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpGet("disable2fa")]
    public async Task<IActionResult> Disable2fa()
    {
        var dbContextTransaction = _MiturDBContext.Database.BeginTransaction();
        try
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AspNetUsers anu = _MiturDBContext.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            anu.TwoFactorEnabled = false;
            _MiturDBContext.Entry(anu).State = EntityState.Modified;
            _MiturDBContext.SaveChanges();
            dbContextTransaction.Commit();
            return Ok();
        }
        catch (Exception ex)
        {
            dbContextTransaction.Rollback();
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }


        /*
         * using (var dbContextTransaction = _MiturDBContext.Database.BeginTransaction())
        {
            try
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                AspNetUsers anu = _MiturDBContext.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
                anu.TwoFactorEnabled = false;
                _MiturDBContext.Entry(anu).State = EntityState.Modified;
                _MiturDBContext.SaveChanges();
                dbContextTransaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        */
    }


    [HttpGet("externalLogin")]
    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action("Callback", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    [HttpGet("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> Callback(string returnUrl = null, string remoteError = null)
    {
        var redirectUrl = _config.GetSection("AppSettings")["AngularPath"];

        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            return Redirect(redirectUrl + "/login?error=Something wrong");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Redirect(redirectUrl + "/login?error=Something wrong");
        }

        var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(userEmail))
        {
            return Redirect(redirectUrl + "/login?error=Something wrong");
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
            isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            var loginResult = await LoginSuccess(user);
            var token = loginResult.GetType().GetProperty("token").GetValue(loginResult, null);
            var index = loginResult.GetType().GetProperty("index").GetValue(loginResult, null);
            var ulhid = loginResult.GetType().GetProperty("ulhid").GetValue(loginResult, null);
            return Redirect(redirectUrl + "/login?token=" + token + "&index=" + index + "&ulhid=" + ulhid);
        }

        if (user != null)
        {
            if (!user.EmailConfirmed)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Host.Value);  //Only use when Core & Angular are in same domain
                var host = _config.GetSection("AppSettings")["AngularPath"];
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, host);

                var webRoot = System.IO.Directory.GetCurrentDirectory();
                var pathToFile = System.IO.Path.Combine(webRoot, "EmailTemplate/email-confirmation.html");
                var builder = new StringBuilder();
                using (var reader = System.IO.File.OpenText(pathToFile))
                {
                    builder.Append(reader.ReadToEnd());
                }

                builder.Replace("{logo}", _config.GetSection("AppSettings")["Logo"]);
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                builder.Replace("{username}", myTI.ToTitleCase(user.UserName));
                builder.Replace("{appname}", _config.GetSection("AppSettings")["AppName"]);
                builder.Replace("{verifylink}", callbackUrl);
                builder.Replace("{twitterlink}", _config.GetSection("Company")["SocialTwitter"]);
                builder.Replace("{facebooklink}", _config.GetSection("Company")["SocialFacebook"]);
                builder.Replace("{companyname}", _config.GetSection("Company")["Company"]);
                builder.Replace("{companyemail}", _config.GetSection("Company")["MailAddress"]);

                await _emailSender.SendEmailConfirmationAsync(user.Email, builder.ToString());

                return Redirect(redirectUrl + "/login?error=Email not confirmed");
            }

            // Add the external provider
            await _userManager.AddLoginAsync(user, info);

            result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                            isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var loginResult = await LoginSuccess(user);
                var token = loginResult.GetType().GetProperty("token").GetValue(loginResult, null);
                var index = loginResult.GetType().GetProperty("index").GetValue(loginResult, null);
                var ulhid = loginResult.GetType().GetProperty("ulhid").GetValue(loginResult, null);
                return Redirect(redirectUrl + "/login?token=" + token + "&index=" + index + "&ulhid=" + ulhid);
            }
        }
        return Redirect(redirectUrl + "/register?associate=" + userEmail + "&loginProvider=" + info.LoginProvider + "&providerDisplayName=" + info.ProviderDisplayName + "&providerKey=" + info.ProviderKey);
    }

    [HttpGet("providers")]
    [AllowAnonymous]
    public async Task<IActionResult> Providers()
    {
        bool isAllowed = Convert.ToBoolean(_MiturDBContext.Setting.Where(x => x.VSettingId == "FAC27C8C-ED92-453E-84A9-659C5D8FD651").FirstOrDefault().VSettingOption);
        if (isAllowed)
        {
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return Ok(schemes.Select(s => s.DisplayName).ToList());
        }
        return Ok();
    }

    [HttpPost]
    [Route("associate")]
    [AllowAnonymous]
    public async Task<IActionResult> Associate([FromBody] AssociateViewModel associate)
    {
        var user = new ApplicationUser { UserName = associate.UserName, Email = associate.Email, Date = DateTime.UtcNow };

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var roleId = "";
                if (_MiturDBContext.AspNetUsers.Where(x => x.UserName == associate.UserName).Any())
                {
                    return BadRequest("User with the same username already taken.");
                }
                else
                {
                    roleId = _MiturDBContext.Setting.Where(x => x.VSettingId == "9B55EDC6-2B9C-4869-B5D7-8B2A788DAA12").FirstOrDefault().VSettingOption;
                    if (!_MiturDBContext.AspNetRoles.Where(x => x.Id == roleId).Any())
                        return BadRequest("User registration role not set at settings. Please go to settings page and set the user registration role.");
                }

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    AspNetUserRoles anur = new AspNetUserRoles
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };
                    _MiturDBContext.AspNetUserRoles.Add(anur);
                    _MiturDBContext.SaveChanges();

                    result =
                        await _userManager.AddLoginAsync(user,
                            new ExternalLoginInfo(null, associate.LoginProvider, associate.ProviderKey,
                                associate.ProviderDisplayName));

                    if (result.Succeeded)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);

                        await _signInManager.ExternalLoginSignInAsync(associate.LoginProvider, associate.ProviderKey, false);

                        scope.Complete();
                    }
                    else
                    {
                        scope.Dispose();
                        return BadRequest(result.Errors);
                    }
                }
                else
                {
                    scope.Dispose();
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                scope.Dispose();
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        var loginResult = await LoginSuccess(user);
        return Ok(loginResult);
    }

    [HttpPost("setpassword")]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid");
            }
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                return BadRequest(ex.InnerException.Message);
            }
            else
            {
                return BadRequest(ex.Message);
            }
        }
    }
}