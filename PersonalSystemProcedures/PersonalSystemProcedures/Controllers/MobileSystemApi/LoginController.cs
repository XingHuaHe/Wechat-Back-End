using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Models.MobileModels;
using PersonalSystemProcedures.Models.MobileModels.DbModel;
using PersonalSystemProcedures.Services;
using PersonalSystemProcedures.Services.MobileServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalSystemProcedures.Controllers.MobileSystemApi
{
    //[Produces("application/json")]
    [Route("Mobile/api/[controller]")]
    public class LoginController : Controller
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        private readonly TuShanContext _tuShanContext;
        public LoginController(TuShanContext tuShanContext)
        {
            _tuShanContext = tuShanContext;
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userLogin")]
        public IActionResult Login([FromBody] UserLogin item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                var db = _tuShanContext.Users.Find(item.UserID.ToString());
                if (db == null)
                {
                    return BadRequest(ErrorCode.RecordNotFound.ToString());
                }
                else
                {
                    if (string.Compare(db.UserPasswd, item.UserPassword) == 0)
                    {
                        var linq = (from obj in _tuShanContext.Users
                                    where obj.UserID == item.UserID
                                    select obj).SingleOrDefault();

                        //var _toDoList = new List<UserInfo>();
                        var user = new UserInfo
                        {
                            UserID = linq.UserID,
                            UserName = linq.UserName,
                            UserSex = linq.UserSex,
                            UserHPortraitURL = linq.UserHPortraitURL,
                            UserCollege = linq.UserCollege,
                            UserEmail = linq.UserEmail,
                            UserIndiSignature = linq.UserIndiSignature,
                            UserPhoneNum = linq.UserPhoneNum,
                        };
                        //_toDoList.Add(user);
                        linq.UserOnline = true;
                        _tuShanContext.SaveChanges();
                        return Ok(JsonConvert.SerializeObject(user));
                        //return Ok(user);
                    }
                    else
                    {
                        return BadRequest(ErrorCode.CouldNotLoginItem.ToString());
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.ExceptionError.ToString());
            }
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userBuild")]
        public IActionResult BuildUser([FromBody] UserBuild item)
        {
            try
            {
                if (item.UserID != null && item.UserPasswd != null && item.UserName != null)
                {
                    //密码不能短于5位
                    if (item.UserPasswd.Length <= 5)
                    {
                        return Ok(TokenCode.UnSuccessedBuildUser.ToString());
                    }
                    var db = _tuShanContext.Users.Find(item.UserID);
                    if (db != null)
                    {
                        return Ok(TokenCode.UnSuccessedBuildUser.ToString());//用户名已存在
                    }
                    var user_info = new User
                    {
                        UserID = item.UserID,
                        UserPasswd = item.UserPasswd,
                        UserName = item.UserName,
                        UserPhoneNum = item.UserPhoneNum,
                        UserOnline = false,
                        UserCollege = item.UserCollege,
                        UserSex = item.UserSex,
                        UserEmail = item.UserEmail,
                        UserIndiSignature = "",
                        UserHPortraitURL = ""
                    };
                    _tuShanContext.Users.Add(user_info);
                    int row_pic = _tuShanContext.SaveChanges();
                    if (row_pic > 0)
                    {
                        return Ok(TokenCode.SuccessedBuildUser.ToString());
                    }
                    return Ok(TokenCode.UnSuccessedBuildUser.ToString());
                }
                else
                {
                    return BadRequest(ErrorCode.CouldNotBuildUser.ToString());
                }
            }
            catch
            {
                return Ok(ErrorCode.ExceptionError.ToString());
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateInfo")]
        public IActionResult UpdateInfo([FromBody] UserInfo item)
        {
            try
            {
                var db = _tuShanContext.Users.Find(item.UserID);
                if (db == null)
                {
                    return Ok(TokenCode.UnSuccessedModify.ToString());
                }
                var linq = (from obj in _tuShanContext.Users
                            where obj.UserID == item.UserID
                            select obj).SingleOrDefault();
                if (object.Equals(linq, null))
                {
                    return Ok(TokenCode.UnSuccessedModify.ToString());
                }
                linq.UserName = item.UserName;
                linq.UserSex = item.UserSex;
                linq.UserPhoneNum = item.UserPhoneNum;
                linq.UserCollege = item.UserCollege;
                linq.UserEmail = item.UserEmail;
                linq.UserIndiSignature = item.UserIndiSignature;
                int row = _tuShanContext.SaveChanges();
                if (row > 0)
                {
                    return Ok();
                }
                else
                {
                    return Ok(TokenCode.UnSuccessedModify.ToString());
                }
            }
            catch
            {
                return Ok(ErrorCode.ExceptionError.ToString());
            }
        }

        /// <summary>
        /// 上传个人头像 图片格式为：UserID-图片名.jpg
        /// </summary>
        [HttpPost]
        [Route("buildUserHP")]
        public IActionResult UploadUserHPAsync()
        {
            string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            if (size > 104857600) //检测图片是否大于100M
            {
                return BadRequest();
            }
            //List<string> filePathResultList = new List<string>(); 
            string filePathResultList = string.Empty;//记录图片存储地址
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //客户端传过来的数据格式：userId-pictrueName.jpg 例如：1721019594-test.jpg
                string suffix = fileName.Split('.')[1];//后缀:jpg
                string userId = fileName.Split('-')[0];//用户名: 1721019594
                string filePath = "wwwroot\\UserHPic\\" + userId + "\\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!pictureFormatArray.Contains(suffix))//检测是否为png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG"等后缀
                {
                    return BadRequest();
                }
                try
                {
                    fileName = Guid.NewGuid() + "." + suffix;
                    string fileFullName = filePath + fileName;
                    using (FileStream fs = System.IO.File.Create(fileFullName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    filePathResultList = "UserHPic\\" + userId + "\\" + fileName;
                }
                catch
                {
                    return BadRequest();
                }

            }
            return Ok(filePathResultList);
        }

        /// <summary>
        /// 修改数据库用户头像URL
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateUserPic")]
        public IActionResult UpdateUserHP([FromBody] UserHPortrait item)
        {
            try
            {
                var db = _tuShanContext.Users.Find(item.UserID);
                if (db == null)
                {
                    return Ok(TokenCode.UnSuccessedModify.ToString());
                }
                var linq = (from obj in _tuShanContext.Users
                            where obj.UserID == item.UserID
                            select obj).SingleOrDefault();
                if (object.Equals(linq, null))
                {
                    return BadRequest();
                }
                linq.UserHPortraitURL = item.UserHPortraitURL;
                int row = _tuShanContext.SaveChanges();
                if (row > 0)
                {
                    return Ok(TokenCode.SuccessedModify.ToString());
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Photos")]
        public async Task<IActionResult> UploadPhotosAsync(IFormFileCollection files)
        {
            long size = files.Sum(f => f.Length);
            var fileFolder = "G:/UserHPic/296588989";

            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +
                                   Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(fileFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }
    }
}
