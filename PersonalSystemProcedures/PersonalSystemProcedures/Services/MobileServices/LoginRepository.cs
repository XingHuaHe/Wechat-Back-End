using Microsoft.AspNetCore.Mvc;
using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Interfaces;
using PersonalSystemProcedures.Models.MobileModels;
using PersonalSystemProcedures.Models.MobileModels.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Services.MobileServices
{
    public class LoginRepository : ILoginRepository
    {
        //[Produces("application/json")]
        //[Route("api/[controller]")]
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
            public List<UserInfo> Login(UserLogin item)
            {
                try
                {
                    if (item == null || !ModelState.IsValid)
                    {
                        return null;
                    }
                    var db = _tuShanContext.Users.Find(item.UserID);
                    if (db == null)
                    {
                        return null;
                    }
                    else
                    {
                        if (string.Compare(db.UserPasswd, item.UserPassword) == 0)
                        {
                            var linq = (from obj in _tuShanContext.Users
                                        where obj.UserID == item.UserID
                                        select obj).SingleOrDefault();

                            var _toDoList = new List<UserInfo>();
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
                            _toDoList.Add(user);
                            linq.UserOnline = true;
                            _tuShanContext.SaveChanges();
                            return _toDoList;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            /// <summary>
            /// 新建用户
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool BuildUser(UserBuild item)
            {
                try
                {
                    if (item.UserID != null && item.UserPasswd != null && item.UserName != null)
                    {
                        var db = _tuShanContext.Users.Find(item.UserID);
                        if (db != null)
                        {
                            return false;//用户名已存在
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
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 更新用户信息
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool UpdateInfo(UserInfo item)
            {
                try
                {
                    var db = _tuShanContext.Users.Find(item.UserID);
                    if (db == null)
                    {
                        return false;
                    }
                    var linq = (from obj in _tuShanContext.Users
                                where obj.UserID == item.UserID
                                select obj).SingleOrDefault();
                    if (object.Equals(linq, null))
                    {
                        return false;
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// 建立个人头像
            /// </summary>
            public void BuidUserHP()
            {

            }

            /// <summary>
            /// 修改用户头像
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool UpdateUserHP(UserHPortrait item)
            {
                try
                {
                    var db = _tuShanContext.Users.Find(item.UserID);
                    if (db == null)
                    {
                        return false;
                    }
                    var linq = (from obj in _tuShanContext.Users
                                where obj.UserID == item.UserID
                                select obj).SingleOrDefault();
                    if (object.Equals(linq, null))
                    {
                        return false;
                    }
                    linq.UserHPortraitURL = item.UserHPortraitURL;
                    int row = _tuShanContext.SaveChanges();
                    if (row > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
