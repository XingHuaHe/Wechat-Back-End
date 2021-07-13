using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Models.MobileModels;
using PersonalSystemProcedures.Models.MobileModels.DbModel;
using PersonalSystemProcedures.Services;
using PersonalSystemProcedures.Services.MobileServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalSystemProcedures.Controllers.MobileSystemApi
{
    [Route("Mobile/api/[controller]")]
    public class SystemApiController : Controller
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        private readonly TuShanContext _tuShanContext;
        public SystemApiController(TuShanContext tuShanContext)
        {
            _tuShanContext = tuShanContext;
        }

        /// <summary>
        /// 上传失物图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("buildLostPic")]
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
                string filePath = "wwwroot\\LostPicure" + "\\";
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
                    fileName = userId + Guid.NewGuid() + "." + suffix;
                    string fileFullName = filePath + fileName;
                    using (FileStream fs = System.IO.File.Create(fileFullName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    filePathResultList = "LostPicure" + "\\" + fileName;
                }
                catch
                {
                    return BadRequest();
                }
            }
            return Ok(filePathResultList);
        }

        /// <summary>
        /// 发布事物信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sendLostMsge")]
        public IActionResult LostInformation([FromBody] ToLostInfo item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                if (item.PhoneNumber.Length != 11)
                {
                    return BadRequest();
                }
                var infor = new LostInfo
                {
                    LostInfoID = item.UserID + System.DateTime.Now.ToString().ToLower(),
                    UserID = item.UserID,
                    UserName = item.UserName,
                    LostName = item.LostName,
                    PhoneNumber = item.PhoneNumber,
                    LostInfos = item.LostInfos,
                    LostInfoPic = item.LostInfoPic,
                    LostTime = System.DateTime.Now,
                    States = false //false为未寻回
                };
                _tuShanContext.LostInfors.Add(infor);
                int row = _tuShanContext.SaveChanges();
                if (row > 0)
                {
                    //根目录下创建失物图片  Redirect();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 删除个人失物信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delPesonLostIofo")]
        public IActionResult GetPesonLostInfo([FromBody] ToDealLostInformation item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                if (item.UserID == null)
                {
                    return BadRequest();
                }
                var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                _tuShanContext.Remove(db);
                int row = _tuShanContext.SaveChanges();
                if (row > 0)
                {
                    return Ok(TokenCode.SuccessedDel.ToString());
                }
                else
                {
                    //删除根目录下的照片  Redirect()
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 标记失物寻回
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("maklostInfoState")]
        public IActionResult MakLostInfoState([FromBody] ToStateLostInformation item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                if (item.LostInfoID == null)
                {
                    return BadRequest(ErrorCode.ExceptionError.ToString());
                }
                var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                if (db == null)
                {
                    return BadRequest(TokenCode.StateFalse.ToString());
                }
                var ling = (from obj in _tuShanContext.LostInfors
                            where obj.LostInfoID == item.LostInfoID
                            select obj).SingleOrDefault();
                ling.States = true;
                bool flag = ling.States;
                int row = _tuShanContext.SaveChanges();
                if (flag)
                {
                    return Ok(TokenCode.StateTrue.ToString());
                }
                else
                {
                    return BadRequest(TokenCode.StateFalse.ToString());
                }
            }
            catch
            {
                return BadRequest(ErrorCode.ExceptionError.ToString());
            }
        }

        /// <summary>
        /// 获取个人单个所有失物信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getLostInfo")]
        public IActionResult GetLostInfoList([FromBody] ToGetLostInfo item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                if (db == null)
                {
                    return Ok();
                }
                var linq = (from obj in _tuShanContext.LostInfors
                            where obj.LostInfoID == item.LostInfoID
                            select obj).SingleOrDefault();
                var _toDoList = new List<LostInfo>();
                _toDoList.Add(linq);
                if (_toDoList != null)
                {
                    return Ok(_toDoList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return Ok(TokenCode.UnSuccessedSend);
            }
        }

        /// <summary>
        /// 获取个人所有失物信息
        /// </summary>
        /// <param name="itrm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getAllUserLostInfo")]
        public IActionResult GetLostInfosList([FromBody] ToGetAllInfos item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                //var db = _tuShanContext.LostInfors.
                //if (db == null)
                //{
                //    return Ok();
                //}
                //提取前5条信息
                var linq = (from obj in _tuShanContext.LostInfors
                            where obj.UserID == item.UserID
                            select obj).Take(5);
                var _toDoList = new List<LostInfo>();
                foreach (var i in linq)
                {
                    _toDoList.Add(i);
                }
                if (_toDoList != null)
                {
                    return Ok(_toDoList);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 获取最新失物信息(存在Bug)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getNewLostInfo")]
        public IActionResult GetNewLostInfos()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                var linq = (from obj in _tuShanContext.LostInfors
                            where obj.LostTime.Day == System.DateTime.Now.Day
                            select obj).SingleOrDefault();
                var _toDoList = new List<ToLostInfo>();
                var lostinfo = new ToLostInfo
                {
                    UserID = linq.UserID,
                    LostName = linq.LostName,
                    LostInfos = linq.LostInfos,
                    PhoneNumber = linq.PhoneNumber,
                    LostInfoPic = linq.LostInfoPic
                };
                _toDoList.Add(lostinfo);
                int row = _tuShanContext.SaveChanges();
                if (row == 0)
                {
                    return Ok(_toDoList);
                }
                else
                {
                    return Ok(TokenCode.UnSuccessedDel.ToString());
                }
            }
            catch
            {
                return Ok(ErrorCode.ExceptionError.ToString());
            }
        }
    }
}
