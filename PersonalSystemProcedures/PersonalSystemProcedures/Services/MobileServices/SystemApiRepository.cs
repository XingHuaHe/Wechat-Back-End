using Microsoft.AspNetCore.Mvc;
using PersonalSystemProcedures.DBContext;
using PersonalSystemProcedures.Models.MobileModels;
using PersonalSystemProcedures.Models.MobileModels.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.Services.MobileServices
{
    public class SystemApiRepository
    {
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
            /// 发布事物信息
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool LostInformation(ToLostInfo item)
            {
                try
                {
                    if (item == null)
                    {
                        return false;
                    }
                    if (item.PhoneNumber.Length != 11)
                    {
                        return false;
                    }
                    var infor = new LostInfo
                    {
                        UserID = item.UserID,
                        LostInfoID = item.UserID + System.DateTime.Now.ToString().ToLower(),
                        LostName = item.LostName,
                        LostInfos = item.LostInfos,
                        LostInfoPic = item.LostInfoPic,
                        LostTime = System.DateTime.Now,
                        PhoneNumber = item.PhoneNumber,
                        States = false
                    };
                    _tuShanContext.LostInfors.Add(infor);
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
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// 删除个人失物信息
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool GetPesonLostInfo(ToDealLostInformation item)
            {
                try
                {
                    if (item == null)
                    {
                        return false;
                    }
                    if (item.UserID == null)
                    {
                        return false;
                    }
                    var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                    _tuShanContext.Remove(db);
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
            /// 标记失物寻回
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool MakLostInfoState(ToDealLostInformation item)
            {
                try
                {
                    if (item == null)
                    {
                        return false;
                    }
                    if (item.LostInfoID == null)
                    {
                        return false;
                    }
                    var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                    if (db == null)
                    {
                        return false;
                    }
                    var ling = (from obj in _tuShanContext.LostInfors
                                where obj.LostInfoID == item.LostInfoID
                                select obj).SingleOrDefault();
                    ling.States = true;
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
                    return true;
                }
            }

            /// <summary>
            /// 获取个人单个所有失物信息
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public List<LostInfo> GetLostInfoList(ToGetLostInfo item)
            {
                try
                {
                    if (item == null)
                    {
                        return null;
                    }
                    var db = _tuShanContext.LostInfors.Find(item.LostInfoID);
                    if (db == null)
                    {
                        return null;
                    }
                    var linq = (from obj in _tuShanContext.LostInfors
                                where obj.LostInfoID == item.LostInfoID
                                select obj).SingleOrDefault();
                    var _toDoList = new List<LostInfo>();
                    _toDoList.Add(linq);
                    int row = _tuShanContext.SaveChanges();
                    if (row > 0)
                    {
                        return _toDoList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// 获取个人所有失物信息
            /// </summary>
            /// <param name="itrm"></param>
            /// <returns></returns>
            public List<LostInfo> GetLostInfosList(ToGetLostInfo itrm)
            {
                return null;
            }

            /// <summary>
            /// 获取最新失物信息(存在Bug)
            /// </summary>
            /// <returns></returns>
            public List<ToLostInfo> GetNewLostInfos()
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return null;
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
                        return _toDoList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
